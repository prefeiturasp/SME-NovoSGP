using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal
    {
        Task ExcluirFrequenciaMensal(int anoLetivo);
        Task<bool> SalvarFrequenciaMensal(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> consolidacoes);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>> ObterFrequenciaMensal(int anoLetivo, string codigoDre, string codigoUe);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal> registros);
    }
}
