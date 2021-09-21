<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="UniformPDF.aspx.cs" Inherits="SRF.P.Module_Inventory.UniformPDF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
      <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
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

        function GetFromEmpid() {
            $('#txtfromempid').autocomplete({
                source: function (request, response) {


                    $.ajax({
                        url: 'Autocompletion.asmx/GetFormEmpIDs',
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,

            });
        }

        function GetToEmpid() {
            $('#txttoempid').autocomplete({
                source: function (request, response) {


                    $.ajax({
                        url: 'Autocompletion.asmx/GetFormEmpIDs',
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,

            });
        }

        function GetEmpid() {
            $('#txtEmpid').autocomplete({
                source: function (request, response) {


                    $.ajax({
                        url: 'Autocompletion.asmx/GetFormEmpIDs',
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify({
                            term: request.term,
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,
                select: function (event, ui) {

                    $("#txtEmpid").attr("data-Empid", ui.item.value); OnAutoCompletetxtEmpidchange(event, ui);
                }
            });
        }

        function GetEmpName() {

            $('#txtName').autocomplete({
                source: function (request, response) {
                    $.ajax({

                        url: 'Autocompletion.asmx/GetFormEmpNames',
                        method: 'post',
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify({
                            term: request.term
                        }),
                        datatype: 'json',
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (err) {
                            alert(err);
                        }
                    });
                },
                minLength: 4,
                select: function (event, ui) {
                    $("#txtName").attr("data-EmpName", ui.item.value); OnAutoCompletetxtEmpNamechange(event, ui);
                }
            });

        }

        function OnAutoCompletetxtEmpidchange(event, ui) {
            $('#txtEmpid').trigger('change');

        }
        function OnAutoCompletetxtEmpNamechange(event, ui) {
            $('#txtName').trigger('change');

        }

        $(document).ready(function () {

            GetEmpid();
            GetEmpName();
            GetFromEmpid();
            GetToEmpid();
        });

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
                        <li class="first"><a href="ViewItems.aspx" style="z-index: 9;"><span></span>Inventory</a></li>
                        <li class="active"><a href="UniformPDF.aspx" style="z-index: 8;">Uniform PDF</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">UNIFORM PDF
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <asp:HiddenField ID="hdempid" runat="server" />
                                        <div>

                                            <table style="width: 100%">

                                                <tr>

                                                    <td>
                                                        <asp:Label runat="server" ID="lblempid" Width="50px" Text="Emp ID"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpid" runat="server" CssClass="form-control" AutoPostBack="true" Style="width: 200px" OnTextChanged="txtEmpid_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblempname" Width="50px" Text="Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" AutoPostBack="true" Style="width: 200px" OnTextChanged="txtName_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="Label1" Width="50px" Text="Loan No"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlLoanNos" Width="150px" Height="30px" AutoPostBack="True" CssClass="form-control" TabIndex="2"
                                                            OnSelectedIndexChanged="ddlLoanNos_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>

                                                </tr>

                                            </table>
                                            <table>
                                                <tr></tr>
                                                <tr></tr>
                                                <tr></tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btndownload" runat="server" Style="margin-left: 850px; margin-top: 15px" Text="Download"
                                                            class="btn save" OnClick="btndownload_Click" />

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
            <!-- DASHBOARD CONTENT END -->
            <!-- FOOTER BEGIN -->
       
            <!-- FOOTER END -->
            <!-- CONTENT AREA END -->
        </div>

     <script type="text/javascript">
        Sys.Browser.WebKit = {};
        if (navigator.userAgent.indexOf('WebKit/') > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = 'WebKit';
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function () {


                GetEmpid();
                GetEmpName();
                GetFromEmpid();
                GetToEmpid();

            });
        };
    </script>

</asp:Content>
