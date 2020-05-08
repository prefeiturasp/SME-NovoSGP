using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterImpressaoPorTurmaUseCase
    {
        public static async Task<EventosAulasNoDiaCalendarioDto> Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo,
          IServicoUsuario servicoUsuario, IServicoEOL servicoEOL)
        {
        }
    }
}
