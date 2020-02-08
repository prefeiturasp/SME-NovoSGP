using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoTurmaDisciplina
    {
        Task<IEnumerable<AuditoriaDto>> Inserir(IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma);
        Task<IEnumerable<AuditoriaDto>> Alterar(long id, IEnumerable<FechamentoTurmaDisciplinaDto> fechamentosTurma);
    }
}
