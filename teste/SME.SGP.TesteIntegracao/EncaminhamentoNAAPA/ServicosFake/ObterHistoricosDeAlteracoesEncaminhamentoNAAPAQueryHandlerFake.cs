﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandlerFake : IRequestHandler<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery, EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public async Task<EncaminhamentoNAAPAHistoricoAlteracoes> Handle(ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new EncaminhamentoNAAPAHistoricoAlteracoes
            {
                Id = 1,
                EncaminhamentoNAAPAId = 1,
                UsuarioId = 1,
                CamposInseridos = string.Empty,
                CamposAlterados = string.Empty,
                DataAtendimento = DateTime.Now.ToString(),
                DataHistorico = DateTime.Now,
                TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao
            });
        }
    }
}