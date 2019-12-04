using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAtribuicaoCJ
    {
        Task Salvar(AtribuicaoCJ atribuicaoCJ, IEnumerable<AtribuicaoCJ> atribuicoesAtuais = null);
    }
}