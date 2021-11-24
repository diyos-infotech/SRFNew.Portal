<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Module_Reports/ReportMaster.master" CodeBehind="BillModification.aspx.cs" Inherits="SRF.P.Module_Reports.BillModification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">

    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link rel="dns-prefetch" href="https://fonts.gstatic.com" />

    <!-- Bootstrap CSS -->

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" />
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.0/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>



    <script type="text/javascript">

        function ShowPopup(Clientname, month) {
            debugger
            alert("Bill No already exists for client '" + Clientname + "' for month '" + month + "'");
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
                select: function (event, ui) {
                    $("#<%=ddlClient.ClientID %>").attr("data-clientId", ui.item.value);
                    minLength: 4
                }
            });
        }

        $(document).ready(function () {
            setProperty();
        });

      


        function onCalendarShown() {

            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);

            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }

        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }

        $(document).on("click", "[id*=btnDeleteBill]", function () {

            var Month = document.getElementById("<%=txtMonthval.ClientID %>").value;
            var Client = document.getElementById("<%=txtClientID.ClientID %>").value;
            var BillNo = document.getElementById("<%=txtBillNo.ClientID %>").value;

            //if (Client == "") {
            //    alert("Please select client");
            //    return false;
            //}

            //else if (Month == "") {
            //    alert("Please select month");
            //    return false;
            //}

            //else
                if (BillNo == "") {
                alert("Please select Bill no");
                return false;
            }

            else {
                $("#dialog").dialog({
                    title: "Bill Deletion",
                    width: 350,
                    buttons: {
                        "Submit": function () {
                            var Password = document.getElementById("<%=txtPassword.ClientID %>").value;
                            if (Password == '') {
                                alert("Please fill Password")
                                document.getElementById("<%=txtPassword.ClientID %>").focus();
                            }
                            else {

                                document.getElementById("<%=hfPassword.ClientID %>").value = Password;
                                $(this).dialog("close");
                                document.getElementById("<%=btnTemp.ClientID %>").click();
                                return true;
                            }

                        }
                    },
                    modal: true
                });
                return false;
            }

        });



        $(document).on("click", "[id*=btnDelete]", function () {

            var Month = document.getElementById("<%=txtMonth.ClientID %>").value;
            var Client = document.getElementById("<%=ddlClient.ClientID %>").selectedIndex;
            var BillNo = document.getElementById("<%=ddlBillNo.ClientID %>").selectedIndex;

            if (Client == 0) {
                alert("Please select client");
                return false;
            }

            else if (Month == "") {
                alert("Please select month");
                return false;
            }

            else if (BillNo == 0) {
                alert("Please select Bill no");
                return false;
            }

            else {
                $("#dialog").dialog({
                    title: "Bill Deletion",
                    width: 350,
                    buttons: {
                        "Submit": function () {
                            var Password = document.getElementById("<%=txtPassword.ClientID %>").value;
                            if (Password == '') {
                                alert("Please fill Password")
                                document.getElementById("<%=txtPassword.ClientID %>").focus();
                            }

                            else {

                                document.getElementById("<%=hfPassword.ClientID %>").value = Password;
                                $(this).dialog("close");
                                document.getElementById("<%=btnTemp.ClientID %>").click();
                                return true;
                            }

                        }
                    },
                    modal: true
                });
                return false;
            }
        });


        $(document).on("click", "[id*=btnUpdate]", function () {

            var NewBillNo = document.getElementById("<%=txtNewBillNoV.ClientID %>").value;
            var BillNo = document.getElementById("<%=txtOldBillNo.ClientID %>").value;

            if (BillNo == "") {
                alert("Please select Bill No");
                return false;
            }          

            else if (NewBillNo == "") {
                alert("Please select New Bill no");
                return false;
            }

            else {
                $("#dialog").dialog({
                    title: "Bill Modification",
                    width: 350,
                    buttons: {
                        "Submit": function () {
                            var Password = document.getElementById("<%=txtPassword.ClientID %>").value;
                            if (Password == '') {
                                alert("Please fill Password")
                                document.getElementById("<%=txtPassword.ClientID %>").focus();
                            }
                            else {

                                document.getElementById("<%=hfPassword.ClientID %>").value = Password;
                                $(this).dialog("close");
                                document.getElementById("<%=Button1.ClientID %>").click();
                                return true;
                            }

                        }
                    },
                    modal: true
                });
                return false;
            }

        });

        $(document).on("click", "[id*=btnClear]", function () {

            document.getElementById("<%=txtOldBillNo.ClientID %>").value = "";
            document.getElementById("<%=txtClientIDV.ClientID %>").value = "";
            document.getElementById("<%=txtClientNameV.ClientID %>").value = "";
            document.getElementById("<%=txtmonthv.ClientID %>").value = "";
            document.getElementById("<%=ddlBillTypeV.ClientID %>").selectedIndex = 0;
            document.getElementById("<%=txtNewBillSeq.ClientID %>").value = "";
            document.getElementById("<%=txtNewBillNoV.ClientID %>").value = "";

        });

        var selected_tab = 1;
        $(function () {
            var tabs = $("#tabs").tabs({
                select: function (e, i) {
                    selected_tab = i.index;
                }
            });
            selected_tab = $("[id$=selected_tab]").val() != "" ? parseInt($("[id$=selected_tab]").val()) : 0;
            tabs.tabs('select', selected_tab);
            $("form").submit(function () {
                $("[id$=selected_tab]").val(selected_tab);
            });
        });


    </script>

    <style type="text/css">
        .custom-combobox {
            position: relative;
            display: inline-block;
            width: 263px;
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

        .ui-widget.ui-widget-content {
            width: 260px;
        }
    </style>

    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
    </asp:ScriptManager>
    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">

            <!-- DASHBOARD CONTENT BEGIN -->

            <main class="my-form">

                <div id="tabs" style="width: 100%">
                    <ul>
                        <li><a href="#tabs-1">Bill Deletion</a></li>
                        <li><a href="#tabs-2">Bill Modification</a></li>
                    </ul>
                    <div id="tabs-1">

                        <asp:UpdatePanel ID="upTab1" runat="server">
                            <ContentTemplate>
                                <%----%>
                                <div>
                                    <asp:RadioButton ID="rdbBillNo" runat="server" GroupName="g1" Text="By Bill No" Checked="True" AutoPostBack="true" OnCheckedChanged="rdbBillNo_CheckedChanged" />
                                    <asp:RadioButton ID="rdClient" runat="server" GroupName="g1" Text="By Client" Style="margin-left: 10px;" AutoPostBack="true" OnCheckedChanged="rdbBillNo_CheckedChanged" />
                                </div>

                                <asp:Panel runat="server" ID="PnlbyBillNo" Visible="false">

                                    <div class="cotainer">
                                        <div class="row justify-content-center">
                                            <div class="col-md-8">

                                                <div class="form-group row">
                                                    <asp:Label ID="Label4" class="col-md-4 col-form-label text-md-right" runat="server">Bill No</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtBillNo" class="form-control" AutoPostBack="true" OnTextChanged="txtBillNo_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row" style="margin-top: 20px">
                                                    <label for="lblclientID" class="col-md-4 col-form-label text-md-right">Client ID</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtClientID" class="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row" style="margin-top: 20px">
                                                    <label for="lblclientName" class="col-md-4 col-form-label text-md-right">Client Name</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtClientName" class="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="Label2" class="col-md-4 col-form-label text-md-right" runat="server">Type</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlBillTypeVal" runat="server" class="form-control" Enabled="false">
                                                            <asp:ListItem Value="N">Normal</asp:ListItem>
                                                            <asp:ListItem Value="M">Manual</asp:ListItem>
                                                            <asp:ListItem Value="A">Arrears</asp:ListItem>
                                                            <asp:ListItem Value="I">Material</asp:ListItem>
                                                            <asp:ListItem Value="P">Petty Cash</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="Label3" class="col-md-4 col-form-label text-md-right" runat="server">Month</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtMonthval" class="form-control" Enabled="false"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblmonth" class="form-control" Visible="false"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="col-md-6 offset-md-4">
                                                    <asp:Button runat="server" ID="btnDeleteBill" class="btn btn-primary" Style="margin-bottom: 10px; padding: 10px; margin-left: 230px;" Text="Delete" OnClientClick='return confirm(" Are you sure, you want to delete the bill ?");'></asp:Button>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>

                                <asp:Panel runat="server" ID="PnlbyClient" Visible="false">

                                    <div class="cotainer">
                                        <div class="row justify-content-center">
                                            <div class="col-md-8">

                                                <div class="form-group row" style="margin-top: 20px">
                                                    <label for="lblclient" class="col-md-4 col-form-label text-md-right">Client</label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList runat="server" ID="ddlClient" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="txtMonth_TextChanged"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="Label1" class="col-md-4 col-form-label text-md-right" runat="server">Type</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlBillType" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="txtMonth_TextChanged">
                                                            <asp:ListItem Value="N">Normal</asp:ListItem>
                                                            <asp:ListItem Value="M">Manual</asp:ListItem>
                                                            <asp:ListItem Value="A">Arrears</asp:ListItem>
                                                            <asp:ListItem Value="I">Material</asp:ListItem>
                                                            <asp:ListItem Value="P">Petty Cash</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="lblMonthv" class="col-md-4 col-form-label text-md-right" runat="server">Month</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtMonth" class="form-control" AutoPostBack="true" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="Txt_Month_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                            Enabled="true" Format="MMM-yyyy" TargetControlID="txtMonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"></cc1:CalendarExtender>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="lblBillNo" class="col-md-4 col-form-label text-md-right" runat="server">Bill No</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList runat="server" ID="ddlBillNo" class="form-control">
                                                            <asp:ListItem>-Select-</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>


                                                <div class="col-md-6 offset-md-4">
                                                    <asp:Button runat="server" ID="btnDelete" class="btn btn-primary" Style="margin-bottom: 10px; padding: 10px; margin-left: 230px;" Text="Delete" OnClientClick='return confirm(" Are you sure, you want to delete the bill ?");'></asp:Button>
                                                </div>



                                            </div>
                                        </div>
                                    </div>

                                </asp:Panel>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-2">
                        <asp:UpdatePanel ID="UpTab2" runat="server">
                            <ContentTemplate>

                                  <div class="cotainer">
                                        <div class="row justify-content-center">
                                            <div class="col-md-8">

                                                <div class="form-group row">
                                                    <asp:Label ID="Label5" class="col-md-4 col-form-label text-md-right" runat="server">Enter Old Bill No</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtOldBillNo" class="form-control" AutoPostBack="true" OnTextChanged="txtOldBillNo_TextChanged"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row" style="margin-top: 20px">
                                                    <label for="Label6" class="col-md-4 col-form-label text-md-right">Client ID</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtClientIDV" class="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row" style="margin-top: 20px">
                                                    <label for="lblclientName" class="col-md-4 col-form-label text-md-right">Client Name</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtClientNameV" class="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="Label6" class="col-md-4 col-form-label text-md-right" runat="server">Type</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlBillTypeV" runat="server" class="form-control" Enabled="false">
                                                            <asp:ListItem Value="N">Normal</asp:ListItem>
                                                            <asp:ListItem Value="M">Manual</asp:ListItem>
                                                            <asp:ListItem Value="A">Arrears</asp:ListItem>
                                                            <asp:ListItem Value="I">Material</asp:ListItem>
                                                            <asp:ListItem Value="P">Petty Cash</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group row">
                                                    <asp:Label ID="Label7" class="col-md-4 col-form-label text-md-right" runat="server">Month</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtmonthv" class="form-control" Enabled="false"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblmontv" class="form-control" Visible="false"></asp:Label>
                                                    </div>
                                                </div>

                                                 <div class="form-group row">
                                                    <asp:Label ID="Label9" class="col-md-4 col-form-label text-md-right" runat="server">New Bill No</asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox runat="server" ID="txtNewBillSeq" class="form-control" Width="185px" Enabled="false" ></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtNewBillNoV" class="form-control" Width="90px" Style="float: right;margin-top: -38px;" MaxLength="5" ></asp:TextBox>
                                                         <cc1:FilteredTextBoxExtender ID="FTB" runat="server" Enabled="True" TargetControlID="txtNewBillNoV"
                                                            ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="col-md-6 offset-md-4">
                                                    <asp:Button runat="server" ID="btnUpdate" class="btn btn-primary" Style="margin-bottom: 10px; padding: 10px; margin-left: 132px;" Text="Update" OnClientClick='return confirm(" Are you sure, you want to modify the bill ?");'></asp:Button>
                                                    <asp:Button runat="server" ID="btnClear" class="btn btn-primary" Style="margin-bottom: 10px; padding: 10px; " Text="Clear" ></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div id="dialog" style="display: none; overflow-x: hidden; margin: 15px;">
                    <div class="row" style="margin-top: 5px; font-size: 12px;">
                        <label style="font-weight: 700">Enter Password</label>
                        <asp:TextBox runat="server" ID="txtPassword" class="form-control custm-input custm-input-md" TextMode="Password"></asp:TextBox>
                    </div>
                </div>

                <asp:HiddenField runat="server" ID="selected_tab" />
                <asp:HiddenField runat="server" ID="hfPassword" />

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnTemp" runat="server" OnClick="btnTemp_Click" Text="Submit" Style="display: none" />
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" Style="display: none" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </main>
        </div>
        <!-- DASHBOARD CONTENT END -->
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

                setProperty();
            });
        };
    </script>


</asp:Content>
