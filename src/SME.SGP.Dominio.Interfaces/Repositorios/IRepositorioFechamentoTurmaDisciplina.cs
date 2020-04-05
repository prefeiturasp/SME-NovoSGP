using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplina : IRepositorioBase<FechamentoTurmaDisciplina>
    {
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, string[] disciplinasId, int bimestre);

        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre);

        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
    }
}