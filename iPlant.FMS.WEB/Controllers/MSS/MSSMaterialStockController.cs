using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iPlant.FMS.WEB
{
    public class MSSMaterialStockController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialStockController));

        [HttpGet]
        public ActionResult AllMaterialStock()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                String wCode = StringUtils.parseString(Request.QueryParamString("Code"));
                int wActive = StringUtils.parseInt(Request.QueryParamString("Active"));
                int wMaterialPointID = StringUtils.parseInt(Request.QueryParamString("MaterialPointID"));
                int wPlateID = StringUtils.parseInt(Request.QueryParamString("PlateID"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MSSMaterialStock>> wServerRst = ServiceInstance.mFMCService.MSS_QueryAllMaterialStock(wBMSEmployee, wCode, wActive, wMaterialPointID, wPlateID, wPagination);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), wPagination.TotalPage);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }
    }
}
