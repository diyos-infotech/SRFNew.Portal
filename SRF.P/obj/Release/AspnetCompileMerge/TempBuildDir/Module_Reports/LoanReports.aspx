<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="LoanReports.aspx.cs" Inherits="SRF.P.Module_Reports.LoanReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 135px;
        }
    </style>

    <script type="text/javascript">

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }
		
    </script>

      <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                    <li class="active"><a href="LoanReports.aspx" style="z-index: 7;" class="active_bread">
                        LOANS</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                LOANS
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div style="margin-left:20px">
                                <div class="dashboard_firsthalf" style="width: 100%; display:none;">
                                    <table>
                                    <tr>
                                        <td>
                                         <asp:Label ID="LblFromDate" runat="server" Text="From Date" > </asp:Label>
                                         </td>
                                        <td>
                                            <asp:TextBox ID="txtStrtDate" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtStrtDate" Format="MM/dd/yyyy">
                                            </cc1:CalendarExtender>
                                     <cc1:FilteredTextBoxExtender ID="FTBEDOI"
                                          runat="server" Enabled="True" TargetControlID="txtStrtDate"
                                           ValidChars="/0123456789">
                                           </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        
                    Designation
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server"  ID="ddlDesignation"  Width="155px"
                                                onselectedindexchanged="ddlDesignation_SelectedIndexChanged" >
                                                <asp:ListItem> Choose Designation</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                      <tr>
                                                    <td>
                                                    Loan Type<span style=" color:Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlLoanType" Width="153px">
                                                        <asp:ListItem>--Select--</asp:ListItem>
                                                        <asp:ListItem>Sal. Adv</asp:ListItem>
                                                        <asp:ListItem>Uniform</asp:ListItem>
                                                        <asp:ListItem>Others</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                    
                                    
                                    <tr> 
                                    
                                    <td colspan="2">
                                            
                            <asp:Label  ID="LblResult" runat="server" Text=""  style=" color:Red"/>
                   
                                     </td>
                                    </tr>
                                    
                                    
                                </table>
                                </div>
                                
                                <div class="dashboard_secondhalf" style="display: none">
                                    <table>
                                    <tr>
                                        <td>
                                          <asp:Label ID="LblToDate" runat="server" Text=" To Date" > </asp:Label></td><td>
                                            <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                Format="MM/dd/yyyy">
                                            </cc1:CalendarExtender>
                                           <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                          runat="server" Enabled="True" TargetControlID="txtEndDate"
                                           ValidChars="/0123456789">
                                           </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" 
                                                class="btn save"  />
                                        </td>
                                    </tr>
                                  
                                    
                                    
                                    <tr>
                                        <td>
                                      Loan Amount : </td>
                                      <td>
                                     <asp:DropDownList ID="ddlAmount" runat="server" Width="155px">
                                    <asp:ListItem>--Select-- </asp:ListItem>
                                    <asp:ListItem>0-1000</asp:ListItem>
                                      <asp:ListItem>1000-5000</asp:ListItem>
                                        <asp:ListItem>5000-10000</asp:ListItem>
                                          <asp:ListItem>10000-Above</asp:ListItem>
                                     
                                    </asp:DropDownList>
                                        </td>
                                      </tr>
                                        </table>
                                </div>
                    <div class="dashboard_firsthalf" style="width:750px; margin-right:120px;">
                        <table width="100%" cellpadding="5" cellspacing="5">
                        <tr>
                            <td>
                                <div style="margin-bottom:32px">
                               <asp:Label runat="server" ID="lblLoanOperation" Text="Loan Operations:" Width="125px"></asp:Label> 
                               
                               <asp:DropDownList ID="ddlloanoperations" runat="server"  Width="175px" OnSelectedIndexChanged="ddlloanoperations_OnSelectedIndexChanged">
                               <asp:ListItem >--Select--</asp:ListItem>
                               <asp:ListItem >Issued Loans</asp:ListItem>
                               <asp:ListItem >Pending Loans</asp:ListItem>
                               <asp:ListItem >Deducted Loans</asp:ListItem>
                               <asp:ListItem >Search Options</asp:ListItem>
                               </asp:DropDownList>
                               </div>
                           </td>
                            <td>
                                <div style="margin-bottom:32px;">
                                <asp:Label runat="server" ID="lblissuedloans" Text="Select Options:" Width="125px"></asp:Label>
                                <asp:DropDownList ID="ddlissuedloans" runat="server" Width="175px" OnSelectedIndexChanged="ddlissuedloans_OnSelectedIndexChanged" >
                                <asp:ListItem>Monthly Wise</asp:ListItem>
                                <asp:ListItem>Loan Type Wise</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </td>
                         </tr>
                         <tr>
                            <td>
                                <asp:Label runat="server" ID="lblLoantype" Text="Select Loan Type :" Width="125px"></asp:Label>
                                <asp:DropDownList  ID="ddlloantypes" runat="server" Width="175px">
                                <asp:ListItem>ALL</asp:ListItem>
                                <asp:ListItem>Sal.Adv</asp:ListItem>
                                <asp:ListItem>Uniform</asp:ListItem>
                                <asp:ListItem>Other Loans</asp:ListItem>
                                <asp:ListItem>Mobile Deductions</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSelectmonth" Text="Select Month:" Width="125px"></asp:Label>
                                <asp:TextBox ID="txtloanissue" runat="server"  Width="145px"> </asp:TextBox>
                                <cc1:CalendarExtender ID="CEloanissue" runat="server"  Enabled="true" 
                                TargetControlID="txtloanissue" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                <cc1:FilteredTextBoxExtender ID="FTBEloanissue" runat="server" Enabled="True" 
                                TargetControlID="txtloanissue" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender> 
                            </td>
                         </tr>
                         <tr><td></td>
                            <td>
                                <div style="margin-left:190px">
                                <asp:Button ID="Btn_Search_Loans" runat="server" OnClick="Btn_Search_Loans_Click"  Text="Search" 
                                style="float:right"  class="btn save"/>
                                </div>
                            </td>
                         </tr>
                         </table>                  
                           
                           
                    </div> 
                                <div class="rounded_corners" style="overflow:auto">
                                    <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CssClass="datagrid" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowDataBound="GVListEmployees_RowDataBound" ShowFooter="true">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                    
                                    
                                     <asp:TemplateField HeaderText="S.No"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            <asp:Label ID="lblSno" runat="server"  Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                            </EditItemTemplate>
                                   </asp:TemplateField>
                                            
                                    
                                        <asp:TemplateField HeaderText="Loan Id">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloanno" Text="<%# Bind('LoanNo') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                      <asp:TemplateField HeaderText="Loan Type">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloantype" Text="<%# Bind('TypeOfLoan') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        
                                             <asp:TemplateField HeaderText="empid">
                                         <ItemTemplate>
                                                <asp:Label runat="server" ID="lblempid" Text="<%# Bind('EmpId') %>"></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblempmname" Text="<%# Bind('Fullname') %>"></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                                <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbldesignation" Text="<%# Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                        <asp:TemplateField HeaderText="Loan Amount">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloanamount" Text="<%# Bind('LoanAmount') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                             
                                             
                                      <asp:TemplateField HeaderText="No Of Installments">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpLastName" Text="<%# Bind('NoInstalments') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                             
                                        <asp:TemplateField HeaderText="Amount To Be Deducted">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblamounttobededucted" Text="<%# Bind('AmountTobeDeducted') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                             
                                        <asp:TemplateField HeaderText="Amount Deducted">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblamountdeducted" Text="<%# Bind('AmountDeducted') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                             
                                             <asp:TemplateField HeaderText="Due Amount">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbldueamount" Text="<%# Bind('DueAmount') %>"></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                                  <asp:TemplateField HeaderText="Loan Issue Date">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloanissuedate"
                                                 Text='<%#Eval("LoanIssedDate", "{0:dd/MM/yyyy}")%>'
                                                     ></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                             
                                                  <asp:TemplateField HeaderText="Loan Cutting Month">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloancuttingmonth" 
                                                 Text='<%#Eval("LoanCuttingMonth", "{0:dd/MM/yyyy}")%>'
                                                 ></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                           <%-- <asp:TemplateField HeaderText="Loan Description">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblloandescription" Text="<%# Bind('LoanType') %>"></asp:Label>
                                            </ItemTemplate>
                                             </asp:TemplateField>--%>
                                             
                                               <asp:TemplateField HeaderText="Deduction Details">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkloandetails" runat="server" Text="Deduction Details"></asp:LinkButton>
                                            </ItemTemplate>
                                             </asp:TemplateField>
                                    
                                        <asp:TemplateField HeaderText="Loan Status">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpDesignation"   Text="<%# Bind('LoanStatus') %>" ></asp:Label>
                                            </ItemTemplate>
                                               </asp:TemplateField>
                                          </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                           </div>
                                 </div>
                                <div>
                                    <table width="100%">
                                        <tr style="width: 100%; font-weight: bold">
                                            <td style="width: 50%" align="right">
                                                <asp:Label ID="lbltamttext" runat="server" Text="&nbsp;&nbsp;Total Amount :"></asp:Label>
                                            </td>
                                            <td style="width: 62%">
                                                <asp:Label ID="lbltmt" runat="server" Text="" Style="margin-left: 2%"></asp:Label>
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
      
    </div>

</asp:Content>