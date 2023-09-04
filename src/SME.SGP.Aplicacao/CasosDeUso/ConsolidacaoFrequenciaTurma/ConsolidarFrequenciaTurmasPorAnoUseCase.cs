using MediatR;
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

            var parametroPercentualMinimo = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaCritico, filtro.Data.Year));
            var parametroPercentualMinimoInfantil = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(Dominio.TipoParametroSistema.PercentualFrequenciaMinimaInfantil, filtro.Data.Year));

            await ConsolidarFrequenciasTurmas(filtro, double.Parse(parametroPercentualMinimo.Valor), double.Parse(parametroPercentualMinimoInfantil.Valor));

            return true;

        }

        private async Task ConsolidarFrequenciasTurmas(FiltroAnoDto filtroAno, double percentualMinimo, double percentualMinimoInfantil)
        {
            var dres = await mediator.Send(ObterIdsDresQuery.Instance);

            foreach(var dre in dres)
            {
                var filtro = new FiltroConsolidacaoFrequenciaTurmaPorDre(filtroAno.Data, filtroAno.TipoConsolidado, dre, percentualMinimo, percentualMinimoInfantil);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre, filtro, Guid.NewGuid(), null));
            }
        }
    }
}
