using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioTaxaAlfabetizacao : IRepositorioBase<TaxaAlfabetizacao>
    {
        Task<TaxaAlfabetizacao> ObterRegistroAlfabetizacaoAsync(int anoLetivo, string codigoEOLEscola);
        Task<IEnumerable<TaxaAlfabetizacaoDto>> ObterTaxaAlfabetizacaoAsync();
    }
}
