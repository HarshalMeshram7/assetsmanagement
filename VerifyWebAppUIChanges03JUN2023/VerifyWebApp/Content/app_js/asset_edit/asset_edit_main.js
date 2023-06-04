function validateform() {
    var errorlist = [];


    var VoucherDate = $('#txtVDate').val();
    var DtPutToUse = $('#txtdpucomDate').val();
    var ITGroupIDID = $('#txtItgroupName').val();
    // var itgroup = $("#txtItgroupName").find("option:selected").val();
    var UsefulLife = $("#txtuseful").val();
    var AssetName = $('#assetname').val();
    var assetidno = $("#assetidno").val();
    var grossvalue = $("#txtgrossvalue").val();
    var Qty = $('#txtqty').val();
    var DtPutToUseIT = $('#txtdpuItDate').val();
    var assetno = $('#txtassetno').val();
    if (assetno == "") {
        errorlist.push("Assetno   is not entered");
    }
    if (Qty == "") {
        errorlist.push("Quantity   is not entered");
    }
    if (grossvalue == "") {
        errorlist.push("Gross value  is not entered");
    }
    if (DtPutToUseIT == "") {
        errorlist.push("Date put to use (IT) is not entered");
    }
    //if (assetidno == "") {
    //    errorlist.push("Asset Identification No  is not entered");
    //}
    //if (ITGroupIDID == "") {
    //    errorlist.push("Itgroup  is not Selected");
    //}
    if (UsefulLife == "") {
        errorlist.push("Usefullife is not entered");
    }

    if (AssetName == "") {
        errorlist.push("Asset Name is not entered");
    }
    if (VoucherDate == "") {
        errorlist.push("Voucher date is not entered");
    }
    if (DtPutToUse == "") {
        errorlist.push("Date put to use company is not entered");
    }
    //  alert(errorlist)
    return errorlist;
}


