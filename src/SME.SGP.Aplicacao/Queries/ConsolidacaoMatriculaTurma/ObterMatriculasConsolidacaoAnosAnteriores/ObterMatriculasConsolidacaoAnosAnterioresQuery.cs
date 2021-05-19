using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasConsolidacaoAnosAnterioresQuery : IRequest<IEnumerable<ConsolidacaoMatriculaTurmaDto>>
    {
        public ObterMatriculasConsolidacaoAnosAnterioresQuery(int anoLetivo, string ueCodigo)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; set; }
    }

    public class ObterMatriculasConsolidacaoAnosAnterioresQueryValidator : AbstractValidator<ObterMatriculasConsolidacaoAnosAnterioresQuery>
    {
        public ObterMatriculasConsolidacaoAnosAnterioresQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter os dados de consolidação de matrículas das turmas");
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para obter os dados de consolidação de matrículas das turmas");
        }
    }
}
