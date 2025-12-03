using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery : IRequest<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>>
    {
        public ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery>
    {
        public ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPAId)
                .NotEmpty()
                .WithMessage("O Id do Encaminhamento NAAPA deve ser informado para obter as seções de itinerância do encaminhamento.");
        }
    }

}
