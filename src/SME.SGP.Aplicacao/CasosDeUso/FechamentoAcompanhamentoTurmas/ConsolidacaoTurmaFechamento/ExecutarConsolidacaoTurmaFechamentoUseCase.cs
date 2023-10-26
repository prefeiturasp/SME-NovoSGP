using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
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
            var consolidacaoTurma = mensagemRabbit.ObterObjetoMensagem<ConsolidacaoTurmaDto>();

            if (consolidacaoTurma.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma e o bimestre não foram informados", LogNivel.Negocio, LogContexto.Fechamento));                
                return false;
            }

            if (consolidacaoTurma.TurmaId == 0)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível iniciar a consolidação do fechamento da turma. O id da turma não foi informado", LogNivel.Negocio, LogContexto.Fechamento));                
                return false;
            }

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(consolidacaoTurma.TurmaId));

            if (turma.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível encontrar a turma de id {consolidacaoTurma.TurmaId}.", LogNivel.Negocio, LogContexto.Fechamento));                
                return false;
            }

            var componentes = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmaECodigoUeQuery(new string[] { turma.CodigoTurma }, turma.Ue.CodigoUe));

            if (componentes.EhNulo() || !componentes.Any())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível encontrar os componentes curricularres da turma de id {consolidacaoTurma.TurmaId}.", LogNivel.Negocio, LogContexto.Fechamento));                
                return false;
            }

            var dadosTurmaEOL = await mediator.Send(new ObterDadosTurmaEolPorCodigoQuery(turma.CodigoTurma));

            if (dadosTurmaEOL.NaoEhNulo())
            {
                if (!dadosTurmaEOL.Extinta)
                {
                    foreach (var componente in componentes)
                    {
                        var mensagem = JsonConvert.SerializeObject(new FechamentoConsolidacaoTurmaComponenteBimestreDto(consolidacaoTurma.TurmaId, consolidacaoTurma.Bimestre, Convert.ToInt64(componente.Codigo)));
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoComponenteTratar, mensagem, mensagemRabbit.CodigoCorrelacao, null));
                    }
                }
                else
                {
                    int bimestre = consolidacaoTurma.Bimestre.HasValue ? consolidacaoTurma.Bimestre.Value : 0;
                    var consolidacoesTurmaBimestre = await mediator.Send(new ObterFechamentoConsolidadoPorTurmaBimestreQuery(consolidacaoTurma.TurmaId, bimestre, new int[] { -99 }));

                    if (consolidacoesTurmaBimestre.Any())
                        await mediator.Send(new ExcluirConsolidacaoFechamentoPorTurmaIdEBimestreCommand(consolidacaoTurma.TurmaId, bimestre));
                }

                return true;
            }
            else
                return false;

        }
    }
}
