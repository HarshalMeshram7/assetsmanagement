using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VerifyWebApp.Models;

namespace VerifyWebApp.Validators
{
    public class AssetValidator:AbstractValidator<Assets>
    {


        public AssetValidator()
        {
            var depconditions = new List<string>() { "SLM","WDV"};


            RuleFor(x => x.AssetName).NotEmpty();
            RuleFor(x => x.AssetName).MaximumLength(150);


            RuleFor(x=>x.VoucherDate).NotEmpty();
            RuleFor(x => x.DtPutToUse).NotEmpty();
            RuleFor(x => x.DtPutToUseIT).NotEmpty();

            RuleFor(x => x.Qty).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.AGroupID).NotEmpty().GreaterThan(0);

            RuleFor(x => x.DepreciationMethod).Must(x => depconditions.Contains(x));

            RuleFor(x => x.NormalRatae).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalRate).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.Usefullife).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.ExpiryDate).NotEmpty();

            RuleFor(x => x.GrossVal).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.AmountCapitalised).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.AmountCapitalisedCompany).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.AmountCApitalisedIT).NotEmpty().GreaterThanOrEqualTo(0);

            RuleFor(x => x.AssetNo).NotEmpty();


        }
    }
}