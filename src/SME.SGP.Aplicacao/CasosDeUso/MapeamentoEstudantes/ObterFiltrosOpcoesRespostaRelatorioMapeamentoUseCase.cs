using MediatR;
using SME.SGP.Aplicacao.Constantes;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios.MapeamentoEstudantes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase : IObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase
    {
        private readonly IMediator mediator;
        
        public ObterFiltrosOpcoesRespostaRelatorioMapeamentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto> Executar()
        {
            var retorno = new OpcoesRespostaFiltroRelatorioMapeamentoEstudanteDto();
            var opcoesResposta = await mediator.Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NomesComponentesMapeamentoEstudante.DISTORCAO_IDADE_ANO_SERIE, TipoQuestionario.MapeamentoEstudante));
            retorno.OpcoesRespostaDistorcaoIdadeAnoSerie.AddRange(opcoesResposta);

            opcoesResposta = await mediator.Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NomesComponentesMapeamentoEstudante.POSSUI_PLANO_AEE, TipoQuestionario.MapeamentoEstudante));
            retorno.OpcoesRespostaPossuiPlanoAEE.AddRange(opcoesResposta);

            opcoesResposta = await mediator.Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NomesComponentesMapeamentoEstudante.ACOMPANHADO_NAAPA, TipoQuestionario.MapeamentoEstudante));
            retorno.OpcoesRespostaAcompanhadoNAAPA.AddRange(opcoesResposta);

            opcoesResposta = await mediator.Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NomesComponentesMapeamentoEstudante.PROGRAMA_SAO_PAULO_INTEGRAL, TipoQuestionario.MapeamentoEstudante));
            retorno.OpcoesRespostaProgramaSPIntegral.AddRange(opcoesResposta);

            opcoesResposta = await mediator.Send(new ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionarioQuery(NomesComponentesMapeamentoEstudante.FREQUENCIA, TipoQuestionario.MapeamentoEstudante));
            retorno.OpcoesRespostaFrequencia.AddRange(opcoesResposta);

            return retorno;
        }

    }
}
