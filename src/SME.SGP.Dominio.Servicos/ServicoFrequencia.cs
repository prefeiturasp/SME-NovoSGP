using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFrequencia : IServicoFrequencia
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ServicoFrequencia(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaAusenciasPorAula(long aulaId)
        {
            return repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);
        }
    }
}