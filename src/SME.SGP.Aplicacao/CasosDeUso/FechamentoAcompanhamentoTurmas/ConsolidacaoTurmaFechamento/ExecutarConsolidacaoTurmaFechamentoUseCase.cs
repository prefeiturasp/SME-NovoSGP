using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoTurmaFechamentoUseCase : AbstractUseCase, IExecutarConsolidacaoTurmaFechamentoUseCase
    {
        public ExecutarConsolidacaoTurmaFechamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var (parametroConsolidacaoTurma, componentesCurriculares, dadosTurmaEOL, parametrosValidos) = await ValidarParametros(mensagemRabbit);
            if (!parametrosValidos) return false;

            if (!dadosTurmaEOL.Extinta)
            {
                foreach (var componente in componentesCurriculares)
                {
                    var mensagem = JsonConvert.SerializeObject(new FechamentoConsolidacaoTurmaComponenteBimestreDto(parametroConsolidacaoTurma.TurmaId, parametroConsolidacaoTurma.Bimestre, Convert.ToInt64(componente.Codigo)));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoComponenteTratar, mensagem, mensagemRabbit.CodigoCorrelacao, null));
                }
            }
            else
            {
                int bimestre = parametroConsolidacaoTurma.Bimestre.HasValue ? parametroConsolidacaoTurma.Bimestre.Value : 0;
                var consolidacoesTurmaBimestre = await mediator.Send(new ObterFechamentoConsolidadoPorTurmaBimestreQuery(parametroConsolidacaoTurma.TurmaId, bimestre, new int[] { -99 }));

                if (consolidacoesTurmaBimestre.Any())
                    await mediator.Send(new ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand(parametroConsolidacaoTurma.TurmaId, bimestre));
            }
            return true;
        }

        private async Task<(ConsolidacaoTurmaDto parametroConsolidacaoTurma, IEnumerable<ComponenteCurricularDto> componentesCurriculares, 
                            DadosTurmaEolDto dadosTurma, bool parametrosValidos)> ValidarParametros(MensagemRabbit mensagemRabbit)
        {
            var consolidacaoTurma = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurma.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma e o bimestre não foram informados", LogNivel.Negocio, LogContexto.Fechamento));
                return (consolidacaoTurma, null, null, false);
            }

            if (consolidacaoTurma.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma não foi informado", LogNivel.Negocio, LogContexto.Fechamento));
                return (consolidacaoTurma, null, null, false);
            }

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(consolidacaoTurma.TurmaId));
            if (turma.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível encontrar a turma de id {consolidacaoTurma.TurmaId}.", LogNivel.Negocio, LogContexto.Fechamento));
                return (consolidacaoTurma, null, null, false);
            }

            var componentes = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery(new string[] { turma.CodigoTurma }, turma.Ue.CodigoUe));
            if (componentes.NaoPossuiRegistros())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível encontrar os componentes curricularres da turma de id {consolidacaoTurma.TurmaId}.", LogNivel.Negocio, LogContexto.Fechamento));
                return (consolidacaoTurma, componentes, null, false);
            }

            var dadosTurmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(turma.CodigoTurma));
            if (dadosTurmaEOL.EhNulo())
                return (consolidacaoTurma, componentes, dadosTurmaEOL, false);

            return (consolidacaoTurma, componentes, dadosTurmaEOL, true);
        }
    }
}
