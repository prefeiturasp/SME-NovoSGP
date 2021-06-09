using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTiposCalendarioPorAnoLetivoModalidadeUseCase
    {
        Task<IEnumerable<TipoCalendarioDto>> Executar(int anoLetivo, Modalidade[] modalidades);
    }
}
