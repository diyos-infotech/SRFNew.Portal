using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using KLTS.Data;
using System.Globalization;
using System.Collections;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class InventoryDetailsReport : System.Web.UI.Page
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
            if (Session.Keys.Count > 0)
            {
                CmpIDPrefix = Session["CmpIDPrefix"].ToString();
            }
            else
            {
                Response.Redirect("login.aspx");
            }

        }

        protected void ClearData()
        {
            GVListEmployees.DataSource = null;
            GVListEmployees.DataBind();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {

            ClearData();

            var FromDate = "";
            var ToDate = "";

            DateTime Fdate = DateTime.Now;
            DateTime Tdate = DateTime.Now;
            string FrmDt = "";
            string ToDt = "";
            var SPName = "";
            Hashtable HtDetails = new Hashtable();
            DataTable Dt = null;



            if (txtfromdate.Text.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select The From To Date');", true);
                return;
            }

            if (txtfromdate.Text.Trim().Length > 0)
            {
                Fdate = DateTime.Parse(txtfromdate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }

            if (txttodate.Text.Trim().Length > 0)
            {
                Tdate = DateTime.Parse(txttodate.Text, CultureInfo.GetCultureInfo("en-gb"));
            }



            FrmDt = Fdate.ToString("yyyy/MM/dd");
            ToDt = Tdate.ToString("yyyy/MM/dd");

            int form = 0;
            string empid = "";
            SPName = "StockConsumptionReport";

            HtDetails.Add("@FromDate", FrmDt);
            HtDetails.Add("@Todate", ToDt);

            Dt = config.ExecuteAdaptorAsyncWithParams(SPName, HtDetails).Result;
            if (Dt.Rows.Count > 0)
            {

                GVListEmployees.DataSource = Dt;
                GVListEmployees.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('The Details Are Not Avaialable');", true);
            }

        }


        protected void lbtn_Export_Click(object sender, EventArgs e)
        {
            GVUtil.Export("StockDetails.xls", this.GVListEmployees);
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            if (GVListEmployees.Rows.Count > 0)
            {
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.LEGAL);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.AddTitle("DIYOS");
                document.AddAuthor("DIYOS");
                document.AddSubject("Invoice");
                document.AddKeywords("Keyword1, keyword2, …");

                int columns = GVListEmployees.Columns.Count;
                int rows = GVListEmployees.Rows.Count;
                PdfPTable gvTable = new PdfPTable(columns);
                gvTable.TotalWidth = 500f;
                gvTable.LockedWidth = true;
                float[] widtlogo = { 1f, 2f, 4f, 2f, 2f, 2f, 2f, 2f }; //new float[columns]; 
                gvTable.SetWidths(widtlogo);

                uint FONT_SIZE = 9;
                string Fromdate = txtfromdate.Text;
                string Todate = txttodate.Text;

                PdfPCell c1 = new PdfPCell(new Phrase("STOCK DETAILS From " + Fromdate + " to " + Todate, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK)));
                c1.Border = 0;
                c1.HorizontalAlignment = 1;
                c1.Colspan = columns;
                gvTable.AddCell(c1);
                PdfPCell cBlank = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.BOLD, BaseColor.BLACK)));
                cBlank.Border = 0;
                cBlank.HorizontalAlignment = 1;
                cBlank.Colspan = columns;
                gvTable.AddCell(cBlank);

                PdfPCell cell;
                string cellText = "";

                for (int i = 0; i < columns; i++)
                {
                    widtlogo[i] = (int)GVListEmployees.Columns[i].ItemStyle.Width.Value;
                    //fetch the header text
                    cellText = Server.HtmlDecode(GVListEmployees.HeaderRow.Cells[i].Text);
                    cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, Font.BOLD, BaseColor.BLACK)));
                    gvTable.AddCell(cell);
                }

                for (int rowCounter = 0; rowCounter < rows; rowCounter++)
                {
                    if (GVListEmployees.Rows[rowCounter].RowType == DataControlRowType.DataRow)
                    {
                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblSno")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblItemId")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 1;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblItemName")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblOpeningstock")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblInflowStock")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblQuantity")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblResourceReturned")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        gvTable.AddCell(cell);

                        cellText = ((Label)GVListEmployees.Rows[rowCounter].FindControl("lblClosingStock")).Text;
                        cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = 2;
                        gvTable.AddCell(cell);

                        //cellText = ((Label)GVListOfItems.Rows[rowCounter].FindControl("lblIBPrice")).Text;
                        //cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        //gvTable.AddCell(cell);

                        //cellText = ((Label)GVListOfItems.Rows[rowCounter].FindControl("lblISPrice")).Text;
                        //cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        //gvTable.AddCell(cell);

                        //cellText = ((Label)GVListOfItems.Rows[rowCounter].FindControl("lblDSPrice")).Text;
                        //cell = new PdfPCell(new Phrase(cellText, FontFactory.GetFont(FontFactory.TIMES_ROMAN, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                        //gvTable.AddCell(cell);
                    }
                }
                document.Add(gvTable);
                document.NewPage();
                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=StockReport.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
            }

        }

    }
}