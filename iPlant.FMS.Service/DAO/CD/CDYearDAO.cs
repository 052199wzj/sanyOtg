using iPlant.Common.Tools;
using iPlant.FMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPlant.Data.EF;

namespace iPlant.FMC.Service
{
    class CDYearDAO : BaseDAO
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CDYearDAO));
        private static CDYearDAO Instance = null;

        private CDYearDAO() : base()
        {

        }

        public static CDYearDAO getInstance()
        {

            if (Instance == null)
                Instance = new CDYearDAO();
            return Instance;
        }

        private static int RoleManageEnable = StringUtils
                .parseInt(GlobalConstant.GlobalConfiguration.GetValue("Role.Manager.Enable"));
        

        public AnnualIndicators CDZC_SearchDate(OutResult<Int32> wErrorCode)
        {
            wErrorCode.set(0);
            List<AnnualIndicators> wAnnualIndicatorsList = new List<AnnualIndicators>();

            try
            {
                String wSQLText = StringUtils.Format(
                        "select * FROM {0}.mbs_annualindicators ",
                        MESDBSource.Basic.getDBName());
                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                //wParms.Add("Active", wActive);
                //wParms.Add("ID", wID);
                //wParms.Add("Name", StringUtils.isEmpty(wName) ? wName : "%" + wName + "%");
                //wParms.Add("Code", wCode);
                //wParms.Add("UserID", wUserID);
                //wParms.Add("DepartmentID", wDepartmentID);

                List<Dictionary<String, Object>> wQueryResultList = base.mDBPool.queryForList(wSQLText, wParms);
                foreach (Dictionary<String, Object> wSqlDataReader in wQueryResultList)
                {
                    AnnualIndicators wRole = new AnnualIndicators();
                    wRole.ID = StringUtils.parseInt(wSqlDataReader["ID"]);
                    wRole.DAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["DAnnualIndicators"]);
                    wRole.TRQAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["TRQAnnualIndicators"]);
                    wRole.SAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["SAnnualIndicators"]);
                    wRole.HHQAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["HHQAnnualIndicators"]);
                    wRole.YSQAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["YSQAnnualIndicators"]);
                    wRole.YQAnnualIndicators = StringUtils.parseDouble(wSqlDataReader["YQAnnualIndicators"]);

                    wRole.Quality_D = StringUtils.parseDouble(wSqlDataReader["Quality_D"]);
                    wRole.Quality_TRQ = StringUtils.parseDouble(wSqlDataReader["Quality_TRQ"]);
                    wRole.Quality_S = StringUtils.parseDouble(wSqlDataReader["Quality_S"]);
                    wRole.Quality_HHQ = StringUtils.parseDouble(wSqlDataReader["Quality_HHQ"]);
                    wRole.Quality_YSQ = StringUtils.parseDouble(wSqlDataReader["Quality_YSQ"]);
                    wRole.Quality_YQ = StringUtils.parseDouble(wSqlDataReader["Quality_YQ"]);

                    wRole.Synthesis_D = StringUtils.parseDouble(wSqlDataReader["Synthesis_D"]);
                    wRole.Synthesis_TRQ = StringUtils.parseDouble(wSqlDataReader["Synthesis_TRQ"]);
                    wRole.Synthesis_S = StringUtils.parseDouble(wSqlDataReader["Synthesis_S"]);
                    wRole.Synthesis_HHQ = StringUtils.parseDouble(wSqlDataReader["Synthesis_HHQ"]);
                    wRole.Synthesis_YSQ = StringUtils.parseDouble(wSqlDataReader["Synthesis_YSQ"]);
                    wRole.Synthesis_YQ = StringUtils.parseDouble(wSqlDataReader["Synthesis_YQ"]);

                    wRole.PartsWorkShop_D = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_D"]);
                    wRole.PartsWorkShop_TRQ = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_TRQ"]);
                    wRole.PartsWorkShop_S = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_S"]);
                    wRole.PartsWorkShop_HHQ = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_HHQ"]);
                    wRole.PartsWorkShop_YSQ = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_YSQ"]);
                    wRole.PartsWorkShop_YQ = StringUtils.parseDouble(wSqlDataReader["PartsWorkShop_YQ"]);

                    wRole.BodyWorkShop_D = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_D"]);
                    wRole.BodyWorkShop_TRQ = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_TRQ"]);
                    wRole.BodyWorkShop_S = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_S"]);
                    wRole.BodyWorkShop_HHQ = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_HHQ"]);
                    wRole.BodyWorkShop_YSQ = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_YSQ"]);
                    wRole.BodyWorkShop_YQ = StringUtils.parseDouble(wSqlDataReader["BodyWorkShop_YQ"]);

                    wRole.PaintingWorkShop_D = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_D"]);
                    wRole.PaintingWorkShop_TRQ = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_TRQ"]);
                    wRole.PaintingWorkShop_S = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_S"]);
                    wRole.PaintingWorkShop_HHQ = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_HHQ"]);
                    wRole.PaintingWorkShop_YSQ = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_YSQ"]);
                    wRole.PaintingWorkShop_YQ = StringUtils.parseDouble(wSqlDataReader["PaintingWorkShop_YQ"]);

                    wRole.AssemblyWorkShop_D = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_D"]);
                    wRole.AssemblyWorkShop_TRQ = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_TRQ"]);
                    wRole.AssemblyWorkShop_S = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_S"]);
                    wRole.AssemblyWorkShop_HHQ = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_HHQ"]);
                    wRole.AssemblyWorkShop_YSQ = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_YSQ"]);
                    wRole.AssemblyWorkShop_YQ = StringUtils.parseDouble(wSqlDataReader["AssemblyWorkShop_YQ"]);

                    wRole.CheckWorkShop_D = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_D"]);
                    wRole.CheckWorkShop_TRQ = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_TRQ"]);
                    wRole.CheckWorkShop_S = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_S"]);
                    wRole.CheckWorkShop_HHQ = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_HHQ"]);
                    wRole.CheckWorkShop_YSQ = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_YSQ"]);
                    wRole.CheckWorkShop_YQ = StringUtils.parseDouble(wSqlDataReader["CheckWorkShop_YQ"]);

                    wAnnualIndicatorsList.Add(wRole);
                }

            }
            catch (Exception ex)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error("CDZC_SearchDate", ex);
            }
            if (wAnnualIndicatorsList.Count > 0)
            {
                return wAnnualIndicatorsList[0];
            }
            else { 
                return null;
            }

        }

        public void CDZC_Save(BMSEmployee wLoginUser, AnnualIndicators wAnnualIndicators,
               OutResult<Int32> wErrorCode)
        {

            try
            {
                wErrorCode.set(0);

                AnnualIndicators wAnnualIndicatorsInfo = this.CDZC_SearchDate(wErrorCode);

                String wSQLText = "";
                if (wAnnualIndicatorsInfo == null)
                {
                    wSQLText = StringUtils.Format(
                            "Insert into {0}.mbs_annualindicators (" +
                            "DAnnualIndicators,TRQAnnualIndicators,SAnnualIndicators,HHQAnnualIndicators,YSQAnnualIndicators,YQAnnualIndicators" +
                            "Quality_D,Quality_TRQ,Quality_S,Quality_HHQ,Quality_YSQ,Quality_YQ" +
                            "Synthesis_D,Synthesis_TRQ,Synthesis_S,Synthesis_HHQ,Synthesis_YSQ,Synthesis_YQ" +
                            "PartsWorkShop_D,PartsWorkShop_TRQ,PartsWorkShop_S,PartsWorkShop_HHQ,PartsWorkShop_YSQ,PartsWorkShop_YQ" +
                            "BodyWorkShop_D,BodyWorkShop_TRQ,BodyWorkShop_S,BodyWorkShop_HHQ,BodyWorkShop_YSQ,BodyWorkShop_YQ" +
                            "PaintingWorkShop_D,PaintingWorkShop_TRQ,PaintingWorkShop_S,PaintingWorkShop_HHQ,PaintingWorkShop_YSQ,PaintingWorkShop_YQ" +
                            "AssemblyWorkShop_D,AssemblyWorkShop_TRQ,AssemblyWorkShop_S,AssemblyWorkShop_HHQ,AssemblyWorkShop_YSQ,AssemblyWorkShop_YQ" +
                            "CheckWorkShop_D,CheckWorkShop_TRQ,CheckWorkShop_S,CheckWorkShop_HHQ,CheckWorkShop_YSQ,CheckWorkShop_YQ" +
                            ")" +
                            " Values (" +
                            "@DAnnualIndicators,@TRQAnnualIndicators,@SAnnualIndicators,@HHQAnnualIndicators,@YSQAnnualIndicators,@YQAnnualIndicators" +
                            "@Quality_D,@Quality_TRQ,@Quality_S,@Quality_HHQ,@Quality_YSQ,@Quality_YQ" +
                            "@Synthesis_D,@Synthesis_TRQ,@Synthesis_S,@Synthesis_HHQ,@Synthesis_YSQ,@Synthesis_YQ" +
                            "@PartsWorkShop_D,@PartsWorkShop_TRQ,@PartsWorkShop_S,@PartsWorkShop_HHQ,@PartsWorkShop_YSQ,@PartsWorkShop_YQ" +
                            "@BodyWorkShop_D,@BodyWorkShop_TRQ,@BodyWorkShop_S,@BodyWorkShop_HHQ,@BodyWorkShop_YSQ,@BodyWorkShop_YQ" +
                            "@PaintingWorkShop_D,@PaintingWorkShop_TRQ,@PaintingWorkShop_S,@PaintingWorkShop_HHQ,@PaintingWorkShop_YSQ,@PaintingWorkShop_YQ" +
                            "@AssemblyWorkShop_D,@AssemblyWorkShop_TRQ,@AssemblyWorkShop_S,@AssemblyWorkShop_HHQ,@AssemblyWorkShop_YSQ,@AssemblyWorkShop_YQ" +
                            "@CheckWorkShop_D,@CheckWorkShop_TRQ,@CheckWorkShop_S,@CheckWorkShop_HHQ,@CheckWorkShop_YSQ,@CheckWorkShop_YQ" +
                            ");",
                            MESDBSource.Basic.getDBName());
                }
                else
                {
                    wSQLText = StringUtils.Format(
                            "Update {0}.mbs_annualindicators set " +
                            "DAnnualIndicators=@DAnnualIndicators ,TRQAnnualIndicators=@TRQAnnualIndicators,SAnnualIndicators=@SAnnualIndicators,HHQAnnualIndicators=@HHQAnnualIndicators,YSQAnnualIndicators=@YSQAnnualIndicators ,YQAnnualIndicators=@YQAnnualIndicators ," +
                            "Quality_D=@Quality_D ,Quality_TRQ=@Quality_TRQ,Quality_S=@Quality_S,Quality_HHQ=@Quality_HHQ,Quality_YSQ=@Quality_YSQ,Quality_YQ=@Quality_YQ," +
                            "Synthesis_D=@Synthesis_D ,Synthesis_TRQ=@Synthesis_TRQ,Synthesis_S=@Synthesis_S,Synthesis_HHQ=@Synthesis_HHQ,Synthesis_YSQ=@Synthesis_YSQ ,Synthesis_YQ=@Synthesis_YQ, " +
                            "PartsWorkShop_D=@PartsWorkShop_D ,PartsWorkShop_TRQ=@PartsWorkShop_TRQ,PartsWorkShop_S=@PartsWorkShop_S,PartsWorkShop_HHQ=@PartsWorkShop_HHQ,PartsWorkShop_YSQ=@PartsWorkShop_YSQ ,PartsWorkShop_YQ=@PartsWorkShop_YQ ," +
                            "BodyWorkShop_D=@BodyWorkShop_D ,BodyWorkShop_TRQ=@BodyWorkShop_TRQ,BodyWorkShop_S=@BodyWorkShop_S,BodyWorkShop_HHQ=@BodyWorkShop_HHQ,BodyWorkShop_YSQ=@BodyWorkShop_YSQ ,BodyWorkShop_YQ=@BodyWorkShop_YQ, " +
                            "PaintingWorkShop_D=@PaintingWorkShop_D ,PaintingWorkShop_TRQ=@PaintingWorkShop_TRQ,PaintingWorkShop_S=@PaintingWorkShop_S,PaintingWorkShop_HHQ=@PaintingWorkShop_HHQ,PaintingWorkShop_YSQ=@PaintingWorkShop_YSQ ,PaintingWorkShop_YQ=@PaintingWorkShop_YQ, " +
                            "AssemblyWorkShop_D=@AssemblyWorkShop_D ,AssemblyWorkShop_TRQ=@AssemblyWorkShop_TRQ,AssemblyWorkShop_S=@AssemblyWorkShop_S,AssemblyWorkShop_HHQ=@AssemblyWorkShop_HHQ,AssemblyWorkShop_YSQ=@AssemblyWorkShop_YSQ ,AssemblyWorkShop_YQ=@AssemblyWorkShop_YQ, " +
                            "CheckWorkShop_D=@CheckWorkShop_D ,CheckWorkShop_TRQ=@CheckWorkShop_TRQ,CheckWorkShop_S=@CheckWorkShop_S,CheckWorkShop_HHQ=@CheckWorkShop_HHQ,CheckWorkShop_YSQ=@CheckWorkShop_YSQ ,CheckWorkShop_YQ=@CheckWorkShop_YQ " +
                            "Where ID=@ID ;",
                            MESDBSource.Basic.getDBName());
                }

                Dictionary<String, Object> wParms = new Dictionary<String, Object>();
                if (wAnnualIndicatorsInfo != null) {
                    wParms.Add("ID", wAnnualIndicators.ID);
                }
                wParms.Add("DAnnualIndicators", wAnnualIndicators.DAnnualIndicators);
                wParms.Add("TRQAnnualIndicators", wAnnualIndicators.TRQAnnualIndicators);
                wParms.Add("SAnnualIndicators", wAnnualIndicators.SAnnualIndicators);
                wParms.Add("HHQAnnualIndicators", wAnnualIndicators.HHQAnnualIndicators);
                wParms.Add("YSQAnnualIndicators", wAnnualIndicators.YSQAnnualIndicators);
                wParms.Add("YQAnnualIndicators", wAnnualIndicators.YQAnnualIndicators);

                wParms.Add("Quality_D", wAnnualIndicators.Quality_D);
                wParms.Add("Quality_TRQ", wAnnualIndicators.Quality_TRQ);
                wParms.Add("Quality_S", wAnnualIndicators.Quality_S);
                wParms.Add("Quality_HHQ", wAnnualIndicators.Quality_HHQ);
                wParms.Add("Quality_YSQ", wAnnualIndicators.Quality_YSQ);
                wParms.Add("Quality_YQ", wAnnualIndicators.Quality_YQ);

                wParms.Add("Synthesis_D", wAnnualIndicators.Synthesis_D);
                wParms.Add("Synthesis_TRQ", wAnnualIndicators.Synthesis_TRQ);
                wParms.Add("Synthesis_S", wAnnualIndicators.Synthesis_S);
                wParms.Add("Synthesis_HHQ", wAnnualIndicators.Synthesis_HHQ);
                wParms.Add("Synthesis_YSQ", wAnnualIndicators.Synthesis_YSQ);
                wParms.Add("Synthesis_YQ", wAnnualIndicators.Synthesis_YQ);

                wParms.Add("PartsWorkShop_D", wAnnualIndicators.PartsWorkShop_D);
                wParms.Add("PartsWorkShop_TRQ", wAnnualIndicators.PartsWorkShop_TRQ);
                wParms.Add("PartsWorkShop_S", wAnnualIndicators.PartsWorkShop_S);
                wParms.Add("PartsWorkShop_HHQ", wAnnualIndicators.PartsWorkShop_HHQ);
                wParms.Add("PartsWorkShop_YSQ", wAnnualIndicators.PartsWorkShop_YSQ);
                wParms.Add("PartsWorkShop_YQ", wAnnualIndicators.PartsWorkShop_YQ);

                wParms.Add("BodyWorkShop_D", wAnnualIndicators.BodyWorkShop_D);
                wParms.Add("BodyWorkShop_TRQ", wAnnualIndicators.BodyWorkShop_TRQ);
                wParms.Add("BodyWorkShop_S", wAnnualIndicators.BodyWorkShop_S);
                wParms.Add("BodyWorkShop_HHQ", wAnnualIndicators.BodyWorkShop_HHQ);
                wParms.Add("BodyWorkShop_YSQ", wAnnualIndicators.BodyWorkShop_YSQ);
                wParms.Add("BodyWorkShop_YQ", wAnnualIndicators.BodyWorkShop_YQ);

                wParms.Add("PaintingWorkShop_D", wAnnualIndicators.PaintingWorkShop_D);
                wParms.Add("PaintingWorkShop_TRQ", wAnnualIndicators.PaintingWorkShop_TRQ);
                wParms.Add("PaintingWorkShop_S", wAnnualIndicators.PaintingWorkShop_S);
                wParms.Add("PaintingWorkShop_HHQ", wAnnualIndicators.PaintingWorkShop_HHQ);
                wParms.Add("PaintingWorkShop_YSQ", wAnnualIndicators.PaintingWorkShop_YSQ);
                wParms.Add("PaintingWorkShop_YQ", wAnnualIndicators.PaintingWorkShop_YQ);

                wParms.Add("AssemblyWorkShop_D", wAnnualIndicators.AssemblyWorkShop_D);
                wParms.Add("AssemblyWorkShop_TRQ", wAnnualIndicators.AssemblyWorkShop_TRQ);
                wParms.Add("AssemblyWorkShop_S", wAnnualIndicators.AssemblyWorkShop_S);
                wParms.Add("AssemblyWorkShop_HHQ", wAnnualIndicators.AssemblyWorkShop_HHQ);
                wParms.Add("AssemblyWorkShop_YSQ", wAnnualIndicators.AssemblyWorkShop_YSQ);
                wParms.Add("AssemblyWorkShop_YQ", wAnnualIndicators.AssemblyWorkShop_YQ);

                wParms.Add("CheckWorkShop_D", wAnnualIndicators.CheckWorkShop_D);
                wParms.Add("CheckWorkShop_TRQ", wAnnualIndicators.CheckWorkShop_TRQ);
                wParms.Add("CheckWorkShop_S", wAnnualIndicators.CheckWorkShop_S);
                wParms.Add("CheckWorkShop_HHQ", wAnnualIndicators.CheckWorkShop_HHQ);
                wParms.Add("CheckWorkShop_YSQ", wAnnualIndicators.CheckWorkShop_YSQ);
                wParms.Add("CheckWorkShop_YQ", wAnnualIndicators.CheckWorkShop_YQ);

                base.mDBPool.update(wSQLText, wParms);

            }
            catch (Exception ex)
            {
                wErrorCode.set(MESException.DBSQL.Value);
                logger.Error("CDZC_Save", ex);
            }
        }

    }
}
