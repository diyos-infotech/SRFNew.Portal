using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ImportPFNos : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {

            GetWebConfigdata();

            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {
                   
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

                string ImagesFolderPath = Server.MapPath("ImportDocuments");
                string[] filePaths = Directory.GetFiles(ImagesFolderPath);

                foreach (string file in filePaths)
                {
                    File.Delete(file);
                }

                SampleExport();

            }

        }


        bool EmpStatus = false;

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }


        private void BindData(string SqlQuery)
        {

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQuery).Result;
            if (dt.Rows.Count > 0)
            {
                gvlistofemp.DataSource = dt;
                gvlistofemp.DataBind();
            }
            else
            {


                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The details Are Not Available What you are searching...');", true);
            }

        }

        protected void Cleardata()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();
        }

        public void SampleExport()
        {
            string query = "Select top 1 '' as EmpID, '' as PFNo from empepfcodes";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            gvlistofemp.DataSource = dt;
            gvlistofemp.DataBind();
        }

        protected void lnkSample_Click(object sender, EventArgs e)
        {
            GVUtil.Export("SamplePF.xls", this.gvlistofemp);

        }

        public void NotInsertGridDisplay()
        {

            string SelectQry = "select * from NotInsertDataPF";
            DataTable dtQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectQry).Result;

            if (dtQry.Rows.Count > 0)
            {
                GvNotInsertedlist.Visible = true;
                GvNotInsertedlist.DataSource = dtQry;
                GvNotInsertedlist.DataBind();
            }

        }

        DataTable dt = new DataTable();

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string deleteQry = "";
            deleteQry = "delete from NotInsertDataPF ";
            int STATUS = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

            GvNotInsertedlist.DataSource = null;
            GvNotInsertedlist.DataBind();
            BtnUnSave.Visible = false;


            int result = 0;
            string ExcelSheetname = "";
            string FileName = FlUploadPF.FileName;
            string path = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadPF.PostedFile.FileName));
            FlUploadPF.PostedFile.SaveAs(path);

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;");
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();

            OleDbCommand cmd = new OleDbCommand("Select [Emp ID],[PF No] from [" + ExcelSheetname + "]", con);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            da.Dispose();
            con.Close();
            con.Dispose();
            GC.Collect();
            using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString))
            {
                sqlcon.Open();

                string EmpID = ""; string PFNO = "";



                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    EmpID = ds.Rows[i]["Emp ID"].ToString();
                    PFNO = ds.Rows[i]["PF No"].ToString();
                    if (EmpID.Length > 0)
                    {
                        string SelectQry = "select * from empepfcodes where empid='" + EmpID + "'";
                        DataTable dtQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectQry).Result;


                        string SelectAllESINos = "select * from empepfcodes where empepfno='" + PFNO + "'";
                        DataTable dtAllQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectAllESINos).Result;

                        if (dtQry.Rows.Count > 0)
                        {

                            deleteQry = "delete NotInsertDataPF where empid='" + EmpID + "'";
                            result = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

                            if (result > 0)
                            {
                                GvNotInsertedlist.DataSource = null;
                                GvNotInsertedlist.DataBind();
                                BtnUnSave.Visible = false;

                            }

                            if (dtAllQry.Rows.Count > 0)
                            {
                                string empidPF = dtAllQry.Rows[0]["Empid"].ToString();
                                if (PFNO == dtAllQry.Rows[0]["EmpEPFNO"].ToString() && EmpID == empidPF)
                                {
                                    string UpdatQry = "update EMPEPFCodes set EmpEPFNo='" + PFNO + "' where empid='" + EmpID + "'";
                                    result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;

                                    if (result > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee PF NOs Updated Successfully');", true);
                                    }
                                }
                                else
                                {
                                    string Remark = "PFNo already exists for empid " + empidPF + "";

                                    string InsertQryDuples = "insert into NotInsertDataPF (Empid,PFNo,Remark) values ('" + EmpID + "','" + PFNO + "','" + Remark + "')";
                                    result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;

                                    BtnUnSave.Visible = true;
                                }
                            }
                            else
                            {
                                string UpdatQry = "update EMPEPFCodes set EmpEPFNo='" + PFNO + "' where empid='" + EmpID + "'";
                                result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;

                                if (result > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee PF NOs Updated Successfully');", true);

                                }
                            }
                        }
                        else
                        {

                            string Remark = "Empid " + EmpID + " doesnt exists ";

                            string InsertQryDuples = "insert into NotInsertDataPF (Empid,PFNo,Remark) values ('" + EmpID + "','" + PFNO + "','" + Remark + "')";
                            result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                            BtnUnSave.Visible = true;
                        }
                    }

                    NotInsertGridDisplay();
                }
            }
        }
        protected void BtnUnSave_Click(object sender, EventArgs e)
        {
            if (GvNotInsertedlist.Rows.Count > 0)
            {
                GVUtil.Export("UnSavedPFData.xls", this.GvNotInsertedlist);
            }
        }
    }
}