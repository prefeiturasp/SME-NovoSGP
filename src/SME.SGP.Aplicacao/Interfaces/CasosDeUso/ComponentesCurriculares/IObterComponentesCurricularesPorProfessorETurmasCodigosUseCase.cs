using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase
    {
        Task<IEnumerable<DisciplinaNomeDto>> Executar(IEnumerable<string> codigoUe, bool realizarAgrupamentoComponente);
    }
}
