using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaRegistroFrequenciaAlunoDuplicadoUseCase : ILimpezaRegistroFrequenciaAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaRegistroFrequenciaAlunoDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroDuplicado = mensagem.ObterObjetoMensagem<RegistroFrequenciaAlunoDuplicado>();

            var possuiCompensacao =
                await repositorioSGP.AtualizarCompensacoesRegistroFrequenciaAlunoDuplicado(registroDuplicado.RegistroFrequenciaId,
                                                                                      registroDuplicado.AulaId,
                                                                                      registroDuplicado.NumeroAula,
                                                                                      registroDuplicado.AlunoCodigo,
                                                                                      registroDuplicado.UltimoId);
            if (possuiCompensacao)
                await repositorioSGP.AtualizarAusenciaRegistroFrequenciaAlunoDuplicado(registroDuplicado.UltimoId);

            await repositorioSGP.ExcluirRegistroFrequenciaAlunoDuplicado(registroDuplicado.RegistroFrequenciaId,
                                                                         registroDuplicado.AulaId,
                                                                         registroDuplicado.NumeroAula,
                                                                         registroDuplicado.AlunoCodigo,
                                                                         registroDuplicado.UltimoId);

            return true;
        }
    }
}
