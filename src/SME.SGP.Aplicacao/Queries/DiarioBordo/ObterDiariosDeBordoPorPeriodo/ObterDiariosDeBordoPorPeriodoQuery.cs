using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoPorPeriodoQuery: IRequest<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>>
    {
        public ObterDiariosDeBordoPorPeriodoQuery(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
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

    public class ObterDiariosDeBordoPorPeriodoQueryValidator : AbstractValidator<ObterDiariosDeBordoPorPeriodoQuery>
    {
        public ObterDiariosDeBordoPorPeriodoQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de seus diários de bordo");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .Must(a => a > 0)
                .WithMessage("O código do componente curricular deve ser informado para consulta dos diários de bordo da turma");

            RuleFor(a => a.PeriodoInicio)
                .Must(a => a > DateTime.MinValue)
                .WithMessage("O período de inicio deve ser informado para consulta dos diários de bordo da turma");

            RuleFor(a => a.PeriodoFim)
                .Must(a => a > DateTime.MinValue)
                .WithMessage("O período de fim deve ser informado para consulta dos diários de bordo da turma");
        }
    }
}
