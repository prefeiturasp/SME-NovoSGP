using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasProfessorPorTurmaEComponenteQuery : IRequest<IEnumerable<PendenciaProfessor>>
    {
        public ObterPendenciasProfessorPorTurmaEComponenteQuery(long turmaId, long[] componentesCurriculares, long? periodoEscolarId, TipoPendencia tipoPendencia)
        {
            TurmaId = turmaId;
            ComponentesCurriculares = componentesCurriculares;
            PeriodoEscolarId = periodoEscolarId;
            TipoPendencia = tipoPendencia;
        }

        public long TurmaId { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciasProfessorPorUeEComponenteQueryValidator : AbstractValidator<ObterPendenciasProfessorPorTurmaEComponenteQuery>
    {
        public ObterPendenciasProfessorPorUeEComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para consulta de pendencias do professor.");

            RuleFor(c => c.ComponentesCurriculares)
               .NotEmpty()
               .WithMessage("Os componentes curriculares devem ser informados para consulta de pendencias do professor.");
        }
    }
}
