﻿using FinancialHub.Domain.Models;
using FinancialHub.WebApi.Resources;
using FluentValidation;

namespace FinancialHub.WebApi.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ErrorMessages.Required)
                .Length(0,200)
                .WithMessage(ErrorMessages.ExceedMaxLength);

            RuleFor(x => x.Description)
                .Length(0,500)
                .WithMessage(ErrorMessages.ExceedMaxLength);
        }
    }
}
