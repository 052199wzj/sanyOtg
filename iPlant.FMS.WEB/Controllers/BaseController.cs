using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using iPlant.Data.EF.Repository;
using iPlant.Data.EF;

namespace iPlant.FMS.WEB
{

    public class RetCode
    {

        public static int SERVER_CODE_SUC = 1000;
        public static int SERVER_CODE_ERR = 9999;
        public static String SERVER_CODE_ERR_MSG = "处理异常";
        public static int SERVER_CODE_UNLOGIN = 9998;
        public static String SERVER_CODE_UNLOGIN_ALARM_NOPD = "密码错误!";
        public static String SERVER_CODE_UNLOGIN_ALARM_TOKEN = "免密验证失败!";

        public static String SERVER_CODE_UNROLE = "无权限！";

        public static String SERVER_CODE_UNLOGIN_ALARM = "用户名或密码不正确!";
        public static String SERVER_CODE_UNLOGIN_ALARM_NOMAC = "终端不匹配！";

        public static int LOGIN_ERR_CODE_LOGIN_FAIL = 9997;
        public static int PERMISSION_DENIED = 9994;

        public static String SERVER_RST_NULL = "服务返回值为空！";
        public static String SERVER_RST_ERROR_IN = "服务器内部错误！";
        public static String SERVER_RST_ERROR_FAILED = "操作失败！";
        public static String SERVER_RST_ERROR_OUT = "输入参数错误或参数不合法！";
        public static String SERVER_RST_MONITOR_ERR = "设备采集内部错误或未启用！";
    }

