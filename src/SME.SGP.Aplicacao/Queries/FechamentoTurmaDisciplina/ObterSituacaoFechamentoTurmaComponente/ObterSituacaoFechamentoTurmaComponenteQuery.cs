using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacaoFechamentoTurmaComponenteQuery : IRequest<SituacaoFechamento>
    {
        public ObterSituacaoFechamentoTurmaComponenteQuery(long turmaId, long componenteCurricularId, long periodoEscolarId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long PeriodoEscolarId { get; set; }
    }

    public class ObterSituacaoFechamentoTurmaComponenteQueryValidator : AbstractValidator<ObterSituacaoFechamentoTurmaComponenteQuery>
    {
        public ObterSituacaoFechamentoTurmaComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta da situação do fechamento.");

            RuleFor(c => c.ComponenteCurricularId)
               .NotEmpty()
               .WithMessage("O id do componente curricular deve ser informado para consulta da situação do fechamento.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O id do período escolar deve ser informado para consulta da situação do fechamento.");
        }
    }
}
