using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase : IUseCase<FiltroQuantidadeEncaminhamentoNAAPAEmAbertoDto, IEnumerable<QuantidadeEncaminhamentoNAAPAEmAbertoDto>>
    {
    }
}
