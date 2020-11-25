using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase : AbstractUseCase, IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase
    {
        

        public ExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
            
        }

        public async Task Executar(MensagemRabbit mensagem)
        {
            var anoAtual = DateTime.Now.Year;
            var parametroDiasAusenciaFechamento = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasGeracaoPendenciaAusenciaFechamento, anoAtual));

            await ExecutaVerificacaoPendenciaAusenciaFechamentoPorModalidade(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.FundamentalMedio);
            await ExecutaVerificacaoPendenciaAusenciaFechamentoPorModalidade(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.EJA);
            

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
