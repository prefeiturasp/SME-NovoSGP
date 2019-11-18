using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ComandoFrequencia : IComandoFrequencia
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ComandoFrequencia(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
        }

        public void Registrar(FrequenciaDto frequenciaDto)
        {
            var registro = repositorioFrequencia.ObterListaFrequenciaPorAula(frequenciaDto.AulaId);
            registro
        }
    }
}