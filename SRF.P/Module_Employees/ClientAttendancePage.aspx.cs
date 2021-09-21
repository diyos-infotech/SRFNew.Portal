using KLTS.Data;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using SRF.P.DAL;

namespace SRF.P.Module_Employees
{
    public partial class ClientAttendancePage : System.Web.UI.Page
    {

        AppConfiguration config = new AppConfiguration();

        private static string EmpIDPrefix = string.Empty;

        private static string CmpIDPrefix = string.Empty;

        private static DataTable _dtEmployees = new DataTable();

        public const string _trString = @"<tr class='tr-emp-att' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##' >
                                 <td>##EMPID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##</td>
                                 <td><input type='text' class='form-control num-txt txt-nod' value='##NOD##'></td>
                                 <td><input type='text' class='form-control num-txt txt-ot' value='##OT##'></td>
                                 <td><input type='text' class='form-control num-txt txt-wo' value='##WO##'></td>
                                 <td><input type='text' class='form-control num-txt txt-nhs' value='##NHS##'></td>
                                 <td><input type='text' class='form-control num-txt txt-nposts' value='##NPOSTS##'></td>
                                 <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>
                                 <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>
                                 <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td>           
                                 <td><label class='txt-linetotal'/></td>           
                                 <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>
                                </tr>";

