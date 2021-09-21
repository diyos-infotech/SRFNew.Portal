<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="QuarterlyandYearlyReport.aspx.cs" Inherits="SRF.P.Module_Reports.QuarterlyandYearlyReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <%--<style type="text/css">
        .style2 {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
    </style>--%>
    <script type="text/javascript">
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
                select: function (event, ui) { $("#ddlclientid").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#ddlcname").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLClientidchange(event, ui) {
            $('#ddlclientid').trigger('change');

        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {

            $('#ddlcname').trigger('change');
        }


    </script>
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
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
    </style>


         <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                        <li class="active"><a href="#" style="z-index: 7;" class="active_bread"> Quarterly & Yearly Report</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <asp:ScriptManager runat="server" ID="Scriptmanager1"></asp:ScriptManager>
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center"> Quarterly & Yearly Report
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <div class="dashboard_firsthalf" style="width: 100%">

                                        <div style="text-align:right">
                                            <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" >Export to Excel</asp:LinkButton>
                                        </div>
                                        <table width="40%" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td>Component</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlComponent" runat="server" class="sdrop" TabIndex="4">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem>LA</asp:ListItem>
                                                                <asp:ListItem>Gratuity</asp:ListItem>
                                                                <asp:ListItem>Bonus</asp:ListItem>
                                                            </asp:DropDownList>
                                                </td>
                                               <%-- <td></td>--%>
                                                
                                                <td>Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlType" runat="server" class="sdrop" TabIndex="4">
                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                <asp:ListItem Value="0" Text="Monthly"> </asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Quarterly"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Half Yearly"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="Yearly"></asp:ListItem>
                                                            </asp:DropDownList>
                                                </td>
                                                <td>Month</td>
                                                <td>
                                                    <asp:TextBox ID="txtmonth" runat="server" class="sinput" AutoComplete="off"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td>Client Id
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclientid" runat="server" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlclientid_SelectedIndexChanged" CssClass="ddlautocomplete chosen-select"  >
                                                    </asp:DropDownList>
                                                </td>
                                               
                                                <td style="padding-left: 40px">
                                                    <asp:Label ID="lblClientname" runat="server" Text="Client Name" ></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged" CssClass="ddlautocomplete chosen-select">
                                                    </asp:DropDownList>
                                                </td>
                                                
                                                <td style="padding-left: 60px"></td>
                                                
                                                <%--<td></td><td></td>
                                                <td></td>--%>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" TabIndex="4" OnClick="btn_Submit_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br /><br /><br />
                                    <div id="forExport" class="rounded_corners" style="overflow: scroll">
                                     <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="true" Width="100%"
                                            CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="Both" ShowFooter="true"
                                          OnRowDataBound="GVListEmployees_RowDataBound" >
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
           
        </div>


</asp:Content>
