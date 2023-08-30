using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class MSSCallMaterialDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MSSCallMaterialDAO));

        #region 单实例
        private MSSCallMaterialDAO() { }
        private static MSSCallMaterialDAO _Instance;

        public static MSSCallMaterialDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MSSCallMaterialDAO();
                return MSSCallMaterialDAO._Instance;
            }
        }
        #endregion

        public int MSS_SaveMSSCallMaterial(MSSCallMaterial wMSSCallMaterial, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wMSSCallMaterial.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.mss_callmaterial(Code,Name,Remark,Active,CreateID,CreateTime,EditID,EditTime,PlateID,DemandNumber,MaterialPointID,ArriveTime,ArriveNumber,ConfirmID,ConfirmTime,Status,Type,StartTime,EndTime) VALUES(@wCode,@wName,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wPlateID,@wDemandNumber,@wMaterialPointID,@wArriveTime,@wArriveNumber,@wConfirmID,@wConfirmTime,@wStatus,@wType,@wStartTime,@wEndTime);", wInstance);
                else if (wMSSCallMaterial.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.mss_callmaterial SET Code=@wCode,Name=@wName,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,PlateID=@wPlateID,DemandNumber=@wDemandNumber,MaterialPointID=@wMaterialPointID,ArriveTime=@wArriveTime,ArriveNumber=@wArriveNumber,ConfirmID=@wConfirmID,ConfirmTime=@wConfirmTime,Status=@wStatus,Type=@wType,StartTime=@wStartTime,EndTime=@wEndTime WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wMSSCallMaterial.ID);
                wParms.Add("wCode", wMSSCallMaterial.Code);
                wParms.Add("wName", wMSSCallMaterial.Name);
                wParms.Add("wRemark", wMSSCallMaterial.Remark);
                wParms.Add("wActive", wMSSCallMaterial.Active);
                wParms.Add("wCreateID", wMSSCallMaterial.CreateID);
                wParms.Add("wCreateTime", wMSSCallMaterial.CreateTime);
                wParms.Add("wEditID", wMSSCallMaterial.EditID);
                wParms.Add("wEditTime", wMSSCallMaterial.EditTime);
                wParms.Add("wPlateID", wMSSCallMaterial.PlateID);
                wParms.Add("wDemandNumber", wMSSCallMaterial.DemandNumber);
                wParms.Add("wMaterialPointID", wMSSCallMaterial.MaterialPointID);
                wParms.Add("wArriveTime", wMSSCallMaterial.ArriveTime);
                wParms.Add("wArriveNumber", wMSSCallMaterial.ArriveNumber);
                wParms.Add("wConfirmID", wMSSCallMaterial.ConfirmID);
                wParms.Add("wConfirmTime", wMSSCallMaterial.ConfirmTime);
                wParms.Add("wStatus", wMSSCallMaterial.Status);
                wParms.Add("wType", wMSSCallMaterial.Type);
                wParms.Add("wStartTime", wMSSCallMaterial.StartTime);
                wParms.Add("wEndTime", wMSSCallMaterial.EndTime);

                wSQLText = this.DMLChange(wSQLText);

                if (wMSSCallMaterial.ID <= 0)
                    wMSSCallMaterial.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("MSS_SaveMSSCallMaterial", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int MSS_DeleteMSSCallMaterialList(List<MSSCallMaterial> wMSSCallMaterialList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wMSSCallMaterialList != null && wMSSCallMaterialList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wMSSCallMaterialList.Count; i++)
                    {
                        if (i == wMSSCallMaterialList.Count - 1)
                            wStringBuilder.Append(wMSSCallMaterialList[i].ID);
                        else
                            wStringBuilder.Append(wMSSCallMaterialList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.mss_callmaterial WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MSS_DeleteMSSCallMaterialList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<MSSCallMaterial> MSS_QueryMSSCallMaterialList(int wID, string wCode, string wName, int wActive, int wPlateID, int wMaterialPointID, int wStatus, int wType, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, out int wErrorCode)
        {
            List<MSSCallMaterial> wResultList = new List<MSSCallMaterial>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor,t3.Name Confirmer,t4.Name PlateName,t5.Name MaterialPointName FROM {0}.mss_callmaterial t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + " left join {0}.mbs_user t3 on t.ConfirmID=t3.ID "
                    + " left join {0}.fpc_structuralpart t4 on t.PlateID=t4.ID "
                    + " left join {0}.mss_materialpoint t5 on t.MaterialPointID=t5.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wPlateID <=0 or t.PlateID= @wPlateID)"
                + " and(@wMaterialPointID <=0 or t.MaterialPointID= @wMaterialPointID)"
                + " and(@wStatus <=0 or t.Status= @wStatus)"
                + " and(@wType <=0 or t.Type= @wType)"
                + " and(@wStartTime <= '2010-1-1' or t.EditTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wName", wName);
                wParms.Add("wCode", wCode);
                wParms.Add("wActive", wActive);
                wParms.Add("wPlateID", wPlateID);
                wParms.Add("wMaterialPointID", wMaterialPointID);
                wParms.Add("wStatus", wStatus);
                wParms.Add("wType", wType);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    MSSCallMaterial wMSSCallMaterial = new MSSCallMaterial();
                    wMSSCallMaterial.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wMSSCallMaterial.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wMSSCallMaterial.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wMSSCallMaterial.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wMSSCallMaterial.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wMSSCallMaterial.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wMSSCallMaterial.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wMSSCallMaterial.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wMSSCallMaterial.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wMSSCallMaterial.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wMSSCallMaterial.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wMSSCallMaterial.PlateID = StringUtils.parseInt(wSqlDataReader["PlateID"]);
                    wMSSCallMaterial.PlateName = StringUtils.parseString(wSqlDataReader["PlateName"]);
                    wMSSCallMaterial.DemandNumber = StringUtils.parseInt(wSqlDataReader["DemandNumber"]);
                    wMSSCallMaterial.MaterialPointID = StringUtils.parseInt(wSqlDataReader["MaterialPointID"]);
                    wMSSCallMaterial.MaterialPointName = StringUtils.parseString(wSqlDataReader["MaterialPointName"]);
                    wMSSCallMaterial.ArriveTime = StringUtils.parseDate(wSqlDataReader["ArriveTime"]);
                    wMSSCallMaterial.ArriveNumber = StringUtils.parseInt(wSqlDataReader["ArriveNumber"]);
                    wMSSCallMaterial.ConfirmID = StringUtils.parseInt(wSqlDataReader["ConfirmID"]);
                    wMSSCallMaterial.Confirmer = StringUtils.parseString(wSqlDataReader["Confirmer"]);
                    wMSSCallMaterial.ConfirmTime = StringUtils.parseDate(wSqlDataReader["ConfirmTime"]);
                    wMSSCallMaterial.Status = StringUtils.parseInt(wSqlDataReader["Status"]);
                    wMSSCallMaterial.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wMSSCallMaterial.StartTime = StringUtils.parseDate(wSqlDataReader["StartTime"]);
                    wMSSCallMaterial.EndTime = StringUtils.parseDate(wSqlDataReader["EndTime"]);

                    wResultList.Add(wMSSCallMaterial);
                }
            }
            catch (Exception ex)
            {
                logger.Error("MSS_QueryMSSCallMaterialList", ex);
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
                        "SELECT Code FROM {0}.mss_callmaterial WHERE id IN( SELECT MAX(ID) FROM {0}.mss_callmaterial);",
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
                wResult = StringUtils.Format("{0}{1}{2}F{3}", DateTime.Now.Year,
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

