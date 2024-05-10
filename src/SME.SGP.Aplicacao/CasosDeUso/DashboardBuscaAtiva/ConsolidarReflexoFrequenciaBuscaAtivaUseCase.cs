﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarReflexoFrequenciaBuscaAtivaUseCase : AbstractUseCase, IConsolidarReflexoFrequenciaBuscaAtivaUseCase
    {
        public ConsolidarReflexoFrequenciaBuscaAtivaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.EhNulo() || mensagemRabbit.Mensagem.EhNulo()
                            ? new FiltroIdAnoLetivoDto(0, DateTime.Now.Date.AddDays(-1))
                            : mensagemRabbit.ObterObjetoMensagem<FiltroIdAnoLetivoDto>();

            var dres = await mediator.Send(ObterIdsDresQuery.Instance);
            foreach (long dreId in dres.Where(dreId => filtro.Id.Equals(0) 
                                                       || dreId.Equals(filtro.Id)).ToList())
            {
                filtro.Id = dreId;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaDre, filtro, Guid.NewGuid()));
            }
            return true;
        }
    }
}
