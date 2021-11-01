<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Employees/EmployeeMaster.Master" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="SRF.P.Module_Employees.AddEmployee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            filter: alpha(opacity=80);
            z-index: 999;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../script/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="../script/Nitgen.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            function ShowProgress() {
                setTimeout(function () {
                    var modal = $('<div />');
                    modal.addClass("modal");
                    $('body').append(modal);
                    var loading = $(".loading");
                    loading.show();
                    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                    loading.css({ top: top, left: left });
                }, 200);
            }

            var clkBtn = "";

            $('input[type="submit"]').click(function (evt) {
                clkBtn = evt.target.id;
            });




            $('form').live("submit", function (evt) {

                var btnID = clkBtn;
                if (btnID != 'btnadd' && (btnID != 'btnEduadd') && (btnID != 'btnPrevExpAdd') && (btnID != 'btnSubmit')) {
                    ShowProgress();

                }


            });

        });
    </script>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link rel="shortcut icon" href="../assets/Mushroom.ico" />
    <link href="../css/chosen.css" rel="stylesheet" />
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/Calendar.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />


    <!-- jQuery -->
    <script type="text/javascript" src="../date/jquery00.js"></script>

    <!-- required plugins -->

    <script type="text/javascript" src="../date/date0000.js"></script>

    <!--[if lt IE 7]><script type="text/javascript" src="scripts/jquery.bgiframe.min.js"></script><![endif]-->
    <!-- jquery.datePicker.js -->

    <script type="text/javascript" src="../date/jquery01.js"></script>

    <!-- datePicker required styles -->
    <link rel="stylesheet" type="text/css" media="screen" href="../date/datePick.css">
    <!-- page specific scripts -->

    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>


    <script type="text/javascript" charset="utf-8">
        $(function () {
            $('.date-pick').datePicker({ startDate: '01/01/1996' });

        });
    </script>


    <style type="text/css">
        .pstyle {
            width: 450px;
            margin: 0px auto;
        }
    </style>

    <script type="text/javascript">

        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
        }

        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                // if the key pressed is the escape key, dismiss the dialog
                $find('modelExRejoin').hide();
                $("select#ddloldempdrp")[0].selectedIndex = 0;
            }
        }

        if (typeof (Sys.Browser.WebKit) == "undefined") {
            Sys.Browser.WebKit = {};
        }
        if (navigator.userAgent.indexOf("WebKit/") > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = "WebKit";
        }

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

    <script type="text/javascript">

        if (typeof (Sys.Browser.WebKit) == "undefined") {
            Sys.Browser.WebKit = {};
        }
        if (navigator.userAgent.indexOf("WebKit/") > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = "WebKit";
        }

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

    <script type="text/javascript">
        $(function () {
            $('#ChkCriminalOff').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtCriminalOffCName').prop("disabled", false),
                        $('#txtCriminalOffcaseNo').prop("disabled", false),
                        $('#txtCriminalOff').prop("disabled", false);
                else
                    $('#txtCriminalOffCName').prop("disabled", true),
                        $('#txtCriminalOffcaseNo').prop("disabled", true),
                        $('#txtCriminalOff').prop("disabled", true);
            })

            $('#ChkCriminalProc').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtCriminalProCName').prop("disabled", false),
                        $('#txtCriminalProCaseNo').prop("disabled", false),
                        $('#txtCriminalProOffence').prop("disabled", false);
                else
                    $('#txtCriminalProCName').prop("disabled", true),
                        $('#txtCriminalProCaseNo').prop("disabled", true),
                        $('#txtCriminalProOffence').prop("disabled", true);
            })

            $('#ChkCrimalArrest').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtCriminalArrestCName').prop("disabled", false),
                        $('#txtCriminalArrestCaseNo').prop("disabled", false),
                        $('#txtCriminalArrestOffence').prop("disabled", false);
                else
                    $('#txtCriminalArrestCName').prop("disabled", true),
                        $('#txtCriminalArrestCaseNo').prop("disabled", true),
                        $('#txtCriminalArrestOffence').prop("disabled", true);
            })

            $('#rdbResigned').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtDofleaving').prop("disabled", false);
                else
                    $('#txtDofleaving').prop("disabled", true);
            })

            $('#rdbactive').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtDofleaving').prop("disabled", true);
                else
                    $('#txtDofleaving').prop("disabled", true);
            })

            $('#rdbVerified').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtPoliceVerificationNo').prop("disabled", false);

                else
                    $('#txtPoliceVerificationNo').prop("disabled", true);

            })

            $('#rdbNotVerified').change(function () {
                var status = this.checked;
                if (status)
                    $('#txtPoliceVerificationNo').prop("disabled", true);


                else
                    $('#txtPoliceVerificationNo').prop("disabled", false);

            })

        })
    </script>

    <link rel="stylesheet" href="../script/jquery-ui.css" />
    <script type="text/javascript" src="../script/jquery.min.js"></script>
    <script type="text/javascript" src="../script/jquery-ui.js"></script>


    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var currentTab = 0;
        $(function () {
            $("#tabs").tabs({
                select: function (e, i) {
                    currentTab = i.index;
                }
            });
        });
        $("#btnNext").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == (c - 1) ? currentTab : (currentTab + 1);
            tabs.tabs('select', currentTab);
            $("#btnPrevious").show();
            if (currentTab == (c - 1)) {
                $("#btnNext").hide();
            } else {
                $("#btnNext").show();
            }
        });
        $("#btnPrevious").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == 0 ? currentTab : (currentTab - 1);
            tabs.tabs('select', currentTab);
            if (currentTab == 0) {
                $("#btnNext").show();
                $("#btnPrevious").hide();
            }
            if (currentTab < (c - 1)) {
                $("#btnNext").show();
            }
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

        .modalBackground {
            background-color: rgba(195,195,195,0.5);
            z-index: 10000;
        }
    </style>

    <script type="text/javascript">

        jQuery(document).ready(function mchoose() {
            jQuery(".chosen").data("placeholder", "Select Frameworks...").chosen();
        });



        var quality = 60; //(1 to 100) (recommanded minimum 55)
        var timeout = 10; // seconds (minimum=10(recommanded), maximum=60, unlimited=0 )

        function GetInfo() {
            document.getElementById('tdSerial').innerHTML = "";
            document.getElementById('tdMake').innerHTML = "";
            document.getElementById('tdModel').innerHTML = "";
            document.getElementById('tdWidth').innerHTML = "";
            document.getElementById('tdHeight').innerHTML = "";
            document.getElementById('tdLocalMac').innerHTML = "";
            document.getElementById('tdLocalIP').innerHTML = "";

            var res = GetNitgenInfo();
            if (res.httpStaus) {

                document.getElementById('txtStatus').value = "ErrorCode: " + res.data.ErrorCode + " ErrorDescription: " + res.data.ErrorDescription;

                if (res.data.ErrorCode == "0") {
                    document.getElementById('tdSerial').innerHTML = res.data.DeviceInfo.SerialNo;
                    document.getElementById('tdMake').innerHTML = res.data.DeviceInfo.Make;
                    document.getElementById('tdModel').innerHTML = res.data.DeviceInfo.Model;
                    document.getElementById('tdWidth').innerHTML = res.data.DeviceInfo.Width;
                    document.getElementById('tdHeight').innerHTML = res.data.DeviceInfo.Height;
                    document.getElementById('tdLocalMac').innerHTML = res.data.DeviceInfo.LocalMac;
                    document.getElementById('tdLocalIP').innerHTML = res.data.DeviceInfo.LocalIP;
                }
            }
            else {
                alert(res.err);
            }
            return false;
        }

        function RightHandThumbCapture() {
            try {

                document.getElementById('imgRHThumb').src = "data:image/bmp;base64,";
                document.getElementById('txtRHThumb').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {

                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgRHThumb').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtRHThumb').value = res.data.BitmapData;

                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function RightHandIndexCapture() {
            try {

                document.getElementById('imgRHIndex').src = "data:image/bmp;base64,";
                document.getElementById('txtRHIndex').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgRHIndex').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtRHIndex').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function RightHandMiddleCapture() {
            try {

                document.getElementById('imgRHMiddle').src = "data:image/bmp;base64,";
                document.getElementById('txtRHMiddle').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgRHMiddle').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtRHMiddle').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function RightHandRingCapture() {
            try {

                document.getElementById('imgRHRing').src = "data:image/bmp;base64,";
                document.getElementById('txtRHRing').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgRHRing').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtRHRing').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function RightHandLittleCapture() {
            try {

                document.getElementById('imgRHLittle').src = "data:image/bmp;base64,";
                document.getElementById('txtRHLittle').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgRHLittle').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtRHLittle').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function LeftHandThumbCapture() {
            try {

                document.getElementById('imgLHThumb').src = "data:image/bmp;base64,";
                document.getElementById('txtLHThumb').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {

                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgLHThumb').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtLHThumb').value = res.data.BitmapData;

                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function LeftHandIndexCapture() {
            try {

                document.getElementById('imgLHIndex').src = "data:image/bmp;base64,";
                document.getElementById('txtLHIndex').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgLHIndex').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtLHIndex').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function LeftHandMiddleCapture() {
            try {

                document.getElementById('imgLHMiddle').src = "data:image/bmp;base64,";
                document.getElementById('txtLHMiddle').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgLHMiddle').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtLHMiddle').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function LeftHandRingCapture() {
            try {

                document.getElementById('imgLHRing').src = "data:image/bmp;base64,";
                document.getElementById('txtLHRing').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgLHRing').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtLHRing').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

        function LeftHandLittleCapture() {
            try {

                document.getElementById('imgLHLittle').src = "data:image/bmp;base64,";
                document.getElementById('txtLHLittle').value = "";

                var res = CaptureFinger(quality, timeout);

                if (res.httpStaus) {


                    if (res.data.ErrorCode == "0") {
                        document.getElementById('imgLHLittle').src = "data:image/bmp;base64," + res.data.BitmapData;
                        document.getElementById('txtLHLittle').value = res.data.BitmapData;
                    }
                }
                else {
                    alert(res.err);
                }
            }
            catch (e) {
                alert(e);
            }
            return false;
        }

    </script>

    <style type="text/css">
        .form-controldrop {
            display: block;
            margin-left: 0px;
            width: 160px;
            height: 28px;
            padding: 0px 12px;
            font-size: 12px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            /*border-radius: 4px;*/
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

        .form-control {
            display: block;
            margin-left: 0px;
            width: 160px;
            height: 28px;
            padding: 0px 12px;
            font-size: 12px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            /*border-radius: 4px;*/
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }

            .form-control:focus {
                border-color: #66afe9;
                outline: 0;
                -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
                box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
            }

            .form-control::-moz-placeholder {
                color: #999;
                opacity: 1;
            }

            .form-control:-ms-input-placeholder {
                color: #999;
            }

            .form-control::-webkit-input-placeholder {
                color: #999;
            }

            .form-control[disabled],
            .form-control[readonly],
            fieldset[disabled] .form-control {
                cursor: not-allowed;
                background-color: #eee;
                opacity: 1;
            }

        textarea.form-control {
            height: auto;
        }

        input[type="search"] {
            -webkit-appearance: none;
        }

        .auto-style1 {
            width: 375px;
        }

        .auto-style2 {
            width: 137px;
        }

        .auto-style3 {
            width: 150px;
        }
    </style>

    <link rel="stylesheet" href="https://harvesthq.github.io/chosen/chosen.css" type="text/css" />
    <script src="https://harvesthq.github.io/chosen/chosen.jquery.js" type="text/javascript"></script>

    <div id="content-holder">
        <div class="content-holder">
            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">
                <asp:ScriptManager runat="server" ID="Scriptmanager2">
                </asp:ScriptManager>

                <div align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div align="center">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>


                <div class="panel panel-inverse">
                    <div class="panel-heading">



                        <table width="100%">
                            <tr>
                                <td>
                                    <h3 class="panel-title">Add Employee</h3>
                                </td>
                                <td align="right"><< <a href="Employees.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>

                    <div id="dialog" style="display: none">

                        <table cellpadding="5" cellspacing="5">
                            <tr>
                                <td>Emp ID
                                </td>
                                <td>

                                    <asp:TextBox ID="txtoldid" runat="server"
                                        class="sinput"></asp:TextBox>

                                    <cc1:AutoCompleteExtender ID="EmpIdtoAutoCompleteExtender" runat="server"
                                        ServiceMethod="GetEmpID"
                                        ServicePath="AutoCompleteAA.asmx"
                                        MinimumPrefixLength="4"
                                        CompletionInterval="100"
                                        EnableCaching="true"
                                        TargetControlID="txtoldid"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="completionList"
                                        CompletionListItemCssClass="listItem"
                                        CompletionListHighlightedItemCssClass="itemHighlighted">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="BtnOldEmpidDetails" runat="server" Text="Search" />

                                </td>
                            </tr>
                        </table>



                    </div>




                    <div class="panel-body">

                        <div style="text-align: right">
                            <asp:Label ID="txtmodifyempid" runat="server"></asp:Label>
                        </div>
                        <div id="tabs">
                            <ul>
                                <li><a href="#tabs-1">Personal Information</a></li>
                                <li><a href="#tabs-2">References</a></li>
                                <li><a href="#tabs-3">Bank/PF/ESI</a></li>
                                <%-- <li><a href="#tabs-4">Images</a></li>--%>
                                <li><a href="#tabs-4">Proofs</a></li>
                                <li><a href="#tabs-5">Qualification/Previous Experience</a></li>
                                <%--<li><a href="#tabs-5">Images</a></li>--%>
                                <li><a href="#tabs-6">Police Record</a></li>
                            </ul>
                            <div id="tabs-1">
                                <asp:UpdatePanel runat="server" ID="uppersonal">
                                    <ContentTemplate>
                                        <div class="dashboard_firsthalf">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdbGeneral" TabIndex="1" runat="server" GroupName="E1" Text=" General Enrollment" Checked="True" AutoPostBack="True" OnCheckedChanged="rdbGeneral_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbStaff" TabIndex="2" runat="server" GroupName="E1" Text=" Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged" />
                                                        <asp:RadioButton ID="rdbmanual" runat="server" GroupName="E1" Text=" Manual" Style="padding-left: 3px" AutoPostBack="True" OnCheckedChanged="rdbmanual_CheckedChanged" />
                                                        <asp:RadioButton ID="rdbRejoin" runat="server" GroupName="E1" Text=" Rejoin" Style="padding-left: 3px" />

                                                    </td>
                                                    <td>
                                                        <cc1:ModalPopupExtender ID="modelRejoin" runat="server" TargetControlID="rdbRejoin" PopupControlID="pnlRadioButton1"
                                                            BackgroundCssClass="modalBackground" BehaviorID="modelExRejoin">
                                                        </cc1:ModalPopupExtender>

                                                        <asp:Panel ID="pnlRadioButton1" runat="server" Height="200px" Width="400px" Style="display: none; position: absolute; background-color: white; border-radius: 10px; box-shadow: 0 0 15px #333333;">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="margin-top: 10px">
                                                                            <td style="font: bold; font-size: medium; padding-left: 12px">&nbsp;&nbsp;&nbsp;
                                                                                Empid
                                                                            </td>
                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                           <asp:DropDownList ID="ddloldempdrp" runat="server" class="chosen"
                                                                                               Width="180px" Style="margin-left: 10px">
                                                                                           </asp:DropDownList>
                                                                            </td>

                                                                        </tr>

                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <br />
                                                            <table style="background-position: center;">
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSubmit" runat="server" Text="Ok" Style="float: right; margin-left: 190px" CssClass="btn Save" OnClick="BtnOldEmpidDetails_Click" />
                                                                    </td>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <%-- <asp:Button ID="btnClose" runat="server" Text="Close" Style="float: right; margin-left: -13px" class="btn Save" Visible="false" />--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Emp ID
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmID" TabIndex="1" runat="server" MaxLength="6" ReadOnly="True"
                                                            class="sinput"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextEmpid" runat="server" Enabled="True" TargetControlID="txtEmID"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>First Name<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpFName" TabIndex="2" runat="server" class="sinput" MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Last Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmplname" TabIndex="4" runat="server" class="sinput" MaxLength="25"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Gender<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbmale" TabIndex="6" runat="server" GroupName="g1" Text="Male" Checked="True" />
                                                        <asp:RadioButton ID="rdbfemale" TabIndex="7" runat="server" GroupName="g1" Text="Female" />
                                                        <asp:RadioButton ID="rdbTransgender" runat="server" GroupName="g1" Text="Transgender" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Status
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbactive" TabIndex="12" runat="server" GroupName="g2" Text="Active" Checked="true" />
                                                        &nbsp;
                                                    <asp:RadioButton ID="rdbResigned" TabIndex="13" runat="server" GroupName="g2" Text="Resigned" />
                                                        &nbsp;
                                                    <asp:RadioButton ID="rdbAbsconded" runat="server" GroupName="g2" Text="Absconding" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Qualification
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" TabIndex="14" ID="txtQualification" MaxLength="15" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Interview
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofInterview" TabIndex="16" runat="server" class="sinput"
                                                            MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEDtofInterview" runat="server" Enabled="true" TargetControlID="txtEmpDtofInterview"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtEmpDtofInterview"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td></td>
                                                </tr>




                                                <tr>
                                                    <td>Phone No.<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPhone" TabIndex="18" MaxLength="12" runat="server" class="sinput">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtPhone" FilterMode="ValidChars" FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Mother Tongue
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtmtongue" TabIndex="20" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Nationality
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtnationality" TabIndex="22" runat="server" class="sinput" MaxLength="50" Text="INDIAN"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Father Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFatherName" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Father Occupation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfatheroccupation" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Spouse Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSpousName" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                        <td>
                                                            Old Employee ID
                                                        </td>
                                                        <td>
                                                           
                                                            <asp:TextBox ID="txtoldemployeeid"  runat="server" MaxLength="50" ReadOnly="true" class="sinput"></asp:TextBox>
                                                       
                                                        </td>
                                                    </tr>--%>
                                                <tr>
                                                    <td>Branch
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBranch" runat="server" TabIndex="30"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Department
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddldepartment" runat="server" TabIndex="32"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Site Posted to
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlPreferedUnit" TabIndex="34" runat="server"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>PSARA Emp Code
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtpsaraempcode" runat="server" CssClass="sinput" TabIndex="36"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>ID card issued date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtIDCardIssuedDt" runat="server" CssClass="sinput" TabIndex="38" OnTextChanged="TxtIDCardIssuedDt_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true" TargetControlID="TxtIDCardIssuedDt"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FtBIDCardIssuedDt" runat="server" Enabled="True" TargetControlID="TxtIDCardIssuedDt"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td>Rejoin Empid
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRejoinEmpid" runat="server" CssClass="sinput" TabIndex="36" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>



                                                <tr style="visibility: hidden">
                                                    <td>Community/Classification
                                                    </td>
                                                    <td style="padding-top: 10px">
                                                        <asp:RadioButton ID="rdsc" runat="server" GroupName="m1" Text="SC" />
                                                        <asp:RadioButton ID="rdst" runat="server" GroupName="m1" Text="ST" />
                                                        <asp:RadioButton ID="rdobc" runat="server" GroupName="m1" Text="OBC" />
                                                        <asp:RadioButton ID="rdur" runat="server" GroupName="m1" Text="Others"
                                                            Checked="true" />
                                                    </td>
                                                </tr>
                                            </table>


                                        </div>

                                        <div class="dashboard_secondhalf">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr style="display: none">
                                                    <td>Old Emp ID
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txoldempid" TabIndex="2" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>Title </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTitle" runat="server" class="sdrop" TabIndex="1" OnSelectedIndexChanged="ddlTitle_SelectedIndexChanged" AutoPostBack="true">

                                                            <asp:ListItem>Mr</asp:ListItem>
                                                            <asp:ListItem>Miss</asp:ListItem>
                                                            <asp:ListItem>Mrs</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Middle Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpmiName" TabIndex="3" MaxLength="40" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Date of Birth
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofBirth" TabIndex="5" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEEmpDtofBirth" runat="server" Enabled="true" TargetControlID="txtEmpDtofBirth"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEDOB" runat="server" Enabled="True" TargetControlID="txtEmpDtofBirth"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Marital Status <span style="color: Red">*</span>
                                                    </td>
                                                    <td>

                                                        <asp:RadioButton ID="rdbsingle" TabIndex="8" runat="server" GroupName="m1" Text="Single" />
                                                        <asp:RadioButton ID="rdbmarried" TabIndex="9" runat="server" GroupName="m1" Text="Married" Style="margin-left: 17px" Checked="true" />

                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbdivorcee" runat="server" GroupName="m1" Text="Divorcee" TabIndex="10" Style="margin-top: 10px" />
                                                        <asp:RadioButton ID="rdbWidower" runat="server" GroupName="m1" Text="Widower" TabIndex="11" Style="margin-top: 10px" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Designation<span style="color: Red">*</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" TabIndex="15" class="sdrop" ID="ddlDesignation">
                                                        </asp:DropDownList>

                                                        <%--<cc1:ComboBox ID="ddlDesignation" runat="server"></cc1:ComboBox>--%>


                                                           
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Joining 
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpDtofJoining" TabIndex="17" runat="server" class="sinput" size="20"
                                                            MaxLength="10"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEEmpDtofJoining" runat="server" Enabled="true" TargetControlID="txtEmpDtofJoining"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                            TargetControlID="txtEmpDtofJoining" ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Date of Leaving
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDofleaving" TabIndex="19" runat="server" class="sinput" MaxLength="10" Enabled="false"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEDofleaving" runat="server" Enabled="true" TargetControlID="txtDofleaving"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtDofleaving" ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Languages Known
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" TabIndex="21" ID="txtLangKnown" class="sinput" MaxLength="80">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Religion
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtreligion" TabIndex="23" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Previous Employer
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPreEmp" TabIndex="29" runat="server" TextMode="MultiLine" Style="height: 50px" class="sinput"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Mother Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMotherName" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Division
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDivision" runat="server" TabIndex="31"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Reporting Manager
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlReportingMgr" runat="server" TabIndex="33"
                                                            class="sdrop">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Gross Salary
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGrossSalary" runat="server" class="sinput" TabIndex="35"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" TargetControlID="txtGrossSalary"
                                                            ValidChars="0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Email
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtemail" runat="server" class="sinput" TabIndex="37"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>ID card valid till
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtIdCardValid" runat="server" CssClass="sinput" TabIndex="39"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true" TargetControlID="TxtIdCardValid"
                                                            Format="dd/MM/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBIdCardValid" runat="server" Enabled="True" TargetControlID="TxtIdCardValid"
                                                            ValidChars="/0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>



                                            </table>
                                        </div>
                                        <br />
                                        <%-- OnClientClick='return confirm("Are you sure you want to create an employee?");'--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>


                            </div>
                            <div id="tabs-2">
                                <asp:UpdatePanel runat="server" ID="Updatepanel3">
                                    <ContentTemplate>
                                        <asp:Panel ID="PnlEmployeeInfo" runat="server" GroupingText="<strong>&nbsp;Employee Info&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">

                                                <table cellpadding="5" cellspacing="5">

                                                    <%-- <tr style="visibility:hidden">
                                                            <td>Birth Village
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBirthVillage" runat="server" class="sinput" TabIndex="1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility:hidden">
                                                            <td>Birth State
                                                            </td>
                                                            <td>
                                                                <%--<asp:TextBox ID="txtBirthState" runat="server" class="sinput" TabIndex="3"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlbirthstate" runat="server" class="sdrop" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlbirthstate_SelectedIndexChanged"></asp:DropDownList>

                                                            </td>
                                                        </tr>--%>
                                                    <tr>
                                                        <td>Ref Name &amp; Address1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr1" runat="server" TabIndex="5" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Blood Group
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" TabIndex="7" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Physical Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhyRem" runat="server" TabIndex="9" class="sinput" MaxLength="55"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark1" runat="server" TabIndex="10" class="sinput" MaxLength="80"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>

                                                        <td>Specially Abled</td>
                                                        <td>
                                                            <asp:CheckBox ID="ChkSpeciallyAbled" runat="server" Text=" Specially Abled" TabIndex="11" AutoPostBack="True" OnCheckedChanged="ChkSpeciallyAbled_CheckedChanged" />
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Family Details
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFamDetails" runat="server" TextMode="MultiLine"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>

                                            </div>



                                            <div class="dashboard_secondhalf" style="padding-top: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <%-- <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                    <%-- <tr style="visibility:hidden">
                                                            <td>Birth Country
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBirthCountry" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="2" Text="INDIA"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility:hidden">
                                                            <td>Birth District
                                                            </td>
                                                            <td>
                                                                <%--<asp:TextBox ID="txtBirthDistrict" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="4"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlBirthDistrict" runat="server" CssClass="sdrop" Style="margin-left: 5px" TabIndex="4" Enabled="false"></asp:DropDownList>
                                                            </td>
                                                        </tr>--%>

                                                    <tr>
                                                        <td>Ref Name &amp; Address2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr2" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpRemarks" runat="server" TabIndex="8" TextMode="MultiLine"
                                                                class="sinput" MaxLength="50" Height="50px" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark2" runat="server" TabIndex="10" class="sinput" MaxLength="80" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>

                                                        <td>Applicant Category</td>
                                                        <td>
                                                            <%--<asp:TextBox ID="TxtAppCategory" runat="server" class="sinput" ></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlAppCategory" runat="server" Style="margin-left: 5px" TabIndex="12" CssClass="sdrop" Enabled="false">
                                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="LOCOMOTIVE" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="VISUAL" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="HEARING" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="OTHERS" Value="4"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblrefresult" Style="color: Red"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlphysicalstandard" runat="server" GroupingText="<strong>&nbsp;Physical Standard &nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Height
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtheight" runat="server" TabIndex="13" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Weight
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtweight" runat="server" TabIndex="15" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Hair Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txthaircolour" runat="server" class="sinput" MaxLength="80" TabIndex="17" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">


                                                    <tr>
                                                        <td>Chest UnExpand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheunexpan" runat="server" TabIndex="14" class="sinput" MaxLength="50" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Chest Expand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheexpan" runat="server" TabIndex="16" class="sinput" MaxLength="25" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Eye Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEyeColour" runat="server" class="sinput" MaxLength="25" Style="margin-left: 48px" TabIndex="18"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:Panel ID="PnlAddressDetails" runat="server" GroupingText="<strong>&nbsp;Address Details&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Present Address :</strong>
                                                        </td>
                                                        <td>

                                                            <asp:CheckBox ID="chkSame" runat="server" Text=" Copy" AutoPostBack="true" OnCheckedChanged="chkSame_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <%-- <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPresentAddress" runat="server" TabIndex="19" class="sinput" Height="55px"  TextMode="MultiLine" Style="margin-left: 12px"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprLandmark" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprvillage" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPostOffice" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprtaluka" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPoliceStation" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>

                                                            <%--<asp:TextBox ID="txtstate" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreStates" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlpreStates_SelectedIndexChanged1"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtcity" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreCity" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="23" Enabled="false"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprpin" runat="server" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprResidingDate" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="txtprResidingDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                TargetControlID="txtprResidingDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPeriodofStay" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>

                                                        </td>
                                                    </tr>

                                                    <%--<td>
                                                                <asp:TextBox ID="txtprntaddress" runat="server" TabIndex="4" Width="160px"></asp:TextBox>
                                                            </td>

                                              <tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPrdoor" runat="server" TabIndex="12" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet" runat="server" TabIndex="13" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                               <tr>
                                                    <td>
                                                        District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdistrictt" runat="server" TabIndex="16" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Permanent Address :</strong>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPermanentAddress" runat="server" TabIndex="20" class="sinput" Height="55px" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeLandmark" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpevillage" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePostOffice" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeTaluka" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePoliceStattion" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtstate8" runat="server" TabIndex="28" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="DdlStates" runat="server" class="sdrop" TabIndex="22" AutoPostBack="true" OnSelectedIndexChanged="DdlStates_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlcity" runat="server" class="sdrop" TabIndex="24" Enabled="false"></asp:DropDownList>
                                                            <%-- <asp:TextBox ID="txtcity5" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePin" runat="server" TabIndex="27" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtResidingDate" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="true" TargetControlID="txtResidingDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                TargetControlID="txtResidingDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPeriodofStay" runat="server" class="sinput"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdoor1" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet2" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark3" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea4" runat="server" TabIndex="24" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>



                                                    <%--<tr>
                                                    <td>
                                                        Perm. District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPDist" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile9" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                            <tr>
                                                <td style="font-weight: 900">Police Verification</td>
                                                <td>
                                                    <asp:RadioButton ID="rdbnotrequired" runat="server" GroupName="A1" Text="Not Required" Checked="true" />

                                                    <asp:RadioButton ID="rdbpreaddress" runat="server" GroupName="A1" Text="Present Address" AutoPostBack="true" OnCheckedChanged="rdbpreaddress_CheckedChanged" />

                                                    <asp:RadioButton ID="rdbperaddress" runat="server" GroupName="A1" Text="Permanent Address" AutoPostBack="true" OnCheckedChanged="rdbperaddress_CheckedChanged" />
                                                </td>

                                            </tr>



                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tabs-3">
                                <asp:UpdatePanel runat="server" ID="Updatepanel6">
                                    <ContentTemplate>
                                        <asp:Panel ID="PnlBankDetails" runat="server" GroupingText="<strong>&nbsp;Bank Details&nbsp;</strong>">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlbankname" runat="server" TabIndex="1" class="sdrop" MaxLength="100">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Branch Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbranchname" runat="server" MaxLength="80" TabIndex="3" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Branch Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchCode" runat="server" TabIndex="5" class="sinput" MaxLength="50"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                TargetControlID="txtBranchCode" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank App No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAppNum" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Insurance Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpInsNominee" runat="server" TabIndex="9" class="sinput" MaxLength="100"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>Nominee Date of Birth
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNomDoB" runat="server" TabIndex="11" class="sinput"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CENomDoB" runat="server" Enabled="true" TargetControlID="txtNomDoB"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                TargetControlID="txtNomDoB" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Insurance Cover
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInsCover" TabIndex="13" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FTBEInsCover" runat="server" Enabled="True" TargetControlID="txtInsCover"
                                                                FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td>Aadhaar No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="15" ID="txtaadhaar" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Cmp Short Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCmpShortName" class="sinput" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank A/C No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAccNum" TabIndex="2" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>IFSC Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIFSCcode" runat="server" MaxLength="20" TabIndex="4" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank Code No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankCodenum" TabIndex="6" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Region Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRegCode" TabIndex="8" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtRegCode" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank Card Reference
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankCardRef" TabIndex="10" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpNomRel" TabIndex="12" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Ins Debt Amount
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInsDeb" TabIndex="14" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                TargetControlID="txtInsDeb" FilterMode="ValidChars" FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>UAN No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSSNumber" TabIndex="16" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>
                                            <%--  <div style="float: right; margin-top: 300px; margin-left: 250px">
                                                    <asp:Button ID="btn_BankSave" runat="server" Text="Save" class="btn save" OnClick="btn_BankSave_Click"
                                                        OnClientClick='return confirm("Are you sure you want to Add Details?");' />
                                                    <asp:Button ID="btn_BankCancel" runat="server" Text="Cancel" class="btn save" OnClick="btn_BankCancel_Click"
                                                        OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                                    <div style="text-align: center float:right">
                                                        <asp:Label runat="server" ID="lblBankRes" Visible="false" Style="color: Red"></asp:Label>
                                                    </div>
                                                </div>
                                            --%>
                                        </asp:Panel>

                                        <asp:Panel ID="PnlPFDetails" runat="server" GroupingText="<strong>&nbsp;PF Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>PF Deduct 
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" Checked="true" ID="ChkPFDed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>EPF No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="17" ID="txtEmpPFNumber" class="sinput" MaxLength="15" Style="margin-left: 68px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="19" ID="txtPFNominee" class="sinput" MaxLength="80" Style="margin-left: 68px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>PT Deduct
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" TabIndex="28" ID="ChkPTDed" Checked="true" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>PF Enroll Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="18" class="sinput" ID="txtPFEnrollDate" size="20" Style="margin-left: 2px"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CEPFEnrollDate" runat="server" Enabled="true" TargetControlID="txtPFEnrollDate"
                                                                Format="dd/MM/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                TargetControlID="txtPFEnrollDate" ValidChars="/0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPFNomineeRel" TabIndex="20" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </asp:Panel>


                                        <asp:Panel ID="PnlESIDetails" runat="server" GroupingText="<strong>&nbsp;ESI Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>

                                                        <td>ESI Deduct </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkESIDed" Text="" Checked="true" /><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="21" ID="txtESINum" class="sinput" MaxLength="15" Style="margin-left: 63px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="23" ID="txtESINominee" class="sinput" MaxLength="80" Style="margin-left: 63px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI Disp Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="22" ID="txtESIDiSName" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="24" ID="txtESINomRel" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="PnlSalaryDetails" runat="server" GroupingText="<strong>&nbsp;Salary Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td style="height: 20px">Additional Amount
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtaddlamt" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 35px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td style="height: 20px">Food Allowance
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtfoodallowance" runat="server" TabIndex="26" class="sinput" MaxLength="50" Style="margin-left: 50px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </asp:Panel>



                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div id="tabs-4">

                                <asp:UpdatePanel runat="server" ID="Updatepanel7">
                                    <ContentTemplate>

                                        <asp:Panel ID="pnlimages" runat="server" GroupingText="<strong>&nbsp;Images&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5" style="margin-top: 10px">
                                                    <tr>
                                                        <td>Employee Photo</td>

                                                        <td>
                                                            <asp:FileUpload ID="FileUploadImage" runat="server" ViewStateMode="Enabled" /></td>
                                                        <%--<cc1:AsyncFileUpload OnClientUploadError="uploadError"
                                                OnClientUploadComplete="uploadComplete" runat="server"
                                                ID="FileUploadImage" Width="400px" UploaderStyle="Modern"
                                                CompleteBackColor = "White"
                                                UploadingBackColor="#CCFFFF"  
                                                 />
                                                </td>--%>
                                                    </tr>

                                                </table>
                                            </div>

                                            <div class="dashboard_Secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5" style="margin-top: 10px;">
                                                    <tr>

                                                        <td>Emp Sign</td>
                                                        <td>
                                                            <asp:FileUpload ID="FileUploadSign" runat="server" ViewStateMode="Enabled" /></td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:UpdatePanel runat="server" ID="Upproofs" UpdateMode="Always">
                                            <ContentTemplate>

                                                <asp:Panel ID="PnlProofsSubmitted" runat="server" GroupingText="<strong>&nbsp;Proofs Submitted&nbsp;</strong>" Style="margin-top: 10px">

                                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                                        <table cellpadding="5" cellspacing="5">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkAadharCard" runat="server" Text="  Aadhar Card" TabIndex="1" OnCheckedChanged="ChkAadharCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>

                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAadharCard" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="2"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>

                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAadharName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkdrivingLicense" runat="server" Text=" Driving License" TabIndex="5" OnCheckedChanged="ChkdrivingLicense_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDrivingLicense" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="6"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDrivingLicenseName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>

                                                                <td>
                                                                    <asp:CheckBox ID="ChkVoterID" runat="server" Text=" Voter ID" TabIndex="9" OnCheckedChanged="ChkVoterID_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVoterID" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="10"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtVoterName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkRationCard" runat="server" Text=" Ration Card" TabIndex="13" OnCheckedChanged="ChkRationCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRationCard" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="14"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRationCardName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>

                                                                <td>
                                                                    <asp:CheckBox ID="Chkother" runat="server" Text=" if Others, Specify" TabIndex="15" OnCheckedChanged="Chkother_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>

                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOther" runat="server" class="sinput" Enabled="false" TabIndex="16" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOtherName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                                        <table cellpadding="5" cellspacing="5">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkPanCard" runat="server" Text=" Pan Card" TabIndex="3" OnCheckedChanged="ChkPanCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>


                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPanCard" runat="server" class="sinput" Enabled="false" TabIndex="4"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPanCardName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkBankPassbook" runat="server" Text=" Bank PassBook" TabIndex="7" OnCheckedChanged="ChkBankPassbook_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBankPassbook" runat="server" class="sinput" Enabled="false" TabIndex="8"></asp:TextBox>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBankPassBookName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkElectricityBill" runat="server" Text=" Electricity Bill" TabIndex="11" OnCheckedChanged="ChkElectricityBill_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtElectricityBill" runat="server" class="sinput" Enabled="false" TabIndex="12"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtElecBillname" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkESICCard" runat="server" Text=" ESIC Card" TabIndex="15" AutoPostBack="true" OnCheckedChanged="ChkESICCard_CheckedChanged" Style="font-weight: bold" />
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">Number</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtESICCardNo" runat="server" class="sinput" Enabled="false" TabIndex="16"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 18px">KYC Name</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtESICName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="PnlExService" runat="server" GroupingText="<strong>&nbsp;Ex-Service&nbsp;</strong>" Style="margin-top: 15px">

                                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                                        <table cellpadding="5" cellspacing="5">

                                                            <tr>
                                                                <td>EMP Ex-service
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="ChkExService" Text="" />
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td style="height: 20px">Service No.
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtServiceNum" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Date of Enrollment
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtDOfEnroll" runat="server" TabIndex="19" class="sinput" size="20"
                                                                        MaxLength="10"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CEDOfEnroll" runat="server" Enabled="true" TargetControlID="txtDOfEnroll"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEDOfEnroll" runat="server" Enabled="True" TargetControlID="txtDOfEnroll"
                                                                        ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Crops
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtCrops" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Medical Category
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtMCategory" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Conduct
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtConduct" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </div>
                                                    <div class="dashboard_secondhalf" style="padding: 10px">
                                                        <table cellpadding="5" cellspacing="5">
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Rank
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtRank" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Date of Discharge
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtDofDischarge" runat="server" TabIndex="20" class="sinput" size="20"
                                                                        MaxLength="10"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CEDofDischarge" runat="server" Enabled="true" TargetControlID="txtDofDischarge"
                                                                        Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEDofDischarge" runat="server" Enabled="True"
                                                                        TargetControlID="txtDofDischarge" ValidChars="/0123456789">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Trade
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="txtTrade" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 20px">Reason of Discharge
                                                                </td>
                                                                <td style="height: 20px">
                                                                    <asp:TextBox ID="TxtROfDischarge" runat="server" TabIndex="24" TextMode="MultiLine" MaxLength="50"
                                                                        class="sinput"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label runat="server" ID="lblExRes" Visible="false" Style="color: Red"></asp:Label>
                                                                </td>
                                                            </tr>

                                                        </table>

                                                    </div>
                                                </asp:Panel>
                                                <style type="text/css">
                                                    .HeaderStyle {
                                                        font-weight: bold;
                                                    }
                                                </style>

                                                <asp:Panel ID="pnlfamilydetails" runat="server" GroupingText="<strong>&nbsp;Family Details&nbsp;</strong>" Style="margin-top: 10px">
                                                    <div style="padding: 10px">
                                                        <asp:GridView ID="gvFamilyDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                            BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                            ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5" OnRowCreated="grvMergeHeader_RowCreated">
                                                            <HeaderStyle Wrap="True" />
                                                            <PagerSettings Mode="NextPreviousFirstLast" />
                                                            <RowStyle />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEmpName" Width="" runat="server" Text=""></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="150" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="Date Of Birth" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRelDtofBirth" runat="server"
                                                                            MaxLength="10" placeholder="DD/MM/YYYY"></asp:TextBox>

                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="90" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="40" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="Relationship" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlRelation" runat="server" Width="99%">
                                                                            <asp:ListItem runat="server" Value="--Select--" />
                                                                            <asp:ListItem runat="server" Value="Father" />
                                                                            <asp:ListItem runat="server" Value="Wife" />
                                                                            <asp:ListItem runat="server" Value="Husband" />
                                                                            <asp:ListItem runat="server" Value="Son" />
                                                                            <asp:ListItem runat="server" Value="Daughter" />
                                                                            <asp:ListItem runat="server" Value="Brother" />
                                                                            <asp:ListItem runat="server" Value="Sister" />
                                                                            <asp:ListItem runat="server" Value="Mother" />
                                                                            <asp:ListItem runat="server" Value="Uncle" />
                                                                            <asp:ListItem runat="server" Value="Aunty" />
                                                                            <asp:ListItem runat="server" Value="Other" />
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="Occupation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtReloccupation" runat="server" Text=""></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="90" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="AAdhar No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAAdharNo" runat="server" Text=""></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="110" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="PF" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkPFNominee" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="40" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="ESI" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkESINominee" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="40" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="Whether residing with him/her ?" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlresidence" runat="server">
                                                                            <asp:ListItem runat="server" Value="--Select--" />
                                                                            <asp:ListItem runat="server" Value="Yes" />
                                                                            <asp:ListItem runat="server" Value="No" />
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="90" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                                    HeaderText="If 'No' Place of Residence" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtplace" runat="server" Text=""></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="80" />
                                                                    <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <asp:Button ID="btnFamilyDetailsAdd" runat="server" Text="Add" Style="margin-left: 10px; margin-right: 10px; margin-bottom: 10px;" OnClick="btnFamilyDetailsAdd_Click" />
                                                </asp:Panel>

                                            </ContentTemplate>


                                        </asp:UpdatePanel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div id="tabs-5">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <%--<div class="dashboard_firsthalf">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>SSC :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtschool" runat="server" TabIndex="1" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtbrd" runat="server" TabIndex="2" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtyear" runat="server" TabIndex="3" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpsfi" runat="server" TabIndex="4" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpmarks" runat="server" TabIndex="5" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>INTERMEDIATE :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimschool" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimbrd" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimyear" runat="server" TabIndex="8" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimpsfi" runat="server" TabIndex="9" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtimpmarks" runat="server" TabIndex="10" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                                    <div class="dashboard_secondhalf">
                                        <table cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>DEGREE :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgschool" runat="server" TabIndex="11" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgbrd" runat="server" TabIndex="12" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgyear" runat="server" TabIndex="13" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgpsfi" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtdgpmarks" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" class="style4">
                                                    <strong>PG :</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Name &amp; Address of School/Clg
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgschool" runat="server" TabIndex="16" TextMode="MultiLine" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Board/University
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgbrd" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Year of Study
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgyear" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Whether Pass/Failed
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgpsfi" runat="server" TabIndex="19" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px">Percentage of Marks
                                                </td>
                                                <td style="height: 20px">
                                                    <asp:TextBox ID="txtpgpmarks" runat="server" TabIndex="20" class="sinput" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblquresult" Visible="false" Style="color: Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>--%>

                                        <asp:Panel ID="pnlEducationDetails" runat="server" GroupingText="<strong>&nbsp;Education Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px">
                                                <asp:GridView ID="GvEducationDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Qualification" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlQualification" runat="server" Width="92%">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="ILLITERATE" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="NON-MATRIC" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="MATRIC" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SENIOR SECONDARY" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="GRADUATE" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="POST GRADUATE" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="DOCTOR" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="TECHNICAL/PROFESSIONAL" Value="8"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Description" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEdLevel" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Name & Address of School/College" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNameofSchoolColg" runat="server" TextMode="MultiLine" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Board / University" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBoard" runat="server" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Year of Study" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyear" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Pass / Fail" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassFail" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Percentage of Marks" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPercentage" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnEduAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnEduAdd_Click" />

                                            </div>
                                        </asp:Panel>


                                        <asp:Panel ID="pnlPreviousExpereince" runat="server" GroupingText="<strong>&nbsp;Previous Experience&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px;">
                                                <asp:GridView ID="GvPreviousExperience" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto;" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Region Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtregioncode" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Employer Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtempcode" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Extension" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtExtension" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Designation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPrevDesignation" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="19%"
                                                            HeaderText="Company Name/Address" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCompAddress" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Years of Experience" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyearofexp" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="PF No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPFNo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="ESI No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtESINo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="15%"
                                                            HeaderText="Date Of Resigned" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDtofResigned" runat="server"
                                                                    MaxLength="10" placeholder="DD/MM/YYYY" Width="95%"></asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnPrevExpAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnPrevExpAdd_Click" />
                                            </div>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>

                            <div id="tabs-6">
                                <asp:UpdatePanel runat="server" ID="Updatepanel8">
                                    <ContentTemplate>
                                        <div>
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Police Verification No</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbVerified" runat="server" GroupName="P1" Text=" Verified" OnCheckedChanged="rdbVerified_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rdbNotVerified" runat="server" GroupName="P1" Text=" Not Verified" Checked="True" AutoPostBack="true" OnCheckedChanged="rdbNotVerified_CheckedChanged" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPoliceVerificationNo" runat="server" CssClass="sinput" Enabled="false"></asp:TextBox></td>
                                                    <td>Nearest Police Station</td>
                                                    <td>
                                                        <asp:TextBox ID="txtPoliceStation" runat="server" CssClass="sinput"></asp:TextBox></td>
                                                </tr>
                                            </table>

                                            <asp:Panel ID="Panel1" runat="server" GroupingText="<strong>&nbsp;PVC Address Details&nbsp;</strong>" Style="margin-top: 10px">

                                                <div class="dashboard_firsthalf" style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <%--<tr>
                                                            <td class="style4">
                                                                <strong>PVC Address :</strong>
                                                            </td>--%>
                                                        <%--<td>

                                                                <asp:CheckBox ID="CheckBox1" runat="server" Text=" Copy" AutoPostBack="true" OnCheckedChanged="chkSame_CheckedChanged" />
                                                            </td>--%>
                                                        <%--</tr>--%>

                                                        <tr>
                                                            <td>Land Mark</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvclandmark" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Village/Town</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcvillage" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Post Office</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpostofc" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Taluka/Hobli</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvctaluka" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>Police Station</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpolicestation" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>State
                                                            </td>
                                                            <td>


                                                                <asp:DropDownList ID="ddlpvcstate" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlpvcstate_SelectedIndexChanged"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="dashboard_secondhalf" style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td class="style4"></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>

                                                            <td>District
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlpvccity" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="23" Enabled="false"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Pin code
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpin" runat="server" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Date Since Residing
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcresidedate" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="CEPvcreside" runat="server" Enabled="true" TargetControlID="txtpvcresidedate"
                                                                    Format="dd/MM/yyyy">
                                                                </cc1:CalendarExtender>
                                                                <cc1:FilteredTextBoxExtender ID="FTBpvcreside" runat="server" Enabled="True"
                                                                    TargetControlID="txtpvcresidedate" ValidChars="/0123456789">
                                                                </cc1:FilteredTextBoxExtender>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Period of stay
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcstay" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Phone(if any)
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcphone" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                            <div style="margin-top: 10px">
                                                Criminal Offence 
                                             <asp:CheckBox ID="ChkCriminalOff" runat="server" Text=" (if criminal off is there)" />


                                                <asp:Panel ID="pnlGroupBox" runat="server" GroupingText="<strong>&nbsp;Criminal Offence&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Off Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOffCName" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Off Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOffcaseNo" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOff" runat="server" class="sinput" Enabled="false" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                Criminal Proceeding
                                            <asp:CheckBox ID="ChkCriminalProc" runat="server" Text=" (if any criminal proceeding are there,then tick)" />
                                                <asp:Panel ID="PnlCriminalProceeding" runat="server" GroupingText="<strong>&nbsp;Criminal Proceeding&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Pro Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProCName" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Pro Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProCaseNo" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Pro Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProOffence" runat="server" class="sinput" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                Criminal Arrest Warrant
                                            <asp:CheckBox ID="ChkCrimalArrest" runat="server" Text=" (if any criminal arrest warrant is issued,then tick)" />
                                                <asp:Panel ID="PnlCriminalArrest" runat="server" GroupingText="<strong>&nbsp;Criminal Arrest Warrant&nbsp;</strong>" CssClass="pstyle" Enabled="false" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Arrest Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestCName" runat="server" class="sinput"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Arrest Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestCaseNo" runat="server" class="sinput"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Arrest Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestOffence" runat="server" class="sinput"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>


                        </div>
                    </div>
                </div>
                <br />

                <table width="20%" align="right">
                    <tr>
                        <td>
                            <input type="button" id="btnPrevious" value="Previous" style="display: none" /></td>
                        <td>
                            <input type="button" id="btnNext" value="Next" /></td>
                        <td>
                            <asp:Button ID="Btn_Save_Personal_Tab" runat="server" Text="Save" CssClass="btnsubmit"
                                OnClick="Btn_Save_Personal_Tab_Click" ValidationGroup="a" TabIndex="32" /></td>
                        <td>
                            <asp:Button ID="Btn_Cancel_Personal_Tab" runat="server" Text="Cancel" OnClientClick='return confirm("Are you sure you want to Cancel this entry?");'
                                OnClick="Btn_Cancel_Personal_Tab_Click" TabIndex="33" /></td>


                    </tr>
                </table>





                <br />



                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" AutoPostBack="true">
                </cc1:TabContainer>
            </div>
        </div>
    </div>

    <asp:GridView ID="GvSampleExcel" runat="server" Width="100%"
        AutoGenerateColumns="False" CellPadding="2" CellSpacing="2"
        ForeColor="#333333" BorderStyle="Solid"
        BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
        HeaderStyle-CssClass="HeaderStyle">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>



            <asp:TemplateField HeaderText="EmpId" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblsEmpid" runat="server" Text=" " Style="text-align: center" Width="50px"></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="EmpName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblempname" Width="70px" Text=" "></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Gender" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblGender" Style="text-align: center" Width="20px" Text=" "></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="MaritalStatus" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblMaritalStatus" Style="text-align: center" Width="20px" Text=" "></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="DateofBirth" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDOB" Style="text-align: center" Width="50px" Text=" "></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Designation" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblsDesg" Text=" " Width="50px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>

    <div class="loading" align="center">
        Loading. Please wait.<br />
        <br />
        <img src="../assets/loader.gif" alt="Loading" />
    </div>

    <div class="clear">
    </div>

</asp:Content>
