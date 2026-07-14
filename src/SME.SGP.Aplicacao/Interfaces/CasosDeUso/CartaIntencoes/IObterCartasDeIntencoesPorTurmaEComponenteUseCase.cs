using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterCartasDeIntencoesPorTurmaEComponenteUseCase: IUseCase<ObterCartaIntencoesDto, IEnumerable<CartaIntencoesRetornoDto>>
    {
    }
}
