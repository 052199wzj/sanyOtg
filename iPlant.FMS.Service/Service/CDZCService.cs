using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public interface CDZCService
    {
        ServiceResult<Int32> CDZC_Save(BMSEmployee wLoginUser, AnnualIndicators wAnnualIndicators);
        ServiceResult<AnnualIndicators> CDZC_SearchDate();
    }
}