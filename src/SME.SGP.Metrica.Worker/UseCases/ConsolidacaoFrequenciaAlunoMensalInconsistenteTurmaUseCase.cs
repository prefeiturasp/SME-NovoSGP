using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase : IConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;
        private readonly IRepositorioConsolidacaoFrequenciaAlunoMensalInconsistente repositorioInconsistencia;

        public ConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase(IRepositorioSGP repositorioSGP,
                                                                          IRepositorioConsolidacaoFrequenciaAlunoMensalInconsistente repositorioInconsistencia)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioInconsistencia = repositorioInconsistencia ?? throw new System.ArgumentNullException(nameof(repositorioInconsistencia));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turma = mensagem.ObterObjetoMensagem<FiltroIdDto>();
            var registrosInconsistentes = await repositorioSGP.ObterConsolidacaoFrequenciaAlunoMensalInconsistente(turma.Id);

            foreach(var registroInconsistente in registrosInconsistentes)
                await repositorioInconsistencia.InserirAsync(registroInconsistente);

            return true;
        }
    }
}
