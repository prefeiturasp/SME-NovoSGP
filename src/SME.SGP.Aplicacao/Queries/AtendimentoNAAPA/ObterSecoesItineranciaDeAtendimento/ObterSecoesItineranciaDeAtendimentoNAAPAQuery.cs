using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesItineranciaDeAtendimentoNAAPAQuery : IRequest<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>>
    {
        public ObterSecoesItineranciaDeAtendimentoNAAPAQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterSecoesItineranciaDeAtendimentoNAAPAQueryValidator : AbstractValidator<ObterSecoesItineranciaDeAtendimentoNAAPAQuery>
    {
        public ObterSecoesItineranciaDeAtendimentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .NotEmpty()
                .WithMessage("O Id do Atendimento NAAPA deve ser informado para obter as seções de itinerância do atendimento.");
        }
    }

}
