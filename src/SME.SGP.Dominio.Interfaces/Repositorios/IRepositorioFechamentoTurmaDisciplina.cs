using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplina : IRepositorioBase<FechamentoTurmaDisciplina>
    {
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, string[] disciplinasId, int bimestre = 0);

        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo, long disciplinaId, int bimestre = 0);

        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
    }
}