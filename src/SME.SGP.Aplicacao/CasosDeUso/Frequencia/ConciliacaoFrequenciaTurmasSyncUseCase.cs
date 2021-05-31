using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasSyncUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasSyncUseCase
    {
        public ConciliacaoFrequenciaTurmasSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmasSyncDto>();
            await mediator.Send(new ConciliacaoFrequenciaTurmasCommand(filtro.DataPeriodo, filtro.TurmaCodigo, string.Empty));
            return true;
        }
    }
}
