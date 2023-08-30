using iPlant.Common.Tools;
using iPlant.FMS.Models;
using iPlant.FMC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iPlant.FMS.WEB
{  
     /// <summary>
     /// 与数采系统接口
     /// </summary>
    public class DAQInterfaceController : BaseController
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DAQInterfaceController));


        /// <summary>
        /// 中控系统接收料点料框状态信息（数采→中控）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DAQContainerStatus()
        {
            Dictionary<string, object> wResult = new Dictionary<string, object>();
            DAQResponse response = new DAQResponse();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                ServiceInstance.mINTERFACEService.WriteLog("数采系统", " 中控系统接收料点料框状态信息", " 中控系统接收料点料框状态信息", 1, "中控系统接收料点料框状态信息", "[接收报文:" + JsonTool.ObjectToJson(wParam) + "]");
                FMS.Models.DAQContainerStatus.DAQContainerStatus data = CloneTool.Clone< FMS.Models.DAQContainerStatus.DAQContainerStatus>(wParam);
                BMSEmployee wLoginUser = this.GetSession();
                response = ServiceInstance.mINTERFACEService.INTERFACE_DAQContainerStatus(data);
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                response.response_result = "999";
                response.response_data = ex.Message;
            }
            wResult.Add("response_ID", response.response_ID);
            wResult.Add("response_time", response.response_time);
            wResult.Add("response_result", response.response_result);
            wResult.Add("response_data", response.response_data);
            return Json(wResult);
        }
    }
}
