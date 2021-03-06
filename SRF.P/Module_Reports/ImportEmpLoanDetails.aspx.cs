using System;
using System.Web.UI;
using KLTS.Data;
using System.Data;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using SRF.P.DAL;
using ClosedXML.Excel;

namespace SRF.P.Module_Reports
{
    public partial class ImportEmpLoanDetails : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string UserID = "";
        

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        protected void GetWebConfigdata()
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            UserID = Session["UserId"].ToString();

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            sampleGrid();

            try
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
                }
            }
            catch (Exception ex)
            {

            }

        }

      

        public void sampleGrid()
        {

            string query = "select top 1 '' as 'ID NO','' as 'Loan Type', '' as 'Amount', '' as 'NoofInstalments','' as 'LoanIssuedDate', '' as 'LoanCuttingFrom','' as 'Description' ";

            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (dt.Rows.Count > 0)
            {
                GvInputEmpLoanDetails.DataSource = dt;
                GvInputEmpLoanDetails.DataBind();
            }
            else
            {
                GvInputEmpLoanDetails.DataSource = null;
                GvInputEmpLoanDetails.DataBind();
            }
        }
        protected void LinkSample_Click(object sender, EventArgs e)
        {
            GVUtil.NewExport("SampleLoanDetailsSheet.xlsx", this.GvInputEmpLoanDetails);
        }
        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName));
            FlUploadLoanDetails.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName);
            string conStr = string.Empty;
            if (extn.ToLower() == ".xls")
            {
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (extn.ToLower() == ".xlsx")
            {
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
            }

            con = new OleDbConnection(conStr);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }
            ExcelSheetname = dt.Rows[0]["TABLE_NAME"].ToString();


            return ExcelSheetname;
        }



        protected void btnsave_Click(object sender, EventArgs e)
        {
            string Sheetname = string.Empty;
            Sheetname = FlUploadLoanDetails.PostedFile.FileName;

            string qrychk = "select top(1) * from emploanmaster where isnull(SheetName,'')='" + Sheetname + "'";
            DataTable dtchk = config.ExecuteAdaptorAsyncWithQueryParams(qrychk).Result;
            if (dtchk.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Attached excel Sheet is already uploaded');", true);
                return;
            }


            string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(FlUploadLoanDetails.PostedFile.FileName);
            FlUploadLoanDetails.PostedFile.SaveAs(filePath);

            string extn = Path.GetExtension(FlUploadLoanDetails.PostedFile.FileName);

            //Create a new DataTable.
            DataTable dtexcel = new DataTable();

            if (extn.EndsWith(".xlsx"))
            {
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);


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
                                    dtexcel.Columns.Add(cell.Value.ToString());
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
                            DataRow toInsert = dtexcel.NewRow();
                            foreach (IXLCell cell in row.Cells(1, dtexcel.Columns.Count))
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
                            dtexcel.Rows.Add(toInsert);
                        }

                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please save file in Excel WorkBook(.xlsx) format');", true);
                return;
            }

            for (int s = 0; s < dtexcel.Rows.Count; s++)
            {
                string clid = dtexcel.Rows[s][1].ToString().Trim();

                if (clid.Length == 0)
                {
                    dtexcel.Rows.RemoveAt(s);
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtexcel);

            #region Begin Getmax Id from DB
            int ExcelNo = 0;
            string selectquerycomppanyid = "select max(cast(Excel_No as int )) as Id from emploanmaster";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquerycomppanyid).Result;

            if (dt.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dt.Rows[0]["Id"].ToString()) == false)
                {
                    ExcelNo = Convert.ToInt32(dt.Rows[0]["Id"].ToString()) + 1;
                }
                else
                {
                    ExcelNo = int.Parse("1");
                }
            }
            #endregion End Getmax Id from DB

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string Empid = string.Empty;
                string Loantype = string.Empty;
                string Amount = string.Empty;
                int NoOfInstalments = 1;
                string LoanIssuedDate = "";
                string LoanCuttingMonth = "";
                int loanStatus = 0;
                string TypeOfLoan = "0";
                string Date = "";
                string Description = "";



                Empid = ds.Tables[0].Rows[i]["ID NO"].ToString();
                TypeOfLoan = ds.Tables[0].Rows[i]["Loan Type"].ToString();
                Amount = ds.Tables[0].Rows[i]["Amount"].ToString();
                NoOfInstalments = int.Parse(ds.Tables[0].Rows[i]["NoofInstalments"].ToString());
                LoanIssuedDate = ds.Tables[0].Rows[i]["LoanIssuedDate"].ToString();
                LoanCuttingMonth = ds.Tables[0].Rows[i]["LoanCuttingFrom"].ToString();
                Description = ds.Tables[0].Rows[i]["Description"].ToString();

                if (LoanIssuedDate.Length > 0)
                {
                    string db1 = Convert.ToDateTime(LoanIssuedDate).ToString("dd/MM/yyyy");
                    LoanIssuedDate = Timings.Instance.CheckDateFormat(db1);
                }


                if (LoanCuttingMonth.Length > 0)
                {
                    string db2 = Convert.ToDateTime(LoanCuttingMonth).ToString("dd/MM/yyyy");
                    LoanCuttingMonth = Timings.Instance.CheckDateFormat(db2);
                }

                Date = DateTime.Now.ToString("dd/MM/yyyy");
                Date = DateTime.Parse(Date, CultureInfo.GetCultureInfo("en-gb")).ToString();

              

                string insertquery = " insert into EmpLoanMaster(loandt,empid,loanamount,NoInstalments,  " +
              " LoanStatus,TypeOfLoan,LoanIssuedDate,Created_By,Created_On,Excel_No,LoanType,Sheetname) values( '" + LoanCuttingMonth + "', '" + Empid + "', '" + Amount + "','" + NoOfInstalments + "' ,'" + loanStatus + "' ,'" + TypeOfLoan + "' ,'" + LoanIssuedDate + "' ,'" + UserID + "' ,'" + Date + "' ,'" + ExcelNo + "' ,'" + Description + "' ,'"+Sheetname+"')";
                int status = config.ExecuteNonQueryWithQueryAsync(insertquery).Result;
                if (status != 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('New Loans Generated Successfuly');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Loan not Generated for '" + Empid + "'');", true);
                }
                

            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }
}