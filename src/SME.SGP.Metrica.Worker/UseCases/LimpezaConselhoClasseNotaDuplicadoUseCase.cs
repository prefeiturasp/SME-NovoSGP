using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaConselhoClasseNotaDuplicadoUseCase : ILimpezaConselhoClasseNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaConselhoClasseNotaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var conselhoClasseNotaDuplicado = mensagem.ObterObjetoMensagem<ConselhoClasseNotaDuplicado>();

            await repositorioSGP.AtualizarWfAprovacaoConselhoClasseNotaDuplicado(conselhoClasseNotaDuplicado.ConselhoClasseAlunoId,
                                                                                 conselhoClasseNotaDuplicado.ComponenteCorricularId,
                                                                                 conselhoClasseNotaDuplicado.UltimoId);

            await repositorioSGP.AtualizarHistoricoNotaConselhoClasseNotaDuplicado(conselhoClasseNotaDuplicado.ConselhoClasseAlunoId,
                                                                                   conselhoClasseNotaDuplicado.ComponenteCorricularId,
                                                                                   conselhoClasseNotaDuplicado.UltimoId);

            await repositorioSGP.AtualizarMaiorNotaConselhoClasseNotaDuplicado(conselhoClasseNotaDuplicado.ConselhoClasseAlunoId,
                                                                               conselhoClasseNotaDuplicado.ComponenteCorricularId,
                                                                               conselhoClasseNotaDuplicado.UltimoId);

            await repositorioSGP.ExcluirConselhoClasseNotaDuplicado(conselhoClasseNotaDuplicado.ConselhoClasseAlunoId,
                                                                    conselhoClasseNotaDuplicado.ComponenteCorricularId,
                                                                    conselhoClasseNotaDuplicado.UltimoId);

            return true;
        }
    }
}
