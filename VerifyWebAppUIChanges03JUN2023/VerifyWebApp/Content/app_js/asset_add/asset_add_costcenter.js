/*
    Functions related to cost center for asset add screen
 */



function ShowAddCostCenterModal() {
    $("#txtccA").val("");
    $("#txtccB").val("");
    $("#txtpercentage").val("")
    $("#txtccDate").val("");

    $('#ccmodal').modal('show');
}



function getcostcenterb(id) {
    if (id == "") {
        id = 0;

        var index = $("#txtccB option:selected").val(0);
    }
    else {
        var ControllerURL = URL_Get_CostCenterB;// '@Url.Action("getcostcenterb", "Asset")';
        var url = ControllerURL + "/" + id;
        var procemessage = "<option value='0'> Please wait...</option>";
        $("txtccB").html(procemessage).show();


        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Cost Center</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txtccB").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}

function costcenterdate() {

    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var ccdate = moment($("#txtccDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(ccdate)) {
        //$("#ccdateerror").html(" * This Date must be greater than Voucher Date");
        //$("#ccdateerror").show();
        return true;
    }
    else {
        //$("#ccdateerror").hide();
        return false;
    }
}


function Addcctotable() {

    var accname = $("#txtccA").find("option:selected").text();
    var bccname = $("#txtccB").find("option:selected").text();
    var percentage = $("#txtpercentage").val()


    var accid = $("#txtccA option:selected").val();
    var bccid = $("#txtccB option:selected").val();

    var date = $("#txtccDate").val();
    if (accname == "Select Cost Center") {
        accname = "";
    }
    if (bccname == "Select Cost Center") {
        bccname = ""
    }
    var validdate = costcenterdate();
    // alert(validdate);
    if (validdate == true) {
        swal("Alert", "This Date must be greater than Voucher Date", "warning");
    } else {


        $('#tblcostcenter').last().append(addcostcenterrow(srnocccnt, accid, bccid, date, accname, bccname, percentage));

        srnocccnt++;
    }
}

function addcostcenterrow(srnocccnt, accid, bccid, date, accname, bccname, percentage) {

    let A_EditButton = '<button type="button" class="btn modBtn btn-sm mx-1" onclick=Editcc(this,' + srnocnt + ');>';
    A_EditButton = A_EditButton + '<i class="fa fa-edit"></i></button>';


    let A_Delete_Link = '<button type="button" class="btn delBtn btn-sm mx-1" onclick=deletecc(this);>';
    A_Delete_Link = A_Delete_Link + '<i class="fa fa-trash"></i></button> ';



    let td_1 = '<td class="align-center text-center text-xs">' + srnocccnt + '</td>';
    let td_2 = '<td hidden>' + accid + '</td>';
    let td_3 = '<td hidden>' + bccid + '</td>';

    let td_4 = '<td class="align-center text-center text-xs">' + date + '</td>';
    let td_5 = '<td class="align-start text-start text-xs">' + accname + '</td>';
    let td_6 = '<td class="align-start text-start text-xs">' + bccname + '</td>';
    let td_7 = '<td class="align-start text-start text-xs">' + percentage + '</td>';

    let td_8 = '<td class="align-center text-center text-xs " >' + A_EditButton + A_Delete_Link + '</td>';


    let tr_row = "<tr>" + td_1 + td_2 + td_3 + td_4 + td_5 + td_6 + td_7 + td_8;
    return tr_row;

}

function closecc() {
    $('#ccmodal').modal('hide');
}


function deletecc(obj) {
    $(obj).parent().parent().remove();
}

function savecc() {
    if ($("#txtccDate").val() == '') {
        $("#ccdateemerror").html(" * Please select date");
        $("#ccdateemerror").show();

    }
    if ($("#txtccA").find("option:selected").text() == 'Select Cost Center') {
        $("#ccaerror").html(" * Please select cost center");
        $("#ccaerror").show();

    }

    if ($("#txtpercentage").val() == '') {
        $("#ccpercentageerror").html(" * Enter percentage");
        $("#ccpercentageerror").show();

    }
    var checkpercentage = $("#txtpercentage").val();
    //  alert(checkpercentage)
    if (checkpercentage > 100) {
        // alert("if")
        //   swal("Alert", "Percentage cannot exceed 100", "warning");
        $("#ccpercentageerror").html(" * Percentage cannot exceed 100");
        $("#ccpercentageerror").show();
        return;
    }

    // $('#myModal').modal('hide');
    if ($("#txtccA").find("option:selected").text() != 'Select Cost Center' &&
        $("#txtccDate").val() != '' && $("#txtpercentage").val() != '') {

        Addcctotable();



        $('#ccmodal').modal('hide');


    }
}


// edit functions

function Editcc(id, editcnt) {
    //$(id).parent();
    editccindex = editcnt;
    var tr = $(id).parent().parent();
    var ccaid = $(tr).find("td").eq(1).html();
    var ccbid = $(tr).find("td").eq(2).html();
    var date = $(tr).find("td").eq(3).html();
    var percentage = $(tr).find("td").eq(6).html();

    $("#txteditccA").val(ccaid);

    // $("#txtedassetno").prop('selectedIndex', assetno)
    $('#txteditccB').val(ccbid);
    $('#txteditpercentage').val(percentage);
    $('#txteditccDate').val(date);

    geteditcostcenterb2(ccaid, ccbid)
    $('#editccmodal').modal('show');
}

function saveeditcc() {
    if ($("#txteditccA").find("option:selected").text() == 'Select Cost Center') {
        $("#editccaerror").html(" * Please select cost center");
        $("#editccaerror").show();

    }
    if ($("#txteditccDate").val() == '') {
        $("#editccdateemerror").html(" * Please select date");
        $("#editccdateemerror").show();

    }
    if ($("#txteditpercentage").val() == '') {
        $("#editccpercentageerror").html(" * Enter percentage");
        $("#editccpercentageerror").show();

    }
    var checkpercentage = $("#txteditpercentage").val();
    if (checkpercentage > 100) {
        $("#editccpercentageerror").html(" * Percentage cannot exceed 100");
        $("#editccpercentageerror").show();
        return;
    }

    // $('#myModal').modal('hide');
    if ($("#txteditccA").find("option:selected").text() != 'Select Cost Center' &&
        $("#txteditpercentage").val() != '' && $("#txteditccDate").val() != '') {

        editcctotable();



        $('#editccmodal').modal('hide');

    }
}

function editcctotable() {
    var accname = $("#txteditccA").find("option:selected").text();
    var bccname = $("#txteditccB").find("option:selected").text();
    var percentage = $("#txteditpercentage").val();


    var accid = $("#txteditccA option:selected").val();
    var bccid = $("#txteditccB option:selected").val();

    var date = $("#txteditccDate").val();
    if (accname == "Select Cost Center") {
        accname = "";
    }
    if (bccname == "Select Cost Center") {
        bccname = ""
    }
    var validdate = editcostcenterdate();
    // alert(validdate);
    if (validdate == true) {
        swal("Alert", "This Date must be greater than Voucher Date", "warning");

    } else {
        var tbl = $('#tblcostcenter tbody tr:has(td)').each(function () {
            var srno = $(this).find("td").eq(0).html();
            if (srno == editccindex) {
                //alert('check');
                $(this).find("td").eq(1).html(accid);
                $(this).find("td").eq(2).html(bccid);
                $(this).find("td").eq(3).html(date);
                $(this).find("td").eq(4).html(accname);
                $(this).find("td").eq(5).html(bccname);
                $(this).find("td").eq(6).html(percentage);

            }
        })

    }
}

function geteditcostcenterb(id) {
    if (id == "") {
        id = 0;

        var index = $("#txteditccB option:selected").val(0);
    }
    else {
        var ControllerURL = URL_Get_CostCenterB; //'@Url.Action("getcostcenterb", "Asset")';
        var url = ControllerURL + "/" + id;
        var procemessage = "<option value='0'> Please wait...</option>";
        $("txteditccB").html(procemessage).show();


        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Cost Center</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txteditccB").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}



function editcostcenterdate() {

    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var ccdate = moment($("#txteditccDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(ccdate)) {
        //$("#editccdateerror").html(" * This Date must be greater than Voucher Date");
        //$("#editccdateerror").show();
        return true;
    }
    else {
        //$("#editccdateerror").hide();
        return false;
    }
}

function closeeditcc() {
    $('#editccmodal').modal('hide');
}

function geteditcostcenterb2(ccaid, ccbid) {
    if (ccaid == "") {
        ccaid = 0;

        var index = $("#txteditccB option:selected").val(0);
    }
    else {
        var ControllerURL = URL_Get_CostCenterB; //'@Url.Action("getcostcenterb", "Asset")';
        var url = ControllerURL + "/" + ccaid;
        var procemessage = "<option value='0'> Please wait...</option>";
        $("txteditccB").html(procemessage).show();


        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Cost Center</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txteditccB").html(markup).show();
                $("#txteditccB ").val(ccbid);

            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}