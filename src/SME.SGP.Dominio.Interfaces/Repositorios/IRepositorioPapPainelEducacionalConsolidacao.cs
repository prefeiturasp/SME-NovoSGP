using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPapPainelEducacionalConsolidacao
    {
        Task LimparConsolidacao();
        Task<bool> Inserir(IEnumerable<ConsolidacaoInformacoesPap> indicadoresPap);
    }
}