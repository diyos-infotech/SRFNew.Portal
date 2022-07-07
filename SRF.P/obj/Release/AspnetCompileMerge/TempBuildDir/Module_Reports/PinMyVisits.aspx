<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" 
    CodeBehind="PinMyVisits.aspx.cs" Inherits="SRF.P.Module_Reports.PinMyVisits" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="RightOne" ContentPlaceHolderID="ContentPlaceHolder3" runat="Server">
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <style type="text/css">
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



        .Grid th, .Grid td {
            border: 1px solid #66CCFF;
        }
    </style>

    <script type="text/javascript">

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
                select: function (event, ui) { $("#<%=ddlEmpid.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLVendoridchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlClientID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientidchange(event, ui); },
                select: function (event, ui) { $("#<%=ddlCName.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLClientnamechange(event, ui); },
                minLength: 4
            });
        }

        function ShowPopup() {
            $(function () {
                $("#dialog").dialog({
                    title: "Zoomed Image",
                    width: 350,
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');

                        }
                    },
                    modal: true
                });
            });
        };

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLVendoridchange(event, ui) {
            $("#<%=ddlEmpid.ClientID %>").trigger('change');

        }
        function OnAutoCompleteDDLClientidchange(event, ui) {
            $("#<%=ddlClientID.ClientID %>").trigger('change');

        }

        function OnAutoCompleteDDLClientnamechange(event, ui) {

            $("#<%=ddlCName.ClientID %>").trigger('change');
        }

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

       $(function () {


            $("[id*=GVpinmyvisit]").find("[id*=btnview]").click(function () {


                //Reference the GridView Row.
                var row = $(this).closest("tr");

                document.getElementById("<%=hfPitstopAttachmentId.ClientID %>").value = row.find("td").eq(11).find(":text").val();
                document.getElementById("<%=btnGetImage.ClientID %>").click();


                return false;
            });


        });

    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading"></h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                    <div class="dashboard_full">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">Pin My Visit&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </h2>
                            </div>

                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px; height: auto">
                                <!--  Content to be add here> -->
                                <div style="float: right">
                                    <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click">Export to Excel</asp:LinkButton>
                                </div>
                                <div>
                                    <table width="100%" style="margin: 10px auto">
                                        <tr>
                                            <td>Type</td>
                                            <td>
                                                <asp:DropDownList ID="ddltype" CssClass="sdrop" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged" runat="server">
                                                    <asp:ListItem>Day Wise</asp:ListItem>
                                                    <asp:ListItem>Month Wise</asp:ListItem>
                                                    <asp:ListItem>From To</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>Option</td>
                                            <td>
                                                <asp:DropDownList ID="ddloption" CssClass="sdrop" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddloption_SelectedIndexChanged" runat="server">
                                                    <asp:ListItem>Employee Wise</asp:ListItem>
                                                    <asp:ListItem>Client Wise</asp:ListItem>
                                                    <asp:ListItem>Activity</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblclientid" Visible="false" Text="Client ID"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlClientID" Visible="false" runat="server" CssClass="ddlautocomplete chosen-select" AutoPostBack="True" OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" Visible="false" ID="lblclientname" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCName" Visible="false" runat="server" placeholder="select" CssClass="ddlautocomplete chosen-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID/Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEmpid" runat="server" CssClass="ddlautocomplete chosen-select" Width="150px"></asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label ID="lblDay" runat="server" Text="Day"></asp:Label>
                                                <asp:Label ID="lblMonth" runat="server" Visible="false" Text="Month"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtMonth" runat="server" class="sinput" autocomplete="off"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="true" TargetControlID="txtMonth"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                    TargetControlID="txtMonth" ValidChars="/0123456789-">
                                                </cc1:FilteredTextBoxExtender>

                                            </td>
                                            <td>
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class=" btn save" OnClick="btnSubmit_Click" ToolTip="Submit" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label Visible="false" runat="server" ID="lblActivity" Text="Activity"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlFOID" runat="server" Visible="false" CssClass="ddlautocomplete chosen-select"
                                                    Width="120px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>


                                            <td>
                                                <asp:Label ID="lblfrom" runat="server" Visible="false" Text="From"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtfrom" runat="server" Visible="false" autocomplete="off" class="form-control" Width="200px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                    Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtfrom">
                                                </cc1:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblto" runat="server" Visible="false" Text="To"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtto" runat="server" Visible="false" autocomplete="off" class="form-control" Width="200px"></asp:TextBox>
                                                <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                    Enabled="true" Format="dd/MM/yyyy" TargetControlID="txtto">
                                                </cc1:CalendarExtender>
                                            </td>

                                        </tr>



                                    </table>
                                </div>
                                <div>
                                    <table>
                                        <tr>

                                            <td></td>
                                            <td>
                                                <asp:Button ID="Button1" runat="server" Text="PDF" class=" btn save" OnClick="Button1_Click" ToolTip="Submit" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>


                                <div style="width: 100%; margin-top: 30px">




                                    <asp:GridView ID="GVpinmyvisit" runat="server" CssClass="table table-striped table-bordered table-condensed table-hover" AutoGenerateColumns="false" OnRowDataBound="gvdata_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emp ID" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EmpFName" HeaderText="Name" />
                                            <asp:BoundField DataField="Clientid" HeaderText="Client ID" />
                                            <asp:TemplateField HeaderText="Created On" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdatedOn" runat="server" Text='<%#Eval("UpdatedOn", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EmpRemarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                            <asp:BoundField DataField="CheckinLat" HeaderText="Checkin Lat" />
                                            <asp:BoundField DataField="CheckinLng" HeaderText="Checkin Lng" />
                                            <asp:BoundField DataField="Address" HeaderText="Checkin Address" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnview" runat="server" Text="View"  CssClass="viewbutton" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblpitstopattid" runat="server" Text='<%#Bind("PitstopAttachmentId")%>' Style="display: none"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>


                                </div>

                                <asp:HiddenField ID="hfPitstopAttachmentId" runat="server" />
                                <asp:Button ID="btnGetImage" runat="server" OnClick="btnGetImage_Click" Style="display: none" />

                                <div id="dialog" style="display: none">

                                    <asp:Image ID="imgphoto" runat="server" Width="320" Height="300" />
                                </div>
                            </div>
                        </div>
                        <!-- DASHBOARD CONTENT END -->
                    </div>
                </div>
            <!-- CONTENT AREA END -->

        </div>
    </div>




</asp:Content>
