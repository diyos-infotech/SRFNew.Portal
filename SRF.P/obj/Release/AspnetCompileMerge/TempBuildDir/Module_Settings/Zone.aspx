<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Zone.aspx.cs" Inherits="SRF.P.Module_Settings.Zone" %>
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
                            <li class="active"><a href="#" style="z-index: 7;" class="active_bread">Zone</a></li>
                        </ul>
                    </div>
                    <!-- DASHBOARD CONTENT BEGIN -->
                    <div class="contentarea" id="contentarea">
                        <div class="dashboard_center">
                            <div class="sidebox">
                                <div class="boxhead">
                                    <h2 style="text-align: center">
                                        Zone
                                    </h2>
                                </div>
                                <div class="boxbody" style="padding: 5px 5px 5px 5px;">
                                    <div class="boxin">
                                        <div class="dashboard_firsthalf" style="width: 100%">
                                          <table width="50%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Lbl_Zone" runat="server" Text="Zone :" class="fontstyle"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Txt_Zone" runat="server" class="sinput"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="Btn_Zone" runat="server" Text="Add Zone" class="btn save"
                                                            Width="120px" OnClick="Btn_Zone_Click" OnClientClick='return confirm(" Are you sure you  want to add the Zone?");' />
                                                    </td>
                                                </tr>
                                            </table>
                                             </div>
                                          
                                            <div class="rounded_corners">
                                                <asp:GridView ID="GVZone" runat="server" AutoGenerateColumns="false" Width="100%"
                                                    OnRowEditing="GVZone_RowEditing" OnRowCancelingEdit="GVZone_RowCancelingEdit"
                                                    OnPageIndexChanging="GVZone_PageIndexChanging" OnRowUpdating="GVZone_RowUpdating"
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
                                                        <asp:TemplateField HeaderText="Zone">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZone" runat="server" Text="<%#Bind('ZoneName') %>" MaxLength="50"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtZoneName" runat="server" Text="<%#Bind('ZoneName') %>" MaxLength="50"
                                                                    Width="500px"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblZoneid" runat="server" Visible="false" Text="<%#Bind('ZoneId') %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Label ID="lblZoneid" Visible="false" runat="server" Text="<%#Bind('ZoneId') %>"></asp:Label>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="linkedit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="linkupdate" runat="server" CommandName="update" Text="Update"
                                                                    OnClientClick='return confirm(" Are you sure you want to update the Zone?");' style="color:Black"></asp:LinkButton>
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
