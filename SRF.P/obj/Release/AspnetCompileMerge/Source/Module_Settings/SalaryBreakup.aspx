<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="SalaryBreakup.aspx.cs" Inherits="SRF.P.Module_Settings.SalaryBreakup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <link href="css/global.css" rel="stylesheet" type="text/css" />

       <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Salary Breakup Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Salary Breakup Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                            
                              
                                                <table width="100%" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="5" cellspacing="5">
                                                                <tr>
                                                                    <td>
                                                                        Select Designation
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList runat="server" ID="ddlDesignations" AutoPostBack="True" class="sdrop"
                                                                            OnSelectedIndexChanged="ddlDesignations_SelectedIndexChanged" TabIndex="0">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Basic :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtBasic" TabIndex="1" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtBasic"
                                                                            ErrorMessage="Please Enter Basic in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        HRA :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtHRA" TabIndex="3" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtHRA"
                                                                            ErrorMessage="Please Enter HRA in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Washing Allowance :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtWashAllo" TabIndex="5" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtWashAllo"
                                                                            ErrorMessage="Please Enter Wasing Allowance in Numbers" Style="z-index: 101;
                                                                            left: 450px; position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Conveyance :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtConceyance" TabIndex="7" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtConceyance"
                                                                            ErrorMessage="Please Enter Conceyance in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table cellpadding="5" cellspacing="5">
                                                                <tr>
                                                                    <td>
                                                                        CTC :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCTC" TabIndex="9" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCTC"
                                                                            ErrorMessage="Please Enter CTC in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        DA :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtDA" TabIndex="2" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtDA"
                                                                            ErrorMessage="Please Enter DA in Numbers" Style="z-index: 101; left: 450px; position: absolute;
                                                                            top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        CCA :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCCA" TabIndex="4" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtCCA"
                                                                            ErrorMessage="Please Enter CCA in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Other Allowances :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtOtherAllo" TabIndex="6" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtOtherAllo"
                                                                            ErrorMessage="Please Enter Other Allowance in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Bonus :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtBonus" TabIndex="8" class="sinput"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtBonus"
                                                                            ErrorMessage="Please Enter Bonus in Numbers" Style="z-index: 101; left: 450px;
                                                                            position: absolute; top: 550px" ValidationExpression="^\d+$" ValidationGroup="check"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                                
                                                    <div style="float: right; margin-right: 160px">
                                <asp:Label ID="lblresult" runat="server" Visible="false" Text="" Style="color: Red"></asp:Label>
                                <asp:Button ID="btnsave" runat="server" ValidationGroup="a1" Text="SAVE" ToolTip="SAVE"
                                    OnClientClick='return confirm(" Are you sure you want to update  the record?");'
                                    class=" btn save" OnClick="btnsave_Click" />
                                <asp:Button ID="btncancel" runat="server" ValidationGroup="a1" Text="CANCEL" ToolTip="CANCEL"
                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry?");'
                                    class=" btn save" OnClick="btncancel_Click" />
                            </div><br/><br />
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvSalaryBreakup" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    Height="50%" Style="text-align: center" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                                    GridLines="None" AllowPaging="True" OnRowDeleting="gvcreatelogin_RowDeleting"
                                                    OnPageIndexChanging="gvSalaryBreakup_PageIndexChanging">
                                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Designation">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblempid" runat="server" Text=" <%#Bind('Design')%>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="40px"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Basic" HeaderText="Basic" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DA" HeaderText="DA" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HRA" HeaderText="HRA" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CCA" HeaderText="CCA" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="WashAllowance" HeaderText="Washing Allowance" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="OtherAllowance" HeaderText="Other Allowance" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Conveyance" HeaderText="Conveyance" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Bonus" HeaderText="Bonus" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CTC" HeaderText="CTC" HeaderStyle-Width="220px">
                                                            <HeaderStyle Width="220px" />
                                                        </asp:BoundField>
                                                        <%--  <asp:TemplateField ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkdelete" runat="server" CommandName="delete" Text="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40px"></ItemStyle>
                                                </asp:TemplateField>--%>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" HorizontalAlign="Center" BorderWidth="1px" CssClass="GridPager" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
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
    
        <!-- CONTENT AREA END -->
    </div>

</asp:Content>
