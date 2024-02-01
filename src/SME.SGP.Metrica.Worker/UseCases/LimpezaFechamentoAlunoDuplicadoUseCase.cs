using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaFechamentoAlunoDuplicadoUseCase : ILimpezaFechamentoAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaFechamentoAlunoDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var fechamentoAlunoDuplicado = mensagem.ObterObjetoMensagem<FechamentoAlunoDuplicado>();

            await repositorioSGP.AtualizarNotaFechamentoAlunoDuplicados(fechamentoAlunoDuplicado.FechamentoTurmaDisciplinaId,
                                                                        fechamentoAlunoDuplicado.AlunoCodigo,
                                                                        fechamentoAlunoDuplicado.UltimoId);

            await repositorioSGP.AtualizarAnotacaoFechamentoAlunoDuplicados(fechamentoAlunoDuplicado.FechamentoTurmaDisciplinaId,
                                                                            fechamentoAlunoDuplicado.AlunoCodigo,
                                                                            fechamentoAlunoDuplicado.UltimoId);

            await repositorioSGP.ExcluirFechamentoAlunoDuplicados(fechamentoAlunoDuplicado.FechamentoTurmaDisciplinaId,
                                                                  fechamentoAlunoDuplicado.AlunoCodigo,
                                                                  fechamentoAlunoDuplicado.UltimoId);

            return true;
        }
    }
}
