using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MSSMaterialStockDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialStockDAO));

        #region 单实例
        private MSSMaterialStockDAO() { }
        private static MSSMaterialStockDAO _Instance;

        public static MSSMaterialStockDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSMaterialStockDAO();
                return MSSMaterialStockDAO._Instance;
            }
        }
        #endregion

        public int MSS_SaveMSSMaterialStock(MSSMaterialStock wMSSMaterialStock, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMSSMaterialStock.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mss_materialstock(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,MaterialPointID,PlateID,OrderID) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wMaterialPointID,@wPlateID,@wOrderID);", wInstance);
                else if (wMSSMaterialStock.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mss_materialstock SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,MaterialPointID=@wMaterialPointID,PlateID=@wPlateID,OrderID=@wOrderID WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMSSMaterialStock.ID);
                wParms.Add("wCode", wMSSMaterialStock.Code);
                wParms.Add("wName", wMSSMaterialStock.Name);
                wParms.Add("wRemark", wMSSMaterialStock.Remark);
                wParms.Add("wActive", wMSSMaterialStock.Active);
                wParms.Add("wCreateID", wMSSMaterialStock.CreateID);
                wParms.Add("wCreateTime", wMSSMaterialStock.CreateTime);
                wParms.Add("wEditID", wMSSMaterialStock.EditID);
                wParms.Add("wEditTime", wMSSMaterialStock.EditTime);
                wParms.Add("wMaterialPointID", wMSSMaterialStock.MaterialPointID);
                wParms.Add("wPlateID", wMSSMaterialStock.PlateID);
                wParms.Add("wOrderID", wMSSMaterialStock.OrderID);

                wSQLText = this.DMLChange(wSQLText);

                if (wMSSMaterialStock.ID <= 0)
                    wMSSMaterialStock.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("MSS_SaveMSSMaterialStock", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MSS_DeleteMSSMaterialStockList(List<MSSMaterialStock> wMSSMaterialStockList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMSSMaterialStockList != null && wMSSMaterialStockList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMSSMaterialStockList.Count; i++)
                    {
                        if (i == wMSSMaterialStockList.Count - 1)
                            wStringBuilder.Append(wMSSMaterialStockList[i].ID);
                        else
                            wStringBuilder.Append(wMSSMaterialStockList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mss_materialstock WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MSS_DeleteMSSMaterialStockList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MSSMaterialStock> MSS_QueryMSSMaterialStockList(int wID, string wCode, string wName, int wActive, int wMaterialPointID, int wPlateID, int wOrderID, Pagination wPagination, out int wErrorCode)
        {
            List<MSSMaterialStock> wResultList = new List<MSSMaterialStock>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor,t4.Name PlateName,t5.Name MaterialPointName FROM mss_materialstock t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + " left join {0}.fpc_structuralpart t4 on t.PlateID=t4.ID "
                    + " left join {0}.mss_materialpoint t5 on t.MaterialPointID=t5.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wMaterialPointID <=0 or t.MaterialPointID= @wMaterialPointID)"
                + " and(@wPlateID <=0 or t.PlateID= @wPlateID)"
                + " and(@wOrderID <=0 or t.OrderID= @wOrderID)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wCode", wCode);
                wParms.Add("wName", wName);
                wParms.Add("wActive", wActive);
                wParms.Add("wMaterialPointID", wMaterialPointID);
                wParms.Add("wPlateID", wPlateID);
                wParms.Add("wOrderID", wOrderID);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialStock wMSSMaterialStock = new MSSMaterialStock();
                    wMSSMaterialStock.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSMaterialStock.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMSSMaterialStock.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSMaterialStock.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMSSMaterialStock.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMSSMaterialStock.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMSSMaterialStock.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMSSMaterialStock.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMSSMaterialStock.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMSSMaterialStock.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMSSMaterialStock.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wMSSMaterialStock.MaterialPointID = StringUtils.parseInt(wSqlDataReader["MaterialPointID"]);
                    wMSSMaterialStock.MaterialPointName = StringUtils.parseString(wSqlDataReader["MaterialPointName"]);
                    wMSSMaterialStock.PlateID = StringUtils.parseInt(wSqlDataReader["PlateID"]);
                    wMSSMaterialStock.PlateName = StringUtils.parseString(wSqlDataReader["PlateName"]);
                    wMSSMaterialStock.OrderID = StringUtils.parseInt(wSqlDataReader["OrderID"]);

                    wResultList.Add(wMSSMaterialStock);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MSS_QueryMSSMaterialStockList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        public string GetNewCode(out int wErrorCode)
        {
            string wResult = "";
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQL = StringUtils.Format(
                        "SELECT Code FROM {0}.mss_materialstock WHERE id IN( SELECT MAX(ID) FROM {0}.mss_materialstock);",
                        wInstance);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQL, wParms);

                int wNumber = 1;
                int wMonth = DateTime.Now.Month;
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    String wDemandNo = StringUtils.parseString(wSqlDataReader["Code"]);
                    int wCodeMonth = StringUtils.parseInt(wDemandNo.Substring(4, 2));
                    if (wMonth > wCodeMonth)
                        wNumber = 1;
                    else
                        wNumber = StringUtils.parseInt(wDemandNo.Substring(9)) + 1;
                }

                //20220713F001
                wResult = StringUtils.Format("{0}{1}{2}S{3}", DateTime.Now.Year,
                        (DateTime.Now.Month).ToString("00"), DateTime.Now.Day.ToString("00"),
                        wNumber.ToString("000"));
            }
            catch (Exception ex)
            {
                logger.Error("GetNewCode", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }
    }
}

