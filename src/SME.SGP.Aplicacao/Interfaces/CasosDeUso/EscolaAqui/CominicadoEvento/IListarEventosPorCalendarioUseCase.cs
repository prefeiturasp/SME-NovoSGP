using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui
{
    public interface IListarEventosPorCalendarioUseCase
    {
        Task<IEnumerable<ListarEventosPorCalendarioRetornoDto>> Executar(int tipoCalendario, int anoLetivo, string codigoDre, string codigoUe, int? modalidade);
    }    
}
