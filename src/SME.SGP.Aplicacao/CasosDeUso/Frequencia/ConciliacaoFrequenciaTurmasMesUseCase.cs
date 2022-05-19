using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasMesUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasMesUseCase
    {
        public ConciliacaoFrequenciaTurmasMesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var dto = mensagem.ObterObjetoMensagem<ConciliacaoFrequenciaTurmaMensalDto>();

            foreach (var turma in dto.TurmasDaModalidade)
                await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaMesCommand(turma, dto.Mes));

            return true;
        }
    }
}
