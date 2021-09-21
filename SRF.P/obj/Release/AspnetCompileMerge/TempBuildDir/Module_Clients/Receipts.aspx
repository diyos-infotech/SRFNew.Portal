<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="Receipts.aspx.cs" Inherits="SRF.P.Module_Clients.Receipts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-holder">
        <div class="content-holder">
            <!-- DASHBOARD CONTENT BEGIN -->
            <div class="contentarea" id="contentarea">
                <ul class="shortcuts-r" style="margin-left: 13px">

                    <li><a href="AddReceipts.aspx"><span class="shortcuts-icon iconsi-event"></span>
                        <span class="shortcuts-label">Add Receipt Details</span> </a></li>

                    <li><a href="ReceiptDetails.aspx"><span class="shortcuts-icon iconsi-event"></span>
                        <span class="shortcuts-label">Receipt Details</span> </a></li>
                </ul>
                <ul class="shortcuts-re" style="margin-left: 13px">

                    <li><a href="ReceiveReports.aspx"><span class="shortcuts-icon iconsi-event"></span>
                        <span class="shortcuts-label">Receipts Report</span> </a></li>

                    <li><a href="BillVsReceipts.aspx"><span class="shortcuts-icon iconsi-event"></span>
                        <span class="shortcuts-label">Bills Vs Receipts</span> </a></li>
                </ul>

                <div class="clear">
                </div>
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
        <!-- CONTENT AREA END -->
    </div>

</asp:Content>
