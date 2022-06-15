using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalCompensacoesComponenteNaoLancaNotaUseCase
    {

        Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> Executar(string codigoTurma, int bimestre);
    }
}
