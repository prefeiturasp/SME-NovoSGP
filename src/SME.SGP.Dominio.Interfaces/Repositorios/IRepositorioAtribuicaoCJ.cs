using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoCJ : IRepositorioBase<AtribuicaoCJ>
    {
        IEnumerable<AtribuicaoCJ> ObterAtribuicaoAtiva(string professorRf);

        Task<IEnumerable<AtribuicaoCJ>> ObterPorFiltros(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId, string usuarioRf, string usuarioNome, bool? substituir, string dreCodigo = "", string[] turmaIds = null, int? anoLetivo = null);
    }
}