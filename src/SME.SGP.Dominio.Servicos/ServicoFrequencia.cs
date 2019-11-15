using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFrequencia : IServicoFrequencia
    {
        private readonly IRepositorioFrequencia repositorioFrequencia;

        public ServicoFrequencia(IRepositorioFrequencia repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
        }

        public void ListarFrequenciaPorAula(long aulaId)
        {
            var frequencias = repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);
            for (int i = 0; i < frequencias.Count(); i++)
            {
            }
            foreach (var frequencia in frequencias)
            {
                var registroFrequenciaDto = new RegistroFrequenciaDto
                {
                    CodigoAluno = frequencia.CodigoAluno,
                    NumeroAula = frequencia.NumeroAula,
                    Migrado = frequencia.Migrado,
                };
            }
        }
    }
}