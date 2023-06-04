function closeeditassetf() {
    $('#editassetfreeofcostmodal').modal('hide');
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
        swal("Alert", "location Date must be greater than Voucher Date", "warning");
        //$("#locdateerror").html(" * location Date must be greater than Voucher Date");
        //$("#locdateerror").show();
        //$('#locationmodal').modal('show');

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

function Addassetftotable() {

    var uomname = $("#txtassetfuom").find("option:selected").text();
    var uomid = $("#txtassetfuom").find("option:selected").val();
    var date = $("#txtassetfDate").val()
    // alert(date)

    var description = $("#txtdescription").val();
    var qty = $("#txtassetfqty").val();


    if (uomname == "Select Uom") {
        uomname = "";
    }


    var validdate = assetfdate();
    //  alert(validdate);
    if (validdate == true) {
        swal("Alert", "location Date must be greater than Voucher Date", "warning");
        //$("#locdateerror").html(" * location Date must be greater than Voucher Date");
        //$("#locdateerror").show();
        //$('#locationmodal').modal('show');

    } else {
        //  var assetamt = $("#txtassetamount").val()

        $('#tblassetfreeofcost').last().append('<tr><td>' + srnoassetfcnt + '</td><td hidden>' + uomid + '</td>'

            + '<td>' + description + '</td ><td>' + date + '</td><td>' + qty + '</td><td>' + uomname + '</td><td> <button type="button" class="btn modBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="Editassetf(this,' + srnoassetfcnt + ');">'
            + '  <i class="fa fa-edit"></i></button></td><td> <button type="button" class="btn delBtn btn-sm"'
            + ' aria-label="Left Align"'
            + 'onclick="deleteassetf(this);">'
            + '  <i class="fa fa-edit"></i></button></td></tr > ');

        srnoassetfcnt++;
    }
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