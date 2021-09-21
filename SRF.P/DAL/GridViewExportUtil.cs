using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

/// <summary>
/// 
/// </summary>
/// 
public class GridViewExportUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    /// 
    public void Export(string fileName, GridView gv)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid
                Table table = new Table();
                table.BorderStyle = BorderStyle.Solid;
                table.GridLines = GridLines.Both;
                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                    gv.FooterRow.Style.Add("font-weight", "bold");
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    public void NewExportExcel(string fileName, DataTable dt)
    {

        var products = dt;
        ExcelPackage excel = new ExcelPackage();
        var workSheet = excel.Workbook.Worksheets.Add(fileName);
        var totalCols = products.Columns.Count;
        var totalRows = products.Rows.Count;

        for (var col = 1; col <= totalCols; col++)
        {
            workSheet.Cells[1, col].Value = products.Columns[col - 1].ColumnName;
            workSheet.Cells[1, col].Style.Font.Bold = true;

        }
        for (var row = 1; row <= totalRows; row++)
        {
            for (var col = 0; col < totalCols; col++)
            {
                workSheet.Cells[row + 1, col + 1].Value = products.Rows[row - 1][col];

            }
        }
        using (var memoryStream = new MemoryStream())
        {
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment ;filename=\"" + fileName + "\"");
            excel.SaveAs(memoryStream);
            memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    /// 
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }

            if (current.HasControls())
            {
                GridViewExportUtil.PrepareControlForExport(current);
            }
        }
    }

    public void ExporttoExcel(GridView table, string name, string heading)
    {
        string filename = name + ".xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=LoanDetailsReport.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        HttpContext.Current.Response.Write("<TR valign='middle'>");
        HttpContext.Current.Response.Write("<Td colspan= 7>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("<Center>");
        HttpContext.Current.Response.Write(heading);
        HttpContext.Current.Response.Write("</Center>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='middle'>");
        HttpContext.Current.Response.Write("<Td colspan=7'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("<Center>");
        HttpContext.Current.Response.Write(name);
        HttpContext.Current.Response.Write("</Center>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td >");
            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("<Center>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Center>");
            HttpContext.Current.Response.Write("</Td>");
        }
        HttpContext.Current.Response.Write("</TR>");



        foreach (GridViewRow row in table.Rows)
        {//write in new row

            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {

                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row.Cells[i].Text.ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");


        }

        HttpContext.Current.Response.Write("<TR>");

        for (int k = 0; k < columnscount; k++)
        {
            //write in new column



            HttpContext.Current.Response.Write("<Td >");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.FooterRow.Cells[k].Text.ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write(style);

        }
        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExportGrid(string fileName, HiddenField hidGridView)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.Charset = "";
        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        stringwriter.Write(System.Web.HttpUtility.HtmlDecode(hidGridView.Value));
        HttpContext.Current.Response.Write(style);
        HttpContext.Current.Response.Write(stringwriter.ToString());
        HttpContext.Current.Response.End();
    }

    public void GridExport(string fileName, GridView gv, string line, string line1, int cols)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {

                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                 "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                 "style='font-size:11.0pt; font-family:calibri; background:white;'>");

                HttpContext.Current.Response.Write("<TR valign='top'>");
                HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan='" + cols + "'>");
                //HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(line);
                HttpContext.Current.Response.Write("</B>");
                //HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");

                HttpContext.Current.Response.Write("<TR valign='top'>");
                HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan='" + cols + "'>");
                //HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(" ");
                HttpContext.Current.Response.Write("</B>");
                //HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");

                HttpContext.Current.Response.Write("<TR valign='top'>");
                HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan='" + cols + "'>");
                //HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(line1);
                HttpContext.Current.Response.Write("</B>");
                //HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");

                HttpContext.Current.Response.Write("<TR valign='top'>");
                HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan='" + cols + "'>");
                //HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(" ");
                HttpContext.Current.Response.Write("</B>");
                // HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");

                HttpContext.Current.Response.Write("<TR valign='middle'");
                HttpContext.Current.Response.Write("<Td  colspan='5'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(" ");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");

                HttpContext.Current.Response.Write("<Td  colspan='6'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("Salary Details");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");

                HttpContext.Current.Response.Write("<Td  colspan='2'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("Employer Contribution");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");

                HttpContext.Current.Response.Write("<Td  colspan='1'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(" ");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");

                HttpContext.Current.Response.Write("<Td  colspan='2'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("Gross Profit");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");

                HttpContext.Current.Response.Write("<Td  colspan='3'>");
                HttpContext.Current.Response.Write("<Center>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(" ");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Center>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");


                HttpContext.Current.Response.Write("</Table>");

                //  Create a form to contain the grid
                Table table = new Table();
                table.BorderStyle = BorderStyle.Solid;
                table.GridLines = GridLines.Both;
                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }
    }

    public void ExporttoExcelnew(DataTable table)
    {
        string filename = "ListOfEmployees.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ListOfEmployees.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;




        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelforwagesheet(DataTable table)
    {
        string filename = "WageSheetReport.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=WageSheetReport.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExceFromTpaysheet(DataTable table, string FileName, string line, string line1, string line2, string line3, string line4)
    {


        // filename = "SalarySheet.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename='" + FileName + "'.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;
        int columnscount1 = table.Columns.Count / 2;


        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        //Row2

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left' style='border:none;' colspan= '" + columnscount1 + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line1);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='Left' style='border:none;' colspan= '" + columnscount1 + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line2);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        //Row3
        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left' style='border:none;' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line3);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");


        //Row4
        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='center' style='border:none;' colspan= '" + columnscount + "'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write(line4);
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        //Row5
        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='Left'  colspan= 10>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='center'  colspan= 31>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Attendane (Please mention the date of suspension of employees, If Any");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='center'  colspan= 2>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='center'  colspan= 18>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Earned Wages and Other Allowances");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td align='center' colspan= 11>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("DEDUCTIONS");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        HttpContext.Current.Response.Write("<Td align='left'  colspan= 2>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("</TR>");


        for (int j = 0; j < columnscount; j++)
        {

            HttpContext.Current.Response.Write("<Td valign='middle'>");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void UniformExporttoExcel(GridView table, string name, string heading)
    {
        string filename = name + ".xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=LoanDetailsReport.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;

        HttpContext.Current.Response.Write("<TR valign='middle'>");
        HttpContext.Current.Response.Write("<Td colspan= 5>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("<Center>");
        HttpContext.Current.Response.Write(heading);
        HttpContext.Current.Response.Write("</Center>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='middle'>");
        HttpContext.Current.Response.Write("<Td colspan=5'>");
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("<Center>");
        HttpContext.Current.Response.Write(name);
        HttpContext.Current.Response.Write("</Center>");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("<TR valign='top'>");
        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td >");
            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("<Center>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Center>");
            HttpContext.Current.Response.Write("</Td>");
        }
        HttpContext.Current.Response.Write("</TR>");



        foreach (GridViewRow row in table.Rows)
        {//write in new row

            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {

                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row.Cells[i].Text.ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");


        }

        HttpContext.Current.Response.Write("<TR>");

        for (int k = 0; k < columnscount; k++)
        {
            //write in new column



            HttpContext.Current.Response.Write("<Td >");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.FooterRow.Cells[k].Text.ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write(style);

        }
        HttpContext.Current.Response.Write("</TR>");


        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void LoanDetailsExporttoExcel(GridView table)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=LoanDetailsReport.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;


        HttpContext.Current.Response.Write("<TR valign='top'>");
        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td >");
            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("<Center>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Center>");
            HttpContext.Current.Response.Write("</Td>");
        }
        HttpContext.Current.Response.Write("</TR>");

        foreach (GridViewRow row in table.Rows)
        {//write in new row

            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row.Cells[i].Text.ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }

        HttpContext.Current.Response.Write("<TR>");

        for (int k = 0; k < columnscount; k++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td >");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.FooterRow.Cells[k].Text.ToString());
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write(style);

        }
        HttpContext.Current.Response.Write("</TR>");

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void ExporttoExcelProftmargin(DataTable table)
    {
        string filename = "UnitWiseProfitMarginReport.xls";
        string style = @"<style> .text { mso-number-format:\@; } </style> ";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=UnitWiseProfitMarginReport.xls");

        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:11.0pt; font-family:calibri; background:white;'>");

        //am getting my grid's column headers
        int columnscount = table.Columns.Count;




        for (int j = 0; j < columnscount; j++)
        {
            //write in new column

            HttpContext.Current.Response.Write("<Td valign='middle'>");

            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write(table.Columns[j].ToString());

            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");
        }

        HttpContext.Current.Response.Write("</TR>");

        foreach (DataRow row in table.Rows)
        {//write in new row
            HttpContext.Current.Response.Write("<TR>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row[i].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write(style);
            }

            HttpContext.Current.Response.Write("</TR>");
        }
        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

}