function SaveData() {

    var validateformz = validateform();

    if (validateformz.length != 0) {

        var srno = 1;

        for (var i = 0; i < validateformz.length; i++) {

            $('#errtbl').last().append('<tr><td>' + srno + '</td><td>' + validateformz[i] + '</td></tr>');
            srno++
        }

        $('#errorlist').modal('show');
    }
    else {
        var tbllocation = $('#tbllocation tbody tr:has(td)').map(function (i, v) {
            var $td = $('td', this);
            var Tablelocationa = $td.eq(1).html();
            var Tablelocationb = $td.eq(2).html();
            var Tablelocationc = $td.eq(3).html();
            var Tabledate = $td.eq(4).html();
            Tabledate = moment(Tabledate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            return {
                Date: Tabledate,
                ALocID: Tablelocationa,
                BLocID: Tablelocationb,
                CLocID: Tablelocationc
            }
        }).get();

        var tblcostcenter = $('#tblcostcenter tbody tr:has(td)').map(function (i, v) {
            var $td = $('td', this);
            var Tabledate = $td.eq(3).html();
            Tabledate = moment(Tabledate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            var Tablecosta = $td.eq(1).html();
            var Tablecostb = $td.eq(2).html();
            var Tablepercentage = $td.eq(6).html();
            return {
                Date: Tabledate,
                AcostcenterID: Tablecosta,
                BcostcenterID: Tablecostb,
                Percentage: Tablepercentage
            }
        }).get();

        var tblassetfreeofcost = $('#tblassetfreeofcost tbody tr:has(td)').map(function (i, v) {
            var $td = $('td', this);
            var Tabledate = $td.eq(3).html();
            Tabledate = moment(Tabledate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            var Tabledesc = $td.eq(2).html();
            var Tableqty = $td.eq(4).html();
            var Tableuom = $td.eq(1).html();
            return {
                Date: Tabledate,
                Description: Tabledesc,
                Qty: Tableqty,
                Uom: Tableuom
            }
        }).get();
        var parentassetid = $("#Parent_AssetId").val();
        var depreciationmethod = $("input[name='DepreciationMethod']:checked").val();
        var purchaseaccid = $("#txtPAccName").find("option:selected").val();;
        var accumatedid = $("#txtAccDepAccName").find("option:selected").val();;
        var itgroup = $("#txtItgroupName").find("option:selected").val();
        var depid = $("#txtDepAccName").find("option:selected").val();
        var assetidno = $("#assetidno").val();
        var NRate = $("#txtNRate").val();
        var ARate = $("#txtARate").val();
        var TRate = $("#txtTRate").val();
        var UsefulLife = $("#txtuseful").val();
        var YrMang = $("#txtYrMang").val();
        // var ExpDate = $("#txtExpDate").val();
        var grossvalue = $("#txtgrossvalue").val();
        var servicecharge = $('#txtservicecharge').val();
        var otherexpense = $('#txtotherexpense').val();
        var customduty = $('#txtcustomduty').val();
        var exciseduty = $('#txtexciseduty').val();
        var servicetax = $('#txtservicetax').val();;
        var vat = $('#txtvat').val();;
        var anyotherduty = $('#txtanyotherduty').val();;
        var cst = $('#txtcst').val();;
        //var gst = $('#txtgst').val();
        var anyothertax = $('#txtanyothertax').val();
        var totaladdition = $('#txttotaladdition').val();
        var discount = $('#txtdiscount').val();
        var roundoff = $('#txtroundoff').val();
        var totaldeduction = $('#txttotaldeduction').val();
        var invoiceamt = $('#txtinvoiceamt').val();
        var dutydrawback = $('#txtdutydrawback').val();
        var excisecredit = $('#txtexcisecredit').val();
        var servicetaxcredit = $('#txtservicetaxcredit').val();
        var anyotherdutycredit = $('#txtanyotherdutycredit').val();
        var vatcredit = $('#txtvatcredit').val();
        var anyothercredit = $('#txtanyothercredit').val();
        var cstcredit = $('#txtcstcredit').val();
        //var gstcredit = $('#txtgstcredit').val();
        var cgstcredit = $('#txtcgstcredit').val();
        var igstcredit = $('#txtigstcredit').val();
        var sgstcredit = $('#txtsgstcredit').val();
        var cgst = $('#txtcgst').val();
        var igst = $('#txtigst').val();
        var sgst = $('#txtsgst').val();
        var totalcredit = $('#txttotalcredit').val();
        var amtcap = $('#txtamttobecap').val();
        var amtcapcompanylaw = $('#txtamtcapcompanylaw').val();
        var amtcapincometax = $('#txtamtcapincometax').val();

        ////var AssetNo = $('#txtassetno').val();
        ////var ClientID = $('#txtclientid').val();
        var AssetName = $('#assetname').val();
        ////var AdditionAssetName = $('#txtassdesc').val();
        var VoucherNo = $('#txtvno').val();
        var ExpDate = moment($("#txtExpDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var VoucherDate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var PODate = moment($("#txtPoDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var ReceiptDate = moment($("#txtreceiptDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var CommissioningDate = moment($("#txtcommDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var BillDate = moment($("#txtbillDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var DtPutToUse = moment($("#txtdpucomDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var DtPutToUseIT = moment($("#txtdpuItDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
        var PONo = $('#txtpono').val();
        var BillNo = $('#txtbillno').val();
        var MRRNo = $('#txtmrrno').val();
        var Qty = $('#txtqty').val();
        var SupplierNo = $('#SupplierId').val();
        var UOMNo = $('#UomId').val();
        var BrandName = $('#txtbrandname').val();
        var SrNo = $('#txtsrno').val();
        var Model = $('#txtmodel').val();
        var Remarks = $('#txtremarks').val();
        var IsImported = isimportedflag;
        var Currency = $('#txtcurrency').val();
        var Values = $('#txtvalue').val();
        var Residual = $('#txtresidual').val();
        var OPAccDepreciation = $('#opaccdepreciation').val();
        var ITGroupIDID = $('#txtItgroupName').val();
        var assetno = $('#txtassetno').val();
        //  alert(@Model.ID)
        var iscomponent = iscomponentflag;

        var postdata =
        {
            "iscomponent": iscomponent,
            "ID": '@Model.ID',
            "Parent_AssetId": parentassetid,
            "AssetNo": assetno,
            "ITGroupIDID": ITGroupIDID,
            //"DepreciationMethod": depreciationmethod,
            "AccountID": purchaseaccid,
            "DepAccountId": depid,
            "AccAccountID": accumatedid,

            "OPAccDepreciation": OPAccDepreciation,
            "DepreciationMethod": depreciationmethod,
            //"AccountID": "1",
            //"DepAccountId": "1",
            //"AccAccountID": "1",

            "locationlist": tbllocation,
            "costcenterlist": tblcostcenter,
            "assetfreeofcostlist": tblassetfreeofcost,

            "ResidualVal": Residual,
            "NormalRatae": NRate,
            "AdditionalRate": ARate,
            "TotalRate": TRate,
            "Usefullife": UsefulLife,
            "YrofManufacturing": YrMang,
            "ExpiryDate": ExpDate,
            "AssetName": AssetName,
            "VoucherNo": VoucherNo,
            "VoucherDate": VoucherDate,
            "PODate": PODate,
            "ReceiptDate": ReceiptDate,
            "CommissioningDate": CommissioningDate,
            "BillDate": BillDate,
            "DtPutToUse": DtPutToUse,
            "DtPutToUseIT": DtPutToUseIT,
            "PONo": PONo,
            "BillNo": BillNo,
            "Qty": Qty,
            "SupplierNo": SupplierNo,
            "BrandName": BrandName,
            "SrNo": SrNo,
            "Model": Model,
            "Remarks": Remarks,
            "IsImported": IsImported,
            "Currency": Currency,
            "Values": Values,
            "GrossVal": grossvalue,
            "ServiceCharges": servicecharge,
            "OtherExp": otherexpense,
            "CustomDuty": customduty,
            "ExciseDuty": exciseduty,
            "ServiceTax": servicetax,
            "AnyOtherDuty": anyotherduty,
            "VAT": vat,
            "CSt": cst,
            "CGST": cgst,
            "SGST": sgst,
            "IGST": igst,
            "AnyOtherTax": anyothertax,
            "TotalAddition": totaladdition,
            "Discount": discount,
            "Roundingoff": roundoff,
            "TotDeduction": totaldeduction,
            "InvoiceAmt": invoiceamt,
            "DutyDrawback": dutydrawback,
            "ExciseCredit": excisecredit,
            "ServiceTaxCredit": servicetaxcredit,
            "AnyOtherDutyCredit": anyotherdutycredit,
            "VATCredit": vatcredit,
            "CSTCredit": cstcredit,
            "CGSTCredit": cgstcredit,
            "SGSTCredit": sgstcredit,
            "IGSTCredit": igstcredit,
            "AnyOtherCredit": anyothercredit,
            "TotalCredit": totalcredit,
            "AmountCapitalised": amtcap,
            "AmountCapitalisedCompany": amtcapcompanylaw,
            "AmountCApitalisedIT": amtcapincometax,
            "AssetIdentificationNo": assetidno

        };

        var form = $('#frmNewEmp');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var headers = {};
        headers['__RequestVerificationToken'] = token;
        swal({
            title: "Are You Sure?", text: "You want to edit this record!",
            icon: "warning", buttons: true, dangerMode: true
        }).then((result) => {
            if (result == true) {

                var ControllerURL = '@Url.Action("Edit", "Asset")';
                var returnURL = '@Url.Action("Index", "Asset")';
                $.ajax({
                    type: 'POST',
                    url: ControllerURL,
                    contentType: "application/json",
                    headers: headers,
                    data: JSON.stringify(postdata),
                    success: function (res) {

                        if (res == "Success") {
                            swal({ title: "Successfully Edited!", icon: "success" }).then((result) => {
                                if (result == true) {
                                    window.location.replace(returnURL);
                                }
                            });
                        }
                        else {
                            swal("Alert", res, "warning");
                        }
                    },

                    failure: function () {
                        alert("Error");
                    }

                });
            }
        });
    }
}

function totalrate() {
    var nrate = 0;
    var arate = 0;
    var trate = 0;
    if ($("#txtNRate").val() != "") {

        nrate = parseFloat($("#txtNRate").val());

    }
    if ($('#txtARate').val() != "") {
        arate = parseFloat($('#txtARate').val());
    }

    trate = parseFloat(nrate + arate);
    if (trate > 100) {
        swal("Alert", "total rate must not exceed  than 100 !", "warning");
        $("#txtNRate").val(0);
        $('#txtARate').val(0);
        $('#txtTRate').val(0);


    }
    else {
        $('#txtTRate').val(trate);
    }
}


//////////////////////////////entering asset name it should be filled on other pages////////////////////
function setassetname() {
    var assetname = $('#assetname').val();
    var assetno = @Model.AssetNo;
    // alert(assetno);
    $('#txtsatassetno').val(assetno);
    $('#txtsatassetname').val(assetname);
    $('#txtotherinfoassetno').val(assetno);
    $('#txtotherinfoassetname').val(assetname);

}

function generateexpirydate() {
    var voucherdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var usefullife = $("#txtuseful").val();

    var expirydate = moment(voucherdate, "YYYY-MM-DD").add('years', usefullife);
    var formatteddate = moment(expirydate, "dddd-mmmm-dS-yyyy-h:MM:ss").format("DD/MM/YYYY");

    $("#txtExpDate").val(formatteddate);



}

function totaladdition() {
    var grossvalue = 0;
    var servicecharge = 0;
    var otherexpense = 0;
    var customduty = 0;
    var exciseduty = 0;
    var servicetax = 0;
    var vat = 0;
    var anyotherduty = 0;
    var cst = 0;
    var gst = 0;
    var anyothertax = 0;
    var totaladdition = 0;
    var discount = 0;
    var roundoff = 0;
    var totaldeduction = 0;
    var invoiceamt = 0;


    var dutydrawback = 0;
    var excisecredit = 0;
    var servicetaxcredit = 0;
    var anyotherdutycredit = 0;

    var vatcredit = 0;
    var anyothercredit = 0;
    var cstcredit = 0;
    var gstcredit = 0;
    var totalcredit = 0;
    var amtcap = 0;
    var amtcapcompanylaw = 0;
    var amtcapincometax = 0;
    //
    var cgstcredit = 0;
    var igstcredit = 0;
    var sgstcredit = 0;
    var cgst = 0;
    var igst = 0;
    var sgst = 0;
    //

    if ($("#txtigst").val() != "") {

        igst = parseFloat($("#txtigst").val());

    }
    if ($('#txtsgst').val() != "") {
        sgst = parseFloat($('#txtsgst').val());
    }

    if ($('#txtcgst').val() != "") {
        cgst = parseFloat($('#txtcgst').val());
    }
    if ($("#txtigstcredit").val() != "") {

        igstcredit = parseFloat($("#txtigstcredit").val());

    }
    if ($('#txtsgstcredit').val() != "") {
        sgstcredit = parseFloat($('#txtsgstcredit').val());
    }

    if ($('#txtcgstcredit').val() != "") {
        cgstcredit = parseFloat($('#txtcgstcredit').val());
    }



    ///
    if ($("#txtgrossvalue").val() != "") {

        grossvalue = parseFloat($("#txtgrossvalue").val());

    }
    if ($('#txtservicecharge').val() != "") {
        servicecharge = parseFloat($('#txtservicecharge').val());
    }

    if ($('#txtotherexpense').val() != "") {
        otherexpense = parseFloat($('#txtotherexpense').val());
    }
    if ($('#txtcustomduty').val() != "") {
        customduty = parseFloat($('#txtcustomduty').val());
    }
    if ($('#txtexciseduty').val()) { exciseduty = parseFloat($('#txtexciseduty').val()); }

    if ($('#txtservicetax').val() != "") { servicetax = parseFloat($('#txtservicetax').val()); }

    if ($('#txtvat').val() != "") { vat = parseFloat($('#txtvat').val()); }

    if ($('#txtanyotherduty').val() != "") { anyotherduty = parseFloat($('#txtanyotherduty').val()); }

    if ($('#txtcst').val() != "") { cst = parseFloat($('#txtcst').val()); }
    //if ($('#txtgst').val() != "") { gst = parseFloat($('#txtgst').val()); }
    if ($('#txtanyothertax').val() != "") { anyothertax = parseFloat($('#txtanyothertax').val()); }

    //  totaladdition = parseFloat(grossvalue + servicecharge + otherexpense + customduty + exciseduty + servicetax + vat + anyotherduty
    //    + cst + gst + anyothertax + sgst + igst + cgst);
    totaladdition = parseFloat(grossvalue + servicecharge + otherexpense + customduty + exciseduty + servicetax + vat + anyotherduty
        + cst + anyothertax + sgst + igst + cgst);

    $('#txttotaladdition').val(totaladdition);

    if ($('#txttotaladdition').val() != "") { totaladdition = parseFloat($('#txttotaladdition').val()); }
    if ($('#txtdiscount').val() != "") { discount = parseFloat($('#txtdiscount').val()); }

    if ($('#txtroundoff').val() != "") { roundoff = parseFloat($('#txtroundoff').val()); }

    if ($('#txttotaldeduction').val() != "") { totaldeduction = parseFloat($('#txttotaldeduction').val()); }

    invoiceamt = parseFloat(totaladdition - discount - totaldeduction - roundoff);
    $('#txtinvoiceamt').val(invoiceamt);

    if ($("#txtdutydrawback").val() != "") {

        dutydrawback = parseFloat($("#txtdutydrawback").val());

    }
    if ($('#txtexcisecredit').val() != "") {
        excisecredit = parseFloat($('#txtexcisecredit').val());
    }

    if ($('#txtservicetaxcredit').val() != "") {
        servicetaxcredit = parseFloat($('#txtservicetaxcredit').val());
    }
    if ($('#txtvatcredit').val() != "") {
        vatcredit = parseFloat($('#txtvatcredit').val());
    }
    //if ($('#txtgstcredit').val())
    //{ gstcredit = parseFloat($('#txtgstcredit').val()); }

    if ($('#txtanyothercredit').val() != "") { anyothercredit = parseFloat($('#txtanyothercredit').val()); }

    if ($('#txtcstcredit').val() != "") { cstcredit = parseFloat($('#txtcstcredit').val()); }

    if ($('#txtanyotherdutycredit').val() != "") { anyotherdutycredit = parseFloat($('#txtanyotherdutycredit').val()); }

    // totalcredit = parseFloat(dutydrawback + excisecredit + vatcredit + cstcredit + gstcredit + sgstcredit + igstcredit + cgstcredit + anyotherdutycredit
    //   + anyothercredit + servicetaxcredit);
    totalcredit = parseFloat(dutydrawback + excisecredit + vatcredit + cstcredit + sgstcredit + igstcredit + cgstcredit + anyotherdutycredit
        + anyothercredit + servicetaxcredit);

    $("#txttotalcredit").val(totalcredit)
    amtcap = parseFloat(invoiceamt - totalcredit);

    $('#txtamttobecap').val(amtcap);
    $('#txtamtcapcompanylaw').val(amtcap);
    $('#txtamtcapincometax').val(amtcap);


    if ($('#Parent_AssetId').val() != null) {
        checkadditionassettobeaddedornot();
    }


}



//////date validations for child tables
function validlocationdate() {

    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var locdate = moment($("#txtlocDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');


    if (new Date(vdate) > new Date(locdate)) {

        // $("#locdateerror").html(" * location Date must be greater than Voucher Date");
        // $("#locdateerror").show();
        return true;
    }
    else {
        return false;
        // $("#locdateerror").hide();

    }
}



function editlocationdate() {


    var vdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var locdate = moment($("#txteditlocDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');

    if (new Date(vdate) > new Date(locdate)) {
        //$("#editlocdateerror").html(" * This Date must be greater than Voucher Date");
        // $("#editlocdateerror").show();
        return true;
    }
    else {
        return false;
        // $("#editlocdateerror").hide();

    }
}
