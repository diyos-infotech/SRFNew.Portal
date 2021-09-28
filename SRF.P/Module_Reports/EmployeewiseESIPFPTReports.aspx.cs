using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Data;
using System.Collections;
using System.Globalization;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class EmployeewiseESIPFPTReports : System.Web.UI.Page
    {
        //DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil util = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {

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
                    FillEmployeesList();
                    LoadNames();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Show alert", "alert('Your Session Expired');", true);
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void GetWebConfigdata()
        {
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();

        }

        protected void FillEmployeesList()
        {
            DataTable DtEmpIds = GlobalData.Instance.LoadEmpIds(EmpIDPrefix);
            if (DtEmpIds.Rows.Count > 0)
            {
                ddlEmployee.DataValueField = "empid";
                ddlEmployee.DataTextField = "empid";
                ddlEmployee.DataSource = DtEmpIds;
                ddlEmployee.DataBind();
            }
            ddlEmployee.Items.Insert(0, "-Select-");
            ddlEmployee.Items.Insert(1, "All");
        }

        protected void LoadNames()
        {
            DataTable DtEmpNames = GlobalData.Instance.LoadEmpNames(EmpIDPrefix);
            if (DtEmpNames.Rows.Count > 0)
            {
                ddlempname.DataValueField = "empid";
                ddlempname.DataTextField = "FullName";
                ddlempname.DataSource = DtEmpNames;
                ddlempname.DataBind();
            }
            ddlempname.Items.Insert(0, "-Select-");
            ddlempname.Items.Insert(1, "All");

        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlEmployee.SelectedIndex > 0)
            {
                ddlempname.SelectedValue = ddlEmployee.SelectedValue;
            }
            else
            {
                ddlempname.SelectedIndex = 0;
            }

        }

        protected void ddlempname_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ddlempname.SelectedIndex > 0)
            {
                ddlEmployee.SelectedValue = ddlempname.SelectedValue;
            }
            else
            {
                ddlEmployee.SelectedIndex = 0;
            }
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {
            Lblempid.Visible = true;
            ddlEmployee.Visible = true;
            lblempname.Visible = true;
            ddlempname.Visible = true;
            lbtn_Export.Visible = true;
            ClearData();

            string month = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Month.ToString();
            string Year = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).Year.ToString();
            DateTime date = DateTime.Now;


            DateTime firstday = DateTime.Now;
            firstday = GlobalData.Instance.GetFirstDayMonth(int.Parse(Year), int.Parse(month));
            string fday = firstday.ToShortDateString();
            //string date = string.Empty;

            #region Begin Code  For Validation as on [16-11-2013]


            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The Month');", true);
                return;
            }

            #endregion End  Code  For Validation as on [16-11-2013]

            #region  Begin Code For Variable Declaration   as on [16-11-2013]


            DataTable DtListOfEmployees = null;
            Hashtable HtListOfEmployees = new Hashtable();

            string sqlqry = string.Empty;
            #endregion End  Code For Variable Declaration  as on [16-11-2013]



            if (ddlEmployee.SelectedIndex == 1)
            {

                sqlqry = "select ed.EmpId,c.clientname,eps.clientid,rtrim(ISNULL(ed.EmpFName, '') + ' ' + ISNULL(ed.empmname, '') + ' ' + ISNULL(ed.emplname, '')) as Fullname, sum(round(eps.PFWAGES, 0)) as PFWAGES,case when DATEDIFF(year, EmpDtofBirth, GETDATE())< 58 then sum(round(eps.PFWAGES, 0)) else '0' end EPSWAGES," +
                           " sum(round(eps.ProfTax, 0)) as ProfTax,sum(round(eps.ESIWAGES, 0)) as ESIWAGES,sum(round(eps.ESI, 0)) as ESI,sum(round(eps.ESIONOT, 0)) as ESIONOT,sum(round(eps.ESIONOT*100/1.75, 0)) as ESIonOTWages,(sum(round(eps.ESIONOT*100/1.75, 0))+sum(round(eps.ESIWAGES, 0))) as EmpConsESIWages, " +
                         " sum(round(eps.PF, 0)) as PF,sum(round(eps.ots, 0)) as ots,sum(round(eps.PFEmpr, 0)) as PFEmpr, 0 as NCPDAYS, 0 as ADVREF, 0 as ARREAREPF, 0 as ARREAREPFEE,0 as ARREAREPFER, 0 as ARREAREPS, case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then sum(round((eps.PFWages * 8.33 / 100), 0)) else '' end EPSDue, sum(round(eps.PF, 0)) as PF, sum(round(eps.PFEmpr, 0)) as PFEmpr, sum(eps.NoOfDuties) as NoOfDuties, case when(ed.EmpDtofJoining >= '" + fday + "') then 'F' else '' end as relation, sum(round(eps.PF - (case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then((eps.PFWAGES * 8.33 / 100)) else '' end), 0)) as PfDiff, " +
                         " case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10), ed.EmpDtofBirth, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofBirth, 103) end else '' end EmpDtofBirth,case convert(varchar(10), ed.empdtofleaving, 103) when '01/01/1900' then '' else  convert(varchar(10), ed.empdtofleaving, 103) end empdtofleaving,case when(convert(varchar(10), ed.empdtofleaving, 103) <> '01/01/1900' and convert(varchar(10), ed.empdtofleaving, 103) <> '')  then 'C' else '' end Reason," +
                         " case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpSex else '' end as EmpSex  ,case convert(varchar(10), ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end EmpDtofJoining,  case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpFatherName else '' end EmpFatherName,case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10),ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end else '' end EmpPFEnrolDt," +
                         " ltrim(rtrim(Esi.EmpEsiNo)) EmpEsiNo,ltrim(rtrim(epf.EmpEpfNo)) EmpEpfNo from EmpPaySheet Eps  inner join EmpDetails ed on ed.EmpId = eps.EmpId inner join clients c on c.clientid = eps.clientid left outer join EMPESICodes esi on ed.EmpId = esi.Empid left outer join EMPEPFCodes epf on ed.EmpId = epf.Empid " +
                         " where  eps.month = '" + month + Year.Substring(2, 2) + "'   group by ed.EmpId,c.clientname,eps.clientid, ed.EmpFatherName, ed.EmpDtofJoining, ed.EmpDtofBirth, ed.EmpFName, ed.empmname, ed.emplname, ed.EmpSex,Esi.EmpEsiNo, epf.EmpEpfNo, ed.empdtofleaving ";
                Bindata(sqlqry);
                return;
            }
            sqlqry = "select ed.EmpId,c.clientname,eps.clientid,rtrim(ISNULL(ed.EmpFName, '') + ' ' + ISNULL(ed.empmname, '') + ' ' + ISNULL(ed.emplname, '')) as Fullname, sum(round(eps.PFWAGES, 0)) as PFWAGES,case when DATEDIFF(year, EmpDtofBirth, GETDATE())< 58 then sum(round(eps.PFWAGES, 0)) else '0' end EPSWAGES," +
                         " sum(round(eps.ProfTax, 0)) as ProfTax,sum(round(eps.ESIWAGES, 0)) as ESIWAGES,sum(round(eps.ESI, 0)) as ESI,sum(round(eps.ESIONOT, 0)) as ESIONOT,sum(round(eps.ESIONOT*100/1.75, 0)) as ESIonOTWages,(sum(round(eps.ESIONOT*100/1.75, 0))+sum(round(eps.ESIWAGES, 0))) as EmpConsESIWages, " +
                       " sum(round(eps.PF, 0)) as PF,sum(round(eps.ots, 0)) as ots,sum(round(eps.PFEmpr, 0)) as PFEmpr, 0 as NCPDAYS, 0 as ADVREF, 0 as ARREAREPF, 0 as ARREAREPFEE,0 as ARREAREPFER, 0 as ARREAREPS, case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then sum(round((eps.PFWages * 8.33 / 100), 0)) else '' end EPSDue, sum(round(eps.PF, 0)) as PF, sum(round(eps.PFEmpr, 0)) as PFEmpr, sum(eps.NoOfDuties) as NoOfDuties, case when(ed.EmpDtofJoining >= '" + fday + "') then 'F' else '' end as relation, sum(round(eps.PF - (case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then((eps.PFWAGES * 8.33 / 100)) else '' end), 0)) as PfDiff, " +
                       " case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10), ed.EmpDtofBirth, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofBirth, 103) end else '' end EmpDtofBirth,case convert(varchar(10), ed.empdtofleaving, 103) when '01/01/1900' then '' else  convert(varchar(10), ed.empdtofleaving, 103) end empdtofleaving,case when(convert(varchar(10), ed.empdtofleaving, 103) <> '01/01/1900' and convert(varchar(10), ed.empdtofleaving, 103) <> '')  then 'C' else '' end Reason," +
                       " case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpSex else '' end as EmpSex  ,case convert(varchar(10), ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end EmpDtofJoining,  case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpFatherName else '' end EmpFatherName,case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10),ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end else '' end EmpPFEnrolDt," +
                       " ltrim(rtrim(Esi.EmpEsiNo)) EmpEsiNo,ltrim(rtrim(epf.EmpEpfNo)) EmpEpfNo from EmpPaySheet Eps  inner join EmpDetails ed on ed.EmpId = eps.EmpId inner join clients c on c.clientid = eps.clientid left outer join EMPESICodes esi on ed.EmpId = esi.Empid left outer join EMPEPFCodes epf on ed.EmpId = epf.Empid " +
                       " where  eps.month = '" + month + Year.Substring(2, 2) + "' and ed.empid ='" + ddlEmployee.SelectedValue + "' group by ed.EmpId,c.clientname,eps.clientid, ed.EmpFatherName, ed.EmpDtofJoining, ed.EmpDtofBirth, ed.EmpFName, ed.empmname, ed.emplname, ed.EmpSex,Esi.EmpEsiNo, epf.EmpEpfNo, ed.empdtofleaving ";

            Bindata(sqlqry);

        }


        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                // Fillpfandesidetails();
                lbtn_Export.Visible = true;
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('There Is No Salary Details For The Selected client');", true);

            }
        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            util.Export("PFReport.xls", this.GVListEmployees);
        }
        protected void lbtn_Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("ESIandPFForms.aspx?Value=");
        }
        float totalPFDuties = 0;
        float totalPFWAGES = 0;
        float totalPF = 0;
        float totalPFEmpr = 0;
        float totalTotalPF = 0;

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        float PFDuties = float.Parse(((Label)e.Row.FindControl("lblPFDuties")).Text);
            //        totalPFDuties += PFDuties;
            //        float PFWAGES = float.Parse(((Label)e.Row.FindControl("lblPFWAGES")).Text);
            //        totalPFWAGES += PFWAGES;
            //        float PF = float.Parse(((Label)e.Row.FindControl("lblPF")).Text);
            //        totalPF += PF;
            //        float PFEmpr = float.Parse(((Label)e.Row.FindControl("lblPFEmpr")).Text);
            //        totalPFEmpr += PFEmpr;
            //        float TotalPF = float.Parse(((Label)e.Row.FindControl("lblTotalPF")).Text);
            //        totalTotalPF += TotalPF;
            //    }
            //    if (e.Row.RowType == DataControlRowType.Footer)
            //    {
            //        ((Label)e.Row.FindControl("lblTotalPFDuties")).Text = totalPFDuties.ToString();
            //        ((Label)e.Row.FindControl("lblTotalPFWAGES")).Text = totalPFWAGES.ToString();
            //        ((Label)e.Row.FindControl("lblTotalPF")).Text = totalPF.ToString();
            //        ((Label)e.Row.FindControl("lblTotalPFEmpr")).Text = totalPFEmpr.ToString();
            //         ((Label)e.Row.FindControl("lblTotalTotalPF")).Text = totalTotalPF.ToString();

            //    }
        }
    }
}