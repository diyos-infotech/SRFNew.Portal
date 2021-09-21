<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmpBioData.aspx.cs" Inherits="SRF.P.Module_Reports.EmpBioData" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

      <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
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

        function bindautofilldesgs() {
            $(".txtautofillempid").autocomplete({
                source: eval($("#hdempid").val()),
                minLength: 4
            });
        }

    </script>


    <style type="text/css">
        .style1 {
            width: 135px;
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


      <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="EmpBioData.aspx" style="z-index: 7;" class="active_bread">EMPLOYEE BIO DATA</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">BIO DATA
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <asp:HiddenField ID="hdempid" runat="server" />
                                        <div style="width: 850px; margin: 40px 120px 0 80px">
                                            <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="margin-left: 150px">
                                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID" Width="60px"></asp:Label></td>
                                                            <td>


                                                                <%--<asp:DropDownList ID="ddlEmpID" runat="server" Width="150px" OnSelectedIndexChanged="ddlEmpID_SelectedIndexChanged"
                                                            AutoPostBack="True" TabIndex="1" AutoCompleteMode="SuggestAppend" CssClass="sinput" Height="25px">
                                                        </asp:DropDownList>--%>
                                                                <asp:TextBox ID="txtEmpid" runat="server" CssClass="sinput" AutoPostBack="true" OnTextChanged="txtEmpid_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="EmpIdtoAutoCompleteExtender" runat="server"
                                                                    ServiceMethod="GetEmpID"
                                                                    ServicePath="AutoCompleteAA.asmx"
                                                                    MinimumPrefixLength="4"
                                                                    CompletionInterval="100"
                                                                    EnableCaching="true"
                                                                    TargetControlID="TxtEmpID"
                                                                    FirstRowSelected="false"
                                                                    CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>

                                                            </td>
                                                            <td style="width: 100px">&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblempname" Text="Name" Width="50px"></asp:Label></td>

                                                            <td>
                                                                <%--<asp:DropDownList ID="ddlempname" runat="server" Width="150px" OnSelectedIndexChanged="ddlempname_SelectedIndexChanged" AutoPostBack="True" TabIndex="2"
                                                            AutoCompleteMode="SuggestAppend" CssClass="sinput" Height="25px">
                                                        </asp:DropDownList>--%>

                                                                <asp:TextBox ID="txtName" runat="server" CssClass="sinput" AutoPostBack="true" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                                <cc1:AutoCompleteExtender ID="EmpNameAutoCompleteExtender" runat="server"
                                                                    ServiceMethod="GetEmpName"
                                                                    ServicePath="AutoCompleteAA.asmx"
                                                                    MinimumPrefixLength="4"
                                                                    CompletionInterval="100"
                                                                    EnableCaching="true"
                                                                    TargetControlID="txtName"
                                                                    FirstRowSelected="false"
                                                                    CompletionListCssClass="completionList"
                                                                    CompletionListItemCssClass="listItem"
                                                                    CompletionListHighlightedItemCssClass="itemHighlighted">
                                                                </cc1:AutoCompleteExtender>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnEnrolment" runat="server" Text="Bio Data"
                                                            Style="margin-left: -50px" class="btn save" OnClick="btnEnrolmentForm_Click" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btndeclaration" runat="server" Text="Declaration"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnBioData_Click" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnESIForm" runat="server" Text="ESI Form"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnESIForm_Click" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnPFForm" runat="server" Text="PF Form"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnPFForm_Click" />
                                                    </td>

                                                    <td>
                                                        <%--<asp:Button ID="btnApplForm" runat="server" Text="Appointment Form"
                                                            Style="margin-left:20px" class="btn save" OnClick="btnApplForm_Click" />--%>

                                                        <asp:Button ID="btnPFForm11" runat="server" Text="PF Form11"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnPFForm11_Click" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnICICIForm" runat="server" Text="ICICI Form"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnICICIForm_Click" />
                                                    </td>

                                                    <td>
                                                        <asp:Button ID="btnpolicevrftn" runat="server" Text="Police Verf Form"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnPoliceVfctn_Click" Visible="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPoliceVeriftnNew" runat="server" Text="Police Verf Form" Style="margin-left: 20px" class="btn save" OnClick="btnPoliceVeriftnNew_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnEnrolmentnew" runat="server" Text="Bio Data New"
                                                            Style="margin-left: -50px" class="btn save" OnClick="btnEnrolmentFormNew_Click" />
                                                    </td>

                                                     <td>
                                                        <asp:Button ID="btnEnrolmentnew_IB" runat="server" Text="Bio Data-IB"
                                                            Style="margin-left: 20px" class="btn save" OnClick="btnEnrolmentnew_IB_Click" />
                                                    </td>


                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnAppointment" runat="server" Text="Appointment Letter"
                                                            Style="margin-left: -90px" class="btn save" OnClick="btnAppointment_Click" />
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnappointmentNew" runat="server" Text="Call Letter"
                                                            Style="margin-left: -98px" class="btn save" OnClick="btnappointmentNew_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnempcard" runat="server" Text="Employement Card"
                                                            Style="margin-left: -50px" class="btn save" OnClick="btnempcard_Click" Visible="false" />
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnformq" runat="server" Text="Form Q "
                                                            Style="margin-left: -250px" class="btn save" OnClick="btnformq_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnformf" runat="server" Text="Form F"
                                                            Style="margin-left: -132px" class="btn save" OnClick="btnformf_Click" />
                                                    </td>
                                                </tr>


                                            </table>


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
            
        </div>


</asp:Content>
