﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasPorAnoUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasPorAnoUseCase
    {
        public ConsolidarFrequenciaTurmasPorAnoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {

            var filtro = mensagem.ObterObjetoMensagem<FiltroAnoDto>();

            var parametroPercentualMinimo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaCritico, filtro.Ano));
            var parametroPercentualMinimoInfantil = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaMinimaInfantil, filtro.Ano));

            await ConsolidarFrequenciasTurmas(filtro.Ano, double.Parse(parametroPercentualMinimo.Valor), double.Parse(parametroPercentualMinimoInfantil.Valor));

            return true;

        }

        private async Task ConsolidarFrequenciasTurmas(int ano, double percentualMinimo, double percentualMinimoInfantil)
        {
            var dres = await mediator.Send(new ObterIdsDresQuery());

            foreach(var dre in dres)
            {
                var filtro = new FiltroConsolidacaoFrequenciaTurmaPorDre(ano, dre, percentualMinimo, percentualMinimoInfantil);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre, filtro, Guid.NewGuid(), null));
            }
        }
    }
}
