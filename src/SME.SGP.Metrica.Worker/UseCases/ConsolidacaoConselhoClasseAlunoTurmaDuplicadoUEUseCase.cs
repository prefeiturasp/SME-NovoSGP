using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase : IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado repositorioDuplicados;

        public ConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase(IRepositorioSGPConsulta repositorioSGP,
                                                                      IRepositorioConsolidacaoConselhoClasseAlunoTurmaDuplicado repositorioDuplicados)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
            this.repositorioDuplicados = repositorioDuplicados ?? throw new System.ArgumentNullException(nameof(repositorioDuplicados));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDto>();

            var registrosDuplicados = await repositorioSGP.ObterConsolidacaoCCAlunoTurmaDuplicados(ue.Id);

            foreach (var registroDuplicado in registrosDuplicados)
                await repositorioDuplicados.InserirAsync(registroDuplicado);

            return true;
        }
    }
}
