using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFeriadoCalendario
    {
        FeriadoCalendarioCompletoDto BuscarPorId(long id);

        Task<IEnumerable<FeriadoCalendarioDto>> Listar(FiltroFeriadoCalendarioDto filtro);

        IEnumerable<EnumeradoRetornoDto> ObterAbrangencias();

        IEnumerable<EnumeradoRetornoDto> ObterTipos();
    }
}