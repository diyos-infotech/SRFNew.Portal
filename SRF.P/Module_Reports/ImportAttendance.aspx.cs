using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Data.OleDb;
using KLTS.Data;
using SRF.P.DAL;
using ClosedXML.Excel;

namespace SRF.P.Module_Reports
{
    public partial class ImportAttendance : System.Web.UI.Page
    {
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        GridViewExportUtil util = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();

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




                string sqlemptydata = "select * from tblEmptyforExcel";
                DataTable dtempty = config.ExecuteAdaptorAsyncWithQueryParams(sqlemptydata).Result;
                if (dtempty.Rows.Count > 0)
                {
                    SampleGrid.DataSource = dtempty;
                    SampleGrid.DataBind();

                    grvSample2.DataSource = dtempty;
                    grvSample2.DataBind();
                }


            }
        }



        protected void GetWebConfigdata()
        {
            if (Session.Keys.Count > 0)
            {
                EmpIDPrefix = Session["EmpIDPrefix"].ToString();
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void FillClientList()
        {
            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix);
           

            if (dt.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "clientid";
                ddlClientID.DataTextField = "clientid";
                ddlClientID.DataSource = dt;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "--Select--");

        }

        protected void FillClientNameList()
        {

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix);

            if (dt.Rows.Count > 0)
            {
                ddlCName.DataValueField = "clientid";
                ddlCName.DataTextField = "Clientname";
                ddlCName.DataSource = dt;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "--Select--");

        }



        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtmonth.Text = "";

                if (ddlClientID.SelectedIndex > 0)
                {

                    ddlCName.SelectedValue = ddlClientID.SelectedValue;
                    GetAttSummary("");
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtmonth.Text = "";

                if (ddlCName.SelectedIndex > 0)
                {
                    ddlClientID.SelectedValue = ddlCName.SelectedValue;
                    GetAttSummary("");

                }
                
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtmonth_TextChanged(object sender, EventArgs e)
        {


            if (txtmonth.Text.Trim().Length != 0)
            {
                GetAttSummary("");
            }

        }


        public string Getmonth()
        {

            string month = "";
            string Year = "";
            string date = "";

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();

                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString().Substring(2, 2);
            }

            return month + Year;
        }

        public void GetAttSummary(string ExcelNo)
        {
            gvattsummarydata.DataSource = null;
            gvattsummarydata.DataBind();


            gvnotinsert.DataSource = null;
            gvnotinsert.DataBind();

            string month = Getmonth();

            string clientd = "";

            if (ddloption.SelectedIndex == 0)
            {
                clientd = "%";
            }
            else
            {
                clientd = ddlClientID.SelectedValue;
            }

            DataTable dt = null;

            if (ExcelNo.Length > 0)
            {
                string Proc = "GetAttendanceSummary";
                Hashtable ht = new Hashtable();
                ht.Add("@ExcelNo", ExcelNo);
                ht.Add("@Type", "SuccessfulImportExcelNo");

                dt = config.ExecuteAdaptorAsyncWithParams(Proc, ht).Result;
            }
            else
            {
                string Proc = "GetAttendanceSummary";
                Hashtable ht = new Hashtable();
                ht.Add("@Clientid", clientd);
                ht.Add("@month", month);
                ht.Add("@CmpIDPrefix", CmpIDPrefix);
                ht.Add("@Type", "SuccessfulImport");

                dt = config.ExecuteAdaptorAsyncWithParams(Proc, ht).Result;


            }

            if (dt.Rows.Count > 0)
            {
                pnlAttSummary.Visible = true;
                gvattsummarydata.DataSource = dt;
                gvattsummarydata.DataBind();
            }
            else
            {
                pnlAttSummary.Visible = false;
            }

            DataTable dtn = null;

            if (ExcelNo.Length > 0)
            {
                string Procn = "GetAttendanceSummary";
                Hashtable htn = new Hashtable();
                htn.Add("@Type", "NotinsertdatabyExcelno");
                htn.Add("@ExcelNo", ExcelNo);
                dtn = config.ExecuteAdaptorAsyncWithParams(Procn, htn).Result;
            }
            else
            {
                string Procn = "GetAttendanceSummary";
                Hashtable htn = new Hashtable();
                htn.Add("@Clientid", clientd);
                htn.Add("@CmpIDPrefix", CmpIDPrefix);
                htn.Add("@Type", "Notinsertdata");
                dtn = config.ExecuteAdaptorAsyncWithParams(Procn, htn).Result;
            }

            if (dtn.Rows.Count > 0)
            {
                gvnotinsert.DataSource = dtn;
                gvnotinsert.DataBind();
                pnlnotinsertdata.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                pnlnotinsertdata.Visible = false;
                btnExport.Visible = false;
            }


        }

        public string GetExcelSheetNames()
        {
            string ExcelSheetname = "";
            OleDbConnection con = null;
            DataTable dt = null;
            string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
            fileupload1.PostedFile.SaveAs(filename);
            string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
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

        protected void btnImport_Click(object sender, EventArgs e)
        {

            int days = 0;

            btnExport.Visible = false;


            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);



            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Please Select Month');", true);
                return;
            }

            #region Begin Getmax Id from DB
            int ExcelNo = 0;
            string selectquery = "select max(cast(ExcelNumber as int )) as Id from empattendance";
            DataTable dtExcelID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dtExcelID.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(dtExcelID.Rows[0]["Id"].ToString()) == false)
                {
                    ExcelNo = Convert.ToInt32(dtExcelID.Rows[0]["Id"].ToString()) + 1;
                }
                else
                {
                    ExcelNo = int.Parse("1");
                }
            }
            #endregion End Getmax Id from DB

            #region Begin Code for when select Full Attendance as on 31/07/2014 by Venkat
            //
            if (ddlAttendanceMode.SelectedIndex == 0)
            {
                //string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                //fileupload1.PostedFile.SaveAs(filename);
                //string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                //string constring = "";
                //if (extn.ToLower() == ".xls")
                //{
                //    //constring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
                //    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                //}
                //else if (extn.ToLower() == ".xlsx")
                //{
                //    constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                //}

                //string Sheetname = string.Empty;

                ////code commented on 30-11-2015 by swathi 

                ////string qry = "select [Client Id],[Emp Id],[Designation],[Duties],[WOs],[NHS],[OTs],[OTs1],[Canteen Advance],[Penalty]," +
                ////" [Incentives],[NA],[AB] from  [" + GetExcelSheetNames() + "]" + "";


                //string qry = "select [Employee Name],[Client Id],[Emp Id],[Designation],[Duties],[OTs],[WOs],[NHS],[Leaves],[Canteen Advance],[Incentives],[Rent Ded] " +
                //"  from  [" + GetExcelSheetNames() + "]" + "";


                //OleDbConnection con = new OleDbConnection(constring);
                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();

                //}
                //OleDbCommand cmd = new OleDbCommand(qry, con);
                //OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                //da.Dispose();
                //con.Close();
                //con.Dispose();
                //GC.Collect();

                string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(fileupload1.PostedFile.FileName);
                fileupload1.PostedFile.SaveAs(filePath);

                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);

                //Create a new DataTable.
                DataTable dtexcel = new DataTable();

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

                string empid = string.Empty;
                string clientid = string.Empty;
                string design = string.Empty;
                int empstatus = 0;
                string ContractID = "";
                string DOL = "01/01/1900";
                //foreach (DataRow dr in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Remark = string.Empty;




                    #region Variables for Excel Values


                    //string Month = string.Empty;

                    float penalty = 0;
                    float incentives = 0;
                    float canteenadvance = 0;
                    float Leaves = 0;
                    float Wos = 0;
                    float NHS = 0;
                    float Npots = 0;
                    float Na = 0;
                    float Ab = 0;
                    float duties = 0;
                    float ots = 0;
                    float FoodDed = 0;
                    float RentDed = 0;
                    float SalAdv = 0;
                    float UniformDed = 0;
                    float OtherDed = 0;
                    float AdminCharges = 0;

                    float dayduties = 0;
                    float dayots = 0;
                    float daywos = 0;

                    #endregion

                    #region Variables for Posting order Table data and EmpAttendance(Default Values)

                    int orderid = 0;

                    string PrevUnitid = string.Empty;
                    string Dutyhrs = string.Empty;
                    DateTime Orderdate = DateTime.Now;
                    DateTime Joiningdate = DateTime.Now;
                    DateTime Releivingdate = DateTime.Now;
                    string IssuedAuthority = string.Empty;
                    string Remarks = string.Empty;
                    int TransferType = 1;

                    string AttString = string.Empty;
                    string HrsString = string.Empty;


                    #endregion

                    clientid = ds.Tables[0].Rows[i]["Client Id"].ToString();

                    DateTime DtLastDay = DateTime.Now;
                    DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", clientid);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        ContractID = DTContractID.Rows[0]["contractid"].ToString();
                    }
                   

                    int SNo = 0;
                    selectquery = "select max(cast(sno as int )) as sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                    DataTable dtSnoID = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                    if (dtSnoID.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dtSnoID.Rows[0]["sno"].ToString()) == false)
                        {
                            SNo = Convert.ToInt32(dtSnoID.Rows[0]["sno"].ToString()) + 1;
                        }
                        else
                        {
                            SNo = int.Parse("1");
                        }
                    }

                    var Clientstatus = 0;

                    if (ddloption.SelectedIndex == 0)
                    {
                        Clientstatus = 1;
                    }
                    else
                    {
                        if (ddlClientID.SelectedValue == clientid)
                        {
                            Clientstatus = 1;
                        }
                    }


                    if (Clientstatus == 1)
                    {
                        
                        empstatus = 0;

                        empid = ds.Tables[0].Rows[i]["Emp Id"].ToString();

                        string sqlchkempid = "select empid,convert(varchar(10),empdtofleaving,103) empdtofleaving from empdetails where empid='" + empid + "' and empstatus=1";
                        DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                        if (dtchkempid.Rows.Count > 0)
                        {
                            empstatus = 1;
                            DOL = dtchkempid.Rows[0]["empdtofleaving"].ToString();
                        }
                        else
                        {
                            empstatus = 0;
                        }

                        string RemarksText = "";

                        string Fmonth = (DtLastDay).Month.ToString();
                        string FYear = (DtLastDay).Year.ToString();

                        string DOLDate = "";
                        if (Fmonth.Length == 1)
                        {
                            DOLDate = FYear + "-0" + Fmonth + "-01";
                        }
                        else
                        {
                            DOLDate = FYear + "-" + Fmonth + "-01";
                        }

                        if (empstatus == 1)
                        {
                            string QryDOJCheck = "Select Empid from empdetails where Empid='" + empid + "' and cast(cast(FORMAT(EmpDtofJoining,'MM') as varchar)+'/'+'01/'+cast(year(empdtofjoining) as varchar) as date)<= cast('" + DOLDate + "' as date)   ";
                            DataTable dtDOJ = config.ExecuteAdaptorAsyncWithQueryParams(QryDOJCheck).Result;
                            if (dtDOJ.Rows.Count > 0)
                            {
                            }
                            else
                            {
                                empstatus = 0;
                                RemarksText = "Please check date of Joining";
                            }
                            if (DOL != "01/01/1900")
                            {
                                string QryDOLCheck = "Select Empid from empdetails where Empid='" + empid + "' and cast(cast(FORMAT(empdtofleaving,'MM') as varchar)+'/'+'01/'+cast(year(empdtofleaving) as varchar) as date)>= cast('" + DOLDate + "' as date)   ";
                                DataTable dtDOL = config.ExecuteAdaptorAsyncWithQueryParams(QryDOLCheck).Result;
                                if (dtDOL.Rows.Count > 0)
                                {
                                }
                                else
                                {
                                    empstatus = 0;
                                    RemarksText = "Please check date of Leaving";
                                }
                            }

                        }


                        //Create Procedure


                        if (empid.Length > 0)
                        {

                            design = ds.Tables[0].Rows[i]["Designation"].ToString();

                            #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                            duties = 0;
                            ots = 0;

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Duties"].ToString()) == false)
                            {
                                duties = float.Parse(ds.Tables[0].Rows[i]["Duties"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Wos"].ToString()) == false)
                            {
                                Wos = float.Parse(ds.Tables[0].Rows[i]["WOs"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["NHS"].ToString()) == false)
                            {
                                NHS = float.Parse(ds.Tables[0].Rows[i]["NHS"].ToString());
                            }


                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["OTs"].ToString()) == false)
                            {
                                ots = float.Parse(ds.Tables[0].Rows[i]["OTs"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Leaves"].ToString()) == false)
                            {
                                Leaves = float.Parse(ds.Tables[0].Rows[i]["Leaves"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Canteen Advance"].ToString()) == false)
                            {
                                canteenadvance = float.Parse(ds.Tables[0].Rows[i]["Canteen Advance"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Incentives"].ToString()) == false)
                            {
                                incentives = float.Parse(ds.Tables[0].Rows[i]["Incentives"].ToString());
                            }

                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["Rent Ded"].ToString()) == false)
                            {
                                RentDed = float.Parse(ds.Tables[0].Rows[i]["Rent Ded"].ToString());
                            }


                            #endregion


                            #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                            #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat


                            Hashtable Httable = new Hashtable();

                            Httable.Add("@empidstatus", empstatus);
                            Httable.Add("@Clientid", clientid);
                            Httable.Add("@Month", Month);
                            Httable.Add("@Empid", empid);
                            Httable.Add("@ContractId", ContractID);
                            Httable.Add("@Design", design);
                            Httable.Add("@Duties", duties);
                            Httable.Add("@Ots", ots);
                            Httable.Add("@WOs", Wos);
                            Httable.Add("@CanteenAdv", canteenadvance);
                            Httable.Add("@Leaves", Leaves);
                            Httable.Add("@Incentivs", incentives);
                            Httable.Add("@RentDed", RentDed);
                            Httable.Add("@NHS", NHS);
                            Httable.Add("@Excel_Number", ExcelNo);
                            Httable.Add("@sno", SNo);
                            Httable.Add("@RowNo", i+2);
                            Httable.Add("@RemarksText", RemarksText);


                            #endregion

                            string SPName = "ImportFullAttendanceFromExcel";
                            DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, Httable).Result;


                            #endregion



                        }

                        //End Create Procedure

                    }
                    
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

            }


            #endregion

            #region Begin Code when select Individual attendance as on 31/07/2014 by Venkat
            //
            if (ddlAttendanceMode.SelectedIndex == 1)
            {

                // string filename = Path.Combine(Server.MapPath("~/ImportDocuments"), Guid.NewGuid().ToString() + Path.GetExtension(fileupload1.PostedFile.FileName));
                // fileupload1.PostedFile.SaveAs(filename);
                // string extn = Path.GetExtension(fileupload1.PostedFile.FileName);
                // string constring = "";
                // if (extn.ToLower() == ".xls")
                // {
                //     //constring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended properties=\"excel 8.0;HDR=Yes;IMEX=2\"";
                //     constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                // }
                // else if (extn.ToLower() == ".xlsx")
                // {
                //     constring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended properties=\"excel 12.0;HDR=Yes;IMEX=2\"";
                // }

                // string Sheetname = string.Empty;

                // //code commented on 30-11-2015 by swathi 

                // //string qry = "select [Client Id],[Emp Id],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                // //" [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[Duties],[OTs],[WOs],[NHS],[Canteen Advance],[Penalty]," +
                // //" [Incentives],[NHS],[PF],[ESI],[PT],[NA],[AB] from  [" + GetExcelSheetNames() + "]" + "";


                // string qry = "select [Client Id],[Emp Id],[Designation],[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18]," +
                //" [19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31],[OTs],[Leaves],[Canteen Advance],[Incentives],[Rent Ded]" +
                //"  from  [" + GetExcelSheetNames() + "]" + "";


                // OleDbConnection con = new OleDbConnection(constring);
                // if (con.State == ConnectionState.Closed)
                // {
                //     con.Open();

                // }
                // OleDbCommand cmd = new OleDbCommand(qry, con);
                // OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                // DataSet ds = new DataSet();
                // da.Fill(ds);
                // da.Dispose();
                // con.Close();
                // con.Dispose();
                // GC.Collect();

                string filePath = Server.MapPath("~/ImportDocuments/") + Path.GetFileName(fileupload1.PostedFile.FileName);
                fileupload1.PostedFile.SaveAs(filePath);

                string extn = Path.GetExtension(fileupload1.PostedFile.FileName);

                //Create a new DataTable.
                DataTable dtexcel = new DataTable();

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

                int k = 0;
                int j = 0;

                string empid = string.Empty;
                string clientid = string.Empty;
                string design = string.Empty;
                int empstatus = 0;

                float duties = 0;
                float ots = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string Remark = string.Empty;


                    #region Variables for Excel Values


                    // string Month = string.Empty;

                    float penalty = 0;
                    float incentives = 0;
                    float canteenadvance = 0;
                    float Leaves = 0;
                    float Wos = 0;
                    float NHS = 0;
                    float Npots = 0;
                    float Na = 0;
                    float Ab = 0;
                    float FoodDed = 0;
                    float RentDed = 0;

                    float SalAdv = 0;
                    float UniformDed = 0;
                    float OtherDed = 0;
                    float AdminCharges = 0;


                    string day1 = string.Empty;
                    string day2 = string.Empty;
                    string day3 = string.Empty;
                    string day4 = string.Empty;
                    string day5 = string.Empty;
                    string day6 = string.Empty;
                    string day7 = string.Empty;
                    string day8 = string.Empty;
                    string day9 = string.Empty;
                    string day10 = string.Empty;
                    string day11 = string.Empty;
                    string day12 = string.Empty;
                    string day13 = string.Empty;
                    string day14 = string.Empty;
                    string day15 = string.Empty;
                    string day16 = string.Empty;
                    string day17 = string.Empty;
                    string day18 = string.Empty;
                    string day19 = string.Empty;
                    string day20 = string.Empty;
                    string day21 = string.Empty;
                    string day22 = string.Empty;
                    string day23 = string.Empty;
                    string day24 = string.Empty;
                    string day25 = string.Empty;
                    string day26 = string.Empty;
                    string day27 = string.Empty;
                    string day28 = string.Empty;
                    string day29 = string.Empty;
                    string day30 = string.Empty;
                    string day31 = string.Empty;

                    string day1ot = string.Empty;
                    string day2ot = string.Empty;
                    string day3ot = string.Empty;
                    string day4ot = string.Empty;
                    string day5ot = string.Empty;
                    string day6ot = string.Empty;
                    string day7ot = string.Empty;
                    string day8ot = string.Empty;
                    string day9ot = string.Empty;
                    string day10ot = string.Empty;
                    string day11ot = string.Empty;
                    string day12ot = string.Empty;
                    string day13ot = string.Empty;
                    string day14ot = string.Empty;
                    string day15ot = string.Empty;
                    string day16ot = string.Empty;
                    string day17ot = string.Empty;
                    string day18ot = string.Empty;
                    string day19ot = string.Empty;
                    string day20ot = string.Empty;
                    string day21ot = string.Empty;
                    string day22ot = string.Empty;
                    string day23ot = string.Empty;
                    string day24ot = string.Empty;
                    string day25ot = string.Empty;
                    string day26ot = string.Empty;
                    string day27ot = string.Empty;
                    string day28ot = string.Empty;
                    string day29ot = string.Empty;
                    string day30ot = string.Empty;
                    string day31ot = string.Empty;

                    float dayduties = 0;
                    float dayots = 0;
                    float daywos = 0;
                    float dayNHS = 0;

                    #endregion

                    #region Variables for Posting order Table data and EmpAttendance(Default Values)

                    int orderid = 0;

                    string PrevUnitid = string.Empty;
                    string Dutyhrs = string.Empty;
                    DateTime Orderdate = DateTime.Now;
                    DateTime Joiningdate = DateTime.Now;
                    DateTime Releivingdate = DateTime.Now;
                    string IssuedAuthority = string.Empty;
                    string Remarks = string.Empty;
                    int TransferType = 1;

                    string AttString = string.Empty;
                    string HrsString = string.Empty;
                    float TotalHours = 0;
                    float OTHours = 0;
                    float NHDays = 0;
                    float CL = 0;
                    float PL = 0;
                    float UL = 0;

                    int pf = 0;
                    int esi = 0;
                    int pt = 0;

                    string Contractid = string.Empty;

                    #endregion



                    if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                    {

                        clientid = ds.Tables[0].Rows[k]["Client Id"].ToString();
                        ViewState["clientid"] = clientid;
                        clientid = ViewState["clientid"].ToString();
                    }


                    int Sno = 0;
                    selectquery = "select max(cast(Sno as int )) as Sno from empattendance where clientid='" + clientid + "' and month='" + Month + "' ";
                    DataTable dtSno = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

                    if (dtSno.Rows.Count > 0)
                    {
                        if (String.IsNullOrEmpty(dtSno.Rows[0]["Sno"].ToString()) == false)
                        {
                            Sno = Convert.ToInt32(dtSno.Rows[0]["Sno"].ToString()) + 1;
                        }
                        else
                        {
                            Sno = int.Parse("1");
                        }
                    }

                    DateTime DtLastDay = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));

                    #region  Begin Get Contract Id Based on The Last Day


                    Hashtable HtGetContractID = new Hashtable();
                    var SPNameForGetContractID = "GetContractIDBasedOnthMonth";
                    HtGetContractID.Add("@clientid", clientid);
                    HtGetContractID.Add("@LastDay", DtLastDay);
                    DataTable DTContractID = config.ExecuteAdaptorAsyncWithParams(SPNameForGetContractID, HtGetContractID).Result;

                    if (DTContractID.Rows.Count > 0)
                    {
                        Contractid = DTContractID.Rows[0]["contractid"].ToString();
                    }
                   

                    #endregion  End Get Contract Id Based on The Last Day

                    var Clientstatus = 0;

                    if (ddloption.SelectedIndex == 0)
                    {
                        Clientstatus = 1;
                    }
                    else
                    {
                        if (ddlClientID.SelectedValue == clientid)
                        {
                            Clientstatus = 1;
                        }
                    }


                    if (Clientstatus == 1)
                    {
                        if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                        {
                            empstatus = 0;

                            empid = ds.Tables[0].Rows[k]["Emp Id"].ToString();
                            ViewState["empid"] = empid;
                            empid = ViewState["empid"].ToString();

                            string sqlchkempid = "select empid from empdetails where empid='" + empid + "' and empstatus=1";
                            DataTable dtchkempid = config.ExecuteAdaptorAsyncWithQueryParams(sqlchkempid).Result;

                            for (int i = 0; i < dtchkempid.Rows.Count; i++)
                            {
                                string empid2 = "";
                                empid2 = dtchkempid.Rows[i]["empid"].ToString();
                                if (empid == empid2)
                                {
                                    empstatus = 1;
                                }
                            }
                        }


                        //Create Procedure


                        if (empid.Length > 0)
                        {

                            if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                            {
                                design = ds.Tables[0].Rows[k]["Designation"].ToString();
                                ViewState["design"] = design;
                                design = ViewState["design"].ToString();

                            }

                            if ((k % 2) == 0 || k == 0 || (k % 2) == 1)
                            {
                                float dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13, dt14, dt15, dt16, dt17, dt18, dt19, dt20, dt21,
                                    dt22, dt23, dt24, dt25, dt26, dt27, dt28, dt29, dt30, dt31;

                                float Ots1, Ots2, Ots3, Ots4, Ots5, Ots6, Ots7, Ots8, Ots9, Ots10, Ots11, Ots12, Ots13, Ots14, Ots15, Ots16, Ots17, Ots18, Ots19, Ots20, Ots21,
                                   Ots22, Ots23, Ots24, Ots25, Ots26, Ots27, Ots28, Ots29, Ots30, Ots31;

                                float wo1, wo2, wo3, wo4, wo5, wo6, wo7, wo8, wo9, wo10, wo11, wo12, wo13, wo14, wo15, wo16, wo17, wo18, wo19, wo20, wo21,
                                  wo22, wo23, wo24, wo25, wo26, wo27, wo28, wo29, wo30, wo31;


                                float nhs1, nhs2, nhs3, nhs4, nhs5, nhs6, nhs7, nhs8, nhs9, nhs10, nhs11, nhs12, nhs13, nhs14, nhs15, nhs16, nhs17, nhs18, nhs19, nhs20, nhs21,
                                  nhs22, nhs23, nhs24, nhs25, nhs26, nhs27, nhs28, nhs29, nhs30, nhs31;

                                dt1 = dt2 = dt3 = dt4 = dt5 = dt6 = dt7 = dt8 = dt9 = dt10 = dt11 = dt12 = dt13 = dt14 = dt15 = dt16 = dt17 = dt18 = dt19 = dt20 = dt21 =
                                    dt22 = dt23 = dt24 = dt25 = dt26 = dt27 = dt28 = dt29 = dt30 = dt31 = 0;

                                Ots1 = Ots2 = Ots3 = Ots4 = Ots5 = Ots6 = Ots7 = Ots8 = Ots9 = Ots10 = Ots11 = Ots12 = Ots13 = Ots14 = Ots15 = Ots16 = Ots17 = Ots18 = Ots19 = Ots20 = Ots21 =
                                   Ots22 = Ots23 = Ots24 = Ots25 = Ots26 = Ots27 = Ots28 = Ots29 = Ots30 = Ots31 = 0;

                                wo1 = wo2 = wo3 = wo4 = wo5 = wo6 = wo7 = wo8 = wo9 = wo10 = wo11 = wo12 = wo13 = wo14 = wo15 = wo16 = wo17 = wo18 = wo19 = wo20 = wo21 =
                                  wo22 = wo23 = wo24 = wo25 = wo26 = wo27 = wo28 = wo29 = wo30 = wo31 = 0;


                                nhs1 = nhs2 = nhs3 = nhs4 = nhs5 = nhs6 = nhs7 = nhs8 = nhs9 = nhs10 = nhs11 = nhs12 = nhs13 = nhs14 = nhs15 = nhs16 = nhs17 = nhs18 = nhs19 = nhs20 = nhs21 =
                                  nhs22 = nhs23 = nhs24 = nhs25 = nhs26 = nhs27 = nhs28 = nhs29 = nhs30 = nhs31 = 0;

                                #region Begin New code on 28/04/2014 by venkat for Duties,ots,wos,penalty,Incentives,NHS,Ab and Na

                                duties = 0;
                                ots = 0;

                                //if (String.IsNullOrEmpty(dr["Duties"].ToString()) == false)
                                //{
                                //    duties = float.Parse(dr["Duties"].ToString());
                                //}

                                if (String.IsNullOrEmpty(dr["OTs"].ToString()) == false)
                                {
                                    ots = float.Parse(dr["OTs"].ToString());
                                }

                                if (String.IsNullOrEmpty(dr["Canteen Advance"].ToString()) == false)
                                {
                                    canteenadvance = float.Parse(dr["Canteen Advance"].ToString());
                                }
                                if (String.IsNullOrEmpty(dr["Leaves"].ToString()) == false)
                                {
                                    Leaves = float.Parse(dr["Leaves"].ToString());
                                }
                                if (String.IsNullOrEmpty(dr["Incentives"].ToString()) == false)
                                {
                                    incentives = float.Parse(dr["Incentives"].ToString());
                                }


                                if (String.IsNullOrEmpty(dr["Rent Ded"].ToString()) == false)
                                {
                                    RentDed = float.Parse(dr["Rent Ded"].ToString());
                                }

                                #endregion

                                #region Day wise Even data insert



                                day1 = dr["1"].ToString();
                                if (day1.Trim().Length == 0 || day1 == "0")
                                { day1 = "A"; }

                                day2 = dr["2"].ToString();
                                if (day2.Length == 0 || day2 == "0")
                                { day2 = "A"; }

                                day3 = dr["3"].ToString();
                                if (day3.Length == 0 || day3 == "0")
                                { day3 = "A"; }

                                day4 = dr["4"].ToString();
                                if (day4.Length == 0 || day4 == "0")
                                {
                                    day4 = "A";
                                }

                                day5 = dr["5"].ToString();
                                if (day5.Length == 0 || day5 == "0")
                                {
                                    day5 = "A";
                                }

                                day6 = dr["6"].ToString();
                                if (day6.Length == 0 || day6 == "0")
                                {
                                    day6 = "A";
                                }

                                day7 = dr["7"].ToString();
                                if (day7.Length == 0 || day7 == "0")
                                {
                                    day7 = "A";
                                }

                                day8 = dr["8"].ToString();
                                if (day8.Length == 0 || day8 == "0")
                                {
                                    day8 = "A";
                                }

                                day9 = dr["9"].ToString();
                                if (day9.Length == 0 || day9 == "0")
                                {
                                    day9 = "A";
                                }

                                day10 = dr["10"].ToString();
                                if (day10.Length == 0 || day10 == "0")
                                {
                                    day10 = "A";
                                }

                                day11 = dr["11"].ToString();
                                if (day11.Length == 0 || day11 == "0")
                                {
                                    day11 = "A";
                                }

                                day12 = dr["12"].ToString();
                                if (day12.Length == 0 || day12 == "0")
                                {
                                    day12 = "A";
                                }

                                day13 = dr["13"].ToString();
                                if (day13.Length == 0 || day13 == "0")
                                {
                                    day13 = "A";
                                }

                                day14 = dr["14"].ToString();
                                if (day14.Length == 0 || day14 == "0")
                                {
                                    day14 = "A";
                                }

                                day15 = dr["15"].ToString();
                                if (day15.Length == 0 || day15 == "0")
                                {
                                    day15 = "A";
                                }

                                day16 = dr["16"].ToString();
                                if (day16.Length == 0 || day16 == "0")
                                {
                                    day16 = "A";
                                }

                                day17 = dr["17"].ToString();
                                if (day17.Length == 0 || day17 == "0")
                                {
                                    day17 = "A";
                                }

                                day18 = dr["18"].ToString();
                                if (day18.Length == 0 || day18 == "0")
                                {
                                    day18 = "A";
                                }

                                day19 = dr["19"].ToString();
                                if (day19.Length == 0 || day19 == "0")
                                {
                                    day19 = "A";
                                }

                                day20 = dr["20"].ToString();
                                if (day20.Length == 0 || day20 == "0")
                                {
                                    day20 = "A";
                                }

                                day21 = dr["21"].ToString();
                                if (day21.Length == 0 || day21 == "0")
                                {
                                    day21 = "A";
                                }

                                day22 = dr["22"].ToString();
                                if (day22.Length == 0 || day22 == "0")
                                {
                                    day22 = "A";
                                }

                                day23 = dr["23"].ToString();
                                if (day23.Length == 0 || day23 == "0")
                                {
                                    day23 = "A";
                                }

                                day24 = dr["24"].ToString();
                                if (day24.Length == 0 || day24 == "0")
                                {
                                    day24 = "A";
                                }

                                day25 = dr["25"].ToString();
                                if (day25.Length == 0 || day25 == "0")
                                {
                                    day25 = "A";
                                }

                                day26 = dr["26"].ToString();
                                if (day26.Length == 0 || day26 == "0")
                                {
                                    day26 = "A";
                                }

                                day27 = dr["27"].ToString();
                                if (day27.Length == 0 || day27 == "0")
                                {
                                    day27 = "A";
                                }

                                day28 = dr["28"].ToString();
                                if (day28.Length == 0 || day28 == "0")
                                {
                                    day28 = "A";
                                }

                                day29 = dr["29"].ToString();
                                if (day29.Length == 0 || day29 == "0")
                                {
                                    day29 = "A";
                                }

                                day30 = dr["30"].ToString();
                                if (day30.Length == 0 || day30 == "0")
                                {
                                    day30 = "A";
                                }

                                day31 = dr["31"].ToString();
                                if (day31.Length == 0 || day31 == "0")
                                {
                                    day31 = "A";
                                }

                                #endregion


                                #region Values for Duties

                                //1

                                if (day1.Trim() == "P" || day1.Trim() == "p")
                                {
                                    dt1 = 1;
                                }
                                if (day1.Trim() == "P/P" || day1.Trim() == "p/p")
                                {
                                    dt1 = 1;
                                    Ots1 = 1;
                                }
                                if (day1.Trim() == "J" || day1.Trim() == "j")
                                {
                                    dt1 = 1.5f;
                                }

                                if (day1.Trim() == "W/O" || day1.Trim() == "w/o")
                                {
                                    wo1 = 1;
                                }

                                if (day1.Trim() == "A" || day1.Trim() == "a")
                                {
                                    dt1 = 0;
                                }
                                if (day1.Trim() == "N" || day1.Trim() == "n")
                                {
                                    nhs1 = 1;
                                }

                                //2
                                if (day2.Trim() == "P" || day2.Trim() == "p")
                                {
                                    dt2 = 1;
                                }
                                if (day2.Trim() == "P/P" || day2.Trim() == "P/P")
                                {
                                    dt2 = 1;
                                    Ots2 = 1;
                                }
                                if (day2.Trim() == "J" || day2.Trim() == "j")
                                {
                                    dt2 = 1.5f;
                                }
                                if (day2.Trim() == "W/O" || day2.Trim() == "w/o")
                                {
                                    wo2 = 1;
                                }
                                if (day2.Trim() == "A" || day2.Trim() == "a")
                                {
                                    dt2 = 0;
                                }
                                if (day2.Trim() == "N" || day2.Trim() == "n")
                                {
                                    nhs2 = 1;
                                }



                                //3
                                if (day3.Trim() == "P" || day3.Trim() == "p")
                                {
                                    dt3 = 1;
                                }
                                if (day3.Trim() == "P/P" || day3.Trim() == "p/p")
                                {
                                    dt3 = 1; Ots3 = 1;
                                }
                                if (day3.Trim() == "J" || day3.Trim() == "j")
                                {
                                    dt3 = 1.5f;
                                }

                                if (day3.Trim() == "W/O" || day3.Trim() == "w/o")
                                {
                                    wo3 = 1;
                                }
                                if (day3.Trim() == "A" || day3.Trim() == "a")
                                {
                                    dt3 = 0;
                                }
                                if (day3.Trim() == "N" || day3.Trim() == "n")
                                {
                                    nhs3 = 1;
                                }



                                //4
                                if (day4.Trim() == "P" || day4.Trim() == "p")
                                {
                                    dt4 = 1;
                                }
                                if (day4.Trim() == "P/P" || day4.Trim() == "p/p")
                                {
                                    dt4 = 1; Ots4 = 1;
                                }
                                if (day4.Trim() == "J" || day4.Trim() == "j")
                                {
                                    dt4 = 1.5f;
                                }

                                if (day4.Trim() == "W/O" || day4.Trim() == "w/o")
                                {
                                    wo4 = 1;
                                }
                                if (day4.Trim() == "A" || day4.Trim() == "a")
                                {
                                    dt4 = 0;
                                }
                                if (day4.Trim() == "N" || day4.Trim() == "n")
                                {
                                    nhs4 = 1;
                                }

                                //5
                                if (day5.Trim() == "P" || day5.Trim() == "p")
                                {
                                    dt5 = 1;
                                }
                                if (day5.Trim() == "P/P" || day5.Trim() == "p/p")
                                {
                                    dt5 = 1; Ots5 = 1;
                                }
                                if (day5.Trim() == "J" || day5.Trim() == "j")
                                {
                                    dt5 = 1.5f;
                                }
                                if (day5.Trim() == "W/O" || day5 == "w/o")
                                {
                                    wo5 = 1;
                                }
                                if (day5.Trim() == "A" || day5.Trim() == "a")
                                {
                                    dt5 = 0;
                                }
                                if (day5.Trim() == "N" || day5.Trim() == "n")
                                {
                                    nhs5 = 1;
                                }

                                //6
                                if (day6.Trim() == "P" || day6.Trim() == "p")
                                {
                                    dt6 = 1;
                                }
                                if (day6.Trim() == "P/P" || day6.Trim() == "p/p")
                                {
                                    dt6 = 1; Ots6 = 1;
                                }
                                if (day6.Trim() == "J" || day6.Trim() == "j")
                                {
                                    dt6 = 1.5f;
                                }

                                if (day6.Trim() == "W/O" || day6.Trim() == "w/o")
                                {
                                    wo6 = 1;
                                }
                                if (day6.Trim() == "A" || day6.Trim() == "a")
                                {
                                    dt6 = 0;
                                }
                                if (day6.Trim() == "N" || day6.Trim() == "n")
                                {
                                    nhs6 = 1;
                                }

                                //7
                                if (day7.Trim() == "P" || day7.Trim() == "p")
                                {
                                    dt7 = 1;
                                }
                                if (day7.Trim() == "P/P" || day7.Trim() == "p/p")
                                {
                                    dt7 = 1; Ots7 = 1;
                                }
                                if (day7.Trim() == "J" || day7.Trim() == "j")
                                {
                                    dt7 = 1.5f;
                                }
                                if (day7.Trim() == "W/O" || day7.Trim() == "w/o")
                                {
                                    wo7 = 1;
                                }
                                if (day7.Trim() == "A" || day7.Trim() == "a")
                                {
                                    dt7 = 0;
                                }
                                if (day7.Trim() == "N" || day7.Trim() == "n")
                                {
                                    nhs7 = 1;
                                }
                                //8
                                if (day8.Trim() == "P" || day8.Trim() == "p")
                                {
                                    dt8 = 1;
                                }
                                if (day8.Trim() == "P/P" || day8.Trim() == "p/p")
                                {
                                    dt8 = 1; Ots8 = 1;
                                }
                                if (day8.Trim() == "J" || day8.Trim() == "j")
                                {
                                    dt8 = 1.5f;
                                }
                                if (day8.Trim() == "W/O" || day8.Trim() == "w/o")
                                {
                                    wo8 = 1;
                                }
                                if (day8.Trim() == "A" || day8.Trim() == "a")
                                {
                                    dt8 = 0;
                                }
                                if (day8.Trim() == "N" || day8.Trim() == "n")
                                {
                                    nhs8 = 1;
                                }

                                //9
                                if (day9.Trim() == "P" || day9.Trim() == "p")
                                {
                                    dt9 = 1;
                                }
                                if (day9.Trim() == "P/P" || day9.Trim() == "p/p")
                                {
                                    dt9 = 1; Ots9 = 1;
                                }
                                if (day9.Trim() == "J" || day9.Trim() == "j")
                                {
                                    dt9 = 1.5f;
                                }

                                if (day9.Trim() == "W/O" || day9.Trim() == "w/o")
                                {
                                    wo9 = 1;
                                }
                                if (day9.Trim() == "A" || day9.Trim() == "a")
                                {
                                    dt9 = 0;
                                }
                                if (day9.Trim() == "N" || day9.Trim() == "n")
                                {
                                    nhs9 = 1;
                                }

                                //10
                                if (day10.Trim() == "P" || day10.Trim() == "p")
                                {
                                    dt10 = 1;
                                }
                                if (day10.Trim() == "P/P" || day10.Trim() == "p/p")
                                {
                                    dt10 = 1; Ots10 = 1;
                                }
                                if (day10.Trim() == "J" || day10.Trim() == "j")
                                {
                                    dt10 = 1.5f;
                                }
                                if (day10.Trim() == "W/O" || day10.Trim() == "w/o")
                                {
                                    wo10 = 1;
                                }
                                if (day10.Trim() == "A" || day10.Trim() == "a")
                                {
                                    dt10 = 0;
                                }
                                if (day10.Trim() == "N" || day10.Trim() == "n")
                                {
                                    nhs10 = 1;
                                }
                                //11

                                if (day11.Trim() == "P" || day11.Trim() == "p")
                                {
                                    dt11 = 1;
                                }
                                if (day11.Trim() == "P/P" || day11.Trim() == "p/p")
                                {
                                    dt11 = 1; Ots11 = 1;
                                }
                                if (day11.Trim() == "J" || day11.Trim() == "j")
                                {
                                    dt11 = 1.5f;
                                }
                                if (day11.Trim() == "W/O" || day11.Trim() == "w/o")
                                {
                                    wo11 = 1;
                                }
                                if (day11.Trim() == "A" || day11.Trim() == "a")
                                {
                                    dt11 = 0;
                                }
                                if (day11.Trim() == "N" || day11.Trim() == "n")
                                {
                                    nhs11 = 1;
                                }
                                //12
                                if (day12.Trim() == "P" || day12.Trim() == "p")
                                {
                                    dt12 = 1;
                                }
                                if (day12.Trim() == "P/P" || day12.Trim() == "p/p")
                                {
                                    dt12 = 1; Ots12 = 1;
                                }
                                if (day12.Trim() == "J" || day12.Trim() == "j")
                                {
                                    dt12 = 1.5f;
                                }
                                if (day12.Trim() == "W/O" || day12.Trim() == "w/o")
                                {
                                    wo12 = 1;
                                }
                                if (day12.Trim() == "A" || day12.Trim() == "a")
                                {
                                    dt12 = 0;
                                }
                                if (day12.Trim() == "N" || day12.Trim() == "n")
                                {
                                    nhs12 = 1;
                                }
                                //13
                                if (day13.Trim() == "P" || day13.Trim() == "p")
                                {
                                    dt13 = 1;
                                }
                                if (day13.Trim() == "P/P" || day13.Trim() == "p/p")
                                {
                                    dt13 = 1; Ots13 = 1;
                                }
                                if (day13.Trim() == "J" || day13.Trim() == "j")
                                {
                                    dt13 = 1.5f;
                                }
                                if (day13.Trim() == "W/O" || day13.Trim() == "w/o")
                                {
                                    wo13 = 1;
                                }
                                if (day13.Trim() == "A" || day13.Trim() == "a")
                                {
                                    dt13 = 0;
                                }
                                if (day13 == "N" || day13 == "n")
                                {
                                    nhs13 = 1;
                                }
                                //14
                                if (day14.Trim() == "P" || day14.Trim() == "p")
                                {
                                    dt14 = 1;
                                }
                                if (day14.Trim() == "P/P" || day14.Trim() == "p/p")
                                {
                                    dt14 = 1; Ots14 = 1;
                                }
                                if (day14.Trim() == "J" || day14.Trim() == "j")
                                {
                                    dt14 = 1.5f;
                                }
                                if (day14.Trim() == "W/O" || day14.Trim() == "w/o")
                                {
                                    wo14 = 1;
                                }
                                if (day14.Trim() == "A" || day14.Trim() == "a")
                                {
                                    dt14 = 0;
                                }
                                if (day14.Trim() == "N" || day14.Trim() == "n")
                                {
                                    nhs14 = 1;
                                }
                                //15
                                if (day15.Trim() == "P" || day15.Trim() == "p")
                                {
                                    dt15 = 1;
                                }
                                if (day15.Trim() == "P/P" || day15.Trim() == "p/p")
                                {
                                    dt15 = 1; Ots15 = 1;
                                }
                                if (day15.Trim() == "J" || day15.Trim() == "j")
                                {
                                    dt15 = 1.5f;
                                }
                                if (day15.Trim() == "W/O" || day15.Trim() == "w/o")
                                {
                                    wo15 = 1;
                                }
                                if (day15.Trim() == "A" || day15.Trim() == "a")
                                {
                                    dt15 = 0;
                                }
                                if (day15.Trim() == "N" || day15.Trim() == "n")
                                {
                                    nhs15 = 1;
                                }
                                //16
                                if (day16.Trim() == "P" || day16.Trim() == "p")
                                {
                                    dt16 = 1;
                                }
                                if (day16.Trim() == "P/P" || day16.Trim() == "p/p")
                                {
                                    dt16 = 1; Ots16 = 1;
                                }
                                if (day16.Trim() == "J" || day16.Trim() == "j")
                                {
                                    dt16 = 1.5f;
                                }
                                if (day16.Trim() == "W/O" || day16.Trim() == "w/o")
                                {
                                    wo16 = 1;
                                }
                                if (day16.Trim() == "A" || day16.Trim() == "a")
                                {
                                    dt16 = 0;
                                }
                                if (day16.Trim() == "N" || day16.Trim() == "n")
                                {
                                    nhs16 = 1;
                                }
                                //17
                                if (day17.Trim() == "P" || day17.Trim() == "p")
                                {
                                    dt17 = 1;
                                }
                                if (day17.Trim() == "P/P" || day17.Trim() == "p/p")
                                {
                                    dt17 = 1; Ots17 = 1;
                                }
                                if (day17.Trim() == "J" || day17.Trim() == "j")
                                {
                                    dt17 = 1.5f;
                                }
                                if (day17.Trim() == "W/O" || day17.Trim() == "w/o")
                                {
                                    wo17 = 1;
                                }
                                if (day17.Trim() == "A" || day17.Trim() == "a")
                                {
                                    dt17 = 0;
                                }
                                if (day17.Trim() == "N" || day17.Trim() == "n")
                                {
                                    nhs17 = 1;
                                }
                                //18
                                if (day18.Trim() == "P" || day18.Trim() == "p")
                                {
                                    dt18 = 1;
                                }
                                if (day18.Trim() == "P/P" || day18.Trim() == "p/p")
                                {
                                    dt18 = 1; Ots18 = 1;
                                }
                                if (day18.Trim() == "J" || day18.Trim() == "j")
                                {
                                    dt18 = 1.5f;
                                }
                                if (day18.Trim() == "W/O" || day18.Trim() == "w/o")
                                {
                                    wo18 = 1;
                                }
                                if (day18.Trim() == "A" || day18.Trim() == "a")
                                {
                                    dt18 = 0;
                                }
                                if (day18.Trim() == "N" || day18.Trim() == "n")
                                {
                                    nhs18 = 1;
                                }
                                //19
                                if (day19.Trim() == "P" || day19.Trim() == "p")
                                {
                                    dt19 = 1;
                                }
                                if (day19.Trim() == "P/P" || day19.Trim() == "p/p")
                                {
                                    dt19 = 1; Ots19 = 1;
                                }
                                if (day19.Trim() == "J" || day19.Trim() == "j")
                                {
                                    dt19 = 1.5f;
                                }
                                if (day19.Trim() == "W/O" || day19.Trim() == "w/o")
                                {
                                    wo19 = 1;
                                }
                                if (day19.Trim() == "A" || day19.Trim() == "a")
                                {
                                    dt19 = 0;
                                }
                                if (day19.Trim() == "N" || day19.Trim() == "n")
                                {
                                    nhs19 = 1;
                                }
                                //20
                                if (day20.Trim() == "P" || day20.Trim() == "p")
                                {
                                    dt20 = 1;
                                }
                                if (day20.Trim() == "P/P" || day20.Trim() == "p/p")
                                {
                                    dt20 = 1; Ots20 = 1;
                                }
                                if (day20.Trim() == "J" || day20.Trim() == "j")
                                {
                                    dt20 = 1.5f;
                                }
                                if (day20.Trim() == "W/O" || day20.Trim() == "w/o")
                                {
                                    wo20 = 1;
                                }
                                if (day20.Trim() == "A" || day20.Trim() == "a")
                                {
                                    dt20 = 0;
                                }
                                if (day20.Trim() == "N" || day20.Trim() == "n")
                                {
                                    nhs20 = 1;
                                }
                                //21
                                if (day21.Trim() == "P" || day21.Trim() == "p")
                                {
                                    dt21 = 1;
                                }
                                if (day21.Trim() == "P/P" || day21.Trim() == "p/p")
                                {
                                    dt21 = 1; Ots21 = 1;
                                }
                                if (day21.Trim() == "J" || day21.Trim() == "j")
                                {
                                    dt21 = 1.5f;
                                }
                                if (day21.Trim() == "W/O" || day21.Trim() == "w/o")
                                {
                                    wo21 = 1;
                                }
                                if (day21.Trim() == "A" || day21.Trim() == "a")
                                {
                                    dt21 = 0;
                                }
                                if (day21.Trim() == "N" || day21.Trim() == "n")
                                {
                                    nhs21 = 1;
                                }
                                //22
                                if (day22.Trim() == "P" || day22.Trim() == "p")
                                {
                                    dt22 = 1;
                                }
                                if (day22.Trim() == "P/P" || day22.Trim() == "p/p")
                                {
                                    dt22 = 1; Ots22 = 1;
                                }
                                if (day22.Trim() == "J" || day22.Trim() == "j")
                                {
                                    dt22 = 1.5f;
                                }
                                if (day22.Trim() == "W/O" || day22.Trim() == "w/o")
                                {
                                    wo22 = 1;
                                }
                                if (day22.Trim() == "A" || day22.Trim() == "a")
                                {
                                    dt22 = 0;
                                }
                                if (day22.Trim() == "N" || day22.Trim() == "n")
                                {
                                    nhs22 = 1;
                                }
                                //23
                                if (day23.Trim() == "P" || day23.Trim() == "p")
                                {
                                    dt23 = 1;
                                }
                                if (day23.Trim() == "P/P" || day23.Trim() == "p/p")
                                {
                                    dt23 = 1; Ots23 = 1;
                                }
                                if (day23.Trim() == "J" || day23.Trim() == "j")
                                {
                                    dt23 = 1.5f;
                                }
                                if (day23.Trim() == "W/O" || day23.Trim() == "w/o")
                                {
                                    wo23 = 1;
                                }
                                if (day23.Trim() == "A" || day23.Trim() == "a")
                                {
                                    dt23 = 0;
                                }
                                if (day23.Trim() == "N" || day23.Trim() == "n")
                                {
                                    nhs23 = 1;
                                }
                                //24
                                if (day24.Trim() == "P" || day24.Trim() == "p")
                                {
                                    dt24 = 1;
                                }
                                if (day24.Trim() == "P/P" || day24.Trim() == "p/p")
                                {
                                    dt24 = 1; Ots24 = 1;
                                }
                                if (day24.Trim() == "J" || day24.Trim() == "j")
                                {
                                    dt24 = 1.5f;
                                }
                                if (day24.Trim() == "W/O" || day24.Trim() == "w/o")
                                {
                                    wo24 = 1;
                                }
                                if (day24.Trim() == "A" || day24.Trim() == "a")
                                {
                                    dt24 = 0;
                                }
                                if (day24.Trim() == "N" || day24.Trim() == "n")
                                {
                                    nhs24 = 1;
                                }
                                //25
                                if (day25.Trim() == "P" || day25.Trim() == "p")
                                {
                                    dt25 = 1;
                                }
                                if (day25.Trim() == "P/P" || day25.Trim() == "p/p")
                                {
                                    dt25 = 1; Ots25 = 1;
                                }
                                if (day25.Trim() == "J" || day25.Trim() == "j")
                                {
                                    dt25 = 1.5f;
                                }
                                if (day25.Trim() == "W/O" || day25.Trim() == "w/o")
                                {
                                    wo25 = 1;
                                }
                                if (day25.Trim() == "A" || day25.Trim() == "a")
                                {
                                    dt25 = 0;
                                }
                                if (day25.Trim() == "N" || day25.Trim() == "n")
                                {
                                    nhs25 = 1;
                                }
                                //26
                                if (day26.Trim() == "P" || day26.Trim() == "p")
                                {
                                    dt26 = 1;
                                }
                                if (day26.Trim() == "P/P" || day26.Trim() == "p/p")
                                {
                                    dt26 = 1; Ots26 = 1;
                                }
                                if (day26.Trim() == "J" || day26.Trim() == "j")
                                {
                                    dt26 = 1.5f;
                                }
                                if (day26.Trim() == "W/O" || day26.Trim() == "w/o")
                                {
                                    wo26 = 1;
                                }
                                if (day26.Trim() == "A" || day26.Trim() == "a")
                                {
                                    dt26 = 0;
                                }
                                if (day26.Trim() == "N" || day26.Trim() == "n")
                                {
                                    nhs26 = 1;
                                }
                                //27
                                if (day27.Trim() == "P" || day27.Trim() == "p")
                                {
                                    dt27 = 1;
                                }
                                if (day27.Trim() == "P/P" || day27.Trim() == "p/p")
                                {
                                    dt27 = 1; Ots27 = 1;
                                }
                                if (day27.Trim() == "J" || day27.Trim() == "j")
                                {
                                    dt27 = 1.5f;
                                }
                                if (day27.Trim() == "W/O" || day27.Trim() == "w/o")
                                {
                                    wo27 = 1;
                                }
                                if (day27.Trim() == "A" || day27.Trim() == "a")
                                {
                                    dt27 = 0;
                                }
                                if (day27.Trim() == "N" || day27.Trim() == "n")
                                {
                                    nhs27 = 1;
                                }
                                //28
                                if (day28.Trim() == "P" || day28.Trim() == "p")
                                {
                                    dt28 = 1;
                                }
                                if (day28.Trim() == "P/P" || day28.Trim() == "p/p")
                                {
                                    dt28 = 1; Ots28 = 1;
                                }
                                if (day28.Trim() == "J" || day28.Trim() == "j")
                                {
                                    dt28 = 1.5f;
                                }
                                if (day28.Trim() == "W/O" || day28.Trim() == "w/o")
                                {
                                    wo28 = 1;
                                }
                                if (day28.Trim() == "A" || day28.Trim() == "a")
                                {
                                    dt28 = 0;
                                }
                                if (day28.Trim() == "N" || day28.Trim() == "n")
                                {
                                    nhs28 = 1;
                                }
                                //29
                                if (day29.Trim() == "P" || day29.Trim() == "p")
                                {
                                    dt29 = 1;
                                }
                                if (day29.Trim() == "P/P" || day29.Trim() == "p/p")
                                {
                                    dt29 = 1; Ots29 = 1;
                                }
                                if (day29.Trim() == "J" || day29.Trim() == "j")
                                {
                                    dt29 = 1.5f;
                                }
                                if (day29.Trim() == "W/O" || day29.Trim() == "w/o")
                                {
                                    wo29 = 1;
                                }
                                if (day29.Trim() == "A" || day29.Trim() == "a")
                                {
                                    dt29 = 0;
                                }
                                if (day29.Trim() == "N" || day29.Trim() == "n")
                                {
                                    nhs29 = 1;
                                }
                                //30
                                if (day30.Trim() == "P" || day30.Trim() == "p")
                                {
                                    dt30 = 1;
                                }
                                if (day30.Trim() == "P/P" || day30.Trim() == "p/p")
                                {
                                    dt30 = 1; Ots30 = 1;
                                }
                                if (day30.Trim() == "J" || day30.Trim() == "j")
                                {
                                    dt30 = 1.5f;
                                }
                                if (day30.Trim() == "W/O" || day30.Trim() == "w/o")
                                {
                                    wo30 = 1;
                                }
                                if (day30.Trim() == "A" || day30.Trim() == "a")
                                {
                                    dt30 = 0;
                                }
                                if (day30.Trim() == "N" || day30.Trim() == "n")
                                {
                                    nhs30 = 1;
                                }
                                //31
                                if (day31.Trim() == "P" || day31.Trim() == "p")
                                {
                                    dt31 = 1;
                                }
                                if (day31.Trim() == "P/P" || day31.Trim() == "p/p")
                                {
                                    dt31 = 1; Ots31 = 1;
                                }
                                if (day31.Trim() == "J" || day31.Trim() == "j")
                                {
                                    dt31 = 1.5f;
                                }
                                if (day31.Trim() == "W/O" || day31.Trim() == "w/o")
                                {
                                    wo31 = 1;
                                }
                                if (day31.Trim() == "A" || day31.Trim() == "a")
                                {
                                    dt31 = 0;
                                }
                                if (day31.Trim() == "N" || day31.Trim() == "n")
                                {
                                    nhs31 = 1;
                                }


                                #endregion Values for Duties

                                dayduties = dt1 + dt2 + dt3 + dt4 + dt5 + dt6 + dt7 + dt8 + dt9 + dt10 + dt11 + dt12 + dt13 + dt14 + dt15 + dt16 + dt17 + dt18 + dt19 + dt20 + dt21 +
                             dt22 + dt23 + dt24 + dt25 + dt26 + dt27 + dt28 + dt29 + dt30 + dt31;

                                dayots = Ots1 + Ots2 + Ots3 + Ots4 + Ots5 + Ots6 + Ots7 + Ots8 + Ots9 + Ots10 + Ots11 + Ots12 + Ots13 + Ots14 + Ots15 + Ots16 + Ots17 + Ots18 + Ots19 + Ots20 + Ots21 +
                           Ots22 + Ots23 + Ots24 + Ots25 + Ots26 + Ots27 + Ots28 + Ots29 + Ots30 + Ots31;

                                daywos = wo1 + wo2 + wo3 + wo4 + wo5 + wo6 + wo7 + wo8 + wo9 + wo10 + wo11 + wo12 + wo13 + wo14 + wo15 + wo16 + wo17 + wo18 + wo19 + wo20 + wo21 +
                             wo22 + wo23 + wo24 + wo25 + wo26 + wo27 + wo28 + wo29 + wo30 + wo31;

                                dayNHS = nhs1 + nhs2 + nhs3 + nhs4 + nhs5 + nhs6 + nhs7 + nhs8 + nhs9 + nhs10 + nhs11 + nhs12 + nhs13 + nhs14 + nhs15 + nhs16 + nhs17 + nhs18 + nhs19 + nhs20 + nhs21 +
                             nhs22 + nhs23 + nhs24 + nhs25 + nhs26 + nhs27 + nhs28 + nhs29 + nhs30 + nhs31;


                                duties = dayduties + duties;
                                ots = dayots + ots;
                                Wos = daywos + Wos;

                                NHS = dayNHS;

                            }


                            #region Begin New code for Stored Procedure as on 29/04/2014 by venkat


                            #region Begin code for passing values to the Stored Procedure as 29/04/2014 by Venkat

                            Hashtable Httable = new Hashtable();

                            Httable.Add("@k", k);
                            Httable.Add("@empidstatus", empstatus);


                            Httable.Add("@Clientid", clientid);
                            Httable.Add("@Month", Month);
                            Httable.Add("@Empid", empid);
                            Httable.Add("@ContractId", Contractid);
                            Httable.Add("@Design", design);

                            Httable.Add("@day1", day1);
                            Httable.Add("@day2", day2);
                            Httable.Add("@day3", day3);
                            Httable.Add("@day4", day4);
                            Httable.Add("@day5", day5);
                            Httable.Add("@day6", day6);
                            Httable.Add("@day7", day7);
                            Httable.Add("@day8", day8);
                            Httable.Add("@day9", day9);
                            Httable.Add("@day10", day10);
                            Httable.Add("@day11", day11);
                            Httable.Add("@day12", day12);
                            Httable.Add("@day13", day13);
                            Httable.Add("@day14", day14);
                            Httable.Add("@day15", day15);
                            Httable.Add("@day16", day16);
                            Httable.Add("@day17", day17);
                            Httable.Add("@day18", day18);
                            Httable.Add("@day19", day19);
                            Httable.Add("@day20", day20);
                            Httable.Add("@day21", day21);
                            Httable.Add("@day22", day22);
                            Httable.Add("@day23", day23);
                            Httable.Add("@day24", day24);
                            Httable.Add("@day25", day25);
                            Httable.Add("@day26", day26);
                            Httable.Add("@day27", day27);
                            Httable.Add("@day28", day28);
                            Httable.Add("@day29", day29);
                            Httable.Add("@day30", day30);
                            Httable.Add("@day31", day31);

                            Httable.Add("@day1ot", day1ot);
                            Httable.Add("@day2ot", day2ot);
                            Httable.Add("@day3ot", day3ot);
                            Httable.Add("@day4ot", day4ot);
                            Httable.Add("@day5ot", day5ot);
                            Httable.Add("@day6ot", day6ot);
                            Httable.Add("@day7ot", day7ot);
                            Httable.Add("@day8ot", day8ot);
                            Httable.Add("@day9ot", day9ot);
                            Httable.Add("@day10ot", day10ot);
                            Httable.Add("@day11ot", day11ot);
                            Httable.Add("@day12ot", day12ot);
                            Httable.Add("@day13ot", day13ot);
                            Httable.Add("@day14ot", day14ot);
                            Httable.Add("@day15ot", day15ot);
                            Httable.Add("@day16ot", day16ot);
                            Httable.Add("@day17ot", day17ot);
                            Httable.Add("@day18ot", day18ot);
                            Httable.Add("@day19ot", day19ot);
                            Httable.Add("@day20ot", day20ot);
                            Httable.Add("@day21ot", day21ot);
                            Httable.Add("@day22ot", day22ot);
                            Httable.Add("@day23ot", day23ot);
                            Httable.Add("@day24ot", day24ot);
                            Httable.Add("@day25ot", day25ot);
                            Httable.Add("@day26ot", day26ot);
                            Httable.Add("@day27ot", day27ot);
                            Httable.Add("@day28ot", day28ot);
                            Httable.Add("@day29ot", day29ot);
                            Httable.Add("@day30ot", day30ot);
                            Httable.Add("@day31ot", day31ot);

                            Httable.Add("@Duties", duties);
                            Httable.Add("@Ots", ots);
                            Httable.Add("@WOs", Wos);
                            Httable.Add("@NHs", NHS);
                            Httable.Add("@Sno", Sno);
                            Httable.Add("@Excel_Number", ExcelNo);
                            Httable.Add("@CanteenAdv", canteenadvance);
                            Httable.Add("@Leaves", Leaves);
                            Httable.Add("@Incentivs", incentives);
                            Httable.Add("@RentDed", RentDed);
                            Httable.Add("@RowNo", j + 2);


                            #endregion

                            string SPName = "ImportAttendanceFromExcel";
                            DataTable dtstatus = config.ExecuteAdaptorAsyncWithParams(SPName, Httable).Result;


                            #endregion


                        }

                      

                    }

                    
                    k++;
                    j++;
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }


            }

           


            #endregion




            ddlAttendanceMode.SelectedIndex = 0;
            GetAttSummary(ExcelNo.ToString());

        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvnotinsert.Rows.Count > 0)
            {
                util.NewExport("Unsaveddata.xlsx", this.gvnotinsert);
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {

            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();

            string Month = month + Year.Substring(2, 2);

            if (ddlClientID.SelectedIndex > 0 && txtmonth.Text.Trim().Length != 0)
            {

                string sqldeleteempattendance = "delete empattendance where clientid='" + ddlClientID.SelectedValue + "' and month='" + Month + "'";
                int status = config.ExecuteNonQueryWithQueryAsync(sqldeleteempattendance).Result;
                if (status > 0)
                {
                    GetAttSummary("");
                }


            }
        }



        protected void lnkImportfromexcel_Click(object sender, EventArgs e)
        {//
            if (ddlAttendanceMode.SelectedIndex == 0)
            {
                util.NewExport("Employee Attendance.xlsx", this.grvSample2);
            }
            if (ddlAttendanceMode.SelectedIndex == 1)
            {
                util.NewExport("Employee Attendance.xlsx", this.SampleGrid);
            }
        }

        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddloption.SelectedIndex == 1)
            {
                FillClientNameList();
                FillClientList();
                lblclientid.Visible = true;
                lblclientname.Visible = true;
                ddlClientID.Visible = true;
                ddlCName.Visible = true;
                btnClear.Visible = true;
            }
            else
            {
                lblclientid.Visible = false;
                lblclientname.Visible = false;
                ddlClientID.Visible = false;
                ddlCName.Visible = false;
                btnClear.Visible = false;
            }
        }


    }
}