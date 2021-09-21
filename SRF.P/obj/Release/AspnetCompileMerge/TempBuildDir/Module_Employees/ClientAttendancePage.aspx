<%@ Page Title="" Language="C#" MasterPageFile="~/Module_Employees/EmployeeMaster.Master" AutoEventWireup="true" CodeBehind="ClientAttendancePage.aspx.cs" Inherits="SRF.P.Module_Employees.ClientAttendancePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

    <link rel="shortcut icon" href="../assets/Mushroom.ico" />
    <link rel="stylesheet" href="../css/global.css" />
    <link href="../css/boostrap/css/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script src="../script/jquery.stickytableheaders.js" type="text/javascript"></script>

    <script type="text/javascript">
        //<![CDATA[
        window.onbeforeunload = function () {
            return '';
        };
        //]]>
        (function ($) {
            $.widget("custom.combobox", {
                _create: function () {
                    this.wrapper = $("<span>")
          .addClass("custom-combobox")
          .insertAfter(this.element);

                    this.element.hide();
                    this._createAutocomplete();
                    this._createShowAllButton();
                    this.input.attr("placeholder", this.element.attr('data-placeholder'));
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
              tooltipClass: "ui-state-highlight"
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
          .attr("title", "Show All")
          .tooltip()
          .appendTo(this.wrapper)
          .button({
              icons: {
                  primary: "ui-icon-triangle-1-s"
              },
              text: false
          })
          .removeClass("ui-corner-all")
          .addClass("custom-combobox-toggle ui-corner-right btnhgtwt")
          .mousedown(function () {
              wasOpen = input.autocomplete("widget").is(":visible");
          })
          .click(function () {
              input.focus();

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
        })(jQuery);

        // forceNumeric() plug-in implementation
        jQuery.fn.forceNumeric = function () {

            return this.each(function () {
                $(this).keydown(function (e) {
                    var key = e.which || e.keyCode;

                    if (!e.shiftKey && !e.altKey && !e.ctrlKey &&
                        // numbers   
                         key >= 48 && key <= 57 ||
                        // Numeric keypad
                         key >= 96 && key <= 105 ||
                        // comma, period and minus, . on keypad
                        key == 190 || key == 188 || key == 109 || key == 110 ||
                        // Backspace and Tab and Enter
                        key == 8 || key == 9 || key == 13 ||
                        // Home and End
                        key == 35 || key == 36 ||
                        // left and right arrows
                        key == 37 || key == 39 ||
                        // Del and Ins
                        key == 46 || key == 45)
                        return true;

                    return false;
                });
                $(this).keydown(function (e) {
                    CalculateTotals();
                    var linetotal = 0;
                    $(this).parent().parent().find(".num-txt").each(function () {
                        linetotal += parseInt($(this).val());
                    });
                    $(this).parent().parent().find(".txt-linetotal").text(linetotal);
                });
            });
        }


        function chkchange() {
            if (document.getElementById("<%=chkold.ClientID%>").checked) {

                document.getElementById("<%=txtmonth.ClientID %>").style.visibility = "visible";
                document.getElementById("<%=ddlMonth.ClientID %>").style.visibility = "hidden";
                $("<%=txtmonth.ClientID %>").val("");


            } else {
                document.getElementById("<%=txtmonth.ClientID %>").style.visibility = "hidden";
                document.getElementById("<%=ddlMonth.ClientID %>").style.visibility = "visible";
                $("<%=txtmonth.ClientID %>").val("");

            }
        }


        $(document).ready(function () {
            // passing a fixedOffset param will cause the table header to stick to the bottom of this element
            $("table").stickyTableHeaders({ scrollableArea: window, "fixedOffset": 0 });
            $('.destroy').on('click', function (e) {
                $("#tblattendancegrid").stickyTableHeaders('destroy');
            });
            $('.apply').on('click', function (e) {
                $('#tblattendancegrid').stickyTableHeaders({ scrollableArea: $(".scrollable-area")[2], "fixedOffset": 2 });
            });
        });

        $(document).ready(function () {
            $(".num-txt").forceNumeric();
            $(".txt-calender").datepicker({ defaultDate: new Date(), dateFormat: 'dd/mm/yy' });
            var tdate = new Date();
            $(".txt-calender").val(getFormattedDate(tdate));
            GetClientsValues();
            $(".ddlautocomplete").combobox({
                select: function (event, ui) { $("#<%=divClient.ClientID %>").attr("data-clientId", ui.item.value); OnAutoCompleteDDLchange(event, ui); },
                minLength: 4
            });


            $("#<%=txtEmpId.ClientID %>").autocomplete({
                source: function (request, response) {

                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetEmployessData";

                   
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        data: "{strid:" + request.term + "}",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            if (json != "") {
                                // var data = eval(json.d);
                                response($.map(json, function (item) {
                                    var obj = { value: item.EmpDesg + "|<>|" + item.EmpName, label: item.EmpId };
                                    return obj;
                                }));
                            }
                        },
                        error: function (json) { InvalidEmpData(); }
                    });
                },
                minLength: 4,
                select: function (event, ui) {
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-id");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-name");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-desg");
                    var vals = ui.item.value.split('|<>|');
                    $("#<%=txtEmpName.ClientID %>").val(vals[1]);
                    $("#<%=ddlEmpDesg.ClientID %>").val(vals[0]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-id", ui.item.label);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-name", vals[1]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-desg", vals[0]);
                    this.value = ui.item.label



                    return false;
                }
            });

            $("#<%=txtEmpName.ClientID %>").autocomplete({
                source: function (request, response) {
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetEmployessNameData";

                 
                    var dataitem = JSON.stringify({ "strname": request.term });
                    $.ajax({
                        type: "POST",
                        url: ajaxUrl,
                        data: { strname: request.term },
                        data: dataitem,
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (json) {
                            if (json != "") {
                                // var data = eval(json.d);
                                response($.map(json, function (item) {
                                    var obj = { value: item.EmpDesg + "|<>|" + item.EmpId, label: item.EmpName };
                                    return obj;
                                }));
                            }
                        },
                        error: function (json) { InvalidEmpData(); }
                    });
                },
                minLength: 4,
                select: function (event, ui) {
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-id");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-name");
                    $("#<%=trAddData.ClientID %>").removeAttr("data-emp-desg");
                    var vals = ui.item.value.split('|<>|');
                    $("#<%=txtEmpId.ClientID %>").val(vals[1]);
                    $("#<%=ddlEmpDesg.ClientID %>").val(vals[0]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-id", vals[1]);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-name", ui.item.label);
                    $("#<%=trAddData.ClientID %>").attr("data-emp-desg", vals[0]);
                    this.value = ui.item.label
                    return false;
                }
            });


        });

        function InvalidEmpData() {
            $("#<%=txtEmpName.ClientID %>").val("");
            $("#<%=txtEmpId.ClientID %>").val("");
            $("#<%=trAddData.ClientID %>").attr("data-emp-id", "");
            $("#<%=trAddData.ClientID %>").attr("data-emp-name", "");
            $("#<%=trAddData.ClientID %>").attr("data-emp-desg", "");
            alert("invalid!!");
        }

        function GetClientsValues() {
            var json = JSON.parse($("#<%=hdClientData.ClientID %>").val());
            $("#<%=divClient.ClientID %>").attr("data", JSON.stringify(json));
            var data = json;
            BindClientIdDDL(data);
            BindClientNameDDL(data);
        }

        function BindClientIdDDL(data) {
            $("#<%=ddlClientID.ClientID %>").html("");
            $("#<%=ddlClientID.ClientID %>").append("<option value='-1'></option>");

            var databs = [];
            $.each(data, function (index, element) {
                databs.push(element.ClientId);
            });
            databs.sort();
            $.each(databs, function (index, element) {
                $("#<%=ddlClientID.ClientID %>").append("<option value=" + element + ">" + element + "</option>");
            });
            }

            function BindClientNameDDL(data) {
                $("#<%=ddlClientName.ClientID %>").html("");
                $("#<%=ddlClientName.ClientID %>").append("<option value='-1'></option>");
                $.each(data, function (index, element) {
                    $("#<%=ddlClientName.ClientID %>").append("<option value=" + element.ClientId + ">" + element.ClientName + "</option>");
               });
               }

               function SetAutoCompleteValue(ddlid, defValue) {

                   if (ddlid == "id") {
                       $("#<%=ddlClientName.ClientID %>").combobox("destroy");
                        $("#<%=ddlClientName.ClientID %>").val(defValue);
                        $("#<%=ddlClientName.ClientID %>").combobox({
                            select: function (event, ui) { OnAutoCompleteDDLchange(event, ui); }
                        });
                    }
                    else {
                        $("#<%=ddlClientID.ClientID %>").combobox("destroy");
                        $("#<%=ddlClientID.ClientID %>").val(defValue);
                        $("#<%=ddlClientID.ClientID %>").combobox({
                            select: function (event, ui) { OnAutoCompleteDDLchange(event, ui); }
                        });
                    }
                }

                function OnAutoCompleteDDLchange(event, ui) {

                    var targetddlid = "";
                    console.log(event.target.id);
                    if (event.target.id === "ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_ddlClientID") { targetddlid = "id"; }
                    else if (event.target.id === "ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder3_ddlClientName") targetddlid = "name";
                    SetAutoCompleteValue(targetddlid, ui.item.value);
                    $("#<%=ddlMonth.ClientID %>").val(0);
                $("#tblattendancegrid>tbody").html("");  //tblattendancegrid
                $("#tblSummary >tbody").html("");
                if (targetddlid == '#<%=ddlClientName.ClientID %>' || targetddlid == '#<%=ddlClientID.ClientID %>') { ChangeClientValues(ui.item.value); }
                }

                function ChangeClientValues(cid) {
                    var datastr = $("#<%=divClient.ClientID %>").attr("data");
                    var data = eval(datastr);
                    $.each(data, function (indx, ele) {
                        if (ele.ClientId == cid) {
                            $("#txtphonenumbers").val(ele.PhoneNumber);
                            $("#txtocp").val(ele.ContactPerson);
                            $("#<%=ddlMonth.ClientID %>").val(0);
                        }
                    });
                    $("#tblattendancegrid >tbody").html("");
                    $(".num-txt").forceNumeric();
                    CalculateTotals();
                }


                function GetMonth() {

                    var month = "0";;
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();

                    }
                    else {
                        month = $("#ddlMonth option:selected").index();
                    }
                }

                function AddNewEmp(ele) {
                    var empid = $("#<%=txtEmpId.ClientID %>").val();
                    var empname = $("#<%=txtEmpName.ClientID %>").val();
                    var empdesgid = $("#<%=ddlEmpDesg.ClientID %>").val();
                    var empdesgname = $('#<%= ddlEmpDesg.ClientID %> option:selected').text();
                    var empttype = $('#<%= ddlTransfertype.ClientID %> option:selected').val();
                    var jdate = $("#txtJoingingDate").val();
                    var rdate = $("#txtRelievingDate").val();
                    var esi = $("#chkESI").is(":checked");
                    var pt = $("#chkPT").is(":checked");
                    var pf = $("#chkPF").is(":checked");
                    var nod = $("#txt-add-nod").val();
                    var ot = $("#txt-add-ot").val();
                    var wo = $("#txt-add-wo").val();
                    var nhs = $("#txt-add-nhs").val();
                    var npots = $("#txt-add-npots").val();
                    var canadv = $("#txt-add-canadv").val();
                    var pen = $("#txt-add-pen").val();
                    var inctvs = $("#txt-add-inctvs").val();
                    var Leaves = $("#txt-add-Leaves").val();
                    //var Extra = $("#txt-add-Extra").val();
                    var updated = false;
                    if ($('#tblattendancegrid > tbody > tr').length > 0) {
                        $('#tblattendancegrid > tbody > tr').each(function (i, row) {
                            var trempid = $(row).attr("data-emp-id");
                            var trempdesg = $(row).attr("data-emp-desg");
                            if (empid == trempid && empdesgid == trempdesg) {
                                $(row).find(".txt-nod").val(nod);
                                $(row).find(".txt-ot").val(ot);
                                $(row).find(".txt-wo").val(wo);
                                $(row).find(".txt-nhs").val(nhs);
                                $(row).find(".txt-nposts").val(npots);
                                $(row).find(".txt-candav").val(canadv);
                                $(row).find(".txt-pen").val(pen);
                                $(row).find(".txt-inctvs").val(inctvs);
                                $(row).find(".txt-Leaves").val(Leaves);
                                //$(row).find(".txt-Extra").val(Extra);
                                alert("Employee attendance updated.");
                                updated = true;
                            }
                        });
                    }
                    if (!updated) {
                        var nr = "<tr class='tr-emp-att new-row' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-ttype='##EMPTTYPE##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##' >" +
                                         "<td>##EMPID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##" +
                                         "<td><input type='text' class='form-control num-txt txt-nod line-cal' value='##NOD##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-ot line-cal' value='##OT##'></td>" +
                                         "<td><input type='text' class='form-control num-txt txt-wo line-cal' value='##WO##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-nhs line-cal' value='##NHS##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-nposts line-cal' value='##NPOSTS##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td>  " +
                                        " <td><input type='text' class='form-control num-txt txt-Leaves' value='##LEAVES##'></td>  " +
                                        //" <td><input type='text' class='form-control num-txt txt-Extra' value='##EXTRA##'></td>  " +
                                        " <td><label class='txt-linetotal'/> " +
                                        " <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>" +
                                       " </tr>";
                        if (empid != "" && empdesgid != "0") {
                            var newrow = nr.replace("##EMPID##", empid).replace("##EMPID##", empid)
                                      .replace('##EMPNAME##', empname)
                                      .replace('##EMPDESG##', empdesgid)
                                      .replace('##EMPDESGNAME##', empdesgname)
                                      .replace('##EMPJDATE##', jdate)
                                      .replace('##EMPRDATE##', rdate)
                                      .replace('##EMPPF##', pf)
                                      .replace('##EMPPT##', pt)
                                      .replace('##EMPESI##', esi)
                                      .replace('##EMPTTYPE##', empttype)
                                      .replace('##NOD##', nod)
                                      .replace('##OT##', ot)
                                      .replace('##WO##', wo)
                                      .replace('##NHS##', nhs)
                                      .replace('##NPOSTS##', npots)
                                      .replace('##CANADV##', canadv)
                                      .replace('##PEN##', pen)
                                      .replace('##INCTVS##', inctvs)
                               .replace('##LEAVES##', Leaves)
                            //.replace('##EXTRA##', Extra)

                            ;
                            $("#tblattendancegrid >tbody").append(newrow);
                            alert("Employee added.");
                        }
                        else {
                            alert("Select Employee and Designation");
                        }
                    }
                    $(".num-txt").forceNumeric();
                    CalculateTotals();
                    ClearEmpAddValues();
                }

                function ClearEmpAddValues() {
                    $("#<%=txtEmpId.ClientID %>").val("");
                    $("#<%=txtEmpName.ClientID %>").val("");
                    $("#<%=ddlEmpDesg.ClientID %>").val(0);
                   $("#<%=ddlTransfertype.ClientID %>").val(0);
                    //var tdate = new Date();
                    //$(".txt-calender").val(getFormattedDate(tdate));
                    $("#chkESI")[0].checked = true;
                    $("#chkPT")[0].checked = true;
                    $("#chkPF")[0].checked = true;
                    $("#txt-add-nod").val("0");
                    $("#txt-add-ot").val("0");
                    $("#txt-add-wo").val("0");
                    $("#txt-add-nhs").val("0");
                    $("#txt-add-npots").val("0");
                    $("#txt-add-canadv").val("0");
                    $("#txt-add-pen").val("0");
                    $("#txt-add-inctvs").val("0");
                    $("#txt-add-Leaves").val("0");
                    //$("#txt-add-Extra").val("0");
                    $("#<%=txtEmpId.ClientID %>").focus();
                }

                function DeleteRow(ele) {
                    if (confirm("Are you sure you want to remove the employee from this unit?")) {
                        if ($(ele).parent().parent().hasClass("new-row")) {
                            $(ele).parent().parent().remove();
                            alert("Employee deleted for current month.");
                        }
                        else {
                            var trclientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                            //var trmonth = $("#ddlMonth option:selected").index();
                            var trmonth = 0;
                            var trChk = $("#<%=chkold.ClientID %>").is(":checked");
                            if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                                trmonth = $("#<%=txtmonth.ClientID %>").val();
                            }
                            else {
                                trmonth = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                            }
                            var trempid = $(ele).parent().parent().attr("data-emp-id");
                            var trempdesg = $(ele).parent().parent().attr("data-emp-desg");
                            var ajaxUrl = window.location.href.substring(0, window.location.href.lastIndexOf('/')) + "/FameService.asmx/DeleteAttendance";
                            if (trclientId != undefined && trclientId != "0" && trclientId != "" && trmonth != undefined && trmonth != "0") {
                                var dataparam = JSON.stringify({ empId: trempid, empDesgId: trempdesg, clientId: trclientId, month: trmonth, Chk: trChk });
                                $.ajax({
                                    type: "POST",
                                    url: ajaxUrl,
                                    data: dataparam,
                                    async: false,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (json) {
                                        if (json != "") {
                                            $(ele).parent().parent().remove();
                                            alert("Employee deleted for current month.");
                                        }
                                    },
                                    error: function (json) { alert(json); }
                                });
                            } else {
                                alert('select ClientID');
                            }
                        }
                        CalculateTotals();
                    }
                }

                function Empddlchange(ele) {
                    var id = $(ele).attr("id");
                    if (id == "ddlEmpId") {
                        var val = $("#ddlEmpId option:selected").val();
                        var txt = $("#ddlEmpId option:selected").text();
                        $("#ddlEmpName").val(txt);
                        $("#<%=ddlEmpDesg.ClientID %>").val(val);
                    }
                    if (id == "ddlEmpName") {
                        $("#ddlEmpId option").removeAttr("selected");
                        var val = $("#ddlEmpName option:selected").val();
                        var empdes = $("#ddlEmpId option:contains(" + val + ")").val();
                        var empid = $("#ddlEmpId option:contains(" + val + ")").text();
                        $("#ddlEmpId option").each(function () {
                            if ($(this).text() == empid) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        $("#<%=ddlEmpDesg.ClientID %>").val(empdes);
                    }
                }

                function GetEmpAttendanceData() {
                    openModal();
                    var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                    //var month = $("#ddlMonth option:selected").index();
                    var month = 0;
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                         month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }

                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetAttendanceGrid";
                    if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                        $.ajax({
                            type: "POST",
                            url: ajaxUrl,
                            data: "{clientId:'" + clientId + "',month:'" + month + "',Chk:'" + Chk + "'}",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (json) {
                                if (json != "") {
                                    if (json.msg == "success") {
                                        AddrowstoTbl(json.Obj);
                                    }
                                    else if (json.msg == "fail") {
                                        $("#tblattendancegrid >tbody").html("");
                                    }
                                    else if (json.msg == "nodata") {
                                        $("#tblattendancegrid >tbody").html("");
                                    }
                                }
                            },
                            error: function (json) { alert('fail'); }
                        });
                    } else {
                        alert("Select ClientId and month");
                    }
                    closeModal();
                    GetEmpAttendanceDataSummarry();
                }

                function AddrowstoTbl(data) {
                    data = eval(data);
                    var MonthDays = "";
                    $("#tblattendancegrid >tbody").html("");

                    $.each(data, function (i, item) {
                        var nr = "<tr class='tr-emp-att' data-emp-id='##EMPID##' data-emp-desg='##EMPDESG##' data-emp-ttype='##EMPTTYPE##' data-emp-jdate='##EMPJDATE##' data-emp-rdate='##EMPRDATE##' data-emp-pf='##EMPPF##' data-emp-pt='##EMPPT##' data-emp-esi='##EMPESI##'  >" +
                                         "<td>##EMPID##</td><td>##EMPNAME##</td><td>##EMPDESGNAME##<br>Month Days : ##NOOFDAYS##</td>" +
                                         "<td><input type='text' class='form-control num-txt txt-nod line-cal' value='##NOD##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-ot line-cal' value='##OT##'></td>" +
                                         "<td><input type='text' class='form-control num-txt txt-wo line-cal' value='##WO##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-nhs line-cal' value='##NHS##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-nposts line-cal' value='##NPOSTS##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-candav' value='##CANADV##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-pen' value='##PEN##'></td>" +
                                        " <td><input type='text' class='form-control num-txt txt-inctvs' value='##INCTVS##'></td>  " +
                                        " <td><input type='text' class='form-control num-txt txt-Leaves' value='##LEAVES##'></td>  " +
                                        //" <td><input type='text' class='form-control num-txt txt-Extra' value='##EXTRA##'></td>  " +

                                        " <td><label class='txt-linetotal'/> " +
                                        " <td><button type='button' class='btn btn-danger' onclick='DeleteRow(this); return false;'><i class='glyphicon glyphicon-trash'></i></button></td>" +
                                       " </tr>";




                        if (item.noofdays == 3) {
                            MonthDays = "P.Hr";
                        }
                        else if (item.noofdays == 4) {
                            MonthDays = "P.Day";
                        }
                        else {
                            MonthDays = item.noofdays.toString();
                        }





                        var newrow = nr.replace("##EMPID##", item.EmpId).replace("##EMPID##", item.EmpId)
                                  .replace('##EMPNAME##', item.EmpName)
                                  .replace('##EMPDESG##', item.DesgId)
                                  .replace('##EMPDESGNAME##', item.DesgName)
                                    .replace('##NOOFDAYS##', MonthDays)
                                  .replace('##NOD##', item.NoOfDuties)
                                  .replace('##OT##', item.OT)
                                  .replace('##WO##', item.WO)
                                  .replace('##NHS##', item.NHS)
                                  .replace('##NPOSTS##', item.NPosts)
                                  .replace('##CANADV##', item.CanteenAdv)
                                  .replace('##PEN##', item.Penalty)
                                  .replace('##INCTVS##', item.Incentivs)
                         .replace('##LEAVES##', item.Leaves);
                        //.replace('##EXTRA##', item.Extra);
                        $("#tblattendancegrid >tbody").append(newrow);


                    });

                    $(".num-txt").forceNumeric();
                    CalculateTotals();
                }

                function GetEmpAttendanceDataSummarry() {
                    openModal();
                    var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                    // var month = $("#ddlMonth option:selected").index();
                    var month = 0;
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if ($("#<%=chkold.ClientID %>").is(':checked') == true) {
                        month = $("#<%=txtmonth.ClientID %>").val();
                    }
                    else {
                         month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                    var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/GetAttendanceSummary";
                    if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                        $.ajax({
                            type: "POST",
                            url: ajaxUrl,
                            data: "{clientId:'" + clientId + "',month:'" + month + "',Chk:'" + Chk + "'}",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (json, b, c) {
                                if (json != "") {
                                    //var res = JSON.parse(json.d);
                                    if (json.msg == "success") {
                                        AddSummaryTbl(json.Obj);
                                        $("#divSummary").show();
                                    }
                                    else if (json.msg == "nodata") {
                                        $("#tblSummary >tbody").html("");
                                        $("#divSummary").hide();
                                    }
                                }
                            },
                            error: function (json) { alert('hi'); }
                        });
                    }
                    closeModal();
                }

                function AddSummaryTbl(data) {
                    data = eval(data);
                    $("#tblSummary >tbody").html("");
                    $.each(data, function (i, item) {
                        var strr = "<tr class='tr-emp-summary'>" +
                           " <td><label class='lbl-tdesg lbl-thin'>##Designation##</label></td>" +
                            "<td><label class='lbl-tnod lbl-thin lbl-tots'>##TNOD##</label></td>" +
                            "<td><label class='lbl-tot lbl-thin lbl-tots'>##TOT##</label></td>" +
                            "<td><label class='lbl-two lbl-thin lbl-tots'>##TWO##</label></td>" +
                            "<td><label class='lbl-tnhs lbl-thin lbl-tots'>##TNHS##</label></td>" +
                            "<td><label class='lbl-tnpots lbl-thin lbl-tots'>##TNPOTS##</label></td>" +
                                "<td><label class='lbl-Totals'></label></td>" +
                            "<td><label class='lbl-tcadv lbl-thin'>##TCADV##</label></td>" +
                            "<td><label class='lbl-tpen lbl-thin'>##TPEN##</label></td>" +
                            "<td><label class='lbl-tinctvs lbl-thin'>##TINTVS##</label></td>" +
                            "<td><label class='lbl-tLeaves lbl-thin'>##TLEAVES##</label></td></tr>";
                        //"<td><label class='lbl-tExtra lbl-thin'>##TEXTRA##</label></td>
                        var newrow = strr.replace("##Designation##", item.DesgName)
                                    .replace('##TNOD##', item.NODTotal)
                                    .replace('##TOT##', item.OTTotal)
                                    .replace('##TWO##', item.WOTotal)
                                    .replace('##TNHS##', item.NHSTotal)
                                    .replace('##TNPOTS##', item.NpotsTotal)
                                    .replace('##TCADV##', item.CanAdvTotal)
                                    .replace('##TPEN##', item.PenTotal)
                                    .replace('##TINTVS##', item.InctvsTotal)
                         .replace('##TLEAVES##', item.LeavesTotal)
                        //.replace('##TEXTRA##', item.ExtraTotal)
                        ;
                        $("#tblSummary >tbody").append(newrow);
                    });
                    CalculateSummaryTotals();
                }

                function CalculateTotals() {
                    var nodtotal = 0;
                    $('.txt-nod').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            nodtotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblNOD").text(nodtotal);

                    var ottotal = 0;
                    $('.txt-ot').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            ottotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblOT").text(ottotal);

                    var wototal = 0;
                    $('.txt-wo').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            wototal += parseFloat($(this).val());
                        }
                    });
                    $("#lblWO").text(wototal);

                    var nhstotal = 0;
                    $('.txt-nhs').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            nhstotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblNHS").text(nhstotal);

                    var npoststotal = 0;
                    $('.txt-nposts').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            npoststotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblNpots").text(npoststotal);

                    var candavtotal = 0;
                    $('.txt-candav').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            candavtotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblCanAdv").text(candavtotal);

                    var pentotal = 0;
                    $('.txt-pen').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            pentotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblPen").text(pentotal);

                    var inctvstotal = 0;
                    $('.txt-inctvs').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            inctvstotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblInctvs").text(inctvstotal);
                    //
                    var Leavestotal = 0;
                    $('.txt-Leaves').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            Leavestotal += parseFloat($(this).val());
                        }
                    });
                    $("#lblLeaves").text(Leavestotal);
                    //2
                    //var Extratotal = 0;
                    //$('.txt-Extra').each(function () {
                    //    if ($(this).val() != "" && $(this).val() != undefined) {
                    //        Extratotal += parseFloat($(this).val());
                    //    }
                    //});
                    //$("#lblExtra").text(Extratotal);

                    $(".tr-emp-att").each(function () {
                        var linetotal = 0;
                        $(this).find(".line-cal").each(function () {
                            if ($(this).val() != "" && $(this).val() != undefined) {
                                linetotal += parseFloat($(this).val());
                            }
                        });
                        $(this).find(".txt-linetotal").text(linetotal);
                    });
                    var ttls = 0;
                    $('.txt-linetotal').each(function () {
                        if ($(this).val() != "" && $(this).val() != undefined) {
                            ttls += parseFloat($(this).text());
                        }
                    });
                    $("#lblTTotals").text(ttls);

                }

                function CalculateLineTotals() {

                }

                function CalculateSummaryTotals() {
                    var nodtotal = 0;
                    $('.lbl-tnod').each(function () {
                        nodtotal += parseFloat($(this).text());
                    });
                    $("#lblTNOD").text(nodtotal);

                    var ottotal = 0;
                    $('.lbl-tot').each(function () {
                        ottotal += parseFloat($(this).text());
                    });
                    $("#lblTOT").text(ottotal);

                    var wototal = 0;
                    $('.lbl-two').each(function () {
                        wototal += parseFloat($(this).text());
                    });
                    $("#lblTWO").text(wototal);

                    var nhstotal = 0;
                    $('.lbl-tnhs').each(function () {
                        nhstotal += parseFloat($(this).text());
                    });
                    $("#lblTNHS").text(nhstotal);

                    var npoststotal = 0;
                    $('.lbl-tnpots').each(function () {
                        npoststotal += parseFloat($(this).text());
                    });
                    $("#lblTNPOTS").text(npoststotal);

                    var candavtotal = 0;
                    $('.lbl-tcadv').each(function () {
                        candavtotal += parseFloat($(this).text());
                    });
                    $("#lblTCADV").text(candavtotal);

                    var pentotal = 0;
                    $('.lbl-tpen').each(function () {
                        pentotal += parseFloat($(this).text());
                    });
                    $("#lblTPEN").text(pentotal);
                    var inctvstotal = 0;
                    $('.lbl-tinctvs').each(function () {
                        inctvstotal += parseFloat($(this).text());
                    });
                    $("#lblTInctvs").text(inctvstotal);
                    //1
                    var Leavestotal = 0;
                    $('.lbl-tLeaves').each(function () {
                        Leavestotal += parseFloat($(this).text());
                    });
                    $("#lblTLeaves").text(Leavestotal);

                    //1
                    //var Extratotal = 0;
                    //$('.lbl-tExtra').each(function () {
                    //    Extratotal += parseFloat($(this).text());
                    //});
                    //$("#lblTExtra").text(Extratotal);

                    //2

                    //2
                    $(".tr-emp-summary").each(function () {
                        var linetotal = 0;
                        $(this).find(".lbl-tots").each(function () {
                            linetotal += parseFloat($(this).text());
                        });
                        $(this).find(".lbl-Totals").text(linetotal);
                    });
                    var ttls = 0;
                    $('.lbl-Totals').each(function () {
                        ttls += parseFloat($(this).text());
                    });
                    $("#lblTotals").text(ttls);
                }

                function SaveAttendance() {
                    var datalst = [];
                    openModal();
                    var clientId = $("#<%=divClient.ClientID %>").attr("data-clientId");
                    //var month = $("#ddlMonth option:selected").index();
                    var month = "0";
                    var Chk = $("#<%=chkold.ClientID %>").is(":checked");
                    if (Chk == true) {
                        var date = $("#<%=txtmonth.ClientID %>").datepicker('getDate');
                        var year = date.getFullYear().toString();
                        var monthv = date.getMonth();
                        if (monthv == 11) {
                            monthv = 12;
                        }
                        else {
                            monthv = date.getMonth() + 1;
                        }
                        month = monthv + year.substr(2, 2);
                    }
                    else {
                          month = $("#<%=ddlMonth.ClientID %>").find(":selected").index();
                    }
                    var ottype = parseInt($("#ddlOTtype").val());
                    if ($('#tblattendancegrid > tbody > tr').length != undefined && $('#tblattendancegrid > tbody > tr').length > 0) {
                        $('#tblattendancegrid > tbody > tr').each(function (i, row) {
                            var isnewrow = $(row).hasClass("new-row");
                            var EmpAttendance = {
                                ClientId: clientId,
                                MonthIndex: month,
                                Chkbox: Chk,
                                NewAdd: isnewrow,
                                EmpId: $(row).attr("data-emp-id"),
                                EmpDesg: $(row).attr("data-emp-desg"),
                                JoiningDate: (isnewrow) ? $(row).attr("data-emp-jdate") : "",
                                RelievingDate: (isnewrow) ? $(row).attr("data-emp-rdate") : "",
                                PF: (isnewrow) ? $(row).attr("data-emp-pf") : false,
                                PT: (isnewrow) ? $(row).attr("data-emp-pt") : false,
                                ESI: (isnewrow) ? $(row).attr("data-emp-esi") : false,
                                TransferType: (isnewrow) ? $(row).attr("data-emp-ttype") : 1,
                                NOD: parseFloat($(row).find(".txt-nod").val()),
                                OT: parseFloat($(row).find(".txt-ot").val()),
                                WO: parseFloat($(row).find(".txt-wo").val()),
                                NHS: parseFloat($(row).find(".txt-nhs").val()),
                                Nposts: parseFloat($(row).find(".txt-nposts").val()),
                                CanAdv: parseFloat($(row).find(".txt-candav").val()),
                                Penality: parseFloat($(row).find(".txt-pen").val()),
                                Incentives: parseFloat($(row).find(".txt-inctvs").val()),
                                Leaves: parseFloat($(row).find(".txt-Leaves").val()),
                                //Extra: parseFloat($(row).find(".txt-Extra").val()),
                                OTtype: ottype
                            };
                            datalst.push(EmpAttendance);
                        });
                        var url = window.location.href.substring(0, window.location.href.lastIndexOf('/'));
                        var ajaxUrl = url.substring(0, url.lastIndexOf('/')) + "/FameService.asmx/SaveAttendance";
                        if (clientId != undefined && clientId != "0" && clientId != "" && month != undefined && month != "0") {
                            if (datalst.length > 200) {
                                var lstdata = [];
                                var startindx = 0; var looplength = 200; var nxtlooplength = 200;
                                do {
                                    if (startindx > 0 && looplength < datalst.length) {
                                        nxtlooplength = datalst.length - looplength;
                                        looplength += nxtlooplength
                                    }
                                    lstdata = datalst.slice(startindx, looplength);
                                    var dataparam = JSON.stringify({ lst: lstdata });
                                    $.ajax({
                                        type: "POST",
                                        url: ajaxUrl,
                                        data: dataparam,
                                        async: false,
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (json) {
                                            if (json != "") {
                                                if (json.msg == "success") {
                                                    console.log("startindx:" + startindx + " looplenth:" + looplength);
                                                }
                                                else {
                                                    console.log("startindx:" + startindx + " looplenth:" + looplength);
                                                    console.log(json.Obj);
                                                }
                                            }
                                        },
                                        error: function (json) { alert('fail'); }
                                    });
                                    startindx += looplength;

                                } while (startindx < datalst.length);

                                alert("Employees Attendance Saved.");
                                GetEmpAttendanceData();
                            }
                            else if (datalst.length > 0) {
                                var dataparam = JSON.stringify({ lst: datalst });
                                $.ajax({
                                    type: "POST",
                                    url: ajaxUrl,
                                    data: dataparam,
                                    async: false,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (json) {
                                        if (json != "") {
                                            if (json.msg == "success") {
                                                alert("Employees Attendance Saved.");
                                                GetEmpAttendanceData();
                                            }
                                            else {
                                                alert(json.Obj);
                                            }
                                        }
                                    },
                                    error: function (json) { alert('fail'); }
                                });
                            }
                        } else {
                            alert("Select ClientId and month.");
                        }
                    }
                    else {
                        alert("Enter Employee to Save Attendance.");
                    }
                    closeModal();
                }

                function openModal() {
                    document.getElementById('modal').style.display = 'block';
                    document.getElementById('fade').style.display = 'block';
                }

                function closeModal() {
                    document.getElementById('modal').style.display = 'none';
                    document.getElementById('fade').style.display = 'none';
                }

                function getFormattedDate(date) {
                    var year = date.getFullYear();
                    var month = (1 + date.getMonth()).toString();
                    month = month.length > 1 ? month : '0' + month;
                    var day = date.getDate().toString();
                    day = day.length > 1 ? day : '0' + day;
                    return day + '/' + month + '/' + year;
                }





    </script>

    <style type="text/css">
        .lbl-thin {
            font-weight: 100 !important;
        }

        #fade {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 2000px;
            background-color: #ababab;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .70;
            filter: alpha(opacity=80);
        }

        #modal {
            display: none;
            position: absolute;
            top: 45%;
            left: 45%;
            width: 100px;
            height: 100px;
            padding: 30px 15px 0px;
            border: 3px solid #ababab;
            box-shadow: 1px 1px 10px #ababab;
            border-radius: 20px;
            background-color: white;
            z-index: 1002;
            text-align: center;
            overflow: auto;
        }

        #results {
            font-size: 1.25em;
            color: red;
        }

        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
   * we use height instead, but this forces the menu to always be this tall
   */ * html .ui-autocomplete {
            height: 200px;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
            width: 84%;
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
            width: 100%;
        }

        .btnhgtwt {
            top: 0px;
            height: 31px;
        }

        .num-txt {
            padding: 0 5px;
            width: 40px;
        }
    </style>

    <div id="content-holder">
        <div class="content-holder">
            <h1 class="dashboard_heading">Clients Dashboard</h1>
            <div class="row">
                <div class="row">
                    <div id="divClient" runat="server">
                        <asp:HiddenField ID="hdClientData" runat="server" />
                        <table class="table">
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Client Id</label>
                                </td>
                                <td>
                                    <select id="ddlClientID" data-placeholder="select" class="ddlautocomplete chosen-select" runat="server" name="clientid"
                                        style="width: 350px;">
                                    </select>
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Client Name</label>
                                </td>
                                <td>
                                    <select id="ddlClientName" data-placeholder="select" class="ddlautocomplete chosen-select" runat="server"
                                        style="width: 350px;">
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Phone N0(s)
                                    </label>
                                </td>
                                <td>
                                    <input id="txtphonenumbers" type="text" class="form-control" id="dd" style="width: 350px;">
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Our Contact Person</label>
                                </td>
                                <td>
                                    <input id="txtocp" type="text" class="form-control" id="dd" style="width: 350px;">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="exampleInputEmail1">
                                        Month</label>
                                </td>
                                <td>
                                    <select id="ddlMonth" runat="server" class="form-control" onchange="GetEmpAttendanceData();"
                                        style="width: 350px;">
                                    </select>
                                    <input id="txtmonth" type="text" class="form-control txt-calender" runat="server" style="width: 350px; position: relative; bottom: 25px; visibility: hidden"
                                        onchange="GetEmpAttendanceData();" />
                                </td>
                                <td>
                                    <label for="exampleInputEmail1">
                                        OT in terms of</label>
                                </td>
                                <td>
                                    <select id="ddlOTtype" class="form-control" style="width: 350px;">
                                        <option value="0">Days</option>
                                        <option value="1">Hours</option>
                                    </select>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="2">

                                    <label for="exampleInputEmail1">
                                        Old</label>
                                    <input type="checkbox" id="chkold" title="Old" runat="server" value="false" style="padding-bottom: 5px" onchange="chkchange();" />

                                </td>
                                <td colspan="2"></td>
                            </tr>

                        </table>
                    </div>
                </div>
                <div id="divSummary" class="row" style="display: none;">
                    <table id="tblSummary" class="table table-striped table-bordered table-condensed table-hover">
                        <thead>
                            <tr class="warning">
                                <th>Designation
                                </th>
                                <th>Number of Duties
                                </th>
                                <th>Ot's
                                </th>
                                <th>WO's
                                </th>
                                <th>NHS's
                                </th>
                                <th>Npots's
                                </th>
                                <th>Totals
                                </th>
                                <th>Incentives
                                </th>
                                <th>Rent
                                </th>
                                <th>Canteen Advance
                                </th>
                                <th>Leaves
                                </th>
                                <%-- <th>Extra
                                    </th>--%>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot class="active">
                            <tr>
                                <td>
                                    <label id="lblHead">
                                        Total :</label>
                                </td>
                                <td>
                                    <label id="lblTNOD">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTOT">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTWO">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTNHS">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTNPOTS">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTotals">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTCADV">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTPEN">
                                    </label>
                                </td>
                                <td>
                                    <label id="lblTInctvs">
                                    </label>
                                </td>

                                <td>
                                    <label id="lblTLeaves">
                                    </label>
                                </td>
                                <%--<td>
                                        <label id="lblTExtra">
                                        </label>
                                    </td>--%>
                            </tr>
                        </tfoot>
                    </table>
                </div>
                <div id="divAttendanceGrid" class="row">
                    <div>
                        <table id="tblattendancegrid" class="table table-striped table-bordered table-condensed table-hover">
                            <thead>
                                <tr id="trAddData" runat="server" data-emp-id='' data-emp-desg='' data-emp-name="" class="active">
                                    <td>
                                        <input id="txtEmpId" class="form-control" runat="server" placeholder="Employee Id" />
                                    </td>
                                    <td>
                                        <input id="txtEmpName" class="form-control" runat="server" placeholder="Employee Name" />
                                    </td>
                                    <td>
                                        <select id="ddlEmpDesg" runat="server" class="form-control emp-ddl" style="width: 150px;">
                                        </select>
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-nod" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-ot" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-wo" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-nhs" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt line-cal" id="txt-add-npots" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-canadv" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-pen" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-inctvs" value="0" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control num-txt" id="txt-add-Leaves" value="0" />
                                    </td>
                                    <%-- <td>
                                            <input type="text" class="form-control num-txt" id="txt-add-Extra" value="0" />
                                        </td>--%>
                                    <td rowspan="2"></td>
                                    <td rowspan="2">
                                        <button class="btn btn-primary" onclick="AddNewEmp(this);return false;" style="height: 60px;">
                                            <i class="glyphicon glyphicon-plus"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr class="active">
                                    <td>
                                        <input type="text" class="form-control txt-calender" id="txtJoingingDate" placeholder="JoiningDate" />
                                    </td>
                                    <td>
                                        <input type="text" class="form-control txt-calender" id="txtRelievingDate" placeholder="RevlievingDate" />
                                    </td>
                                    <td>
                                        <select id="ddlTransfertype" class="form-control" runat="server" style="width: 150px;">
                                            <option value="1">PostingOrder</option>
                                            <option value="0" selected="selected">Temporary Transfer</option>
                                            <option value="-1">Dumy Transfer</option>
                                        </select>
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkESI" checked="checked" />
                                        &nbsp; ESI
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkPF" checked="checked" />
                                        &nbsp; PF
                                    </td>
                                    <td>
                                        <input type="checkbox" id="chkPT" checked="checked" />
                                        &nbsp; PT
                                    </td>
                                    <td>
                                        <input type="text" class="form-control txt-noofdays" id="lblnoofdays" style="visibility: hidden" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <%-- <td></td>--%>
                                </tr>
                                <tr class="warning">
                                    <th>Emp Id
                                    </th>
                                    <th>Emp Name
                                    </th>
                                    <th>Emp Designation
                                    </th>
                                    <th>NoOfDuties
                                    </th>
                                    <th>OT's
                                    </th>
                                    <th>WO's
                                    </th>
                                    <th>NHS
                                    </th>
                                    <th>Npots
                                    </th>
                                    <th>CanteenAdv
                                    </th>
                                    <th>Rent
                                    </th>
                                    <th>Incentives
                                    </th>
                                    <th>Leaves
                                    </th>
                                    <%-- <th>Extra
                                        </th>--%>
                                    <th>Totals
                                    </th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                            <tfoot class="active">
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th>
                                        <label>
                                            Totals :</label>
                                    </th>
                                    <th>
                                        <label id="lblNOD">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblOT">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblWO">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblNHS">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblNpots">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblCanAdv">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblPen">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblInctvs">
                                        </label>
                                    </th>
                                    <th>
                                        <label id="lblLeaves">
                                        </label>
                                    </th>
                                    <%-- <th>
                                            <label id="lblExtra">
                                            </label>
                                        </th>--%>
                                    <th>
                                        <label id="lblTTotals">
                                        </label>
                                    </th>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-md-10">
                        </div>
                        <div class="col-md-1">
                            <button type='button' id="btnSave" class="btn btn-success" onclick="SaveAttendance();return false;">
                                Save</button>
                        </div>
                        <div class="col-md-1">
                            <button type='button' id="btnCancel" class="btn btn-default" onclick="Cancel();return false;">
                                Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="fade">
    </div>
    <div id="modal">
        <img id="loader" src="../css/ajax-loader.gif" />
    </div>

</asp:Content>
