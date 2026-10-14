using RAProductsRegestration.Domain.Common;
using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.ProductBasicDatas
    .ApprovedPrices;

public sealed class ApprovedPrice : Entity
{
    private ApprovedPrice()
    {
    }

    private ApprovedPrice(
        decimal price,
        string pricePerDescription,
        DateOnly issueDate,
        DateOnly expiryDate)
        : base(Guid.Empty)
    {
        Price = price;
        IssueDate = issueDate;
        PricePerDescription = pricePerDescription;
        ExpiryDate = expiryDate;
    }

    public decimal Price { get; private set; }
    public string PricePerDescription { get; private set; } //10 l.e/leaflet/ 10 capsules

    public DateOnly IssueDate { get; private set; }

    public DateOnly ExpiryDate { get; private set; }

    public DateOnly ReminderDate =>
        ExpiryDate.AddMonths(-3);

    internal static Result<ApprovedPrice> Create(
        decimal price,
        string pricePerDescription,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        if (price < 0)
        {
            return ApprovedPriceErrors.InvalidPrice;
        }

        if (expiryDate < issueDate)
        {
            return ApprovedPriceErrors
                .InvalidValidityPeriod;
        }

        return new ApprovedPrice(
            price,
            pricePerDescription,
            issueDate,
            expiryDate);
    }

    internal Result<Updated> Update(
        decimal price,
        string pricePerDescription,
        DateOnly issueDate,
        DateOnly expiryDate)
    {
        if (price < 0)
        {
            return ApprovedPriceErrors.InvalidPrice;
        }

        if (expiryDate < issueDate)
        {
            return ApprovedPriceErrors
                .InvalidValidityPeriod;
        }

        Price = price;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        PricePerDescription = pricePerDescription;
        return Result.Updated;
    }
}