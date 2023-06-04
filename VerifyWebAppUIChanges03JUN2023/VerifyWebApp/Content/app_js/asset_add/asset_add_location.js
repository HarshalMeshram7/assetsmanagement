
/*
    Asset Add Screen JS
 */



function InitLocation() {
    $('#txtlocA').change(function () {

        console.log(" txtlocA change fired ");

        let index = $("#txtlocA option:selected").val();

        getlocationb(index);
    });

    $('#txtlocB').change(function () {

        let index = $("#txtlocB option:selected").val();


        // $('#txtassetno').val(index);
        getlocationc(index);
    });

    ///on edit modal location a select load location b dropdown
    $('#txteditlocA').change(function () {


        let index = $("#txteditlocA option:selected").val();


        //  $('#txtassetno').val(index);
        console.log("mm inddex " + index);
        if (typeof index != "undefined") {
            console.log('-- fetch location b');
            geteditlocationb(index);
        }


    });

    $('#txteditlocB').change(function () {

        let index = $("#txteditlocB option:selected").val();


        // $('#txtassetno').val(index);
        geteditlocationc(index);
    });
}

function test(aa) {
    console.log("Test function");
    console.log(aa);
  //  console.log(isimportedflag);
}

/* Location Tab */

function ShowAddLocationModal() {
    $('#locationmodal').modal('show');
}

function closeeditloc() {
    $('#editlocationmodal').modal('hide');
}


function closeloc() {
    $('#locationmodal').modal('hide');
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
        $("#locdateerror").hide();
        //  var assetamt = $("#txtassetamount").val()
        $('#tbllocation').last().append(addLocationRow(srnocnt, alocid, blocid, clocid, date, alocname, blocname, clocname));
      
        srnocnt++;
    }
}

