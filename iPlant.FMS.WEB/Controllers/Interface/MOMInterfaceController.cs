
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
using Newtonsoft.Json;

namespace iPlant.FMS.WEB
{
    /// <summary>
    /// 与MOM系统接口
    /// </summary>
    public class MOMInterfaceController : BaseController
    {

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MOMInterfaceController));

        /// <summary>
        /// 查询料点料框状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryContainer()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);

                BMSEmployee wBMSEmployee = GetSession();
                int wUserID = wBMSEmployee.ID;

                if (!wParam.ContainsKey("data"))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, RetCode.SERVER_RST_ERROR_OUT);
                    return Json(wResult);
                }

                MSSMaterialPoint wMSSMaterialPoint = CloneTool.Clone<MSSMaterialPoint>(wParam["data"]);

                ServiceResult<Int32> wServiceResult = ServiceInstance.mINTERFACEService.INTERFACE_QueryContainer(wBMSEmployee, wMSSMaterialPoint);

                if (StringUtils.isEmpty(wServiceResult.getFaultCode()))
                {
                    wResult = GetResult(RetCode.SERVER_CODE_SUC, "", null, wMSSMaterialPoint);
                }
                else
                {
                    wResult = GetResult(RetCode.SERVER_CODE_ERR, wServiceResult.getFaultCode(), null, wMSSMaterialPoint);
                }
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                wResult = GetResult(RetCode.SERVER_CODE_ERR, ex.ToString(), null, null);
            }
            return Json(wResult);
        }

        /// <summary>
        /// 自动发送中控站点状态反馈信息(中控→MOM)
        /// </summary>
        /// <returns></returns>

        public ActionResult MOMSendFrameStatus()
        {
            Dictionary<String, Object> wResult = new Dictionary<String, Object>();
            try
            {
                BMSEmployee wBMSEmployee = GetSession();

                ServiceResult<List<String>> wServerRst = ServiceInstance.mINTERFACEService.INTERFACE_SendStationState(wBMSEmployee);

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

        /// <summary>
        /// 发送MOM接口测试 查询料框信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendMOMTest()
        {
            Dictionary<string, object> wResult = new Dictionary<string, object>();
            FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse response = new FMS.Models.MOMSysQueryContainerResponse.MOMSysQueryContainerResponse();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                //ServiceInstance.mINTERFACEService.WriteLog("[接收报文:" + JsonTool.ObjectToJson(wParam) + "]", "发送LES接口测试", "LES系统", "V1");
                ServiceInstance.mINTERFACEService.WriteLog("MOM系统", "发送MOM接口测试", "发送MOM接口测试", 1, "接收报文", "[接收报文:" + JsonTool.ObjectToJson(wParam) + "]");
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                response.code = "999";
                response.msg = ex.Message;
            }
            response.version = 1;
            response.taskId = "";
            response.code = "0";
            response.msg = "返回料点信息报文";
            response.returnData.palletType = "大件料框";
            response.returnData.palletNo = "AP114514";
            response.returnData.materialList.Add(new FMS.Models.MOMSysQueryContainerResponse.materialList
            {
                wiporderId = "QDZ20220814001",
                materialNo = "SG1000",
                quantity = 1,
                serialNo = "",
                sourceSequenceNo = "AP20230814",
                sourceOprSequenceNo = "AP202308140001"
            }
             );
            response.returnData.materialList.Add(new FMS.Models.MOMSysQueryContainerResponse.materialList
            {
                wiporderId = "QD100001",
                materialNo = "SG250",
                quantity = 1,
                serialNo = "",
                sourceSequenceNo = "AP20230814",
                sourceOprSequenceNo = "AP202308140001"
            }
 );
            wResult.Add("code", response.code);
            wResult.Add("taskId", response.taskId);
            wResult.Add("msg", response.msg);
            wResult.Add("returnData", response.returnData);

            //ServiceInstance.mINTERFACEService.WriteLog("[应答报文:" + JsonTool.ObjectToJson(wResult) + "]", "发送LES接口测试", "LES系统", "V1");
            //ServiceInstance.mINTERFACEService.WriteLog("MOM系统", "发送MOM接口测试", "发送MOM接口测试", 1, "接收报文", "[应答报文:" + JsonTool.ObjectToJson(wResult) + "]");
            return Json(wResult);
        }

        /// <summary>
        /// 发送MOM接口测试 工序配送
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendMOMTest1()
        {
            Dictionary<string, object> wResult = new Dictionary<string, object>();
            FMS.Models.MOMSysProdureManageResponse.MOMSysProdureManageResponse response = new FMS.Models.MOMSysProdureManageResponse.MOMSysProdureManageResponse();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                //ServiceInstance.mINTERFACEService.WriteLog("[接收报文:" + JsonTool.ObjectToJson(wParam) + "]", "发送LES接口测试", "LES系统", "V1");
                ServiceInstance.mINTERFACEService.WriteLog("MOM系统", "发送MOM接口测试", "发送MOM接口测试", 1, "接收报文", "[接收报文:" + JsonTool.ObjectToJson(wParam) + "]");
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                response.code = "999";
                response.msg = ex.Message;
            }
            response.version = 1;
            response.taskId = "";
            response.code = "0";
            response.msg = "返回工序配送信息报文";

            wResult.Add("code", response.code);
            wResult.Add("taskId", response.taskId);
            wResult.Add("msg", response.msg);
            wResult.Add("returnData", response.returnData);

            return Json(wResult);
        }

        public ActionResult SendMOMTest2()
        {
            Dictionary<string, object> wResult = new Dictionary<string, object>();
            FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse response = new FMS.Models.MOMSysQuerySeqNoResponse.MOMSysQuerySeqNoResponse();
            try
            {
                Dictionary<string, object> wParam = GetInputDictionaryObject(Request);
                //ServiceInstance.mINTERFACEService.WriteLog("[接收报文:" + JsonTool.ObjectToJson(wParam) + "]", "发送LES接口测试", "LES系统", "V1");
                ServiceInstance.mINTERFACEService.WriteLog("MOM系统", "发送MOM接口测试", "发送MOM接口测试", 1, "接收报文", "[接收报文:" + JsonTool.ObjectToJson(wParam) + "]");
            }
            catch (Exception ex)
            {
                logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                response.code = "999";
                response.msg = ex.Message;
            }
            response.version = 1;
            response.taskId = "";
            response.code = "0";
            response.msg = "返回工序配送信息报文";
            response.returnData.OprSequenceNo = "Opr114514";
            response.returnData.OprSequenceName = "OprName114514";


            wResult.Add("code", response.code);
            wResult.Add("taskId", response.taskId);
            wResult.Add("msg", response.msg);
            wResult.Add("returnData", response.returnData);

            return Json(wResult);
        }
    }
}
