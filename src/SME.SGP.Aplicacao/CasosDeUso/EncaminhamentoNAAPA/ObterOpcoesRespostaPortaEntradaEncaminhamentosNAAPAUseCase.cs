using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterOpcoesRespostaPortaEntradaEncaminhamentosNAAPAUseCase : IObterOpcoesRespostaPortaEntradaAtendimentosNAAPAUseCase
    {
        private readonly IMediator mediator;
        private const string NOME_COMPONENTE_QUESTAO_PORTA_ENTRADA = "PORTA_ENTRADA";

        public ObterOpcoesRespostaPortaEntradaEncaminhamentosNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoRespostaSimplesDto>> Executar()
        {
            return await mediator
                        .Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NOME_COMPONENTE_QUESTAO_PORTA_ENTRADA, TipoQuestionario.EncaminhamentoNAAPA));
        }
    }
}
