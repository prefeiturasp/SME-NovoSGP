using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasNaoHistoricasUseCase : AbstractUseCase, IObterTurmasNaoHistoricasUseCase
    {
        public ObterTurmasNaoHistoricasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Executar()
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            bool possuiAbrangenciaUE = usuario.EhAbrangenciaSomenteUE();

            if (possuiAbrangenciaUE)
            {
                long usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
                int anoLetivo = DateTime.Now.Year;
                return await mediator.Send(new ObterTurmasPorAnoEUsuarioIdQuery(usuarioId, anoLetivo));
            }

            return new List<TurmaNaoHistoricaDto>();
        }
    }
}
