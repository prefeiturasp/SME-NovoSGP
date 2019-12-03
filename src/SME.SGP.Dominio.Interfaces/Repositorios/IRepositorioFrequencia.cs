using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId);

        RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId);

        IEnumerable<RegistroFrequenciaFaltanteDto> ObterTurmasSemRegistroDeFrequencia();

        Task ExcluirFrequenciaAula(long aulaId);
    }
}