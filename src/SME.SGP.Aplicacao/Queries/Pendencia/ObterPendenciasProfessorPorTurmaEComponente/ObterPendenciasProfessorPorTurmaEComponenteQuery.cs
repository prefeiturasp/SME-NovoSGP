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
        public ObterPendenciasProfessorPorTurmaEComponenteQuery(string turmaCodigo, long[] componentesCurriculares, long periodoEscolarId, TipoPendencia tipoPendencia)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            PeriodoEscolarId = periodoEscolarId;
            TipoPendencia = tipoPendencia;
        }

        public string TurmaCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public long PeriodoEscolarId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterPendenciasProfessorPorUeEComponenteQueryValidator : AbstractValidator<ObterPendenciasProfessorPorTurmaEComponenteQuery>
    {
        public ObterPendenciasProfessorPorUeEComponenteQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para consulta de pendencias do professor.");

            RuleFor(c => c.ComponentesCurriculares)
               .NotEmpty()
               .WithMessage("Os componentes curriculares devem ser informados para consulta de pendencias do professor.");

            RuleFor(c => c.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("O período escolar deve ser informados para consulta de pendencias do professor.");
        }
    }
}
