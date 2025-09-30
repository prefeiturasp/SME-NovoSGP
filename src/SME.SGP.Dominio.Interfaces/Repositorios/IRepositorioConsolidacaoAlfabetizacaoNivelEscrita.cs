using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioConsolidacaoAlfabetizacaoNivelEscrita
    {
        Task ExcluirConsolidacaoNivelEscrita();
        Task<bool> SalvarConsolidacaoNivelEscrita(ConsolidacaoAlfabetizacaoNivelEscrita entidade);
        Task<IEnumerable<ConsolidacaoAlfabetizacaoNivelEscrita>> ObterNumeroAlunos(int anoLetivo, int periodo, string codigoDre = null, string codigoUe = null);
    }
}