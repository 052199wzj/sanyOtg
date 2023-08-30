using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MCSInterfaceConfigDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MCSInterfaceConfigDAO));

        #region 单实例
        private MCSInterfaceConfigDAO() { }
        private static MCSInterfaceConfigDAO _Instance;

        public static MCSInterfaceConfigDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MCSInterfaceConfigDAO();
                return MCSInterfaceConfigDAO._Instance;
            }
        }
        #endregion

        public int MCS_SaveMCSInterfaceConfig(MCSInterfaceConfig wMCSInterfaceConfig, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMCSInterfaceConfig.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mcs_interfaceconfig(Name,Type,Uri,EnumFlag,Remark,CreateID,CreateTime,EditID,EditTime) VALUES(@wName,@wType,@wUri,@wEnumFlag,@wRemark,@wCreateID,@wCreateTime,@wEditID,@wEditTime);", wInstance);
                else if (wMCSInterfaceConfig.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mcs_interfaceconfig SET Name=@wName,Type=@wType,Uri=@wUri,EnumFlag=@wEnumFlag,Remark=@wRemark,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMCSInterfaceConfig.ID);
                wParms.Add("wName", wMCSInterfaceConfig.Name);
                wParms.Add("wType", wMCSInterfaceConfig.Type);
                wParms.Add("wUri", wMCSInterfaceConfig.Uri);
                wParms.Add("wEnumFlag", wMCSInterfaceConfig.EnumFlag);
                wParms.Add("wRemark", wMCSInterfaceConfig.Remark);
                wParms.Add("wCreateID", wMCSInterfaceConfig.CreateID);
                wParms.Add("wCreateTime", wMCSInterfaceConfig.CreateTime);
                wParms.Add("wEditID", wMCSInterfaceConfig.EditID);
                wParms.Add("wEditTime", wMCSInterfaceConfig.EditTime);

                wSQLText = this.DMLChange(wSQLText);

                if (wMCSInterfaceConfig.ID <= 0)
                    wMCSInterfaceConfig.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("MCS_SaveMCSInterfaceConfig", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MCS_DeleteMCSInterfaceConfigList(List<MCSInterfaceConfig> wMCSInterfaceConfigList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMCSInterfaceConfigList != null && wMCSInterfaceConfigList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMCSInterfaceConfigList.Count; i++)
                    {
                        if (i == wMCSInterfaceConfigList.Count - 1)
                            wStringBuilder.Append(wMCSInterfaceConfigList[i].ID);
                        else
                            wStringBuilder.Append(wMCSInterfaceConfigList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mcs_interfaceconfig WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MCS_DeleteMCSInterfaceConfigList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MCSInterfaceConfig> MCS_QueryMCSInterfaceConfigList(int wID, string wName, int wType, string wEnumFlag, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<MCSInterfaceConfig> wResultList = new List<MCSInterfaceConfig>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM mcs_interfaceconfig t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wType <=0 or t.Type= @wType)"
                + " and(@wEnumFlag is null or @wEnumFlag = '' or t.EnumFlag= @wEnumFlag)"
                + " and(@wStartTime <= '2010-1-1' or t.EditTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wName", wName);
                wParms.Add("wType", wType);
                wParms.Add("wEnumFlag", wEnumFlag);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MCSInterfaceConfig wMCSInterfaceConfig = new MCSInterfaceConfig();
                    wMCSInterfaceConfig.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMCSInterfaceConfig.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMCSInterfaceConfig.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wMCSInterfaceConfig.Uri = StringUtils.parseString(wSqlDataReader["Uri"]);
                    wMCSInterfaceConfig.EnumFlag = StringUtils.parseString(wSqlDataReader["EnumFlag"]);
                    wMCSInterfaceConfig.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMCSInterfaceConfig.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMCSInterfaceConfig.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMCSInterfaceConfig.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMCSInterfaceConfig.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMCSInterfaceConfig.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMCSInterfaceConfig.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);

                    wResultList.Add(wMCSInterfaceConfig);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MCS_QueryMCSInterfaceConfigList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

