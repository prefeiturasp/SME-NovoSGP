using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoNotaConselho
    {
        Task Salvar(WFAprovacaoNotaConselho entidade);
    }
}
