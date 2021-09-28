<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="IDCardReport.aspx.cs" Inherits="SRF.P.Module_Reports.IDCardReport" %>
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
                        <li><a href="EmployeeReports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">ID Card Report</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">ID Card Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                                    <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="false">Export to Excel</asp:LinkButton>
                                                    </div>
        
                                    
                            
                            <table width="70%" cellpadding="5" cellspacing="5">
                                    <%-- <table cellpadding="5" cellspacing="5" width="60%" style="margin: 10px">--%>

                                         <tr>
                                                                              </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfrom" runat="server" Text="From" ></asp:Label>
                                                </td>

                                               <td>
                                                <asp:TextBox ID="Txt_From_Date" runat="server" class="sinput" ></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_From_Date" runat="server" Enabled="True" TargetControlID="Txt_From_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                                 
                                            </td>
                                                <td></td>

                                                <td>
                                                    <asp:Label ID="lblto" runat="server" Text="To" ></asp:Label>
                                                </td>
                                                 <td>
                                                 <asp:TextBox ID="Txt_To_Date"  runat="server" class="sinput" ></asp:TextBox>
                                                <cc1:CalendarExtender ID="CE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FTBE_To_Date" runat="server" Enabled="True" TargetControlID="Txt_To_Date"
                                                    ValidChars="/0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                                
                                                &nbsp;
                                            </td>
                                          

                                                <td>
                                                    <div align="right">
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                                </div>

                                                </td>
                                                
                                            </tr>
                                        </table>


                                        <%--  <asp:HiddenField ID="hidGridView" runat="server" />--%>
                                        <asp:GridView ID="GVIDCard" runat="server" AutoGenerateColumns="true"
                                            EmptyDataText="No Records Found" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" >
                                            <Columns>
                                            </Columns>
                                        </asp:GridView>
                                   

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
