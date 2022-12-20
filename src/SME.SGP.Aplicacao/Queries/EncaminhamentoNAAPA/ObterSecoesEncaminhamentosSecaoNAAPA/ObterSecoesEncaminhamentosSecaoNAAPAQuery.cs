using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentosSecaoNAAPAQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentosSecaoNAAPAQuery(int modalidade, long? encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            Modalidade = modalidade;
        }

        public long? EncaminhamentoNAAPAId { get; }
        public int Modalidade { get; }
    }

    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator : AbstractValidator<ObterSecoesEncaminhamentosSecaoNAAPAQuery>
    {
        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryValidator()
        {
            RuleFor(c => c.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada para obter as seções/encaminhamentos seção do encaminhamento NAAPA.");
        }
    }

}
