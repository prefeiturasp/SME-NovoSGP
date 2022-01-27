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
            Modalidade[] modalidades;
            Turma turma = null;

            if (mensagemRabbit?.Mensagem != null)
            {
                turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(mensagemRabbit.Mensagem.ToString()));

                if (turma == null || (turma.ModalidadeCodigo != Modalidade.Fundamental && turma.ModalidadeCodigo != Modalidade.EJA))
                    return false;
                else
                    modalidades = new Modalidade[] { turma.ModalidadeCodigo };
            }
            else
                modalidades = new Modalidade[] { Modalidade.Fundamental, Modalidade.EJA };

            foreach (var modalidade in modalidades)
            {
                if (modalidade == Modalidade.EJA)
                {
                    await ObterDados(modalidade, 1, turma);
                    await ObterDados(modalidade, 2, turma);
                }
                else await ObterDados(modalidade, turma: turma);
            }
            return true;
        }

        private async Task<bool> ObterDados(Modalidade modalidade, int? semestre = null, Turma turma = null)
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(modalidade, anoAtual, semestre));
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            var componentesCurricularesFundamental = new string[] { "508", "1105", "1112", "1115", "1117", "1121", "1124", "1211", "1212", "1213", "1290", "1301" };
            var componentesCurricularesEja = new string[] { "1113", "1114", "1125" };
            if (periodosEscolares != null && periodosEscolares.Any())
            {
                var diasForaDoPeriodoEscolar = (List<DateTime>)await mediator.Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));
                var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId, true));
                var uesCodigos = turma != null ? new string[] { turma.Ue.CodigoUe } : await mediator.Send(new ObterUesCodigosPorModalidadeEAnoLetivoQuery(modalidade, anoAtual));
                foreach (var ueCodigo in uesCodigos)
                {
                    var componentesCurriculares = modalidade == Modalidade.Fundamental ? componentesCurricularesFundamental : componentesCurricularesEja;

                    var dadosTurmaComponente = new List<DadosTurmaAulasAutomaticaDto>();
                    if (turma != null)
                    {
                        var componentesTurmaEol = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(new string[] { turma.CodigoTurma }, false));
                        var componentesConsiderados = componentesTurmaEol.Where(ct => ct.Regencia && componentesCurriculares.Contains(ct.Codigo));
                        var dadosTurmaEol = await mediator.Send(new ObterDadosTurmaEolQuery(turma.CodigoTurma));
                        componentesConsiderados.ToList().ForEach(cc => dadosTurmaComponente.Add(new DadosTurmaAulasAutomaticaDto()
                        {
                            ComponenteCurricularCodigo = cc.Codigo,
                            ComponenteCurricularDescricao = cc.Descricao,
                            TurmaCodigo = turma.CodigoTurma,
                            DataInicioTurma = dadosTurmaEol.DataInicioTurma
                        }));

                    }
                    else
                        dadosTurmaComponente.AddRange(await mediator.Send(new ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery(anoAtual, ueCodigo, componentesCurriculares)));

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
