using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SRF.P.DAL;

namespace SRF.P
{
    public partial class Reports : System.Web.UI.Page
    {
        MenuBAL BalObj = new MenuBAL();
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil gve = new GridViewExportUtil();
        static DataTable dtrep;
        static string[] brdcrumb = new string[10];
        static string[] brdid = new string[10];
        static int bi = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Master != null)
                {
                    HtmlControl mid = (HtmlControl)this.Master.Master.FindControl("ContentPlaceHolder1").FindControl("sli1");
                    mid.Attributes["class"] = "current";
                }
                if (Session["UserId"] != null && Session["AccessLevel"] != null)
                {

                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

                bi = 0;

                LoadIcons();
                //PreviligeUsers(Convert.ToInt32(Session["AccessLevel"]));
            }
        }

        protected void LoadIcons()
        {

            DataSet ds = BalObj.GetAllFolderMenus("ReportsLink", Convert.ToInt32(Session["AccessLevel"]));
            dtrep = ds.Tables[0];
            if (dtrep.Select("URL = ' ' and PARENT_FOLDER_ID=0").Length > 0)
            {
                DataTable dt = dtrep.Select("URL = ' ' and PARENT_FOLDER_ID=0").CopyToDataTable();
                dllist.DataSource = dt;
                dllist.DataBind();
            }
            if (dtrep.Select("LEN(URL) > 1 and PARENT_FOLDER_ID=0").Length > 0)
            {
                DataTable dt = dtrep.Select("LEN(URL) > 1 and PARENT_FOLDER_ID=0").CopyToDataTable();
                DlLiList.DataSource = dt;
                DlLiList.DataBind();
            }

        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            LinkButton bobj = (LinkButton)sender;
            string ID = bobj.CommandArgument.ToString();
            string fname = bobj.CommandName.ToString();
            int fid = Convert.ToInt32(ID);
            //    AddBrdCrumbs(fname, fid);
            FillData(fid);

        }

        public void FillData(int fid)
        {
            //  Response.Write(fid);
            if (dtrep.Select("URL=' ' and PARENT_FOLDER_ID=" + fid + "").Length > 0)
            {
                DataTable dt = dtrep.Select("URL=' ' and PARENT_FOLDER_ID=" + fid + "").CopyToDataTable();
                dllist.DataSource = dt;
                dllist.DataBind();

            }
            else
            {
                dllist.DataSource = null;
                dllist.DataBind();
            }
            if (dtrep.Select("ISNULL(FOLDER_ID,0)=0 and PARENT_FOLDER_ID=" + fid + "").Length > 0)
            {
                DataTable dt = dtrep.Select("ISNULL(FOLDER_ID, 0) = 0 and PARENT_FOLDER_ID=" + fid + "").CopyToDataTable();
                DlLiList.DataSource = dt;
                DlLiList.DataBind();
            }
            else
            {
                DlLiList.DataSource = null;
                DlLiList.DataBind();
            }
        }

        protected void DlLiList_ItemDataBound(object sender, DataListItemEventArgs e)
        {

        }
    }
}