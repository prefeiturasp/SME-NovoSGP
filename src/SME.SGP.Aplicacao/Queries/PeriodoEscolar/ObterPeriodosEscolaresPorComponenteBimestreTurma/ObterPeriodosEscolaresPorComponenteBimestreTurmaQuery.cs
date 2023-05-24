using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery : IRequest<IEnumerable<PeriodoEscolarVerificaRegenciaDto>>
    {
        public string TurmaCodigo { get; set; }
        public long[] ComponentesCodigos { get; set; }
        public int Bimestre { get; set; }
        public bool AulaCj { get; set; }

        public ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(string turmaCodigo, long[] componentesCodigos, int bimestre, bool aulaCj)
        {
            TurmaCodigo = turmaCodigo;
            ComponentesCodigos = componentesCodigos;
            Bimestre = bimestre;
            AulaCj = aulaCj;
        }
    }

    public class ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>
    {
        public ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para buscar os períodos");

            RuleFor(a => a.ComponentesCodigos)
               .NotEmpty()
               .WithMessage("Os componentes devem ser informados para buscar os períodos");

            RuleFor(a => a.Bimestre)
               .NotEmpty()
               .WithMessage("O bimestre da turma deve ser informado para buscar os períodos");
        }
    }
}
