using MediatR;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FrequenciaAlunoInconsistenteUEUseCase : IFrequenciaAlunoInconsistenteUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IMediator mediator;

        public FrequenciaAlunoInconsistenteUEUseCase(IRepositorioSGPConsulta repositorioSGP, IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();
            var turmas = await repositorioSGP.ObterTurmasIdsPorUE(ue.Id);

            foreach(var turma in turmas)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.FrequenciaAlunoInconsistenteTurma, new FiltroIdDto(turma)));

            return true;
        }
    }
}
