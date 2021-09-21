<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Reports/ReportMaster.master" AutoEventWireup="true" CodeBehind="ImportMonthlyAttendance.aspx.cs" Inherits="SRF.P.Module_Reports.ImportMonthlyAttendance" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="scripts\Calendar.js" type="text/javascript"></script>

    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #social div
        { display: block; }
        
        .HeaderStyle
        {
            text-align: Left;
        }
        .style1
        {
            width: 106px;
        }
        </style>


    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">
                Clients Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <div class="dashboard_full">
                    <div class="sidebox">
                     <div>
                            <h4 style="text-align: right">
                               <asp:LinkButton ID="lnkImportfromexcel" Text="Export Sample Excel" runat="server" 
                                    onclick="lnkImportfromexcel_Click"></asp:LinkButton> </h4>
                        </div>
                        <div class="boxhead">
                            <h2 style="text-align: center">
                                Employee ATTENDANCE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </h2>
                        </div>
                        
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="boxbody" style="padding: 5px 5px 5px 5px; height: auto">
                            <!--  Content to be add here> -->
                         
                               <div>
                                   
                                        <table>
                                            <tr>
                                                <td style="width: 60px">
                                                    Client ID<span style=" color:Red">*</span>
                                                </td>
                                                 <td>
                                                    <asp:DropDownList ID="ddlClientID" runat="server" Width="125px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlClientID_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="style1">
                                                    Client Name
                                                </td>
                                                
                                                 <td>
                                                      <asp:DropDownList ID="ddlCName" runat="server" AutoPostBack="True" 
                                                        Width="125px" OnSelectedIndexChanged="ddlCName_SelectedIndexChanged"></asp:DropDownList>
                                                  
                                                </td>
                                                <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                
                                                <td style="width: 60px" > Select File: </td>
                                                <td>
                                                <asp:FileUpload  ID="fileupload1" runat="server"/>
                                                </td>
                                                <td>
                                                 <asp:Button ID="btnImport" runat="server" Text="Import Data"  class=" btn save" OnClick="btnImport_Click"/> 
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td style="width: 60px">
                                                    Month</td>
                                                 <td>
                                                  <asp:DropDownList runat="server" ID="ddlMonth" Width="125px" AutoPostBack="True" 
                                                             onselectedindexchanged="ddlMonth_SelectedIndexChanged"></asp:DropDownList>
                                                           
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td class="style1">
                                                    OT in terms of</td>
                                                
                                                 <td>
                                                    <asp:DropDownList runat="server" ID="ddlOTType" Width="125px" >
                                                        <asp:ListItem>Days</asp:ListItem>
                                                        <asp:ListItem>Hours</asp:ListItem>
                                                    </asp:DropDownList>
                                                  
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                
                                                <td style="width: 60px" > 
                                             
                                                 <asp:Button ID="btnClear" runat="server" Text="Clear"  class=" btn save" 
                                                         onclick="btnClear_Click" /> 
                                              
                                                </td>
                                                <td>
                                             
                                                 <asp:Button ID="btnClearAll" runat="server" Text="Clear All"  class=" btn save" 
                                                         onclick="btnClearAll_Click" /> 
                                             
                                                </td>
                                                <td>
                                                 <asp:Button ID="btnExport" runat="server" Text="Unsaved"  class=" btn save" 
                                                         onclick="btnExport_Click" Visible="false"/> 
                                                </td>
                                                
                                            </tr>
                                           
                                            <tr>
                                                <td style="width: 60px">
                                                    Contract Id</td>
                                                 <td>
                                                  <asp:DropDownList runat="server" ID="ddlContractId" Width="125px" 
                                                            ></asp:DropDownList>
                                                           
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td class="style1">
                                                    Attendance Mode</td>
                                                
                                                 <td>
                                                      <asp:DropDownList runat="server" ID="ddlAttendanceMode" Width="125px">
                                                      <asp:ListItem>Full Attendance</asp:ListItem>
                                                      <asp:ListItem>Individual Attendance</asp:ListItem>
                                                      </asp:DropDownList>&nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                
                                                <td style="width: 60px" > 
                                             
                                                    &nbsp;</td>
                                                <td>
                                             
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                
                                &nbsp;</td>
                                                
                                            </tr>
                                           
                                </table>
                                   
                             </div>   
                               
        <br />
                            <div>
                                <asp:GridView ID="gvAttendancestatus" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                    GridLines="Both" HeaderStyle-CssClass="HeaderStyle" Height="140px" 
                                    onrowdatabound="gvAttendancestatus_RowDataBound" ShowFooter="True" 
                                    Style="margin-left: 50px" Width="90%">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesign" runat="server" Text="<%#Bind('Design') %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Duties">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotDuties" runat="server" Text="<%#Bind('Duties')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotDuties" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="OTs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotOts" runat="server" Text="<%#Bind('ot')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotOts" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                            
                                         <asp:TemplateField HeaderText="Total Duties">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%#Bind('TotalDuties')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>


                                         <%-- <asp:TemplateField HeaderText="WOs">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotWos" runat="server" Text="<%#Bind('Wo')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotWos" runat="server"></asp:Label>
                                            </FooterTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="NHS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotNHS" runat="server" Text="<%#Bind('NHS')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotNHS" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>--%>
                                        
                                      <%--  <asp:TemplateField HeaderText="Canteen Advance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotCanteenadv" runat="server" Text="<%#Bind('Canteenadv')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotCanteenadv" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Penalty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotPenalty" runat="server" Text="<%#Bind('penalty')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotPenalty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Incentives">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotIncentives" runat="server" Text="<%#Bind('Incentives')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotIncentives" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="N.A">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotNa" runat="server" Text="<%#Bind('Na')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotNa" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        
                                         <asp:TemplateField HeaderText="A.B">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotAb" runat="server" Text="<%#Bind('Ab')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGTotAb" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>--%>
                                        
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </div>
                             <br />
                                      <asp:Label ID="lblMessage" runat="server" style="color:Red"></asp:Label>
                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     
                                      <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" 
                                onclick="btnExportExcel_Click" Visible="false" class="btn Save"/>
                        <br />
              
                                
                               
                                <div style="overflow:scroll">
                                <asp:GridView ID="GridView1" runat="server"  Width="100%" Visible="false"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" 
                                        ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="Both" ShowFooter="true"
                                    HeaderStyle-CssClass="HeaderStyle" 
                                    OnRowDataBound="GridView1_RowDataBound">
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                            <ItemTemplate>
                                               <asp:Label ID="lblEmpid" runat="server" Text=" <%#Bind('EmpId')%>" style="text-align:left" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName"  Width="200px" style="text-align:left" Text=" <%#Bind('FullName')%>"></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesg" style="text-align:left"  Text=" <%#Bind('Designation')%>" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                Duties
                                            <br />
                                                OTs
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <%--************************************************************************************************************************************--%>
                                     <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtday1" runat="server" style="text-align:center" Width="20px" Text=" <%#Bind('day1')%>"></asp:Label>
                                    <br />
                                   <asp:Label  ID="txtday1ot" runat="server" style="text-align:center" Width="20px" Text=" <%#Bind('day1ot')%>"></asp:Label>
                                     </ItemTemplate>
                                      <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday1"></asp:Label>
                                        </FooterTemplate>       
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="2" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday2" style="text-align:center" Width="20px" Text=" <%#Bind('day2')%>" ></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday2ot" style="text-align:center" Width="20px" Text=" <%#Bind('day2ot')%>" ></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday2"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="3" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday3" style="text-align:center" Width="20px" Text=" <%#Bind('day3')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday3ot" style="text-align:center" Width="20px" Text=" <%#Bind('day3ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday3"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     
                                     <asp:TemplateField HeaderText="4" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday4" style="text-align:center" Width="20px" Text=" <%#Bind('day4')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday4ot" style="text-align:center" Width="20px" Text=" <%#Bind('day4ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday4"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="5" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday5" style="text-align:center" Width="20px" Text=" <%#Bind('day5')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday5ot" style="text-align:center" Width="20px" Text=" <%#Bind('day5ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday5"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="6" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday6" style="text-align:center" Width="20px" Text=" <%#Bind('day6')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday6ot" style="text-align:center" Width="20px" Text=" <%#Bind('day6ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday6"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="7" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday7" style="text-align:center" Width="20px" Text=" <%#Bind('day7')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday7ot" style="text-align:center" Width="20px" Text=" <%#Bind('day7ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday7"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="8" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday8" style="text-align:center" Width="20px" Text=" <%#Bind('day8')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday8ot" style="text-align:center" Width="20px" Text=" <%#Bind('day8ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday8"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="9" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday9" style="text-align:center" Width="20px" Text=" <%#Bind('day9')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday9ot" style="text-align:center" Width="20px" Text=" <%#Bind('day9ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday9"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="10" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday10" style="text-align:center" Width="20px" Text=" <%#Bind('day10')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday10ot" style="text-align:center" Width="20px" Text=" <%#Bind('day10ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday10"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="11" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday11" style="text-align:center" Width="20px" Text=" <%#Bind('day11')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday11ot" style="text-align:center" Width="20px" Text=" <%#Bind('day11ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday11"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="12" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday12" style="text-align:center" Width="20px" Text=" <%#Bind('day12')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday12ot" style="text-align:center" Width="20px" Text=" <%#Bind('day12ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday12"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     
                                     <asp:TemplateField HeaderText="13" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday13" style="text-align:center" Width="20px" Text=" <%#Bind('day13')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday13ot" style="text-align:center" Width="20px" Text=" <%#Bind('day13ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday13"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     
                                     <asp:TemplateField HeaderText="14" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday14" style="text-align:center" Width="20px" Text=" <%#Bind('day14')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday14ot" style="text-align:center" Width="20px" Text=" <%#Bind('day14ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday14"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="15" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday15" style="text-align:center" Width="20px" Text=" <%#Bind('day15')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday15ot" style="text-align:center" Width="20px" Text=" <%#Bind('day15ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday15"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="16" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday16" style="text-align:center" Width="20px" Text=" <%#Bind('day16')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday16ot" style="text-align:center" Width="20px" Text=" <%#Bind('day16ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday16"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="17" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday17" style="text-align:center" Width="20px" Text=" <%#Bind('day17')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday17ot" style="text-align:center" Width="20px" Text=" <%#Bind('day17ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday17"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="18" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday18" style="text-align:center" Width="20px" Text=" <%#Bind('day18')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday18ot" style="text-align:center" Width="20px" Text=" <%#Bind('day18ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday18"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="19" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday19" style="text-align:center" Width="20px" Text=" <%#Bind('day19')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday19ot" style="text-align:center" Width="20px" Text=" <%#Bind('day19ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday19"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="20" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday20" style="text-align:center" Width="20px" Text=" <%#Bind('day20')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday20ot" style="text-align:center" Width="20px" Text=" <%#Bind('day20ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday20"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <asp:TemplateField HeaderText="21" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday21" style="text-align:center" Width="20px" Text=" <%#Bind('day21')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday21ot" style="text-align:center" Width="20px" Text=" <%#Bind('day21ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday21"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="22" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday22" style="text-align:center" Width="20px" Text=" <%#Bind('day22')%>"></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday22ot" style="text-align:center" Width="20px" Text=" <%#Bind('day22ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday22"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="23" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday23" style="text-align:center" Width="20px" Text=" <%#Bind('day23')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday23ot" style="text-align:center" Width="20px" Text=" <%#Bind('day23ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday23"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="24" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday24" style="text-align:center" Width="20px" Text=" <%#Bind('day24')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday24ot" style="text-align:center" Width="20px" Text=" <%#Bind('day24ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday24"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                   
                                     <asp:TemplateField HeaderText="25" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday25" style="text-align:center" Width="20px" Text=" <%#Bind('day25')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday25ot" style="text-align:center" Width="20px" Text=" <%#Bind('day25ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday25"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="26" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday26" style="text-align:center" Width="20px" Text=" <%#Bind('day26')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday26ot" style="text-align:center" Width="20px" Text=" <%#Bind('day26ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday26"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="27" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday27" style="text-align:center" Width="20px" Text=" <%#Bind('day27')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday27ot" style="text-align:center" Width="20px" Text=" <%#Bind('day27ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday27"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                     
                                     
                                     <asp:TemplateField HeaderText="28" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday28" style="text-align:center" Width="20px" Text=" <%#Bind('day28')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday28ot" style="text-align:center" Width="20px" Text=" <%#Bind('day28ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday28"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                                                           
                                     <asp:TemplateField HeaderText="29" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday29" style="text-align:center" Width="20px" Text=" <%#Bind('day29')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday29ot" style="text-align:center" Width="20px" Text=" <%#Bind('day29ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday29"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                                                          
                                     <asp:TemplateField HeaderText="30" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday30" style="text-align:center" Width="20px" Text=" <%#Bind('day30')%>"></asp:Label>
                                     <br />
                                    <asp:Label runat="server" ID="txtday30ot" style="text-align:center" Width="20px" Text=" <%#Bind('day30ot')%>"></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday30"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                                                           
                                     <asp:TemplateField HeaderText="31" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" >
                                     <ItemTemplate>
                                     <asp:Label runat="server" ID="txtday31" style="text-align:center" Width="20px" Text=" <%#Bind('day31')%>" ></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="txtday31ot" style="text-align:center" Width="20px" Text=" <%#Bind('day31ot')%>" ></asp:Label>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                            <asp:Label runat="server" ID="lblTotalday31"></asp:Label>
                                        </FooterTemplate>   
                                     </asp:TemplateField>
                                      
                                     <%--************************************************************************************************************************************--%>
                                     
                                    
                                    <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtDuties" style="text-align:center" Width="5px" Text=" <%#Bind('noofduties')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            <asp:Label ID="lblTotalDuties" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                       <%-- <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtWos" style="text-align:center" Width="5px" Text=" <%#Bind('wo')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalWOs" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="NHS" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtNHS" style="text-align:center" Width="5px" Text=" <%#Bind('Wo')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalNHS" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>
                                       
                                       <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtOTs" style="text-align:center" Width="5px" Text=" <%#Bind('ot')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalOTs" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>

                                       <%-- <asp:TemplateField HeaderText="OTs1" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtOTs1" style="text-align:center" Width="5px" Text=" <%#Bind('ots1')%>"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                            <asp:Label ID="lblTotalOTs1" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>
                                       
                                        <asp:TemplateField HeaderText="Total"  FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label ID="lblTotalDts" runat="server"  Text=" <%#Bind('Total')%>"></asp:Label>
                                   <%--  <br />
                                      <asp:Label ID="lblTotalOts" runat="server"  Text=" <%#Bind('ot')%>"></asp:Label>--%>
                                     </ItemTemplate>
                                     <FooterTemplate>
                                      <asp:Label ID="lblGrandTotal" runat="server" ></asp:Label>
                                     </FooterTemplate>
                                     </asp:TemplateField>
                                       
                                       <%--<asp:TemplateField HeaderText="Canteen Adv." FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtCanAdv" style="text-align:center" Width="5px" Text=" <%#Bind('CanteenAdv')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalCanteenAdv" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Penalty" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtPenalty" style="text-align:center" Width="5px" Text=" <%#Bind('Penalty')%>"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            <asp:Label ID="lblTotalPenalty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Incentive" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtIncentivs" style="text-align:center" Width="5px" Text=" <%#Bind('Incentivs')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalIncentives" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                    
                                    
                                     <asp:TemplateField HeaderText="NA" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtNa" style="text-align:center" Width="5px" Text=" <%#Bind('na')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalNa" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="AB" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtAb" style="text-align:center" Width="5px" Text=" <%#Bind('ab')%>"></asp:Label>
                                            </ItemTemplate>
                                              <FooterTemplate>
                                            <asp:Label ID="lblTotalAb" runat="server"></asp:Label>
                                            </FooterTemplate>
                                       </asp:TemplateField>--%>
                                    
                                    </Columns>
                                   
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                </div>
                                
                                
                                <div>
                                 <asp:GridView ID="GridView2" runat="server" Height="140px" Width="90%" Style="margin-left: 50px"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="1px" GridLines="None" HeaderStyle-CssClass="HeaderStyle" Visible="false" >
                                   <Columns>
                                        <asp:TemplateField HeaderText=" Emp Id" >
                                            <ItemTemplate>
                                               <asp:Label ID="lblEmpid1" runat="server" Text=" <%#Bind('EmpId')%>" Width="60px"></asp:Label>
                                            </ItemTemplate>
                                        <ItemStyle   VerticalAlign="Middle"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation" >
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesg1" Text=" <%#Bind('Design')%>" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                     
                                       <asp:TemplateField HeaderText="Duties">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="lblDuties1" Text=" <%#Bind('NoOfDuties')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="OTs" >
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="lblOts1" Text=" <%#Bind('ot')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                           
                                       </asp:TemplateField>
                                      <%-- <asp:TemplateField HeaderText="Canteen Adv" >
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="lblCanteenAdv1" Text=" <%#Bind('CanteenAdv')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Penalty" >
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPenalty1" Text=" <%#Bind('Penalty')%>" Width="90px"></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Incentive" >
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblIncentivs1" Text=" <%#Bind('Incentivs')%>" Width="90px"></asp:Label>
                                                                              </ItemTemplate>
                                            <ItemStyle Width="60px"></ItemStyle>
                                       </asp:TemplateField>--%>
                                       
                                       <asp:TemplateField HeaderText="Remarks" >
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblRemarks" Text=" <%#Bind('Remark')%>" Width="90px"></asp:Label>
                                                                              </ItemTemplate>
                                            <ItemStyle Width="60px"></ItemStyle>
                                       </asp:TemplateField>
                                  
                                    
                                    </Columns>
                                </asp:GridView>
                                </div>
                                
                               <%--<asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                            <asp:Label ID="lblTotOts" runat="server" Text="<%#Bind('ot')%>"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblGTotOts" runat="server"></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>--%>
                                <div>
                                
                                <asp:GridView ID="SampleGrid" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" 
                                        ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle" >
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                    
                                     <asp:TemplateField HeaderText="Client Id"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid"  Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                    
                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                            <ItemTemplate>
                                               <asp:Label ID="lblsEmpid" runat="server" Text=" " style="text-align:center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle"/>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Designation"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                Duties
                                            <br />
                                                OTs
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                        <%--************************************************************************************************************************************--%>
                                     
                                     <%--Duties --%>
                                     <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday1" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday1ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday2" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday2ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday3" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                    <br />
                                      <asp:Label  ID="txtsday3ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday4" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday4ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday5" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday5ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday6" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday6ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday7" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday7ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday8" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday8ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday9" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday9ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday10" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday10ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday11" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday11ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday12" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday12ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday13" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday13ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday14" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday14ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday15" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                    <br />
                                      <asp:Label  ID="txtsday15ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="16" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday16" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                    <br />
                                      <asp:Label  ID="txtsday16ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="17" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday17" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday17ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="18" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday18" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday18ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="19" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday19" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday19ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="20" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday20" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday20ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="21" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday21" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday21ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="22" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday22" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday22ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="23" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday23" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                    <br />
                                      <asp:Label  ID="txtsday23ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="24" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday24" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday24ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="25" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday25" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday25ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="26" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday26" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday26ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="27" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday27" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                    <br />
                                      <asp:Label  ID="txtsday27ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="28" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday28" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday28ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="29" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday29" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday29ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday30" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday30ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                      <asp:TemplateField HeaderText="31" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsday31" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     <br />
                                      <asp:Label  ID="txtsday31ot" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                    
                                     <asp:TemplateField HeaderText="PF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsPf" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="ESI" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsEsi" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                     
                                      <asp:TemplateField HeaderText="PT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px" FooterStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                     <asp:Label  ID="txtsPt" runat="server" style="text-align:center" Width="20px" Text="0"></asp:Label>
                                     </ItemTemplate>
                                     </asp:TemplateField>
                                    
                                      
                                     <%--************************************************************************************************************************************--%>
                                     
                                    
                                    <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtsDuties" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtsOTs" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                             
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtsWos" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="NHS" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtsNHS" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       
                                      
                                       
                                       <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtsCanAdv" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                             
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Penalty" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsPenalty" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentivs" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                    
                                    
                                       
                                        <asp:TemplateField HeaderText="NA" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNa" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                       
                                       <asp:TemplateField HeaderText="AB" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsAb" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                    
                                    </Columns>
                                   
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                
                                <asp:GridView ID="grvSample2" runat="server"  Width="100%"
                                    AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" 
                                        ForeColor="#333333" BorderStyle="Solid"
                                    BorderColor="Black" BorderWidth="0" GridLines="None" Visible="false"
                                    HeaderStyle-CssClass="HeaderStyle" >
                                    <RowStyle BackColor="#EFF3FB" />
                                    <Columns>
                                    
                                     <asp:TemplateField HeaderText="Client Id"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsClientid"  Width="200px" Text=" "></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                    
                                        <asp:TemplateField HeaderText="Emp Id" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                            <ItemTemplate>
                                               <asp:Label ID="lblsEmpid" runat="server" Text=" " style="text-align:center" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle"/>
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Designation"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblsDesg" Text=" " Width="200px"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Duties" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtsDuties" style="text-align:center" Width="5px" Text=" <%#Bind('day1')%>"></asp:Label>
                                            </ItemTemplate>
                                          
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="OTs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtsOTs" style="text-align:center" Width="5px" Text=" <%#Bind('day2')%>"></asp:Label>
                                            </ItemTemplate>
                                             
                                       </asp:TemplateField>

                                       <%-- <asp:TemplateField HeaderText="OTs1" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtsOts1" style="text-align:center" Width="5px" Text="0"></asp:Label>
                                            </ItemTemplate>
                                             
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="WOs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                              <asp:Label runat="server" ID="txtsWos" style="text-align:center" Width="5px" Text=" <%#Bind('day3')%>"></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="NHs" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNhs" style="text-align:center" Width="5px" Text=""></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                      
                                       
                                       <asp:TemplateField HeaderText="Canteen Advance" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                           <asp:Label runat="server" ID="txtsCanAdv" style="text-align:center" Width="5px" Text=" <%#Bind('day5')%>"></asp:Label>
                                            </ItemTemplate>
                                             
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Penalty" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsPenalty" style="text-align:center" Width="5px" Text=" <%#Bind('day6')%>"></asp:Label>
                                            </ItemTemplate>
                                            
                                       </asp:TemplateField>
                                       
                                       
                                        <asp:TemplateField HeaderText="Incentives" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsIncentivs" style="text-align:center" Width="5px" Text="0"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                    
                                 
                                       
                                        <asp:TemplateField HeaderText="NA" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsNa" style="text-align:center" Width="5px" Text="0"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                       
                                       <asp:TemplateField HeaderText="AB" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="txtsAb" style="text-align:center" Width="5px" Text="0"></asp:Label>
                                            </ItemTemplate>
                                       </asp:TemplateField>--%>
                                    
                                    </Columns>
                                   
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                
                             
                                    <table width = "100%">
                                        <tr>
                                            <td width="25%"></td>
                                            <td width="25%">
                                                <asp:Label ID="lblTotalDuties" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            </td>                                        
                                            <td width="25%">
                                                <asp:Label ID="lblTotalOts" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td width="25%"></td>
                                        </tr>
                                        <tr>
                                        <td colspan="4">
                                        <asp:Label ID="lbltotaldesignationlist" runat="server" Text="" > </asp:Label>
                                        </td>
                                        
                                        </tr>
                                    </table>
                               
                                       <br />
                                <asp:Label ID="LblResult" runat="server" Text="" style="color:Red"></asp:Label>
                                       
                    
                      
                        </div>
                    </div>
                </div>
                 <!-- DASHBOARD CONTENT END -->
            </div>
        </div>
        <!-- CONTENT AREA END -->
   
    </div>

</asp:Content>
