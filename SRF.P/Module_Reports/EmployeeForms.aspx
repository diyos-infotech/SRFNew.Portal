<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmployeeForms.aspx.cs" Inherits="SRF.P.Module_Reports.EmployeeForms" %>
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
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Employee Forms</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                              Employee Forms
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 100%">
                                    <table width="120%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td>
                                            Forms</td>
                                             <td>  <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlForms" class="sdrop"
                                                    OnSelectedIndexChanged="ddlForms_SelectedIndexChanged">
                                                 <asp:ListItem>--Select--</asp:ListItem>
                                                 <asp:ListItem>Form Q</asp:ListItem>
                                                 <asp:ListItem>Form F (Leave Wages)</asp:ListItem>
                                                 <asp:ListItem>Form F (Gratuity)</asp:ListItem>
                                                 <asp:ListItem>Form A</asp:ListItem>
                                                 <asp:ListItem>Police Verification Form</asp:ListItem>
                                                 <asp:ListItem>EMPLOYMENT CARD</asp:ListItem>
                                               <%--  <asp:ListItem>Form 5</asp:ListItem>
                                                 <asp:ListItem>Form 13</asp:ListItem>
                                                 <asp:ListItem>Declaration</asp:ListItem>
                                                 <asp:ListItem>Form-3A</asp:ListItem>--%>
                                               

                                                </asp:DropDownList>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lblempid" runat="server" Text="Employee ID " Visible="false"></asp:Label> </td>
                                              <td>  <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlEmployee" class="sdrop" Visible="false"
                                                    OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                               <asp:Label ID="lblempname" runat="server" Text=" Employee Name " Visible="false"></asp:Label></td>
                                               <td> <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlempname" class="sdrop" Visible="false"
                                                    OnSelectedIndexChanged="ddlempname_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                           
                                             </tr>
                                             <tr >
                                                        <td style="width: 100px">
                                                            <asp:Label ID="lblfrom" runat="server" Text="From" Visible="false"></asp:Label>
                                                        </td>

                                                        <td>
                                                            <asp:TextBox ID="txtfrom" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtfrom">
                                                            </cc1:CalendarExtender>
                                                        </td>

                                                       
                                                        <td style="width: 100px">
                                                            <asp:Label ID="lblto" runat="server" Text="To" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtto" runat="server" CssClass="sinput" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtto">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                    </tr>
                                        <tr>
                                             <td>
                                                            <asp:Label ID="lblmonth" runat="server" Text="Month" Visible="false"></asp:Label>

                                                </td>
                                         <td>
                                                    <asp:TextBox ID="TxtMonth" Width="120px" runat="server" AutoPostBack="true" class="sinput"
                                                        Text="" Visible="false"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="TxtMonth_CalendarExtender" runat="server"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="TxtMonth">
                                                    </cc1:CalendarExtender>

                                                </td>

                                             <td>
                                                    <asp:Label ID="lblDOJ" runat="server" Text="  Date Of Joining" Visible="false" ></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpDtofJoining" runat="server" Text="" class="sinput" Visible="false" ></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                        TargetControlID="txtEmpDtofJoining" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI1" runat="server" Enabled="True" TargetControlID="txtEmpDtofJoining"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>


                                              <td>
                                                    <asp:Label ID="lblDOL" runat="server" Text="  Date Of Leaving" Visible="false" Style="margin-left: -124px"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpDtofLeaveing" runat="server" Text="" class="sinput" Visible="false" Style="margin-left: -377px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                        TargetControlID="txtEmpDtofJoining" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOL1" runat="server" Enabled="True" TargetControlID="txtEmpDtofLeaveing"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                             
                                       </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                           
                                        </tr>
                                    </table>
                                    <asp:Button runat="server" ID="BtnSubmit" Text="Submit" class="btn save" OnClick="btnForms_Click" style="float:right;margin-right:90px"
                                                    />
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