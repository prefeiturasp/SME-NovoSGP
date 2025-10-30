using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarResultadoInsatisfatorioUseCase : INotificarResultadoInsatisfatorioUseCase
    {
        private readonly IMediator mediator;

        public NotificarResultadoInsatisfatorioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var anoAtual = DateTime.Now.Year;
            var parametroDiasAusenciaFechamento = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.DiasNotificacaoResultadoInsatisfatorio, anoAtual));

            await NotificarResultadoInsatisfatorio(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.FundamentalMedio);
            await NotificarResultadoInsatisfatorio(parametroDiasAusenciaFechamento, ModalidadeTipoCalendario.EJA);

            return true;
        }

        private async Task NotificarResultadoInsatisfatorio(IEnumerable<ParametrosSistema> parametro, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            if (parametro == null)
                return;

            var parametroPendenciaAusencia = parametro.FirstOrDefault(c => c.Ativo && c.Nome == "DiasNotificacaoResultadoInsatisfatorio");
            if (parametroPendenciaAusencia.NaoEhNulo())
            {
                await mediator.Send(new NotificarResultadoInsatisfatorioCommand(int.Parse(parametroPendenciaAusencia.Valor), (long)modalidadeTipoCalendario));
            }
        }
    }
}
