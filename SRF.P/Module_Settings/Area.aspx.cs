using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTS.Data;
using SRF.P.DAL;

namespace SRF.P.Module_Settings
{
    public partial class Area : System.Web.UI.Page
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
                    LoadZones();
                    Displaydata();
                    //displaydataZones();




                }
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadZones()
        {
            DataTable DtZone = GlobalData.Instance.LoadZone();
            if (DtZone.Rows.Count > 0)
            {
                ddlZoneonDefault.DataValueField = "Zoneid";
                ddlZoneonDefault.DataTextField = "ZoneName";
                ddlZoneonDefault.DataSource = DtZone;
                ddlZoneonDefault.DataBind();
            }
            ddlZoneonDefault.Items.Insert(0, "-Select-");

        }


        private void Displaydata()
        {

            DataTable DtArea = GlobalData.Instance.LoadArea();
            if (DtArea.Rows.Count > 0)
            {
                GvArea.DataSource = DtArea;
                GvArea.DataBind();

                for (int i = 0; i < GvArea.Rows.Count; i++)
                {
                    //DropDownList ddlZone = GvArea.Rows[i].FindControl("ddlZoneid") as DropDownList;
                    //ddlZone.SelectedValue = DtArea.Rows[i]["ZoneId"].ToString();

                    DataTable DtDesignation = GlobalData.Instance.LoadZone();

                    DropDownList ddlZoneid = GvArea.Rows[i].FindControl("ddlZoneid") as DropDownList;




                    if (DtDesignation.Rows.Count > 0)
                    {
                        ddlZoneid.DataValueField = "ZoneID";
                        ddlZoneid.DataTextField = "Zonename";
                        ddlZoneid.DataSource = DtDesignation;
                        ddlZoneid.DataBind();
                    }
                    ddlZoneid.Items.Insert(0, "-Select-");

                    Label lblAreaId = GvArea.Rows[i].FindControl("lblAreaId") as Label;


                    string qry = "select zoneid from area where areaid='" + lblAreaId.Text + "'";
                    DataTable dtz = config.ExecuteReaderWithQueryAsync(qry).Result;

                    if (dtz.Rows.Count > 0)
                    {
                        ddlZoneid.SelectedValue = dtz.Rows[0]["Zoneid"].ToString();
                    }

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Area Names Are Not Avialable');", true);
                return;
            }
        }

        protected void GvArea_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GvArea.EditIndex = e.NewEditIndex;
            Displaydata();
            DropDownList dlsd = GvArea.Rows[e.NewEditIndex].FindControl("ddlZoneid") as DropDownList;
            dlsd.Enabled = true;

        }

        protected void GvArea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GvArea.EditIndex = -1;
            Displaydata();
        }

        protected void GvArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvArea.PageIndex = e.NewPageIndex;
            GvArea.DataBind();
        }


        DropDownList bind_dropdownlist;
        private void displaydataZones()
        {

            DataTable DtZone = GlobalData.Instance.LoadZone();

            foreach (GridViewRow grdRow in GvArea.Rows)
            {
                bind_dropdownlist = (DropDownList)(GvArea.Rows[grdRow.RowIndex].Cells[3].FindControl("ddlZoneid"));
                bind_dropdownlist.Items.Clear();


                if (DtZone.Rows.Count > 0)
                {
                    bind_dropdownlist.DataValueField = "ZoneID";
                    bind_dropdownlist.DataTextField = "Zonename";
                    bind_dropdownlist.DataSource = DtZone;
                    bind_dropdownlist.DataBind();

                }
                bind_dropdownlist.Items.Insert(0, "--Select--");
                bind_dropdownlist.SelectedIndex = 0;
                bind_dropdownlist.SelectedValue = DtZone.Rows[0]["Zoneid"].ToString();

            }


        }
        protected void GvArea_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {


                #region  Begin  Code  for  Retrive  Data From  Gridview as on [14-10-2013]
                Label AreaId = GvArea.Rows[e.RowIndex].FindControl("lblAreaId") as Label;
                TextBox AreaName = GvArea.Rows[e.RowIndex].FindControl("txtArea") as TextBox;
                DropDownList DdlZone = GvArea.Rows[e.RowIndex].FindControl("ddlZoneid") as DropDownList;
                #endregion End  Code  for  Retrive  Data From  Gridview as on [14-10-2013]


                #region  Begin  Code  for  validaton as on [14-10-2013]
                if (AreaName.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter the Area AreaName');", true);
                    return;
                }
                #endregion End  Code  for  validaton as on [14-10-2013]


                #region  Begin  Code  for  Variable  Declaration as on [14-10-2013]
                var Areaname = string.Empty;
                var ProcedureName = string.Empty;
                var IRecordStatus = 0;
                Hashtable HtSPParameters = new Hashtable();
                #endregion End  Code  for  Variable  Declaration as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  Variables as on [14-10-2013]
                Areaname = AreaName.Text.Trim();
                ProcedureName = "ModifyArea";
                #endregion End  Code  for  Assign Values to  Variables as on [14-10-2013]

                #region  Begin  Code  for  Assign Values to  SP Parameters as on [14-10-2013]
                HtSPParameters.Add("@AreaName", Areaname.ToUpper());
                HtSPParameters.Add("@AreaId", AreaId.Text);
                HtSPParameters.Add("@ZoneId", DdlZone.SelectedValue);
                #endregion End  Code  for  Assign Values to  SP Parameters as on [14-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [14-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [14-10-2013]

                #region  Begin Code For Display Status Of the Record as on [14-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' AreaName  Updated  SucessFully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' AreaName Not  Updated.Because  The AreaName Already Exist. NOTE:Area Names Are UNIQUE');", true);
                }
                #endregion  End Code For Display Status Of the Record as on [14-10-2013]

                #region  Begin Code For Re-Call All the Area As on [29-03-2018]
                GvArea.EditIndex = -1;
                Displaydata();
                #endregion End Code For Re-Call All the Area As on [14-10-2013]

            }

            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Contact Admin.');", true);
                return;
            }
        }

        protected void Btn_Area_Click(object sender, EventArgs e)
        {

            try
            {


                #region Begin Code For  Validations   as [12-10-2013]
                if (Txt_Area.Text.Trim().Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Enter  AreaName.');", true);
                    return;
                }
                if (ddlZoneonDefault.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert('Please Select  Zone.');", true);
                    return;
                }
                #endregion Begin Code For  Validations as [12-10-2013]

                #region Begin Code For Variable Declaration as [12-10-2013]
                var AreaName = string.Empty;
                var IRecordStatus = 0;
                var zoneid = string.Empty;
                #endregion Begin Code For Variable Declaration as [12-10-2013]

                #region Begin Code For  Assign Values to Variable  as [12-10-2013]
                AreaName = Txt_Area.Text.Trim().ToUpper();
                zoneid = ddlZoneonDefault.SelectedValue;
                #endregion Begin Code For Assign Values to Variable as [12-10-2013]


                #region  Begin Code For Stored Procedure Parameters  as on [12-10-2013]

                Hashtable HtSPParameters = new Hashtable();
                var ProcedureName = "AddArea";
                #endregion End Code For Stored Procedure Parameters  as on [12-10-2013]

                #region  Begin Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]
                HtSPParameters.Add("@AreaName", AreaName);
                HtSPParameters.Add("@Zoneid", zoneid);
                #endregion  End  Code For Assign Values to the Stored Procedure Parameters as on [12-10-2013]

                #region  Begin Code For Calling Stored Procedure As on [12-10-2013]
                IRecordStatus = config.ExecuteNonQueryParamsAsync(ProcedureName, HtSPParameters).Result;
                #endregion  End Code For Calling Stored Procedure As on [12-10-2013]

                #region  Begin Code For Display Status Of the Record as on [12-10-2013]
                if (IRecordStatus > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' AreaName Added SucessFully.');", true);
                    Txt_Area.Text = string.Empty;
                    ddlZoneonDefault.SelectedIndex = 0;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "show alert", "alert(' AreaName Not  Added.Because  The AreaName Already Exist. NOTE:Area Names Are UNIQUE');", true);
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