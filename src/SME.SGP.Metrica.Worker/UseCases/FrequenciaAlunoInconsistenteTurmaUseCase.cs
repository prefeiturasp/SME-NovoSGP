using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FrequenciaAlunoInconsistenteTurmaUseCase : IFrequenciaAlunoInconsistenteTurmaUseCase
    {
        private readonly IRepositorioFrequenciaAlunoInconsistente repositorioFrequenciaAlunoInconsistente;
        private readonly IRepositorioSGPConsulta repositorioSGP;

        public FrequenciaAlunoInconsistenteTurmaUseCase(IRepositorioFrequenciaAlunoInconsistente repositorioFrequenciaAlunoInconsistente,
                                                        IRepositorioSGPConsulta repositorioSGPConsulta)
        {
            this.repositorioFrequenciaAlunoInconsistente = repositorioFrequenciaAlunoInconsistente ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaAlunoInconsistente));
            this.repositorioSGP = repositorioSGPConsulta ?? throw new System.ArgumentNullException(nameof(repositorioSGPConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turma = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var frequenciasInconsistentes = await repositorioSGP.ObterFrequenciaAlunoInconsistente(turma.Id);

            foreach (var frequenciaInconsistente in frequenciasInconsistentes)
                await repositorioFrequenciaAlunoInconsistente.InserirAsync(frequenciaInconsistente);

            return true;
        }
    }
}
