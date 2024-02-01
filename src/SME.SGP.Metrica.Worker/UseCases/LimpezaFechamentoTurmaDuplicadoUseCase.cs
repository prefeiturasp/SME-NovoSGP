using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaFechamentoTurmaDuplicadoUseCase : ILimpezaFechamentoTurmaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaFechamentoTurmaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var fechamentoTurmaDuplicado = mensagem.ObterObjetoMensagem<FechamentoTurmaDuplicado>();

            await repositorioSGP.AtualizarConselhoClasseFechamentoTurmaDuplicados(fechamentoTurmaDuplicado.TurmaId,
                                                                                  fechamentoTurmaDuplicado.PeriodoEscolarId,
                                                                                  fechamentoTurmaDuplicado.UltimoId);

            await repositorioSGP.AtualizarComponenteFechamentoTurmaDuplicados(fechamentoTurmaDuplicado.TurmaId,
                                                                              fechamentoTurmaDuplicado.PeriodoEscolarId,
                                                                              fechamentoTurmaDuplicado.UltimoId);

            await repositorioSGP.ExcluirFechamentoTurmaDuplicados(fechamentoTurmaDuplicado.TurmaId,
                                                                  fechamentoTurmaDuplicado.PeriodoEscolarId,
                                                                  fechamentoTurmaDuplicado.UltimoId);

            return true;
        }
    }
}
