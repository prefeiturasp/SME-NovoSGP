using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaCommand: IRequest<AuditoriaDto>
    {
        public long CodigoComponenteCurricular { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public string Descricao { get; set; }

        public InserirDevolutivaCommand(long codigoComponenteCurricular, DateTime periodoInicio, DateTime periodoFim, string descricao)
        {
            CodigoComponenteCurricular = codigoComponenteCurricular;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            Descricao = descricao;
        }
    }

    public class InserirDevolutivaCommandValidator: AbstractValidator<InserirDevolutivaCommand>
    {
        public InserirDevolutivaCommandValidator()
        {
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
