using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAtendimentoNAAPAEmAbertoQuery : IRequest<GraficoAtendimentoNAAPADto>
    {
        public ObterQuantidadeAtendimentoNAAPAEmAbertoQuery(int anoLetivo, long? dreId, Modalidade? modalidade)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            Modalidade = modalidade;
        }

        public int AnoLetivo { get; set; }
        public long? DreId { get; set; }
        public Modalidade? Modalidade { get; set; }
    }

    public class ObterQuantidadeAtendimentoNAAPAEmAbertoQueryValidator : AbstractValidator<ObterQuantidadeAtendimentoNAAPAEmAbertoQuery>
    {
        public ObterQuantidadeAtendimentoNAAPAEmAbertoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
