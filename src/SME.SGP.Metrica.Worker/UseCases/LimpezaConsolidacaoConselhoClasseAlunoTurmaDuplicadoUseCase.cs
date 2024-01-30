using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase : ILimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroDuplicado = mensagem.ObterObjetoMensagem<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>();

            await repositorioSGP.AtualizarNotaConsolidacaoCCAlunoTurmaDuplicado(registroDuplicado.AlunoCodigo,
                                                                                registroDuplicado.TurmaId,
                                                                                registroDuplicado.UltimoId);

            await repositorioSGP.ExcluirConsolidacaoCCAlunoTurmaDuplicado(registroDuplicado.AlunoCodigo,
                                                                          registroDuplicado.TurmaId,
                                                                          registroDuplicado.UltimoId);

            return true;
        }
    }
}
