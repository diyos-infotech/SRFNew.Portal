<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.Master" AutoEventWireup="true" CodeBehind="ResourceReturnEmp.aspx.cs" Inherits="SRF.P.Module_Inventory.ResourceReturnEmp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/boostrap/css/bootstrap.css" rel="stylesheet" />

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <style type="text/css">
        .style1 {
            width: 135px;
        }
    </style>

    <script type="text/javascript">

        function dtval(d, e) {
            var pK = e ? e.which : window.event.keyCode;
            if (pK == 8) { d.value = substr(0, d.value.length - 1); return; }
            var dt = d.value;
            var da = dt.split('/');
            for (var a = 0; a < da.length; a++) { if (da[a] != +da[a]) da[a] = da[a].substr(0, da[a].length - 1); }
            if (da[0] > 31) { da[1] = da[0].substr(da[0].length - 1, 1); da[0] = '0' + da[0].substr(0, da[0].length - 1); }
            if (da[1] > 12) { da[2] = da[1].substr(da[1].length - 1, 1); da[1] = '0' + da[1].substr(0, da[1].length - 1); }
            if (da[2] > 9999) da[1] = da[2].substr(0, da[2].length - 1);
            dt = da.join('/');
            if (dt.length == 2 || dt.length == 5) dt += '/';
            d.value = dt;
        }

        function bindautofilldesgs() {
            $(".txtautofillempid").autocomplete({
                source: eval($("#hdempid").val()),
                minLength: 4
            });
        }



    </script>


    <style type="text/css">
        .style1 {
            width: 135px;
        }

        /*.FixedHeader {
            position: absolute;
            font-weight: bold;
        }*/

        .completionList {
            background: white;
            border: 1px solid #DDD;
            border-radius: 3px;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            min-width: 165px;
            height: 120px;
            overflow: auto;
        }

        .listItem {
            display: block;
            padding: 5px 5px;
            border-bottom: 1px solid #DDD;
        }

        .itemHighlighted {
            color: black;
            background-color: rgba(0, 0, 0, 0.1);
            text-decoration: none;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
            border-bottom: 1px solid #DDD;
            display: block;
            padding: 5px 5px;
        }
    </style>

       <div id="content-holder">
            <div class="content-holder">
                <%--<div id="breadcrumb">
                    <ul class="crumbs">
                        <li class="first"><a href="#" style="z-index: 9;"><span></span>Reports</a></li>
                        <li><a href="Reports.aspx" style="z-index: 8;">Employee Reports</a></li>
                        <li class="active"><a href="EmpBioData.aspx" style="z-index: 7;" class="active_bread">RESOURCE RETURN EMP</a></li>
                    </ul>
                </div>--%>
                <!-- DASHBOARD CONTENT BEGIN -->
                <div class="contentarea" id="contentarea">
                    <div class="dashboard_center">
                        <div class="sidebox">
                            <div class="boxhead">
                                <h2 style="text-align: center">RESOURCE RETURN EMP
                                </h2>
                            </div>
                            <div class="boxbody">
                                <div class="boxin" >
                                    <asp:ScriptManager runat="server" ID="ScriptEmployReports">
                                    </asp:ScriptManager>
                                    <div style="margin-left: 20px">
                                        <asp:HiddenField ID="hdempid" runat="server" />
                                        <div>
                                            <table style="width: 100%">

                                                <tr style="height: 32px">
                                                    <td>
                                                        <asp:Label runat="server" ID="lblloanno" Width="50px" Text="Loan No"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLoanno" runat="server" CssClass="form-control" AutoPostBack="true" Style="width: 200px" OnTextChanged="txtloanid_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblUniformId" Width="60px" Text="Uniform Id"></asp:Label>

                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtuniformid" runat="server" Enabled="false" Style="width: 200px" CssClass="form-control"></asp:TextBox>
                                                    </td>


                                                </tr>
                                                <tr style="height: 32px">

                                                    <td>
                                                        <asp:Label runat="server" ID="lblempid" Width="100px" Text="Emp ID"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmpid" runat="server" Enabled="false" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblempname" Width="50px" Text="Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="false" AutoPostBack="true" Style="width: 200px"></asp:TextBox>
                                                    </td>



                                                </tr>
                                                <tr style="height: 32px">
                                                    <td>
                                                        <asp:Label runat="server" ID="Label1" Width="100px" Text="Issue Mode"></asp:Label>
                                                    </td>
                                                    <td>

                                                        <asp:DropDownList ID="ddlFreepaid" runat="server" CssClass="form-control" Width="200px" Height="30px" Enabled="false">
                                                            <asp:ListItem Value="0">Chargeble</asp:ListItem>
                                                            <asp:ListItem Value="1">Free Issue</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblloandate" Width="100px" Text="Loan Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtloandate" runat="server" Enabled="false" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                                                    </td>


                                                </tr>
                                                <tr style="height: 32px">
                                                    <td>
                                                        <asp:Label runat="server" ID="lblnoofinstallments" Width="100px" Text=" No Of Installments"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtnoofinstallments" runat="server" Enabled="false" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblpaidAmount" Width="100px" Text="Paid Amount"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPaidAmnt" runat="server" Enabled="false" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                                                    </td>
                                                </tr>

                                            </table>
                                        </div>
                                    </div>
                                    <div style="text-align: right; padding: 5px 30px 20px 0px;">
                                        <asp:Label ID="lblresult" runat="server" Text="" Visible="true" Style="color: Red"></asp:Label>
                                        <asp:Button ID="btnSave" runat="server" ValidationGroup="a" Text="SAVE" ToolTip="SAVE"
                                            TabIndex="5" class="btn save" OnClientClick='return confirm("Are you sure you want to return the resources?");'
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btncancel" runat="server" ValidationGroup="b" TabIndex="6" Text="CANCEL"
                                            ToolTip="CANCEL" class=" btn save" OnClientClick='return confirm("Are you sure you want  to cancel this entry?");' />
                                    </div>
                                    <div style="margin-top: 20px; height: 300px; width: 100%">
                                        <asp:GridView ID="gvresources" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table table-striped table-bordered table-condensed table-hover"
                                            ForeColor="#333333" GridLines="None" CellPadding="5" CellSpacing="5" Style="text-align: center; margin: 0px auto" Height="50px" HeaderStyle-HorizontalAlign="Center">

                                            <Columns>

                                                <%-- 0 --%>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CbChecked" runat="server" Checked="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 1 --%>
                                                <asp:TemplateField HeaderText="Resource ID" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblresourceid" runat="server" Text='<%#Bind("ResourceID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 2 --%>
                                                <asp:TemplateField HeaderText="Resource Name" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" ItemStyle-Font-Italic="true" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblresourcename" runat="server" Text='<%#Bind("ItemName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Issued Qty" HeaderStyle-Width="70px" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtIssuedQty" runat="server" Width="70px"  Text='<%#Bind("IssuedQty")%>'></asp:Label>
                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 3 --%>
                                                <asp:TemplateField HeaderText="Return Qty" HeaderStyle-Width="80px" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQty" CssClass="form-control" runat="server" Width="90px"  Text='<%#Bind("Qty")%>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEQty" runat="server" Enabled="True" TargetControlID="txtQty"
                                                            ValidChars="0123456789">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- 4 --%>
                                                <asp:TemplateField HeaderText="Price" HeaderStyle-Width="80px" ItemStyle-Font-Italic="true" HeaderStyle-BackColor="#fcf8e3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtresourceprice" CssClass="form-control" runat="server" Width="90px" Enabled="false" Text='<%#Bind("Price") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <%-- 5 --%>
                                                <asp:TemplateField HeaderText="type" HeaderStyle-Width="80px" ItemStyle-Font-Italic="true" HeaderStyle-BackColor="#fcf8e3" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltype" CssClass="form-control" runat="server" Width="90px" Enabled="false" Text='<%#Bind("type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label ID="lblTotalamt" runat="server" Text=""></asp:Label>
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
