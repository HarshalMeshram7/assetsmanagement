
function InitPage() {

    // 7 bind to events triggered on the tree
    $('#jstree').on("changed.jstree", function (e, data) {
        console.log(data.selected);
        var node = $('#jstree').jstree().get_selected(true)[0];
        console.log('node' + node);
        console.log('id' + node.id);
        console.log('text' + node.text);

        let nodelevel = 0;
        nodelevel = node.id.substring(1, 2);
        console.log('nodelevel' + nodelevel);
        saveselectednode(nodelevel, node);
    });

    $('#mnuExpand').on('click', function () {
        $('#jstree').jstree("open_all");
    });
    $('#mnuContract').on('click', function () {
        $('#jstree').jstree("close_all");
    });

    $("#btnsave").click(function () {
        SaveData();
    });


    $('#mnuSelectGroup').on('click', function () {

        $("#txtAgroupname").val(AGroupName);
        $("#txtBgroupname").val(BGroupName);
        $("#txtCgroupname").val(CGroupName);
        $("#txtDgroupname").val(DGroupName);
        $('#AssetGroupmodal').modal('hide');

    });


}

function InitTree() {

   
    $('#jstree').jstree({
        'core': {
            'data': {
                'url': function (node) {
                   // var url = '@Url.Action("GetAssetGroup", "AssetGroups")';
                    let url = URL_Get_AssetGroups;
                    //return node.id === '#' ? url : url;
                    return url;

                },
                'headers': getToken(),
                'data': function (node) {
                    return { 'id': node.id };
                }
            },
            'themes': {
                'name': 'proton',
                'responsive': true
            }
        }

    });

    $("#jstree").bind("hover_node.jstree", function (e, data) {

        $("#" + data.node.id).prop("title", data.node.text);
    });

    $("#jstree").bind("ready.jstree", function (e, data) {

        $('#jstree').jstree("open_all");
    });

}

function saveselectednode(level, node) {

    if (level == 0) {
        return;
    }

    if (level == 1) { // L1

        AGroupID = node.id.substring(3, node.id.length);
        AGroupName = node.text;

        console.log("AGroupID " + AGroupID);
        console.log("AGroupID Text " + AGroupName);

        BGroupID = '';
        BGroupName = '';

        CGroupID = '';
        CGroupName = '';

        DGroupID = '';
        DGroupName = '';

    }
    else if (level == 2) { // L2

        BGroupID = node.id.substring(3, node.id.length);
        BGroupName = node.text;

        console.log("BGroupID " + BGroupID);
        console.log("BGroupID Text " + BGroupName);


        let p1node_id = $('#jstree').jstree("get_parent", node);
        let p1node = $('#jstree').jstree("get_node", p1node_id);

        console.log(p1node);
        if (p1node) {


            AGroupID = p1node.id.substring(3, p1node.id.length);
            AGroupName = p1node.text;

            console.log("AGroupID " + AGroupID);
            console.log("AGroupName Text " + AGroupName);

        }

        CGroupID = '';
        CGroupName = '';

        DGroupID = '';
        DGroupName = '';
    } else if (level == 3) { // L2


        CGroupID = node.id.substring(3, node.id.length);
        CGroupName = node.text;

        console.log("GroupID " + CGroupID);
        console.log("GroupID Text " + CGroupName);

        let p1node_id = $('#jstree').jstree("get_parent", node);
        let p1node = $('#jstree').jstree("get_node", p1node_id);

        if (p1node) {


            BGroupID = p1node.id.substring(3, p1node.id.length);
            BGroupName = p1node.text;

            console.log("BGroupID " + BGroupID);
            console.log("BGroupName Text " + BGroupName);



            let p2node = $('#jstree').jstree("get_node", BGroupID);

            if (p2node) {

                AGroupID = p2node.id.substring(3, p2node.id.length);
                AGroupName = p2node.text;

            }


        }


    } else if (level == 4) { // L2


        DGroupID = node.id.substring(3, node.id.length);
        DGroupName = node.text;

        let p1node_id = $('#jstree').jstree("get_parent", node);
        let p1node = $('#jstree').jstree("get_node", p1node_id); // LVEL C

        if (p1node) {

            CGroupID = p1node.id.substring(3, p1node.id.length);
            CGroupName = p1node.text;

            let p2node = $('#jstree').jstree("get_node", CGroupID);
            if (p2node) {

                BGroupID = p2node.id.substring(3, p2node.id.length);
                BGroupName = p2node.text;


                let p3node = $('#jstree').jstree("get_node", BGroupID);

                if (p3node) {
                    AGroupID = p3node.id.substring(3, p3node.id.length);
                    AGroupName = p3node.text;

                }

            }

        }

    }

}


