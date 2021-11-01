using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoCJ : IRepositorioBase<AtribuicaoCJ>
    {
        IEnumerable<AtribuicaoCJ> ObterAtribuicaoAtiva(string professorRf);
        
        Task<IEnumerable<AtribuicaoCJ>> ObterAtribuicaoAtivaAsync(string professorRf);

        Task<IEnumerable<AtribuicaoCJ>> ObterPorFiltros(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId, string usuarioRf, string usuarioNome, bool? substituir, string dreCodigo = "", string[] turmaIds = null, int? anoLetivo = null, bool? historico = null);
        Task<bool> PossuiAtribuicaoPorDreUeETurma(string turmaId, string dreCodigo, string ueCodigo, string professorRf);
        Task<IEnumerable<int>> ObterAnosDisponiveis(string professorRF, bool consideraHistorico);
        Task<bool> RemoverRegistros(string dreCodigo, string ueCodigo, string turmaCodigo, string professorRf, long disciplinaId = 0);
        Task<bool> PossuiAtribuicaoPorTurmaRFAnoLetivo(string turmaCodigo, string rfProfessor, long disciplinaId);
    }
}