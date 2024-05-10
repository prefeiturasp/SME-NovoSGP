using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarTipoCalendarioUseCase : IExecutarTipoCalendarioUseCase
    {
        public class ExecutarTipoCalendarioParametro
        {
            public TipoCalendarioDto Dto { get; set; }
            public TipoCalendario TipoCalendario { get; set; }
        }
        private readonly IComandosTipoCalendario comandosTipoCalendario;

        public ExecutarTipoCalendarioUseCase(IComandosTipoCalendario comandosTipoCalendario)
        {
            this.comandosTipoCalendario = comandosTipoCalendario;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<ExecutarTipoCalendarioParametro>();

            await comandosTipoCalendario.ExecutarReplicacao(data.Dto, false, data.TipoCalendario);
            return true;
        }
    }
}