/**
 *  Validate Form Before Saving
 * */
function validateform() {

    let errorlist = [];

    console.log('validateform');


    var VoucherDate = $('#txtVDate').val();
    var DtPutToUse = $('#txtdpucomDate').val();


    var UsefulLife = $("#txtuseful").val();
    var AssetName = $('#assetname').val();
    var assetidno = $("#assetidno").val();
    var grossvalue = $("#txtgrossvalue").val();
    var Qty = $('#txtqty').val();
    var DtPutToUseIT = $('#txtdpuItDate').val();
    var assetno = $('#txtassetno').val();

    let normalrate = $('#txtNRate').val();

    if (assetno == "") {
        errorlist.push("Assetno is not entered");
    }
    if (Qty == "") {
        errorlist.push("Quantity is not entered");
    }
    if (grossvalue == "") {
        errorlist.push("Gross value  is not entered");
    }
    if (DtPutToUseIT == "") {
        errorlist.push("Date put to use (IT) is not entered");
    }

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
        errorlist.push("Date put to use (Company) is not entered");
    }
    

    if (normalrate == "") {
        errorlist.push("Normal Rate is not entered");
    }
  
    if (AssetName) {
        if (AssetName.length > 100) {
            errorlist.push("Asset Name should be upto 100 characters.");
        }

    }
   
    if (assetidno) {
        if (assetidno.length > 100) {
            errorlist.push("Asset Identification No should be upto 100 characters.");
        }

    }

    let VoucherNo = $('#txtvno').val();
  
    if (VoucherNo) {
        if (VoucherNo.length > 100) {
            errorlist.push("Voucher No No should be upto 100 characters.");
        }

    }

    let PONo = $('#txtpono').val();

    if (PONo) {
        if (PONo.length > 50) {
            errorlist.push("PO No should be upto 50 characters.");
        }

    }

    let BillNo = $('#txtbillno').val();

    if (BillNo) {

        if (BillNo.length > 50) {
            errorlist.push("Bill No should be upto 50 characters.");
        }

    }
    let MRRNo = $('#txtmrrno').val();

    if (MRRNo) {
        if (MRRNo.length > 50) {
            errorlist.push("MRR No should be upto 50 characters.");
        }

    }

    let BrandName = $('#txtbrandname').val();

    if (BrandName) {

        if (BrandName.length > 50) {
            errorlist.push("Brand Name should be upto 50 characters.");
        }

    }

    let SrNo = $('#txtsrno').val();
    if (SrNo) {
        if (SrNo.length > 50) {
            errorlist.push("Sr No should be upto 50 characters.");
        }

    }

    let Model = $('#txtmodel').val();

    if (Model) {
        if (Model.length > 50) {
            errorlist.push("Model should be upto 50 characters.");
        }

    }

    let Remarks = $('#txtremarks').val();
    if (Remarks) {
        if (Remarks.length > 100) {
            errorlist.push("Model should be upto 100 characters.");
        }

    }

    return errorlist;
}


