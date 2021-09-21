<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Area.aspx.cs" Inherits="SRF.P.Module_Settings.Area" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .fontstyle
        {
            font-family: Arial;
            font-size: 13px;
            font-weight: normal;
            font-variant: normal;
        }
    </style>

      <!-- CONTENT AREA BEGIN -->
    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">
                Settings Dashboard</h1>
            <!-- DASHBOARD CONTENT BEGIN -->
            <div id="Div1">
                <div class="content-holder">
                    <div id="breadcrumb">
                        <ul class="crumbs">
                            <li class="first"><a href="Settings.aspx" style="z-index: 9;"><span></span>Settings</a></li>
                            <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Area</a></li>
                        </ul>
                    </div>
                    <!-- DASHBOARD CONTENT BEGIN -->
                    <div class="contentarea" id="contentarea">
                        <div class="dashboard_center">
                            <div class="sidebox">
                                <div class="boxhead">
                                    <h2 style="text-align: center">
                                     Area
                                    </h2>
                                </div>
                                <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                    <div class="boxin">
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                          <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lbl_Area" runat="server" Text="Area:" class="fontstyle"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Area" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td> </td>
                                                    <td> </td>
                                                         <td>
                                                        <asp:Label ID="LblZone" runat="server" Text="Zone:" class="fontstyle"></asp:Label>
                                                         </td>
                                                 <td >
                                                    <asp:DropDownList ID="ddlZoneonDefault" runat="server" ValidationGroup="a" class="sdrop" TabIndex="6">
                                                    </asp:DropDownList>
                                                    
                                                </td>
                                                    <td> </td>
                                                    <td>
                                                        <asp:Button ID="Btn_Area" runat="server" Text="Add Area" class="btn save"
                                                            Width="120px" OnClick="Btn_Area_Click" OnClientClick='return confirm(" Are you sure you  want to add the Area?");' />
                                                    </td>
                                                </tr>
                                            </table>
                                             </div>
                                          
                                            <div class="rounded_corners">
                                                <asp:GridView ID="GvArea" runat="server" AutoGenerateColumns="false" Width="100%"
                                                    OnRowEditing="GvArea_RowEditing" OnRowCancelingEdit="GvArea_RowCancelingEdit"
                                                    OnPageIndexChanging="GvArea_PageIndexChanging" OnRowUpdating="GvArea_RowUpdating"
                                                    Style="text-align: center" CellPadding="5" CellSpacing="3" ForeColor="#333333"
                                                    GridLines="None">
                                                    <RowStyle BackColor="#EFF3FB" Height="30" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Area">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblArea" runat="server" Text="<%#Bind('AreaName') %>" MaxLength="50"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtArea" runat="server" Text="<%#Bind('AreaName') %>" MaxLength="50"
                                                                    Width="500px"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAreaId" Visible="false" runat="server" Text="<%#Bind('AreaId') %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Label ID="lblAreaId" Visible="false" runat="server" Text="<%#Bind('AreaId') %>"></asp:Label>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Zone" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                             <asp:DropDownList ID="ddlZoneid" runat="server" Enabled="false" Width="180px">
                                                </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Height="3px" />
                                                <ItemStyle Width="10px"></ItemStyle>
                                                  <EditItemTemplate>
                                                 <asp:DropDownList ID="ddlZoneid" runat="server" Enabled="false" Width="180px">
                                                </asp:DropDownList>
                                                      </EditItemTemplate>
                                            </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                                    OnClientClick='return confirm(" Are you sure you want to update the Area?");' style="color:Black"></asp:LinkButton>
                                                                <asp:LinkButton ID="linkcancel" runat="server" CommandName="cancel" Text="Cancel"
                                                                    OnClientClick='return confirm(" Are you sure you want to cancel this entry?");' style="color:Black">
                                                                </asp:LinkButton>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" Height="30" />
                                                    <EditRowStyle ForeColor="#000" BackColor="#C2D69B" />
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
                <!-- DASHBOARD CONTENT END -->
          
                <!-- CONTENT AREA END -->
            </div>

</asp:Content>
