using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmaDreSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmaDreSyncUseCase
    {
        public ConciliacaoFrequenciaTurmaDreSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmaDreSyncDto>();
            await mediator.Send(new ConciliacaoFrequenciaTurmaDreCommand(filtro.DreId, filtro.AnoLetivo));
            return true;
        }
    }
}
