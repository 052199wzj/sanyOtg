using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace iPlant.FMS.WEB
{
    public class FPCGasVelocityController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FPCGasVelocityController));

        [HttpGet]
        public ActionResult AllGasVelocityList()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();
                
                int wID = StringUtils.parseInt(Request.QueryParamString("ID"));
                int wType = StringUtils.parseInt(Request.QueryParamString("Type"));
                double wThickness = StringUtils.parseDouble(Request.QueryParamString("Thickness"));
                string wName = StringUtils.parseString(Request.QueryParamString("Name"));
                string wDescription = StringUtils.parseString(Request.QueryParamString("Description"));

                ServiceResult<List<FPCGasVelocity>> wServerRst = ServiceInstance.mFMCService.FPC_QueryGasVelocityList(wBMSEmployee, wID, wType, wThickness, wName, wDescription);

                if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
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
        public ActionResult ImportGasVelocity(IFormFileCollection files)
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            string wMsg = "";
            try
            {
                if (files == null || files.Count <= 0)
                    files = Request.Form.Files;
                if (files.Count == 0)
                {
                    wMsg = "提示：没有要导入的Excel文件！";
                    return Json(GetResult(RetCode.SERVER_CODE_ERR, wMsg, null, null));
                }

                for (int i = 0; i < files.Count; i++)
                {
                    IFormFile wCurFile = files[i];

                    if (wCurFile == null && wCurFile.Length < 1)
                        continue;

                    //获取文件名  
                    string wFileName = Path.GetFileName(wCurFile.FileName);
                    if (wFileName == null)
                        continue;

                    ServiceResult<List<FPCGasVelocity>> wServerRst = ServiceInstance.mFMCService.FPC_ImportGasVelocity(wCurFile.OpenReadStream(), wCurFile.FileName, out wMsg);

                    if (StringUtils.isEmpty(wServerRst.getFaultCode()))
                        wResult = GetResult(RetCode.SERVER_CODE_SUC, "", wServerRst.getResult(), null);
                    else
                        wResult = GetResult(RetCode.SERVER_CODE_ERR, wServerRst.getFaultCode(), wServerRst.getResult(), null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

                wResult = GetResult(RetCode.SERVER_CODE_ERR, wMsg + "\n" + ex.ToString());
            }

            return Json(wResult);
        }
    }
}
