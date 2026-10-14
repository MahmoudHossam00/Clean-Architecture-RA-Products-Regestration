using RAProductsRegestration.Application.Common.Interfaces.Integrations.CTD;
using RAProductsRegestration.Application.Common.Interfaces.Messaging;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.ActiveIngredients;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Application.Integrations
    .CTD.ActiveIngredients.Sync;

internal sealed class Handler(
    ICTDDataSource ctdDataSource,
    IRepository<ActiveIngredient> activeIngredientRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<SyncActiveIngredientsCommand, Success>
{
    public async Task<Result<Success>> Handle(
        SyncActiveIngredientsCommand request,
        CancellationToken cancellationToken)
    {
        var ctdActiveIngredients =
            await ctdDataSource.GetActiveIngredientsAsync(
                cancellationToken);

        if (ctdActiveIngredients.Count == 0)
        {
            return Result.Success;
        }

        // Normalize and remove duplicate CTD IDs
        var ctdActiveIngredientsById =
            ctdActiveIngredients
                .Where(x => x.CTDId > 0)
                .GroupBy(x => x.CTDId)
                .ToDictionary(
                    x => x.Key,
                    x => x.First());

        var ctdIds =
            ctdActiveIngredientsById.Keys.ToArray();

        // One local DB query
        var existingActiveIngredients =
            await activeIngredientRepository.WhereAsync(
                x => ctdIds.Contains(x.CTDId),
                cancellationToken);

        // CTDId -> ActiveIngredient
        var activeIngredientsByCTDId =
            existingActiveIngredients.ToDictionary(
                x => x.CTDId);

        var newActiveIngredients =
            new List<ActiveIngredient>();

        foreach (var (ctdId, ctdIngredient)
                 in ctdActiveIngredientsById)
        {
            if (activeIngredientsByCTDId.TryGetValue(
                    ctdId,
                    out var activeIngredient))
            {
                activeIngredient.UpdateFromCTD(
                    ctdIngredient.Name);

                continue;
            }

            var newActiveIngredient =
                new ActiveIngredient(
                    ctdIngredient.CTDId,
                    ctdIngredient.Code,
                    ctdIngredient.Name);

            newActiveIngredients.Add(
                newActiveIngredient);

            activeIngredientsByCTDId.Add(
                ctdId,
                newActiveIngredient);
        }

        if (newActiveIngredients.Count > 0)
        {
            activeIngredientRepository.AddRange(
                newActiveIngredients);
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success;
    }
}