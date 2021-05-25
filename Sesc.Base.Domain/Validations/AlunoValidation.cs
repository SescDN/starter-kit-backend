using FluentValidation;
using Sesc.Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sesc.Base.Domain.Validations
{
    public class AlunoValidation : AbstractValidator<Aluno>
    {
        public AlunoValidation()
        {
            RuleSet("Insert", () =>
            {
                RuleFor(x => x.Nome).NotNull().NotEmpty().MaximumLength(40);               
                RuleFor(x => x.Documento).NotNull().NotEmpty();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotEqual(0);
                RuleFor(x => x.Nome).NotNull().NotEmpty().MaximumLength(40);
                RuleFor(x => x.Documento).NotNull().NotEmpty();
            });
        }
    }
}