    public class BaseController : Controller
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BaseController));

        public new HttpRequest Request { get { return this.HttpContext.Request; } }

        public new HttpResponse Response { get { return this.HttpContext.Response; } }

        public ISession Session { get { return this.HttpContext.Session; } }


        public const String RESULT_KEY = "resultCode";

        public const String RESULT_MSG = "msg";

        public const String RESULT_RETURN = "returnObject";

        public const String DATA_LIST = "list";

        public const String DATA_INFO = "info";




        protected int GetShiftID(int wCompanyID, int wUserID)
        {
            return StringUtils.parseInt(DateTime.Now.ToString("yyyyMMdd"));
        }


        protected Boolean CheckCookieEmpty()
        {
            Boolean wRst = false;


            BMSEmployee wBMSEmployee = this.GetSession();
            if (wBMSEmployee.ID <= 0 && wBMSEmployee.ID != -100)
            {
                String wLoginName = getCookieValue(SessionContants.CookieUser, Request);
                String wID = getCookieValue(SessionContants.CookieUserID, Request);
                if (String.IsNullOrWhiteSpace(wID))
                {

                    wRst = true;
                }
                else
                {
                    wBMSEmployee.ID = StringUtils.parseInt(wID);
                }
                if (StringUtils.isEmpty(wLoginName))
                {
                    wRst = true;
                }
                else
                {
                    wBMSEmployee.LoginName = wLoginName;
                }
                SetSession(HttpContext.Session, wBMSEmployee);
            }
            if (wBMSEmployee.ID <= 0 && wBMSEmployee.ID != -100)
            {
                wRst = true;
            }
            return wRst;

        }



        protected BMSEmployee GetSession()
        {
            BMSEmployee wBMSEmployee = new BMSEmployee();

            if (!String.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionContants.SessionUser)))
            {
                wBMSEmployee = JsonSerializer.Deserialize<BMSEmployee>(HttpContext.Session.GetString(SessionContants.SessionUser));
                if (wBMSEmployee == null)
                    wBMSEmployee = new BMSEmployee();
            }
            return wBMSEmployee;

        }


        public static BMSEmployee GetSession(ISession wSession)
        {
            BMSEmployee wBMSEmployee = new BMSEmployee();

            if (!String.IsNullOrWhiteSpace(wSession.GetString(SessionContants.SessionUser)))
            {
                wBMSEmployee = JsonSerializer.Deserialize<BMSEmployee>(wSession.GetString(SessionContants.SessionUser));
                if (wBMSEmployee == null)
                    wBMSEmployee = new BMSEmployee();
            }
            return wBMSEmployee;

        }

        public static void RmoveSession(ISession wSession)
        {

            if (!String.IsNullOrWhiteSpace(wSession.GetString(SessionContants.SessionUser)))
            {
                wSession.Remove(SessionContants.SessionUser);
            }
        }

        public static void SetSession(ISession wSession, BMSEmployee wBMSEmployee)
        {

            if (wBMSEmployee == null)
                return;
            if (wSession.Keys.Contains(SessionContants.SessionUser))
            {
                wSession.Remove(SessionContants.SessionUser);
            }
            wSession.SetString(SessionContants.SessionUser, JsonSerializer.Serialize<BMSEmployee>(wBMSEmployee));
        }


        public static void RmoveCookie(HttpRequest wRequest, HttpResponse wResponse)
        {


            if (wRequest.Cookies != null && wResponse.Cookies != null)
            {


                if (wRequest.Cookies.ContainsKey(SessionContants.CookieUser))
                {
                    wResponse.Cookies.Delete(SessionContants.CookieUser);
                }
                if (wRequest.Cookies.ContainsKey(SessionContants.CookieUserID))
                {
                    wResponse.Cookies.Delete(SessionContants.CookieUserID);
                }


                var co = new CookieOptions();
                co.Path = "/";

                //on localhost, Domain value is not needed !!
                //co.Domain = "";

                co.Expires = DateTime.Now.AddDays(-1);
                co.HttpOnly = true;

                wResponse.Cookies.Append(SessionContants.CookieUser, "", co);
                wResponse.Cookies.Append(SessionContants.CookieUserID, "", co);
            }
        }



        public static void SetCookie(HttpRequest wRequest, HttpResponse wResponse, BMSEmployee wBMSEmployee)
        {
            if (wBMSEmployee == null)
                return;

            RmoveCookie(wRequest, wResponse);


            var co = new CookieOptions();
            co.Path = "/";

            //on localhost, Domain value is not needed !!
            //co.Domain = "";

            co.HttpOnly = true;

            wResponse.Cookies.Append(SessionContants.CookieUser, wBMSEmployee.LoginName, co);

            wResponse.Cookies.Append(SessionContants.CookieUserID, DesUtil.encrypt(wBMSEmployee.ID + "", SessionContants.appSecret), co);

        }


        public static String getCookieValue(String cookie_key, HttpRequest request)
        {

            String cookie_val = null;

            if (request.Cookies != null && request.Cookies.Count > 0)
            {
                if (request.Cookies.ContainsKey(cookie_key))
                {
                    request.Cookies.TryGetValue(cookie_key, out cookie_val);
                }
            }
            else
            {
                StringValues wStringValues = new StringValues();
                if (request.Query.TryGetValue(cookie_key, out wStringValues))
                {

                    foreach (var item in wStringValues)
                    {
                        if (String.IsNullOrWhiteSpace(item))
                            continue;

                        cookie_val = item;
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(cookie_val))
                return cookie_val;

            try
            {
                cookie_val = DesUtil.decrypt(cookie_val, SessionContants.appSecret);
            }
            catch (Exception e)
            {
                // TODO 自动生成的 catch 块 
                logger.Error("getCookieValue", e);
            }

            return cookie_val;
        }

        public static String CreateToken(String account)
        {
            String wToken = "";

            try
            {
                String wT4 = account.Substring(0, account.Length / 2);
                String wT2 = account.Substring(account.Length / 2);
                String wT3 = DateTime.Now.ToString("yyyy-MM");
                String wT5 = String.Format("{0:D2}", DateTime.Now.Day);
                String wT1 = DateTime.Now.ToString("HH:mm:ss");

                wToken = String.Format("{0}+-abc072-+{1}+-abc072-+{2}+-abc072-+{3}+-abc072-+{4}", wT1, wT2, wT3, wT4,
                        wT5);

                wToken = DesUtil.encrypt(wToken, SessionContants.appSecret);
            }
            catch (Exception e)
            {
                logger.Error("CreateToken", e);
            }

            return wToken;
        }


        public static String GetProjectName(HttpRequest request)
        {

            String wResult = "";


            String wURL = new StringBuilder()

             .Append(request.PathBase)
             .Append(request.Path)
             .ToString();


            if (wURL.IndexOf("/api/") <= 0)
                return wResult;

            wResult = wURL.Substring(0, wURL.IndexOf("/api/"));
            return wResult;
        }

        /// <summary>
        /// 获取返回ReturnObject
        /// </summary>
        /// <param name="wMsg"></param>
        /// <param name="wReturnObjectList"></param>
        /// <param name="wReturnObjectInfo"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetReturnObject(string wMsg, object wReturnObjectList, object wReturnObjectInfo)
        {
            return new Dictionary<string, object> { { RESULT_MSG, wMsg }, { DATA_LIST, wReturnObjectList }, { DATA_INFO, wReturnObjectInfo } };
        }

        /// <summary>
        /// 特殊处理返回  可修改ReturnObject后返回
        /// </summary>
        /// <param name="wResultCode"></param>
        /// <param name="wReturnObject"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetResult(int wResultCode, Dictionary<string, object> wReturnObject)
        {
            return new Dictionary<string, object> { { RESULT_KEY, wResultCode }, { RESULT_RETURN, wReturnObject } };
        }
        /// <summary>
        /// 普通返回
        /// </summary>
        /// <param name="wResultCode"></param>
        /// <param name="wMsg"></param>
        /// <param name="wReturnObjectList">返回数组</param>
        /// <param name="wReturnObjectInfo">返回对象</param>
        /// <returns></returns>
        public Dictionary<string, object> GetResult(int wResultCode, string wMsg, object wReturnObjectList, object wReturnObjectInfo)
        {
            return GetResult(wResultCode, GetReturnObject(wMsg, wReturnObjectList, wReturnObjectInfo));
        }
        public Dictionary<string, object> SetResult(Dictionary<string, object> wResult, String wObjectName, Object wObject)
        {
            if (wResult.ContainsKey(RESULT_RETURN))
            {
                ((Dictionary<string, object>)wResult[RESULT_RETURN])[wObjectName] = wObject;
            }

            return wResult;
        }


        /// <summary>
        /// 报错返回
        /// </summary>
        /// <param name="wResultCode"></param>
        /// <param name="wMsg"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetResult(int wResultCode, string wMsg)
        {
            return GetResult(wResultCode, GetReturnObject(wMsg, null, null));
        }



        public Dictionary<String, Object> GetInputDictionaryObject(HttpRequest wRequest)
        {
            Dictionary<String, Object> wResult = new Dictionary<string, object>();

            
            StreamReader wStreamReader = new StreamReader(wRequest.Body);
         
            string wJson = wStreamReader.ReadToEndAsync().Result;

            try
            {
                wResult = JsonTool.JsonToObject<Dictionary<String, Object>>(wJson);
            }
            catch (Exception)
            {
                logger.Error(StringUtils.Format("GetInputDictionaryObject Json:{0}", wJson));
                throw;
            }

            if (wResult != null)
                RemoveExtensionData(wResult);

            return wResult;
        }

        private void RemoveExtensionData(Dictionary<String, Object> wInput)
        {
            if (wInput.ContainsKey("ExtensionData"))
                wInput.Remove("ExtensionData");

            foreach (String wKey in wInput.Keys)
            {
                if (wInput[wKey] == null)
                    continue;
                if (wInput[wKey] is Dictionary<String, Object>)
                {
                    RemoveExtensionData((Dictionary<String, Object>)wInput[wKey]);
                    continue;
                }

                if (wInput[wKey] is List<Dictionary<String, Object>>)
                {
                    foreach (Dictionary<String, Object> item in (List<Dictionary<String, Object>>)wInput[wKey])
                    {
                        RemoveExtensionData(item);
                    }
                    continue;
                }
                if (wInput[wKey] is Dictionary<String, Object>[])
                {
                    foreach (Dictionary<String, Object> item in (Dictionary<String, Object>[])wInput[wKey])
                    {
                        RemoveExtensionData(item);
                    }
                    continue;
                }
                if (wInput[wKey] is ArrayList)
                {
                    //
                    foreach (var item in (ArrayList)wInput[wKey])
                    {
                        if (item is Dictionary<String, Object>)
                            RemoveExtensionData((Dictionary<String, Object>)item);
                    }
                    continue;
                }
                if (wInput[wKey] is Array)
                {
                    //
                    foreach (var item in (Array)wInput[wKey])
                    {
                        if (item is Dictionary<String, Object>)
                            RemoveExtensionData((Dictionary<String, Object>)item);
                    }
                    continue;
                }

            }

        }



        public MemoryStream SetInputObject<T>(T wT)
        {
            string wJson = JsonTool.ObjectToJson<T>(wT);

            byte[] wArray = Encoding.UTF8.GetBytes(wJson);

            MemoryStream wMemoryStream = new MemoryStream(wArray);

            return wMemoryStream;
        }


        public static void SaveApiLog(int wID, String wURI,
                String wResult, DateTime wResponseTime, long wInterval, int wStatus)
        {
            try
            {

                if (wID <= 0)
                    return;

                if (wResult == null)
                    wResult = "";

                if (!wURI.Contains("Sync"))
                {
                    if (wResult.Length > 200)
                    {
                        wResult = wResult.Substring(0, 200) + "...";
                    }
                }

                using (DbConnection wMySqlConnection = RepositoryFactory.GetDBPool().GetConnection())
                {
                    String wSQL = StringUtils.Format(
                          "Update  {0}.mbs_apilog  set Result = @Result , ResponseTime= @ResponseTime,IntervalTime= @IntervalTime,ResponseStatus= @ResponseStatus where ID=@ID;",
                          MESDBSource.Basic.getDBName());

                    CommandTool wCommandTool = new CommandTool(wSQL, wMySqlConnection);

                    wCommandTool.AddWithValue("ID", wID);
                    wCommandTool.AddWithValue("Result", wResult);
                    wCommandTool.AddWithValue("ResponseTime", wResponseTime);
                    wCommandTool.AddWithValue("IntervalTime", wInterval);
                    wCommandTool.AddWithValue("ResponseStatus", wStatus);

                    wCommandTool.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                logger.Error("SaveApiLog", ex);
            }
        }

        public static long SaveApiLog(int wCompanyID, int wLoginID, String wProjectName, String wURI, String wMethod,
                String wParams, String wRequestBody, DateTime wRequestTime)
        {
            long wID = 0;
            try
            {
                if (wParams == null)
                    wParams = "";

                if (wProjectName == null)
                    wProjectName = "";
                if (wURI == null || wMethod == null)
                    return wID;

                if (!wURI.Contains("Sync"))
                {
                    if (wParams.Length > 200)
                    {
                        wParams = wParams.Substring(0, 200) + "...";
                    }
                    if (wRequestBody.Length > 200)
                    {
                        wRequestBody = wRequestBody.Substring(0, 200) + "...";
                    }
                }

                using (DbConnection wMySqlConnection = RepositoryFactory.GetDBPool().GetConnection())
                {
                    String wSQL = StringUtils.Format(
                          "INSERT INTO {0}.mbs_apilog (CompanyID,LoginID,ProjectName,URI,Method,Params,RequestBody,Result,RequestTime,ResponseTime,IntervalTime,ResponseStatus)"
                                  + " Values (@CompanyID, @LoginID,@ProjectName,@URI,@Method,@Params,@RequestBody,@Result,@RequestTime,@ResponseTime,@IntervalTime,@ResponseStatus)",
                          MESDBSource.Basic.getDBName());


                    CommandTool wCommandTool = new CommandTool(wSQL, wMySqlConnection);

                    wCommandTool.AddWithValue("CompanyID", wCompanyID);
                    wCommandTool.AddWithValue("LoginID", wLoginID);
                    wCommandTool.AddWithValue("ProjectName", wProjectName);
                    wCommandTool.AddWithValue("URI", wURI);
                    wCommandTool.AddWithValue("Method", wMethod);
                    wCommandTool.AddWithValue("Params", wParams);
                    wCommandTool.AddWithValue("Result", "");
                    wCommandTool.AddWithValue("RequestTime", wRequestTime);
                    wCommandTool.AddWithValue("ResponseTime", new DateTime(2000, 1, 1));
                    wCommandTool.AddWithValue("RequestBody", wRequestBody);
                    wCommandTool.AddWithValue("IntervalTime", 0);
                    wCommandTool.AddWithValue("ResponseStatus", 0);

                    wCommandTool.ExecuteNonQuery();
                    wID = wCommandTool.LastInsertID;
                }

            }
            catch (Exception ex)
            {
                logger.Error("SaveApiLog", ex);
            }
            return wID;
        }



        private static int GetPageCount(String wCondition, int wPageSize, Dictionary<String, Object> wParamMap)
        {
            String wSQLTextSize = "Select count(0) as ItemCount " + wCondition + ";";
            Dictionary<String, Object> wQueryResultDic = RepositoryFactory.GetDBPool().queryForMap(wSQLTextSize, wParamMap);
            int wSize = 0;
            if (wQueryResultDic != null && wQueryResultDic.ContainsKey("ItemCount"))
            {
                wSize = StringUtils.parseInt(wQueryResultDic["ItemCount"]);
            }
            if (wSize > 0)
                return wSize / wPageSize + ((wSize % wPageSize) > 0 ? 1 : 0);

            return 0;
        }


        public static List<Dictionary<String, Object>> GetApiLog(int wCompanyID, int wLoginID, String wProjectName, String wUri,
             int wIntervalMin, int wIntervalMax, int wResponseStatus, DateTime wStartTime, DateTime wEndTime, Pagination wPagination)
        {

            List<Dictionary<String, Object>> wResult = new List<Dictionary<String, Object>>();
            try
            {
                if (wProjectName == null)
                    wProjectName = "";
                if (wUri == null)
                    wUri = "";
                DateTime wBaseTime = new DateTime(2000, 1, 1);
                if (wStartTime.CompareTo(wBaseTime) < 0)
                    wStartTime = wBaseTime;
                if (wEndTime.CompareTo(wBaseTime) < 0)
                    wEndTime = wBaseTime;
                if (wStartTime.CompareTo(wEndTime) > 0)
                    return wResult;



                String wSQL = StringUtils.Format("SELECT *  FROM {0}.mbs_apilog WHERE 1=1 "
                        + "AND ( @StartTime <= str_to_date('2010-01-01', '%Y-%m-%d') or @StartTime <= mbs_apilog.ResponseTime) "
                        + "AND ( @EndTime <= str_to_date('2010-01-01', '%Y-%m-%d') or @EndTime >= mbs_apilog.RequestTime) "
                        + "AND (@CompanyID <= 0  OR CompanyID = @CompanyID) "
                        + "AND (@LoginID <= 0  OR LoginID = @LoginID) "
                        + "AND (@IntervalMin <= 0  OR IntervalTime >= @IntervalMin) "
                        + "AND (@IntervalMax <= 0  OR IntervalTime <= @IntervalMax) "
                        + "AND (@ResponseStatus <= 0  OR ResponseStatus = @ResponseStatus) "
                        + "AND (@ProjectName =''  OR  ProjectName like @ProjectName) "
                        + "AND (@Uri =''  OR  URI like @Uri) ",
                        MESDBSource.Basic.getDBName());


                Dictionary<String, Object> wParamMap = new Dictionary<String, Object>();
                wParamMap.Add("CompanyID", wCompanyID);
                wParamMap.Add("LoginID", wLoginID);
                wParamMap.Add("ProjectName", String.IsNullOrWhiteSpace(wProjectName) ? "" : ("%" + wProjectName.Trim() + "%"));
                wParamMap.Add("Uri", String.IsNullOrWhiteSpace(wUri) ? "" : ("%" + wUri.Trim() + "%"));
                wParamMap.Add("StartTime", wStartTime);
                wParamMap.Add("EndTime", wEndTime);
                wParamMap.Add("IntervalMin", wIntervalMin);
                wParamMap.Add("IntervalMax", wIntervalMax);
                wParamMap.Add("ResponseStatus", wResponseStatus);

                wResult = RepositoryFactory.GetDBPool().queryForList(wSQL, wParamMap, wPagination);

            }
            catch (Exception ex)
            {
                logger.Error("GetApiLog", ex);
            }
            return wResult;
        }




    }

    public class SessionContants
    {
        public static String TOKEN = "";

        public static String appSecret = "c5e330214fb33e2d80f14e3fc45ed214";

        public static String Key = "bl6L4gzCvFSQQseTTapubIFQiZOuc3g2suvwFdNoACz";

        public static String USER_INFO = "cadv_ao";

        public static String USER_PASSWORD = "cade_po";

        public static String SessionUser = "user_info";

        public static String Extension_Module_ID = "User_ModuleID";

        public static String CookieUser = "_user_info_";

        public static String CookieUserID = "_user_info_ct_";

    }

}