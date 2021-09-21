using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using SRF.P.DAL;

namespace SRF.P.Module_Reports
{
    public partial class PTReport : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();


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

                    LoadClientList();
                    LoadClientNames();

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
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void LoadClientNames()
        {
            DataTable DtClientids = GlobalData.Instance.LoadCNames(CmpIDPrefix);
            if (DtClientids.Rows.Count > 0)
            {
                ddlcname.DataValueField = "Clientid";
                ddlcname.DataTextField = "clientname";
                ddlcname.DataSource = DtClientids;
                ddlcname.DataBind();
            }
            ddlcname.Items.Insert(0, "-Select-");
            ddlcname.Items.Insert(1, "ALL");

        }

        protected void LoadClientList()
        {
            DataTable DtClientNames = GlobalData.Instance.LoadCIds(CmpIDPrefix);
            if (DtClientNames.Rows.Count > 0)
            {
                ddlclientid.DataValueField = "Clientid";
                ddlclientid.DataTextField = "Clientid";
                ddlclientid.DataSource = DtClientNames;
                ddlclientid.DataBind();
            }
            ddlclientid.Items.Insert(0, "-Select-");
            ddlclientid.Items.Insert(1, "ALL");
        }

        protected void ddlcname_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlcname.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlclientid.SelectedValue = ddlcname.SelectedValue;

            }
            else
            {
                ddlclientid.SelectedIndex = 0;
            }
        }

        protected void ddlclientid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlclientid.SelectedIndex > 0)
            {
                txtmonth.Text = "";
                ddlcname.SelectedValue = ddlclientid.SelectedValue;
            }
            else
            {
                ddlcname.SelectedIndex = 0;
            }
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();



            if (txtmonth.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('Please Select Month');", true);

                return;
            }
            loadallsalarydetails();
        }

        protected void loadallsalarydetails()
        {
            string Clientid = ddlclientid.SelectedValue;
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();
            string sqlqry = string.Empty;





            sqlqry = "Select  EP.Empid,(E.Empfname + E.EmpMname + E.emplname) as Empname ," +
                     " EP.Desgn, " +
                     " round(EP.Proftax,0) as Proftax, " +
                     " round(EP.Gross,0) as  Gross,  round(EP.otamt,0) as  otamt,round(EP.WOAmt,0) as  WOAmt,round(EP.Nhsamt,0) as  Nhsamt,round(EP.Npotsamt,0) as  Npotsamt,  round(EP.pf,0) as  pf,  " +
                     " (round(EP.Gross,0)+ round(EP.otamt,0) + round(EP.WOAmt,0) + round(EP.Nhsamt,0)+ round(EP.Npotsamt,0) ) as actualamount from  emppaysheet EP " +
                     " inner join  empdetails E on E.Empid=EP.Empid   " +
                     " and    EP.Proftax >0  and EP.month='" + month + Year.Substring(2, 2) + " ' ";


            Bindata(sqlqry);
            return;



        }

        protected void Bindata(string Sqlqry)
        {
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                GVListEmployees.DataSource = dt;
                GVListEmployees.DataBind();
                lbtn_Export_PDF.Visible = true;
                lbtn_Export.Visible = true;
            }
            else
            {
                GVListEmployees.DataSource = null;
                GVListEmployees.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showlalert", "alert('No records found');", true);

            }
        }



        protected void ClearData()
        {
            LblResult.Text = "";
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
            lbtn_Export.Visible = false;
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("PTReport.xls", this.GVListEmployees);
        }

        public string GetMonthName()
        {
            string monthname = string.Empty;
            int payMonth = 0;
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();

            DateTime date = Convert.ToDateTime(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            monthname = mfi.GetMonthName(date.Month).ToString();
            //payMonth = GetMonth(monthname);

            return monthname;
        }

        public string GetMonthOfYear()
        {
            string MonthYear = "";

            int month = GetMonthBasedOnSelectionDateorMonth();
            if (month.ToString().Length == 4)
            {
                MonthYear = "20" + month.ToString().Substring(2, 2);
            }
            if (month.ToString().Length == 3)
            {
                MonthYear = "20" + month.ToString().Substring(1, 2);

            }
            return MonthYear;
        }

        public int GetMonthBasedOnSelectionDateorMonth()
        {

            var testDate = 0;
            string EnteredDate = "";

            #region Validation

            if (txtmonth.Text.Trim().Length > 0)
            {

                try
                {

                    testDate = GlobalData.Instance.CheckEnteredDate(txtmonth.Text);
                    if (testDate > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                        return 0;
                    }
                    EnteredDate = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb")).ToString();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('You Are Entered Invalid  DATE.Date Format Should be [DD/MM/YYYY].Ex.01/01/1990');", true);
                    return 0;
                }
            }
            #endregion


            #region  Month Get Based on the Control Selection
            int month = 0;

            DateTime date = DateTime.Parse(txtmonth.Text, CultureInfo.GetCultureInfo("en-gb"));
            month = Timings.Instance.GetIdForEnteredMOnth(date);
            return month;



            #endregion
        }

        protected void lbtn_Export_PDF_Click(object sender, EventArgs e)
        {
            uint Fontsize = 10;
            string FontStyle = "Tahoma";

            DataTable dt = null;
            String Sqlqry = "";
            string date = string.Empty;

            if (txtmonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtmonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
            }

            string month = DateTime.Parse(date).Month.ToString();
            string Year = DateTime.Parse(date).Year.ToString();


            Sqlqry = "Select  EP.Empid,(E.Empfname + E.EmpMname + E.emplname) as Empname ," +
                         " EP.Desgn, " +
                         " round(EP.Proftax,0) as Proftax, " +
                         " round(EP.Gross+ep.otamt,0) as  Gross,  round(EP.otamt,0) as  otamt,round(EP.WOAmt,0) as  WOAmt,round(EP.Nhsamt,0) as  Nhsamt,round(EP.Npotsamt,0) as  Npotsamt,  round(EP.pf,0) as  pf,  " +
                         " (round(EP.Gross,0)+ round(EP.otamt,0) + round(EP.WOAmt,0) + round(EP.Nhsamt,0)+ round(EP.Npotsamt,0) ) as actualamount from  emppaysheet EP " +
                         " inner join  empdetails E on E.Empid=EP.Empid   " +
                         " and    EP.Proftax >0  and EP.month='" + month + Year.Substring(2, 2) + " ' ";

            dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;

            int j = 1;
            string empid = "";
            string Name = "";
            float PTGross = 0;
            float PT = 0;
            float TotalPTGross = 0;
            float TotalPT = 0;

            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A4);

                var writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                string imagepath1 = Server.MapPath("images");

                int totalfonts = FontFactory.RegisterDirectory("c:\\WINDOWS\\fonts");
                StringBuilder sa = new StringBuilder();
                foreach (string fontname in FontFactory.RegisteredFonts)
                {
                    sa.Append(fontname + "\n");
                }

                //document.Add(new Paragraph(sa.ToString()));

                string fontpath = Server.MapPath(".");

                Font FontStyle1 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 10, Font.BOLD, BaseColor.BLACK);
                Font FontStyle2 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 10, Font.BOLD, BaseColor.BLACK);
                Font FontStyle3 = FontFactory.GetFont("Tahoma", BaseFont.CP1252, BaseFont.EMBEDDED, 10, Font.NORMAL, BaseColor.BLACK);


                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 500f;
                table.HeaderRows = 3;
                table.LockedWidth = true;
                float[] width = new float[] { 2f, 3f, 5f, 3f, 3f };
                table.SetWidths(width);

                PdfPCell cellspace = new PdfPCell(new Phrase("  ", FontStyle2));
                cellspace.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cellspace.Colspan = 6;
                cellspace.Border = 0;
                cellspace.PaddingTop = 0;


                PdfPCell cellcompanyname = new PdfPCell(new Phrase("EAGLE DETECTIVE AGENCY PVT LTD", FontStyle1));
                cellcompanyname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cellcompanyname.Colspan = 3;
                cellcompanyname.Border = 0;
                cellcompanyname.PaddingBottom = 5;
                table.AddCell(cellcompanyname);

                PdfPCell EmplPF = new PdfPCell(new Phrase("Print Date : " + DateTime.Now.ToString("dd/MM/yyyy"), FontStyle1));
                EmplPF.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                EmplPF.Colspan = 2;
                EmplPF.Border = 0;
                EmplPF.PaddingBottom = 5;
                table.AddCell(EmplPF);


                PdfPCell PFReport = new PdfPCell(new Phrase("PT Detailed report for the month of " + GetMonthName() + "/" + GetMonthOfYear(), FontStyle1));
                PFReport.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                PFReport.Colspan = 5;
                PFReport.BorderWidthLeft = 0;
                PFReport.BorderWidthRight = 0;
                PFReport.BorderWidthTop = 0.5f;
                PFReport.BorderWidthBottom = 0.5f;
                PFReport.PaddingTop = 5;
                PFReport.PaddingBottom = 5;
                table.AddCell(PFReport);




                /////

                PdfPCell CellSlNo = new PdfPCell(new Phrase("Sl.No.", FontStyle2));
                CellSlNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellSlNo.BorderWidthTop = 0.5f;
                CellSlNo.BorderWidthLeft = 0;
                CellSlNo.BorderWidthRight = 0;
                CellSlNo.BorderWidthBottom = 0.5f;
                CellSlNo.PaddingTop = 3;
                CellSlNo.PaddingBottom = 3;
                table.AddCell(CellSlNo);


                PdfPCell CellPFNo = new PdfPCell(new Phrase("Emp ID", FontStyle2));
                CellPFNo.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CellPFNo.BorderWidthTop = 0.5f;
                CellPFNo.BorderWidthLeft = 0;
                CellPFNo.BorderWidthRight = 0;
                CellPFNo.BorderWidthBottom = 0.5f;
                CellPFNo.PaddingTop = 3;
                CellPFNo.PaddingBottom = 3;
                table.AddCell(CellPFNo);

                PdfPCell CellName = new PdfPCell(new Phrase("Employee Name", FontStyle2));
                CellName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                CellName.BorderWidthTop = 0.5f;
                CellName.PaddingTop = 0;
                CellName.BorderWidthLeft = 0;
                CellName.BorderWidthRight = 0;
                CellName.BorderWidthBottom = 0.5f;
                CellName.PaddingTop = 3;
                CellName.PaddingBottom = 3;
                table.AddCell(CellName);


                PdfPCell CellESIEarnings = new PdfPCell(new Phrase("PT Gross", FontStyle2));
                CellESIEarnings.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                CellESIEarnings.Border = 0;
                CellESIEarnings.BorderWidthTop = 0.5f;
                CellESIEarnings.BorderWidthLeft = 0;
                CellESIEarnings.BorderWidthRight = 0;
                CellESIEarnings.BorderWidthBottom = 0.5f;
                CellESIEarnings.PaddingTop = 3;
                CellESIEarnings.PaddingBottom = 3;
                table.AddCell(CellESIEarnings);


                PdfPCell CellESIContribution = new PdfPCell(new Phrase("PT Deducted", FontStyle2));
                CellESIContribution.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                CellESIContribution.BorderWidthLeft = 0;
                CellESIContribution.BorderWidthRight = 0;
                CellESIContribution.BorderWidthTop = 0.5f;
                CellESIContribution.BorderWidthBottom = 0.5f;
                CellESIContribution.PaddingTop = 3;
                CellESIContribution.PaddingBottom = 3;
                table.AddCell(CellESIContribution);







                for (int k = 0; k < dt.Rows.Count; k++)
                {

                    empid = dt.Rows[k]["empid"].ToString();
                    Name = dt.Rows[k]["Empname"].ToString();
                    PTGross = Convert.ToSingle(dt.Rows[k]["gross"].ToString());
                    PT = Convert.ToSingle(dt.Rows[k]["Proftax"].ToString());

                    PdfPCell CellSlNo3 = new PdfPCell(new Phrase(j.ToString(), FontStyle3));
                    CellSlNo3.HorizontalAlignment = 1; //0=Left, 1=Centre, 3=Right
                    CellSlNo3.Border = 0;
                    CellSlNo3.PaddingTop = 0;
                    CellSlNo3.PaddingBottom = 3;
                    table.AddCell(CellSlNo3);


                    PdfPCell CellPFNo3 = new PdfPCell(new Phrase(empid, FontStyle3));
                    CellPFNo3.HorizontalAlignment = 1; //0=Left, 1=Centre, 3=Right
                    CellPFNo3.Border = 0;
                    CellPFNo3.PaddingTop = 0;
                    CellPFNo3.PaddingBottom = 3;
                    table.AddCell(CellPFNo3);

                    PdfPCell CellName3 = new PdfPCell(new Phrase(Name, FontStyle3));
                    CellName3.HorizontalAlignment = 0; //0=Left, 1=Centre, 3=Right
                    CellName3.Border = 0;
                    CellName3.BorderWidthRight = 0;
                    CellName3.PaddingBottom = 3;
                    CellName3.PaddingTop = 0;
                    table.AddCell(CellName3);




                    PdfPCell CellPFEarnings3 = new PdfPCell(new Phrase(PTGross.ToString("0.00"), FontStyle3));
                    CellPFEarnings3.HorizontalAlignment = 2; //0=Left, 1=Centre, 3=Right
                    CellPFEarnings3.Border = 0;
                    CellPFEarnings3.PaddingTop = 0;
                    CellPFEarnings3.PaddingBottom = 3;
                    table.AddCell(CellPFEarnings3);
                    TotalPTGross += PTGross;

                    PdfPCell CellPFContribution3 = new PdfPCell(new Phrase(PT.ToString("0.00"), FontStyle3));
                    CellPFContribution3.HorizontalAlignment = 2; //0=Left, 1=Centre, 3=Right
                    CellPFContribution3.Border = 0;
                    CellPFContribution3.PaddingTop = 0;
                    CellPFContribution3.PaddingBottom = 3;
                    table.AddCell(CellPFContribution3);
                    TotalPT += PT;

                    j++;
                }



                PdfPCell CellSlNo4 = new PdfPCell(new Phrase("Grand Total", FontStyle2));
                CellSlNo4.HorizontalAlignment = 0; //0=Left, 1=Centre, 4=Right
                CellSlNo4.BorderWidthLeft = 0;
                CellSlNo4.BorderWidthRight = 0;
                CellSlNo4.Colspan = 2;
                CellSlNo4.PaddingTop = 3;
                CellSlNo4.PaddingBottom = 3;
                table.AddCell(CellSlNo4);




                PdfPCell CellName4 = new PdfPCell(new Phrase("", FontStyle2));
                CellName4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellName4.BorderWidthLeft = 0;
                CellName4.BorderWidthRight = 0;
                CellName4.PaddingTop = 3;
                CellName4.PaddingBottom = 3;
                table.AddCell(CellName4);



                PdfPCell CellPFEarnings4 = new PdfPCell(new Phrase(TotalPTGross.ToString("0.00"), FontStyle2));
                CellPFEarnings4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellPFEarnings4.BorderWidthLeft = 0;
                CellPFEarnings4.BorderWidthRight = 0;
                CellPFEarnings4.PaddingTop = 3;
                CellPFEarnings4.PaddingBottom = 3;
                table.AddCell(CellPFEarnings4);


                PdfPCell CellPFContribution4 = new PdfPCell(new Phrase(TotalPT.ToString("0.00"), FontStyle2));
                CellPFContribution4.HorizontalAlignment = 2; //0=Left, 1=Centre, 4=Right
                CellPFContribution4.BorderWidthLeft = 0;
                CellPFContribution4.BorderWidthRight = 0;
                CellPFContribution4.PaddingTop = 3;
                CellPFContribution4.PaddingBottom = 3;
                table.AddCell(CellPFContribution4);


                document.Add(table);



                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Consolidated.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

            }

        }

        float Totalgross = 0;
        float TotalOTamt = 0;
        float TotalNpotsAmt = 0;
        float TotalWOAmt = 0;
        float TotalNHsAmt = 0;
        float TotalActualAmt = 0;
        float TotalPT = 0;

        protected void GVListEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("class", "text");

                float Gross = float.Parse(((Label)e.Row.FindControl("lblgross")).Text);
                Totalgross += Gross;

                //float OTamt = float.Parse(((Label)e.Row.FindControl("lblotamt")).Text);
                //TotalOTamt += OTamt;

                //float NpotsAmt = float.Parse(((Label)e.Row.FindControl("lblnpotsAmt")).Text);
                //TotalNpotsAmt += NpotsAmt;

                //float WOAmt = float.Parse(((Label)e.Row.FindControl("lblwoamt")).Text);
                //TotalWOAmt += WOAmt;

                //float NHsAmt = float.Parse(((Label)e.Row.FindControl("lblnhsamt")).Text);
                //TotalNHsAmt += NHsAmt;

                //float ActualAmt = float.Parse(((Label)e.Row.FindControl("lblActualamt")).Text);
                //TotalActualAmt += ActualAmt;

                float PT = float.Parse(((Label)e.Row.FindControl("lblPT")).Text);
                TotalPT += PT;


            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {

                Label lblTotalgross = (Label)e.Row.FindControl("lblTotalgross") as Label;
                lblTotalgross.Text = Totalgross.ToString();

                //Label lblTotalotamt = (Label)e.Row.FindControl("lblTotalotamt") as Label;
                //lblTotalotamt.Text = TotalOTamt.ToString();

                //Label lblTotalnpotsAmt = (Label)e.Row.FindControl("lblTotalnpotsAmt") as Label;
                //lblTotalnpotsAmt.Text = TotalNpotsAmt.ToString();

                //Label lblTotalwoamt = (Label)e.Row.FindControl("lblTotalwoamt") as Label;
                //lblTotalwoamt.Text = TotalWOAmt.ToString();

                //Label lblTotalnhsamt = (Label)e.Row.FindControl("lblTotalnhsamt") as Label;
                //lblTotalnhsamt.Text = TotalNHsAmt.ToString();

                //Label lblTotalActualamt = (Label)e.Row.FindControl("lblTotalActualamt") as Label;
                //lblTotalActualamt.Text = TotalActualAmt.ToString();

                Label lblTotalPT = (Label)e.Row.FindControl("lblTotalPT") as Label;
                lblTotalPT.Text = TotalPT.ToString();

            }

        }

    }
}