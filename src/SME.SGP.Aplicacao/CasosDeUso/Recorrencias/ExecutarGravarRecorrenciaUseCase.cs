using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarGravarRecorrenciaUseCase : AbstractUseCase, IExecutarGravarRecorrencia
    {
        private readonly IComandosEvento comandosEvento;

        public class ExecutarGravarRecorrenciaParametro
        {
            public EventoDto Dto { get; set; }
            public Evento Evento { get; set; }
        }

        public ExecutarGravarRecorrenciaUseCase(IMediator mediator, IComandosEvento comandosEvento) : base(mediator)
        {
            this.comandosEvento = comandosEvento;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<ExecutarGravarRecorrenciaParametro>();
            await comandosEvento.GravarRecorrencia(data.Dto, data.Evento);
            return true;
        }
    }

}