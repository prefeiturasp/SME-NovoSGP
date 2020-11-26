using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaPorTurmaCCPeriodoEscolarQuery : IRequest<long>
    {
        public ObterPendenciaPorTurmaCCPeriodoEscolarQuery(long turmaId, long componenteCurricularId, long periodoEscolarId, TipoPendencia tipoPendencia)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            PeriodoEscolarId = periodoEscolarId;
            TipoPendencia = tipoPendencia;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long PeriodoEscolarId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciaPorTurmaCCPeriodoEscolarQueryValidator : AbstractValidator<ObterPendenciaPorTurmaCCPeriodoEscolarQuery>
    {
        public ObterPendenciaPorTurmaCCPeriodoEscolarQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para localizar a pendência.");

        }
    }
}
