using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class InserirDevolutivaDto
    {
        public long CodigoComponenteCurricular { get; set; }

        public List<long> DiariosBordoIds { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

        public string Descricao { get; set; }
    }

    public class InserirDevolutivaDtoValidator: AbstractValidator<InserirDevolutivaDto>
    {
        public InserirDevolutivaDtoValidator()
        {
            RuleFor(a => a.CodigoComponenteCurricular)
                   .NotEmpty()
                   .WithMessage("O componente curricular deve ser informado!");

            RuleFor(a => a.DiariosBordoIds)
                   .NotEmpty()
                   .WithMessage("Os diários de bordo devem ser informados !");

            RuleFor(a => a.Descricao)
                   .NotEmpty()
                   .WithMessage("A descrição deve ser informada!");
        }
    }
}
