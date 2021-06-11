using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterRestruturacoesPlanoAEEPorIdUseCase : IUseCase<long, IEnumerable<PlanoAEEReestruturacaoDto>>
    {
    }
}
