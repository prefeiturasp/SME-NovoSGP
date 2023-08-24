﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            bool possuiAbrangenciaUE = usuario.EhAbrangenciaSomenteUE();

            if (possuiAbrangenciaUE)
            {
                long usuarioId = await mediator.Send(ObterUsuarioLogadoIdQuery.Instance);
                int anoLetivo = DateTime.Now.Year;
                return await mediator.Send(new ObterTurmasPorAnoEUsuarioIdQuery(usuarioId, anoLetivo));
            }

            return Enumerable.Empty<TurmaNaoHistoricaDto>();
        }
    }
}
