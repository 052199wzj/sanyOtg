using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Common.Tools;
using iPlant.FMS.Models;

namespace iPlant.FMC.Service
{
    public class FPCStructuralPartDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FMCSchedulingItemDAO));

        #region 单实例
        private FPCStructuralPartDAO() { }
        private static FPCStructuralPartDAO _Instance;

        public static FPCStructuralPartDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCStructuralPartDAO();
                return FPCStructuralPartDAO._Instance;
            }
        }
        #endregion

        public int FPC_SaveFPCStructuralPart(FPCStructuralPart wFPCStructuralPart, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFPCStructuralPart.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fpc_structuralpart(Name,Code,Length,Width,Height,Weight,Remark,Active,CreateID,CreateTime,EditID,EditTime,MaterialNo,MaterialTypeNo) VALUES(@wName,@wCode,@wLength,@wWidth,@wHeight,@wWeight,@wRemark,@wActive,@wCreateID,@wCreateTime,@wEditID,@wEditTime,@wMaterialNo,@wMaterialTypeNo);", wInstance);
                else if (wFPCStructuralPart.ID > 0)
                    wSQLText = string.Format("UPDATE {0}.fpc_structuralpart SET Name=@wName,Code=@wCode,Length=@wLength,Width=@wWidth,Height=@wHeight,Weight=@wWeight,Remark=@wRemark,Active=@wActive,CreateID=@wCreateID,CreateTime=@wCreateTime,EditID=@wEditID,EditTime=@wEditTime,MaterialNo=@wMaterialNo,MaterialTypeNo=@wMaterialTypeNo WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFPCStructuralPart.ID);
                wParms.Add("wName", wFPCStructuralPart.Name);
                wParms.Add("wCode", wFPCStructuralPart.Code);
                wParms.Add("wLength", wFPCStructuralPart.Length);
                wParms.Add("wWidth", wFPCStructuralPart.Width);
                wParms.Add("wHeight", wFPCStructuralPart.Height);
                wParms.Add("wWeight", wFPCStructuralPart.Weight);
                wParms.Add("wRemark", wFPCStructuralPart.Remark);
                wParms.Add("wActive", wFPCStructuralPart.Active);
                wParms.Add("wCreateID", wFPCStructuralPart.CreateID);
                wParms.Add("wCreateTime", wFPCStructuralPart.CreateTime);
                wParms.Add("wEditID", wFPCStructuralPart.EditID);
                wParms.Add("wEditTime", wFPCStructuralPart.EditTime);
                wParms.Add("wMaterialNo", wFPCStructuralPart.MaterialNo);
                wParms.Add("wMaterialTypeNo", wFPCStructuralPart.MaterialTypeNo);

                wSQLText = this.DMLChange(wSQLText);

                if (wFPCStructuralPart.ID <= 0)
                    wFPCStructuralPart.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FPC_SaveFPCStructuralPart", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FPC_DeleteFPCStructuralPartList(List<FPCStructuralPart> wFPCStructuralPartList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFPCStructuralPartList != null && wFPCStructuralPartList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFPCStructuralPartList.Count; i++)
                    {
                        if (i == wFPCStructuralPartList.Count - 1)
                            wStringBuilder.Append(wFPCStructuralPartList[i].ID);
                        else
                            wStringBuilder.Append(wFPCStructuralPartList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fpc_structuralpart WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_DeleteFPCStructuralPartList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FPCStructuralPart> FPC_QueryFPCStructuralPartList(int wID, string wName, string wCode, int wActive, DateTime wStartTime, DateTime wEndTime, Pagination wPagination, String wMaterialNo, String wMaterialTypeNo, out int wErrorCode)
        {
            List<FPCStructuralPart> wResultList = new List<FPCStructuralPart>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Name Creator,t2.Name Editor FROM fpc_structuralpart t "
                    + " left join {0}.mbs_user t1 on t.CreateID=t1.ID "
                    + " left join {0}.mbs_user t2 on t.EditID=t2.ID "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wCode is null or @wCode = '' or t.Code= @wCode)"
                + " and(@wActive <=0 or t.Active= @wActive)"
                + " and(@wMaterialTypeNo <=0 or t.MaterialTypeNo= @wMaterialTypeNo)"
                + " and(@wMaterialNo is null or @wMaterialNo = '' or t.MaterialNo= @wMaterialNo)"
                + " and(@wStartTime <= '2010-1-1' or t.CreateTime>= @wStartTime)"
                + " and(@wEndTime <= '2010-1-1' or t.CreateTime<= @wEndTime)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wMaterialTypeNo", wMaterialTypeNo);
                wParms.Add("wName", wName);
                wParms.Add("wMaterialNo", wMaterialNo);
                wParms.Add("wCode", wCode);
                wParms.Add("wActive", wActive);
                wParms.Add("wStartTime", wStartTime);
                wParms.Add("wEndTime", wEndTime);
                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms, wPagination);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FPCStructuralPart wFPCStructuralPart = new FPCStructuralPart();
                    wFPCStructuralPart.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFPCStructuralPart.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wFPCStructuralPart.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wFPCStructuralPart.Length = StringUtils.parseDouble(wSqlDataReader["Length"]);
                    wFPCStructuralPart.Width = StringUtils.parseDouble(wSqlDataReader["Width"]);
                    wFPCStructuralPart.Height = StringUtils.parseDouble(wSqlDataReader["Height"]);
                    wFPCStructuralPart.Weight = StringUtils.parseDouble(wSqlDataReader["Weight"]);
                    wFPCStructuralPart.Remark = StringUtils.parseString(wSqlDataReader["Remark"]);
                    wFPCStructuralPart.Active = StringUtils.parseInt(wSqlDataReader["Active"]);
                    wFPCStructuralPart.CreateID = StringUtils.parseInt(wSqlDataReader["CreateID"]);
                    wFPCStructuralPart.Creator = StringUtils.parseString(wSqlDataReader["Creator"]);
                    wFPCStructuralPart.CreateTime = StringUtils.parseDate(wSqlDataReader["CreateTime"]);
                    wFPCStructuralPart.EditID = StringUtils.parseInt(wSqlDataReader["EditID"]);
                    wFPCStructuralPart.Editor = StringUtils.parseString(wSqlDataReader["Editor"]);
                    wFPCStructuralPart.EditTime = StringUtils.parseDate(wSqlDataReader["EditTime"]);
                    wFPCStructuralPart.MaterialNo = StringUtils.parseString(wSqlDataReader["MaterialNo"]);
                    wFPCStructuralPart.MaterialTypeNo = StringUtils.parseString(wSqlDataReader["MaterialTypeNo"]);
                    wResultList.Add(wFPCStructuralPart);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_QueryFPCStructuralPartList",
                       ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }
    }
}

