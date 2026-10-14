using MediatR;
using Microsoft.EntityFrameworkCore;
using RAProductsRegestration.Application.Common.Interfaces.Persistence;
using RAProductsRegestration.Domain.ActiveIngredients;
using RAProductsRegestration.Domain.Appeals;
using RAProductsRegestration.Domain.AppealTypes;
using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.ValueObjects;
using RAProductsRegestration.Domain.Companies;
using RAProductsRegestration.Domain.Departments;
using RAProductsRegestration.Domain.DosageForms;
using RAProductsRegestration.Domain.LicenseCategories;
using RAProductsRegestration.Domain.MarketingTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovalFileTypes;
using RAProductsRegestration.Domain.MiscellaneousApprovals;
using RAProductsRegestration.Domain.ProductBasicDatas;
using RAProductsRegestration.Domain.ProductLicenses;
using RAProductsRegestration.Domain.Products;
using RAProductsRegestration.Domain.ProductsTechnicalDocs;
using RAProductsRegestration.Domain.ProductsTechnicalDocs.Common;
using RAProductsRegestration.Domain.PromotionalMaterials;
using RAProductsRegestration.Domain.PromotionalMaterialTypes;
using RAProductsRegestration.Domain.StorageConditions;
using RAProductsRegestration.Domain.Suppliers;
using RAProductsRegestration.Domain.Variations;
using ActiveIngredient = RAProductsRegestration.Domain.ActiveIngredients.ActiveIngredient;



namespace RAProductsRegestration.Infrastructure.Data;

internal sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options, IMediator mediator)
    : DbContext(options), IUnitOfWork
{


    public DbSet<MiscellaneousApproval>
    MiscellaneousApprovals =>
        Set<MiscellaneousApproval>();

    public DbSet<MiscellaneousApprovalFileType>
        MiscellaneousApprovalFileTypes =>
            Set<MiscellaneousApprovalFileType>();


    public DbSet<PromotionalMaterial>
    PromotionalMaterials =>
        Set<PromotionalMaterial>();


    public DbSet<PromotionalMaterialType>
        PromotionalMaterialTypes =>
            Set<PromotionalMaterialType>();


    public DbSet<Appeal> Appeals =>
    Set<Appeal>();


    public DbSet<AppealType> AppealTypes =>
        Set<AppealType>();


    public DbSet<Product> Products =>
        Set<Product>();

    public DbSet<Company> Companies =>
        Set<Company>();

    public DbSet<Variation> Variations =>
    Set<Variation>();
    public DbSet<ProductLicense> ProductLicenses =>
    Set<ProductLicense>();


    public DbSet<LicenseCategory> LicenseCategories =>
        Set<LicenseCategory>();


    public DbSet<MarketingType> MarketingTypes =>
        Set<MarketingType>();


    public DbSet<ActiveIngredient> ActiveIngredients =>
        Set<ActiveIngredient>();
    public DbSet<Supplier> Suppliers =>
    Set<Supplier>();

    public DbSet<ProductBasicData> ProductBasicData =>
        Set<ProductBasicData>();
    public DbSet<ProductTechnicalDocs> ProductTechnicalDocs =>
        Set<ProductTechnicalDocs>();
    public DbSet<Department> Departments =>
    Set<Department>();
    public DbSet<DosageForm> DosageForms =>
    Set<DosageForm>();

    public DbSet<StorageCondition> StorageConditions =>
        Set<StorageCondition>();
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
        foreach (var entityType in modelBuilder.Model
                 .GetEntityTypes()
                 .Where(entityType =>
                     typeof(Entity).IsAssignableFrom(
                         entityType.ClrType)))
        {
            modelBuilder.Entity(entityType.ClrType)
                .Ignore(nameof(Entity.DomainEvents));
        }
    }
        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
            .Select(e => (Entity)e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }
    }

    protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .ComplexProperties<AttachmentReference>();

        configurationBuilder
            .ComplexProperties<DatedAttachment>();

        configurationBuilder
            .ComplexProperties<FulfillmentDeadline>();

        configurationBuilder
            .ComplexProperties<DatedApprovalDocument>();
    }
}
