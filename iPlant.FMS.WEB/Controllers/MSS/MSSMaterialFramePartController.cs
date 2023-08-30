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
    public class MSSMaterialFramePartController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialFramePartController));

        [HttpGet]
        public ActionResult All()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();

            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                int wMaterialFrameID = StringUtils.parseInt(Request.QueryParamString("MaterialFrameID"));

                int wPageSize = StringUtils.parseInt(Request.QueryParamString("PageSize"));
                int wPageIndex = StringUtils.parseInt(Request.QueryParamString("PageIndex"));

                Pagination wPagination = Pagination.Create(wPageIndex, wPageSize);

                ServiceResult<List<MSSMaterialFramePart>> wServerRst = ServiceInstance.mMSSService.MSS_QueryMaterialFramePartList(
                    wBMSEmployee, wMaterialFrameID, wPagination);

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

        [HttpPost]
        public ActionResult Add()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();

            try
            {
                Dictionary<String, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();

                if ((wParam == null) || (!wParam.ContainsKey("data")))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                MSSMaterialFramePart wItem = CloneTool.Clone<MSSMaterialFramePart>(wParam["data"]);

                ServiceResult<int> wServerRst = ServiceInstance.mMSSService.MSS_AddMaterialFramePart(wBMSEmployee, wItem);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wItem);
                else
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), null, wItem);
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
