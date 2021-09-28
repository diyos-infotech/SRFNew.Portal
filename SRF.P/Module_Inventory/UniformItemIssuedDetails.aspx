<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="UniformItemIssuedDetails.aspx.cs" Inherits="SRF.P.Module_Inventory.UniformItemIssuedDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="css/global.css" rel="stylesheet" type="text/css" />
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
    </style>
    <script type="text/javascript">

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

    </script>

    <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="Reports.aspx" style="z-index: 8;" class="active_bread">Uniform Item Issued Details</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">Uniform Item Issued Details
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                                <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                </asp:ScriptManager>
                                <div class="dashboard_firsthalf" style="width: 98%">

                                    <table width="100%" cellpadding="5" cellspacing="5">
                                        <tr>
                                            <td colspan="4">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Style="margin-left: 800px">Export to Excel</asp:LinkButton>

                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:Label runat="server" ID="lblfrmdate" Text="From Date" Width="60px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfromdate" runat="server" Text="" class="sinput" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged"></asp:TextBox>
                                                <cc1:calendarextender id="txtfrom_CalendarExtender" runat="server" enabled="true"
                                                    targetcontrolid="txtfromdate" format="dd/MM/yyyy">
                                                    </cc1:calendarextender>
                                                <cc1:filteredtextboxextender id="FTBEDOI" runat="server" enabled="True" targetcontrolid="txtfromdate"
                                                    validchars="/0123456789">
                                                    </cc1:filteredtextboxextender>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="lbltodate" Text="To Date" Width="60px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttodate" runat="server" Text="" class="sinput" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                                <cc1:calendarextender id="txtto_CalendarExtender" runat="server" enabled="true"
                                                    targetcontrolid="txttodate" format="dd/MM/yyyy">
                                                    </cc1:calendarextender>
                                                <cc1:filteredtextboxextender id="FTBEDOI2" runat="server" enabled="True" targetcontrolid="txttodate"
                                                    validchars="/0123456789">
                                                    </cc1:filteredtextboxextender>
                                            </td>

                                        </tr>


                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblempid" Text="Emp ID"></asp:Label></td>
                                            <td>
                                                <asp:DropDownList ID="ddlEmpID" runat="server" Width="190px" OnSelectedIndexChanged="ddlEmpID_SelectedIndexChanged"
                                                    AutoPostBack="True" CssClass="sdrop">
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <asp:Label runat="server" ID="lblempname" Text="Name" Width="50px"></asp:Label></td>

                                            <td>
                                                <asp:DropDownList ID="ddlEmpName" runat="server" Width="190px" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"
                                                    AutoPostBack="True" CssClass="sdrop">
                                                </asp:DropDownList>

                                            </td>

                                        </tr>


                                        <tr>
                                            <td colspan="4">
                                                <asp:Button ID="btnsubmit" runat="server" Text="Submit" Style="float: left; margin-left: 800px" OnClick="btnsearch_Click" />
                                            </td>
                                        </tr>



                                    </table>

                                    <div class="rounded_corners">
                                        <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            Height="50px" CellPadding="5" CellSpacing="5" ForeColor="#333333" GridLines="None">

                                            <Columns>
                                            </Columns>

                                        </asp:GridView>
                                    </div>

                                </div>


                                <div>
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

    <!-- DASHBOARD CONTENT END -->
</asp:Content>
