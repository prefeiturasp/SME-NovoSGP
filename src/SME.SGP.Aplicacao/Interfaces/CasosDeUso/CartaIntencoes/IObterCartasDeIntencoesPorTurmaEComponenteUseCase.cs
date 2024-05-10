using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterCartasDeIntencoesPorTurmaEComponenteUseCase: IUseCase<ObterCartaIntencoesDto, IEnumerable<CartaIntencoesRetornoDto>>
    {
    }
}
