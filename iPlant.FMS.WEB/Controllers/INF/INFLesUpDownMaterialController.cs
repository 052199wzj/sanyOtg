using iPlant.Common.Tools;
using iPlant.FMC.Service;
using iPlant.FMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPlant.FMS.WEB
{
    public class INFLesUpDownMaterialController : BaseController
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(INFLesUpDownMaterialController));

        /// <summary>
        /// by Demin 20221117
        ///  报工信息统计
        /// </summary>
        /// <param name="wBMSEmployee"></param>
        /// </returns>
        [HttpGet]
        public ActionResult All()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();

            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                String wFrameCode = StringUtils.parseString(Request.QueryParamString("FrameCode"));
                String wNestId = StringUtils.parseString(Request.QueryParamString("NestId"));
                String wOrder = StringUtils.parseString(Request.QueryParamString("Order"));
                String wProductNo = StringUtils.parseString(Request.QueryParamString("ProductNo"));
                String wSeq = StringUtils.parseString(Request.QueryParamString("Seq"));
                String wStationCode = StringUtils.parseString(Request.QueryParamString("StationCode"));
                String wSub = StringUtils.parseString(Request.QueryParamString("Sub"));
                int wUseType = StringUtils.parseInt(Request.QueryParamString("UseType"), -1);

                SByte wStatus = StringUtils.parseSByte(Request.QueryParamString("Status"), -1);

                DateTime wStartTime = StringUtils.parseDate(Request.QueryParamString("StartTime"));
                DateTime wEndTime = StringUtils.parseDate(Request.QueryParamString("EndTime"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));
                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);
                ServiceResult<List<INFLesUpDownMaterial>> wServerRst = ServiceInstance.mINFService.INF_QueryINFLesUpDownMaterialList(
                    wBMSEmployee, wID, wFrameCode, wNestId, wOrder, wProductNo, wSeq, wStationCode,
                    wSub, wUseType, wStatus, wStartTime, wEndTime, wPagination);

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
