using System;
using System.Collections.Generic;
using System.Text;

namespace RAProductsRegestration.Application.Common.Interfaces.Files
{
    public interface IEmployeeExcelTemplateGenerator
    {
        byte[] Generate();
    }
}
