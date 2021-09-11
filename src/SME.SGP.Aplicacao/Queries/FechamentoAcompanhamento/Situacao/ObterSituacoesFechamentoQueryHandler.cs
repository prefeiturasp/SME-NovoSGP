using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.FechamentoAcompanhamento.Situacao
{
    public class ObterSituacoesFechamentoQueryHandler : IRequestHandler<ObterSituacoesFechamentoQuery, List<SituacaoDto>>
    {
        public async Task<List<SituacaoDto>> Handle(ObterSituacoesFechamentoQuery request, CancellationToken cancellationToken)
        {
            var listaSituacoes = new List<SituacaoDto>();
            foreach (var status in Enum.GetValues(typeof(SituacaoFechamento)))
            {
                    var situacao = new SituacaoDto
                    {
                        Codigo = (int)status,
                        Descricao = ((SituacaoFechamento)status).ObterNome()
                    };
                    listaSituacoes.Add(situacao);
            }

            if (request.UnificarNaoIniciado)
                listaSituacoes.RemoveAt((int)(SituacaoFechamento.EmProcessamento));
            return listaSituacoes;
        }
    }
}
