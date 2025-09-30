using FluentValidation;
using MediatR;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosTurmaPap
{
    public class ObterAlunosTurmaPapQuery : IRequest<IEnumerable<DadosMatriculaAlunoTipoPapDto>>
    {
        public int? AnoLetivo { get; set; }

        public ObterAlunosTurmaPapQuery(int? anoLetivo = null)
        {
            AnoLetivo = anoLetivo;
        }
    }

    public class ObterAlunosTurmaPapQueryValidator : AbstractValidator<ObterAlunosTurmaPapQuery>
    {
        public ObterAlunosTurmaPapQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThanOrEqualTo(PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE)
                .When(c => c.AnoLetivo.HasValue)
                .WithMessage($"Ano letivo deve ser maior ou igual a {PainelEducacionalConstants.ANO_LETIVO_MIM_LIMITE}.");
        }
    }
}
