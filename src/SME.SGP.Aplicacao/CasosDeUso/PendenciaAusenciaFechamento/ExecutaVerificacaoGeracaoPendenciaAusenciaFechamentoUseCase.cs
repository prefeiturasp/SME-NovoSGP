using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase : AbstractUseCase, IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase
    {


        public ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var anoAtual = DateTime.Now.Year;
            var parametroDiasAusenciaFechamento = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasGeracaoPendenciaAusenciaFechamento, anoAtual));

            await ExecutaVerificacaoPendenciaAusenciaFechamentoPorModalidade(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.FundamentalMedio);
            await ExecutaVerificacaoPendenciaAusenciaFechamentoPorModalidade(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.EJA);

            return true;
        }

        private async Task ExecutaVerificacaoPendenciaAusenciaFechamentoPorModalidade(IEnumerable<ParametrosSistema> parametroDiasAusenciaFechamento, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var parametroPendenciaAusencia = parametroDiasAusenciaFechamento.FirstOrDefault(c => c.Ativo && c.Nome == "DiasGeracaoPendenciaAusenciaFechamento");
            if (parametroPendenciaAusencia != null)
            {
                await mediator.Send(new ExecutarVerificacaoPendenciaAusenciaFechamentoCommand(int.Parse(parametroPendenciaAusencia.Valor), modalidadeTipoCalendario));
            }

        }

    }
}
