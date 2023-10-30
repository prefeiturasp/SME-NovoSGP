using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaFrequenciaAlunoDuplicadoUseCase : ILimpezaFrequenciaAlunoDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaFrequenciaAlunoDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroDuplicado = mensagem.ObterObjetoMensagem<FrequenciaAlunoDuplicado>();

            await repositorioSGP.ExcluirFrequenciaAlunoDuplicado(registroDuplicado.TurmaCodigo,
                                                                 registroDuplicado.AlunoCodigo,
                                                                 registroDuplicado.Bimestre,
                                                                 registroDuplicado.Tipo,
                                                                 registroDuplicado.ComponenteCurricularId,
                                                                 registroDuplicado.UltimoId);

            return true;
        }
    }
}
