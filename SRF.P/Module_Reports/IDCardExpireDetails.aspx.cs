using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KLTS.Data;
using SRF.P.DAL;


namespace SRF.P.Module_Reports
{
    public partial class IDCardExpireDetails : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        DataTable dt;

        string EmpIDPrefix = "";
        string CmpIDPrefix = "";


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
                DataBinder();
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

        protected void DataBinder()
        {
            LblResult.Text = "";
            LblResult.Visible = true;//SELECT * FROM (SELECT * FROM topten ORDER BY datetime DESC) tmp GROUP BY home
            //string sqlQry = "Select Licenses.UnitId,LicenseStartDate,LicenseEndDate,LicenseExpired," +
            //    "LicenseOfficeLoc,Clients.ClientName from Clients Inner JOIN  Licenses ON " +
            //    "Licenses.UnitId=Clients.ClientId and LicenseEndDate<='" + DateTime.Now.AddMonths(1).ToShortDateString() + "'";
            string sqlQry = "select ROW_NUMBER() over(order By EmpId) as 'S.No',EmpId as 'EmployeeID',(EmpFName + '' + EmpMName + '' + EmpLName) as 'EmployeeName',case convert(nvarchar(20), IDCardIssued, 103) when '01/01/1900' then '' else convert(nvarchar(20), IDCardIssued, 103) end IDCardIssued,case convert(nvarchar(20), IDCardValid, 103) when '01/01/1900' then '' else convert(nvarchar(20), IDCardValid, 103) end IDCardValid from empdetails  where IDCardValid >='" + DateTime.Now.ToShortDateString() + "' and IDCardValid <='" + DateTime.Now.AddMonths(1).ToShortDateString() + "'";

            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;

            if (data.Rows.Count > 0)
            {

                dgLicExpire.DataSource = data;
                dgLicExpire.DataBind();
            }
            else
            {
                LblResult.Text = "There is no IDCard Expiring till next month";
            }
        }
        protected void btncancel_Click(object sender, EventArgs e)
        {
            // ClearAll();
        }
        protected void dgLicExpire_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgLicExpire.PageIndex = e.NewPageIndex;
            DataBinder();
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            string sqlQry = "select ROW_NUMBER() over(order By EmpId) as 'S.No',EmpId as 'EmployeeID',(EmpFName + '' + EmpMName + '' + EmpLName) as 'EmployeeName',case convert(nvarchar(20), IDCardIssued, 103) when '01/01/1900' then '' else convert(nvarchar(20), IDCardIssued, 103) end IDCardIssued,case convert(nvarchar(20), IDCardValid, 103) when '01/01/1900' then '' else convert(nvarchar(20), IDCardValid, 103) end IDCardValid from empdetails  where IDCardValid >='" + DateTime.Now.ToShortDateString() + "' and IDCardValid <='" + DateTime.Now.AddMonths(1).ToShortDateString() + "'";

            DataTable data = config.ExecuteAdaptorAsyncWithQueryParams(sqlQry).Result;

            if (data.Rows.Count > 0)
            {
                gve.NewExportExcel("IDCardReport.xls", data);
            }

        }

    }
}