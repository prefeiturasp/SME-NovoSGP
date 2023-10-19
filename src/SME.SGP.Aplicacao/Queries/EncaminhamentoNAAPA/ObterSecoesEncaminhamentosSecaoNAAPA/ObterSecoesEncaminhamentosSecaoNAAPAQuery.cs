using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesEncaminhamentosSecaoNAAPAQuery : IRequest<IEnumerable<SecaoQuestionarioDto>>
    {
        public ObterSecoesEncaminhamentosSecaoNAAPAQuery(int? modalidade, long? encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
            Modalidade = modalidade;
        }

        public long? EncaminhamentoNAAPAId { get; }
        public int? Modalidade { get; }
    }
}
