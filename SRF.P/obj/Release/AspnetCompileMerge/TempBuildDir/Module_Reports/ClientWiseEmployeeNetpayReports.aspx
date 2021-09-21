<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ClientWiseEmployeeNetpayReports.aspx.cs" Inherits="SRF.P.Module_Reports.ClientWiseEmployeeNetpayReports" %>
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
    </style>

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
                    <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Net Pay</a></li>
                </ul>
            </div>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_center">
                    <div class="sidebox">
                        <div class="boxhead">
                            <h2 style="text-align: center">
                             Net Pay
                            </h2>
                        </div>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                            <div class="boxin">
                            
                    <asp:ScriptManager runat="server" ID="ScriptEmployReports"></asp:ScriptManager>
                    
                        <div class="dashboard_firsthalf" style="width: 100%">
                            
                            <div align="right">
                                                <asp:LinkButton ID="lbtn_Export" runat="server" OnClick="lbtn_Export_Click" Visible="false"><%--Export to Excel--%></asp:LinkButton>
                                                <asp:LinkButton ID="lbn_Export_PDF" runat="server" OnClick="lbtn_Export_PDF_Click">Export to PDF</asp:LinkButton>
                                            </div>
                            
                            
                          
                                <table width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            Month
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmonth" runat="server" Text="" class="sinput" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" BehaviorID="calendar1"
                                                        Enabled="true" Format="MMM-yyyy" TargetControlID="txtmonth" DefaultView="Months" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown">
                                                    </cc1:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btn_Submit" Text="Submit" class="btn save" 
                                                onclick="btn_Submit_Click" />
                                        </td>
                                         <td>
                                            <asp:Button runat="server" ID="btnExcel" Text="Excel" class="btn save" 
                                                onclick="btnExcel_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 30%">
                                            <asp:Label ID="LblResult" runat="server" Text="" Style="color: Red"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                  
                                 <asp:GridView ID="GVNetPayDetails" runat="server" AutoGenerateColumns="True"
                                        EmptyDataText="No Records Found" Width="960px" 
                                        CellPadding="4" CellSpacing="3" ForeColor="#333333" GridLines="None">

                                        <Columns>

                                        </Columns>

                                    </asp:GridView>
                         <div style="overflow: scroll" class="rounded_corners" >
                             

                             <asp:GridView ID="GVListEmployees" runat="server" AutoGenerateColumns="False" Width="100%"
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
                                        <asp:BoundField DataField="clientname" HeaderText="Name" />
                                        <asp:BoundField DataField="TotalGross" HeaderText="Gross" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="TotalOTamt" HeaderText="OT Amount" DataFormatString="{0:0.00}" />
                                        <asp:BoundField DataField="TotalDeductions" HeaderText="Total Deductions" DataFormatString="&nbsp; {0}" />
                                         <asp:BoundField DataField="TotalNetAmount" HeaderText="Net Amount" DataFormatString="{0:0.00}" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  Height="30" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="White" />
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
      
    </div>

</asp:Content>
