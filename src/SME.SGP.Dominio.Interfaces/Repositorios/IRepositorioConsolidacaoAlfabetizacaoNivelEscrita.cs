using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsolidacaoAlfabetizacaoNivelEscrita
    {
        Task ExcluirConsolidacaoNivelEscrita();
        Task<bool> SalvarConsolidacaoNivelEscrita(ConsolidacaoAlfabetizacaoNivelEscrita entidade);
    }
}