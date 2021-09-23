using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarTipoCalendarioUseCase : IExecutarTipoCalendario
    {
        public class ExecutarTipoCalendarioParametro
        {
            public TipoCalendarioDto Dto { get; set; }
            public TipoCalendario TipoCalendario { get; set; }
        }

        private readonly IMediator mediator;
        private readonly IComandosTipoCalendario comandosTipoCalendario;

        public ExecutarTipoCalendarioUseCase(IMediator mediator, IComandosTipoCalendario comandosTipoCalendario)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.comandosTipoCalendario = comandosTipoCalendario;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<ExecutarTipoCalendarioParametro>();

            await comandosTipoCalendario.ExecutarMetodosAsync(data.Dto, false, data.TipoCalendario);
            return true;
        }
    }
}
