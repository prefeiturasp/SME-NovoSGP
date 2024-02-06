using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaConsolidacaoCCNotaDuplicadoUseCase : ILimpezaConsolidacaoCCNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaConsolidacaoCCNotaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroDuplicado = mensagem.ObterObjetoMensagem<ConsolidacaoCCNotaDuplicado>();

            await repositorioSGP.ExcluirConsolidacaoCCNotaDuplicado(registroDuplicado.ConsolicacaoCCAlunoTurmaId,
                                                                    registroDuplicado.Bimestre,
                                                                    registroDuplicado.ComponenteCurricularId,
                                                                    registroDuplicado.UltimoId);

            return true;
        }
    }
}
