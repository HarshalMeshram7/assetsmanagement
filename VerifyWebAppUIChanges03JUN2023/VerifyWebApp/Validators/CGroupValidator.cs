using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using VerifyWebApp.Models;

namespace VerifyWebApp.Validators
{
    public class CGroupValidator : AbstractValidator<CGroup>
    {
        public CGroupValidator()
        {
            RuleFor(x => x.CGroupName).NotEmpty();
            RuleFor(x => x.CGroupName).MaximumLength(50);
            RuleFor(x => x.Companyid).NotEmpty();
        }
    }
}