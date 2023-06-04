function closeloc() {
    $('#locationmodal').modal('hide');
}


function deleteloc(obj) {
    $(obj).parent().parent().remove();
}
function saveloc() {

    if ($("#txtlocDate").val() == '') {
        alert("test")
        $("#locdateemerror").html(" * Please select date");
        $("#locdateemerror").show();

    }

    if ($("#txtlocA").find("option:selected").text() == 'Select Location') {
        $("#locaerror").html(" * Please select location");
        $("#locaerror").show();

    }

    if ($("#txtlocA").find("option:selected").text() != 'Select Location'
        && $("#txtlocDate").val() != '') {

        Addlocationtotable();



        $('#locationmodal').modal('hide');


    }
}


function Addlocationtotable() {

    var alocname = $("#txtlocA").find("option:selected").text();
    var blocname = $("#txtlocB").find("option:selected").text();
    var clocname = $("#txtlocC").find("option:selected").text();

    var alocids = $("#txtlocA").val();
    var alocid = $("#txtlocA option:selected").val();
    var blocid = $("#txtlocB option:selected").val();
    var clocid = $("#txtlocC option:selected").val();
    var date = $("#txtlocDate").val();
    if (alocname == "Select Location") {
        alocname = "";
    }
    if (blocname == "Select Location") {
        blocname = ""
    }
    if (clocname == "Select Location") {
        clocname = ""
    }
    var validdate = validlocationdate();
    // alert(validdate);
    if (validdate == true) {
        swal("Alert", "location Date must be greater than Voucher Date", "warning");

    } else {
        //  var assetamt = $("#txtassetamount").val()
        $('#tbllocation').last().append('<tr><td>' + srnocnt + '</td><td hidden>' + alocid + '</td>'
            + '<td hidden>' + blocid + '</td ><td hidden>' + clocid + '</td><td>' + date + '</td>'
            + '<td>' + alocname + '</td ><td>' + blocname + '</td><td>' + clocname + '</td><td> <button type="button" class="btn modBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="Editloc(this,' + srnocnt + ');">'
            + '  <i class="fa fa-edit"></i></button></td><td> <button type="button" class="btn delBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="deleteloc(this);">'
            + '  <i class="fa fa-edit"></i></button></td></tr > ');

        srnocnt++;
    }
}

function closeeditloc() {
    $('#editlocationmodal').modal('hide');
}


function Editloc(id, editcnt) {
    //$(id).parent();
    // alert("editloc")
    editindex = editcnt;
    var tr = $(id).parent().parent();
    var locaid = $(tr).find("td").eq(1).html();
    var locbid = $(tr).find("td").eq(2).html();
    var loccid = $(tr).find("td").eq(3).html();
    var date = $(tr).find("td").eq(4).html();

    $("#txteditlocA").val(locaid);

    // $("#txtedassetno").prop('selectedIndex', assetno)
    $('#txteditlocB').val(locbid);
    $('#txteditlocC').val(loccid);
    $('#txteditlocDate').val(date);

    geteditlocationb2(locaid, locbid, loccid)
    $('#editlocationmodal').modal('show');
}

function saveeditloc() {

    if ($("#txteditlocDate").val() == '') {
        $("#editlocdateemerror").html(" * Please select date");
        $("#editlocdateemerror").show();

    }
    if ($("#txteditlocA").find("option:selected").text() == 'Select Location') {
        $("#editlocaerror").html(" * Please select location");
        $("#editlocaerror").show();

    }

    if ($("#txteditlocA").find("option:selected").text() != 'Select Location' &&
        $("#txteditlocDate").val() != '') {

        editlocationtotable();



        $('#editlocationmodal').modal('hide');

    }
}

function editlocationtotable() {
    var alocname = $("#txteditlocA").find("option:selected").text();
    var blocname = $("#txteditlocB").find("option:selected").text();
    var clocname = $("#txteditlocC").find("option:selected").text();
    //  alert(blocname)
    //alert(clocname)

    var alocid = $("#txteditlocA option:selected").val();
    var blocid = $("#txteditlocB option:selected").val();
    var clocid = $("#txteditlocC option:selected").val();
    var date = $("#txteditlocDate").val();
    if (alocname == "Select Location") {
        alocname = "";
    }
    if (blocname == "Select Location") {
        blocname = ""
    }
    if (clocname == "Select Location") {
        clocname = ""
    }
    var validdate = editlocationdate();
    // alert(validdate);
    if (validdate == true) {
        swal("Alert", "location Date must be greater than Voucher Date", "warning");

    } else {
        var tbl = $('#tbllocation tbody tr:has(td)').each(function () {
            var srno = $(this).find("td").eq(0).html();
            if (srno == editindex) {
                //alert('check');
                $(this).find("td").eq(1).html(alocid);
                $(this).find("td").eq(2).html(blocid);
                $(this).find("td").eq(3).html(clocid);
                $(this).find("td").eq(4).html(date);
                $(this).find("td").eq(5).html(alocname);
                $(this).find("td").eq(6).html(blocname);
                $(this).find("td").eq(7).html(clocname);
            }
        })
    }
}