using SME.SGP.Dominio.Entidades;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita
    {
        Task ExcluirConsolidacaoAlfabetizacaoCriticaEscrita();
        Task<bool> SalvarConsolidacaoAlfabetizacaoCriticaEscrita(ConsolidacaoAlfabetizacaoCriticaEscrita entidade);
    }
}
