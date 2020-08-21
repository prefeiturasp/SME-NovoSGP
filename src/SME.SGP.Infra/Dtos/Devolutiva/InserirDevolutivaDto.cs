using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class InserirDevolutivaDto
    {
        public string TurmaCodigo { get; set; }
        public long CodigoComponenteCurricular { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFim { get; set; }

        public string Descricao { get; set; }
    }

    public class InserirDevolutivaDtoValidator: AbstractValidator<InserirDevolutivaDto>
    {
        public InserirDevolutivaDtoValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                   .NotEmpty()
                   .WithMessage("O código da turma deve ser informado para geração da devolutiva");

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
