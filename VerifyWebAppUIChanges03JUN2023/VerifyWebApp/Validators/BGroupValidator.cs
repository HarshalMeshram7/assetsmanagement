using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VerifyWebApp.Models;

namespace VerifyWebApp.Validators
{
    public class BGroupValidator : AbstractValidator<BGroup>
    {
        public BGroupValidator()
        {
            RuleFor(x => x.BGroupName).NotEmpty();
            RuleFor(x => x.BGroupName).MaximumLength(50);
            RuleFor(x => x.Companyid).NotEmpty();
        }
    }
}