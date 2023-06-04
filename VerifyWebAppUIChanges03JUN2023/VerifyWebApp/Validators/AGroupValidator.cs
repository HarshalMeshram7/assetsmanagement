using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VerifyWebApp.Models;

namespace VerifyWebApp.Validators
{
    public class AGroupValidator : AbstractValidator<AGroup>
    {
        public AGroupValidator()
        {
            RuleFor(x=>x.AGroupName).NotEmpty();
            RuleFor(x => x.AGroupName).MaximumLength(50);
            RuleFor(x => x.Companyid).NotEmpty();
        }
    }
}