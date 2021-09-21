using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Settings
{
    public partial class Zone : System.Web.UI.Page
    {
        AppConfiguration config = new AppConfiguration();
        GridViewExportUtil GVUtil = new GridViewExportUtil();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                if (!IsPostBack)
                {
                    if (Session["UserId"] != null && Session["AccessLevel"] != null)
                    {
                       

                    }
                    else
                    {
                        Response.Redirect("login.aspx");
                    }
                    Displaydata();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void Displaydata()
        {

            DataTable DtZone = GlobalData.Instance.LoadZone();
            if (DtZone.Rows.Count > 0)
            {
                GVZone.DataSource = DtZone;
                GVZone.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Zone Names Are Not Avialable');", true);
                return;
            }
        }

        protected void GVZone_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVZone.EditIndex = e.NewEditIndex;
            Displaydata();
        }

        protected void GVZone_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVZone.EditIndex = -1;
            Displaydata();
        }

        protected void GVZone_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVZone.PageIndex = e.NewPageIndex;
            GVZone.DataBind();
        }

        protected void GVZone_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {


                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label ZoneId = GVZone.Rows[e.RowIndex].FindControl("lblZoneid") as Label;
                TextBox ZoneName = GVZone.Rows[e.RowIndex].FindControl("txtZoneName") as TextBox;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (ZoneName.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Zone Name');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var ZName = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                ZName = ZoneName.Text.Trim();
                ProcedureName = "ModifyZone";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@ZoneName", ZName.ToUpper());
                HtSPParameters.Add("ZoneId", ZoneId.Text);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Zone Name  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Zone Name Not  Updated.Because  The Name Already Exist. NOTE:Zone Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [14-10-2013]
                GVZone.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }

        protected void Btn_Zone_Click(object sender, EventArgs e)
        {

            try
            {


                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Zone.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter Zone Name.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var ZoneName = string.Empty;
                var IRecordStatus = 0;

                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                ZoneName = Txt_Zone.Text.Trim().ToUpper();
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddZone";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@ZoneName", ZoneName);
                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Zone Name Added SucessFully.');", true);
                    Txt_Zone.Text = string.Empty;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Zone Name Not  Added.Because  The Name Already Exist. NOTE:Zone Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [12-10-2013]

                #region  Begin Code For Re-Call All the Departments As on [12-10-2013]
                Displaydata();
                #endregion End Code For Re-Call All the Departments As on [12-10-2013]

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Your Admin..');", true);
                return;
            }
        }
    }
}