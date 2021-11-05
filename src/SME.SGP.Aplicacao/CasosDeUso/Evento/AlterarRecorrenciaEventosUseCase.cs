using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRecorrenciaEventosUseCase : AbstractUseCase, IAlterarRecorrenciaEventos
    {
        public class AlterarRecorrenciaEventosParametro
        {
            public Evento Evento { get; set; }
            public bool AlterarRecorrenciaCompleta { get; set; }
        }

        private readonly IServicoEvento servicoEvento;

        public AlterarRecorrenciaEventosUseCase(IMediator mediator, IServicoEvento servicoEvento) : base(mediator)
        {
            this.servicoEvento = servicoEvento;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dados = param.ObterObjetoMensagem<AlterarRecorrenciaEventosParametro>();
            servicoEvento.AlterarRecorrenciaEventos(dados.Evento, dados.AlterarRecorrenciaCompleta);
            return true;
        }
    }
}
