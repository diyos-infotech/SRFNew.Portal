using System;
using KLTS.Data;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SRF.P.DAL;

namespace SRF.P
{
    public partial class companyinfo : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();

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



                LoadPreviousData();
            }
        }


        protected void btnaddclint_Click(object sender, EventArgs e)
        {
            lblresult.Visible = true;
            try
            {

                if (txtcsname.Text.Trim().Length == 0)
                {
                    ErrorMessage1 = ErrorMessage1 + "* Company Short Name Don't Leave Empty <br>";

                }
                if (txtcsname.Text.Trim().Length == 0)
                {
                    ErrorMessage1 = ErrorMessage1 + "*  Bill Seq Don't Leave Empty <br>";
                }

                Modify();
            }
            catch (Exception ex)
            {
              
            }
        }


        public void Modify()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string cname = txtcname.Text;
            string csname = txtcsname.Text;
            string address = txtaddress.Text;
            string pfno = txtpfno.Text;
            string esino = txtesino.Text;
            string billdesc = txtbilldesc.Text;
            string cinfo = txtcinfo.Text;
            string billnotes = txtbnotes.Text;
            string billseq = txtbillsq.Text;
            string labourrule = txtlabour.Text;

            //string Accountno = txtAccountno.Text;
            //string IFSCCOde = txtifsccode.Text;
            //string ChequePREPARE = txtPREPARE.Text;
            //string Bank = txtBANK.Text;
            //string Addresslineone = txtaddresslineone.Text;
            //string Addresslinetwo = txtaddresslinetwo.Text;
            //string SASTC = txtsastcc.Text;

            string Phoneno = txtPhoneno.Text;
            string Faxno = txtFaxno.Text;
            string Emailid = txtEmail.Text;
            string Website = txtWebsite.Text;
            string notes = txtNotes.Text;
            string CorporateIDNO = txtcorporateIDNo.Text;
            string RegNo = txtregno.Text;
            string ESICNoForms = txtESICNoForms.Text;
            string BranchOffice = txtBranchOffice.Text;
            var BankName = txtBankname.Text;
            var BankAccountNo = txtBankAccNo.Text;
            var IFSCCode = txtifsccode.Text;
            //string ISOCertNo = txtISOCertNo.Text;
            //string PsaraAct = txtPsaraAct.Text;
            //string KSSAMemberShipNo = txtKSSAMemberShipNo.Text;

            string SqlQry = "Select * From CompanyInfo   where  ClientidPrefix  ='" + CmpIDPrefix + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(SqlQry).Result;
            cmd.Parameters.Clear();
            if (dt.Rows.Count > 0)
            {
                cmd.CommandText = "modifyaddcompanyinfo";
            }
            else
            {
                cmd.CommandText = "addcompanyinfo";
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Add("@pcompanyname", cname);
            cmd.Parameters.Add("@pshortname", csname);
            cmd.Parameters.Add("@paddress", address);
            cmd.Parameters.Add("@ppfno", pfno);
            cmd.Parameters.Add("@pesino", esino);
            cmd.Parameters.Add("@pbilldesc", billdesc);
            cmd.Parameters.Add("@pcompanyinfo", cinfo);
            cmd.Parameters.Add("@pbillnotes", billnotes);
            cmd.Parameters.Add("@pbillseq", billseq);
            cmd.Parameters.Add("@plabourrule", labourrule);
            //cmd.Parameters.Add("@plogo", Session["imagebytes"]  );
            //cmd.Parameters.Add("@ChequePrepare", ChequePREPARE);
            //cmd.Parameters.Add("@Bankname", Bank);
            //cmd.Parameters.Add("@bankaccountno", Accountno);
            //cmd.Parameters.Add("@Addresslineone", Addresslineone);
            //cmd.Parameters.Add("@Addresslinetwo",Addresslinetwo);
            //cmd.Parameters.Add("@IfscCode", IFSCCOde);
            //cmd.Parameters.Add("@SASTC", SASTC);
            cmd.Parameters.Add("@ClientidPrefix", CmpIDPrefix);
            cmd.Parameters.Add("@Phoneno", Phoneno);
            cmd.Parameters.Add("@Faxno", Faxno);
            cmd.Parameters.Add("@Emailid", Emailid);
            cmd.Parameters.Add("@Website", Website);
            cmd.Parameters.Add("@Notes", notes);
            cmd.Parameters.Add("@CorporateIDNo", CorporateIDNO);
            cmd.Parameters.Add("@RegNo", RegNo);
            cmd.Parameters.Add("@ESICNoForms", ESICNoForms);
            cmd.Parameters.Add("@BranchOffice", BranchOffice);
            cmd.Parameters.Add("@Bankname", BankName);
            cmd.Parameters.Add("@bankaccountno", BankAccountNo);
            cmd.Parameters.Add("@IfscCode", IFSCCode);
            //cmd.Parameters.Add("@ISOCertfNo", ISOCertNo);
            //cmd.Parameters.Add("@PSARARegNo", PsaraAct);
            //cmd.Parameters.Add("@KSSAMembershipNo", KSSAMemberShipNo);

            int status = cmd.ExecuteNonQuery();
            con.Close();

            if (status != 0)
            {
                lblresult.Visible = true;
                lblresult.Text = "Record  added Successfully";
                Disablefields();
            }
            else
            {
                lblresult.Visible = true;
                lblresult.Text = "Record Not Inserted";
            }
        }

        public void Insert()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KLTSConnectionString"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string cname = txtcname.Text;
            string csname = txtcsname.Text;
            string address = txtaddress.Text;
            string pfno = txtpfno.Text;
            string esino = txtesino.Text;
            string billdesc = txtbilldesc.Text;
            string cinfo = txtcinfo.Text;
            string billnotes = txtbnotes.Text;
            string billseq = txtbillsq.Text;
            string labourrule = txtlabour.Text;
            string Phoneno = txtPhoneno.Text;
            string Faxno = txtFaxno.Text;
            string Emailid = txtEmail.Text;
            string Website = txtWebsite.Text;
            string notes = txtNotes.Text;
            string CorporateIDNO = txtcorporateIDNo.Text;
            string RegNo = txtregno.Text;
            string ESICNoForms = txtESICNoForms.Text;
            string BranchOffice = txtBranchOffice.Text;
            var BankName = txtBankname.Text;
            var BankAccountNo = txtBankAccNo.Text;
            var IFSCCode = txtifsccode.Text;
            //string ISOCertNo = txtISOCertNo.Text ;
            //string PsaraAct = txtPsaraAct.Text ;
            //string KSSAMemberShipNo = txtKSSAMemberShipNo.Text ;

            cmd.CommandText = "addcompanyinfo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.Parameters.Add("@pcompanyname", cname);
            cmd.Parameters.Add("@pshortname", csname);
            cmd.Parameters.Add("@paddress", address);
            cmd.Parameters.Add("@ppfno", pfno);
            cmd.Parameters.Add("@pesino", esino);
            cmd.Parameters.Add("@pbilldesc", billdesc);
            cmd.Parameters.Add("@pcompanyinfo", cinfo);
            cmd.Parameters.Add("@pbillnotes", billnotes);
            cmd.Parameters.Add("@pbillseq", billseq);
            cmd.Parameters.Add("@plabourrule", labourrule);
            // cmd.Parameters.Add("@plogo", Session["imagebytes"]);
            cmd.Parameters.Add("@notes", notes);
            cmd.Parameters.Add("@Phoneno", Phoneno);
            cmd.Parameters.Add("@Faxno", Faxno);
            cmd.Parameters.Add("@Emailid", Emailid);
            cmd.Parameters.Add("@Website", Website);
            cmd.Parameters.Add("@CorporateIDNo", CorporateIDNO);
            cmd.Parameters.Add("@RegNo", RegNo);
            cmd.Parameters.Add("@ESICNoForms", ESICNoForms);
            cmd.Parameters.Add("@BranchOffice", BranchOffice);

            cmd.Parameters.Add("@Bankname", BankName);
            cmd.Parameters.Add("@bankaccountno", BankAccountNo);
            cmd.Parameters.Add("@IfscCode", IFSCCode);
            //cmd.Parameters.Add("@ISOCertfNo", ISOCertNo);
            //cmd.Parameters.Add("@PSARARegNo", PsaraAct);
            //cmd.Parameters.Add("@KSSAMembershipNo", KSSAMemberShipNo);


            int status = cmd.ExecuteNonQuery();
            con.Close();

            if (status != 0)
            {
                lblresult.Visible = true;
                lblresult.Text = "Record  Added Successfully";
                Disablefields();
            }
            else
            {
                lblresult.Visible = true;
                lblresult.Text = "Record Not Inserted";
            }
        }


        private void clearData()
        {
            txtcname.Text = txtcsname.Text = txtaddress.Text = txtpfno.Text = txtesino.Text = txtbilldesc.Text = string.Empty;
            txtcinfo.Text = txtbnotes.Text = txtbillsq.Text = txtlabour.Text = string.Empty;
            //imglogo.Src = "";
            txtPhoneno.Text = txtFaxno.Text = txtEmail.Text = txtWebsite.Text = txtNotes.Text = txtregno.Text = txtcorporateIDNo.Text = string.Empty;
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            clearData();
        }

        string ErrorMessage1 = "<br> Please Fill Check The Following Errors <br>";

        protected void GetWebConfigdata()
        {

            CmpIDPrefix = Session["CmpIDPrefix"].ToString();
        }

        protected void LoadPreviousData()
        {
            string selectquery = "select * from companyinfo   where  ClientidPrefix  ='" + CmpIDPrefix + "'";
            DataTable dt = config.ExecuteAdaptorAsyncWithQueryParams(selectquery).Result;

            if (dt.Rows.Count > 0)
            {
                txtcname.Text = dt.Rows[0]["companyname"].ToString();
                txtcsname.Text = dt.Rows[0]["shortname"].ToString();
                txtaddress.Text = dt.Rows[0]["address"].ToString();
                txtpfno.Text = dt.Rows[0]["pfno"].ToString();
                txtesino.Text = dt.Rows[0]["esino"].ToString();
                txtbilldesc.Text = dt.Rows[0]["billdesc"].ToString();
                txtcinfo.Text = dt.Rows[0]["companyinfo"].ToString();
                txtbnotes.Text = dt.Rows[0]["billnotes"].ToString();
                txtbillsq.Text = dt.Rows[0]["billseq"].ToString();
                txtlabour.Text = dt.Rows[0]["labourrule"].ToString();

                txtBankname.Text = dt.Rows[0]["BankName"].ToString();
                txtBankAccNo.Text = dt.Rows[0]["BankAccountNo"].ToString();
                txtifsccode.Text = dt.Rows[0]["IFSCCode"].ToString();

                //txtPREPARE.Text = dt.Rows[0]["ChequePrepare"].ToString();
                //txtBANK.Text = dt.Rows[0]["Bankname"].ToString();
                //txtAccountno.Text = dt.Rows[0]["bankaccountno"].ToString();
                //txtaddresslineone.Text = dt.Rows[0]["Addresslineone"].ToString();
                //txtaddresslinetwo.Text = dt.Rows[0]["Addresslinetwo"].ToString();
                //txtifsccode.Text = dt.Rows[0]["IfscCode"].ToString();

                txtPhoneno.Text = dt.Rows[0]["Phoneno"].ToString();
                txtFaxno.Text = dt.Rows[0]["Faxno"].ToString();
                txtEmail.Text = dt.Rows[0]["Emailid"].ToString();
                txtWebsite.Text = dt.Rows[0]["Website"].ToString();
                txtNotes.Text = dt.Rows[0]["Notes"].ToString();
                txtcorporateIDNo.Text = dt.Rows[0]["CorporateIDNo"].ToString();
                txtregno.Text = dt.Rows[0]["RegNo"].ToString();
                txtESICNoForms.Text = dt.Rows[0]["ESICNoForms"].ToString();
                txtBranchOffice.Text = dt.Rows[0]["BranchOffice"].ToString();
                //txtISOCertNo.Text = dt.Rows[0]["ISOCertfNo"].ToString();
                //txtPsaraAct.Text = dt.Rows[0]["PSARARegNo"].ToString();
                //txtKSSAMemberShipNo.Text = dt.Rows[0]["KSSAMembershipNo"].ToString();

                Byte[] barr = new Byte[100000];

                //if (String.IsNullOrEmpty(dt.Rows[0]["logo"].ToString()) == false)
                //{
                //    barr = (Byte[])dt.Rows[0]["logo"];
                //    Session["image"] = barr;
                //    MemoryStream a = new MemoryStream(barr, false);
                //    System.Drawing.Image image = System.Drawing.Image.FromStream(a);
                //    Random rnd = new Random();
                //    string imagename = rnd.Next() + ".jpg";
                //    image.Save(Server.MapPath("Images" + "\\" + "\\" + imagename), System.Drawing.Imaging.ImageFormat.Jpeg);
                //    imglogo.Src = "~/Images/" + imagename;
                //    // Session["Image"] = "~/Images/" + imagename;
                //}
            }

            else
            {

                Enabledfields();
            }
        }

        public void Enabledfields()
        {
            txtcname.Enabled = true;
            txtcsname.Enabled = true;
            txtaddress.Enabled = true;
            txtpfno.Enabled = true;
            txtesino.Enabled = true;
            txtbilldesc.Enabled = true;
            txtcinfo.Enabled = true;
            txtbnotes.Enabled = true;
            txtBANK.Enabled = true;
            txtbillsq.Enabled = true;
            txtlabour.Enabled = true;
            txtPhoneno.Enabled = true;
            txtFaxno.Enabled = true;
            txtEmail.Enabled = true;
            txtWebsite.Enabled = true;
            txtNotes.Enabled = true;
            txtpfno.Enabled = true;
            txtcorporateIDNo.Enabled = true;
            txtregno.Enabled = true;
            txtESICNoForms.Enabled = true;
            txtBranchOffice.Enabled = true;

            //txtISOCertNo.Enabled = true;
            //txtPsaraAct.Enabled = true;
            //txtKSSAMemberShipNo.Enabled = true;
            txtBankname.Enabled = true;
            txtBankAccNo.Enabled = true;
            txtifsccode.Enabled = true;


            btnaddclint.Enabled = true;
            btncancel.Enabled = true;
            btnEdit.Enabled = false;
            lblresult.Visible = false;


        }

        public void Disablefields()
        {

            txtESICNoForms.Enabled = false;
            txtBranchOffice.Enabled = false;

            //txtISOCertNo.Enabled = false;
            //txtPsaraAct.Enabled = false;
            //txtKSSAMemberShipNo.Enabled = false;
            txtBankname.Enabled = false;
            txtBankAccNo.Enabled = false;
            txtifsccode.Enabled = false;

            txtcname.Enabled = false;
            txtcsname.Enabled = false;
            txtaddress.Enabled = false;
            txtpfno.Enabled = false;
            txtesino.Enabled = false;
            txtbilldesc.Enabled = false;
            txtBANK.Enabled = false;
            txtcinfo.Enabled = false;
            txtbnotes.Enabled = false;
            txtbillsq.Enabled = false;
            txtlabour.Enabled = false;
            txtPhoneno.Enabled = false;
            txtFaxno.Enabled = false;
            txtEmail.Enabled = false;
            txtWebsite.Enabled = false;
            txtNotes.Enabled = false;
            txtpfno.Enabled = false;
            txtcorporateIDNo.Enabled = false;
            txtregno.Enabled = false;
            btnaddclint.Enabled = false;
            btncancel.Enabled = false;
            btnEdit.Enabled = true;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string selectquery = "select * from companyinfo  where  ClientidPrefix  ='" + CmpIDPrefix + "'";
            DataTable dt = config.ExecuteReaderWithQueryAsync(selectquery).Result;

            if (dt.Rows.Count > 0)
            {
                Enabledfields();
            }
        }
    }
}