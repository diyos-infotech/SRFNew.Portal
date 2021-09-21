<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="EmpWageSlips.aspx.cs" Inherits="SRF.P.Module_Reports.EmpWageSlips" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


     <link href="css/global.css" rel="stylesheet" type="text/css" />

    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <script src="script/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/jscript.js"> </script>

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
                select: function (event, ui) { $("#ddlEmpID").attr("data-clientId", ui.item.value); OnAutoCompleteDDLEmpidchange(event, ui); },
                select: function (event, ui) { $("#ddlEmpName").attr("data-clientId", ui.item.value); OnAutoCompleteDDLEmpnamechange(event, ui); },
                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLEmpidchange(event, ui) {
            $('#ddlEmpID').trigger('change');

        }

        function OnAutoCompleteDDLEmpnamechange(event, ui) {

            $('#ddlEmpName').trigger('change');
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

    </script>


    <div id="content-holder">
            <div class="content-holder">
                <div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="EmpBioData.aspx" style="z-index: 7;" class="active_bread">EMPLOYEE WAGE SLIPS</a></li>
                    </ul>
                </div>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">WAGE SLIPS
                                </h2>
                            </div>
                            <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                <div class="boxin">
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <div>


                                            <div class="dashboard_firsthalf">

                                                <table cellpadding="5" cellspacing="5">

                                                    <tr style="height: 35px">
                                                        <td style="width: 100px">Emp ID<span style="color: Red">*</span>
                                                        </td>
                                                        <td>

                                                            <asp:DropDownList ID="ddlEmpID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlEmpID_OnSelectedIndexChanged">
                                                            </asp:DropDownList>

                                                        </td>

                                                    </tr>

                                                    <tr style="height: 35px">
                                                        <td style="width: 100px">
                                                            <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                                        </td>

                                                        <td>
                                                            <asp:TextBox ID="txtfrom" AutoComplete="off" runat="server" class="form-control" Width="200px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtfrom_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtfrom"></cc1:CalendarExtender>
                                                        </td>

                                                    </tr>
                                                     
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf">

                                                <table cellpadding="5" cellspacing="5">

                                                    <tr style="height: 36px">
                                                        <td style="width: 150px">Employee Name
                                                        </td>
                                                        <td>

                                                            <asp:DropDownList ID="ddlEmpName" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlEmpName_OnSelectedIndexChanged">
                                                            </asp:DropDownList>


                                                        </td>
                                                    </tr>

                                                    <tr style="height: 36px">

                                                        <td style="width: 150px">
                                                            <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtto" AutoComplete="off" runat="server" class="form-control" Width="200px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="txtto_CalendarExtender" runat="server" BehaviorID="calendar2"
                                                                Enabled="true" Format="MMM-yyyy" TargetControlID="txtto"></cc1:CalendarExtender>
                                                        </td>
                                                      
                                                    </tr>
                                                   
                                                </table>


                                            </div>
                                            
                                            <br />
                                            <br />
                                            <table width="43%" align="right">


                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btndownload" runat="server" Text="View Details" class="btn save" OnClick="btndownload_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPayslip" runat="server" Text="Download Pay Slips" class="btn save" OnClick="btnPaySlip_Click" /></td>
                                                    <%--  <td>
                                                        <asp:Button ID="btnEmployeeWageSlip" runat="server" Text="Pay Slips Mail" class="btn save" OnClick="btnEmpWageSlip_Click" /></td>
                                                  
                                                    --%>
                                                </tr>
                                            </table>

                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <table>
                                                <tr style="width: 300px">
                                                          <td >
                                                            <asp:Label runat="server" ID="lblresult" Style="color: Red" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                            </table>
                                            <div class="rounded_corners">
                                                <asp:GridView ID="gvattendancezero" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-condensed table-hover"
                                                    ForeColor="#333333" GridLines="None" CellPadding="4" CellSpacing="3" Style="text-align: center; margin: 0px auto" Height="50px" HeaderStyle-HorizontalAlign="Center"
                                                    OnRowDataBound="gvattendancezero_RowDataBound">

                                                    <Columns>

                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Id" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientId" runat="server" Text='<%#Bind("clientid") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Client Name " ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientName" runat="server" Text='<%#Bind("clientname") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Month " ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMonth" runat="server" Text='<%#Bind("month") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Emp Id" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Emp Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("EmpmName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Designation" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldesgn" runat="server" Text='<%#Bind("Desgn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Duties" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDuties" runat="server" Text='<%#Bind("NoOfDuties") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Salary Rate" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalaryRate" runat="server" Text='<%#Bind("salaryrate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Gross" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGross" runat="server" Text='<%#Bind("Gross") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PF" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPF" runat="server" Text='<%#Bind("PF") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ESI" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblESI" runat="server" Text='<%#Bind("ESI") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Deductions" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalDeductions" runat="server" Text='<%#Bind("Deductions") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="NetPay " ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNetPay" runat="server" Text='<%#Bind("ActualAmount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>




                                                    </Columns>

                                                </asp:GridView>
                                                <br />
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
                    <div id="footerouter">
                        <div class="footer">
                            <div class="footerlogo">
                                <a href="http://www.diyostech.Com" target="_blank">Powered by DIYOS©
                    <asp:Label ID="lblcname" runat="server" meta:resourcekey="lblcnameResource1"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <!-- FOOTER END -->
                    <!-- CONTENT AREA END -->
                </div>
    </form>

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


            });
        };
    </script>

</asp:Content>