using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlant.FMC.Service
{
    public class FPCGasVelocityDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FPCGasVelocityDAO));

        #region 单实例
        private FPCGasVelocityDAO() { }
        private static FPCGasVelocityDAO _Instance;

        public static FPCGasVelocityDAO Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new FPCGasVelocityDAO();
                return FPCGasVelocityDAO._Instance;
            }
        }
        #endregion

        public int FPC_SaveFPCGasVelocity(FPCGasVelocity wFPCGasVelocity, out int wErrorCode)
        {
            int wResult = 0;
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();

                String wSQLText = "";
                if (wFPCGasVelocity.ID == 0)
                    wSQLText = string.Format("INSERT INTO {0}.fpc_gasvelocity(Type,Thickness,Name,Description,MinSpeed,MaxSpeed) VALUES(@wType,@wThickness,@wName,@wDescription,@wMinSpeed,@wMaxSpeed);", wInstance);
                else if (wFPCGasVelocity.ID > 0)
                    wSQLText = string.Format("UPDATE fpc_gasvelocity SET Type=@wType,Thickness=@wThickness,Name=@wName,Description=@wDescription,MinSpeed=@wMinSpeed,MaxSpeed=@wMaxSpeed WHERE ID=@wID", wInstance);

                wParms.Clear();
                wParms.Add("wID", wFPCGasVelocity.ID);
                wParms.Add("wType", wFPCGasVelocity.Type);
                wParms.Add("wThickness", wFPCGasVelocity.Thickness);
                wParms.Add("wName", wFPCGasVelocity.Name);
                wParms.Add("wDescription", wFPCGasVelocity.Description);
                wParms.Add("wMinSpeed", wFPCGasVelocity.MinSpeed);
                wParms.Add("wMaxSpeed", wFPCGasVelocity.MaxSpeed);

                wSQLText = this.DMLChange(wSQLText);

                if (wFPCGasVelocity.ID <= 0)
                    wFPCGasVelocity.ID = (int)mDBPool.insert(wSQLText, wParms);
                else
                    mDBPool.update(wSQLText, wParms);
            }
            catch (Exception ex)
            {
                logger.Error("FPC_SaveFPCGasVelocity", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }

        public int FPC_DeleteFPCGasVelocityList(List<FPCGasVelocity> wFPCGasVelocityList)
        {
            int wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                if (wFPCGasVelocityList != null && wFPCGasVelocityList.Count > 0)
                {
                    StringBuilder wStringBuilder = new StringBuilder();
                    for (int i = 0; i < wFPCGasVelocityList.Count; i++)
                    {
                        if (i == wFPCGasVelocityList.Count - 1)
                            wStringBuilder.Append(wFPCGasVelocityList[i].ID);
                        else
                            wStringBuilder.Append(wFPCGasVelocityList[i].ID + ",");
                    }
                    String wSQLText = string.Format("DELETE From {1}.fpc_gasvelocity WHERE ID in({0});", wStringBuilder.ToString(), wInstance);
                    Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                    mDBPool.update(wSQLText, wParms);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_DeleteFPCGasVelocityList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wErrorCode;
        }

        public List<FPCGasVelocity> FPC_QueryFPCGasVelocityList(int wID, int wType, double wThickness, string wName, string wDescription, out int wErrorCode)
        {
            List<FPCGasVelocity> wResultList = new List<FPCGasVelocity>();
            wErrorCode = 0;
            try
            {
                String wInstance = iPlant.Data.EF.MESDBSource.Basic.getDBName();

                string wSQLText = string.Format("SELECT t.*,t1.Code FROM {0}.fpc_gasvelocity t"
                     + " left join {0}.fmc_datadictionary t1 on t.Name=t1.Name "
                    + "WHERE 1=1"
                + " and(@wID <=0 or t.ID= @wID)"
                + " and(@wType <=0 or t.Type= @wType)"
                + " and(@wThickness <=0 or t.Thickness= @wThickness)"
                + " and(@wName is null or @wName = '' or t.Name= @wName)"
                + " and(@wDescription is null or @wDescription = '' or t.Description= @wDescription)", wInstance);

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                wParms.Add("wID", wID);
                wParms.Add("wType", wType);
                wParms.Add("wThickness", wThickness);
                wParms.Add("wName", wName);
                wParms.Add("wDescription", wDescription);

                List<Dictionary<String, Object>> wQueryResultList = mDBPool.queryForList(wSQLText, wParms);

                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    FPCGasVelocity wFPCGasVelocity = new FPCGasVelocity();
                    wFPCGasVelocity.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wFPCGasVelocity.Type = StringUtils.parseInt(wSqlDataReader["Type"]);
                    wFPCGasVelocity.Thickness = StringUtils.parseDouble(wSqlDataReader["Thickness"]);
                    wFPCGasVelocity.Name = StringUtils.parseString(wSqlDataReader["Name"]);
                    wFPCGasVelocity.Code = StringUtils.parseString(wSqlDataReader["Code"]);
                    wFPCGasVelocity.Description = StringUtils.parseString(wSqlDataReader["Description"]);
                    wFPCGasVelocity.MinSpeed = StringUtils.parseDouble(wSqlDataReader["MinSpeed"]);
                    wFPCGasVelocity.MaxSpeed = StringUtils.parseDouble(wSqlDataReader["MaxSpeed"]);

                    wResultList.Add(wFPCGasVelocity);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_QueryFPCGasVelocityList", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResultList;
        }

        internal List<FPCGasVelocity> FPC_ImportGasVelocity(List<Dictionary<string, object>> wDt, out int wErrorCode)
        {
            List<FPCGasVelocity> wResult = new List<FPCGasVelocity>();
            wErrorCode = 0;
            try
            {
                //string[] CusTypeArray = new string[] { "", "火焰切割机", "平面切割机", "坡口切割机" };

                List<string> CusTypeArray = new List<string>() { "", "火焰切割机", "平面切割机", "坡口切割机" };
               
                foreach (Dictionary<string, object> wDic in wDt)
                {
                    string wID = wDic["主键ID"].ToString();
                    string wCusType = wDic["切割类型"].ToString();
                    string wThickness = wDic["厚度(mm)"].ToString();
                    string wName = wDic["名称"].ToString();
                    string wDescription = wDic["气体描述"].ToString();
                    string wMinSpeed = wDic["下限值"].ToString();
                    string wMaxSpeed = wDic["上限值"].ToString();

                    int wCusTypeInt = 0;
                    if (wCusType == "火焰切割机") {
                        wCusTypeInt = 1;
                    } else if (wCusType == "平面切割机") {
                        wCusTypeInt = 2;
                    }else if (wCusType == "坡口切割机")
                    {
                        wCusTypeInt = 3;
                    }

                    FPCGasVelocity wFPCGasVelocity = new FPCGasVelocity();
                    wFPCGasVelocity.ID = StringUtils.parseInt(wID);
                    wFPCGasVelocity.Description = wDescription;
                    wFPCGasVelocity.Name = wName;
                    wFPCGasVelocity.Thickness = StringUtils.parseDouble(wThickness);
                    wFPCGasVelocity.MinSpeed = StringUtils.parseDouble(wMinSpeed);
                    wFPCGasVelocity.MaxSpeed = StringUtils.parseDouble(wMaxSpeed);
                    wFPCGasVelocity.Type = StringUtils.parseInt(wCusTypeInt);
                    FPCGasVelocityDAO.Instance.FPC_SaveFPCGasVelocity(wFPCGasVelocity, out wErrorCode);
                    wResult.Add(wFPCGasVelocity);
                }
            }
            catch (Exception ex)
            {
                logger.Error("FPC_ImportGasVelocity", ex);
                wErrorCode = MESException.DBSQL.Value;
            }
            return wResult;
        }
    }
}

