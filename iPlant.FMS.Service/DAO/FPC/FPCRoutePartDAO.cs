using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FPCRoutePartDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FPCRoutePartDAO));

        #region 单实例
        private FPCRoutePartDAO() { }
        private static FPCRoutePartDAO _Instance;

        public static FPCRoutePartDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCRoutePartDAO();
                return FPCRoutePartDAO._Instance;
            }
        }
        #endregion

        public int FPC_SaveFPCRoutePart(FPCRoutePart wFPCRoutePart, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFPCRoutePart.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fpc_routepart(RouteID,Name,Code,PartID,CreatorID,CreateTime,OrderID,PrevPartID,NextPartIDMap,StandardPeriod,ActualPeriod,ChangeControl) VALUES(@wRouteID,@wName,@wCode,@wPartID,@wCreatorID,@wCreateTime,@wOrderID,@wPrevPartID,@wNextPartIDMap,@wStandardPeriod,@wActualPeriod,@wChangeControl);", wInstance);
                else if (wFPCRoutePart.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fpc_routepart SET RouteID=@wRouteID,Name=@wName,Code=@wCode,PartID=@wPartID,CreatorID=@wCreatorID,CreateTime=@wCreateTime,OrderID=@wOrderID,PrevPartID=@wPrevPartID,NextPartIDMap=@wNextPartIDMap,StandardPeriod=@wStandardPeriod,ActualPeriod=@wActualPeriod,ChangeControl=@wChangeControl WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFPCRoutePart.ID);
                wParms.Add("wRouteID", wFPCRoutePart.RouteID);
                wParms.Add("wName", wFPCRoutePart.Name);
                wParms.Add("wCode", wFPCRoutePart.Code);
                wParms.Add("wPartID", wFPCRoutePart.PartID);
                wParms.Add("wCreatorID", wFPCRoutePart.CreatorID);
                wParms.Add("wCreateTime", wFPCRoutePart.CreateTime);
                wParms.Add("wOrderID", wFPCRoutePart.OrderID);
                wParms.Add("wPrevPartID", wFPCRoutePart.PrevPartID);
                wParms.Add("wNextPartIDMap", JsonTool.ObjectToJson(wFPCRoutePart.NextPartIDMap));
                wParms.Add("wStandardPeriod", wFPCRoutePart.StandardPeriod);
                wParms.Add("wActualPeriod", wFPCRoutePart.ActualPeriod);
                wParms.Add("wChangeControl", wFPCRoutePart.ChangeControl);

                wSQLText = this.DMLChange(wSQLText);

                if (wFPCRoutePart.ID <= 0)
                    wFPCRoutePart.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FMC_SaveFMCShift", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FPC_DeleteFPCRoutePartList(List<FPCRoutePart> wFPCRoutePartList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFPCRoutePartList != null && wFPCRoutePartList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFPCRoutePartList.Count; i++)
                    {
                        if (i == wFPCRoutePartList.Count - 1)
                            wStringBuilder.Append(wFPCRoutePartList[i].ID);
                        else
                            wStringBuilder.Append(wFPCRoutePartList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fpc_routepart WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_DeleteFPCRoutePartList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FPCRoutePart> FPC_QueryFPCRoutePartList(int wID, int wRouteID, string wName, string wCode, int wPartID, Pagination wPagination, out int wErrorCode)
        {
            List<FPCRoutePart> wResultList = new List<FPCRoutePart>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.RouteName,t2.Name PartName,t3.Name PrevPartName,t4.Name Creator FROM {0}.fpc_routepart t"
                    + " left join {0}.fpc_route t1 on t.RouteID=t1.ID "
                    + " left join {0}.fpc_partpoint t2 on t.PartID=t2.ID "
                    + " left join {0}.fpc_partpoint t3 on t.PrevPartID=t3.ID "
                    + " left join {0}.mbs_user t4 on t.CreatorID=t4.ID "
                 + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wRouteID <=0 or t.RouteID= @wRouteID)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wPartID <=0 or t.PartID= @wPartID)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wRouteID", wRouteID);
                wParms.Add("wName", wName);
                wParms.Add("wCode", wCode);
                wParms.Add("wPartID", wPartID);

                Dictionary<int, string> wStepDic = GetStepDic(out wErrorCode);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FPCRoutePart wFPCRoutePart = new FPCRoutePart();
                    wFPCRoutePart.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFPCRoutePart.RouteID = StringUtils.parseInt(wSqlDataReader["RouteID"]);
                    wFPCRoutePart.RouteName = StringUtils.parseString(wSqlDataReader["RouteName"]);
                    wFPCRoutePart.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wFPCRoutePart.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wFPCRoutePart.PartID = StringUtils.parseInt(wSqlDataReader["PartID"]);
                    wFPCRoutePart.PartName = StringUtils.parseString(wSqlDataReader["PartName"]);
                    wFPCRoutePart.CreatorID = StringUtils.parseInt(wSqlDataReader["CreatorID"]);
                    wFPCRoutePart.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFPCRoutePart.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFPCRoutePart.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);
                    wFPCRoutePart.PrevPartID = StringUtils.parseInt(wSqlDataReader["PrevPartID"]);
                    wFPCRoutePart.PrevPartName = StringUtils.parseString(wSqlDataReader["PrevPartName"]);
                    wFPCRoutePart.NextPartIDMap = JsonTool.JsonToObject<Dictionary<string, string>>(StringUtils.parseString(wSqlDataReader["NextPartIDMap"]));
                    wFPCRoutePart.NextPartNames = GetNextPartNames(wFPCRoutePart.NextPartIDMap, wStepDic);
                    wFPCRoutePart.StandardPeriod = StringUtils.parseDouble(wSqlDataReader["StandardPeriod"]);
                    wFPCRoutePart.ActualPeriod = StringUtils.parseDouble(wSqlDataReader["ActualPeriod"]);
                    wFPCRoutePart.ChangeControl = StringUtils.parseInt(wSqlDataReader["ChangeControl"]);

                    wResultList.Add(wFPCRoutePart);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_QueryFPCRoutePartList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        private string GetNextPartNames(Dictionary<string, string> wNextPartIDMap, Dictionary<int, string> wStepDic)
        {
            string wResult = "";
            try
            {
                if (wNextPartIDMap.Count <= 0)
                {
                    return wResult;
                }

                List<string> wNames = new List<string>();
                foreach (string wStepID in wNextPartIDMap.Keys)
                {
                    int wID = StringUtils.parseInt(wStepID);
                    if (wStepDic.ContainsKey(wID))
                        wNames.Add(wStepDic[wID]);
                }
                wResult = String.Join(",", wNames);
            }
            catch (Exception ex)
            {
                logger.Error("GetNextPartNames", ex);
            }
            return wResult;
        }

        private Dictionary<int, string> GetStepDic(out int wErrorCode)
        {
            Dictionary<int, string> wResultList = new Dictionary<int, string>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT ID,Name FROM {0}.fpc_partpoint;", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    int wID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    string wName = StringUtils.parseString(wSqlDataReader["Name"]);
                    wResultList.Add(wID, wName);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_QueryFPCRoutePartList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

