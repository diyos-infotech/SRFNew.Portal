using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Globalization;
using SRF.P.DAL;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using System.Web;
using iTextSharp.text;

namespace SRF.P.Module_Reports
{
    public partial class PinMyVisits : System.Web.UI.Page
    {
        GridViewExportUtil GVUtil = new GridViewExportUtil();
        AppConfiguration config = new AppConfiguration();
        DataTable dt;
        string EmpIDPrefix = "";
        string CmpIDPrefix = "";
        string BranchID = "";
        string companyid = "";
        //page changes done by Mahesh Goud on 2022-07-06

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

                    FillEmpIDList();
                    LoadClientList();
                    LoadClientNames();
                    LoadActivityDropdown();
                }

                string query = "select * from AndroidCompanyDetails";
                DataTable dtAndroidCompanyDetails = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
                if (dtAndroidCompanyDetails.Rows.Count > 0)
                {
                    companyid = dtAndroidCompanyDetails.Rows[0]["CompanyId"].ToString();
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


        protected void FillEmpIDList()
        {

            string Sqlqry = "Select Empid From Empdetails    where Empstatus=1 Order by  EmpId";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                ddlEmpid.DataValueField = "Empid";
                ddlEmpid.DataTextField = "Empid";
                ddlEmpid.DataSource = dt;
                ddlEmpid.DataBind();
            }
            ddlEmpid.Items.Insert(0, "--Select--");
            ddlEmpid.Items.Insert(1, "ALL");

        }

        protected void LoadClientNames()
        {
            string query = "select distinct clientid,clientname from clients where clientstatus=1";
            DataTable DtClientids = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (DtClientids.Rows.Count > 0)
            {
                ddlCName.DataValueField = "Clientid";
                ddlCName.DataTextField = "clientname";
                ddlCName.DataSource = DtClientids;
                ddlCName.DataBind();
            }
            ddlCName.Items.Insert(0, "-Select-");
            ddlCName.Items.Insert(1, "ALL");

        }

        protected void LoadActivityDropdown()
        {
            string query = "select * from ActivityDropdown";
            DataTable DtopmEmpsIDs = config.PocketFameExecuteAdaptorAsyncWithQueryParams(query).Result;

            if (DtopmEmpsIDs.Rows.Count > 0)
            {
                ddlFOID.DataValueField = "ActivityId";
                ddlFOID.DataTextField = "ActivityName";
                ddlFOID.DataSource = DtopmEmpsIDs;
                ddlFOID.DataBind();
            }
            ddlFOID.Items.Insert(0, "-Select-");
            ddlFOID.Items.Insert(1, "ALL");
        }

        protected void LoadClientList()
        {
            string query = "select distinct clientid from clients where clientstatus=1";
            DataTable DtClientNames = config.ExecuteAdaptorAsyncWithQueryParams(query).Result;
            if (DtClientNames.Rows.Count > 0)
            {
                ddlClientID.DataValueField = "clientid";
                ddlClientID.DataTextField = "clientid";
                ddlClientID.DataSource = DtClientNames;
                ddlClientID.DataBind();
            }
            ddlClientID.Items.Insert(0, "-Select-");
            ddlClientID.Items.Insert(1, "ALL");
        }

