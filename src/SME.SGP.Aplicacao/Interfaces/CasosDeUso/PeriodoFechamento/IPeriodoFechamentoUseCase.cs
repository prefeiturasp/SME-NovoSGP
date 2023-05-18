using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IPeriodoFechamentoUseCase
    {
        Task<bool> Executar(string turmaCodigo, DateTime dataReferencia, int bimestre);
    }
}
