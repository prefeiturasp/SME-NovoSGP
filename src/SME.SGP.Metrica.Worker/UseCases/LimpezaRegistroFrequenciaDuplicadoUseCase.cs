using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaRegistroFrequenciaDuplicadoUseCase : ILimpezaRegistroFrequenciaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaRegistroFrequenciaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroDuplicado = mensagem.ObterObjetoMensagem<RegistroFrequenciaDuplicado>();

            await repositorioSGP.AtualizaAlunoRegistroFrequenciaDuplicado(registroDuplicado.AulaId, registroDuplicado.UltimoId);
            await repositorioSGP.ExcluirRegistroFrequenciaDuplicado(registroDuplicado.AulaId, registroDuplicado.UltimoId);

            return true;
        }
    }
}
