using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Data.OleDb;
using KLTS.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class ImportBankACNo : System.Web.UI.Page
    {
        DataTable dt;
        string CmpIDPrefix = "";
        string EmpIDPrefix = "";
        string Elength = "";
        string Clength = "";

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
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        public void SampleExport()
        {

            string query = "Select top 1 '' as EmpID, '' as BankACNo,'' as BankCardRefNo,'' as BankName,'' as IFSCCode from empdetails";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            gvlistofemp.DataSource = dt;
            gvlistofemp.DataBind();

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


        protected void lnkSample_Click(object sender, EventArgs e)
        {
            GVUtil.Export("SampleBankAC.xls", this.gvlistofemp);

        }

        protected void lnkMaster_Click(object sender, EventArgs e)
        {

            string qry = "select bankid,bankname from BankNames where bankid>0";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(qry).Result;
            if (dt.Rows.Count > 0)
            {
                GVBankNames.DataSource = dt;
                GVBankNames.DataBind();
                GVBankNames.Visible = false;
                GVUtil.Export("BankNamesMaster.xls", this.GVBankNames);

            }
            else
            {
                GVBankNames.DataSource = null;
                GVBankNames.DataBind();
            }
        }

        public void NotInsertGridDisplay()
        {

            string SelectQry = "select * from NotInsertDataBankAC ";
            DataTable dtQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectQry).Result;

            if (dtQry.Rows.Count > 0)
            {
                GvNotInsertedlist.Visible = true;
                GvNotInsertedlist.DataSource = dtQry;
                GvNotInsertedlist.DataBind();
            }

        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {

            string deleteQry = "delete from NotInsertDataBankAC ";
            int status = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

            GvNotInsertedlist.DataSource = null;
            GvNotInsertedlist.DataBind();
            BtnUnSave.Visible = false;

            int result = 0;
            string ExcelSheetname = "";
            string FileName = FlUploadBankAC.FileName;
            if (FileName == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please select file to import');", true);
                return;
            }

            string path = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadBankAC.PostedFile.FileName));
            FlUploadBankAC.PostedFile.SaveAs(path);

            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;");
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();

            OleDbCommand cmd = new OleDbCommand("Select [Emp ID],[Bank AC No], [Bank Card Ref No],[Bank Name],[IFSC Code] from [" + ExcelSheetname + "]", con);
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

                string EmpID = ""; string BankACNO = ""; string BankCardRefNO = ""; string BankName = ""; string IFSCCode = "";



                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    EmpID = ds.Rows[i]["Emp ID"].ToString();
                    BankACNO = ds.Rows[i]["Bank AC No"].ToString();
                    BankCardRefNO = ds.Rows[i]["Bank Card Ref No"].ToString();
                    BankName = ds.Rows[i]["Bank Name"].ToString();
                    IFSCCode = ds.Rows[i]["IFSC Code"].ToString();
                    if (EmpID.Length > 0)
                    {

                        {



                            string SelectQry = "select empid from EmpDetails where empid='" + EmpID + "'";
                            DataTable dtQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectQry).Result;


                            string SelectAllBankAcNos = "select EmpBankAcNo,empid from EmpDetails where EmpBankAcNo='" + BankACNO + "' and EmpBankAcNo!='' ";
                            DataTable dtAllBankAcNoQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectAllBankAcNos).Result;

                            string SelectAllBankCardRefNos = "select EmpBankCardRef,empid from EmpDetails where EmpBankCardRef='" + BankCardRefNO + "' and EmpBankCardRef!='' ";
                            DataTable dtAllBankCardRefNoQry = config.ExecuteAdaptorAsyncWithQueryParams(SelectAllBankCardRefNos).Result;




                            if (dtQry.Rows.Count > 0)
                            {

                                deleteQry = "delete from NotInsertDataBankAC where empid='" + EmpID + "'";
                                result = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

                                if (result > 0)
                                {
                                    GvNotInsertedlist.DataSource = null;
                                    GvNotInsertedlist.DataBind();
                                    BtnUnSave.Visible = false;
                                }
                                if (dtAllBankAcNoQry.Rows.Count > 0)
                                {
                                    string empidE = dtAllBankAcNoQry.Rows[0]["Empid"].ToString();
                                    if (BankACNO == dtAllBankAcNoQry.Rows[0]["EmpBankACNO"].ToString() && EmpID == empidE)
                                    {
                                        string UpdatQry = "update EmpDetails set EmpBankACNo='" + BankACNO + "' where empid='" + EmpID + "'";
                                        result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;
                                        if (result > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee Data Imported Successfully');", true);

                                        }
                                    }
                                    else
                                    {
                                        string Remark = "BankACNo already exists for empid " + empidE + "";

                                        string InsertQryDuples = "insert into NotInsertDataBankAC (Empid,BankACNo,Remark) values ('" + EmpID + "','" + BankACNO + "','" + Remark + "')";
                                        result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                        BtnUnSave.Visible = true;
                                    }
                                }
                                else
                                {
                                    string UpdatQry = "update EmpDetails set EmpBankACNo='" + BankACNO + "' where empid='" + EmpID + "'";
                                    result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;
                                    if (result > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee Data Imported Successfully');", true);

                                    }
                                }

                                if (dtAllBankCardRefNoQry.Rows.Count > 0)
                                {
                                    string empidA = dtAllBankCardRefNoQry.Rows[0]["Empid"].ToString();
                                    if (BankCardRefNO == dtAllBankCardRefNoQry.Rows[0]["EmpBankCardRef"].ToString() && EmpID == empidA)
                                    {
                                        string UpdatQry = "update EmpDetails set EmpBankCardRef='" + BankCardRefNO + "' where empid='" + EmpID + "'";
                                        result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;
                                        if (result > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee Data Imported Successfully');", true);

                                        }
                                    }
                                    else
                                    {
                                        string Remark = "BankCardRefNo already exists for empid " + empidA + "";
                                        string InsertQryDuples = "insert into NotInsertDataBankAC (Empid,BankCardRefNo,Remark) values ('" + EmpID + "','" + BankCardRefNO + "','" + Remark + "')";
                                        result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                        BtnUnSave.Visible = true;
                                    }
                                }
                                else
                                {
                                    string UpdatQry = "update EmpDetails set EmpBankCardRef='" + BankCardRefNO + "' where empid='" + EmpID + "'";
                                    result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;
                                    if (result > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Employee Data Imported Successfully');", true);

                                    }

                                }

                                string UpdatQuery = "update EmpDetails set EmpIFSCcode='" + IFSCCode + "'  where empid='" + EmpID + "'";
                                result = config.ExecuteNonQueryWithQueryAsync(UpdatQuery).Result;



                                if (BankName.Length > 0)
                                {

                                    if (!Regex.IsMatch(BankName, "^[0-9]*$"))
                                    {
                                        string Remark = "Please mention Bank ID from Master Excel and Import ";
                                        string InsertQryDuples = "insert into NotInsertDataBankAC (Empid,BankName,Remark) values ('" + EmpID + "','" + BankName + "','" + Remark + "')";
                                        result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                        BtnUnSave.Visible = true;
                                    }
                                    else
                                    {
                                        UpdatQuery = "update EmpDetails set Empbankname='" + BankName + "'  where empid='" + EmpID + "'";
                                        result = config.ExecuteNonQueryWithQueryAsync(UpdatQuery).Result;

                                    }
                                }
                                else
                                {
                                    UpdatQuery = "update EmpDetails set Empbankname='" + BankName + "'  where empid='" + EmpID + "'";
                                    result = config.ExecuteNonQueryWithQueryAsync(UpdatQuery).Result;

                                }
                                //else
                                //{
                                //    UpdatQuery = "update EmpDetails set Empbankname='" + BankName + "'  where empid='" + EmpID + "'";
                                //    result = SqlHelper.Instance.ExecuteDMLQry(UpdatQuery);
                                //}

                            }

                            else
                            {
                                string Remark = "Empid " + EmpID + " doesnt exists ";

                                string InsertQryDuples = "insert into NotInsertDataBankAC (Empid,BankACNo,Remark) values ('" + EmpID + "','" + BankACNO + "','" + Remark + "')";
                                result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                BtnUnSave.Visible = true;
                            }
                        }

                        NotInsertGridDisplay();
                    }
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