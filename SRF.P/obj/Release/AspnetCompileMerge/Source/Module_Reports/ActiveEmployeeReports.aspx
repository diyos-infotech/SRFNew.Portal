<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ActiveEmployeeReports.aspx.cs" Inherits="SRF.P.Module_Reports.ActiveEmployeeReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Load.css" rel="stylesheet" type="text/css" />
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

        .completionList {
            background: white;
            border: 1px solid #DDD;
            border-radius: 3px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            min-width: 165px;
            height: 120px;
            overflow: auto;
        }

        .listItem {
            display: block;
            padding: 5px 5px;
            border-bottom: 1px solid #DDD;
        }

        .itemHighlighted {
            color: black;
            background-color: rgba(0, 0, 0, 0.1);
            text-decoration: none;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            border-bottom: 1px solid #DDD;
            display: block;
            padding: 5px 5px;
        }
    </style>

    <script language="javascript">
        function OnFocus(txt, text) {
            if (txt.value == text) {
                txt.value = "";
            }
        }


        function OnBlur(txt, text) {
            if (txt.value == "") {
                txt.value = text;
            }
        }
    </script>

       <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="ActiveEmployeeReports.aspx" style="z-index: 7;" class="active_bread">List of Employees</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">LIST OF EMPLOYEES
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <%--<div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" >Export to Excel</asp:LinkButton>
                            </div>--%>

                                    <div class="dashboard_firsthalf" style="width: 650px;">
                                        <br />
                                        <table width="150%" border="0">
                                            <tr>
                                                <td width="90px">Search Mode :
                                                </td>
                                                <td width="190px">

                                                    <div style="margin-bottom: 3px;">
                                                        <asp:DropDownList ID="ddlActiveEmp" runat="server" Width="157px"
                                                            OnSelectedIndexChanged="ddlActiveEmp_SelectedIndexChanged" AutoPostBack="true">
                                                        <%--<cc1:ComboBox ID="ddlActiveEmp" Height="23px" Width="157px" runat="server" AutoPostBack="True"
                                                            BorderStyle="Solid" BorderColor="#F0F0F0" OnSelectedIndexChanged="ddlActiveEmp_SelectedIndexChanged"
                                                            AutoCompleteMode="SuggestAppend" RenderMode="Block" DropDownStyle="DropDownList">--%>
                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="All"></asp:ListItem>
                                                            <asp:ListItem Text="Active"></asp:ListItem>
                                                            <asp:ListItem Text="InActive"></asp:ListItem>
                                                            <asp:ListItem Text="Absconding"></asp:ListItem>
                                                            <asp:ListItem Text="EmpId"></asp:ListItem>
                                                            <asp:ListItem Text="EmpName"></asp:ListItem>
                                                            <asp:ListItem Text="Designation"></asp:ListItem>
                                                            <asp:ListItem Text="JoiningDate"></asp:ListItem>
                                                            <asp:ListItem Text="LeavingDate"></asp:ListItem>
                                                            <asp:ListItem Text="NonPfDeduct"></asp:ListItem>
                                                            <asp:ListItem Text="NonESIdeduct"></asp:ListItem>
                                                            <asp:ListItem Text="noPFNumber"></asp:ListItem>
                                                            <asp:ListItem Text="noESINumber"></asp:ListItem>
                                                            <asp:ListItem Text="noBankA/CNumber"></asp:ListItem>
                                                            <asp:ListItem Text="PFNumber"></asp:ListItem>
                                                            <asp:ListItem Text="ESINumber"></asp:ListItem>
                                                            <asp:ListItem Text="BankA/CNumber"></asp:ListItem>
                                                            <asp:ListItem Text="EmployeeDetails"></asp:ListItem>
                                                            <asp:ListItem Text="EmployeeAadharDetails"></asp:ListItem>
                                                            <asp:ListItem Text="EmployeeNomineeDetails"></asp:ListItem>
                                                            <asp:ListItem Text="PSTDataFormat"></asp:ListItem>
                                                            

                                                        </asp:DropDownList>
                                                    </div>

                                                </td>
                                                <asp:Panel ID="panelfromto" runat="server" Visible="false">

                                                    <td>
                                                        <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfrom" runat="server" Class="sinput"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="EmpIdtoAutoCompleteExtender" runat="server"
                                                            ServiceMethod="GetEmpID"
                                                            ServicePath="AutoCompleteAA.asmx"
                                                            MinimumPrefixLength="4"
                                                            CompletionInterval="100"
                                                            EnableCaching="true"
                                                            TargetControlID="txtfrom"
                                                            FirstRowSelected="false"
                                                            CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem"
                                                            CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtto" runat="server" class="sinput"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                            ServiceMethod="GetEmpID"
                                                            ServicePath="AutoCompleteAA.asmx"
                                                            MinimumPrefixLength="4"
                                                            CompletionInterval="100"
                                                            EnableCaching="true"
                                                            TargetControlID="txtto"
                                                            FirstRowSelected="false"
                                                            CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem"
                                                            CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>

                                                    </td>
                                                </asp:Panel>

                                                <asp:Panel ID="panelEmpIdFromTo" runat="server" Visible="false">

                                                    <td>
                                                        <asp:Label ID="lblfromEmpid" runat="server" Text="From EmpId"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromEmpid" runat="server" Class="sinput" Width="100px"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                            ServiceMethod="GetEmpID"
                                                            ServicePath="AutoCompleteAA.asmx"
                                                            MinimumPrefixLength="4"
                                                            CompletionInterval="100"
                                                            EnableCaching="true"
                                                            TargetControlID="txtFromEmpid"
                                                            FirstRowSelected="false"
                                                            CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem"
                                                            CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>
                                                    </td>

                                                    <td>
                                                        <asp:Label ID="lblToEmpid" runat="server" Text="To EmpId"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txttoEmpid" runat="server" class="sinput"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server"
                                                            ServiceMethod="GetEmpID"
                                                            ServicePath="AutoCompleteAA.asmx"
                                                            MinimumPrefixLength="4"
                                                            CompletionInterval="100"
                                                            EnableCaching="true"
                                                            TargetControlID="txttoEmpid"
                                                            FirstRowSelected="false"
                                                            CompletionListCssClass="completionList"
                                                            CompletionListItemCssClass="listItem"
                                                            CompletionListHighlightedItemCssClass="itemHighlighted">
                                                        </cc1:AutoCompleteExtender>

                                                    </td>


                                                </asp:Panel>
                                                <td>
                                                    <asp:Panel ID="panelempid" runat="server" Visible="false">
                                                        <%-- <asp:TextBox ID="TextEmpid" runat="server" Text="Enter EmpId..." 
                          onfocus="OnFocus(this,'Enter EmpId...')" onblur="OnBlur(this,'Enter EmpId...')" ></asp:TextBox>--%>
                                                        <asp:TextBox ID="TextEmpid" runat="server" Text="Enter EmpId..."></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="Tbwmeempid" runat="server" TargetControlID="TextEmpid"
                                                            WatermarkText="Enter EmpId..."></cc1:TextBoxWatermarkExtender>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelemp" runat="server" Visible="false">
                                                        <%--   <asp:TextBox ID="TxtEmpname" runat="server" Text="Enter EmpName..."
                           onfocus="OnFocus(this,'Enter EmpName...')" onblur="OnBlur(this,'Enter EmpName...')" ></asp:TextBox>--%>
                                                        <asp:TextBox ID="TxtEmpname" runat="server" Text="Enter EmpName..."></asp:TextBox>
                                                        <cc1:TextBoxWatermarkExtender ID="Tbwmeempname" runat="server" TargetControlID="TxtEmpname"
                                                            WatermarkText="Enter EmpName..."></cc1:TextBoxWatermarkExtender>
                                                    </asp:Panel>
                                                    <asp:Panel ID="paneldesignation" runat="server" Visible="false">
                                                        <asp:DropDownList ID="ddldesgn" runat="server" Width="125px">
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelJdate" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtJdateFrom" runat="server" Text="Enter From Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                                        TargetControlID="TxtJdateFrom" Format="MM/dd/yyyy"></cc1:CalendarExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TxtLdateFrom"
                                                                        WatermarkText="From Date..."></cc1:TextBoxWatermarkExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                        TargetControlID="TxtJdateFrom" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtJdateTo" runat="server" Text="Enter To Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                                        TargetControlID="TxtJdateTo" Format="MM/dd/yyyy"></cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                        TargetControlID="TxtJdateTo" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="TxtJdateTo"
                                                                        WatermarkText="To Date..."></cc1:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelLdate" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtLdateFrom" runat="server" Text="Enter From Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtLFrom_CalendarExtender" runat="server" Enabled="true"
                                                                        TargetControlID="TxtLdateFrom" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TbwmeLdateFrom" runat="server" TargetControlID="TxtLdateFrom"
                                                                        WatermarkText="From Date..."></cc1:TextBoxWatermarkExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                        TargetControlID="TxtLdateFrom" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtLdateTo" runat="server" Text="Enter To Date..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtLto_CalendarExtender" runat="server" Enabled="true"
                                                                        TargetControlID="TxtLdateTo" Format="dd/MM/yyyy"></cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                        TargetControlID="TxtLdateTo" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                                    <cc1:TextBoxWatermarkExtender ID="TbwmeLdateTo" runat="server" TargetControlID="TxtLdateTo"
                                                                        WatermarkText="To Date..."></cc1:TextBoxWatermarkExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panelNonAtten" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="TxtAdatefrom" runat="server" Text="Enter Month..."></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="txtA_CalendarExtender" runat="server" Enabled="true" TargetControlID="TxtAdatefrom"
                                                                        Format="MM/yyyy"></cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                        TargetControlID="TxtAdatefrom" ValidChars="/0123456789"></cc1:FilteredTextBoxExtender>
                                                                </td>
                                                        </table>
                                                    </asp:Panel>



                                                </td>
                                                <td>
                                                    <asp:Button ID="Submit" Text="Search" class="btn save" runat="server" OnClick="Esearch_Click" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbtn_Export" runat="server" Visible="False" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>

                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="lbtn_Export1" runat="server" Visible="False" OnClick="lbtn_Export1_Click">Export to Excel</asp:LinkButton>

                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" Visible="False" OnClick="lbtn_Export2_Click">Export to Excel</asp:LinkButton>

                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton3" runat="server" Visible="False" OnClick="lbtn_Export3_Click">Export to Excel</asp:LinkButton>

                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" Visible="False" OnClick="lbtn_Export4_Click">Export to Excel</asp:LinkButton>

                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Visible="False" OnClick="lbtn_Export4_Click">Export to Excel</asp:LinkButton>

                                                </td>

                                                <td>
                                                    <asp:LinkButton ID="linkExcelNew" runat="server" OnClick="lbtn_ExportNew_Click">Export to Excel(New)</asp:LinkButton>

                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <div class="rounded_corners" style="overflow: auto">
                                            <asp:GridView ID="gvlistofemp" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="gvlistofemp_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempid" Text="<%# Bind('Empid') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempName" Text="<%# Bind('EmpFName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desgn" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempDesgn" Text="<%# Bind('EmpDesgn') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spouse name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblSpousename" Text="<%# Bind('EmpSpouseName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gender">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempGen" Text="<%# Bind('EmpSex') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Marital Status">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempMs" Text="<%# Bind('EmpMaritalStatus') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Pan Card No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblpan" Text="<%# Bind('PanCardNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Qualification">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblqualification" Text="<%# Bind('EmpQualification') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Identification Marks">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblidentification" Text="<%# Bind('EmpIdMark') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField HeaderText="Phone">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPhone" Text="<%# Bind('EmpPhone') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="FatherName">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempFname" Text="<%# Bind('EmpFatherName') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EmpDtofJoining" HeaderText="Date of Joining" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:BoundField DataField="EmpDtofBirth" HeaderText="Date of Birth" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:TemplateField HeaderText="Bank Ac.No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempBacc" Text="<%# Bind('EmpBankAcNo') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBankname" Text="<%# Bind('Bankname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PF No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPfno"  Text="<%#Bind('empepfno') %>" ></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ESI No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempEsino"  Text="<%#Bind('Empesino') %>"   ></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PF Deduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPdedu" 
                                                         Text="<%# Bind('EmpPFDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PT Deduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpstatus" 
                                                        Text="<%# Bind('EmpPTDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ESIDeduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempEdedu"
                                                            Text="<%# Bind('EmpESIDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Permanent Address" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPadd" Text="<%# Bind('EmpPermanentAddress') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Empstatus">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpstatus" Text="<%# Bind('Empstatus') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>

                                            <asp:GridView ID="GVEmpDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="GVEmpDetails_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp Code" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempid" Text="<%# Bind('Empid') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempName" Text="<%# Bind('Empfname') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Designation" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempDesgn" Text="<%# Bind('EmpDesgn') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spouse name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblSpousename" Text="<%# Bind('EmpSpouseName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Permanent Address" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPadd" Text="<%# Bind('EmpPermanentAddress') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Correspondence Address" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblCorrPadd" Text="<%# Bind('EmpPresentAddress') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Mobile No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempPhone" Text="<%# Bind('EmpPhone') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Qualification">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblqulification" Text="<%# Bind('EmpQualification') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Native State">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblnativestate" Text="<%# Bind('NativeState') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Father Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblfathername" Text="<%# Bind('EmpFatherName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Father Occupation">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblfatherocc" Text="<%# Bind('EmpFatherOccupation') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Mothers Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempmothername" Text="<%# Bind('EmpMotherName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="EmpDtofBirth" HeaderText="DOB" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="EmpDtofJoining" HeaderText="DOJ" DataFormatString="{0:dd/MM/yyyy}" />

                                                    <asp:TemplateField HeaderText="Experience">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblexperience" Text="<%# Bind('EmpPreviousExp') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Spouse Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblspousename" Text="<%# Bind('EmpSpouseName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Lang Known">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbllang" Text="<%# Bind('EmpLanguagesKnown') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="EmpHeight" HeaderText="Height" />
                                                    <asp:BoundField DataField="EmpWeight" HeaderText="Weight" />
                                                    <asp:BoundField DataField="EmpChestunex" HeaderText="Chest" />

                                                    <asp:TemplateField HeaderText="IdentityMark">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIdentityMark" Text="<%# Bind('idmark') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="MaritalStatus">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblMaritalStatus" Text="<%# Bind('EmpMaritalStatus') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="NomineeName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblNomineeName" Text="<%# Bind('RName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="RelationDesc">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRelationDesc" Text="<%# Bind('RType') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="RefOne">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRefOne" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="RefOneAdd">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRefOneAdd" Text="<%# Bind('refaddone') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="RefTwo">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRefTwo" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="RefTwoAdd">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRefTwoAdd" Text="<%# Bind('refaddtwo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PhoneNo">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPhoneNo" Text="<%# Bind('pePoliceStation') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PermanentAdrsPoliceStn">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPermanentAdrsPoliceStn" Text="<%# Bind('pePoliceStation') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PresentAdrsPoliceStn">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPresentAdrsPoliceStn" Text="<%# Bind('prPoliceStation') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="QualificationType">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblQualificationType" Text="<%# Bind('QualificationType') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="UnitName">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblUnitName" Text="<%# Bind('unitname') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Gender">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblGender" Text="<%# Bind('EmpSex') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="BloodGroup">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblBloodGroup" Text="<%# Bind('EmpBloodGroup') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="EmpDtofLeaving" HeaderText="DateOfLeaving" DataFormatString="{0:dd/MM/yyyy}" />


                                                    <asp:TemplateField HeaderText="EmpStatus">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEmpStatus" Text="<%# Bind('Empstatus') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Pan Card No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblpan" Text="<%# Bind('PanCardNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Identification Marks">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblidentification" Text="<%# Bind('EmpIdMark') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%-- 
                                                
                                                
                                                <asp:TemplateField HeaderText="Bank Ac.No">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempBacc" Text="<%# Bind('EmpBankAcNo') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBankname" Text="<%# Bind('Bankname') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PF No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPfno"  Text="<%#Bind('empepfno') %>" ></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ESI No.">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempEsino"  Text="<%#Bind('Empesino') %>"   ></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PF Deduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempPdedu" 
                                                         Text="<%# Bind('EmpPFDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PT Deduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpstatus" 
                                                        Text="<%# Bind('EmpPTDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ESIDeduct">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblempEdedu"
                                                            Text="<%# Bind('EmpESIDeduct') %>" ></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField HeaderText="Empstatus">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblEmpstatus" Text="<%# Bind('Empstatus') %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>

                                            <asp:GridView ID="gvnomineelist" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="gvnomineelist_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EmpCode" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempid" Text="<%# Bind('Empid') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="EmpName" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempName" Text="<%# Bind('EmpFName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="NomineeName" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblNomineeName" Text="<%# Bind('RName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RelationDesc">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRelationDesc" Text="<%# Bind('RType') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="empnomineedob" HeaderText="DateofBirth" DataFormatString="{0:dd/MM/yyyy}" />


                                                    <asp:TemplateField HeaderText="IsPFNominee">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIsPFNominee" Text="<%# Bind('emppfnominee') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IsESINominee">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIsESINominee" Text="<%# Bind('empESINominee') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="IsInsured">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIsInsured" Text="<%# Bind('Insured') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>

                                            <asp:GridView ID="gvlistofadhar" runat="server" AutoGenerateColumns="False" Width="100%"
                                                ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center" OnRowDataBound="gvlistofadharr_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempid" Text="<%# Bind('Empid') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblempName" Text="<%# Bind('EmpFName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ESI No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblEsiNo" Text="<%# Bind('EmpESINo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Voter ID Card Details" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblDetails" Text="<%# Bind('VoterIDNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Driving Licence" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblDriving" Text="<%# Bind('DrivingLicenseNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Gun Licence">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lbLicencec" Text="<%# Bind('gunlicencs') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PF No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPanNo" Text="<%# Bind('EmpEpfNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PAN No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIsESINominee" Text="<%# Bind('PanCardNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Aadhar Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblAdharName" Text="<%# Bind('AadharCardName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Aadhar No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblAdharNo" Text="<%# Bind('AadharCardNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="UAN No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblIsInsured" Text="<%# Bind('EmpUANNumber') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Police Verification No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPoliceVerificationNo" Text="<%# Bind('PoliceVerificationNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Psara No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPsaraNo" Text="<%# Bind('PsaraEmpCode') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Passport No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblPassportNo" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Ration Card Name">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRationCardName" Text="<%# Bind('RationCardName') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Ration Card No">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblRationCardNo" Text="<%# Bind('RationCardNo') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Qualification">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblqualification" Text="<%# Bind('EmpQualification') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Emp Identification Marks">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblidentification" Text="<%# Bind('EmpIdMark') %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>

                                        </div>

                                        <div class="rounded_corners" style="overflow: scroll">
                                            <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
                                                CellSpacing="2" CellPadding="5" ForeColor="#333333" GridLines="None"
                                                OnPageIndexChanging="GVListEmployees_PageIndexChanging" OnRowDataBound="GVEmpDetails_RowDataBound">
                                                <RowStyle BackColor="#EFF3FB" Height="30" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempid" runat="server" Text='<%#Bind("empid") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_FName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_fname" runat="server" Text='<%#Bind("EmpFName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_LName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_lname" runat="server" Text='<%#Bind("EmpLName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name_Chng_Appli_FName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblname_chng_appli_fname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name_Chng_Appli_LName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblname_chng_appli_lname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sex" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsex" runat="server" Text='<%#Bind("EmpSex") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="DOB" DataField="EmpDtofBirth" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <%-- <asp:TemplateField HeaderText="DOB" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldob" runat="server" Text='<%# Eval("EmpDtofBirth","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Place_Birth_Village_Town" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblplace_birth_village_town" runat="server" Text='<%#Bind("peTown") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Place_Birth_District" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblplace_birth_district" runat="server" Text='<%#Bind("pecity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Place_Birth_State" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblplace_birth_state" runat="server" Text='<%#Bind("pestate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Place_Birth_Country" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblplace_birth_country" runat="server" Text='<%#Bind("Nationality") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_Father_FName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_father_fname" runat="server" Text='<%#Bind("EmpFatherName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_Father_LName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_father_lname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_Mother_FName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_mother_fname" runat="server" Text='<%#Bind("EmpMotherName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appli_Mother_LName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblappli_mother_lname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spouse_FName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblspouse_fname" runat="server" Text='<%#Bind("EmpSpouseName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Spouse_LName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblspouse_lname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Address" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_address" runat="server" Text='<%#Bind("presentaddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_StreetName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_streetname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_Village_Twn" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_village_twn" runat="server" Text='<%#Bind("prTown") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_District" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_district" runat="server" Text='<%#Bind("prCity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_State" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_state" runat="server" Text='<%#Bind("prState") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_PoliceStn" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_policestn" runat="server" Text='<%#Bind("prPoliceStation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_TelNo" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_telno" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_Mobno" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_mobno" runat="server" Text='<%#Bind("prphone") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pst_Addr_PinCode" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPst_addr_pincode" runat="server" Text='<%#Bind("prPincode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date_Since_Residing" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate_since_residing" runat="server" Text='<%#Bind("prperiodofstay") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Address" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_address" runat="server" Text='<%#Bind("permanentaddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_StreetName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_streetname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_Village_Twn" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_village_twn" runat="server" Text='<%#Bind("peTown") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_District" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_district" runat="server" Text='<%#Bind("peCity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_State" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_state" runat="server" Text='<%#Bind("pestate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_PoliceStn" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_policestn" runat="server" Text='<%#Bind("pePoliceStation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_TelNo" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_telno" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_MobNo" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_mobno" runat="server" Text='<%#Bind("pephone") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pmt_Addr_PinCode" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPmt_addr_pincode" runat="server" Text='<%#Bind("pepincode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Incase_Stay_Abroad_Addr" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblincase_stay_abroad_addr" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Incase_Stay_Abroad_State" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblincase_stay_abroad_state" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Incase_Stay_Abroad_Country" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblincase_stay_abroad_country" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Incase_Stay_Abroad_PinCode" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblincase_stay_abroad_pincode" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Education" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbleducation" runat="server" Text='<%#Bind("EmpQualification") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Height" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblheight" runat="server" Text='<%#Bind("EmpHeight") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Previous_Employers_Name" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblprevious_employers_name" runat="server" Text='<%#Bind("CompAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Previous_Employers_Addr" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblprevious_employers_addr" runat="server" Text='<%#Bind("CompAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reason_Quiting_Job" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblreason_quiting_job" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Visible_Distinguishing_Mark" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblvisible_distinguishing_mark" runat="server" Text='<%#Bind("EmpIdMark1") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Citizen_Of_India" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcitizen_of_india" runat="server" Text='<%#Bind("Nationality") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Other_Citizenship" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblother_citizenship" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Off_CourtName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_off_courtname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Off_Case_No" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_off_case_no" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Offence" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_offence" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Pro_CourtName" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_pro_courtname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Pro_CaseNo" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_pro_caseno" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Pro_Offence" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_pro_offence" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Arrest_Courtname" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_arrest_courtname" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Arrest_Caseno" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_arrest_caseno" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Arrest_Offence" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_arrest_offence" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Category_Id" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcategory_Id" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Date_Joining_Agency" DataField="EmpDtofJoining" DataFormatString="{0:dd/MM/yyyy}" />

                                                    <%-- <asp:TemplateField HeaderText="Date_Joining_Agency" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate_joining_agency" runat="server" Text='<%#Bind("EmpDtofJoining") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Weight" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWeight" runat="server" Text='<%#Bind("EmpWeight") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eye_Color" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEye_Color" runat="server" Text='<%#Bind("EmpEyesColor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hair_color" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHair_color" runat="server" Text='<%#Bind("EmpHairColor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Blood_Group_Id" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblblood_group_id" runat="server" Text='<%#Bind("BloodGroupName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Off" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_off" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Proceeding" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_proceeding" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criminal_Arrest_Warrant" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcriminal_arrest_warrant" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earlier_Operated_Psa_Name" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblearlier_operated_psa_name" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earlier_Operated_Psa_Addr" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblearlier_operated_psa_addr" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Earlier_Operated_Psa_Licence" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblearlier_operated_psa_licence" runat="server" Text=""></asp:Label>
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
                                        <asp:Label ID="LblResult" runat="server" Text="" Style="color: red"></asp:Label>
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
