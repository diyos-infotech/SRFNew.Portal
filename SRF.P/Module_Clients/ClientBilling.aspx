<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Clients/Clients.master" AutoEventWireup="true" CodeBehind="ClientBilling.aspx.cs" Inherits="SRF.P.Module_Clients.ClientBilling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


    <script type="text/javascript">

        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                //  add our handler to the document's
                //  keydown event
                $addHandler(document, "keydown", onKeyDown);
            }
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


        function setProperty() {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
                        .addClass("custom-combobox")
                        .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                },

                _createAutocomplete: function () {
                    var selected = this.element.children(":selected"),
                        value = selected.val() ? selected.text() : "";

                    this.input = $("<input>")
                        .appendTo(this.wrapper)
                        .val(value)
                        .attr("title", "")
                        .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                        .autocomplete({
                            delay: 0,
                            minLength: 0,
                            source: $.proxy(this, "_source")
                        })
                        .tooltip({
                            classes: {
                                "ui-tooltip": "ui-state-highlight"
                            }
                        });

                    this._on(this.input, {
                        autocompleteselect: function (event, ui) {
                            ui.item.option.selected = true;
                            this._trigger("select", event, {
                                item: ui.item.option
                            });
                        },

                        autocompletechange: "_removeIfInvalid"
                    });
                },

                _createShowAllButton: function () {
                    var input = this.input,
                        wasOpen = false;

                    $("<a>")
                        .attr("tabIndex", -1)
                        .attr("title", "Show All Items")
                        .tooltip()
                        .appendTo(this.wrapper)
                        .button({
                            icons: {
                                primary: "ui-icon-triangle-1-s"
                            },
                            text: false
                        })
                        .removeClass("ui-corner-all")
                        .addClass("custom-combobox-toggle ui-corner-right")
                        .on("mousedown", function () {
                            wasOpen = input.autocomplete("widget").is(":visible");
                        })
                        .on("click", function () {
                            input.trigger("focus");

                            // Close if already visible
                            if (wasOpen) {
                                return;
                            }

                            // Pass empty string as value to search for, displaying all results
                            input.autocomplete("search", "");
                        });
                },

                _source: function (request, response) {
                    var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                    response(this.element.children("option").map(function () {
                        var text = $(this).text();
                        if (this.value && (!request.term || matcher.test(text)))
                            return {
                                label: text,
                                value: text,
                                option: this
                            };
                    }));
                },

                _removeIfInvalid: function (event, ui) {

                    // Selected an item, nothing to do
                    if (ui.item) {
                        return;
                    }

                    // Search for a match (case-insensitive)
                    var value = this.input.val(),
                        valueLowerCase = value.toLowerCase(),
                        valid = false;
                    this.element.children("option").each(function () {
                        if ($(this).text().toLowerCase() === valueLowerCase) {
                            this.selected = valid = true;
                            return false;
                        }
                    });

                    // Found a match, nothing to do
                    if (valid) {
                        return;
                    }

                    // Remove invalid value
                    this.input
                        .val("")
                        .attr("title", value + " didn't match any item")
                        .tooltip("open");
                    this.element.val("");
                    this._delay(function () {
                        this.input.tooltip("close").attr("title", "");
                    }, 2500);
                    this.input.autocomplete("instance").term = "";
                },

                _destroy: function () {
                    this.wrapper.remove();
                    this.element.show();
                }
            });

            $(".ddlautocomplete").combobox({
                select: function (event, ui) { $("#<%=ddlclientid.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCname.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },

                minLength: 4
            });

        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlclientid.ClientID %>").trigger('change');
        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {
            $("#<%=ddlCname.ClientID %>").trigger('change');
        }


        $(function () {
            bindautofilldesgs();
        });
        var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        prmInstance.add_endRequest(function () {
            //you need to re-bind your jquery events here
            bindautofilldesgs();
        });

        function bindautofilldesgs() {
            $(".txtautofilldesg").autocomplete({
                source: eval($("#hdDesignations").val()),
                minLength: 4
            });
        }
    </script>

    <style type="text/css">
        #social div {
            display: block;
        }

        .HeaderStyle {
            text-align: Left;
        }


        .modalBackground {
            background-color: Gray;
            z-index: 10000;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 5px 10px;
        }

        .PnlBackground {
            background-color: rgba(128, 128, 128,0.5);
            z-index: 10000;
        }

        .auto-style1 {
            width: 179px;
        }

        .auto-style2 {
            width: 90px;
        }

        .auto-style3 {
            width: 105px;
        }
    </style>

    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">CLIENT BILLING</h2>
                        </div>

                        <div style="text-align: center">
                            <asp:Label ID="lblResult" runat="server" Text="" Style="color: Red"></asp:Label>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <!--  Content to be add here> -->
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="Scriptmanager1">
                                </asp:ScriptManager>

                                <script type="text/javascript">


                                    $(document).on("click", "[id*=btnCancelIRN]", function () {
                                        document.getElementById("<%=ddlCnclRsn.ClientID %>").selectedIndex = 0;
                                        var e = document.getElementById("<%=ddlCname.ClientID %>");
                                        var clientname = e.options[e.selectedIndex].text;


                                        document.getElementById("<%=lblClientdetails.ClientID %>").innerHTML = clientname;
                                        $("#dialog").dialog({
                                            title: "Cancel IRN",
                                            width: 480,
                                            height: 250,
                                            buttons: {
                                                "Submit": function () {
                                                    debugger;
                                                    var CancelRsn = document.getElementById("<%=ddlCnclRsn.ClientID %>").selectedIndex;
                                                    var CancelRemarks = document.getElementById("<%=txtCnclRemarks.ClientID %>").value;

                                                    if (CancelRsn == 0) {
                                                        alert("Please select cancel reason")
                                                        document.getElementById("<%=ddlCnclRsn.ClientID %>").focus();
                                                    }
                                                    else if (CancelRemarks == '') {
                                                        alert("Please fill cancel remarks")
                                                        document.getElementById("<%=txtCnclRemarks.ClientID %>").focus();
                                                    }
                                                    else {
                                                        debugger;

                                                        document.getElementById("<%=hfnCnclRsn.ClientID %>").value = CancelRsn;
                                                            document.getElementById("<%=hfCnclRemarks.ClientID %>").value = CancelRemarks;
                                                        $(this).dialog("close");
                                                        document.getElementById("<%=tempBtn.ClientID %>").click();
                                                            return true;
                                                        }

                                                }
                                            },
                                            modal: true
                                        });
                                        return false;
                                    });

                                    function BindData() {
                                        $.ajax({
                                            type: "POST",
                                            contentType: "application/json; charset=utf-8",
                                            url: "Autocompletion.asmx/Bindgrid",
                                            data: "{}",
                                            dataType: "json",
                                            success: function (data) {
                                                if (data.d.length > 0) {
                                                    var row = $("[id*=gvCreditNoteSummary] tr:last-child").unbind().clone(true);


                                                    $("[id*=gvCreditNoteSummary] tr").not($("[id*=gvCreditNoteSummary] tr:first-child")).remove();


                                                    $("td", row).eq(0).html("");
                                                    $("td", row).eq(1).html("");
                                                    $("td", row).eq(2).html("0");
                                                    $("td", row).eq(3).html("0");
                                                    $("td", row).eq(4).html("0");
                                                    $("td", row).eq(5).html("0");
                                                    $("td", row).eq(6).html("0");
                                                    $("td", row).eq(7).html("0");
                                                    $("td", row).eq(8).html("0");
                                                    $("td", row).eq(9).html("0");

                                                    $('#<%= gvCreditNoteSummary.ClientID %>').unbind().append(row);
                                                    row = $("[id*=gvCreditNoteSummary] tr:last-child").unbind().clone(true);
                                                }
                                            },

                                            error: function (xhr, status, error) {
                                                console.log(xhr.responseText);
                                            }
                                        })
                                    }

                                    $(function () {


                                        $('#<%=TxtservicechrgPrc.ClientID %>').change(function () {

                                            var grid = document.getElementById("<%= gvClientBilling.ClientID%>");
                                            for (var i = 0; i < grid.rows.length - 1; i++) {
                                                var lblSchrgPrc = $("input[id*=lblSchrgPrc]")
                                                if (lblSchrgPrc[i].value != '') {
                                                    lblSchrgPrc[i].value = $('#<%=TxtservicechrgPrc.ClientID %>').val();
                                                }
                                            }
                                        });


                                        $(".rg").change(function () {


                                            var crpayment = "";

                                            if ($('#<%= rdbfull.ClientID %>').is(":checked")) {
                                                crpayment = "rdbfull";
                                            }

                                            if ($('#<%= rdbPart.ClientID %>').is(":checked")) {
                                                crpayment = "rdbPart";
                                            }

                                            document.getElementById("<%=hfRadio.ClientID %>").value = crpayment;


                                            if ($('#<%= rdbfull.ClientID %>').is(":checked")) {



                                                $('#<%= lblcreditnoteamtn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteAmt.ClientID %>').hide();
                                                $('#<%=lblCreditNoteCGSTPrcn.ClientID %>').hide();
                                                $('#<%=txtCreditNoteCGSTPrc.ClientID %>').hide();
                                                $('#<%= lblCreditNoteCGSTn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteCGST.ClientID %>').hide();
                                                $('#<%=lblCreditNoteSGSTPrcn.ClientID %>').hide();
                                                $('#<%=txtCreditNoteSGSTPrc.ClientID %>').hide();
                                                $('#<%=txtCreditNoteCGSTPrc.ClientID %>').hide();
                                                $('#<%= lblCreditNoteSGSTn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteSGST.ClientID %>').hide();
                                                $('#<%=lblCreditNoteIGSTPrcn.ClientID %>').hide();
                                                $('#<%=txtCreditNoteIGSTPrc.ClientID %>').hide();
                                                $('#<%= lblCreditNoteIGSTn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteIGST.ClientID %>').hide();
                                                $('#<%= lbltotaltaxamtn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteTaxamt.ClientID %>').hide();
                                                $('#<%= lblcnhsn.ClientID %>').hide();
                                                $('#<%= txtCreditNoteHSN.ClientID %>').hide();

                                                BindData();
                                                return false;


                                            }

                                            if ($('#<%= rdbPart.ClientID %>').is(":checked")) {

                                                $('#<%= lblcreditnoteamtn.ClientID %>').show();
                                                $('#<%= txtCreditNoteAmt.ClientID %>').show();
                                                $('#<%=lblCreditNoteCGSTPrcn.ClientID %>').show();
                                                $('#<%=txtCreditNoteCGSTPrc.ClientID %>').show();
                                                $('#<%= lblCreditNoteCGSTn.ClientID %>').show();
                                                $('#<%= txtCreditNoteCGST.ClientID %>').show();
                                                $('#<%=lblCreditNoteSGSTPrcn.ClientID %>').show();
                                                $('#<%=txtCreditNoteSGSTPrc.ClientID %>').show();
                                                $('#<%=txtCreditNoteCGSTPrc.ClientID %>').show();
                                                $('#<%= lblCreditNoteSGSTn.ClientID %>').show();
                                                $('#<%= txtCreditNoteSGST.ClientID %>').show();
                                                $('#<%=lblCreditNoteIGSTPrcn.ClientID %>').show();
                                                $('#<%=txtCreditNoteIGSTPrc.ClientID %>').show();
                                                $('#<%= lblCreditNoteIGSTn.ClientID %>').show();
                                                $('#<%= txtCreditNoteIGST.ClientID %>').show();
                                                $('#<%= lbltotaltaxamtn.ClientID %>').show();
                                                $('#<%= txtCreditNoteTaxamt.ClientID %>').show();
                                                $('#<%= lblcnhsn.ClientID %>').show();
                                                $('#<%= txtCreditNoteHSN.ClientID %>').show();

                                                BindData();
                                                return false;

                                            }


                                        })


                                        var CreditNoteRemarksValues = [];
                                        var CreditNoteHSNValues = [];
                                        var CreditNoteAmtValues = [];
                                        var CreditNoteAmtValues = [];
                                        var CreditNoteCGSTPrcValues = [];
                                        var CreditNoteCGSTValues = [];
                                        var CreditNoteSGSTPrcValues = [];
                                        var CreditNoteSGSTValues = [];
                                        var CreditNoteIGSTPrcValues = [];
                                        var CreditNoteIGSTValues = [];
                                        var CreditNoteTotalTaxAmtValues = [];

                                        $('#<%= btnAdd.ClientID %>').click(function (e) {

                                            e.preventDefault();

                                            if ($('#<%= rdbfull.ClientID %>').is(":checked")) {

                                                if (document.getElementById("<%=txtCreditNoteRemarks.ClientID %>").value == "") {

                                                    alert("Please enter credit note remarks")
                                                    return;
                                                }
                                                else {

                                                    document.getElementById("<%=hfCreditNoteRemarks.ClientID %>").value = document.getElementById("<%=txtCreditNoteRemarks.ClientID %>").value;
                                                    document.getElementById("<%=hfCreditNoteDt.ClientID %>").value = document.getElementById("<%=txtCreditnotedt.ClientID %>").value;

                                                    $('#<%= rdbfull.ClientID %>').prop('checked', true);

                                                    getvalues();

                                                    document.getElementById("<%=tempCnDetails.ClientID %>").click();
                                                        return false;
                                                    }
                                                }
                                                else {

                                                    if (document.getElementById("<%=txtCreditNoteRemarks.ClientID %>").value == "") {

                                                    alert("Please enter credit note remarks")
                                                    return;
                                                }
                                                else if (document.getElementById("<%=txtCreditNoteHSN.ClientID %>").value == "") {

                                                        alert("Please enter credit note HSN")
                                                        return;
                                                    }
                                                    else if (document.getElementById("<%=txtCreditNoteAmt.ClientID %>").value == "0" || document.getElementById("<%=txtCreditNoteAmt.ClientID %>").value == "") {

                                                    alert("Please enter credit note Amount")
                                                    return;
                                                }
                                                else {

                                                    getvalues();

                                                    return false;
                                                }
                                        }
                                        });


                                        function getvalues() {
                                            //Reference the GridView.
                                            var gridView = $("[id*=gvCreditNoteSummary]");

                                            //Reference the first row.
                                            var row = gridView.find("tr").eq(1);



                                            //Check if row is dummy, if yes then remove.
                                            if ($.trim(row.find("td").eq(1).html()) == "") {
                                                row.remove();
                                            }

                                            //Clone the reference first row.
                                            row = row.clone(true);



                                            debugger;
                                            //Add the Country value to second cell.
                                            var txtCreditNoteRemarks = $("[id*=txtCreditNoteRemarks]");
                                            CreditNoteRemarksValues.push(txtCreditNoteRemarks.val());
                                            document.getElementById("<%=hfnCreditNoteRemarks.ClientID %>").value = CreditNoteRemarksValues;
                                            SetValue(row, 0, "CreditNoteRemarks", txtCreditNoteRemarks);



                                            //Add the Country value to third cell.
                                            var txtCreditNoteHSN = $("[id*=txtCreditNoteHSN]");
                                            CreditNoteHSNValues.push(txtCreditNoteHSN.val());
                                            document.getElementById("<%=hfnCreditNoteHSN.ClientID %>").value = CreditNoteHSNValues;
                                            SetValue(row, 1, "CreditNoteHSN", txtCreditNoteHSN);



                                            var txtCreditNoteAmt = $("[id*=txtCreditNoteAmt]");
                                            CreditNoteAmtValues.push(txtCreditNoteAmt.val());
                                            document.getElementById("<%=hfnCreditNoteAmt.ClientID %>").value = CreditNoteAmtValues;
                                            SetValue(row, 2, "CreditNoteAmt", txtCreditNoteAmt);

                                            var txtCreditNoteCGSTPrc = $("[id*=txtCreditNoteCGSTPrc]");
                                            CreditNoteCGSTPrcValues.push(txtCreditNoteCGSTPrc.val());
                                            document.getElementById("<%=hfnCreditNoteCGSTPrc.ClientID %>").value = CreditNoteCGSTPrcValues;
                                                SetValue(row, 3, "CreditNoteCGSTPrc", txtCreditNoteCGSTPrc);


                                                var txtCreditNoteCGST = $('#<%= txtCreditNoteCGST.ClientID %>');
                                                CreditNoteCGSTValues.push(txtCreditNoteCGST.val());
                                                document.getElementById("<%=hfnCreditNoteCGST.ClientID %>").value = CreditNoteCGSTValues;
                                                SetValue(row, 4, "CreditNoteCGST", txtCreditNoteCGST);

                                                var txtCreditNoteSGSTPrc = $("[id*=txtCreditNoteSGSTPrc]");
                                                CreditNoteSGSTPrcValues.push(txtCreditNoteSGSTPrc.val());
                                                document.getElementById("<%=hfnCreditNoteSGSTPrc.ClientID %>").value = CreditNoteSGSTPrcValues;
                                                SetValue(row, 5, "CreditNoteSGSTPrc", txtCreditNoteSGSTPrc);

                                                var txtCreditNoteSGST = $('#<%= txtCreditNoteSGST.ClientID %>');
                                                CreditNoteSGSTValues.push(txtCreditNoteSGST.val());
                                                document.getElementById("<%=hfnCreditNoteSGST.ClientID %>").value = CreditNoteSGSTValues;
                                                SetValue(row, 6, "CreditNoteSGST", txtCreditNoteSGST);

                                                var txtCreditNoteIGSTPrc = $("[id*=txtCreditNoteIGSTPrc]");
                                                CreditNoteIGSTPrcValues.push(txtCreditNoteIGSTPrc.val());
                                                document.getElementById("<%=hfnCreditNoteIGSTPrc.ClientID %>").value = CreditNoteIGSTPrcValues;
                                                SetValue(row, 7, "CreditNoteIGSTPrc", txtCreditNoteIGSTPrc);

                                                var txtCreditNoteIGST = $('#<%= txtCreditNoteIGST.ClientID %>');
                                                CreditNoteIGSTValues.push(txtCreditNoteIGST.val());
                                                document.getElementById("<%=hfnCreditNoteIGST.ClientID %>").value = CreditNoteIGSTValues;
                                                SetValue(row, 8, "CreditNoteIGST", txtCreditNoteIGST);

                                                var txtCreditNoteTaxamt = $("[id*=txtCreditNoteTaxamt]");
                                                CreditNoteTotalTaxAmtValues.push(txtCreditNoteTaxamt.val());
                                                document.getElementById("<%=hfnCreditNoteTotalTaxAmount.ClientID %>").value = CreditNoteTotalTaxAmtValues;
                                                SetValue(row, 9, "CreditNoteTotalTaxAmt", txtCreditNoteTaxamt);

                                            //Add the row to the GridView.
                                                gridView.append(row);

                                            }

                                        function SetValue(row, index, name, textbox) {


                                            //Reference the Cell and set the value.
                                            row.find("td").eq(index).html(textbox.val());

                                            //Create and add a Hidden Field to send value to server.
                                            var input = $("<input type = 'hidden'  />");
                                            input.prop("name", name);
                                            input.val(textbox.val());
                                            row.find("td").eq(index).append(input);


                                            //Clear the TextBox.

                                            if (name != "CreditNoteCGSTPrc" && name != "CreditNoteSGSTPrc" && name != "CreditNoteIGSTPrc") {
                                                textbox.val("");
                                            }

                                        }


                                        if (parseFloat($('#<%= lblCGST.ClientID %>').val()) > 0) {

                                            $('#<%= txtCreditNoteCGSTPrc.ClientID %>').val((parseFloat($('#<%= TxtCGSTPrc.ClientID %>').val())));
                                            $('#<%= txtCreditNoteSGSTPrc.ClientID %>').val((parseFloat($('#<%= TxtSGSTPrc.ClientID %>').val())));
                                            $('#<%= txtCreditNoteIGSTPrc.ClientID %>').val(0);
                                        }

                                        if (parseFloat($('#<%= lblIGST.ClientID %>').val()) > 0) {

                                            $('#<%= txtCreditNoteIGSTPrc.ClientID %>').val((parseFloat($('#<%= TxtIGSTPrc.ClientID %>').val())));
                                            $('#<%= txtCreditNoteSGSTPrc.ClientID %>').val(0);
                                            $('#<%= txtCreditNoteCGSTPrc.ClientID %>').val(0);
                                        }


                                        var CreditNoteCGSTAmtval = 0; var CreditNoteSGSTAmtval = 0; var CreditNoteIGSTAmtval = 0;

                                        $("input[id*=txtCreditNoteAmt],input[id*=txtCreditNoteCGSTPrc],input[id*=txtCreditNoteSGSTPrc],input[id*=txtCreditNoteIGSTPrc]").keyup(function () {

                                            var textValue = $('#<%= txtCreditNoteAmt.ClientID %>').val();

                                            $('#<%= txtCreditNoteSGSTPrc.ClientID %>').val($('#<%= txtCreditNoteCGSTPrc.ClientID %>').val());

                                            if (parseFloat($('#<%= lblCGST.ClientID %>').val()) > 0) {


                                                CreditNoteCGSTAmtval = (parseFloat(textValue) * (parseFloat($('#<%= txtCreditNoteCGSTPrc.ClientID %>').val())) / 100).toFixed(2)
                                                CreditNoteSGSTAmtval = (parseFloat(textValue) * (parseFloat($('#<%= txtCreditNoteSGSTPrc.ClientID %>').val())) / 100).toFixed(2)


                                                $('#<%= txtCreditNoteCGST.ClientID %>').val(CreditNoteCGSTAmtval);
                                                $('#<%= txtCreditNoteSGST.ClientID %>').val(CreditNoteSGSTAmtval);
                                                $('#<%= txtCreditNoteIGST.ClientID %>').val("0");
                                                $('#<%= txtCreditNoteIGSTPrc.ClientID %>').val("0");

                                            }
                                            else {


                                                $('#<%= txtCreditNoteCGST.ClientID %>').val("0");
                                                $('#<%= txtCreditNoteSGST.ClientID %>').val("0");
                                                $('#<%= txtCreditNoteCGSTPrc.ClientID %>').val("0");
                                                $('#<%= txtCreditNoteSGSTPrc.ClientID %>').val("0");
                                                CreditNoteIGSTAmtval = (parseFloat(textValue) * (parseFloat($('#<%= txtCreditNoteIGSTPrc.ClientID %>').val())) / 100).toFixed(2)
                                                $('#<%= txtCreditNoteIGST.ClientID %>').val(CreditNoteIGSTAmtval);

                                            }


                                            var Taxamt = 0;
                                            Taxamt = (parseFloat($('#<%= txtCreditNoteAmt.ClientID %>').val()) + parseFloat($('#<%= txtCreditNoteCGST.ClientID %>').val()) + parseFloat($('#<%= txtCreditNoteSGST.ClientID %>').val()) + parseFloat($('#<%= txtCreditNoteIGST.ClientID %>').val()))



                                            $('#<%= txtCreditNoteTaxamt.ClientID %>').val(Taxamt);

                                        });



                                    });

                                    $(document).on("click", "[id*=btncredited]", function (e) {

                                        e.preventDefault();
                                        CreditNotePopup();
                                        BindData();
                                    });

                                    function CreditNotePopup() {

                                        $("#dialog1").dialog({
                                            title: "Credit Note Details",
                                            width: 750,
                                            height: 650,
                                            buttons: {
                                                "Save": function () {
                                                    debugger;
                                                    var CreditNoteDate = document.getElementById("<%=txtCreditnotedt.ClientID %>").value;


                                                    var crpayment = "";

                                                    if ($('#<%= rdbfull.ClientID %>').is(":checked")) {
                                                        crpayment = "rdbfull";
                                                    }

                                                    if ($('#<%= rdbPart.ClientID %>').is(":checked")) {
                                                        crpayment = "rdbPart";
                                                    }



                                                    if (crpayment == "rdbfull") {


                                                        if (CreditNoteDate == '') {
                                                            alert("Please fill credit note issued date")
                                                            document.getElementById("<%=txtCreditnotedt.ClientID %>").focus();
                                                        }

                                                        else {


                                                            document.getElementById("<%=hfCreditNoteDt.ClientID %>").value = CreditNoteDate;
                                                            document.getElementById("<%=hfCreditNoteRemarks.ClientID %>").value = "";
                                                            document.getElementById("<%=hfCreditNoteHSN.ClientID %>").value = "";
                                                            document.getElementById("<%=hfCreditNoteAmt.ClientID %>").value = "0";
                                                            document.getElementById("<%=hfCreditPayment.ClientID %>").value = crpayment;

                                                            $(this).dialog("close");
                                                            document.getElementById("<%=tempCN.ClientID %>").click();
                                                                return true;
                                                            }

                                                        }

                                                    if (crpayment == "rdbPart") {

                                                        var CreditNoteRemarks = document.getElementById("<%=txtCreditNoteRemarks.ClientID %>").value;
                                                        var CreditNoteHSN = document.getElementById("<%=txtCreditNoteHSN.ClientID %>").value;
                                                        var CreditNoteAmount = document.getElementById("<%=txtCreditNoteAmt.ClientID %>").value;


                                                        if (CreditNoteDate == '') {
                                                            alert("Please fill credit note issued date")
                                                            document.getElementById("<%=txtCreditnotedt.ClientID %>").focus();
                                                            }
                                                            else {



                                                                document.getElementById("<%=hfCreditNoteDt.ClientID %>").value = CreditNoteDate;
                                                                document.getElementById("<%=hfCreditNoteRemarks.ClientID %>").value = CreditNoteRemarks;
                                                                document.getElementById("<%=hfCreditNoteHSN.ClientID %>").value = CreditNoteHSN;
                                                                document.getElementById("<%=hfCreditNoteAmt.ClientID %>").value = CreditNoteAmount;
                                                                document.getElementById("<%=hfCreditPayment.ClientID %>").value = crpayment;

                                                                $(this).dialog("close");
                                                                document.getElementById("<%=tempCN.ClientID %>").click();
                                                                return true;
                                                            }

                                                        }
                                                }
                                            },
                                            modal: true
                                        });
                                            return false;
                                        }

                                </script>

                                <table width="100%" cellpadding="5" cellspacing="5">


                                    <tr>

                                        <td class="auto-style2">Client ID<span style="color: Red">*</span>
                                        </td>

                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddlclientid" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged"
                                                Width="120px">
                                            </asp:DropDownList>
                                        </td>

                                        <td style="padding-left: 20px">Client Name<span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCname" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCname_OnSelectedIndexChanged" Height="18px" Width="390px">
                                            </asp:DropDownList>
                                        </td>

                                        <td>
                                            <asp:Button ID="btngenratepayment"
                                                runat="server" class="btn save" Text="Genrate Bill" OnClick="Btn_Genrate_Invoice_Click" Style="margin-left: 5px"
                                                OnClientClick='return confirm(" Are you sure you  want to  generate bill ?");' Width="100px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnFreeze" Visible="false"
                                                runat="server" class="btn save" Text="Freeze" OnClick="btnFreeze_Click"
                                                OnClientClick='return confirm(" Are you sure you want to freeze the bill ?");' />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnUnFreeze" Visible="false"
                                                runat="server" class="btn save" Text="UnFreeze" OnClick="btnUnFreeze_Click"
                                                OnClientClick='return confirm(" Are you sure you want to unfreeze the bill ?");' />
                                        </td>



                                    </tr>

                                    <tr>

                                        <td class="auto-style2">Type</td>

                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged"
                                                class="sdrop" Width="120px">
                                                <asp:ListItem>Normal</asp:ListItem>
                                                <asp:ListItem>Manual</asp:ListItem>
                                                <asp:ListItem>Arrears</asp:ListItem>
                                                <asp:ListItem>Bonus</asp:ListItem>
                                                <asp:ListItem>Elnh</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="auto-style3">Year
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtyear" runat="server" Text="2013" Enabled="False" class="sinput"
                                                Width="50px"></asp:TextBox>
                                        </td>

                                        <td>
                                            <asp:Button ID="btncredited" Style="margin-left: 5px" Visible="false"
                                                runat="server" class="btn save" Text="Credit Note" />
                                        </td>

                                        <td>
                                            <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClick="btnSendMail_Click" Visible="false"
                                                OnClientClick='return confirm("Are you sure you want to send mail ?");' Style="margin-left: 46px" />
                                        </td>

                                        <td></td>

                                    </tr>

                                    <tr>
                                        <td class="auto-style2">Month<span style="color: Red">*</span>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged"
                                                class="sdrop" Width="120px">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtmonth" AutoComplete="off" runat="server" AutoPostBack="true" Width="120px"
                                                OnTextChanged="txtmonthOnTextChanged" Visible="false"></asp:TextBox>
                                            &nbsp;&nbsp;
                                                            <asp:CheckBox ID="Chk_Month" runat="server"
                                                                Text="Old" />
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="true"
                                                Format="dd/MM/yyyy" TargetControlID="txtmonth">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="auto-style3">From
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfromdate" AutoComplete="off" runat="server" Enabled="true" class="sinput" Width="80px"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtfromdate_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtfromdate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Style="padding-right: 60px" runat="server" Text="To "></asp:Label></td>

                                        <td>


                                            <asp:TextBox ID="txttodate" AutoComplete="off" runat="server" Enabled="true" class="sinput" Width="80px" Style="margin-left: -60px"
                                                onkeyup="dtval(this,event)"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txttodate_Calender" runat="server" Enabled="true" TargetControlID="txttodate"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td></td>
                                    </tr>

                                    <tr>

                                        <td class="auto-style2">
                                            <asp:Label ID="lblbilltype" runat="server" Text="Bill Type :" Visible="false"></asp:Label>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:RadioButton ID="rdbcreatebill" runat="server" Text="Create" GroupName="MB" Checked="true" Visible="false" />
                                            <asp:RadioButton ID="rdbmodifybill" runat="server" Text="Modify" GroupName="MB" Visible="false" />
                                        </td>

                                        <td class="auto-style3">
                                            <asp:Label ID="lblManualBillNo" runat="server" Text="Manual Billing Bill No's" Visible="false"></asp:Label>

                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMBBillnos" runat="server" OnSelectedIndexChanged="ddlMBBillnos_OnSelectedIndexChanged"
                                                AutoPostBack="true" Width="150px" CssClass="sdrop" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>

                                        <td></td>

                                    </tr>

                                    <tr>
                                        <td class="auto-style2">
                                            <asp:Label ID="lblbillnolatesttest" runat="server" Style="font-weight: bold;" Text="BillNo :"> </asp:Label>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblbillnolatest" runat="server" Style="font-weight: bold;" Text=""> </asp:Label>
                                        </td>
                                        <td class="auto-style3">
                                            <asp:Label ID="lblbilldate" runat="server" Text="Bill Date :" Style="font-weight: bold;"></asp:Label>
                                        </td>


                                        <td>
                                            <asp:TextBox ID="txtbilldate" runat="server" Text="" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtbilldate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEstartdate" runat="server" Enabled="True" TargetControlID="txtbilldate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>


                                    </tr>

                                    <tr>

                                        <td class="auto-style3">
                                            <asp:Label ID="Label1" runat="server" Text="Due Date :" Visible="false" Style="font-weight: bold;"></asp:Label>
                                        </td>

                                        <td>
                                            <asp:TextBox ID="txtduedate" runat="server" Text="" Visible="false" class="sinput" Width="80px" onkeyup="dtval(this,event)"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="true"
                                                TargetControlID="txtduedate" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBduedate" runat="server" Enabled="True" TargetControlID="txtduedate"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>

                                    </tr>


                                    <tr>
                                        <td>
                                            <h3 style="border: none; background: none;">Invoice Description</h3>

                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdescription" runat="server" MaxLength="200" TabIndex="35" Width="170px" Height="110px"
                                                Text="We are presenting our bill for the Security Services provided at your establishment. Kindly release the payment at the earliest."
                                                Style="font-variant: normal; padding: 10px" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr style="display: none">
                                        <td>Bank Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankname" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>Bank A/c No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccNo" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="auto-style1">IFSC
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtifsccode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                </table>


                                <table width="70%" cellpadding="5" cellspacing="5">
                                </table>
                                <table width="50%" cellpadding="5" cellspacing="5" style="margin-left: 17px; visibility: hidden">
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="linkmanualbilling" runat="server" Text="Manual Bills" PostBackUrl="~/Manual Billing.aspx"></asp:LinkButton>
                                        </td>

                                        <td>
                                            <asp:LinkButton ID="LINKNEWMANUALBILLING" runat="server" Text="New Manual Bill Model" PostBackUrl="~/newmanualbill.aspx"></asp:LinkButton>
                                        </td>

                                    </tr>
                                </table>

                                &nbsp;
                                <asp:LinkButton ID="linkdelete" runat="server" Text="Delete Bills" Visible="false"></asp:LinkButton>
                                <div style="margin-left: 30px">
                                    <cc1:ModalPopupExtender ID="mpebilldelete" runat="server" TargetControlID="linkdelete"
                                        PopupControlID="pnlbilldeletedetails" CancelControlID="btncancel">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="pnlbilldeletedetails" runat="server" Width="400px" Style="background-color: Silver"
                                        Visible="false">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Enter Bill No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbillno" runat="server" Width="240px" AutoPostBack="true" OnTextChanged="txtbillno_OnTextChanged"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <br />

                                                    <tr>
                                                        <td>Client Id
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtclientid" runat="server" Width="240px"> 
                                   
                                                            </asp:TextBox>
                                                        </td>
                                                        <tr>
                                                            <td>Client Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtclientname" runat="server" Width="240px"> 
                                   
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btndelelte" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <table style="margin-left: 150px">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btndelelte" runat="server" Text="Delete" CssClass="btn save" OnClientClick='return confirm(" Are you sure you  want to  delete bill ?");'
                                                        OnClick="btndelelte_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btncancel" runat="server" Text="Cancel/Close" CssClass="btn save"
                                                        Width="95px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>

                                &nbsp;
                                    <cc1:ModalPopupExtender ID="ModalFreezeDetails" runat="server" TargetControlID="btnUnFreeze" PopupControlID="pnlFreeze"
                                        BackgroundCssClass="PnlBackground">
                                    </cc1:ModalPopupExtender>

                                <asp:Panel ID="pnlFreeze" runat="server" Height="100px" Width="300px" DefaultButton="btnFreezeSubmit" Style="display: none; position: absolute; background-color: white; box-shadow: rgba(0,0,0,0.4)">
                                    <asp:UpdatePanel ID="UpFreeze" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font: bold; font-size: medium">&nbsp;&nbsp;&nbsp;
                            Enter Password:
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="TxtFreeze" runat="server" TextMode="Password"></asp:TextBox>
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
                              <asp:Button ID="btnFreezeSubmit" runat="server" Text="Submit" OnClick="btnFreezeSubmit_Click" class="btn Save" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnFreezeClose" runat="server" Text="Close" OnClick="btnFreezeClose_Click" class="btn Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>


                                <cc1:ModalPopupExtender ID="modelLogindetails" runat="server" TargetControlID="Chk_Month" PopupControlID="pnllogin"
                                    BackgroundCssClass="modalBackground">
                                </cc1:ModalPopupExtender>

                                <asp:Panel ID="pnllogin" runat="server" Height="100px" Width="300px" Style="display: none; position: absolute; background-color: Silver;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font: bold; font-size: medium">&nbsp;&nbsp;&nbsp;
                            Enter Password:
                                                    </td>
                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
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
                              <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" class="btn Save" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" class="btn Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>




                                <asp:HiddenField ID="hdDesignations" runat="server" ViewStateMode="Enabled" />

                                <table style="margin-top: 10px">
                                    <tr style="margin-left: 20px">
                                        <td>
                                            <asp:Label ID="lblIRN" runat="server" Visible="false" Style="font-weight: bold; font-size: 14px; margin-left: 5px;"></asp:Label>
                                        </td>
                                    </tr>
                                </table>


                                <table style="float: right">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnGenerateIRN" runat="server" Text="Generate IRN" OnClick="btnGenerateIRN_Click" Visible="false" />
                                        </td>

                                        <td>

                                            <asp:Button ID="btnCancelIRN" runat="server" Text="Cancel IRN" Visible="false" />

                                        </td>
                                        <td>
                                            <asp:Button ID="btncleardata" runat="server" Text="Clear" OnClick="btncleardata_Click" />

                                        </td>
                                    </tr>
                                </table>



                                <asp:Panel ID="pnlcreditdetails" runat="server" GroupingText="&nbsp;&nbsp;Credit Note Details&nbsp;&nbsp;" Visible="false">
                                    <div style="overflow-x: scroll; width: 950px;">
                                        <asp:GridView ID="gvCreditNote" runat="server" Width="150%" CssClass="table table-striped table-bordered table-condensed table-hover" AutoGenerateColumns="False" Style="overflow-x: scroll">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Actions" ItemStyle-Width="230px" HeaderStyle-Width="230px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lbtn_Download" ImageUrl="~/css/assets/download.png" runat="server"
                                                            ToolTip="Download" OnClick="btncreditnoteNew_Click" Style="position: relative; top: 8px;" />
                                                        <asp:ImageButton ID="lbtn_Delete" ImageUrl="~/css/assets/delete.png" runat="server" Style="position: relative; top: 8px;"
                                                            OnClientClick='return confirm("Are you sure you want to delete credit note ?");' OnClick="btnDeleteCreditNote_Click" ToolTip="Delete" />
                                                        <asp:Button ID="btnIssueCreditNote" runat="server" Text="Cr IRN" OnClientClick='return confirm("Are you sure you want to generate Credit note IRN?");' OnClick="btnIssueCreditNote_Click" />
                                                        <asp:Button ID="btnCancelCreditNote" runat="server" Text="Cancel IRN" OnClientClick='return confirm("Are you sure you want to cancel Credit note IRN?");' OnClick="btnIssueCreditNote_Click" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="SNo" HeaderText="S.No" />
                                                <asp:TemplateField HeaderText="Bill No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBillNo" runat="server" Text='<%#Bind("BillNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Credit Note No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreditNoteNo" runat="server" Text='<%#Bind("CreditNoteNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CreditNoteDate" HeaderText="Credit Note Date" />
                                                <asp:BoundField DataField="CreditNoteAmount" HeaderText="Credit Note Amount" />
                                                <asp:BoundField DataField="CreditNoteCGST" HeaderText="Credit Note CGST" />
                                                <asp:BoundField DataField="CreditNoteSGST" HeaderText="Credit Note SGST" />
                                                <asp:BoundField DataField="CreditNoteIGST" HeaderText="Credit Note IGST" />
                                                <asp:BoundField DataField="CreditNoteRoundOffamt" HeaderText="Credit Note R.off" />
                                                <asp:BoundField DataField="CreditNoteGrandTotal" HeaderText="Credit Note Grand Total" />
                                                <asp:BoundField DataField="CreditNoteIRN" HeaderText=" Credit Note IRN" />

                                            </Columns>
                                        </asp:GridView>

                                    </div>

                                </asp:Panel>

                                <div class="rounded_corners" style="overflow: auto; width: 99%; margin-left: 17px">
                                    <asp:GridView ID="gvClientBilling" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-BackColor="BlueViolet"
                                        EmptyDataRowStyle-BorderColor="Aquamarine" Width="99%" CellPadding="4" CellSpacing="3"
                                        ForeColor="#333333" GridLines="None">
                                        <RowStyle BackColor="#EFF3FB" Height="30" />


                                        <Columns>
                                            <%-- 0 --%>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>" Width="30px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 1 --%>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("Designid") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lbltype" runat="server" Text='<%# Bind("type") %>' Visible="false"></asp:Label>
                                                    <asp:TextBox ID="lbldesgn" runat="server" Text='<%# Bind("Designation") %>' Width="95%" Enabled="false" CssClass="txtautofilldesg" AutoPostBack="True" OnTextChanged="lbldesgn_TextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender runat="server" ID="Ftbdesignid" TargetControlID="lbldesgn"
                                                        FilterMode="InvalidChars" InvalidChars="'">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:Label ID="lblnoofdays" runat="server" Text='<%# Bind("Noofdays") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 2 --%>
                                            <asp:TemplateField HeaderText="HSN Number" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlHSNNumber" runat="server" Width="95%" Style="text-align: left">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:TextBox ID="txtUOM" runat="server" Text='<%# Bind("UOM") %>' Style="width: 50px" Visible="false"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 3 --%>
                                            <asp:TemplateField HeaderText="No. of Emps " HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblnoofemployees" runat="server" Text='<%#Bind("NoofEmps")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                    <asp:Label ID="lblextra" runat="server" Text='<%# Bind("Extra") %>' Visible="false"></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 4 --%>
                                            <asp:TemplateField HeaderText="No.of Dts/Hrs" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblNoOfDuties" runat="server" Text='<%#Bind("DutyHours")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 5 --%>
                                            <asp:TemplateField HeaderText="Pay Rate" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblpayrate" runat="server" Text='<%#Eval("payrate", "{0:0.##}")%>' Enabled="false" Width="95%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBPayRate" runat="server" Enabled="True"
                                                        TargetControlID="lblpayrate" ValidChars="-0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 6 --%>
                                            <asp:TemplateField HeaderText="New Pay Rate" Visible="false" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewPayRate" runat="server" Style="text-align: center"
                                                        Text='<%#Eval("newpayrate", "{0:0.##}")%>' Width="95%"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBNewPayRate" runat="server" Enabled="True"
                                                        TargetControlID="txtNewPayRate" ValidChars="0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 7 --%>
                                            <asp:TemplateField HeaderText="Duties Type" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddldutytype" runat="server" Width="95%">
                                                        <asp:ListItem Value="0">P.M</asp:ListItem>
                                                        <asp:ListItem Value="1">P.D</asp:ListItem>
                                                        <asp:ListItem Value="2">P.Hr</asp:ListItem>
                                                        <asp:ListItem Value="3">P.Sft</asp:ListItem>
                                                        <asp:ListItem Value="4">Fixed</asp:ListItem>
                                                        <asp:ListItem Value="5">Heading</asp:ListItem>

                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 8 --%>
                                            <asp:TemplateField HeaderText="NOD" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlnod" runat="server" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="22" Selected="True">22</asp:ListItem>
                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="30.41">30.41</asp:ListItem>
                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>




                                            <%-- 9--%>
                                            <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblda" runat="server" Text='<%#Eval("BasicDa", "{0:0.##}")%>' Enabled="false" Width="95%"> </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTBDa" runat="server" Enabled="True"
                                                        TargetControlID="lblda" ValidChars="-0123456789.">
                                                    </cc1:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%-- 10 --%>
                                            <asp:TemplateField HeaderText="Total" HeaderStyle-Width="70px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblAmount" runat="server" Text='<%#Eval("BasicDa", "{0:0.##}")%>' Enabled="false" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 11 --%>
                                            <asp:TemplateField HeaderText="Service Chrg %" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblSchrgPrc" runat="server" Text='<%#Eval("ServiceChargesPrc", "{0:0.##}")%>' Enabled="false" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 12 --%>
                                            <asp:TemplateField HeaderText="Service Chrg Amt" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblSchrgAmt" runat="server" Text='<%#Eval("ServiceCharges", "{0:0.##}")%>' Enabled="false" Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 13 --%>
                                            <asp:TemplateField HeaderText="GST %" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblGSTper" runat="server" Text='<%#Eval("GSTper", "{0:0.##}")%>' Width="99%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <%-- 14 --%>

                                            <asp:TemplateField HeaderText="CGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblCGSTAmount" runat="server" Text='<%#Eval("CGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblCGSTPrc" runat="server" Text='<%#Eval("CGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 15 --%>

                                            <asp:TemplateField HeaderText="SGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblSGSTAmount" runat="server" Text='<%#Eval("SGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblSGSTPrc" runat="server" Text='<%#Eval("SGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- 16 --%>

                                            <asp:TemplateField HeaderText="IGST">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblIGSTAmount" runat="server" Text='<%#Eval("IGSTAmt", "{0:0.##}")%>' Enabled="false" Width="50px"></asp:TextBox>
                                                    <asp:TextBox ID="lblIGSTPrc" runat="server" Text='<%#Eval("IGSTPrc", "{0:0.##}")%>' Enabled="false" Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 17 --%>
                                            <asp:TemplateField HeaderText="Total Amt">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblTotalTaxmount" runat="server" Text='<%#Eval("TotalTaxAmount", "{0:0.##}")%>' Enabled="false" Width="80px"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- 18 --%>
                                            <asp:TemplateField HeaderText="OT Amount" HeaderStyle-Width="70px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOtAmount" runat="server" Text='<%#Eval("OTAmount", "{0:0.##}")%>' Width="70px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="10px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExtra" runat="server" Width="10px" Enabled="false" Style="text-align: center"></asp:CheckBox>
                                                    <asp:Label ID="txthsnno" runat="server" Text='<%#Eval("HSNNumber")%>' Width="70px"></asp:Label>
                                                    <asp:Label ID="lblsrchrgs" runat="server" Text="0" Width="70px" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlCalnType" runat="server" Enabled="false">
                                                        <asp:ListItem>Add</asp:ListItem>
                                                        <asp:ListItem>Subract</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblcaln" runat="server" Text='<%#Eval("CalnType")%>' Width="70px" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                        <EmptyDataRowStyle BackColor="BlueViolet" BorderColor="Aquamarine"></EmptyDataRowStyle>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>

                                <div>
                                    <asp:Button ID="btnAddNewRow" runat="server" Text="Add Row" OnClick="btnAddNewRow_Click" />
                                    <asp:Button ID="btnCalculateTotals" runat="server" Text="Calculate Totals" Visible="false"
                                        OnClick="btnCalculateTotals_Click" />

                                </div>



                                <table width="100%" cellpadding="5" cellspacing="5" style="margin-left: 17px">
                                    <tr>
                                        <td valign="top" width="37%">
                                            <asp:CheckBox ID="checkExtraData" Visible="false" Text="&nbsp;&nbsp;Extra Data for Billing" runat="server"
                                                Checked="false" AutoPostBack="True" OnCheckedChanged="checkExtraData_CheckedChanged" />
                                            <asp:Panel ID="panelRemarks" runat="server" Visible="false">
                                                <table width="100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>Sevice Tax
                                                        </td>
                                                        <td>Service Charge
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtmachinarycost" runat="server" Text="Machinery Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMachinery" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMachinary" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                            TargetControlID="txtMachinery" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMachinary" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterialcost" runat="server" Text="Material Cost :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMaterial" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesMaterial" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtMaterial" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesMaterial" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtMaintanancecost" runat="server" Text="Maintenance Work :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtElectical" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesElectrical" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesElectrical" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtElectical" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonetitle" runat="server" Text="Extra Amount one :" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextraonevalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtraone" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtraone" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                            TargetControlID="txtextraonevalue" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwotitle" runat="server" Height="19px" Text="Extra Amount Two :"
                                                                class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtextratwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTYesExtratwo" runat="server"
                                                                Checked="false" Text=" Yes" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSCYesExtratwo" runat="server" Checked="false"
                                                                Text=" Yes" />
                                                        </td>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                            TargetControlID="txtextratwovalue" ValidChars="0123456789.">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscount" runat="server" Text="Discounts :" class="sinput" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDiscounts" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                TargetControlID="txtDiscounts" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscountone" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwotitle" runat="server" Text="Discount Two:" class="sinput"
                                                                Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtdiscounttwovalue" runat="server" Text="" class="sinput" Width="50px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                TargetControlID="txtdiscounttwovalue" ValidChars="0123456789.">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSTDiscounttwo" runat="server" Checked="false" Text=" Before Service Tax" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td>Remarks :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text="" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td align="right" valign="top">
                                            <table width="70%" cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrTitle" Visible="false" Text=" 1/6 Reliever Charges : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblRelChrgAmt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lbltotal" Visible="false" Text="Total:" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblTotalResources" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceChargeTitle" Visible="false" Text=" Service Charges : " runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtservicechrgPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblServiceCharges" Text="" Visible="false" Enabled="false" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicechargetitle" Visible="false" Text=" Service Tax on Service Charges : " runat="server"></asp:Label></td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblStaxamtonServicecharge" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinarytitle" Visible="false" Text=" Service Charge on Machinary Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSChargeamtonMachinary" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenancetitle" Visible="false" Text=" Service Charge on Maintenance Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaintenance" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterialtitle" Visible="false" Text=" Service Charge on Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraonetitle" Visible="false" Text=" Service Charge on Extra amount one : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtraone" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwotitle" Visible="false" Text=" Service Charge on Extra amount two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblSchargeamtonExtratwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitlewithst" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinerywithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitlewithst" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitlewithst" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalwithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraonetitlewithst" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraonewithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwotitlewithst" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwowithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitlewithst" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountwithst" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwotitlewithst" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwowithst" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblServiceTaxTitle" Visible="false" Text="Service Tax :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtServiceTaxPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblServiceTax" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSBCESSTitle" Visible="false" Text="SB CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSBCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSBCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblKKCESSTitle" Visible="false" Text="KK CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtKKCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblKKCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <%-- region for GST as on 17-6-2017 by swathi--%>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCGSTTitle" Visible="false" Text="CGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSGSTTitle" Visible="false" Text="SGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblIGSTTitle" Visible="false" Text="IGST :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtIGSTPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblIGST" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess1Title" Visible="false" Text="Cess1 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess1Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCess1" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCess2Title" Visible="false" Text="Cess2 :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCess2Prc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCess2" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <%-- endregion for GST as on 17-6-2017 by swathi--%>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblCESSTitle" Visible="false" Text="CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblCESS" Text="" Visible="false" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblSheCESSTitle" Visible="false" Text="S&H Ed. CESS :" runat="server"></asp:Label>
                                                        <asp:TextBox ID="TxtSheCESSPrc" Text="" Visible="false" runat="server" Enabled="false" Width="40px"></asp:TextBox>

                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:TextBox ID="lblSheCESS" Visible="false" Text="" runat="server" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST75Title" Visible="false" Text="Less 75% Service Tax :" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST75" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblST25Title" Visible="false" Text="Service Tax Chargable @3.09%:" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblST25" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMachineryTitle" Visible="false" Text=" Machinery Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMachinery" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblMaterialTitle" Visible="false" Text=" Material Cost : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblMaterial" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblElectricalTitle" Visible="false" Text=" Maintenance Work : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblElectrical" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextraoneamttitle" Visible="false" Text="Extra Amount One : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextraamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamttitle" Visible="false" Text="Extra Amount Two : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblextratwoamt" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscountTitle" Visible="false" Text="Discount : " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscount" Visible="false" Text="" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwoTitle" Visible="false" Text="Discount Two: " runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right">
                                                        <asp:Label ID="lblDiscounttwo" Text="" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right; font-weight: bold">
                                                        <asp:Label ID="lblroundoffs" Text="Round off :" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right; font-weight: bold">
                                                        <asp:TextBox ID="txtRoundoffamt" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="80%" style="text-align: right; font-weight: bold">
                                                        <asp:Label ID="lblgrandtotalss" Text="Grand Total :" Visible="false" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%" style="text-align: right; font-weight: bold">
                                                        <asp:TextBox ID="lblGrandTotal" Text="" runat="server" Visible="false" Enabled="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <asp:Label ID="lblRemarks" Text="" runat="server" Visible="false"></asp:Label>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>


                                            </table>
                                </table>

                                <div>
                                    <asp:Label ID="lbltotalamount" runat="server"> </asp:Label>
                                </div>
                                <div style="width: 100%; font-weight: bold">
                                    <asp:Label ID="lblamtinwords" Text="" runat="server" Visible="false"> </asp:Label>
                                </div>
                                <table>
                                    <tr>

                                        <td>
                                            <asp:DropDownList ID="ddlfontsize" Width="60px" runat="server" CssClass="sdrop">
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="9">9</asp:ListItem>
                                                <asp:ListItem Value="8">8</asp:ListItem>
                                                <asp:ListItem Value="7">7</asp:ListItem>
                                                <asp:ListItem Value="6">6</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>
                                        <td style="text-align: right; font-weight: bold">
                                            <asp:Button ID="btninvoicenew" runat="server" Text="Tax Invoice" class="btn save" OnClick="btninvoicenew_Click" />

                                        </td>


                                        <td style="text-align: right; font-weight: bold">
                                            <asp:Button ID="btncreditbill" runat="server" Text="Credit Note" class="btn save" Visible="false" OnClick="btncreditnoteNew_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDeleteCreditNote" runat="server" Text="Delete Credit Note" OnClick="btnDeleteCreditNote_Click" Visible="false"
                                                OnClientClick='return confirm("Are you sure you want to delete credit note ?");' Style="margin-left: 46px" />

                                        </td>

                                    </tr>




                                </table>
                                </td>
                                        </tr>
                                  

                            </div>
                        </div>
                    </div>
                </div>

                <div id="dialog" style="display: none; height: 110px; margin: 15px; font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif,Apple Color Emoji,Segoe UI Emoji,Segoe UI Symbol;">

                    <div class="row" style="font-size: 12px;">
                        <div class="form-group">
                            <label style="font-weight: 700">Client Details</label>
                            <asp:Label ID="lblClientdetails" runat="server" Style="position: relative; left: 19px"></asp:Label>

                        </div>
                    </div>


                    <div class="row" style="font-size: 12px; margin-top: 17px">
                        <div class="form-group">
                            <label style="font-weight: 700">Cancel Reason</label>
                            <asp:DropDownList ID="ddlCnclRsn" runat="server" Style="position: relative; left: 15px;">
                                <asp:ListItem>Select</asp:ListItem>
                                <asp:ListItem>Duplicate</asp:ListItem>
                                <asp:ListItem>Data entry mistake</asp:ListItem>
                                <asp:ListItem>Order Cancelled</asp:ListItem>
                                <asp:ListItem>Others</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row" style="font-size: 12px;">
                        <div class="form-group">
                            <label style="font-weight: 700">Cancel Remarks</label>
                            <asp:TextBox ID="txtCnclRemarks" TextMode="MultiLine" runat="server" Style="position: relative; left: 7px; top: 11px; width: 200px"></asp:TextBox>
                        </div>
                    </div>

                </div>

                <div id="dialog1" style="display: none; height: 110px; margin: 15px; font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif,Apple Color Emoji,Segoe UI Emoji,Segoe UI Symbol;">

                    <div class="row" style="font-size: 12px;">
                        <div class="form-group">
                            <label style="font-weight: 700; position: relative; top: 15px;">Payment</label>
                            <asp:RadioButton ID="rdbfull" runat="server" class='rg' Checked="true" GroupName="py" Text="Full" Style="position: relative; left: 85px; top: 15px;" />
                            <asp:RadioButton ID="rdbPart" runat="server" class='rg' GroupName="py" Text="Part" Style="position: relative; left: 95px; top: 15px;" />
                        </div>
                    </div>

                    <div class="row" style="font-size: 12px; margin-top: 26px;">
                        <div class="form-group">
                            <label style="font-weight: 700">Credit Note Date</label>
                            <asp:TextBox ID="txtCreditnotedt" runat="server" Style="position: relative; left: 41px; top: 3px; width: 187px;"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true"
                                TargetControlID="txtCreditnotedt" Format="dd/MM/yyyy">
                            </cc1:CalendarExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txtCreditnotedt"
                                ValidChars="/0123456789">
                            </cc1:FilteredTextBoxExtender>
                        </div>
                    </div>

                    <div class="row" style="font-size: 12px;">
                        <div class="form-group">
                            <label style="font-weight: 700">Credit Note Remarks</label>
                            <asp:TextBox ID="txtCreditNoteRemarks" runat="server" TextMode="MultiLine" Style="position: relative; left: 19px; top: 10px; width: 187px; height: 40px"></asp:TextBox>

                        </div>
                    </div>

                    <div class="row" style="font-size: 12px; margin-top: 10px;">
                        <div class="form-group">
                            <label runat="server" id="lblcnhsn" style="font-weight: 700; position: relative; top: 12px; display: none">Credit Note HSN</label>
                            <asp:TextBox ID="txtCreditNoteHSN" Text="998525" runat="server" Style="position: relative; left: 43px; top: 11px; width: 185px; display: none"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row" style="font-size: 12px; margin-top: 15px;">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblcreditnoteamtn" Text="Credit Note Amount" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                            <asp:TextBox ID="txtCreditNoteAmt" Text="0" runat="server" Style="position: relative; left: 22px; top: 11px; width: 185px; display: none"></asp:TextBox>

                            <cc1:FilteredTextBoxExtender ID="FTBCrediteNoteamt" runat="server" Enabled="True" TargetControlID="txtCreditNoteAmt"
                                ValidChars=".0123456789">
                            </cc1:FilteredTextBoxExtender>

                        </div>
                    </div>

                    <div class="row" style="font-size: 12px; margin-top: 15px;">
                        <div class="form-group">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteCGSTPrcn" Text="CGST %" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteCGSTPrc" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none;">
                                        </asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True" TargetControlID="txtCreditNoteCGSTPrc"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteCGSTn" Text="CGST" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteCGST" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none;" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" TargetControlID="txtCreditNoteCGST"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteSGSTPrcn" Text="SGST %" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteSGSTPrc" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none;"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True" TargetControlID="txtCreditNoteSGSTPrc"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteSGSTn" Text="SGST" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteSGST" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" TargetControlID="txtCreditNoteSGST"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteIGSTPrcn" Text="IGST %" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteIGSTPrc" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none;"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True" TargetControlID="txtCreditNoteIGSTPrc"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lblCreditNoteIGSTn" Text="IGST" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteIGST" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" TargetControlID="txtCreditNoteIGST"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                    <td>
                                        <asp:Label runat="server" ID="lbltotaltaxamtn" Text="Taxable amt" Style="font-weight: 700; position: relative; top: 12px; display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditNoteTaxamt" Text="0" runat="server" Style="position: relative; top: 11px; width: 40px; display: none" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True" TargetControlID="txtCreditNoteTaxamt"
                                            ValidChars=".0123456789">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>

                    <div class="row" style="font-size: 12px; float: right; margin-top: 15px; margin-bottom: 15px">
                        <div class="form-group">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" />
                        </div>
                    </div>

                    <asp:GridView ID="gvCreditNoteSummary" CssClass="table" runat="server" AutoGenerateColumns="false">
                        <Columns>

                            <asp:TemplateField HeaderText="Credit Note Remarks" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteRemarks">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteRemarks")%>
                                    <asp:HiddenField ID="hfCreditNoteRemarks" runat="server" Value='<%# Bind("CreditNoteRemarks") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note HSN" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteHSN">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteHSN")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note Amt" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteAmt">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteAmt","{0:n2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note CGST%" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteCGSTPrc">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteCGSTPrc")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note CGST" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteCGST">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteCGST","{0:n2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note SGST%" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteSGSTPrc">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteSGSTPrc")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note SGST" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteSGST">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteSGST","{0:n2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note IGST%" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteIGSTPrc">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteIGSTPrc")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note IGST" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteIGST">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteIGST","{0:n2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credit Note Total Taxable Amt" ItemStyle-Width="150px" ItemStyle-CssClass="CreditNoteTotalTaxAmt">
                                <ItemTemplate>
                                    <%# Eval("CreditNoteTotalTaxAmt","{0:n2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>

                <asp:HiddenField ID="hfnCnclRsn" runat="server" />
                <asp:HiddenField ID="hfCnclRemarks" runat="server" />
                <asp:HiddenField ID="hfCreditNoteDt" runat="server" />
                <asp:HiddenField ID="hfCreditNoteRemarks" runat="server" />
                <asp:HiddenField ID="hfCreditNoteHSN" runat="server" />
                <asp:HiddenField ID="hfCreditNoteAmt" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteRemarks" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteHSN" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteAmt" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteCGSTPrc" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteCGST" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteSGSTPrc" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteSGST" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteIGSTPrc" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteIGST" runat="server" />
                <asp:HiddenField ID="hfnCreditNoteTotalTaxAmount" runat="server" />
                <asp:HiddenField ID="hfCreditPayment" runat="server" />
                <asp:HiddenField ID="hfCreditIsServ" runat="server" />
                <asp:HiddenField ID="hfCreditUOM" runat="server" />
                <asp:HiddenField ID="hfRadio" runat="server" />
                <asp:Button ID="tempBtn" runat="server" OnClick="btnCancelIRN_Click" Text="Submit" Style="display: none" />
                <asp:Button ID="tempCN" runat="server" OnClick="btnCreditremarks_Click" Text="Save" Style="display: none" />
                <asp:Button ID="tempRef" runat="server" Text="Temp" Style="display: none" />
                <asp:Button ID="tempCnDetails" runat="server" OnClick="tempCnDetails_Click" Text="CnDetails" Style="display: none" />
                <asp:Button ID="tempBindGrid" runat="server" OnClick="tempBindGrid_Click" Text="tempCnDetails" Style="display: none" />

                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->
        <!-- FOOTER BEGIN -->
        <div id="footerouter">
            <div class="footer">
                <div class="footerlogo">
                    <a href="http://www.diyostech.in" target="_blank">Powered by DIYOS </a>
                </div>
                <!--    <div class="footerlogo">&nbsp;</div> -->
                <div class="footercontent">
                    <asp:Label ID="lblcname" runat="server"></asp:Label>.
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>

</asp:Content>
