
function closeassetf() {
    $('#assetfreeofcostmodal').modal('hide');
}


function deleteassetf(obj) {
    $(obj).parent().parent().remove();
}

function saveassetf() {

    if ($("#txtassetfDate").val() == '') {
        $("#assetfdateemerror").html(" * Please select date");
        $("#assetfdateemerror").show();

    }
    if ($("#txtdescription").val() == '') {
        $("#assetfdescriptionerror").html(" * Enter description");
        $("#assetfdescriptionerror").show();

    }


    // $('#myModal').modal('hide');
    if ($("#txtassetfDate").val() != '' &&
        $("#txtdescription").val() != '') {

        Addassetftotable();



        $('#assetfreeofcostmodal').modal('hide');


    }
}

function addfreecostrow(srnoassetfcnt, uomid, description, date, qty, uomname) {
    let A_EditButton = '<button type="button" class="btn modBtn btn-sm mx-1" onclick=Editassetf(this,' + srnoassetfcnt + ');>';
    A_EditButton = A_EditButton + '<i class="fa fa-edit"></i></button>';


    let A_Delete_Link = '<button type="button" class="btn delBtn btn-sm mx-1" onclick=deleteassetf(this);>';
    A_Delete_Link = A_Delete_Link + '<i class="fa fa-trash"></i></button> ';



    let td_1 = '<td class="align-center text-center text-xs">' + srnoassetfcnt + '</td>';
    let td_2 = '<td hidden>' + uomid + '</td>';

    let td_3 = '<td class="align-center text-center text-xs">' + description + '</td>';
    let td_4 = '<td class="align-start text-start text-xs">' + date + '</td>';
    let td_5 = '<td class="align-start text-start text-xs">' + qty + '</td>';
    let td_6 = '<td class="align-start text-start text-xs">' + uomname + '</td>';
    let td_7 = '<td class="align-center text-center text-xs " >' + A_EditButton + A_Delete_Link + '</td>';


    let tr_row = "<tr>" + td_1 + td_2 + td_3 + td_4 + td_5 + td_6 + td_7;
    return tr_row;

}

function Addassetftotable() {

    var uomname = $("#txtassetfuom").find("option:selected").text();
    var uomid = $("#txtassetfuom").find("option:selected").val();
    var date = $("#txtassetfDate").val()
    //   alert(date)

    var description = $("#txtdescription").val();
    var qty = $("#txtassetfqty").val();


    if (uomname == "Select Uom") {
        uomname = "";
    }

    var validdate = assetfdate();
    if (validdate == true) {
        swal("Alert", "This Date must be greater than Voucher Date", "warning");
    } else {
        $('#tblassetfreeofcost').last().append(addfreecostrow(srnoassetfcnt, uomid, description, date, uomname));
        srnoassetfcnt++;
    }
}


function closeeditassetf() {
    $('#editccmodal').modal('hide');
}

function Editassetf(id, editcnt) {
    //$(id).parent();
    editassetfindex = editcnt;
    var tr = $(id).parent().parent();
    var oumid = $(tr).find("td").eq(1).html();
    var description = $(tr).find("td").eq(2).html();
    var date = $(tr).find("td").eq(3).html();
    var qty = $(tr).find("td").eq(4).html();


    $("#txteditassetfuom").val(oumid);

    // $("#txtedassetno").prop('selectedIndex', assetno)
    $('#txteditdescription').val(description);
    $('#txteditassetfqty').val(qty);
    $('#txteditassetfDate').val(date);


    $('#editassetfreeofcostmodal').modal('show');
}

function saveeditassetf() {
    if ($("#txteditassetfDate").val() == '') {
        $("#editassetfdateemerror").html(" * Please select date");
        $("#editassetfdateemerror").show();

    }
    if ($("#txteditdescription").val() == '') {
        $("#editassetfdescriptionerror").html(" * Enter description");
        $("#editassetfdescriptionerror").show();

    }


    // $('#myModal').modal('hide');
    if ($("#txteditassetfDate").val() != '' &&
        $("#txteditdescription").val() != '') {

        editassetftotable();

        $('#editassetfreeofcostmodal').modal('hide');

    }
}

function editassetftotable() {


    var uomname = $("#txteditassetfuom").find("option:selected").text();
    var uomid = $("#txteditassetfuom").find("option:selected").val();
    var date = $("#txteditassetfDate").val();


    var qty = $("#txteditassetfqty").val();
    var description = $("#txteditdescription").val();

    var date = $("#txteditassetfDate").val();


    if (uomname == "Select Uom") {
        uomname = "";
    }

    var validdate = editassetfdate();
    // alert(validdate);
    if (validdate == true) {
        swal("Alert", "This Date must be greater than Voucher Date", "warning");

    } else {
        var tbl = $('#tblassetfreeofcost tbody tr:has(td)').each(function () {
            var srno = $(this).find("td").eq(0).html();
            if (srno == editassetfindex) {
                //alert('check');
                $(this).find("td").eq(1).html(uomid);
                $(this).find("td").eq(2).html(description);
                $(this).find("td").eq(3).html(date);
                $(this).find("td").eq(4).html(qty);
                $(this).find("td").eq(5).html(uomname);


            }
        })

    }
}

function assetfdate() {
    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    //  alert(vdate)
    var adate = moment($("#txtassetfDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    //alert(adate)
    if (new Date(vdate) > new Date(adate)) {
        //$("#assetfdateerror").html(" * This Date must be greater than Voucher Date");
        //$("#assetfdateerror").show();
        return true;
    }
    else {
        return false;
        //$("#assetfdateerror").hide();
    }
}

function editassetfdate() {
    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var adate = moment($("#txteditassetfDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(adate)) {
        //$("#editassetfdateerror").html(" * This Date must be greater than Voucher Date");
        //$("#editassetfdateerror").show();
        return true;
    }
    else {
        // $("#editassetfdateerror").hide();
        return false;
    }
}
