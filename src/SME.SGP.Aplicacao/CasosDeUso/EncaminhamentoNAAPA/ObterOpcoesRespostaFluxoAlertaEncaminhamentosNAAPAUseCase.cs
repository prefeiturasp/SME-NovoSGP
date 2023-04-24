﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterOpcoesRespostaFluxoAlertaEncaminhamentosNAAPAUseCase : IObterOpcoesRespostaFluxoAlertaEncaminhamentosNAAPAUseCase
    {
        private readonly IMediator mediator;
        private const string NOME_COMPONENTE_QUESTAO_FLUXO_ALERTA = "FLUXO_ALERTA";

        public ObterOpcoesRespostaFluxoAlertaEncaminhamentosNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoRespostaSimplesDto>> Executar()
        {
            return await mediator
                        .Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NOME_COMPONENTE_QUESTAO_FLUXO_ALERTA, TipoQuestionario.EncaminhamentoNAAPA));
        }
    }
}
