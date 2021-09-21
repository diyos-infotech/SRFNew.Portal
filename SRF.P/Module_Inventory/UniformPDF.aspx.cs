using System;
using System.Data;
using System.Web.UI;
using KLTS.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SRF.P.DAL;

namespace SRF.P.Module_Inventory
{
    public partial class UniformPDF : System.Web.UI.Page
    {
        DataTable dt;
        string EmpIDPrefix = "";
        string Fontstyle = "verdana";
        string CmpIDPrefix = "";

        AppConfiguration config = new AppConfiguration();

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
                GoToLoginPage();
            }


        }



        public void GoToLoginPage()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Your Session Expired.Please Login');", true);
            Response.Redirect("~/login.aspx");
        }

        protected void GetWebConfigdata()
        {
            EmpIDPrefix = Session["EmpIDPrefix"].ToString();
            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void txtEmpid_TextChanged(object sender, EventArgs e)
        {
            GetEmpName();
            GetLoanNos();
        }

        protected void GetEmpName()
        {
            string Sqlqry = "select (empfname+' '+empmname+' '+emplname) as empname,EmpDesgn from empdetails where empid='" + txtEmpid.Text + "' ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtName.Text = dt.Rows[0]["empname"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }


        }

        protected void GetEmpid()
        {

            #region  Old Code
            string Sqlqry = "select Empid from empdetails where (empfname+' '+empmname+' '+emplname)  like '" + txtName.Text + "' ";
            DataTable dt = config.ExecuteReaderWithQueryAsync(Sqlqry).Result;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    txtEmpid.Text = dt.Rows[0]["Empid"].ToString();

                }
                catch (Exception ex)
                {
                    // MessageLabel.Text = ex.Message;
                }
            }
            else
            {
                // MessageLabel.Text = "There Is No Name For The Selected Employee";
            }
            #endregion // End Old Code


        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            GetEmpid();
            GetLoanNos();
        }

        public void GetLoanNos()
        {
            ddlLoanNos.Items.Clear();
            string qry = "select distinct loanno from EmpResourceDetails  where empid='" + txtEmpid.Text + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            if (dt.Rows.Count > 0)
            {
                ddlLoanNos.DataValueField = "loanno";
                ddlLoanNos.DataTextField = "loanno";
                ddlLoanNos.DataSource = dt;
                ddlLoanNos.DataBind();
            }
            ddlLoanNos.Items.Insert(0, "--Select--");
        }

        protected void ddlLoanNos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        string Loanno = "";

        protected void btndownload_Click(object sender, EventArgs e)
        {

            string qry = "select distinct erd.loanno,erd.price,erd.uniformid,erd.qty,sil.itemname,elm.LoanAmount,elm.PaidAmnt,elm.NoInstalments,convert(varchar(10),elm.LoanIssuedDate,103) as LoanIssuedDate from EmpResourceDetails erd inner join invStockItemList sil on erd.ResourceId=sil.itemid inner join EmpLoanMaster elm on elm.empid=erd.empid and elm.LoanNo=erd.loanno where erd.empid='" + txtEmpid.Text + "' and erd.loanno='" + ddlLoanNos.SelectedValue + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(qry).Result;

            string LoanIssuedDate = "";

            string UniformID = "";
            if (dt.Rows.Count > 0)
            {

                MemoryStream ms = new MemoryStream();

                Document document = new Document(PageSize.LEGAL);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.AddTitle("FaMS");
                document.AddAuthor("DIYOS");
                document.AddSubject("Wage Slips");
                document.AddKeywords("Keyword1, keyword2, …");//
                string strQry = "Select * from CompanyInfo  where   ClientidPrefix='" + CmpIDPrefix + "'";
                DataTable compInfo = config.ExecuteReaderWithQueryAsync(strQry).Result;
                string companyName = "Your Company Name";
                string companyAddress = "Your Company Address";
                if (compInfo.Rows.Count > 0)
                {
                    companyName = compInfo.Rows[0]["CompanyName"].ToString();
                    companyAddress = compInfo.Rows[0]["Address"].ToString();
                }

                PdfPTable Maintable = new PdfPTable(4);
                Maintable.TotalWidth = 500f;
                Maintable.LockedWidth = true;
                float[] width = new float[] { 1.5f, 2f, 2.5f, 1f };
                Maintable.SetWidths(width);
                uint FONT_SIZE = 10;
                #region  Table Headings


                LoanIssuedDate = dt.Rows[0]["LoanIssuedDate"].ToString();



                PdfPCell Heading = new PdfPCell(new Phrase("UNIFORM ISSUES", FontFactory.GetFont(Fontstyle, 10, Font.BOLD, BaseColor.BLACK)));
                Heading.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Heading.Colspan = 4;
                Heading.Border = 0;// 15;
                Maintable.AddCell(Heading);

                PdfPCell CompanyName = new PdfPCell(new Phrase(companyName, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                CompanyName.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CompanyName.Border = 0;
                CompanyName.Colspan = 4;
                Maintable.AddCell(CompanyName);

                PdfPCell CompanyAddress = new PdfPCell(new Phrase(companyAddress, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CompanyAddress.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                CompanyAddress.Colspan = 4;
                CompanyAddress.Border = 0;
                Maintable.AddCell(CompanyAddress);

                PdfPCell employerAddress1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                employerAddress1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                employerAddress1.Border = 0;
                employerAddress1.Colspan = 4;
                Maintable.AddCell(employerAddress1);

                PdfPCell empcode = new PdfPCell(new Phrase("Emp Code            : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                empcode.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                empcode.Colspan = 1;
                empcode.Border = 0;
                Maintable.AddCell(empcode);

                PdfPCell empcode1 = new PdfPCell(new Phrase(txtEmpid.Text, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                empcode1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                empcode1.Colspan = 1;
                empcode1.Border = 0;
                Maintable.AddCell(empcode1);

                PdfPCell IssueRefNo = new PdfPCell(new Phrase("Issue RefNo  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                IssueRefNo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                IssueRefNo.Colspan = 1;
                IssueRefNo.Border = 0;
                Maintable.AddCell(IssueRefNo);



                UniformID = dt.Rows[0]["uniformid"].ToString();

                PdfPCell IssueRefNo1 = new PdfPCell(new Phrase(UniformID, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssueRefNo1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                IssueRefNo1.Colspan = 1;
                IssueRefNo1.Border = 0;
                Maintable.AddCell(IssueRefNo1);

                PdfPCell EmployeeName = new PdfPCell(new Phrase("Employee Name  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                EmployeeName.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmployeeName.Colspan = 1;
                EmployeeName.Border = 0;
                Maintable.AddCell(EmployeeName);

                PdfPCell EmployeeName1 = new PdfPCell(new Phrase(txtName.Text, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                EmployeeName1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                EmployeeName1.Colspan = 1;
                EmployeeName1.Border = 0;
                Maintable.AddCell(EmployeeName1);

                PdfPCell IssueDate = new PdfPCell(new Phrase("Issue Date  : ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                IssueDate.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right 
                IssueDate.Colspan = 1;
                IssueDate.Border = 0;
                Maintable.AddCell(IssueDate);

                PdfPCell IssueDate1 = new PdfPCell(new Phrase(LoanIssuedDate, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssueDate1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right 
                IssueDate1.Colspan = 1;
                IssueDate1.Border = 0;
                Maintable.AddCell(IssueDate1);

                Maintable.AddCell(employerAddress1);
                Maintable.AddCell(employerAddress1);


                document.Add(Maintable);

                #endregion

                #region Table Data

                PdfPTable DetailsTable = new PdfPTable(5);
                DetailsTable.TotalWidth = 500f;
                DetailsTable.LockedWidth = true;
                float[] DetailsWidth = new float[] { 0.5f, 2f, 0.5f, 0.5f, 0.5f };
                DetailsTable.SetWidths(DetailsWidth);


                PdfPCell Series = new PdfPCell(new Phrase("S.No", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Series.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(Series);


                PdfPCell ItemCode = new PdfPCell(new Phrase("Item Code ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemCode.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                                  // DetailsTable.AddCell(ItemCode);


                PdfPCell ItemDesc = new PdfPCell(new Phrase("Item Desc ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(ItemDesc);



                PdfPCell ItemRate = new PdfPCell(new Phrase("Item Rate ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                ItemRate.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(ItemRate);


                PdfPCell Quantity = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Quantity.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(Quantity);



                PdfPCell LineAmt = new PdfPCell(new Phrase("Line Amt", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                LineAmt.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                DetailsTable.AddCell(LineAmt);


                //PdfPCell NoOfInstalments = new PdfPCell(new Phrase("No.Of Instalments:", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                //NoOfInstalments.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //DetailsTable.AddCell(NoOfInstalments);

                int j = 1;
                string ItemDescription = "";
                float Itemrate = 0;
                float quantity = 0;
                float Lineamt = 0;
                float TotalLineAmt = 0;

                float TotalAmountreceived = 0;
                float TotalAmountdue = 0;
                float NoOfinstalments = 0;

                float Amountreceived = 0;
                float Amountdue = 0;

                string InWords = "";


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    ItemDescription = dt.Rows[i]["itemname"].ToString();
                    Itemrate = Convert.ToSingle(dt.Rows[i]["price"].ToString());
                    quantity = Convert.ToSingle(dt.Rows[i]["qty"].ToString());
                    Lineamt = (Itemrate * quantity);


                    Amountreceived = Convert.ToSingle(dt.Rows[i]["LoanAmount"].ToString());
                    Amountdue = Convert.ToSingle(dt.Rows[i]["PaidAmnt"].ToString());
                    NoOfinstalments = Convert.ToSingle(dt.Rows[i]["NoInstalments"].ToString());


                    InWords = NumberToEnglish.Instance.changeNumericToWords(Amountreceived.ToString());

                    PdfPCell Series1 = new PdfPCell(new Phrase(j.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Series1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(Series1);


                    PdfPCell ItemCode1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemCode1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                                       //DetailsTable.AddCell(ItemCode1);


                    PdfPCell ItemDesc1 = new PdfPCell(new Phrase(ItemDescription, FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemDesc1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(ItemDesc1);



                    PdfPCell ItemRate1 = new PdfPCell(new Phrase(Itemrate.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    ItemRate1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(ItemRate1);


                    PdfPCell Quantity1 = new PdfPCell(new Phrase(quantity.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    Quantity1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(Quantity1);



                    PdfPCell LineAmt1 = new PdfPCell(new Phrase(Lineamt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                    LineAmt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    DetailsTable.AddCell(LineAmt1);


                    TotalLineAmt += Lineamt;
                    // TotalAmountreceived += Amountreceived;
                    //TotalAmountdue += Amountdue;

                    j++;
                }


                #endregion

                document.Add(DetailsTable);


                PdfPTable DetailsTable2 = new PdfPTable(4);
                DetailsTable2.TotalWidth = 500f;
                DetailsTable2.LockedWidth = true;
                float[] DetailsWidth2 = new float[] { 2f, 2f, 3f, 1f };
                DetailsTable2.SetWidths(DetailsWidth2);


                PdfPCell Noofinstalments = new PdfPCell(new Phrase("No.of Instalments:   " + NoOfinstalments.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Noofinstalments.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Noofinstalments.Border = 0;
                Noofinstalments.Colspan = 4;
                // DetailsTable2.AddCell(Noofinstalments);


                //DetailsTable1.AddCell(cellemp);
                PdfPCell totalamt = new PdfPCell(new Phrase("Total Amount :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamt.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamt.Border = 0;
                totalamt.Colspan = 3;
                DetailsTable2.AddCell(totalamt);

                PdfPCell totalamts1 = new PdfPCell(new Phrase(TotalLineAmt.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamts1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamts1.Border = 0;
                totalamts1.Colspan = 1;
                DetailsTable2.AddCell(totalamts1);



                PdfPCell AmountReceived = new PdfPCell(new Phrase("Amount Received :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountReceived.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountReceived.Border = 0;
                AmountReceived.Colspan = 3;
                // AmountReceived.PaddingLeft = 10;
                DetailsTable2.AddCell(AmountReceived);


                PdfPCell totalamt1 = new PdfPCell(new Phrase(Amountdue.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                totalamt1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                totalamt1.Border = 0;
                totalamt1.Colspan = 1;
                DetailsTable2.AddCell(totalamt1);

                PdfPCell AmountDue = new PdfPCell(new Phrase("Amount Due :", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountDue.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountDue.Border = 0;
                AmountDue.Colspan = 3;
                // AmountDue.PaddingLeft = 10;
                DetailsTable2.AddCell(AmountDue);

                PdfPCell AmountReceived1 = new PdfPCell(new Phrase(Amountreceived.ToString(), FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountReceived1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountReceived1.Border = 0;
                AmountReceived1.Colspan = 1;
                DetailsTable2.AddCell(AmountReceived1);


                document.Add(DetailsTable2);

                PdfPTable DetailsTable1 = new PdfPTable(5);
                DetailsTable1.TotalWidth = 500f;
                DetailsTable1.LockedWidth = true;
                float[] DetailsWidth1 = new float[] { 1f, 1f, 1f, 1f, 1f };
                DetailsTable1.SetWidths(DetailsWidth1);


                PdfPCell AmountDue1 = new PdfPCell(new Phrase("", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                AmountDue1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                AmountDue1.Border = 0;
                AmountDue1.Colspan = 5;
                DetailsTable1.AddCell(AmountDue1);

                PdfPCell Amountinwords = new PdfPCell(new Phrase("Amount in Words: " + InWords.Trim() + " Only \n\n\n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.BOLD, BaseColor.BLACK)));
                Amountinwords.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                Amountinwords.Border = 0;
                Amountinwords.Colspan = 6;
                DetailsTable1.AddCell(Amountinwords);


                PdfPCell PreparedBy = new PdfPCell(new Phrase("Prepared by\n\n\n\n\n", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                PreparedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                PreparedBy.Border = 0;
                PreparedBy.Colspan = 2;
                DetailsTable1.AddCell(PreparedBy);


                PdfPCell IssuedBy = new PdfPCell(new Phrase("Issued by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                IssuedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                IssuedBy.Border = 0;
                IssuedBy.Colspan = 2;
                DetailsTable1.AddCell(IssuedBy);


                PdfPCell ReceivedBy = new PdfPCell(new Phrase("Received by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                ReceivedBy.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                ReceivedBy.Border = 0;
                ReceivedBy.Colspan = 2;
                DetailsTable1.AddCell(ReceivedBy);


                PdfPCell SRActionedBy = new PdfPCell(new Phrase("S.R.Actioned by", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                SRActionedBy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                SRActionedBy.Border = 0;
                SRActionedBy.Colspan = 3;
                DetailsTable1.AddCell(SRActionedBy);

                PdfPCell CAuthority = new PdfPCell(new Phrase("Recovery Actioned in Sys. by ", FontFactory.GetFont(Fontstyle, FONT_SIZE, Font.NORMAL, BaseColor.BLACK)));
                CAuthority.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                CAuthority.Border = 0;
                CAuthority.Colspan = 3;
                DetailsTable1.AddCell(CAuthority);



                document.Add(DetailsTable1);






                //if (hasPages)

                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=UniformPDF.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
                //}



            }
        }
    }
}