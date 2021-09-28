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
    public partial class PFReports : System.Web.UI.Page
    {
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
                    FillClientList();
                    FillClientNameList();
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


        protected void FillClientList()
        {
            DataTable dt = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlClientId.DataValueField = "clientid";
                ddlClientId.DataTextField = "clientid";
                ddlClientId.DataSource = dt;
                ddlClientId.DataBind();
            }
            ddlClientId.Items.Insert(0, "--Select--");
            ddlClientId.Items.Insert(1, "ALL");
        }

        protected void FillClientNameList()
        {

            DataTable dt = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (dt.Rows.Count > 0)
            {
                ddlcname.DataValueField = "clientid";
                ddlcname.DataTextField = "Clientname";
                ddlcname.DataSource = dt;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "--Select--");
            ddlcname.Items.Insert(1, "ALL");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlcname.SelectedIndex > 0)
            {
                ddlClientId.SelectedValue = ddlcname.SelectedValue;
            }
            else
            {
                ddlClientId.SelectedIndex = 0;
            }
        }

        protected void ddlClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlClientId.SelectedIndex > 0)
            {
                ddlcname.SelectedValue = ddlClientId.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
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
            if (ddlClientId.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The client');", true);
                return;
            }

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



            if (ddlClientId.SelectedIndex == 1)
            {
                sqlqry = " select ed.EmpId,c.clientname,eps.clientid,rtrim(ISNULL(ed.EmpFName, '') + ' ' + ISNULL(ed.empmname, '') + ' ' + ISNULL(ed.emplname, '')) as Fullname, (round(eps.PFWAGES, 0)) as PFWAGES,case when DATEDIFF(year, EmpDtofBirth, GETDATE())< 58 then (round(eps.PFWAGES, 0)) else '0' end EPSWAGES," +
                         "  (round(eps.ESIonDuties, 0)) as ESIonDuties,(round(eps.ProfTax, 0)) as ProfTax,(round(eps.ESIWAGES, 0)) as ESIWAGES,(round(eps.ESI, 0)) as ESI,(round(eps.ESIONOT, 0)) as ESIONOT,(round(eps.ESIONOT*100/1.75, 0)) as ESIonOTWages,((round(eps.ESIONOT*100/1.75, 0))+(round(eps.ESIWAGES, 0))) as EmpConsESIWages,eps.otamt as OTWAges,eps.gross as GrossWages, " +
                       " (round(eps.PF, 0)) as PF,(round(eps.ots, 0)) as ots,(round(eps.PFEmpr, 0)) as PFEmpr, 0 as NCPDAYS, 0 as ADVREF, 0 as ARREAREPF, 0 as ARREAREPFEE,0 as ARREAREPFER, 0 as ARREAREPS, case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then (round((eps.PFWages * 8.33 / 100), 0)) else '' end EPSDue, (round(eps.PF, 0)) as PF, (round(eps.PFEmpr, 0)) as PFEmpr, (eps.NoOfDuties) as NoOfDuties, case when(ed.EmpDtofJoining >= '" + fday + "') then 'F' else '' end as relation, (round(eps.PF - (case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then((eps.PFWAGES * 8.33 / 100)) else '' end), 0)) as PfDiff, " +
                       " case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10), ed.EmpDtofBirth, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofBirth, 103) end else '' end EmpDtofBirth,case convert(varchar(10), ed.empdtofleaving, 103) when '01/01/1900' then '' else  convert(varchar(10), ed.empdtofleaving, 103) end empdtofleaving,case when(convert(varchar(10), ed.empdtofleaving, 103) <> '01/01/1900' and convert(varchar(10), ed.empdtofleaving, 103) <> '')  then 'C' else '' end Reason," +
                       " case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpSex else '' end as EmpSex  ,case convert(varchar(10), ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end EmpDtofJoining,  case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpFatherName else '' end EmpFatherName,case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10),ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end else '' end EmpPFEnrolDt," +
                       " ltrim(rtrim(epf.EmpEpfNo)) EmpEpfNo from EmpPaySheet Eps  inner join EmpDetails ed on ed.EmpId = eps.EmpId inner join clients c on c.clientid = eps.clientid left outer join EMPEPFCodes epf on ed.EmpId = epf.Empid " +
                       " where  eps.month = '" + month + Year.Substring(2, 2) + "' order by eps.clientid,ed.empid";


                Bindata(sqlqry);
                return;

            }


            sqlqry = " select ed.EmpId,c.clientname,eps.clientid,rtrim(ISNULL(ed.EmpFName, '') + ' ' + ISNULL(ed.empmname, '') + ' ' + ISNULL(ed.emplname, '')) as Fullname, (round(eps.PFWAGES, 0)) as PFWAGES,case when DATEDIFF(year, EmpDtofBirth, GETDATE())< 58 then (round(eps.PFWAGES, 0)) else '0' end EPSWAGES," +
                           "  (round(eps.ESIonDuties, 0)) as ESIonDuties,(round(eps.ProfTax, 0)) as ProfTax,(round(eps.ESIWAGES, 0)) as ESIWAGES,(round(eps.ESI, 0)) as ESI,(round(eps.ESIONOT, 0)) as ESIONOT,(round(eps.ESIONOT*100/1.75, 0)) as ESIonOTWages,((round(eps.ESIONOT*100/1.75, 0))+(round(eps.ESIWAGES, 0))) as EmpConsESIWages,eps.otamt as OTWAges,eps.gross as GrossWages, " +
                         " (round(eps.PF, 0)) as PF,(round(eps.ots, 0)) as ots,(round(eps.PFEmpr, 0)) as PFEmpr, 0 as NCPDAYS, 0 as ADVREF, 0 as ARREAREPF, 0 as ARREAREPFEE,0 as ARREAREPFER, 0 as ARREAREPS, case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then (round((eps.PFWages * 8.33 / 100), 0)) else '' end EPSDue, (round(eps.PF, 0)) as PF, (round(eps.PFEmpr, 0)) as PFEmpr, (eps.NoOfDuties) as NoOfDuties, case when(ed.EmpDtofJoining >= '" + fday + "') then 'F' else '' end as relation, (round(eps.PF - (case when DATEDIFF(year, EmpDtofBirth, GETDATE()) < 58 then((eps.PFWAGES * 8.33 / 100)) else '' end), 0)) as PfDiff, " +
                         " case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10), ed.EmpDtofBirth, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofBirth, 103) end else '' end EmpDtofBirth,case convert(varchar(10), ed.empdtofleaving, 103) when '01/01/1900' then '' else  convert(varchar(10), ed.empdtofleaving, 103) end empdtofleaving,case when(convert(varchar(10), ed.empdtofleaving, 103) <> '01/01/1900' and convert(varchar(10), ed.empdtofleaving, 103) <> '')  then 'C' else '' end Reason," +
                         " case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpSex else '' end as EmpSex  ,case convert(varchar(10), ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end EmpDtofJoining,  case when(ed.EmpDtofJoining >= '" + fday + "') then ed.EmpFatherName else '' end EmpFatherName,case when(ed.EmpDtofJoining >= '" + fday + "') then case convert(varchar(10),ed.EmpDtofJoining, 103) when '01/01/1900' then '' else convert(varchar(10), ed.EmpDtofJoining, 103) end else '' end EmpPFEnrolDt," +
                         " ltrim(rtrim(epf.EmpEpfNo)) EmpEpfNo from EmpPaySheet Eps  inner join EmpDetails ed on ed.EmpId = eps.EmpId inner join clients c on c.clientid = eps.clientid left outer join EMPEPFCodes epf on ed.EmpId = epf.Empid " +
                         " where  eps.month = '" + month + Year.Substring(2, 2) + "' and eps.clientid ='" + ddlClientId.SelectedValue + "' order by eps.clientid,ed.empid ";


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