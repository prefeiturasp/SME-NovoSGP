using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaConselhoClasseAlunoDuplicadoUseCase : ILimpezaConselhoClasseAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaConselhoClasseAlunoDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var conselhoClasseAlunoDuplicado = mensagem.ObterObjetoMensagem<ConselhoClasseAlunoDuplicado>();

            await repositorioSGP.AtualizarNotasConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                            conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                            conselhoClasseAlunoDuplicado.UltimoId);

            await repositorioSGP.AtualizarRecomendacoesConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                                    conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                                    conselhoClasseAlunoDuplicado.UltimoId);

            await repositorioSGP.ExcluirTurmasComplementaresConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                                         conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                                         conselhoClasseAlunoDuplicado.UltimoId);

            await repositorioSGP.AtualizarWfAprovacaoConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                                  conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                                  conselhoClasseAlunoDuplicado.UltimoId);

            await repositorioSGP.AtualizarParecerConclusivoConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                                        conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                                        conselhoClasseAlunoDuplicado.UltimoId);

            await repositorioSGP.ExcluirConselhoClasseAlunoDuplicado(conselhoClasseAlunoDuplicado.ConselhoClasseId,
                                                                     conselhoClasseAlunoDuplicado.AlunoCodigo,
                                                                     conselhoClasseAlunoDuplicado.UltimoId);

            return true;
        }
    }
}
