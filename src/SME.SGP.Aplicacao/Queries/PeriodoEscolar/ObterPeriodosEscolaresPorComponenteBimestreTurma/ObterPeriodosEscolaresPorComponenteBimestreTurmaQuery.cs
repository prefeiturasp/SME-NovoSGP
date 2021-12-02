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
        public string ComponenteCodigo { get; set; }
        public int Bimestre { get; set; }

        public ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery(string turmaCodigo, string componenteCodigo, int bimestre)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCodigo = componenteCodigo;
            Bimestre = bimestre;
        }
    }

    public class ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryValidator : AbstractValidator<ObterPeriodosEscolaresPorComponenteBimestreTurmaQuery>
    {
        public ObterPeriodosEscolaresPorComponenteBimestreTurmaQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para buscar os períodos");

            RuleFor(a => a.ComponenteCodigo)
               .NotEmpty()
               .WithMessage("O componente deve ser informado para buscar os períodos");

            RuleFor(a => a.Bimestre)
               .NotEmpty()
               .WithMessage("O bimestre da turma deve ser informado para buscar os períodos");
        }
    }
}
