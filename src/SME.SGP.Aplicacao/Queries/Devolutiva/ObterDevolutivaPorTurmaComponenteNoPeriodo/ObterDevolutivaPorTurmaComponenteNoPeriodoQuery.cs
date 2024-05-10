using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorTurmaComponenteNoPeriodoQuery : IRequest<IEnumerable<long>>
    {
        public ObterDevolutivaPorTurmaComponenteNoPeriodoQuery(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }

    public class ObterDevolutivaPorTurmaComponenteNoPeriodoQueryValidator : AbstractValidator<ObterDevolutivaPorTurmaComponenteNoPeriodoQuery>
    {
        public ObterDevolutivaPorTurmaComponenteNoPeriodoQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O Código da Turma deve ser informado para consulta de devolutivas.");

            RuleFor(c => c.ComponenteCurricularCodigo)
            .NotEmpty()
            .WithMessage("O Código do Componente Curricular deve ser informado para consulta de devolutivas.");

            RuleFor(c => c.PeriodoInicio)
            .NotEmpty()
            .WithMessage("O início do periodo deve ser informado para consulta de devolutivas.");

            RuleFor(c => c.PeriodoFim)
            .NotEmpty()
            .WithMessage("O fim do período deve ser informado para consulta de devolutivas.");

        }
    }
}
