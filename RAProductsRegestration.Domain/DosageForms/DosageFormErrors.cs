using RAProductsRegestration.Domain.Common.Results;

namespace RAProductsRegestration.Domain.DosageForms;

public static class DosageFormErrors
{
    public static Error NameRequired =>
        Error.Validation(
            "DosageForm.Name.Required",
            "Dosage form name is required.");
}