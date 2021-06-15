using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarUesTurmasRegenciaAulaAutomaticaUseCase : AbstractUseCase, ICarregarUesTurmasRegenciaAulaAutomaticaUseCase
    {
        public CarregarUesTurmasRegenciaAulaAutomaticaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            foreach (var modalidade in new Modalidade[] { Modalidade.Fundamental, Modalidade.EJA })
            {
                if (modalidade == Modalidade.EJA)
                {
                    await ObterDados(modalidade, 1);
                    await ObterDados(modalidade, 2);
                }
                else await ObterDados(modalidade);
            }
            return true;
        }

        private async Task<bool> ObterDados(Modalidade modalidade, int? semestre = null)
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(modalidade, anoAtual, semestre));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            if (periodosEscolares != null && periodosEscolares.Any())
            {
                var diasForaDoPeriodoEscolar = (List<DateTime>)await mediator.Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));
                var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));
                var uesCodigos = await mediator.Send(new ObterUesCodigosPorModalidadeEAnoLetivoQuery(modalidade, anoAtual));
                foreach (var ueCodigo in uesCodigos)
                {
                    var dadosTurmaComponente = await mediator.Send(new ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery(anoAtual, ueCodigo));
                    if (dadosTurmaComponente.Any())
                    {
                        var dados = new DadosCriacaoAulasAutomaticasDto(ueCodigo, tipoCalendarioId, diasLetivosENaoLetivos, diasForaDoPeriodoEscolar, modalidade, dadosTurmaComponente);
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizarDadosUeTurmaRegenciaAutomaticamente, dados, Guid.NewGuid(), null));
                    }
                }
            }
            return true;
        }
    }
}