function SaveData() {


    var validateformz = validateform();

   // console.log(validateformz.length);


    if (validateformz.length != 0) {
     
        var srno = 1;
        $('#errtbl tbody').empty();

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

        var depreciationmethod = $("input[name='DepreciationMethod']:checked").val();
        var purchaseaccid = $("#txtPAccName").find("option:selected").val();
        var accumatedid = $("#txtAccDepAccName").find("option:selected").val();;
        var itgroup = $("#txtItgroupName").find("option:selected").val();
        var depid = $("#txtDepAccName").find("option:selected").val();
        var assetidno = $("#assetidno").val();

        var NRate = $("#txtNRate").val();
        var ARate = $("#txtARate").val();
        var TRate = $("#txtTRate").val();
        var UsefulLife = $("#txtuseful").val();
        var YrMang = $("#txtYrMang").val();

        var grossvalue = $("#txtgrossvalue").val();
        var servicecharge = $('#txtservicecharge').val();
        var otherexpense = $('#txtotherexpense').val();
        var customduty = $('#txtcustomduty').val();
        var exciseduty = $('#txtexciseduty').val();
        var servicetax = $('#txtservicetax').val();;
        var vat = $('#txtvat').val();;
        var anyotherduty = $('#txtanyotherduty').val();;
        var cst = $('#txtcst').val();;
        //   var gst = $('#txtgst').val();
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
        //  var gstcredit = $('#txtgstcredit').val();
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



        var ExpDate = $("#txtExpDate").val();
        var VoucherDate = $("#txtVDate").val();
        var PODate = $("#txtPoDate").val();
        var ReceiptDate = $("#txtreceiptDate").val();
        var CommissioningDate = $("#txtcommDate").val();
        var BillDate = $("#txtbillDate").val();
        var DtPutToUse = $("#txtdpucomDate").val();
        var DtPutToUseIT = $("#txtdpuItDate").val();


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
        var iscomponent = iscomponentflag;
        var Currency = $('#txtcurrency').val();
        var Values = $('#txtvalue').val();
        var Residual = $('#txtresidual').val();
        var OPAccDepreciation = $('#opaccdepreciation').val();
        var ITGroupIDID = $('#txtItgroupName').val();
        var assetno = $('#txtassetno').val();
        var parentassetid = $("#Parent_AssetId").val();
        var postdata =
        {
            "iscomponent": iscomponent,
            "AssetNo": assetno,
            "Parent_AssetId": parentassetid,
            "levelid":"",   /*'@ViewBag.levelid'*/
            "OPAccDepreciation": OPAccDepreciation,
            //"DepreciationMethod": depreciationmethod,
            "AccountID": purchaseaccid,
            "DepAccountId": depid,
            "AccAccountID": accumatedid,


            "DepreciationMethod": depreciationmethod,
            "ITGroupIDID": ITGroupIDID,
            //"AccountID": "1",
            //"DepAccountId": "1",
            //"AccAccountID": "1",

            "locationlist": tbllocation,
            "costcenterlist": tblcostcenter,
            "assetfreeofcostlist": tblassetfreeofcost,
            "AssetIdentificationNo": assetidno,
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
            "UOMNo": UOMNo,
            "AGroupID": AGroupID,
            "BGroupID": BGroupID,
            "CGroupID": CGroupID,
            "DGroupID": DGroupID,

        };


        console.log("postdata",postdata);
        //return;

        var form = $('#frmNewEmp');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var headers = {};
        headers['__RequestVerificationToken'] = token;

        swal({
            title: "Are You Sure?", text: "You won't be able to revert this!",
            icon: "warning", buttons: true, dangerMode: true
        }).then((result) => {
            if (result == true) {

                $.ajax({
                    type: 'POST',
                    url: URL_AddAssetGroup,
                    contentType: "application/json",
                    headers: headers,
                    data: JSON.stringify(postdata),
                    success: function (res) {

                        if (res == "Success") {
                            swal({ title: "Successfully Added!", icon: "success" }).then((result) => {
                                if (result == true) {
                                    window.location.replace(URL_AssetIndex);
                                }
                            });
                        }
                        else {
                            swal("Alert", res, "warning");
                        }

                    },

                    failure: function () { alert("Error"); }

                });
            }
        });
    }
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
    let totaladdition = 0;
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

    

    if ($('#txtanyothertax').val() != "") { anyothertax = parseFloat($('#txtanyothertax').val()); }

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
    

    if ($('#txtanyothercredit').val() != "") { anyothercredit = parseFloat($('#txtanyothercredit').val()); }

    if ($('#txtcstcredit').val() != "") { cstcredit = parseFloat($('#txtcstcredit').val()); }

    if ($('#txtanyotherdutycredit').val() != "") { anyotherdutycredit = parseFloat($('#txtanyotherdutycredit').val()); }

    


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


function setassetname() {
    var assetname = $('#assetname').val();
    var assetno = $("#txtassetno").val();

    $('#txtsatassetno').val(assetno);
    $('#txtsatassetname').val(assetname);
    $('#txtotherinfoassetno').val(assetno);
    $('#txtotherinfoassetname').val(assetname);

}

function generateexpirydate() {

    var dateputuse = $("#txtdpucomDate").val();
    var usefullife = $("#txtuseful").val();

    //dateputuse=moment(dateputuse, 'DD/MM/YYYY').format('YYYY-MM-DD');
    var expirydate = moment(dateputuse, "YYYY-MM-DD").add('years', usefullife);

    // console.log(expirydate);
    var formatteddate = moment(expirydate, "dddd-mmmm-dS-yyyy-h:MM:ss").format("YYYY-MM-DD");
    // console.log(formatteddate);

    $("#txtExpDate").val(formatteddate);

}

///// satutory expiry dateis less then voucher date warning

function expiredatevalidatewithvoucher() {
    var voucherdate = moment($("#txtVDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    var expirydate = moment($("#txtExpDate").val(), 'DD/MM/YYYY').format('YYYY-MM-DD');
    if (voucherdate != "Invalid date" && expirydate != "Invalid date") {
        if (new Date(expirydate) < new Date(voucherdate)) {

            swal("Alert", "Expiry date cannot be less than voucher date ", "warning");
            //  document.getElementById("txtdpucomDate").focus();
        }
        else {

        }
    }
}