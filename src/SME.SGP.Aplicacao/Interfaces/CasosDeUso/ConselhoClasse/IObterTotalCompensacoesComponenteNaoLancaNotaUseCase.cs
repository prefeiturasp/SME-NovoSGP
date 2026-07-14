using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalCompensacoesComponenteNaoLancaNotaUseCase
    {

        Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> Executar(string codigoTurma, int bimestre);
    }
}
