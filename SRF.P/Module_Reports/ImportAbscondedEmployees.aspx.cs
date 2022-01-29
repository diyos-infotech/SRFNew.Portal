using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using SRF.P.DAL;
using KLTS.Data;
using ClosedXML.Excel;

namespace SRF.P.Module_Reports
{
    public partial class ImportAbscondedEmployees : System.Web.UI.Page
    {
        DataTable dt;
        
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                }
                else
                {
                    Response.Redirect("login.aspx");
                }

              

                SampleExport();

            }

        }


        bool EmpStatus = false;

        



        protected void Cleardata()
        {
            gvlistofemp.DataSource = null;
            gvlistofemp.DataBind();


        }

        public void SampleExport()
        {

            string query = "Select top 1 '' as EmpID,'' as EmpAbscondingDate,''  from EmpDetails";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            gvlistofemp.DataSource = dt;
            gvlistofemp.DataBind();

        }

        protected void lnkSample_Click(object sender, EventArgs e)
        {
            GVUtil.NewExport("SampleDtofLeaving.xlsx", this.gvlistofemp);

        }

        public void NotInsertGridDisplay()
        {

            string SelectQry = "select * from NotInsertAbsconddata";
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
            string deleteQry = "";
            deleteQry = "delete from NotInsertAbsconddata ";
            int status = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

            GvNotInsertedlist.DataSource = null;
            GvNotInsertedlist.DataBind();
            BtnUnSave.Visible = false;

            //int result = 0;
            //string ExcelSheetname = "";
            //string FileName = FlUploadDtofLeaving.FileName;
            //string path = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadDtofLeaving.PostedFile.FileName));
            //FlUploadDtofLeaving.PostedFile.SaveAs(path);

            //OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;");
            //con.Open();
            //dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();

            //OleDbCommand cmd = new OleDbCommand("Select [Emp ID],[Date of Absconding(yyyy/MM/dd)] from [" + ExcelSheetname + "]", con);
            //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            //DataTable ds = new DataTable();
            //da.Fill(ds);
            //da.Dispose();
            //con.Close();
            //con.Dispose();
            //GC.Collect();


            string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(FlUploadDtofLeaving.PostedFile.FileName);
            FlUploadDtofLeaving.PostedFile.SaveAs(filePath);

            string extn = Path.GetExtension(FlUploadDtofLeaving.PostedFile.FileName);

            //Create a new DataTable.
            DataTable ds = new DataTable();

            if (extn.EndsWith(".xlsx"))
            {
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.

                    int lastrow = workSheet.LastRowUsed().RowNumber();
                    var rows = workSheet.Rows(1, lastrow);

                    //Create a new DataTable.

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in rows)
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                                {
                                    ds.Columns.Add(cell.Value.ToString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            firstRow = false;
                        }
                        else
                        {
                            int i = 0;
                            DataRow toInsert = ds.NewRow();
                            foreach (IXLCell cell in row.Cells(1, ds.Columns.Count))
                            {
                                try
                                {
                                    toInsert[i] = cell.Value.ToString();
                                }
                                catch (Exception ex)
                                {

                                }
                                i++;
                            }
                            ds.Rows.Add(toInsert);
                        }

                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please save file in Excel WorkBook(.xlsx) format');", true);
                return;
            }

            for (int s = 0; s < ds.Rows.Count; s++)
            {
                string clid = ds.Rows[s][1].ToString().Trim();

                if (clid.Length == 0)
                {
                    ds.Rows.RemoveAt(s);
                }
            }


            int result = 0;

            using (SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ConnectionString))
            {
                sqlcon.Open();

                string EmpID = ""; string AbscondDate = ""; int EmpStatus = 0;


                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    EmpID = ds.Rows[i]["Emp ID"].ToString();
                    AbscondDate = ds.Rows[i]["Date of Absconding(yyyy/MM/dd)"].ToString();

                    if (AbscondDate.Length > 0)
                    {
                        string db1 = Convert.ToDateTime(AbscondDate).ToString("dd/MM/yyyy");
                        AbscondDate = Timings.Instance.CheckDateFormat(db1);
                    }

                    if (EmpID.Length > 0)
                    {
                        string QryCheck = "select empid,empstatus from empdetails where empid='" + EmpID + "'";
                        DataTable dtQryCheck = config.ExecuteAdaptorAsyncWithQueryParams(QryCheck).Result;

                        if (dtQryCheck.Rows.Count > 0)
                        {

                            deleteQry = "delete from NotInsertAbsconddata where empid='" + EmpID + "'";
                            result = config.ExecuteNonQueryWithQueryAsync(deleteQry).Result;

                            if (result > 0)
                            {
                                GvNotInsertedlist.DataSource = null;
                                GvNotInsertedlist.DataBind();
                                BtnUnSave.Visible = false;
                            }


                            EmpStatus = Convert.ToInt32(dtQryCheck.Rows[0]["EmpStatus"].ToString());

                            if (EmpStatus == 1)
                            {
                                string UpdatQry = "update empdetails set EmpDateofAbsconding='" + AbscondDate + "', Empstatus='2' where empid='" + EmpID + "'";
                                result = config.ExecuteNonQueryWithQueryAsync(UpdatQry).Result;
                                if (result > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Data Updated Successfully');", true);

                                }

                            }

                            else if (EmpStatus == 0)
                            {
                                string Remark = "This Employee is Already in Inactive State ";

                                string InsertQryDuples = "insert into NotInsertAbsconddata (Empid,Remark) values ('" + EmpID + "','" + Remark + "')";
                                result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                BtnUnSave.Visible = true;

                            }

                            else if (EmpStatus == 2)
                            {
                                string Remark = "This Employee is Already in Absconding State ";

                                string InsertQryDuples = "insert into NotInsertAbsconddata (Empid,Remark) values ('" + EmpID + "','" + Remark + "')";
                                result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                                BtnUnSave.Visible = true;

                            }
                        }
                        else
                        {
                            string Remark = "Emp Id  does not exist ";

                            string InsertQryDuples = "insert into NotInsertAbsconddata (Empid,Remark) values ('" + EmpID + "','" + Remark + "')";
                            result = config.ExecuteNonQueryWithQueryAsync(InsertQryDuples).Result;
                            BtnUnSave.Visible = true;
                        }

                    }

                    NotInsertGridDisplay();
                }
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }


        protected void BtnUnSave_Click(object sender, EventArgs e)
        {
            if (GvNotInsertedlist.Rows.Count > 0)
            {

                GVUtil.NewExport("UnSavedAbscondedEmployees.xlsx", this.GvNotInsertedlist);
            }
        }
    }
}