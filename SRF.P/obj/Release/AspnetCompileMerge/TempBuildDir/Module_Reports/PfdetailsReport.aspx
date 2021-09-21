<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="PfdetailsReport.aspx.cs" Inherits="SRF.P.Module_Reports.PfdetailsReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;

        }
   

        function checkAll(objRef) {
        
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        inputList[i].checked = false;
                    }
                }
            }
        }
    
    </script>


     <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread">PF Details</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">PF Details
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <table width="70%" cellpadding="5" cellspacing="5">
                                            <tr>

                                                <td>Month :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                        TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                        ValidChars="/0123456789">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btn_Submit_Click" />
                                                </td>
                                                <td>
                                                    <div align="right">
                                                        <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                                        &nbsp; &nbsp; &nbsp;<asp:LinkButton ID="lbtn_Export_Text" runat="server" OnClick="lbtn_Export_Text_Click">Export to Text</asp:LinkButton>
                                                        &nbsp; &nbsp; &nbsp;<asp:LinkButton ID="lbtn_Export_PDF" runat="server" OnClick="lbtn_Export_PDF_Click">Export Consolidated</asp:LinkButton>

                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                    <asp:RadioButton ID="radiooldpfdetails" runat="server" Text="OLD" Checked="true"
                                                        GroupName="pfdetails" Visible="false" />
                                                </td>
                                            </tr>



                                        </table>
                                    </div>
                                    <div class="rounded_corners">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="90%" Style="margin: 0px auto"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None">
                                            <RowStyle BackColor="#EFF3FB" Height="30" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkindividual" runat="server" onclick="Check_Click(this)" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Client Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="clientname" HeaderText="Client Name" />

                                                 <asp:BoundField DataField="pf" HeaderText="PF Employee" />
                                                 <asp:BoundField DataField="pfempr" HeaderText="PF Employer" />
                                                 <asp:BoundField DataField="totalPF" HeaderText="Total PF" />


                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </div>

                                    <div class="rounded_corners">
                                        <div style="overflow: scroll; width: auto">
                                            <asp:GridView ID="GVListOfClients" runat="server" AutoGenerateColumns="False" Width="100%"
                                                CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None" OnRowDataBound="GVListOfClients_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <%-- <asp:TemplateField HeaderText="Member ID">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMemberid" Text="<%# Bind('Empid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Member ID" DataField="EmpEpfNo" />

                                                    <%-- <asp:TemplateField HeaderText="Member Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMemberName" Text="<%# Bind('Fullname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Member Name" DataField="Fullname" />


                                                    <%-- <asp:TemplateField HeaderText="Client Id">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientId" Text="<%# Bind('Clientid') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <%--   <asp:BoundField HeaderText="Client Id" DataField="Clientid"  />--%>


                                                    <%-- <asp:TemplateField HeaderText="Client Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblClientname" Text="<%# Bind('Clientname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <%--  <asp:BoundField HeaderText="Client Name" DataField="Clientname"  />--%>


                                                    <%--  <asp:TemplateField HeaderText="No of Duties">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblNoofduties" Text="<%# Bind('NoOfDuties') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <%--  <asp:BoundField HeaderText="No of Duties" DataField="NoOfDuties"  />--%>


                                                    <%--  <asp:TemplateField HeaderText="Pf No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpEpfNo" Text="<%# Bind('EmpEpfNo') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <%--<asp:BoundField HeaderText="Pf No." DataField="EmpEpfNo"  />--%>


                                                    <%--  <asp:TemplateField HeaderText="EPF Wages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEpfwages" Text="<%# Bind('PFWAGES') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="EPF Wages" DataField="PFWAGES" />


                                                    <%--   <asp:TemplateField HeaderText="EPF Wages">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEpfwages1" Text="<%# Bind('PFWAGES') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="EPS Wages" DataField="EPSWAGES" />


                                                    <%-- <asp:TemplateField HeaderText="EPF Contribution (EE Share due)">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPF" Text="<%# Bind('PF') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="EPF Contribution (EE Share) due" DataField="PF" />


                                                    <%--                                                <asp:TemplateField HeaderText="EPF Contribution (EE Share) being remitted">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPF1" Text="<%# Bind('pf') %>"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>--%>


                                                    <asp:BoundField HeaderText="EPF Contribution (EE Share) being remitted" DataField="pf" />



                                                    <%--   <asp:TemplateField HeaderText="EPS contibution due">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPFEmpr" Text="<%# Bind('PFEmpr') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="EPS contibution due" DataField="EPSDue" />


                                                    <%-- <asp:TemplateField HeaderText="EPS contibution being remitted">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPFEmpr1" Text="<%# Bind('PFEmpr') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="EPS contibution being remitted" DataField="EPSDue" />



                                                    <%--<asp:TemplateField HeaderText="Diff EPF and EPS Contribution (ER Share) due">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPfDiff" Text="<%# Bind('PfDiff') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>


                                                    <asp:BoundField HeaderText="Diff EPF and EPS Contribution (ER Share) due" DataField="PfDiff" />


                                                    <%-- <asp:TemplateField HeaderText="Diff EPF and EPS Contribution (ER Share) being remitted">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPfDiff1" Text="<%# Bind('PfDiff') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Diff EPF and EPS Contribution (ER Share) being remitted" DataField="PfDiff" />




                                                    <%-- <asp:TemplateField HeaderText="Father's/ Husband's Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFathername" Text="<%# Bind('EmpFatherName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:BoundField HeaderText="NCP Days" DataField="NCPDAYS" NullDisplayText=" " />
                                                    <asp:BoundField HeaderText="Refund of advances" DataField="ADVREF" NullDisplayText=" " />
                                                    <asp:BoundField HeaderText="Arrear EPF Wages" DataField="ARREAREPF" NullDisplayText=" " />

                                                    <asp:BoundField HeaderText="Arrear EPF EE Share" DataField="ARREAREPFEE" NullDisplayText=" " />

                                                    <asp:BoundField HeaderText="Arrear EPF ER Share" DataField="ARREAREPFER" NullDisplayText=" " />
                                                    <asp:BoundField HeaderText="Arrear EPS" DataField="ARREAREPS" NullDisplayText=" " />


                                                    <asp:BoundField HeaderText="Father's/ Husband's Name" DataField="EmpFatherName" NullDisplayText=" " />

                                                    <asp:BoundField HeaderText="Relationship with the member" DataField="Relation" NullDisplayText=" " />




                                                    <asp:BoundField HeaderText="Date of Birth" DataField="EmpDtofBirth" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText=" " />
                                                    <%-- <asp:TemplateField HeaderText="Date of Birth">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDateofBirth" Text="<%# Bind('EmpDtofBirth') %>"></asp:Label>
                                            </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <%-- <asp:TemplateField HeaderText="Gender">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGender" Text="<%# Bind('EmpSex') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Gender" DataField="EmpSex" NullDisplayText=" " />


                                                    <asp:BoundField HeaderText="Date of Joining EPF" DataField="EmpPFEnrolDt" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText=" " />

                                                    <%-- <asp:TemplateField HeaderText="Date of Joining EPF">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDJEpf" Text="<%# Bind('EmpPFEnrolDt')%>"></asp:Label>
                                            </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <%--  <asp:TemplateField HeaderText="Date of Joining EPS">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDJEps" Text=" "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Date of Joining EPS" DataField="EmpPFEnrolDt" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText=" " />



                                                    <%-- <asp:TemplateField HeaderText="Date of exit from EPF">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDEEpf" Text=" "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Date of exit from EPF" DataField="empdtofleaving" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText=" " />

                                                    <%--                                                <asp:TemplateField HeaderText="Date of exit from EPS">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDEEPS" Text=" "></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                    <asp:BoundField HeaderText="Date of exit from EPS" DataField="empdtofleaving" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText=" " />




                                                    <asp:BoundField HeaderText="Reason for Leaving" DataField="Reason" NullDisplayText=" " />

                                                    <%-- <asp:BoundField HeaderText="Date of Joining" DataField="EmpDtofJoining" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText="" />--%>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
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