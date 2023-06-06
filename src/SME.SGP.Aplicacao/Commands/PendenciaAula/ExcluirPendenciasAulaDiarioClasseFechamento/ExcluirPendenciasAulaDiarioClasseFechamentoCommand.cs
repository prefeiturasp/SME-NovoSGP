using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasAulaDiarioClasseFechamentoCommand : IRequest<bool>
    {
        public ExcluirPendenciasAulaDiarioClasseFechamentoCommand(string turmaCodigo, string disciplinaId, DateTime periodoInicio, DateTime periodoFim)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public string TurmaCodigo { get; set; }
        public string DisciplinaId { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

    }

    public class ExcluirPendenciasAulaDiarioClasseFechamentoCommandValidator : AbstractValidator<ExcluirPendenciasAulaDiarioClasseFechamentoCommand>
    {
        public ExcluirPendenciasAulaDiarioClasseFechamentoCommandValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para exclusão de todas suas pendências do período do fechamento.");
            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O id da disciplina deve ser informado para exclusão de todas suas pendências do período do fechamento.");
            RuleFor(c => c.PeriodoInicio)
            .NotEmpty()
            .WithMessage("A data/período início do bimestre deve ser informado para exclusão de todas suas pendências do período do fechamento.");
            RuleFor(c => c.PeriodoFim)
            .NotEmpty()
            .WithMessage("A data/período fim do bimestre deve ser informado para exclusão de todas suas pendências do período do fechamento.");
        }
    }
}
