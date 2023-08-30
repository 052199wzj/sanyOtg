using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FPCRouteDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FPCRouteDAO));

        #region 单实例
        private FPCRouteDAO() { }
        private static FPCRouteDAO _Instance;

        public static FPCRouteDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCRouteDAO();
                return FPCRouteDAO._Instance;
            }
        }
        #endregion

        public int FPC_SaveFPCRoute(FPCRoute wFPCRoute, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFPCRoute.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fpc_route(RouteName,Code,Active,IsStandard,CreateID,CreateTime,EditID,EditTime,SonNumber) VALUES(@wRouteName,@wCode,@wActive,@wIsStandard,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wSonNumber);", wInstance);
                else if (wFPCRoute.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fpc_route SET RouteName=@wRouteName,Code=@wCode,Active=@wActive,IsStandard=@wIsStandard,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,SonNumber=@wSonNumber WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFPCRoute.ID);
                wParms.Add("wRouteName", wFPCRoute.RouteName);
                wParms.Add("wCode", wFPCRoute.Code);
                wParms.Add("wActive", wFPCRoute.Active);
                wParms.Add("wIsStandard", wFPCRoute.IsStandard);
                wParms.Add("wCreateID", wFPCRoute.CreateID);
                wParms.Add("wCreateTime", wFPCRoute.CreateTime);
                wParms.Add("wEditID", wFPCRoute.EditID);
                wParms.Add("wEditTime", wFPCRoute.EditTime);
                wParms.Add("wSonNumber", wFPCRoute.SonNumber);

                wSQLText = this.DMLChange(wSQLText);

                if (wFPCRoute.ID <= 0)
                    wFPCRoute.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FPC_SaveFPCRoute", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FPC_DeleteFPCRouteList(List<FPCRoute> wFPCRouteList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFPCRouteList != null && wFPCRouteList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFPCRouteList.Count; i++)
                    {
                        if (i == wFPCRouteList.Count - 1)
                            wStringBuilder.Append(wFPCRouteList[i].ID);
                        else
                            wStringBuilder.Append(wFPCRouteList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fpc_route WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_DeleteFPCRouteList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FPCRoute> FPC_QueryFPCRouteList(int wID, string wRouteName, string wCode, int wActive, int wIsStandard, out int wErrorCode)
        {
            List<FPCRoute> wResultList = new List<FPCRoute>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM {0}.fpc_route t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wRouteName is null or @wRouteName = '' or t.RouteName= @wRouteName)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wIsStandard <=0 or t.IsStandard= @wIsStandard)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wRouteName", wRouteName);
                wParms.Add("wCode", wCode);
                wParms.Add("wActive", wActive);
                wParms.Add("wIsStandard", wIsStandard);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FPCRoute wFPCRoute = new FPCRoute();
                    wFPCRoute.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFPCRoute.RouteName = StringUtils.parseString(wSqlDataReader["RouteName"]);
                    wFPCRoute.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wFPCRoute.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wFPCRoute.IsStandard = StringUtils.parseInt(wSqlDataReader["IsStandard"]);
                    wFPCRoute.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFPCRoute.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFPCRoute.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFPCRoute.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wFPCRoute.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFPCRoute.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wFPCRoute.SonNumber = StringUtils.parseInt(wSqlDataReader["SonNumber"]);

                    wResultList.Add(wFPCRoute);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_QueryFPCRouteList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

