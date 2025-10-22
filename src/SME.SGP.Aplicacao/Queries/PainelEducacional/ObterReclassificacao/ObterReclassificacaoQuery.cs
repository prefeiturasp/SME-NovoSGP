using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterReclassificacao
{
    public class ObterReclassificacaoQuery : IRequest<IEnumerable<PainelEducacionalReclassificacaoDto>>
    {
        public ObterReclassificacaoQuery(string codigoDre, string codigoUe, int anoLetivo, int anoTurma)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            AnoTurma = anoTurma;
        }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public int AnoTurma { get; set; }
    }

    public class ObterReclassificacaoQueryValidator : AbstractValidator<ObterReclassificacaoQuery>
    {
        public ObterReclassificacaoQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
            .NotEmpty()
            .WithMessage("O código da DRE deve ser informado.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da UE deve ser informado.");

            RuleFor(c => c.AnoLetivo)
           .NotEmpty()
           .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
