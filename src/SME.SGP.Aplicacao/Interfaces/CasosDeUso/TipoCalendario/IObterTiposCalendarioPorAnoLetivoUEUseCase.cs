using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTiposCalendarioPorAnoLetivoUEUseCase
    {
        Task<IEnumerable<TipoCalendarioDto>> Executar(int anoLetivo, string codigoUE);
    }
}
