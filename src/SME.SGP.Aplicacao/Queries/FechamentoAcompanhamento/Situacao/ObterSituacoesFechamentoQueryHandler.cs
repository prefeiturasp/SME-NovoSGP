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

            var naoInciado = new SituacaoDto
            {
                Codigo = (int)SituacaoFechamento.NaoIniciado,
                Descricao = SituacaoFechamento.NaoIniciado.ObterNome()
            };
           
            listaSituacoes.Add(naoInciado);
           
            if (request.UnificarNaoIniciado == false)
            {
                var emProcessamento = new SituacaoDto
                {
                    Codigo = (int)SituacaoFechamento.EmProcessamento,
                    Descricao = SituacaoFechamento.EmProcessamento.ObterNome()
                };

                listaSituacoes.Add(emProcessamento);
            }

            var processadoComPendencia = new SituacaoDto
            {
                Codigo = (int)SituacaoFechamento.ProcessadoComPendencias,
                Descricao = SituacaoFechamento.ProcessadoComPendencias.ObterNome()
            };

            var processadoComSucesso = new SituacaoDto
            {
                Codigo = (int)SituacaoFechamento.ProcessadoComSucesso,
                Descricao = SituacaoFechamento.ProcessadoComSucesso.ObterNome()
            };

            listaSituacoes.Add(processadoComPendencia);
            listaSituacoes.Add(processadoComSucesso);

            return listaSituacoes;

        }
    }
}
