<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="FullandFinal.aspx.cs" Inherits="SRF.P.Module_Reports.FullandFinal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
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
                    <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">FULL AND FINAL</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                               FULL AND FINAL
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="85%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                                Employee ID<span style="color: Red">*</span> </td>
                                              <td>  <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlEmployee" class="sdrop"
                                                    OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Employee Name<span style="color: Red">*</span></td>
                                               <td> <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlempname" class="sdrop"
                                                    OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnSearchFullandFinal" Text="Submit" class="btn save" OnClick="btnSearchFullandFinal_Click"
                                                    />
                                            </td>
                                             
                                        </tr>
                                        
                                    </table>
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
