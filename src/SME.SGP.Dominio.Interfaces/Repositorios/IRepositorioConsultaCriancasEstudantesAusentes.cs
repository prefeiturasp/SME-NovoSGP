using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsultaCriancasEstudantesAusentes
    {
        Task<IEnumerable<AlunosAusentesDto>> ObterTurmasAlunosAusentes(FiltroObterAlunosAusentesDto filtro);
    }
}
