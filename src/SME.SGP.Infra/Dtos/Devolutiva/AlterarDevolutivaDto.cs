using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlterarDevolutivaDto
    {
        public long Id { get; set; }

        public long CodigoComponenteCurricular { get; set; }

        public List<long> DiariosBordoIds { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFim { get; set; }

        public string Descricao { get; set; }
    }

    public class AlterarDevolutivaDtoValidator : AbstractValidator<AlterarDevolutivaDto>
    {
        public AlterarDevolutivaDtoValidator()
        {
            RuleFor(a => a.Id)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.CodigoComponenteCurricular)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.PeriodoInicio)
                   .NotEqual(DateTime.MinValue)
                   .WithMessage("O início do período deve ser informado!");

            RuleFor(a => a.PeriodoFim)
                   .NotEqual(DateTime.MinValue)
                   .WithMessage("O fim do período deve ser informado!");

            RuleFor(a => a.Descricao)
                   .NotEmpty()
                   .WithMessage("A descrição deve ser informada!");
        }
    }
}
