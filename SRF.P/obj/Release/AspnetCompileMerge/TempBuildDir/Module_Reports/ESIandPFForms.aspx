<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ESIandPFForms.aspx.cs" Inherits="SRF.P.Module_Reports.ESIandPFForms" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>

      <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Client Forms</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Client Forms
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">

                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>

                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <table width="100%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Forms
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlForms" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlForms_SelectedIndexChanged" class="sdrop">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>ESI Report</asp:ListItem>
                                                        <asp:ListItem>ESI Upload Report</asp:ListItem>
                                                         <asp:ListItem>PF Report</asp:ListItem>
                                                        <asp:ListItem>UnitWise PF ESI PT Report</asp:ListItem>
                                                         <asp:ListItem>EmployeeWise PF ESI PT Report</asp:ListItem>
                                                        

                                                    </asp:DropDownList>
                                               </td>
                                            </tr>



                                        </table>
                                        <div style="float: right">
                                            <asp:Button runat="server" ID="btn_Submit" Visible="false" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <!-- DASHBOARD CONTENT END -->
    
        </div>

</asp:Content>
