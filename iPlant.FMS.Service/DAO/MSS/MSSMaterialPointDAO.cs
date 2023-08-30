using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MSSMaterialPointDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSMaterialPointDAO));

        #region 单实例
        private MSSMaterialPointDAO() { }
        private static MSSMaterialPointDAO _Instance;

        public static MSSMaterialPointDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSMaterialPointDAO();
                return MSSMaterialPointDAO._Instance;
            }
        }
        #endregion

        public int MSS_SaveMSSMaterialPoint(MSSMaterialPoint wMSSMaterialPoint, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMSSMaterialPoint.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mss_materialpoint(LineID,AssetID,Name,StationPoint,DeliveryPoint,MaterialNo,PlanNo,UpdateTime) VALUES(@wLineID,@wAssetID,@wName,@wStationPoint,@wDeliveryPoint,@wMaterialNo,@wPlanNo,NOW();", wInstance);
                else if (wMSSMaterialPoint.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mss_materialpoint SET LineID=@wLineID,AssetID=@wAssetID,Name=@wName,StationPoint=@wStationPoint,DeliveryPoint=@wDeliveryPoint,MaterialNo=@wMaterialNo,PlanNo=@wPlanNo,UpdateTime=NOW() WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMSSMaterialPoint.ID);
                wParms.Add("wLineID", wMSSMaterialPoint.LineID);
                wParms.Add("wAssetID", wMSSMaterialPoint.AssetID);
                wParms.Add("wName", wMSSMaterialPoint.Name);
                wParms.Add("wStationPoint", wMSSMaterialPoint.StationPoint);
                wParms.Add("wDeliveryPoint", wMSSMaterialPoint.DeliveryPoint);
                wParms.Add("wMaterialNo", wMSSMaterialPoint.MaterialNo);
                wParms.Add("wPlanNo", wMSSMaterialPoint.PlanNo);
                wParms.Add("wUpdateTime", DateTime.Now);
                wSQLText = this.DMLChange(wSQLText);

                if (wMSSMaterialPoint.ID <= 0)
                    wMSSMaterialPoint.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("MSS_SaveMSSMaterialPoint", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MSS_DeleteMSSMaterialPointList(List<MSSMaterialPoint> wMSSMaterialPointList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMSSMaterialPointList != null && wMSSMaterialPointList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMSSMaterialPointList.Count; i++)
                    {
                        if (i == wMSSMaterialPointList.Count - 1)
                            wStringBuilder.Append(wMSSMaterialPointList[i].ID);
                        else
                            wStringBuilder.Append(wMSSMaterialPointList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mss_materialpoint WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MSS_DeleteMSSMaterialPointList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MSSMaterialPoint> MSS_QueryMSSMaterialPointList(int wID, int wLineID, int wAssetID, string wName, string wStationPoint,string wDeliveryPoint,string wMaterialNo, int wPlanNo,DateTime wUpdateTime,  Pagination wPagination, out int wErrorCode)
        {
            List<MSSMaterialPoint> wResultList = new List<MSSMaterialPoint>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name AS AssetName,t2.FrameStatus,t2.Code FROM mss_materialpoint t"
                + " left join {0}.dms_device_ledger t1 on t.AssetID=t1.ID "
                + " left join {0}.mss_materialframe t2 on t.FrameID=t2.ID "
                + " WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wLineID <=0 or t.LineID= @wLineID)"
                + " and(@wAssetID <=0 or t.AssetID= @wAssetID)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wStationPoint is null or @wStationPoint = '' or t.StationPoint= @wStationPoint)"
                + " and(@wDeliveryPoint is null or @wDeliveryPoint = '' or t.DeliveryPoint= @wDeliveryPoint)"
                + " and(@wMaterialNo is null or @wMaterialNo = '' or t.MaterialNo= @wMaterialNo)"
                + " and(@wPlanNo <=0 or t.PlanNo= @wPlanNo)"
                + " and(@wUpdateTime <= '2010-1-1' or t.UpdateTime<= @wUpdateTime)"
                + " and(@wPlanNo <=0 or t.PlanNo= @wPlanNo)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wLineID", wLineID);
                wParms.Add("wAssetID", wAssetID);
                wParms.Add("wName", wName);
                wParms.Add("wStationPoint", wStationPoint);
                wParms.Add("wDeliveryPoint", wDeliveryPoint);
                wParms.Add("wMaterialNo", wMaterialNo);
                wParms.Add("wPlanNo", wPlanNo);
                wParms.Add("wUpdateTime", wUpdateTime);

                DateTime wBaseTime = new DateTime(2000, 1, 1);
                List<FPCStructuralPart> wFPCStructuralPartList = FPCStructuralPartDAO.Instance.FPC_QueryFPCStructuralPartList(-1, "", "", -1, wBaseTime, wBaseTime, Pagination.MaxSize,"","",out wErrorCode);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSMaterialPoint wMSSMaterialPoint = new MSSMaterialPoint();
                    wMSSMaterialPoint.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSMaterialPoint.LineID = StringUtils.parseInt(wSqlDataReader["LineID"]);
                    wMSSMaterialPoint.AssetID = StringUtils.parseInt(wSqlDataReader["AssetID"]);
                    wMSSMaterialPoint.AssetName = StringUtils.parseString(wSqlDataReader["AssetName"]);
                    wMSSMaterialPoint.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSMaterialPoint.StationPoint = StringUtils.parseString(wSqlDataReader["StationPoint"]);
                    wMSSMaterialPoint.DeliveryPoint = StringUtils.parseString(wSqlDataReader["DeliveryPoint"]);
                    wMSSMaterialPoint.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                    wMSSMaterialPoint.PlanNo = StringUtils.parseInt(wSqlDataReader["PlanNo"]);
                    wMSSMaterialPoint.FrameID = StringUtils.parseInt(wSqlDataReader["FrameID"]);
                    wMSSMaterialPoint.FrameStatus = StringUtils.parseInt(wSqlDataReader["FrameStatus"]);
                    wMSSMaterialPoint.UpdateTime = StringUtils.parseDate(wSqlDataReader["UpdateTime"]);
                    //wMSSMaterialPoint.IsExistFrame = StringUtils.parseInt(wSqlDataReader["IsExistFrame"]);
                    wResultList.Add(wMSSMaterialPoint);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FMC_QueryFMCShiftList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        private string GetPlateNames(string wPlateIDs, List<FPCStructuralPart> wFPCStructuralPartList)
        {
            string wResult = "";
            try
            {
                if (string.IsNullOrEmpty(wPlateIDs))
                    return wResult;

                string[] wIDs = wPlateIDs.Split(",");
                List<string> wNames = new List<string>();
                foreach (string wID in wIDs)
                {
                    int wMyID = StringUtils.parseInt(wID);
                    if (wFPCStructuralPartList.Exists(p => p.ID == wMyID))
                        wNames.Add(wFPCStructuralPartList.Find(p => p.ID == wMyID).Name);
                }
                wResult = StringUtils.Join(",", wNames);
            }
            catch (Exception ex)
            {
                logger.Error("GetPlateNames", ex);
            }
            return wResult;
        }
    }
}