function validlocationdate() {
    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var locdate = moment($("#txtlocDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');


    if (new Date(vdate) > new Date(locdate)) {

        //$("#locdateerror").html(" * location Date must be greater than Voucher Date");
        //$("#locdateerror").show();
        // swal("Alert", "location Date must be greater than Voucher Date", "warning");
        return true;
    }
    else {
        return false;

    }
}

function addLocationRow(srnocnt, alocid, blocid, clocid, date
    , alocname, blocname, clocname) {

    let A_EditButton = '<button type="button" class="btn modBtn btn-sm mx-1" onclick=Editloc(this,' + srnocnt + ');>';
    A_EditButton = A_EditButton + '<i class="fa fa-edit"></i></button>';


    let A_Delete_Link = '<button type="button" class="btn delBtn btn-sm mx-1" onclick=deleteloc(this);>';
    A_Delete_Link = A_Delete_Link + '<i class="fa fa-trash"></i></button> ';

    let link = "";

   

    let td_1 = '<td class="align-center text-center text-xs">' + srnocnt + '</td>';
    let td_2 = '<td hidden>' + alocid + '</td>';
    let td_3 = '<td hidden>' + blocid + '</td>';
    let td_4 = '<td hidden>' + clocid + '</td>';

    let td_5 = '<td class="align-center text-center text-xs">' + date + '</td>';
    let td_6 = '<td class="align-start text-start text-xs">' + alocname + '</td>';
    let td_7 = '<td class="align-start text-start text-xs">' + blocname + '</td>';
    let td_8 = '<td class="align-start text-start text-xs">' + clocname + '</td>';
    let td_9 = '<td class="align-center text-center text-xs " >' + A_EditButton + A_Delete_Link + '</td>';


    let tr_row = "<tr>" + td_1 + td_2 + td_3 + td_4 + td_5 + td_6 + td_7 + td_8 + td_9;
    return tr_row;

}

function deleteloc(obj) {
    $(obj).parent().parent().remove();
}


function getlocationb(id) {
    if (id == "") {
        id = 0;
        var index = $("#txtlocB option:selected").val(0);
    }
    else {
        var ControllerURL = URL_Get_LocationB;

        var url = ControllerURL + "/" + id;
        var procemessage = "<option value='0'> Please wait...</option>";
        $("txtlocB").html(procemessage).show();


        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Location</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txtlocB").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}

function getlocationc(id) {
    if (id == "") {
        id = 0;

        var index = $("#txtlocC option:selected").val(0);
    }
    else {
        let ControllerURL = URL_Get_LocationC;
        let url = ControllerURL + "/" + id;
        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Location</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txtlocC").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }

}


// Edit functionality js functions 

function Editloc(id, editcnt) {
    //$(id).parent();

    editindex = editcnt;
    var tr = $(id).parent().parent();

    console.log($(tr).innerHTML);

    var locaid = $(tr).find("td").eq(1).html();
    var locbid = $(tr).find("td").eq(2).html();
    var loccid = $(tr).find("td").eq(3).html();
    var date = $(tr).find("td").eq(4).html();



    //console.log("edit loc" + locaid);
    //console.log(tr);

    $("#txteditlocA").val(locaid);


    $('#txteditlocB').val(locbid);
    $('#txteditlocC').val(loccid);
    $('#txteditlocDate').val(date);

    if (typeof locaid != "undefined") {
        geteditlocationb2(locaid, locbid, loccid)
    }


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

function geteditlocationb(id) {
    if (id == "") {
        id = 0;

        var index = $("#txteditlocB option:selected").val(0);
    }
    else {

        var ControllerURL = URL_Get_LocationB; // '@Url.Action("getlocationb", "Asset")';
        var url = ControllerURL + "/" + id;
        var procemessage = "<option value='0'> Please wait...</option>";
        $("txteditlocB").html(procemessage).show();
        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select location</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txteditlocB").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}

function geteditlocationc(id) {
    if (id == "") {
        id = 0;

        var index = $("#txteditlocC option:selected").val(0);
    }
    else {
        var ControllerURL = URL_Get_LocationC //  '@Url.Action("getlocationc", "Asset")';
        var url = ControllerURL + "/" + id;
        $.ajax({
            url: url,

            cache: false,
            type: "POST",
            success: function (data) {
                var markup = "<option value='0'>Select Location</option>";
                for (var x = 0; x < data.length; x++) {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $("#txteditlocC").html(markup).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }

}

function editlocationdate() {

    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var locdate = moment($("#txteditlocDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(locdate)) {
        //$("#editlocdateerror").html(" * This Date must be greater than Voucher Date");
        //$("#editlocdateerror").show();
        return true;
    }
    else {
        return false;
        // $("#editlocdateerror").hide();

    }
}


//function geteditlocationb2(locaid, locbid, loccid) {
//    if (locaid == "") {
//        locaid = 0;

//        var index = $("#txteditlocB option:selected").val(0);
//    }
//    else {
//        var ControllerURL = '@Url.Action("getlocationb", "Asset")';
//        var url = ControllerURL + "/" + locaid;
//        var procemessage = "<option value='0'> Please wait...</option>";
//        $("txteditlocB").html(procemessage).show();
//        $.ajax({
//            url: url,
//            cache: false,
//            type: "POST",
//            success: function (data) {
//                var markup = "<option value='0'>Select location</option>";
//                for (var x = 0; x < data.length; x++) {
//                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
//                }
//                $("#txteditlocB").html(markup).show();
//                $("#txteditlocB ").val(locbid);
//                geteditlocationc2(locbid, loccid)
//            },
//            error: function (reponse) {
//                alert("error : " + reponse);
//            }
//        });
//    }
//}


//function geteditlocationc2(locbid, loccid) {
//    if (locbid == "") {
//        locbid = 0;

//        var index = $("#txteditlocC option:selected").val(0);
//    }
//    else {
//        var ControllerURL = '@Url.Action("getlocationc", "Asset")';
//        var url = ControllerURL + "/" + locbid;
//        $.ajax({
//            url: url,

//            cache: false,
//            type: "POST",
//            success: function (data) {
//                var markup = "<option value='0'>Select Location</option>";
//                for (var x = 0; x < data.length; x++) {
//                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
//                }
//                $("#txteditlocC").html(markup).show();
//                $("#txteditlocC").val(loccid);
//            },
//            error: function (reponse) {
//                alert("error : " + reponse);
//            }
//        });
//    }

//}