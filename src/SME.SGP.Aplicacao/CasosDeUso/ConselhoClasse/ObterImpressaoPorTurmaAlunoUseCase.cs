using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterImpressaoPorTurmaAlunoUseCase
    {
        public static async Task Executar(IMediator mediator, FiltroAulasEventosCalendarioDto filtroAulasEventosCalendarioDto, long tipoCalendarioId, int mes, int dia, int anoLetivo,
          IServicoUsuario servicoUsuario, IServicoEOL servicoEOL)
        {
        }
    }
}
