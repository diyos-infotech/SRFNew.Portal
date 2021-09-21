<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="BulkWageSlipsReport.aspx.cs" Inherits="SRF.P.Module_Reports.BulkWageSlipsReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            font-size: 10pt;
            font-weight: bold;
            color: #333333;
            background: #cccccc;
            padding: 5px 5px 2px 10px;
            border-bottom: 1px solid #999999;
            height: 26px;
        }
        .auto-style1 {
            width: 28px;
        }
    </style>
     <script type="text/javascript">
         function AssignExportHTML() {

             document.getElementById('hidGridView').value = htmlEscape(forExport.innerHTML);
         }
         function htmlEscape(str) {
             return String(str)
             .replace(/&/g, '&amp;')
             .replace(/"/g, '&quot;')
             .replace(/'/g, '&#39;')
             .replace(/</g, '&lt;')
             .replace(/>/g, '&gt;');
         }
    </script>
    
      <script type="text/javascript">

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
    
    </script>


      <div id="content-holder">
        <div class="content-holder">
              <div id="breadcrumb">
                <ul class="crumbs">
                    <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                    <li><a href="ClientReports.aspx" style="z-index: 8;">Client Reports</a></li>
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Bulk Wage Slips</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
              <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                             Bulk Wage Slips
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                        <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                        </asp:ScriptManager>
                        <div class="dashboard_firsthalf" style="width: 100%">
                           
                                <table width="60%">
                                    <tr style="width: 30%">
                                        <%--<td>
                                            Client ID</td>
                                          <td>  <asp:DropDownList runat="server" class="sdrop" ID="ddlClientId" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlClientId_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Client Name</td>
                                        <td>    <asp:DropDownList ID="ddlcname" runat="server" AutoPostBack="true" class="sdrop"
                                                OnSelectedIndexChanged="ddlcname_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>--%>
                                        <td class="auto-style1">
                                            Month</td>
                                           <td> <asp:TextBox ID="txtmonth" AutoComplete="off" runat="server" Text="" class="sinput"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtmonth" Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FTBEDOI" runat="server" Enabled="True" TargetControlID="txtmonth"
                                                ValidChars="/0123456789">
                                            </cc1:FilteredTextBoxExtender></td>
                                            <td>
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" OnClick="btnsearch_Click" />
                                           <%-- <asp:Button runat="server" ID="Button1" Text="Get data" class="btn save" OnClick="btnsearch2_Click" />--%>
                                                
                                        </td>
                                        <td>
                                                <asp:Button ID="Button3" runat="server" Text="Wage Slips New" class="btn save"
                                                    OnClick="btnEmpWageSlip_Click" />
                                            </td>
                                         <td>
                                               <div align="right">
                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="False" OnClientClick="AssignExportHTML()" ><%--Export to Excel--%></asp:LinkButton>
                            </div>
                            
                                        </td>
                                    </tr>
                                    <tr style="width: 100%">
                                        <td colspan="6">
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            
                             <div class="rounded_corners" >
                                <asp:GridView ID="GVListClients" runat="server" AutoGenerateColumns="False" Width="90%" style="margin:0px auto"
                                    CellSpacing="3" CellPadding="5" ForeColor="#333333" GridLines="None" >
                                                                    <RowStyle BackColor="#EFF3FB" Height="30"/>
                                    <Columns>
                                    <asp:TemplateField>
                                    <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server"  onclick = "checkAll(this);"/>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:CheckBox ID="chkindividual" runat="server" onclick = "Check_Click(this)"  />
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="Client Id">
                                    <ItemTemplate>
                                    <asp:Label ID="lblclientid" runat="server" Text='<%#Eval("clientid") %>'  />
                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Client Name">
                                    <ItemTemplate>
                                    <asp:Label ID="lblclientname" runat="server" Text='<%#Eval("clientname") %>'  />
                                    </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:BoundField DataField="clientname" HeaderText="Client Name" />--%>
                                        
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </div>
                            
                           
                               
                            
                            <div>
                                <table width="100%">
                                    <tr style="width: 100%; font-weight: bold">
                                        <td style="width: 60%">
                                            <asp:Label ID="lbltamttext" runat="server" Visible="false" Text="Total Amount"></asp:Label>
                                        </td>
                                        <td style="width: 40%">
                                            <asp:Label ID="lbltmtemppf" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lbltemprpf" runat="server" Text="" Style="margin-left: 30%"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
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