using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlterarDevolutivaDto
    {
        public long Id { get; set; }

        public string TurmaCodigo { get; set; }

        public string Descricao { get; set; }
    }

    public class AlterarDevolutivaDtoValidator : AbstractValidator<AlterarDevolutivaDto>
    {
        public AlterarDevolutivaDtoValidator()
        {
            RuleFor(a => a.Id)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.Descricao)
                   .NotEmpty()
                   .WithMessage("A descrição deve ser informada!");
        }
    }
}