        protected void ddlCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddlCName.SelectedIndex > 0)
            {
                txtMonth.Text = "";
                ddlClientID.SelectedValue = ddlCName.SelectedValue;

            }
            else
            {
                ddlClientID.SelectedIndex = 0;
            }
        }

        protected void ddlClientID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlClientID.SelectedIndex > 0)
            {
                txtMonth.Text = "";
                ddlCName.SelectedValue = ddlClientID.SelectedValue;
            }
            else
            {
                ddlCName.SelectedIndex = 0;
            }
        }

        protected void ClearData()
        {
            GVpinmyvisit.DataSource = null;
            GVpinmyvisit.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string date = string.Empty;
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            string Day = "";
            string month = "";
            string Year = "";


            if (ddltype.SelectedIndex == 0 || ddltype.SelectedIndex == 1)
            {
                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }

                Day = DateTime.Parse(date).Day.ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();
            }
            else
            {
                if (txtfrom.Text.Trim().Length > 0)
                {
                    Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    Todate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
            }


            string Empid = "";
            string Clientid = "";
            string Activity = "";

            if (ddloption.SelectedIndex == 0)
            {
                if (ddlEmpid.SelectedIndex == 1)
                {
                    Empid = "%";
                }
                else
                {
                    Empid = ddlEmpid.SelectedValue;
                }
            }

            if (ddloption.SelectedIndex == 1)
            {
                if (ddlClientID.SelectedIndex == 1)
                {
                    Clientid = "%";
                }
                else
                {
                    Clientid = ddlClientID.SelectedValue;
                }
            }
            else if (ddloption.SelectedIndex == 2)
            {
                if (ddlFOID.SelectedIndex == 1)
                {
                    Activity = "%";
                }
                else
                {
                    Activity = ddlFOID.SelectedItem.Text;
                }

            }


            string Spname = "GetPinMyvisitImages";
            Hashtable ht = new Hashtable();
            ht.Add("@Day", Day);
            ht.Add("@month", month);
            ht.Add("@Year", Year);
            ht.Add("@CompanyID", companyid);
            ht.Add("@Empid", Empid);
            ht.Add("@Clientid", Clientid);
            ht.Add("@Type", "GetData");
            ht.Add("@Option", ddloption.SelectedIndex);
            ht.Add("@fromDate", Fromdate);
            ht.Add("@ToDate", Todate);
            ht.Add("@Activity", Activity);
            ht.Add("@typewise", ddltype.SelectedIndex);
            DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;
            if (dt.Rows.Count > 0)
            {
                GVpinmyvisit.DataSource = dt;
                GVpinmyvisit.DataBind();



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Button btnview = GVpinmyvisit.Rows[i].FindControl("btnview") as Button;

                    if (dt.Rows[i]["pitstopImage"].ToString() != "0")
                    {
                        btnview.Visible = true;
                    }
                    else
                    {
                        btnview.Visible = false;

                    }

                }



            }
            else
            {
                GVpinmyvisit.DataSource = null;
                GVpinmyvisit.DataBind();
            }
        }

        public string Getmonthval()
        {
            string date = string.Empty;
            string month = "";
            string Year = "";
            string monthval = "";


            if (txtMonth.Text.Trim().Length > 0)
            {
                date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();
                monthval = month + Year.Substring(2, 2);
            }

            return monthval;

        }


        protected void gvdata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //add the thead and tbody section programatically
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnGetImage_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < GVpinmyvisit.Rows.Count; i++)
            {
                Label lblUpdatedOn = GVpinmyvisit.Rows[i].FindControl("lblUpdatedOn") as Label;
                Label lblUpdatedBy = GVpinmyvisit.Rows[i].FindControl("lblUpdatedBy") as Label;

                txtMonth.Text = lblUpdatedOn.Text;

                string date = string.Empty;

                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }

                string Day = DateTime.Parse(date).Day.ToString();
                string month = DateTime.Parse(date).Month.ToString();
                string Year = DateTime.Parse(date).Year.ToString();


                string Empid = "";


                Empid = lblUpdatedBy.Text;

                string Spname = "GetPinMyvisitImages";

                Hashtable ht = new Hashtable();
                ht.Add("@Day", Day);
                ht.Add("@month", month);
                ht.Add("@Year", Year);
                ht.Add("@CompanyID", companyid);
                ht.Add("@Empid", Empid);
                ht.Add("@PitstopAttachmentId", hfPitstopAttachmentId.Value);
                ht.Add("@Type", "GetImageData");

                DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["pitstopImage"].ToString().Length > 0)
                    {

                        if (dt.Rows[0]["pitstopImage"].ToString().StartsWith("data"))
                        {
                            imgphoto.ImageUrl = dt.Rows[0]["pitstopImage"].ToString();
                        }
                        else
                        {
                            imgphoto.ImageUrl = "data:image/jpeg;base64," + dt.Rows[0]["pitstopImage"].ToString();
                        }

                    }



                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
                }
            }
        }

        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            string date = string.Empty;
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            string Day = "";
            string month = "";
            string Year = "";


            if (ddltype.SelectedIndex == 0 || ddltype.SelectedIndex == 1)
            {
                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }

                Day = DateTime.Parse(date).Day.ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();
            }
            else
            {
                if (txtfrom.Text.Trim().Length > 0)
                {
                    Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    Todate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
            }


            string Empid = "";
            string Clientid = "";
            string Activity = "";

            if (ddloption.SelectedIndex == 0)
            {
                if (ddlEmpid.SelectedIndex == 1)
                {
                    Empid = "%";
                }
                else
                {
                    Empid = ddlEmpid.SelectedValue;
                }
            }

            if (ddloption.SelectedIndex == 1)
            {
                if (ddlClientID.SelectedIndex == 1)
                {
                    Clientid = "%";
                }
                else
                {
                    Clientid = ddlClientID.SelectedValue;
                }
            }
            else if (ddloption.SelectedIndex == 2)
            {
                if (ddlFOID.SelectedIndex == 1)
                {
                    Activity = "%";
                }
                else
                {
                    Activity = ddlFOID.SelectedItem.Text;
                }
            }

            string DType = "Excel";



            string Spname = "GetPinMyvisitImages";
            Hashtable ht = new Hashtable();
            ht.Add("@Day", Day);
            ht.Add("@month", month);
            ht.Add("@Year", Year);
            ht.Add("@CompanyID", companyid);
            ht.Add("@Empid", Empid);
            ht.Add("@Type", "GetData");
            ht.Add("@DType", DType);
            ht.Add("@Option", ddloption.SelectedIndex);
            ht.Add("@fromDate", Fromdate);
            ht.Add("@ToDate", Todate);
            ht.Add("@Clientid", Clientid);
            ht.Add("@Activity", Activity);
            ht.Add("@typewise", ddltype.SelectedIndex);


            DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;
            if (dt.Rows.Count > 0)
            {
                GVUtil.NewExportExcel("PinMyvisit.xlsx", dt);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string FontStyle = "calibri";

            string date = string.Empty;
            string Fromdate = string.Empty;
            string Todate = string.Empty;

            string Day = "";
            string month = "";
            string Year = "";


            if (ddltype.SelectedIndex == 0 || ddltype.SelectedIndex == 1)
            {
                if (txtMonth.Text.Trim().Length > 0)
                {
                    date = DateTime.Parse(txtMonth.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }

                Day = DateTime.Parse(date).Day.ToString();
                month = DateTime.Parse(date).Month.ToString();
                Year = DateTime.Parse(date).Year.ToString();
            }
            else
            {
                if (txtfrom.Text.Trim().Length > 0)
                {
                    Fromdate = DateTime.Parse(txtfrom.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
                if (txtto.Text.Trim().Length > 0)
                {
                    Todate = DateTime.Parse(txtto.Text.Trim(), CultureInfo.GetCultureInfo("en-gb")).ToString("yyyy/MM/dd");
                }
            }


            string Empid = "";
            string Clientid = "";
            string Activity = "";

            if (ddloption.SelectedIndex == 0)
            {
                if (ddlEmpid.SelectedIndex == 1)
                {
                    Empid = "%";
                }
                else
                {
                    Empid = ddlEmpid.SelectedValue;
                }
            }

            if (ddloption.SelectedIndex == 1)
            {
                if (ddlClientID.SelectedIndex == 1)
                {
                    Clientid = "%";
                }
                else
                {
                    Clientid = ddlClientID.SelectedValue;
                }
            }
            else if (ddloption.SelectedIndex == 2)
            {
                if (ddlFOID.SelectedIndex == 1)
                {
                    Activity = "%";
                }
                else
                {
                    Activity = ddlFOID.SelectedItem.Text;
                }
            }



            string Spname = "GetPinMyvisitImages";

            Hashtable ht = new Hashtable();
            ht.Add("@Day", Day);
            ht.Add("@month", month);
            ht.Add("@Year", Year);
            ht.Add("@CompanyID", companyid);
            ht.Add("@Empid", Empid);
            ht.Add("@Type", "GetImageDataByEmpid");
            ht.Add("@Option", ddloption.SelectedIndex);
            ht.Add("@fromDate", Fromdate);
            ht.Add("@ToDate", Todate);
            ht.Add("@Clientid", Clientid);
            ht.Add("@Activity", Activity);
            ht.Add("@typewise", ddltype.SelectedIndex);

            DataTable dt = config.ExecuteAdaptorAsyncWithParams(Spname, ht).Result;

            if (dt.Rows.Count > 0)
            {

                byte[] bytes = null;

                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                writer.CloseStream = false;
                document.Open();


                for (int k = 0; k < dt.Rows.Count; k++)
                {

                    if (dt.Rows[k]["pitstopImage"].ToString().Length > 0)
                    {


                        //string imageUrl = dt.Rows[k]["pitstopImage"].ToString();
                        //byte[] imageBytes = null;
                        //imageBytes = Convert.FromBase64String(imageUrl);
                        //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                        //image.ScalePercent(20f);
                        //image.ScaleAbsolute(120f, 129f);
                        //document.Add(image);


                        PdfPTable table2 = new PdfPTable(2);
                        table2.TotalWidth = 500f;
                        table2.LockedWidth = true;
                        float[] width2 = new float[] { 2f, 2f };
                        table2.SetWidths(width2);


                        PdfPTable tempTable1 = new PdfPTable(1);
                        tempTable1.TotalWidth = 400f;
                        tempTable1.LockedWidth = true;
                        float[] tempWidth1 = new float[] { 2f };
                        tempTable1.SetWidths(tempWidth1);

                        PdfPCell cell = new PdfPCell(new Paragraph("Created On : " + dt.Rows[k]["Updatedon"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.BOLD, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Emp ID : " + dt.Rows[k]["UpdatedBy"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Name : " + dt.Rows[k]["Name"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Client ID : " + dt.Rows[k]["Clientid"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);


                        cell = new PdfPCell(new Paragraph("Activity : " + dt.Rows[k]["Activity"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);



                        cell = new PdfPCell(new Paragraph("Remarks : " + dt.Rows[k]["Remarks"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Checkin Lat : " + dt.Rows[k]["CheckinLat"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Checkin Lng : " + dt.Rows[k]["CheckinLng"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        cell = new PdfPCell(new Paragraph("Checkin Address : " + dt.Rows[k]["Address"].ToString(), FontFactory.GetFont(FontStyle, 11, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 0;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        tempTable1.AddCell(cell);

                        PdfPCell childTable1 = new PdfPCell(tempTable1);
                        childTable1.Border = 1;
                        childTable1.Colspan = 1;
                        childTable1.HorizontalAlignment = 0;
                        childTable1.FixedHeight = 150;
                        childTable1.PaddingLeft = 210;
                        childTable1.PaddingTop = 10;
                        tempTable1.AddCell(childTable1);

                        table2.AddCell(childTable1);

                        PdfPTable tempTable2 = new PdfPTable(1);
                        tempTable2.TotalWidth = 100f;
                        tempTable2.LockedWidth = true;
                        float[] tempWidth2 = new float[] { 2f };
                        tempTable2.SetWidths(tempWidth2);

                        string imageUrl = dt.Rows[k]["pitstopImage"].ToString();
                        byte[] imageBytes = null;
                        imageBytes = Convert.FromBase64String(imageUrl);
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                        image.ScalePercent(15f);
                        image.ScaleAbsolute(80f, 80f);
                        PdfPCell imageCell = new PdfPCell(image);
                        imageCell.Colspan = 1; // either 1 if you need to insert one cell
                        imageCell.Border = 0;
                        imageCell.HorizontalAlignment = 2;
                        tempTable2.AddCell(imageCell);

                        PdfPCell childTable2 = new PdfPCell(tempTable2);
                        childTable2.Border = 1;
                        childTable2.Colspan = 1;
                        childTable2.HorizontalAlignment = 0;
                        childTable2.FixedHeight = 100;
                        childTable2.PaddingLeft = 100;
                        childTable2.PaddingTop = 10;
                        tempTable2.AddCell(childTable2);

                        table2.AddCell(childTable2);


                        document.Add(table2);


                    }

                }

                document.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
                string filename = "";
                if (ddlEmpid.SelectedItem.Text == "--Select--")
                {
                    filename = "Pinmyvisits" + ".pdf";
                }
                else
                {
                    filename = ddlEmpid.SelectedItem.Text + ".pdf";
                }


                document.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();

            }


        }

        protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddltype.SelectedIndex == 0)
            {
                lblDay.Visible = true;
                lblMonth.Visible = false;
                txtMonth.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
            }
            else if (ddltype.SelectedIndex == 1)
            {
                lblDay.Visible = false;
                lblMonth.Visible = true;
                txtMonth.Visible = true;
                lblfrom.Visible = false;
                txtfrom.Visible = false;
                lblto.Visible = false;
                txtto.Visible = false;
            }
            else
            {
                txtMonth.Visible = false;
                lblDay.Visible = false;
                lblMonth.Visible = false;
                lblfrom.Visible = true;
                txtfrom.Visible = true;
                lblto.Visible = true;
                txtto.Visible = true;
            }
        }

        protected void ddloption_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            if (ddloption.SelectedIndex == 0)
            {
                lblclientid.Visible = false;
                ddlClientID.Visible = false;
                lblclientname.Visible = false;
                ddlCName.Visible = false;
                lblempid.Visible = true;
                ddlEmpid.Visible = true;
                lblActivity.Visible = false;
                ddlFOID.Visible = false;
            }
            else if (ddloption.SelectedIndex == 1)
            {
                lblclientid.Visible = true;
                ddlClientID.Visible = true;
                lblclientname.Visible = true;
                ddlCName.Visible = true;
                lblempid.Visible = false;
                ddlEmpid.Visible = false;
                lblActivity.Visible = false;
                ddlFOID.Visible = false;
            }
            else if (ddloption.SelectedIndex == 2)
            {
                lblclientid.Visible = false;
                ddlClientID.Visible = false;
                lblclientname.Visible = false;
                ddlCName.Visible = false;
                lblempid.Visible = false;
                ddlEmpid.Visible = false;
                lblActivity.Visible = true;
                ddlFOID.Visible = true;
            }

        }
    }
}


