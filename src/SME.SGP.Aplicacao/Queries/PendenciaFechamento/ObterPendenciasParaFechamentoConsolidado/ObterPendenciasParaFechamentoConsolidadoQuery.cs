using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasParaFechamentoConsolidadoQuery : IRequest<IEnumerable<PendenciaParaFechamentoConsolidadoDto>>
    {
        public ObterPendenciasParaFechamentoConsolidadoQuery(long turmaId, int bimestre, long componenteCurricularId)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public long ComponenteCurricularId { get; set; }
    }

    public class ObterPendenciasParaFechamentoConsolidadoQueryValidator : AbstractValidator<ObterPendenciasParaFechamentoConsolidadoQuery>
    {
        public ObterPendenciasParaFechamentoConsolidadoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");
            RuleFor(a => a.Bimestre)
                .NotNull()
                .WithMessage("O bimestre deve ser informado.");
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado.");
        }
    }
}
