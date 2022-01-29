using System;
using System.Linq;
using System.Web.Services;
using KLTS.Data;
using System.Web.Script.Serialization;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Script.Services;
using SRF.P.DAL;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for FameService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class FameService : System.Web.Services.WebService
{

    private static string EmpIDPrefix = string.Empty;

    private static string CmpIDPrefix = string.Empty;

    private static DataTable _dtEmployees = new DataTable();

    AppConfiguration config = new AppConfiguration();

    private const string _attendanceQuery = @"select EA.EmpId,
			                   ISNULL(EmpFName,'')+' '+ISNULL(EmpMName,'')+' '+ISNULL(EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
			                    isnull(EA.NoOfDuties,0) as NOD ,
			                    isnull(EA.Ot,0) as OT ,
			                    isnull( EA.WO,0) as WO,
			                    isnull( EA.NHS,0) as NHS,
			                    isnull( EA.Npots,0) as NPots,
			                    isnull(EA.CanteenAdv,0) as CanAdv,
			                    isnull(EA.Penalty ,0) as Pen,
			                    isnull(EA.Incentivs,0) as Inctvs ,
                                isnull(EA.Leaves,0) as Leaves ,
                                case isnull(noofdays,0) when 0 then '##GEN##' when 1 then '##G_S##' when 2 then ('##GEN##'-4) else noofdays end noofdays
		                from EmpAttendance EA join contractdetailssw cdsw on cdsw.clientid=ea.clientid and ea.design=cdsw.designations and ea.contractid=cdsw.contractid join EmpDetails ED on Ed.EmpId=EA.EmpId join Designations D on D.DesignId=EA.Design 
		                and EA.ClientId='##CLIENTID##' and EA.Month=##MONTH##  and ( (ea.NoOfDuties+EA.Ot+EA.WO+EA.NHS+ EA.Npots)>0  or ed.empstatus=1) order by ea.empid";

    string _clientempsquery = @"select ep.EmpId,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
                               ep.relievemonth,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
			                   0 as CanAdv,
			                   0 as Pen,
			                   0 as Inctvs ,
                               0 as Leaves ,
                               case isnull(noofdays,0) when 0 then '##GEN##' when 1 then '##G_S##' when 2 then ('##GEN##'-4) else noofdays end noofdays
		                from EmpPostingOrder ep
                        join contractdetailssw cdsw on ep.tounitid = cdsw.clientid and cdsw.designations=ep.desgn
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and ed.empstatus=1 and ep.empid in (select empid from EmpAttendance where MONTH=##PREVMONTH## )
                        union
                        select ep.EmpId,
			                   ISNULL(ed.EmpFName,'')+' '+ISNULL(ed.EmpMName,'')+' '+ISNULL(ed.EmpLName,'') EmpName,
			                   d.DesignId as DesId,
			                   d.Design as DesName,
                               ep.relievemonth,
			                   0 as NOD,
			                   0 as OT,
			                   0 as WO,
			                   0 as NHS,
			                   0 as NPots,
			                   0 as CanAdv,
			                   0 as Pen,
			                   0 as Inctvs ,
                               0 as Leaves ,
                              case isnull(noofdays,0) when 0 then '##GEN##' when 1 then '##G_S##' when 2 then ('##GEN##'-4) else noofdays end noofdays
		                from EmpPostingOrder ep
                          join contractdetailssw cdsw on ep.tounitid = cdsw.clientid and cdsw.designations=ep.desgn
		                inner join EmpDetails ed on ep.EmpId = ed.EmpId
		                inner join Designations d on ep.Desgn = d.DesignId
		                where ToUnitId = '##CLIENTID##' and relievemonth is null and postedmonth=##MONTHNEW## and ed.empstatus=1 and ep.EmpId not in(select empid from EmpAttendance where MONTH='##PREVMONTH##' )";
    //and ep.EmpId not in (select EmpId from EmpAttendance where ClientId = '##CLIENTID##' and month = ##MONTH## and ContractId='##CONTRACTID##')";

    private const string _attendanceSummaryquery = @"select d.Design DesName,
	                                                           cast(sum(ea.NoOfDuties)as nvarchar) NODTotal,
	                                                           cast(sum(ea.OT)as nvarchar) OTTotal,
	                                                           cast(sum(ea.WO)as nvarchar) WOTotal,
	                                                           cast(sum(ea.NHS)as nvarchar) NHSTotal,
	                                                           cast(sum(ea.Npots)as nvarchar) NpotsTotal,
	                                                           cast(sum(ea.Penalty)as nvarchar) PenTotal,
	                                                           cast(sum(ea.Incentivs)as nvarchar) InctvsTotal,
	                                                           cast(sum(ea.CanteenAdv)as nvarchar) CanAdvTotal,
                                                               cast(isnull(sum(ea.Leaves),0) as nvarchar) LeavesTotal
                                                        from EmpAttendance ea 
                                                        inner join Designations d on d.DesignId = ea.Design
                                                        where ea.ClientId = '##CLIENTID##' and ea.[MONTH]= ##MONTH##
                                                        group by ea.Design,d.Design";

    public FameService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    public DataTable EmployeesDataTable
    {
        get
        {
            // if (_dtEmployees.Rows.Count < 1)
            {
                var dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix);
                _dtEmployees = dtEmployees;
            }
            return _dtEmployees;
        }
    }


    public void UpdateEmpDataTable()
    {
        _dtEmployees = GlobalData.Instance.LoadEmpNames(EmpIDPrefix);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetClientsData()
    {
        Context.Response.Clear();
        var result = "";
        string query = "select clientid,clientname,clientphonenumbers,ourcontactpersonid from clients where ClientId like '" + CmpIDPrefix + "%' and ClientName not LIKE '%/%' Order By  Clientname";
        var dtAllClients = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
        if (dtAllClients.Rows.Count > 0)
        {
            var obj = (from row in dtAllClients.AsEnumerable()
                       select new
                       {
                           ClientId = row.Field<string>("clientid"),
                           ClientName = Regex.Replace(row.Field<string>("clientname"), @"[^0-9a-zA-Z\x20]+", ""),
                           PhoneNumber = row.Field<string>("clientphonenumbers"),
                           ContactPerson = row.Field<string>("ourcontactpersonid")
                       }).ToList();
            result = new JavaScriptSerializer().Serialize(obj);

        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetEmployessData(string strid)
    {
        strid = strid == "0" ? "0000" : strid;
        Context.Response.Clear();
        var result = string.Empty;
        try
        {
            if (EmployeesDataTable.Rows.Count > 0)
            {
                var obj = (from row in EmployeesDataTable.AsEnumerable()
                           select new
                           {
                               EmpId = row.Field<string>("Empid"),
                               EmpName = Regex.Replace(row.Field<string>("FullName"), @"[^0-9a-zA-Z\x20]+", ""),
                               EmpDesg = row.Field<string>("Designation"),
                               empstatus = row.Field<bool>("empstatus")
                           }).ToList();

                obj = obj.Where(o => o.EmpId.Contains(strid.Trim())).OrderBy(o => o.EmpId).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetEmployessNameData(string strname)
    {
        Context.Response.Clear();
        var result = string.Empty;
        try
        {
            if (EmployeesDataTable.Rows.Count > 0)
            {
                var obj = (from row in EmployeesDataTable.AsEnumerable()
                           select new
                           {
                               EmpId = row.Field<string>("Empid"),
                               EmpName = Regex.Replace(row.Field<string>("FullName"), @"[^0-9a-zA-Z\x20]+", ""),
                               EmpDesg = row.Field<string>("Designation"),
                               empstatus = row.Field<bool>("empstatus")
                           }).ToList();

                obj = obj.Where(o => o.EmpName.Contains(strname.Trim())).OrderBy(o => o.EmpName).ToList();
                result = new JavaScriptSerializer().Serialize(obj);
            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", result.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(result);
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetAttendanceGrid(string clientId, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;
        try
        {

            //
            if (Chk == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month1 = DateTime.Parse(date).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();
                string olddate = month1 + Year1.Substring(2, 2);
                Month = Convert.ToInt32(olddate);
            }


            var monthval = "";
            var yearval = "";

            if (Month.ToString().Length == 3)
            {
                monthval = "0" + Month.ToString().Substring(0, 1);
                yearval = Month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = Month.ToString().Substring(0, 2);
                yearval = Month.ToString().Substring(2, 2);
            }

            int Selectdays = System.DateTime.DaysInMonth(int.Parse(yearval), int.Parse(monthval));
            string daten = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

            var PrevMonth = 0;

            if (Chk == false)
            {
                LastDate = DateTime.Parse(daten, CultureInfo.GetCultureInfo("en-gb"));
                PrevMonth = Timings.Instance.GetIdForSelectedMonthPrevious(Convert.ToInt32(month));

            }
            else
            {
                LastDate = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();

                string month1 = DateTime.Parse(date).AddMonths(-1).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();

                if (month1 == "12")
                {
                    Year1 = DateTime.Parse(date).AddYears(-1).Year.ToString();
                }

                string olddate = month1 + Year1.Substring(2, 2);
                PrevMonth = Convert.ToInt32(olddate);
            }


            var MonthNew = yearval + monthval.ToString();


            var strquery = "select contractid,paysheetdates from contracts where clientid= '" + clientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = config.ExecuteAdaptorAsyncWithQueryParams(strquery).Result;
            var contractId = string.Empty;

            var Gendays = 0;
            var G_Sdays = 0;
            var PaysheetDates = 0;

            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
                PaysheetDates = int.Parse(contractdata.Rows[0]["paysheetdates"].ToString());
            }

            if (Chk == false)
            {
                Gendays = Timings.Instance.GetNoofDaysForSelectedMonth(Convert.ToInt32(month), PaysheetDates);

            }

            //New Code when select month in Textbox
            if (Chk == true)
            {
                DateTime mGendays = DateTime.Now;
                DateTime dateM = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb"));
                mGendays = DateTime.Parse(dateM.ToString());
                Gendays = Timings.Instance.GetNoofDaysForEnteredMonth(mGendays, PaysheetDates);
            }
            G_Sdays = Timings.Instance.Get_GS_Days(Month, Gendays);


            if (string.IsNullOrEmpty(contractId))
            {
                result = "fail";
                resultobj = "Contract not available for this month.";
            }
            else
            {

                List<EmpAttendanceGrid> empdata = new List<EmpAttendanceGrid>();
                var fromattendancequery = _attendanceQuery.Replace("##MONTH##", Month.ToString())
                                 .Replace("##CLIENTID##", clientId)
                                 .Replace("##GEN##", Gendays.ToString())
                                 .Replace("##G_S##", G_Sdays.ToString());
                // .Replace("##CONTRACTID##", contractId);
                var attData = config.ExecuteAdaptorAsyncWithQueryParams(fromattendancequery).Result;

                if (attData.Rows.Count > 0)
                {
                    var obj = (from row in attData.AsEnumerable()
                               select new EmpAttendanceGrid()
                               {
                                   EmpId = row.Field<string>("EmpId"),
                                   EmpName = Regex.Replace(row.Field<string>("EmpName"), @"[^0-9a-zA-Z\x20]+", ""),
                                   DesgId = row.Field<int>("DesId"),
                                   DesgName = row.Field<string>("DesName"),
                                   noofdays = row.Field<float>("noofdays"),
                                   NoOfDuties = row.Field<float>("NOD"),
                                   OT = row.Field<float>("OT"),
                                   WO = row.Field<float>("WO"),
                                   NHS = row.Field<float>("NHS"),
                                   NPosts = row.Field<float>("NPots"),
                                   CanteenAdv = row.Field<float>("CanAdv"),
                                   Penalty = row.Field<float>("Pen"),
                                   Incentivs = row.Field<float>("Inctvs"),
                                   Leaves = row.Field<float>("Leaves")
                               }).ToList();
                    empdata.AddRange(obj);
                }
                //Extra = row.Field<float>("Extra")

                var frompostingorderquery = _clientempsquery.Replace("##CLIENTID##", clientId)
                                                              .Replace("##PREVMONTH##", PrevMonth.ToString())
                                                               .Replace("##GEN##", Gendays.ToString())
                                                                    .Replace("##G_S##", G_Sdays.ToString())
                                                            .Replace("##MONTHNEW##", MonthNew.ToString());
                var postingData = config.ExecuteAdaptorAsyncWithQueryParams(frompostingorderquery).Result;
                if (postingData.Rows.Count > 0)
                {
                    foreach (DataRow item in postingData.Rows)
                    {
                        var inserttrue = false;
                        if (string.IsNullOrEmpty(item["relievemonth"].ToString().Trim()))
                        {
                            inserttrue = true;
                        }
                        else if (item["relievemonth"].ToString() != Month.ToString())
                        {
                            //dont display if relieve month is less than or equal to selected month 
                            var rmonth = Timings.Instance.GetDateWithMonthString(item["relievemonth"].ToString());
                            var smonth = Timings.Instance.GetDateWithMonthString(Month.ToString());
                            inserttrue = (rmonth > smonth);
                        }
                        var empalreadyexists = empdata.Where(e => e.EmpId == item["EmpId"].ToString()
                                && e.EmpName == item["EmpName"].ToString()
                                && e.DesgId == int.Parse(item["DesId"].ToString())).FirstOrDefault();

                        if (inserttrue && empalreadyexists == null)
                        {
                            empdata.Add(new EmpAttendanceGrid
                            {
                                EmpId = item["EmpId"].ToString(),
                                EmpName = Regex.Replace(item["EmpName"].ToString(), @"[^0-9a-zA-Z\x20]+", ""),
                                DesgId = int.Parse(item["DesId"].ToString()),
                                DesgName = item["DesName"].ToString(),
                                noofdays = float.Parse(item["noofdays"].ToString()),
                                NoOfDuties = float.Parse(item["NOD"].ToString()),
                                OT = float.Parse(item["OT"].ToString()),
                                WO = float.Parse(item["WO"].ToString()),
                                NHS = float.Parse(item["NHS"].ToString()),
                                NPosts = float.Parse(item["NPots"].ToString()),
                                CanteenAdv = float.Parse(item["CanAdv"].ToString()),
                                Penalty = float.Parse(item["Pen"].ToString()),
                                Incentivs = float.Parse(item["Inctvs"].ToString()),
                                Leaves = float.Parse(item["Leaves"].ToString())
                            });
                        }
                    }
                }
                //Extra = float.Parse(item["Extra"].ToString())

                if (empdata.Count > 0)
                {
                    resultobj = new JavaScriptSerializer().Serialize(empdata);
                    result = "success";
                }
                else
                {
                    result = "nodata";
                    resultobj = "Attendance Not Avaialable for  this month of the Selected client";
                }

            }
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var res = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(res);
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void GetAttendanceSummary(string clientId, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = string.Empty;
        var resultobj = string.Empty;
        DateTime LastDate = DateTime.Now;
        string date = "";
        var Month = 0;
        try
        {
            // var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));

            if (Chk == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
            }
            else
            {
                date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();
                string month1 = DateTime.Parse(date).Month.ToString();
                string Year1 = DateTime.Parse(date).Year.ToString();
                string olddate = month1 + Year1.Substring(2, 2);
                Month = Convert.ToInt32(olddate);
            }

            var monthval = "";
            var yearval = "";
            if (Month.ToString().Length == 3)
            {
                monthval = "0" + Month.ToString().Substring(0, 1);
                yearval = Month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = Month.ToString().Substring(0, 2);
                yearval = Month.ToString().Substring(2, 2);
            }

            int Selectdays = System.DateTime.DaysInMonth(int.Parse(yearval), int.Parse(monthval));
            string daten = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();


            var sx = _attendanceSummaryquery.Replace("##MONTH##", Month.ToString())
                                     .Replace("##CLIENTID##", clientId);
            var attData = config.ExecuteAdaptorAsyncWithQueryParams(sx).Result;
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
                               LeavesTotal = row.Field<string>("LeavesTotal"),
                           }).ToList();
                resultobj = new JavaScriptSerializer().Serialize(obj);
                result = "success";
            }
            else
            {
                result = "nodata";
            }
            //ExtraTotal = row.Field<string>("ExtraTotal"),

        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var res = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(res);
        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SaveAttendance(List<EmpAttendance> lst)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;
        string OrderedDAte = DateTime.Now.Date.ToString();
        // var LastDate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
        var Checkbox = false;
        Checkbox = Convert.ToBoolean((lst[0].Chkbox));
        DateTime LastDate = DateTime.Now;
        var Month = 0;
        try
        {

            if (Checkbox == false)
            {
                Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(lst[0].MonthIndex));
            }
            else
            {
                Month = Convert.ToInt32(lst[0].MonthIndex);
            }

            var monthval = "";
            var yearval = "";

            if (Month.ToString().Length == 3)
            {
                monthval = Month.ToString().Substring(0, 1);
                yearval = "20" + Month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = Month.ToString().Substring(0, 2);
                yearval = "20" + Month.ToString().Substring(2, 2);
            }

            int Selectdays = System.DateTime.DaysInMonth(int.Parse(yearval), int.Parse(monthval));
            string daten = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

            if (Checkbox == false)
            {
                LastDate = DateTime.Parse(daten, CultureInfo.GetCultureInfo("en-gb"));
            }
            else
            {
                LastDate = DateTime.Parse(daten, CultureInfo.GetCultureInfo("en-gb"));

            }
            var strquery = "select contractid from contracts where clientid= '" + lst[0].ClientId + "'  and '" + LastDate + "' between contractstartdate and contractenddate";
            var contractdata = config.ExecuteAdaptorAsyncWithQueryParams(strquery).Result;
            var contractId = string.Empty;
            if (contractdata.Rows.Count > 0)
            {
                contractId = contractdata.Rows[0]["contractid"].ToString();
            }
            if (string.IsNullOrEmpty(contractId))
            {
                result = "fail";
                resultobj = "Contract Id not available for the select Client.";
            }
            else
            {
                foreach (var item in lst)
                {
                    if (item.NewAdd) EmpTransfer(item);
                    var empquery = "select COUNT(*) as empcount from  Empattendance Where Empid = '" + item.EmpId +
                                "' and [month]= " + Month + " and ClientId = '" + item.ClientId +
                                "'  and  Design = " + item.EmpDesg + " ";
                    var empdata = config.ExecuteAdaptorAsyncWithQueryParams(empquery).Result;
                    var empcount = string.Empty;
                    if (empdata.Rows.Count > 0)
                    {
                        empcount = empdata.Rows[0]["empcount"].ToString();
                    }
                    var attendancetotal = item.NOD + item.OT + item.WO + item.NHS + item.Nposts + item.CanAdv + item.Penality;

                    var query = string.Empty;
                    if (Convert.ToInt32(empcount) > 0)
                    {
                        query = "update EmpAttendance set NoofDuties=" + item.NOD
                                            + ",OT=" + item.OT
                                            + ",Penalty=" + item.Penality
                                            + ",CanteenAdv=" + item.CanAdv
                                            + ",Incentivs=" + item.Incentives
                                             + ",Leaves=" + item.Leaves
                                            //+ ",Extra=" + item.Extra
                                            + ",Design='" + item.EmpDesg
                                            + "',WO=" + item.WO
                                            + ",NHS=" + item.NHS
                                            + ",NPOTS=" + item.Nposts
                                            + ", contractid= '" + contractId
                                            + "' Where empid='" + item.EmpId
                                            + "' and ClientId='" + item.ClientId
                                            + "' and [Month]=" + Month
                                            + " and  Design='" + item.EmpDesg
                                            + "' ";
                    }
                    else if (attendancetotal > 0)
                    {
                        //Extra,," + item.Extra + "
                        query = "insert  EmpAttendance(clientid,empid,[month],Design,contractId,NoofDuties,OT,Penalty,CanteenAdv,WO,NHS,NPOTS,Incentivs,Leaves,DateCreated)" +
                        "values('" + item.ClientId + "','" + item.EmpId + "'," + Month + ",'" + item.EmpDesg + "','" + contractId + "'," + item.NOD + "," + item.OT + "," + item.Penality + "," + item.CanAdv + "," + item.WO + "," + item.NHS + "," + item.Nposts + "," + item.Incentives + "," + item.Leaves + ",GETDATE() )";
                    }
                    if (!string.IsNullOrEmpty(query))
                    {
                        var res = config.ExecuteNonQueryWithQueryAsync(query).Result;
                    }
                    result = "success";
                }
            }
            resultobj = new JavaScriptSerializer().Serialize(new { Updated = lst.Count });
        }
        catch (Exception ex)
        {
            result = "fail";
            resultobj = ex.Message;
        }

        var resObj = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(resObj);

        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

    private void EmpTransfer(EmpAttendance emp)
    {
        try
        {
            var month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(emp.MonthIndex));
            var monthval = "";
            var yearval = "";
            if (month.ToString().Length == 3)
            {
                monthval = "0" + month.ToString().Substring(0, 1);
                yearval = month.ToString().Substring(1, 2);
            }
            else
            {
                monthval = month.ToString().Substring(0, 2);
                yearval = month.ToString().Substring(2, 2);
            }
            var jdate = DateTime.Parse(emp.JoiningDate, CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy-MM-dd hh:mm:ss");
            var odate = DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
            var query = string.Empty;
            var ordermax = "select max(cast(OrderId as int))+ 1 as ordercount from EmpPostingOrder";
            var orderdata = config.ExecuteAdaptorAsyncWithQueryParams(ordermax).Result;
            var orderId = string.Empty;
            if (orderdata.Rows.Count > 0)
            {
                orderId = orderdata.Rows[0]["ordercount"].ToString();
            }
            query = " Insert into EmpPostingOrder(EmpId,OrderId,OrderDt,JoiningDt,Desgn,TransferType,PF,ESI,PT,tounitId,postedmonth)" +
            "values('" + emp.EmpId + "','" + orderId + "','" + odate + "','" + jdate + "','" + emp.EmpDesg + "'," + emp.TransferType +
            "," + (emp.PF ? 1 : 0) + "," + (emp.ESI ? 1 : 0) + "," + (emp.PT ? 1 : 0) + ",'" + emp.ClientId + "','" + yearval + monthval + "')";
            var res = config.ExecuteNonQueryWithQueryAsync(query).Result;
        }
        catch
        { }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeleteAttendance(string empId, string empDesgId, string clientId, string month, bool Chk)
    {
        Context.Response.Clear();
        var result = "";
        var resultobj = string.Empty;
        string JoiningDate = DateTime.Now.Date.ToString();
        string OrderedDAte = DateTime.Now.Date.ToString();
        string RelievingDate = DateTime.Now.Date.ToString();
        var LastDate = DateTime.Now.Date;
        //var Month = Timings.Instance.GetIdForSelectedMonth(Convert.ToInt32(month));
        //var RelMonth = Timings.Instance.GetReverseIdForSelectedMonth(Convert.ToInt32(month));

        string date = "";
        var Month = 0;
        var monthval = "";
        var yearval = "";
        if (Month.ToString().Length == 3)
        {
            monthval = "0" + Month.ToString().Substring(0, 1);
            yearval = Month.ToString().Substring(1, 2);
        }
        else
        {
            monthval = Month.ToString().Substring(0, 2);
            yearval = Month.ToString().Substring(2, 2);
        }

        int Selectdays = System.DateTime.DaysInMonth(int.Parse(yearval), int.Parse(monthval));
        string daten = Selectdays.ToString() + "/" + monthval.ToString() + "/" + yearval.ToString();

        if (Chk == false)
        {
            date = DateTime.Parse(daten, CultureInfo.GetCultureInfo("en-gb")).ToString();
        }
        else
        {
            date = DateTime.Parse(month, CultureInfo.GetCultureInfo("en-gb")).ToString();
        }


        string month1 = DateTime.Parse(date).Month.ToString();
        string Year1 = DateTime.Parse(date).Year.ToString();
        string olddate = month1 + Year1.Substring(2, 2);
        Month = Convert.ToInt32(olddate);
        try
        {
            var deletequery = "delete from EmpAttendance where [MONTH] = " + Month + " and EmpId = '" + empId + "' and ClientId = '" + clientId + "' and Design = '" + empDesgId + "'";
            var updatequery = "update EmpPostingOrder set RelieveMonth = " + Month + " where EmpId = '" + empId + "' and ToUnitId = '" + clientId + "' and Desgn = '" + empDesgId + "'";
            var res = config.ExecuteNonQueryWithQueryAsync(deletequery).Result;
            var res1 = config.ExecuteNonQueryWithQueryAsync(updatequery).Result;
            resultobj = new JavaScriptSerializer().Serialize(new { Delete = res, Update = res1 });
            result = "success";
        }
        catch (Exception ex)
        {
            result = "fail";
        }
        var resObj = new { msg = result, Obj = resultobj };
        resultobj = new JavaScriptSerializer().Serialize(resObj);

        Context.Response.ContentType = "application/json";
        Context.Response.AddHeader("content-disposition", "attachment; filename=export.json");
        Context.Response.AddHeader("content-length", resultobj.Length.ToString());
        Context.Response.Flush();
        Context.Response.Write(resultobj);
    }

}

public class EmpAttendanceGrid
{
    public string EmpId { get; set; }
    public string EmpName { get; set; }
    public int DesgId { get; set; }
    public string DesgName { get; set; }
    public float NoOfDuties { get; set; }
    public float OT { get; set; }
    public float WO { get; set; }
    public float NHS { get; set; }
    public float NPosts { get; set; }
    public float CanteenAdv { get; set; }
    public float Penalty { get; set; }
    public float Incentivs { get; set; }
    public bool empstatus { get; set; }
    public float Leaves { get; set; }
    public float noofdays { get; set; }
    // public float Extra { get; set; }
    public bool Chkbox { get; set; }

}


public class EmpAttendance
{
    public bool NewAdd { get; set; }
    public bool IsOld { get; set; }
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
    public decimal Leaves { get; set; }
    public float noofdays { get; set; }
    // public decimal Extra { get; set; }
    public bool Chkbox { get; set; }

}


