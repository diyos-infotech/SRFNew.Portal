<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ImportEmpLoanDetails.aspx.cs" Inherits="SRF.P.Module_Reports.ImportEmpLoanDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="css/global.css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
  
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
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

        

        .Grid, .Grid th, .Grid td
        {
            border:1px solid #ddd;
        }
    </style>

       <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Import Loan Details</a></li>
                    </ul>
                </div>
               
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                            Import Loan Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                     
                            <div class="dashboard_firsthalf" style="width: 100%">
                                
                                    <table width="60%" cellpadding="5" cellspacing="5">

                                     <tr >
                                         <td>
                                         
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="LinkSample_Click" >Export Sample Excel</asp:LinkButton>
                                            
                                            </td>
 </tr>
                                         <tr>
                                         
                                        <td style="width: 150px" > Select File:

                                        </td>
                                        <td width="20px">
                                          <asp:FileUpload ID="FlUploadLoanDetails" runat="server" />
                                        </td>
                                          <td>
                                                                    
                                                                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click"  ></asp:Button>
                                                                       

                                                                </td>

                                         </tr> 
                                         </table>
                                        </div>
                                           </div>




                                    
                                <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                    <div class="boxin">
                                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                        </asp:ScriptManager>
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                            <div style="padding: 10px">
                                               
                                                       <asp:GridView ID="GvInputEmpLoanDetails" runat="server" AutoGenerateColumns="False" Width="90%" Visible="false"
                                    ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center;margin: 0px auto;margin-top: 10px;">
                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                    <Columns>
                                        
                                          <asp:TemplateField HeaderText="ID NO">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtidno" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Loan Type">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtloantype" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtamount" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                          <asp:TemplateField HeaderText="NoofInstalments">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblinstalments" Text=" "></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="LoanIssuedDate">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloanissueddate" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="LoanCuttingFrom">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblcuttingmonth" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDescription" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                                            </Columns>
                                                            <HeaderStyle BackColor="#fcf8e3" Font-Bold="True" ForeColor="Black" Height="28px" />
                                                        </asp:GridView>
                                                        
                                                   
                                               <%-- <table style="float: Right;margin-top:10px">
                                                            <tr>
                                                               
                                                            </tr>
                                                        </table>
                                            --%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                    <div class="clear">
                    </div>


                    <!-- DASHBOARD CONTENT END -->
        
        <!-- CONTENT AREA END -->
    </div>

</asp:Content>
