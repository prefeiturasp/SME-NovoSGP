using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioWFAprovacaoParecerConclusivo
    {
        Task Salvar(WFAprovacaoParecerConclusivo entidade);
    }
}
