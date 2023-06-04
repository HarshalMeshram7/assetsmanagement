using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VerifyWebApp.Models;

namespace VerifyWebApp.Validators
{
    public class DGroupValidator : AbstractValidator<DGroup>
    {
        public DGroupValidator()
        {
            RuleFor(x => x.DGroupName).NotEmpty();
            RuleFor(x => x.DGroupName).MaximumLength(50);
            RuleFor(x => x.Companyid).NotEmpty();
        }
    }
}