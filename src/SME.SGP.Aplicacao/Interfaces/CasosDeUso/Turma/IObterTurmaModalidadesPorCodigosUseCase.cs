using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTurmaModalidadesPorCodigosUseCase
    {
        Task<IEnumerable<TurmaModalidadeCodigoDto>> Executar(string[] turmaCodigos);
    }
}
