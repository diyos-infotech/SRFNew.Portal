<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Employees/EmployeeMaster.Master" AutoEventWireup="true" CodeBehind="ModifyEmployee.aspx.cs" Inherits="SRF.P.Module_Employees.ModifyEmployee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

       <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            function ShowProgress() {
                setTimeout(function () {
                    var modal = $('<div />');
                    modal.addClass("modal");
                    $('body').append(modal);
                    var loading = $(".loading");
                    loading.show();
                    var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                    var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                    loading.css({ top: top, left: left });
                }, 200);
            }

            var clkBtn = "";

            $('input[type="submit"]').click(function (evt) {
                clkBtn = evt.target.id;
            });




            $('form').live("submit", function (evt) {

                var btnID = clkBtn;
                if (btnID != 'btnadd' && (btnID != 'btnEduadd') && (btnID != 'btnPrevExpAdd')) {
                    ShowProgress();

                }


            });

        });
    </script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MODIFY EMPLOYEE</title>
    <link rel="shortcut icon" href="assets/Mushroom.ico" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/Calendar.css" rel="stylesheet" type="text/css" />

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

    </script>

    <link rel="stylesheet" href="script/jquery-ui.css" />

    <script type="text/javascript" src="script/jquery.min.js"></script>

    <script type="text/javascript" src="script/jquery-ui.js"></script>

    <script type="text/javascript">
        var currentTab = 0;
        $(function () {
            $("#tabs").tabs({
                select: function (e, i) {
                    currentTab = i.index;
                }
            });
        });
        $("#btnNext").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == (c - 1) ? currentTab : (currentTab + 1);
            tabs.tabs('select', currentTab);
            $("#btnPrevious").show();
            if (currentTab == (c - 1)) {
                $("#btnNext").hide();
            } else {
                $("#btnNext").show();
            }
        });
        $("#btnPrevious").live("click", function () {
            var tabs = $('#tabs').tabs();
            var c = $('#tabs').tabs("length");
            currentTab = currentTab == 0 ? currentTab : (currentTab - 1);
            tabs.tabs('select', currentTab);
            if (currentTab == 0) {
                $("#btnNext").show();
                $("#btnPrevious").hide();
            }
            if (currentTab < (c - 1)) {
                $("#btnNext").show();
            }
        });
    </script>


    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />

    <style type="text/css">
        .pstyle {
            width: 450px;
            margin: 0px auto;
        }

        .completionList {
            border: solid 1px Gray;
            margin: 0px;
            padding: 3px;
            height: 120px;
            overflow: auto;
            background-color: #FFFFFF;
        }

        .listItem {
            color: #191919;
        }

        .itemHighlighted {
            background-color: rgba(192,189,188,0.5);
            border: 1px solid Gray;
            border-radius: 2px;
            padding: 2px;
        }
    </style>

    <div id="content-holder">
        <div class="content-holder">

            <div class="col-md-12" style="margin-top: 8px; margin-bottom: 8px">
                <div align="right">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="#CC3300"></asp:Label>
                </div>
                <div class="panel panel-inverse">
                    <div class="panel-heading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <h3 class="panel-title">Edit Employee</h3>
                                </td>
                                <td align="right"><< <a href="Employees.aspx" style="color: #003366">Back</a>  </td>
                            </tr>
                        </table>


                    </div>
                    <div class="panel-body">
                        <!-- DASHBOARD CONTENT BEGIN -->



                        <asp:ScriptManager runat="server" ID="Scriptmanager2">
                        </asp:ScriptManager>
                        <div style="text-align: right">
                            <asp:Label ID="txtmodifyempid" runat="server"></asp:Label>
                        </div>
                        <div id="tabs" style="height: auto">
                            <ul>
                                <li><a href="#tabs-1">Personal Information</a></li>
                                <li><a href="#tabs-2">References</a></li>
                                <li><a href="#tabs-3">Bank/PF/ESI</a></li>
                                <%-- <li><a href="#tabs-4">Images</a></li>--%>
                                <li><a href="#tabs-4">Proofs</a></li>
                                <li><a href="#tabs-5">Qualification/Previous Experience</a></li>
                                <%-- <li><a href="#tabs-4">Images</a></li>--%>
                                <li><a href="#tabs-6">Police Record</a></li>

                            </ul>
                            <div id="tabs-1" style="height: 900px">

                                <asp:UpdatePanel runat="server" ID="uppersonal">
                                    <ContentTemplate>

                                        <asp:Panel ID="Pnlpersonal" Enabled="false" runat="server" GroupingText="<strong>&nbsp;Personal Information&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rdbGeneral" TabIndex="1" runat="server" GroupName="E1" Text=" General Enrollment" Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbStaff" TabIndex="2" runat="server" GroupName="E1" Text=" Staff" Enabled="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Emp ID
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpid" TabIndex="3" runat="server" MaxLength="6" ReadOnly="True"
                                                                class="sinput"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextEmpid" runat="server" enabled="True" targetcontrolid="txtEmpid"
                                                                validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>First Name<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpFName" TabIndex="5" runat="server" class="sinput" MaxLength="25"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Last Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmplname" TabIndex="7" runat="server" class="sinput" MaxLength="25"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Gender<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="Rdb_Male" TabIndex="9" runat="server" GroupName="g1" Text="Male" Checked="True" />
                                                            <asp:RadioButton ID="Rdb_Female" TabIndex="10" runat="server" GroupName="g1" Text="Female" />
                                                            <asp:RadioButton ID="rdbTransgender" TabIndex="11" runat="server" GroupName="g1" Text="Transgender" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Status
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbactive" TabIndex="16" runat="server" GroupName="g2" Text="Active" AutoPostBack="True" OnCheckedChanged="rdbactive_CheckedChanged" />
                                                            &nbsp;
                                                    <asp:RadioButton ID="rdbResigned" TabIndex="17" runat="server" GroupName="g2" Text="Resigned" OnCheckedChanged="rdbResigned_CheckedChanged" AutoPostBack="True" />
                                                            &nbsp;
                                                    <asp:RadioButton ID="rdbAbsconded" runat="server" GroupName="g2" Text="Absconding" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Qualification
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="18" ID="txtQualification" MaxLength="15" class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Date of Interview
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpDtofInterview" TabIndex="20" runat="server" class="sinput"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEDtofInterview" runat="server" enabled="true" targetcontrolid="txtEmpDtofInterview"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FTBEDOI" runat="server" enabled="True" targetcontrolid="txtEmpDtofInterview"
                                                                validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td></td>
                                                    </tr>




                                                    <tr>
                                                        <td>Phone No.<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhone" TabIndex="22" MaxLength="12" runat="server" class="sinput">
                                                            </asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender1" runat="server" enabled="True"
                                                                targetcontrolid="txtPhone" filtermode="ValidChars" filtertype="Numbers"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Mother Tongue
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmtongue" TabIndex="24" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Nationality
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtnationality" TabIndex="26" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Father Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherName" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Father Occupation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtfatheroccupation" runat="server" MaxLength="50" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>

                                                        <td>Spouse Name </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSpousName" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <tr>
                                                            <td>Branch </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlBranch" runat="server" class="sdrop" TabIndex="33">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Department </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddldepartment" runat="server" class="sdrop" TabIndex="35">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Site Posted to </td>
                                                            <td>
                                                                <asp:DropDownList ID="DdlPreferedUnit" runat="server" class="sdrop" TabIndex="37">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>PSARA Emp Code </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpsaraempcode" runat="server" CssClass="sinput" TabIndex="39"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>ID card issued date </td>
                                                            <td>
                                                                <asp:TextBox ID="TxtIDCardIssuedDt" runat="server" AutoPostBack="True" CssClass="sinput" OnTextChanged="TxtIDCardIssuedDt_TextChanged" TabIndex="41"></asp:TextBox>
                                                                <cc1:calendarextender id="CalendarExtender1" runat="server" enabled="true" format="dd/MM/yyyy" targetcontrolid="TxtIDCardIssuedDt"></cc1:calendarextender>
                                                                <cc1:filteredtextboxextender id="FtBIDCardIssuedDt" runat="server" enabled="True" targetcontrolid="TxtIDCardIssuedDt" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Date of Absconding </td>
                                                            <td>
                                                                <asp:TextBox ID="txtdtofabsconding" runat="server" AutoPostBack="True" CssClass="sinput" TabIndex="42"></asp:TextBox>
                                                                <cc1:calendarextender id="CalendarExtender5" runat="server" enabled="true" format="dd/MM/yyyy" targetcontrolid="txtdtofabsconding"></cc1:calendarextender>
                                                                <cc1:filteredtextboxextender id="FilteredTextBoxExtender12" runat="server" enabled="True" targetcontrolid="txtdtofabsconding" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                            </td>
                                                        </tr>
                                                        <tr style="visibility: hidden">
                                                            <td>Community/Classification </td>
                                                            <td style="padding-top: 10px">
                                                                <asp:RadioButton ID="rdsc" runat="server" GroupName="m1" Text="SC" />
                                                                <asp:RadioButton ID="rdst" runat="server" GroupName="m1" Text="ST" />
                                                                <asp:RadioButton ID="rdobc" runat="server" GroupName="m1" Text="OBC" />
                                                                <asp:RadioButton ID="rdur" runat="server" Checked="true" GroupName="m1" Text="Others" />
                                                            </td>
                                                        </tr>
                                                </table>


                                            </div>

                                            <div class="dashboard_secondhalf">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr style="display: none">
                                                        <td>Old Emp ID
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txoldempid" TabIndex="2" MaxLength="100" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Title </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTitle" runat="server" class="sdrop" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="ddlTitle_SelectedIndexChanged">
                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                <asp:ListItem>Mr</asp:ListItem>
                                                                <asp:ListItem>Miss</asp:ListItem>
                                                                <asp:ListItem>Mrs</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Middle Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpmiName" TabIndex="6" MaxLength="40" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Date of Birth
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpDtofBirth" TabIndex="8" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEEmpDtofBirth" runat="server" enabled="true" targetcontrolid="txtEmpDtofBirth"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FTBEDOB" runat="server" enabled="True" targetcontrolid="txtEmpDtofBirth"
                                                                validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Marital Status <span style="color: Red">*</span>
                                                        </td>
                                                        <td>

                                                            <asp:RadioButton ID="rdbsingle" TabIndex="12" runat="server" GroupName="m1" Text="Single" />
                                                            <asp:RadioButton ID="rdbmarried" TabIndex="13" runat="server" GroupName="m1" Text="Married" Style="margin-left: 17px" Checked="true" />

                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:RadioButton ID="rdbdivorcee" runat="server" GroupName="m1" Text="Divorcee" TabIndex="14" Style="margin-top: 10px" />
                                                            <asp:RadioButton ID="rdbWidower" runat="server" GroupName="m1" Text="Widower" TabIndex="15" Style="margin-top: 10px" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Designation<span style="color: Red">*</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" TabIndex="19" class="sdrop" ID="ddlDesignation">
                                                            </asp:DropDownList>

                                                            <%--<cc1:ComboBox ID="ddlDesignation" runat="server"></cc1:ComboBox>--%>


                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlDesignation"
                                                                SetFocusOnError="true" Display="Dynamic" InitialValue="0" ValidationGroup="a"
                                                                Text="*"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Date of Joining 
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpDtofJoining" TabIndex="21" runat="server" class="sinput" size="20"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEEmpDtofJoining" runat="server" enabled="true" targetcontrolid="txtEmpDtofJoining"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender2" runat="server" enabled="True"
                                                                targetcontrolid="txtEmpDtofJoining" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Date of Leaving
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDofleaving" TabIndex="23" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEDofleaving" runat="server" enabled="true" targetcontrolid="txtDofleaving"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender5" runat="server" enabled="True"
                                                                targetcontrolid="txtDofleaving" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Languages Known
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="25" ID="txtLangKnown" class="sinput" MaxLength="80">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Religion
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtreligion" TabIndex="27" runat="server" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Previous Employer
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPreEmp" TabIndex="32" runat="server" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Mother Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMotherName" class="sinput"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Division
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlDivision" runat="server" TabIndex="33"
                                                                class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Reporting Manager
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlReportingMgr" runat="server" TabIndex="35"
                                                                class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Gross Salary
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtGrossSalary" runat="server" class="sinput" TabIndex="37"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender11" runat="server" enabled="True" targetcontrolid="txtGrossSalary"
                                                                validchars="0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Email
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtemail" runat="server" class="sinput" TabIndex="39"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>ID card valid till
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtIdCardValid" runat="server" CssClass="sinput" TabIndex="41"></asp:TextBox>
                                                            <cc1:calendarextender id="CalendarExtender2" runat="server" enabled="true" targetcontrolid="TxtIdCardValid"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FTBIdCardValid" runat="server" enabled="True" targetcontrolid="TxtIdCardValid"
                                                                validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>



                                                </table>
                                            </div>
                                        </asp:Panel>

                                    </ContentTemplate>

                                </asp:UpdatePanel>

                                <asp:Panel ID="pnlimages" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Images&nbsp;</strong>" Style="margin-top: 10px">

                                    <div class="dashboard_firsthalf" style="padding: 10px">
                                        <table cellpadding="5" cellspacing="5" style="margin-top: 10px">
                                            <tr>
                                                <td>Employee Photo</td>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" Height="150" Width="150" /></td>

                                            </tr>
                                            <tr>
                                                <td>Modify Image</td>
                                                <td>
                                                    <asp:FileUpload ID="FileUploadImage" runat="server" TabIndex="42" /></td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <%--<tr>
                                           
                                           
                                            <td>
                                            <asp:FileUpload ID="FileUpload1" runat="server"  ViewStateMode="Enabled" /></td>
                                                 <%--<cc1:AsyncFileUpload OnClientUploadError="uploadError"
                                                OnClientUploadComplete="uploadComplete" runat="server"
                                                ID="FileUploadImage" Width="400px" UploaderStyle="Modern"
                                                CompleteBackColor = "White"
                                                UploadingBackColor="#CCFFFF"  
                                                 />
                                                </td>
                                        </tr>--%>
                                        </table>
                                    </div>





                                    <div class="dashboard_Secondhalf">
                                        <table cellpadding="5" cellspacing="5" style="margin-top: 10px">

                                            <tr>
                                                <td style="height: 105px"></td>
                                            </tr>

                                            <tr>

                                                <td>Emp Sign</td>
                                                <td>
                                                    <asp:Image ID="Image2" runat="server" Height="50" Width="200" /></td>
                                            </tr>
                                            <tr>
                                                <td>Modify Sign</td>
                                                <td>
                                                    <asp:FileUpload ID="FileUploadSign" runat="server" ViewStateMode="Enabled" /></td>
                                            </tr>

                                        </table>
                                    </div>

                                </asp:Panel>

                            </div>
                            <div id="tabs-2">
                                <asp:UpdatePanel runat="server" ID="UpRef">
                                    <ContentTemplate>
                                        <asp:Panel ID="PnlEmployeeInfo" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Employee Info&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">

                                                <table cellpadding="5" cellspacing="5">

                                                    <%-- <tr style="visibility:hidden">
                                                    <td>Birth Village
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBirthVillage" runat="server" class="sinput" TabIndex="1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="visibility:hidden">
                                                    <td>Birth State
                                                    </td>
                                                    <td>
                                                        <%--<asp:TextBox ID="txtBirthState" runat="server" class="sinput" TabIndex="3" ></asp:TextBox>
                                                        <asp:DropDownList ID="ddlbirthstate" runat="server" class="sdrop" TabIndex="3" AutoPostBack="true" OnSelectedIndexChanged="ddlbirthstate_SelectedIndexChanged" ></asp:DropDownList>
                                                        
                                                    </td>
                                                </tr>--%>
                                                    <tr>
                                                        <td>Ref Name &amp; Address1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr1" runat="server" TabIndex="5" class="sinput" TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Blood Group
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" TabIndex="7" class="sdrop">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Physical Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhyRem" runat="server" TabIndex="9" class="sinput" MaxLength="55"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks1
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark1" runat="server" TabIndex="10" class="sinput" MaxLength="80"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>

                                                        <td>Specially Abled</td>
                                                        <td>
                                                            <asp:CheckBox ID="ChkSpeciallyAbled" runat="server" Text=" Specially Abled" TabIndex="11" />
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Family Details
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtFamDetails" runat="server" TextMode="MultiLine"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>

                                            </div>



                                            <div class="dashboard_secondhalf" style="padding-top: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <%-- <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                    <%--<tr style="visibility:hidden">
                                                    <td>Birth Country
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBirthCountry" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="visibility:hidden">
                                                    <td>Birth District
                                                    </td>
                                                    <td>
                                                       <%-- <asp:TextBox ID="txtBirthDistrict" runat="server" class="sinput" Style="margin-left: 5px" TabIndex="2"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlBirthDistrict" runat="server" CssClass="sdrop" Style="margin-left: 5px" TabIndex="4" Enabled="false"></asp:DropDownList>

                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Ref Name &amp; Address2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtREfAddr2" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Remarks
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpRemarks" runat="server" TabIndex="8" TextMode="MultiLine"
                                                                class="sinput" MaxLength="50" Height="50px" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Identification Marks2
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtImark2" runat="server" TabIndex="10" class="sinput" MaxLength="80" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>

                                                        <td>Applicant Category</td>
                                                        <td>
                                                            <%--<asp:TextBox ID="TxtAppCategory" runat="server" class="sinput" ></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlAppCategory" runat="server" Style="margin-left: 5px" TabIndex="12" CssClass="sdrop" Enabled="false">
                                                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="LOCOMOTIVE" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="VISUAL" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="HEARING" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="OTHERS" Value="4"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblrefresult" Style="color: Red"></asp:Label>
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="pnlphysicalstandard2" Enabled="false" runat="server" GroupingText="<strong>&nbsp;Physical Standard &nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Height
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtheight" runat="server" TabIndex="13" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Weight
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtweight" runat="server" TabIndex="15" class="sinput" MaxLength="80" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Hair Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txthaircolour" runat="server" class="sinput" MaxLength="80" TabIndex="17" Style="margin-left: 70px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">


                                                    <tr>

                                                        <td>Chest UnExpand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheunexpan" runat="server" TabIndex="14" class="sinput" MaxLength="50" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Chest Expand
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtcheexpan" runat="server" TabIndex="16" class="sinput" MaxLength="25" Style="margin-left: 48px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Eye Colour
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEyeColour" runat="server" class="sinput" MaxLength="25" Style="margin-left: 48px" TabIndex="18"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>

                                        </asp:Panel>

                                        <asp:Panel ID="pnlphysicalstandard" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Address Details&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Present Address :</strong>
                                                        </td>
                                                        <td>

                                                            <asp:CheckBox ID="chkSame" runat="server" Text=" Copy" AutoPostBack="true" OnCheckedChanged="chkSame_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPresentAddress" runat="server" TabIndex="19" class="sinput" Height="55px"  TextMode="MultiLine" Style="margin-left: 12px"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprLandmark" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprvillage" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPostOffice" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprtaluka" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPoliceStation" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>

                                                            <%--<asp:TextBox ID="txtstate" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreStates" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlpreStates_SelectedIndexChanged1"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtcity" runat="server" TabIndex="15" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddlpreCity" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="23" Enabled="false"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprpin" runat="server" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprResidingDate" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            <cc1:calendarextender id="CalendarExtender3" runat="server" enabled="true" targetcontrolid="txtprResidingDate"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender9" runat="server" enabled="True"
                                                                targetcontrolid="txtprResidingDate" validchars="/0123456789"></cc1:filteredtextboxextender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtprPeriodofStay" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>

                                                        </td>
                                                    </tr>

                                                    <%--<td>
                                                                <asp:TextBox ID="txtprntaddress" runat="server" TabIndex="4" Width="160px"></asp:TextBox>
                                                            </td>--%>

                                                    <%--<tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPrdoor" runat="server" TabIndex="12" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet" runat="server" TabIndex="13" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea" runat="server" TabIndex="14" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <%--<tr>
                                                    <td>
                                                        District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdistrictt" runat="server" TabIndex="16" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Pin code
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtpin" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td class="style4">
                                                            <strong>Permanent Address :</strong>
                                                        </td>
                                                    </tr>

                                                    <%--<tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPermanentAddress" runat="server" TabIndex="20" class="sinput" Height="55px" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>--%>


                                                    <tr>
                                                        <td>Land Mark</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeLandmark" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Village/Town</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpevillage" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Post Office</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePostOffice" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Taluka/Hobli</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpeTaluka" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>Police Station</td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePoliceStattion" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td>State
                                                        </td>
                                                        <td>
                                                            <%--<asp:TextBox ID="txtstate8" runat="server" TabIndex="28" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="DdlStates" runat="server" class="sdrop" TabIndex="22" AutoPostBack="true" OnSelectedIndexChanged="DdlStates_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>District
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlcity" runat="server" class="sdrop" TabIndex="24" Enabled="false"></asp:DropDownList>
                                                            <%-- <asp:TextBox ID="txtcity5" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Pin code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtpePin" runat="server" TabIndex="27" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <%--<tr>
                                                    <td>
                                                        Door No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdoor1" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Street
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtstreet2" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Land mark
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtlmark3" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Area
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtarea4" runat="server" TabIndex="24" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>

                                                    <tr>
                                                        <td>Date Since Residing
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtResidingDate" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:calendarextender id="CalendarExtender4" runat="server" enabled="true" targetcontrolid="txtResidingDate"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender10" runat="server" enabled="True"
                                                                targetcontrolid="txtResidingDate" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Period of stay
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPeriodofStay" runat="server" class="sinput"></asp:TextBox>

                                                        </td>
                                                    </tr>

                                                    <%--<tr>
                                                    <td>
                                                        Perm. District
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPDist" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>--%>
                                                    <%--<tr>
                                                    <td>
                                                        Pin code
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtpin7" runat="server" TabIndex="27" class="sinput" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                    --%>
                                                    <tr>
                                                        <td>Phone(if any)
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtmobile9" runat="server" TabIndex="26" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>


                                            </div>
                                            <tr>
                                                <td style="font-weight: 900">Police Verification</td>
                                                <td>
                                                    <asp:RadioButton ID="rdbnotrequired" runat="server" GroupName="A1" Text="Not Required" />

                                                    <asp:RadioButton ID="rdbpreaddress" runat="server" GroupName="A1" Text="Present Address" AutoPostBack="true" OnCheckedChanged="rdbpreaddress_CheckedChanged" />

                                                    <asp:RadioButton ID="rdbperaddress" runat="server" GroupName="A1" Text="Permenant Address" AutoPostBack="true" OnCheckedChanged="rdbperaddress_CheckedChanged" />
                                                </td>

                                            </tr>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tabs-3">
                                <asp:UpdatePanel runat="server" ID="up3">
                                    <ContentTemplate>
                                        <asp:Panel ID="PnlBankDetails" Enabled="false" runat="server" GroupingText="<strong>&nbsp;Bank Details&nbsp;</strong>">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlbankname" runat="server" TabIndex="1" class="sdrop" MaxLength="100">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Branch Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtbranchname" runat="server" MaxLength="80" TabIndex="3" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Branch Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBranchCode" runat="server" TabIndex="5" class="sinput" MaxLength="50"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender3" runat="server" enabled="True"
                                                                targetcontrolid="txtBranchCode" filtermode="ValidChars" filtertype="Numbers"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank App No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAppNum" runat="server" TabIndex="7" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Insurance Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpInsNominee" runat="server" TabIndex="9" class="sinput" MaxLength="100"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                    </tr>
                                                    <tr>
                                                        <td>Nominee Date of Birth
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNomDoB" runat="server" TabIndex="11" class="sinput"></asp:TextBox>
                                                            <cc1:calendarextender id="CENomDoB" runat="server" enabled="true" targetcontrolid="txtNomDoB"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender6" runat="server" enabled="True"
                                                                targetcontrolid="txtNomDoB" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Insurance Cover
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInsCover" TabIndex="13" runat="server" class="sinput" MaxLength="10"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FTBEInsCover" runat="server" enabled="True" targetcontrolid="txtInsCover"
                                                                filtermode="ValidChars" filtertype="Numbers"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Aadhaar No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="15" ID="txtaadhaar" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr style="display: none">
                                                        <td>Cmp Short Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCmpShortName" class="sinput" MaxLength="50">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>Bank A/C No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankAccNum" TabIndex="2" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>IFSC Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIFSCcode" runat="server" MaxLength="20" TabIndex="4" class="sinput"> </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank Code No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankCodenum" TabIndex="6" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Region Code
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRegCode" TabIndex="8" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender4" runat="server" enabled="True"
                                                                targetcontrolid="txtRegCode" filtermode="ValidChars" filtertype="Numbers"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Bank Card Reference
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankCardRef" TabIndex="10" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmpNomRel" TabIndex="12" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>Ins Debt Amount
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtInsDeb" TabIndex="14" runat="server" class="sinput"></asp:TextBox>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender7" runat="server" enabled="True"
                                                                targetcontrolid="txtInsDeb" filtermode="ValidChars" filtertype="Numbers"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>UAN No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtUANNumber" TabIndex="16" runat="server" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>


                                                </table>
                                            </div>
                                            <%--  <div style="float: right; margin-top: 300px; margin-left: 250px">
                                                    <asp:Button ID="btn_BankSave" runat="server" Text="Save" class="btn save" OnClick="btn_BankSave_Click"
                                                        OnClientClick='return confirm("Are you sure you want to Add Details?");' />
                                                    <asp:Button ID="btn_BankCancel" runat="server" Text="Cancel" class="btn save" OnClick="btn_BankCancel_Click"
                                                        OnClientClick='return confirm("Are you sure you want to delete this entry?");' />
                                                    <div style="text-align: center float:right">
                                                        <asp:Label runat="server" ID="lblBankRes" Visible="false" Style="color: Red"></asp:Label>
                                                    </div>
                                                </div>
                                            --%>
                                        </asp:Panel>

                                        <asp:Panel ID="PnlPFDetails" runat="server" Enabled="false" GroupingText="<strong>&nbsp;PF Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>PF Deduct 
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" Checked="true" ID="ChkPFDed" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>EPF No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="17" ID="txtEmpPFNumber" class="sinput" MaxLength="15" Style="margin-left: 68px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="19" ID="txtPFNominee" class="sinput" MaxLength="80" Style="margin-left: 68px">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>PT Deduct
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" TabIndex="28" ID="ChkPTDed" Checked="true" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>PF Enroll Date
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="18" class="sinput" ID="txtPFEnrollDate" size="20" Style="margin-left: 2px"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEPFEnrollDate" runat="server" enabled="true" targetcontrolid="txtPFEnrollDate"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FilteredTextBoxExtender8" runat="server" enabled="True"
                                                                targetcontrolid="txtPFEnrollDate" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>PF Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPFNomineeRel" TabIndex="20" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </asp:Panel>


                                        <asp:Panel ID="PnlESIDetails" runat="server" Enabled="false" GroupingText="<strong>&nbsp;ESI Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>

                                                        <td>ESI Deduct </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkESIDed" Text="" Checked="true" /><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI No.
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="21" ID="txtESINum" class="sinput" MaxLength="15" Style="margin-left: 63px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="23" ID="txtESINominee" class="sinput" MaxLength="80" Style="margin-left: 63px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>ESI Disp Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="22" ID="txtESIDiSName" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="visibility: hidden">
                                                        <td>ESI Nominee Relation
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" TabIndex="24" ID="txtESINomRel" class="sinput" Style="margin-left: 2px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="PnlSalaryDetails" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Salary Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td style="height: 20px">Additional Amount
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtaddlamt" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 35px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table>
                                                    <tr>
                                                        <td style="height: 20px">Food Allowance
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtfoodallowance" runat="server" TabIndex="26" class="sinput" MaxLength="50" Style="margin-left: 50px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--                            <asp:UpdatePanel runat="server" ID="UpImages">
                                            <ContentTemplate>
                                                <div class="dashboard_firsthalf">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td class="style3">
                                                                Employee Photo
                                                            </td>
                                                            <td>
                                                                <img id="imglogo" runat="server" height="100" width="100" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btn_EmpPhoto" Text="Select Photo" class="btn select"
                                                                    OnClick="btn_EmpPhoto_Click" />
                                                            </td>
                                                            <td>
                                                                <%--<cc1:AsyncFileUpload ID="FcPicture1" runat="server" Visible="false" OnClientUploadComplete="uploadComplete"
                                                                    OnClientUploadStarted="uploadStart" OnClientUploadError="uploadError" />--%>
                            <%-- <asp:FileUpload ID="FcPicture1" Visible="false" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                Left Hand Image
                                                            </td>
                                                            <td>
                                                                <img id="imgLeft" runat="server" height="100" width="100" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btn_LeftHandImage" Text="Select Photo" class="btn select"
                                                                    OnClick="btn_LeftHandImage_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:FileUpload ID="LeftFcPicture" Visible="false" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                Signature
                                                            </td>
                                                            <td>
                                                                <img id="imgSignature" runat="server" width="100" height="100" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btn_Signature" Text="Select Photo" class="btn select"
                                                                    OnClick="btn_Signature_Click1" />
                                                            </td>
                                                            <td>
                                                                <asp:FileUpload ID="SignaturePic" Visible="false" runat="server" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                Right Hand Image
                                                            </td>
                                                            <td>
                                                                <img id="imgRhImage" runat="server" width="100" height="100" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btn_RightHandImage" Text="Select Photo" class="btn select"
                                                                    OnClick="btn_RightHandImage_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:FileUpload ID="RightFcPicture" Visible="false" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                ESI Photo
                                                            </td>
                                                            <td>
                                                                <img id="imgESIPic" runat="server" width="100" height="100" />
                                                            </td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btn_ESIPhoto" Text="Select Photo" class="btn select"
                                                                    OnClick="btn_ESIPhoto_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:FileUpload ID="ESIPhotoPic" Visible="false" runat="server" Width="100" Height="100" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label runat="server" ID="lblimagesRes" Visible="False" Style="color: Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="dashboard_secondhalf">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td class="style3">
                                                                Certf Details
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmpCertfdeb" runat="server" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                Certf Submit
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="ChkCertfSub" Text="Submitted" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="style3">
                                                                Surity Bond
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkSBond" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Security Deposit
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" class="sinput" ID="txtSecurityDeposit"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                    TargetControlID="txtSecurityDeposit" ValidChars="0123456789">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td align="right">
                                                                <asp:Button ID="Btn_Save_Images" runat="server" Text="Save" class="btn save" OnClick="Btn_Save_Images_Click"
                                                                    OnClientClick='return confirm("Are you sure you want to modify employee details?");' />
                                                                <asp:Button ID="Btn_Cancel_Images" runat="server" Text="Cancel" class="btn save"
                                                                    OnClick="Btn_Cancel_Images_Click" OnClientClick='return confirm("Are you sure you want to cancel the modify employee details?");' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </div>
                                            </ContentTemplate>
                                           
                                          <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="FcPicture1" EventName="btn_EmpPhoto_Click" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_EmpPhoto" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_LeftHandImage" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_RightHandImage" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_ESIPhoto" />
                                            </Triggers>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_Signature" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>--%>
                            <div id="tabs-4">
                                <asp:UpdatePanel runat="server" ID="Updatepanel5" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <asp:Panel ID="PnlProofsSubmitted" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Proofs Submitted&nbsp;</strong>" Style="margin-top: 10px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkAadharCard" runat="server" Text="  Aadhar Card" TabIndex="1" OnCheckedChanged="ChkAadharCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAadharCard" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAadharName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkdrivingLicense" runat="server" Text=" Driving License" TabIndex="5" OnCheckedChanged="ChkdrivingLicense_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDrivingLicense" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtDrivingLicenseName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:CheckBox ID="ChkVoterID" runat="server" Text=" Voter ID" TabIndex="9" OnCheckedChanged="ChkVoterID_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtVoterID" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="10"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtVoterName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkRationCard" runat="server" Text=" Ration Card" TabIndex="13" OnCheckedChanged="ChkRationCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtRationCard" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px" TabIndex="14"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtRationCardName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <asp:CheckBox ID="Chkother" runat="server" Text=" if Others, Specify" TabIndex="15" OnCheckedChanged="Chkother_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtOther" runat="server" class="sinput" Enabled="false" TabIndex="16" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtOtherName" runat="server" class="sinput" Enabled="false" Style="margin-left: 5px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkPanCard" runat="server" Text=" Pan Card" TabIndex="3" OnCheckedChanged="ChkPanCard_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>


                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPanCard" runat="server" class="sinput" Enabled="false" TabIndex="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPanCardName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkBankPassbook" runat="server" Text=" Bank PassBook" TabIndex="7" OnCheckedChanged="ChkBankPassbook_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankPassbook" runat="server" class="sinput" Enabled="false" TabIndex="8"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtBankPassBookName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkElectricityBill" runat="server" Text=" Electricity Bill" TabIndex="11" OnCheckedChanged="ChkElectricityBill_CheckedChanged" AutoPostBack="true" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtElectricityBill" runat="server" class="sinput" Enabled="false" TabIndex="12"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtElecBillname" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="ChkESICCard" runat="server" Text=" ESIC Card" TabIndex="15" AutoPostBack="true" OnCheckedChanged="ChkESICCard_CheckedChanged" Style="font-weight: bold" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">Number</td>
                                                        <td>
                                                            <asp:TextBox ID="txtESICCardNo" runat="server" class="sinput" Enabled="false" TabIndex="16"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left: 18px">KYC Name</td>
                                                        <td>
                                                            <asp:TextBox ID="txtESICName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel ID="PnlExService" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Ex-Service&nbsp;</strong>" Style="margin-top: 15px">

                                            <div class="dashboard_firsthalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">

                                                    <tr>
                                                        <td>EMP Ex-service
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ChkExService" Text="" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="height: 20px">Service No.
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtServiceNum" runat="server" TabIndex="17" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Date of Enrollment
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtDOfEnroll" runat="server" TabIndex="19" class="sinput" size="20"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEDOfEnroll" runat="server" enabled="true" targetcontrolid="txtDOfEnroll"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FTBEDOfEnroll" runat="server" enabled="True" targetcontrolid="txtDOfEnroll"
                                                                validchars="/0123456789"></cc1:filteredtextboxextender>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Crops
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtCrops" runat="server" TabIndex="21" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Medical Category
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtMCategory" runat="server" TabIndex="23" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Conduct
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtConduct" runat="server" TabIndex="25" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf" style="padding: 10px">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Rank
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtRank" runat="server" TabIndex="18" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Date of Discharge
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtDofDischarge" runat="server" TabIndex="20" class="sinput" size="20"
                                                                MaxLength="10"></asp:TextBox>
                                                            <cc1:calendarextender id="CEDofDischarge" runat="server" enabled="true" targetcontrolid="txtDofDischarge"
                                                                format="dd/MM/yyyy"></cc1:calendarextender>
                                                            <cc1:filteredtextboxextender id="FTBEDofDischarge" runat="server" enabled="True"
                                                                targetcontrolid="txtDofDischarge" validchars="/0123456789"></cc1:filteredtextboxextender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Trade
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtTrade" runat="server" TabIndex="22" class="sinput" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">Reason of Discharge
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="TxtROfDischarge" runat="server" TabIndex="24" TextMode="MultiLine" MaxLength="50"
                                                                class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblExRes" Visible="false" Style="color: Red"></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>

                                            </div>
                                        </asp:Panel>

                                        <style type="text/css">
                                            .HeaderStyle {
                                                font-weight: bold;
                                            }
                                        </style>
                                        <asp:Panel ID="pnlfamilydetails" runat="server" GroupingText="<strong>&nbsp;Family Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px">
                                                <asp:GridView ID="gvFamilyDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5" OnRowCreated="grvMergeHeader_RowCreated">
                                                    <HeaderStyle Wrap="True" />


                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEmpName" Width="" runat="server" Text=""></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="150" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Date Of Birth" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRelDtofBirth" runat="server"
                                                                    MaxLength="10" placeholder="DD/MM/YYYY"></asp:TextBox>

                                                            </ItemTemplate>
                                                            <ControlStyle Width="90" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Age" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="40" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Relationship" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlRelation" runat="server" Width="99%">
                                                                    <asp:ListItem runat="server" Value="--Select--" />
                                                                    <asp:ListItem runat="server" Value="Father" />
                                                                    <asp:ListItem runat="server" Value="Wife" />
                                                                    <asp:ListItem runat="server" Value="Husband" />
                                                                    <asp:ListItem runat="server" Value="Son" />
                                                                    <asp:ListItem runat="server" Value="Daughter" />
                                                                    <asp:ListItem runat="server" Value="Brother" />
                                                                    <asp:ListItem runat="server" Value="Sister" />
                                                                    <asp:ListItem runat="server" Value="Mother" />
                                                                    <asp:ListItem runat="server" Value="Uncle" />
                                                                    <asp:ListItem runat="server" Value="Aunty" />
                                                                    <asp:ListItem runat="server" Value="Other" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Occupation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtReloccupation" runat="server" Text=""></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="AAdhar No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAAdharNo" runat="server" Text=""></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="110" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="PF" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkPFNominee" runat="server" />
                                                            </ItemTemplate>
                                                            <ControlStyle Width="40" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="ESI" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkESINominee" runat="server" />
                                                            </ItemTemplate>
                                                            <ControlStyle Width="40" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Whether residing with him/her ?" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlresidence" runat="server">
                                                                    <asp:ListItem runat="server" Value="--Select--" />
                                                                    <asp:ListItem runat="server" Value="Yes" />
                                                                    <asp:ListItem runat="server" Value="No" />
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="90" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="If 'No' Place of Residence" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtplace" runat="server" Text=""></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ControlStyle Width="80" />
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnadd" runat="server" Text="Add" OnClick="btnadd_Click" Style="margin-top: 10px;" />



                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="tabs-5">
                                <asp:UpdatePanel runat="server" ID="Updatepanel3">
                                    <ContentTemplate>
                                        <%-- <div class="dashboard_firsthalf">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td style="height: 20px" class="style4">
                                                            <strong>SSC :</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Name & Address of School/Clg
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtschool" runat="server" TabIndex="1" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Board/University
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtbrd" runat="server" TabIndex="2" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Year of Study
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtyear" runat="server" TabIndex="3" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Whether Pass/Failed
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpsfi" runat="server" TabIndex="4" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Percentage of Marks
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpmarks" runat="server" TabIndex="5" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px" class="style4">
                                                            <strong>INTERMEDIATE :</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Name & Address of School/Clg
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtimschool" runat="server" TabIndex="11" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Board/University
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtimbrd" runat="server" TabIndex="12" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Year of Study
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtimyear" runat="server" TabIndex="13" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Whether Pass/Failed
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtimpsfi" runat="server" TabIndex="14" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Percentage of Marks
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtimpmarks" runat="server" TabIndex="15" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="dashboard_secondhalf">
                                                <table cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td style="height: 20px" class="style4">
                                                            <strong>DEGREE :</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Name & Address of School/Clg
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtdgschool" runat="server" TabIndex="6" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Board/University
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtdgbrd" runat="server" TabIndex="7" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Year of Study
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtdgyear" runat="server" TabIndex="8" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Whether Pass/Failed
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtdgpsfi" runat="server" TabIndex="9" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Percentage of Marks
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtdgpmarks" runat="server" TabIndex="10" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px" class="style4">
                                                            <strong>PG :</strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Name & Address of School/Clg
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpgschool" runat="server" TabIndex="16" TextMode="MultiLine" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Board/University
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpgbrd" runat="server" TabIndex="17" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Year of Study
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpgyear" runat="server" TabIndex="18" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Whether Pass/Failed
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpgpsfi" runat="server" TabIndex="19" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px">
                                                            Percentage of Marks
                                                        </td>
                                                        <td style="height: 20px">
                                                            <asp:TextBox ID="txtpgpmarks" runat="server" TabIndex="20" class="sinput"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                   
                                                   
                                                </table>
                                            </div>--%>

                                        <asp:Panel ID="pnlEducationDetails" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Education Details&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px">
                                                <asp:GridView ID="GvEducationDetails" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Qualification" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlQualification" runat="server" Width="92%">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="ILLITERATE" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="NON-MATRIC" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="MATRIC" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SENIOR SECONDARY" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="GRADUATE" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="POST GRADUATE" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="DOCTOR" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="TECHNICAL/PROFESSIONAL" Value="8"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Description" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEdLevel" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="Name & Address of School/College" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtNameofSchoolColg" runat="server" TextMode="MultiLine" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Board / University" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBoard" runat="server" Width="90%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Year of Study" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyear" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Pass / Fail" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassFail" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Percentage of Marks" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPercentage" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnEduAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnEduAdd_Click" />

                                            </div>
                                        </asp:Panel>


                                        <asp:Panel ID="pnlPreviousExpereince" runat="server" Enabled="false" GroupingText="<strong>&nbsp;Previous Experience&nbsp;</strong>" Style="margin-top: 10px">
                                            <div style="padding: 10px;">
                                                <asp:GridView ID="GvPreviousExperience" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                                    BorderStyle="Solid" CellPadding="5" ForeColor="#333333" Height="180px" PageSize="25" Visible="true"
                                                    ShowHeader="true" Style="margin: 0px auto;" Width="100%" CellSpacing="5">
                                                    <HeaderStyle Wrap="True" />
                                                    <PagerSettings Mode="NextPreviousFirstLast" />
                                                    <RowStyle />
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB"
                                                            HeaderText="S.No" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF3FB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Region Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtregioncode" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Employer Code" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtempcode" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Extension" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtExtension" runat="server" Text="" Width="92%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Designation" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPrevDesignation" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>



                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="19%"
                                                            HeaderText="Company Name/Address" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCompAddress" runat="server" TextMode="MultiLine" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="Years of Experience" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtyearofexp" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="PF No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPFNo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="10%"
                                                            HeaderText="ESI No." ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtESINo" runat="server" Text="" Width="95%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderStyle-Font-Size="Small" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#EFF3FB" HeaderStyle-Width="15%"
                                                            HeaderText="Date Of Resigned" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDtofResigned" runat="server"
                                                                    MaxLength="10" placeholder="DD/MM/YYYY" Width="95%"></asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Font-Size="Small"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                                <asp:Button ID="btnPrevExpAdd" runat="server" Text="Add" Style="margin-top: 10px" OnClick="btnPrevExpAdd_Click" />
                                            </div>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <div id="tabs-6">
                                <asp:UpdatePanel runat="server" ID="Updatepanel6">
                                    <ContentTemplate>
                                        <div>
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>Police Verification No</td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbVerified" runat="server" GroupName="P1" Text=" Verified" OnCheckedChanged="rdbVerified_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rdbNotVerified" runat="server" GroupName="P1" Text=" Not Verified" Checked="True" AutoPostBack="true" OnCheckedChanged="rdbNotVerified_CheckedChanged" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtPoliceVerificationNo" runat="server" CssClass="sinput" Enabled="false"></asp:TextBox></td>
                                                    <td>Nearest Police Station</td>
                                                    <td>
                                                        <asp:TextBox ID="txtPoliceStation" runat="server" CssClass="sinput"></asp:TextBox></td>
                                                </tr>
                                            </table>

                                            <asp:Panel ID="Panel1" Enabled="false" runat="server" GroupingText="<strong>&nbsp;PVC Address Details&nbsp;</strong>" Style="margin-top: 10px">

                                                <div class="dashboard_firsthalf" style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <%--<tr>
                                                            <td class="style4">
                                                                <strong>PVC Address :</strong>
                                                            </td>--%>
                                                        <%--<td>

                                                                <asp:CheckBox ID="CheckBox1" runat="server" Text=" Copy" AutoPostBack="true" OnCheckedChanged="chkSame_CheckedChanged" />
                                                            </td>--%>
                                                        <%--</tr>--%>

                                                        <tr>
                                                            <td>Land Mark</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvclandmark" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Village/Town</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcvillage" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Post Office</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpostofc" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Taluka/Hobli</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvctaluka" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>Police Station</td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpolicestation" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td>State
                                                            </td>
                                                            <td>


                                                                <asp:DropDownList ID="ddlpvcstate" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="21" AutoPostBack="true" OnSelectedIndexChanged="ddlpvcstate_SelectedIndexChanged"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div class="dashboard_secondhalf" style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td class="style4"></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>

                                                            <td>District
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlpvccity" runat="server" class="sdrop" Style="margin-left: 12px" TabIndex="23" Enabled="false"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Pin code
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcpin" runat="server" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>Date Since Residing
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcresidedate" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>
                                                                <cc1:calendarextender id="CEPvcreside" runat="server" enabled="true" targetcontrolid="txtpvcresidedate"
                                                                    format="dd/MM/yyyy"></cc1:calendarextender>
                                                                <cc1:filteredtextboxextender id="FTBpvcreside" runat="server" enabled="True"
                                                                    targetcontrolid="txtpvcresidedate" validchars="/0123456789"></cc1:filteredtextboxextender>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Period of stay
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcstay" runat="server" class="sinput" Style="margin-left: 12px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Phone(if any)
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpvcphone" runat="server" TabIndex="25" class="sinput" MaxLength="50" Style="margin-left: 12px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>

                                            <div style="margin-top: 10px">
                                                Criminal Offence 
                                             <asp:CheckBox ID="ChkCriminalOff" runat="server" Text=" (if criminal off is there)" OnCheckedChanged="ChkCriminalOff_CheckedChanged" AutoPostBack="true" />


                                                <asp:Panel ID="pnlGroupBox" Enabled="false" runat="server" GroupingText="<strong>&nbsp;Criminal Offence&nbsp;</strong>" CssClass="pstyle" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Off Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOffCName" runat="server" class="sinput" Style="margin-left: 15px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Off Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOffcaseNo" runat="server" class="sinput" Style="margin-left: 15px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalOff" runat="server" class="sinput" Enabled="false" Style="margin-left: 15px"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                Criminal Proceeding
                                            <asp:CheckBox ID="ChkCriminalProc" runat="server" Text=" (if any criminal proceeding are there,then tick)" AutoPostBack="True" OnCheckedChanged="ChkCriminalProc_CheckedChanged" />
                                                <asp:Panel ID="PnlCriminalProceeding" Enabled="false" runat="server" GroupingText="<strong>&nbsp;Criminal Proceeding&nbsp;</strong>" CssClass="pstyle" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Pro Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProCName" runat="server" class="sinput" Style="margin-left: 15px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Pro Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProCaseNo" runat="server" class="sinput" Style="margin-left: 15px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Pro Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalProOffence" runat="server" class="sinput" Style="margin-left: 15px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                                Criminal Arrest Warrant
                                            <asp:CheckBox ID="ChkCrimalArrest" runat="server" Text=" (if any criminal arrest warrant is issued,then tick)" AutoPostBack="True" OnCheckedChanged="ChkCrimalArrest_CheckedChanged" />
                                                <asp:Panel ID="PnlCriminalArrest" runat="server" GroupingText="<strong>&nbsp;Criminal Arrest Warrant&nbsp;</strong>" CssClass="pstyle" Style="padding: 10px">
                                                    <table cellpadding="5" cellspacing="5">
                                                        <tr>
                                                            <td>Criminal Arrest Court Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestCName" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Arrest Case No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestCaseNo" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Criminal Arrest Offence 
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCriminalArrestOffence" runat="server" class="sinput" Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <br />
                        <asp:Label runat="server" ID="lblres" Visible="False" Style="color: Red"></asp:Label>
                        <asp:Label runat="server" ID="lblquresult" Visible="false" Style="color: Red"></asp:Label>
                        <table width="20%" align="right">
                            <tr>
                                <td>
                                    <input type="button" id="btnPrevious" value="Previous" style="display: none" />
                                </td>
                                <td>

                                    <asp:Button ID="Btnedit" TabIndex="31" runat="server" Visible="true" class="btn save" OnClick="Btnedit_Click"
                                        Text="EDIT" />

                                </td>

                                <td>
                                    <input type="button" id="btnNext" value="Next" visible="false" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="Btn_Save_Personal_Tab" TabIndex="31" runat="server" Visible="false" class="btn save" OnClick="Btn_Save_Personal_Tab_Click"
                                        OnClientClick="return confirm('Are you sure you want to modify employee details?');"
                                        Text="Save" />
                                </td>
                                <td>
                                    <asp:Button ID="Btn_Cancel_Personal_Tab" TabIndex="32" runat="server" Visible="false" class="btn save" OnClick="Btn_Cancel_Personal_Tab_Click"
                                        OnClientClick="return confirm('Are you sure you want to cancel the modify employee details?');"
                                        Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <cc1:tabcontainer id="TabContainer1" runat="server" activetabindex="0">
                            </cc1:tabcontainer>


                    </div>
                </div>
            </div>
            <div class="loading" align="center">
                Loading. Please wait.<br />
                <br />
                <img src="assets/loader.gif" alt="Loading" />
            </div>
            <div class="clear">
            </div>
        </div>
        <!-- DASHBOARD CONTENT END -->
        <!-- FOOTER BEGIN -->

        <!-- CONTENT AREA END -->
    </div>

</asp:Content>
