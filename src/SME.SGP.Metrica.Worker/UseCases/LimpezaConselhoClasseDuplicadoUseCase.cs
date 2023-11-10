using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaConselhoClasseDuplicadoUseCase : ILimpezaConselhoClasseDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaConselhoClasseDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var conselhoClasseDuplicado = mensagem.ObterObjetoMensagem<ConselhoClasseDuplicado>();
            await repositorioSGP.AtualizarConselhosClasseDuplicados(conselhoClasseDuplicado.FechamentoTurmaId, conselhoClasseDuplicado.UltimoId);
            await repositorioSGP.ExcluirConselhosClasseDuplicados(conselhoClasseDuplicado.FechamentoTurmaId, conselhoClasseDuplicado.UltimoId);

            return true;
        }
    }
}
