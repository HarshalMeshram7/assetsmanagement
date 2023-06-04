
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
        swal("Alert", "location Date must be greater than Voucher Date", "warning");

    } else {
        //  var assetamt = $("#txtassetamount").val()
        $('#tblcostcenter').last().append('<tr><td>' + srnocccnt + '</td><td hidden>' + accid + '</td>'
            + '<td hidden>' + bccid + '<td>' + date + '</td>'
            + '<td>' + accname + '</td ><td>' + bccname + '</td><td>' + percentage + '</td><td> <button type="button" class="btn modBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="Editcc(this,' + srnocccnt + ');">'
            + '  <i class="fa fa-edit"></i></button></td><td> <button type="button" class="btn delBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="deletecc(this);">'
            + '  <i class="fa fa-edit"></i></button></td></tr > ');

        srnocccnt++;
    }
}

function costcenterdate() {

    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var ccdate = moment($("#txtccDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(ccdate)) {
        return true;
    }
    else {
        return false;
        // $("#ccdateerror").hide();

    }
}

function editcostcenterdate() {


    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var ccdate = moment($("#txteditccDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(ccdate)) {
        return true;
    }
    else {
        return false;
        // $("#editccdateerror").hide();

    }


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

function closeeditcc() {
    $('#editccmodal').modal('hide');
}

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
        swal("Alert", "location Date must be greater than Voucher Date", "warning");
        //$("#locdateerror").html(" * location Date must be greater than Voucher Date");
        //$("#locdateerror").show();
        //$('#locationmodal').modal('show');

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

