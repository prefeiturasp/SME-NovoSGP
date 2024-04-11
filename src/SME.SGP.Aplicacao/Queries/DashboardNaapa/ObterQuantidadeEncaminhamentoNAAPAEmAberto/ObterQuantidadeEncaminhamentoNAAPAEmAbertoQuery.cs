using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery : IRequest<GraficoEncaminhamentoNAAPADto>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery(int anoLetivo, long? dreId, Modalidade? modalidade)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long? DreId { get; set; }
        public Modalidade? Modalidade { get; set; }
    }

    public class ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator : AbstractValidator<ObterQuantidadeEncaminhamentoNAAPAEmAbertoQuery>
    {
        public ObterQuantidadeEncaminhamentoNAAPAEmAbertoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
