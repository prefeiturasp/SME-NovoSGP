using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAtribuicaoCJ
    {
        Task Salvar(AtribuicaoCJ atribuicaoCJ, IEnumerable<ProfessorTitularDisciplinaEol> professoresTitularesDisciplinasEol, IEnumerable<AtribuicaoCJ> atribuicoesAtuais = null);
    }
}