        private const string _attendanceQuery = @"select EA.EmpId,
			                   ISNULL(EmpFName,'')+' '+ISNULL(EmpMName,'')+' '+ISNULL(EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                   EA.NoOfDuties as NOD ,
			                   EA.Ot as OT ,
			                   EA.WO as WO,
			                   EA.NHS as NHS,
			                   EA.Npots as NPots,
			                   EA.CanteenAdv as CanAdv,
			                   EA.Penalty as Pen,
			                   EA.Incentivs as Inctvs 
		                from EmpAttendance EA join EmpDetails ED on Ed.EmpId=EA.EmpId join Designations D on D.DesignId=EA.Design 
		                and EA.ClientId='##CLIENTID##' and EA.Month=##MONTH## and EA.ContractId='##CONTRACTID##'
		                union all
		                select ep.EmpId,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
			                   0 as CanAdv,
			                   0 as Pen,
			                   0 as Inctvs 
		                from EmpPostingOrder ep
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and (relievemonth is null or  relievemonth  <>  ##MONTH##)
		                and ep.EmpId not in (select EmpId from EmpAttendance where ClientId = '##CLIENTID##' and month = ##MONTH## and ContractId='##CONTRACTID##')";

        private const string _attendanceSummaryquery = @"select d.Design DesName,
	                                                           cast(sum(ea.NoOfDuties)as nvarchar) NODTotal,
	                                                           cast(sum(ea.OT)as nvarchar) OTTotal,
	                                                           cast(sum(ea.WO)as nvarchar) WOTotal,
	                                                           cast(sum(ea.NHS)as nvarchar) NHSTotal,
	                                                           cast(sum(ea.Npots)as nvarchar) NpotsTotal,
	                                                           cast(sum(ea.Penalty)as nvarchar) PenTotal,
	                                                           cast(sum(ea.Incentivs)as nvarchar) InctvsTotal,
	                                                           cast(sum(ea.CanteenAdv)as nvarchar) CanAdvTotal
                                                        from EmpAttendance ea 
                                                        inner join Designations d on d.DesignId = ea.Design
                                                        inner join EmpPostingOrder ep on ea.EmpId = ep.EmpId
                                                        where ea.ClientId = '##CLIENTID##' and ea.[MONTH]= ##MONTH##
                                                        and ep.RelieveMonth is null
                                                        group by ea.Design,d.Design";

        public static DataTable EmployeesDataTable
        {
            get
            {
                if (_dtEmployees.Rows.Count < 1)
                {
                    var dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix);
                    _dtEmployees = dtEmployees;
                }
                return _dtEmployees;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();

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

            if (!IsPostBack)
            {

                FillMonthList();
                BindEmpddls();
                GetClientsData();
            }
        }

       

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
        }


        protected void FillMonthList()
        {
            //month
            var formatInfoinfo = new DateTimeFormatInfo();
            string[] monthName = formatInfoinfo.MonthNames;
            string month = monthName[DateTime.Now.Month - 1];
            string LastMonth = "";
            ddlMonth.Items.Add(month);
            try
            {
                month = monthName[DateTime.Now.Month - 2];
            }
            catch (IndexOutOfRangeException ex)
            {
                month = monthName[11];
            }
            try
            {
                LastMonth = monthName[DateTime.Now.Month - 3];
            }
            catch (IndexOutOfRangeException ex)
            {
                LastMonth = monthName[12 - (3 - DateTime.Now.Month)];
            }

            ddlMonth.Items.Add(month);
            ddlMonth.Items.Add(LastMonth);

            ddlMonth.Items.Insert(0, new ListItem { Value = "0", Text = "--Select--" });

        }

        private void BindEmpddls()
        {
            DataTable DtDesignations = GlobalData.Instance.LoadDesigns();
            if (DtDesignations.Rows.Count > 0)
            {
                ddlEmpDesg.DataValueField = "Designid";
                ddlEmpDesg.DataTextField = "Design";
                ddlEmpDesg.DataSource = DtDesignations;
                ddlEmpDesg.DataBind();
            }
            ddlEmpDesg.Items.Insert(0, new ListItem { Value = "0", Text = "--Select--" });
        }

        private void GetClientsData()
        {
            var result = "";
            string query = "select clientid,clientname,clientphonenumbers,ourcontactpersonid from clients where ClientId like '" + CmpIDPrefix + "%' Order By  Clientname";
            var dtAllClients = config.ExecuteReaderWithQueryAsync(query).Result;
            if (dtAllClients.Rows.Count > 0)
            {
                var obj = (from row in dtAllClients.AsEnumerable()
                           select new
                           {
                               ClientId = row.Field<string>("clientid"),
                               ClientName = row.Field<string>("clientname"),
                               PhoneNumber = row.Field<string>("clientphonenumbers"),
                               ContactPerson = row.Field<string>("ourcontactpersonid")
                           }).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
                hdClientData.Value = result;
            }
        }

        [WebMethod]
        public static string GetEmployessData(string strid)
        {
            var result = string.Empty;
            try
            {
                if (EmployeesDataTable.Rows.Count > 0)
                {
                    var obj = (from row in EmployeesDataTable.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("Empid"),
                                   EmpName = row.Field<string>("FullName"),
                                   EmpDesg = row.Field<string>("Designation")
                               }).ToList();

                    obj = obj.Where(o => o.EmpId.Contains(strid.Trim())).ToList();
                    result = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        [WebMethod]
        public static string GetEmployessNameData(string strname)
        {
            var result = string.Empty;
            try
            {
                if (EmployeesDataTable.Rows.Count > 0)
                {
                    var obj = (from row in EmployeesDataTable.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("Empid"),
                                   EmpName = row.Field<string>("FullName"),
                                   EmpDesg = row.Field<string>("Designation")
                               }).ToList();

                    obj = obj.Where(o => o.EmpName.Contains(strname.Trim())).ToList();
                    result = new JavaScriptSerializer().Serialize(obj);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        [WebMethod]
        public static string GetAttendanceGrid(string clientId, string month)
        {
            var result = string.Empty;
            var resultobj = string.Empty;
            try
            {
                var empDt = GetAttendanceMainDatatable(clientId, month);
                if (empDt.Rows.Count > 0)
                {
                    var obj = (from row in empDt.AsEnumerable()
                               select new
                               {
                                   EmpId = row.Field<string>("EmpId"),
                                   EmpName = row.Field<string>("EmpName"),
                                   DesgId = row.Field<int>("DesId"),
                                   DesgName = row.Field<string>("DesName"),
                                   NoOfDuties = row.Field<float>("NOD"),
                                   OT = row.Field<float>("OT"),
                                   WO = row.Field<float>("WO"),
                                   NHS = row.Field<float>("NHS"),
                                   NPosts = row.Field<float>("NPots"),
                                   CanteenAdv = row.Field<float>("CanAdv"),
                                   Penalty = row.Field<float>("Pen"),
                                   Incentivs = row.Field<float>("Inctvs"),
                               }).ToList();

                    resultobj = new JavaScriptSerializer().Serialize(obj);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                    resultobj = "Attendance Not Avaialable for  this month of the Selected client";
                }

            }
            catch (Exception ex)
            {
                result = "fail";
            }
            var res = new { msg = result, Obj = resultobj };
            resultobj = new JavaScriptSerializer().Serialize(res);
            return resultobj;
        }

        public static DataTable GetAttendanceMainDatatable(string clientId, string month)
        {
            AppConfiguration config = new AppConfiguration();

            var LastDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
            var result = new DataTable();
            var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            var strquery = "select contractid from contracts where clientid= '" + clientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = config.ExecuteReaderWithQueryAsync(strquery).Result;
            var contractId = string.Empty;
            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
            }
            if (!string.IsNullOrEmpty(contractId))
            {
                var sx = _attendanceQuery.Replace("##MONTH##", Month.ToString())
                                 .Replace("##CLIENTID##", clientId)
                                 .Replace("##CONTRACTID##", contractId);
                var attData = config.ExecuteReaderWithQueryAsync(sx).Result;
                if (attData.Rows.Count > 0)
                {
                    result = attData;
                }
            }
            return result;

        }

        [WebMethod]
        public static string GetAttendanceSummary(string clientId, string month)
        {
            AppConfiguration config = new AppConfiguration();

            var result = string.Empty;
            var resultobj = string.Empty;
            try
            {
                var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
                var sx = _attendanceSummaryquery.Replace("##MONTH##", Month.ToString())
                                         .Replace("##CLIENTID##", clientId);
                var attData = config.ExecuteReaderWithQueryAsync(sx).Result;
                if (attData.Rows.Count > 0)
                {
                    var obj = (from row in attData.AsEnumerable()
                               select new
                               {
                                   DesgName = row.Field<string>("DesName"),
                                   NODTotal = row.Field<string>("NODTotal"),
                                   OTTotal = row.Field<string>("OTTotal"),
                                   WOTotal = row.Field<string>("WOTotal"),
                                   NHSTotal = row.Field<string>("NHSTotal"),
                                   NpotsTotal = row.Field<string>("NpotsTotal"),
                                   PenTotal = row.Field<string>("PenTotal"),
                                   InctvsTotal = row.Field<string>("InctvsTotal"),
                                   CanAdvTotal = row.Field<string>("CanAdvTotal"),
                               }).ToList();
                    resultobj = new JavaScriptSerializer().Serialize(obj);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                }
            }
            catch (Exception ex)
            {
                result = "fail";
            }
            var res = new { msg = result, Obj = resultobj };
            resultobj = new JavaScriptSerializer().Serialize(res);
            return resultobj;
        }

        [WebMethod]
        public static string SaveAttendance(List<EmpAttendance> lst)
        {

            AppConfiguration config = new AppConfiguration();

            string OrderedDAte = DateTime.Now.Date.ToString();
            var LastDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");

            try
            {
                foreach (var item in lst)
                {
                    var attendancetotal = item.NOD + item.OT + item.WO + item.NHS + item.Nposts;
                    if (attendancetotal > 0)
                    {
                        if (item.NewAdd) EmpTransfer(item);
                        var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(item.MonthIndex));
                        var strquery = "select contractid from contracts where clientid= '" + item.ClientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
                        var contractdata = config.ExecuteReaderWithQueryAsync(strquery).Result;
                        var contractId = string.Empty;
                        if (contractdata.Rows.Count > 0)
                        {
                            contractId = contractdata.Rows[0]["contractid"].ToString();
                        }
                        if (!string.IsNullOrEmpty(contractId))
                        {
                            var empquery = "select COUNT(*) as empcount from  Empattendance Where Empid = '" + item.EmpId +
                                        "' and [month]= " + Month + " and ClientId = '" + item.ClientId +
                                        "'  and  Design = " + item.EmpDesg + " and   contractid= '" + contractId + "'";
                            var empdata = config.ExecuteReaderWithQueryAsync(empquery).Result;
                            var empcount = string.Empty;
                            if (empdata.Rows.Count > 0)
                            {
                                empcount = empdata.Rows[0]["empcount"].ToString();
                            }
                            var query = string.Empty;


                            if (Convert.ToInt32(empcount) > 0)
                            {
                                query = "update EmpAttendance set NoofDuties=" + item.NOD
                                                    + ",OT=" + item.OT
                                                    + ",Penalty=" + item.Penality
                                                    + ",CanteenAdv=" + item.CanAdv
                                                    + ",Incentivs=" + item.Incentives
                                                    + ",Design='" + item.EmpDesg
                                                    + "',WO=" + item.WO
                                                    + ",NHS=" + item.NHS
                                                    + ",NPOTS=" + item.Nposts
                                                    + " Where empid='" + item.EmpId
                                                    + "' and ClientId='" + item.ClientId
                                                    + "' and [Month]=" + Month
                                                    + " and  Design='" + item.EmpDesg
                                                    + "' and contractid= '" + contractId + "'";
                            }
                            else
                            {
                                query = "insert  EmpAttendance(clientid,empid,[month],Design,contractId,NoofDuties,OT,Penalty,CanteenAdv,WO,NHS,NPOTS,Incentivs,DateCreated)" +
                                "values('" + item.ClientId + "','" + item.EmpId + "'," + Month + ",'" + item.EmpDesg + "','" + contractId + "'," + item.NOD + "," + item.OT + "," + item.Penality + "," + item.CanAdv + "," + item.WO + "," + item.NHS + "," + item.Nposts + "," + item.Incentives + ",GETDATE() )";
                            }
                            var res = config.ExecuteNonQueryWithQueryAsync(query).Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        private static void EmpTransfer(EmpAttendance emp)
        {
            AppConfiguration config = new AppConfiguration();

            try
            {
                var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
                var ReaddMonth = Timings.Instance.GetReverseIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
                var jdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
                var rdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
                var odate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
                var query = string.Empty;
                var ordermax = "select max(cast(OrderId as int))+ 1 as ordercount from EmpPostingOrder";
                var orderdata = config.ExecuteReaderWithQueryAsync(ordermax).Result;
                var orderId = string.Empty;
                if (orderdata.Rows.Count > 0)
                {
                    orderId = orderdata.Rows[0]["ordercount"].ToString();
                }
                query = " Insert into EmpPostingOrder(EmpId,OrderId,OrderDt,JoiningDt,Desgn,TransferType,PF,ESI,PT,tounitId)" +
                "values('" + emp.EmpId + "','" + orderId + "','" + odate + "','" + jdate + "','" + emp.EmpDesg + "'," + emp.TransferType +
                "," + (emp.PF ? 1 : 0) + "," + (emp.ESI ? 1 : 0) + "," + (emp.PT ? 1 : 0) + ",'" + emp.ClientId + "')";
                var res = config.ExecuteNonQueryWithQueryAsync(query).Result;
            }
            catch
            { }
        }

        [WebMethod]
        public static string DeleteAttendance(string empId, string empDesgId, string clientId, string month)
        {
            AppConfiguration config = new AppConfiguration();

            string JoiningDate = DateTime.Now.Date.ToString();
            string OrderedDAte = DateTime.Now.Date.ToString();
            string RelievingDate = DateTime.Now.Date.ToString();
            var LastDate = DateTime.Now.Date;
            var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            //var RelMonth = Timings.Instance.GetReverseIdForSelectedMonth(Convert.ToInt32(month));
            try
            {
                var deletequery = "delete from EmpAttendance where [MONTH] = " + Month + " and EmpId = '" + empId + "' and ClientId = '" + clientId + "' and Design = '" + empDesgId + "'";
                var updatequery = "update EmpPostingOrder set RelieveMonth = " + Month + " where EmpId = '" + empId + "' and ToUnitId = '" + clientId + "' and Desgn = '" + empDesgId + "'";
                var res = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
                var res1 = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
            }
            catch (Exception ex)
            {
            }
            return "";
        }
    }





    public class EmpAttendance
    {
        public bool NewAdd { get; set; }
        public string EmpId { get; set; }
        public string EmpDesg { get; set; }
        public string ClientId { get; set; }
        public string JoiningDate { get; set; }
        public string RelievingDate { get; set; }
        public bool PF { get; set; }
        public bool PT { get; set; }
        public bool ESI { get; set; }
        public int TransferType { get; set; }
        public int MonthIndex { get; set; }
        public decimal NOD { get; set; }
        public decimal OT { get; set; }
        public int OTtype { get; set; }
        public decimal WO { get; set; }
        public decimal NHS { get; set; }
        public decimal Nposts { get; set; }
        public decimal CanAdv { get; set; }
        public decimal Penality { get; set; }
        public decimal Incentives { get; set; }
    }
}