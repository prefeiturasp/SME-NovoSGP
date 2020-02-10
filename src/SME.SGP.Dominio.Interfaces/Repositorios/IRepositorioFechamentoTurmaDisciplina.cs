using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplina : IRepositorioBase<FechamentoTurmaDisciplina>
    {
        Task<IEnumerable<NotaConceitoBimestreDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre);
    }
}