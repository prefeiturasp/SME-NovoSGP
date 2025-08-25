using SME.SGP.Dominio.Entidades;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalRegistroFrequenciaAgrupamentoMensal : IRepositorioBase<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>
    {
        Task ExcluirFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade);
        Task<bool> SalvarFrequenciaMensal(PainelEducacionalRegistroFrequenciaAgrupamentoMensal entidade);
        Task<IEnumerable<PainelEducacionalRegistroFrequenciaAgrupamentoMensal>> ObterFrequenciaMensal(string codigoDre, string codigoUe);
    }
}
