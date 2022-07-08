<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Employees/EmployeeMaster.Master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="SRF.P.Module_Employees.Employees" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

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
                select: function (event, ui) { $("#<%=ddlEmpID.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLVendoridchange(event, ui); },

                minLength: 4
            });
        }

        $(document).ready(function () {
            setProperty();
        });

        function OnAutoCompleteDDLVendoridchange(event, ui) {
            $("#<%=ddlEmpID.ClientID %>").trigger('change');
        }


    </script>


    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div class="content-holder">
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_full">
                        <div align="center">
                            <asp:Label ID="lblMsg" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #CC3300;"></asp:Label>
                        </div>
                        <div align="center">
                            <asp:Label ID="lblSuc" runat="server" Style="border-color: #f0c36d; background-color: #f9edbe; width: auto; font-weight: bold; color: #000;"></asp:Label>
                        </div>
                        <table style="margin-top: 8px; margin-bottom: 8px" width="100%">
                            <tr>
                                <td style="font-weight: bold; width: 120px">Employee ID:
                                </td>
                                <td style="width: 190px">
                                    <asp:DropDownList ID="ddlEmpID" runat="server" CssClass="ddlautocomplete chosen-select" TabIndex="2" Style="width: 150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="font-weight: bold; width: 120px"></td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" class=" btn save" OnClick="btnSearch_Click" ToolTip="Search" />
                                </td>
                                <td align="right"><a href="AddEmployee.aspx" id="AddEmployeeLink" runat="server" class=" btn save">Add New Employee</a></td>
                                <td align="right"><a href="AndroidEmployee.aspx" id="A1" runat="server" class=" btn save">Android Employee</a></td>
                            </tr>
                        </table>
                        <div class="col-md-12">
                            <div class="panel panel-inverse">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Employee Details</h3>
                                </div>
                                <div class="panel-body">
                                    <asp:GridView ID="gvemployee" runat="server" CellPadding="2" ForeColor="Black"
                                        AutoGenerateColumns="False" Width="100%" BackColor="#f9f9f9" BorderColor="LightGray" PageSize="15"
                                        BorderWidth="1px" AllowPaging="True" OnRowDeleting="gvDetails_RowDeleting" OnPageIndexChanging="gvemployee_PageIndexChanging" OnRowDataBound="gvemployee_RowDataBound">
                                        <RowStyle Height="30px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Emp Id" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblempid" runat="server" Text=" <%#Bind('EmpId')%>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="40px"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Employee Name" ItemStyle-Width="110px" DataField="FullName">
                                                <ItemStyle Width="110px" HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Designation" ItemStyle-Width="60px" DataField="Designation">
                                                <ItemStyle Width="60px" HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Date of Joining" ItemStyle-Width="60px" DataField="EmpDtofJoining">
                                                <ItemStyle Width="60px"></ItemStyle>
                                            </asp:BoundField>

                                             <asp:BoundField HeaderText="Date of Absconding" ItemStyle-Width="60px" DataField="EmpDateofAbsconding">
                                                <ItemStyle Width="60px"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblempGen" Text="<%#Bind('empstatus')%>"></asp:Label>


                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lbtn_Select" ImageUrl="~/css/assets/view.png" runat="server"
                                                        ToolTip="View" OnClick="lbtn_Select_Click" />
                                                    <asp:ImageButton ID="lbtn_Edit" ImageUrl="~/css/assets/edit.png" runat="server" OnClick="lbtn_Edit_Click" ToolTip="Edit" Visible="false" />
                                                    <asp:ImageButton ID="linkdelete" CommandName="Delete" ImageUrl="~/css/assets/delete.png" runat="server"
                                                        OnClientClick='return confirm("Do you want to delete this record?");' ToolTip="Inactive" Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="40px"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="Tan" />
                                        <PagerStyle BackColor="LightBlue" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <HeaderStyle BackColor="White" Font-Bold="True" Height="30px" />
                                        <AlternatingRowStyle BackColor="White" Height="30px" />
                                    </asp:GridView>
                                    <asp:Label ID="lblresult" runat="server" Visible="false" Text="" Style="color: Red"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <!-- DASHBOARD CONTENT END -->
                <%-- </div> </div>--%>
                <!-- CONTENT AREA END -->
                <!-- FOOTER BEGIN -->
            </div>
        </div>
    </div>

</asp:Content>
