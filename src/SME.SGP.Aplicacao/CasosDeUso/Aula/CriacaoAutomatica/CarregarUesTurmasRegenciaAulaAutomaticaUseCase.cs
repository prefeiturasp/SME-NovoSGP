using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CarregarUesTurmasRegenciaAulaAutomaticaUseCase : AbstractUseCase, ICarregarUesTurmasRegenciaAulaAutomaticaUseCase
    {
        private readonly IRepositorioCache repositorioCache;

        public CarregarUesTurmasRegenciaAulaAutomaticaUseCase(IMediator mediator, IRepositorioCache repositorioCache)
            : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            if (await ParametroExecutacaoManutencaoAulasInfantilDesligado())
                return false;

            var executarProximaPagina = false;
            var parametro = ObterDtoParametro(mensagemRabbit);
            Turma turma = await ObterTurmaParametrizada(parametro.CodigoTurma);
            Modalidade[] modalidades = ObterModalidadesParametrizadas(parametro, turma);

            foreach (var modalidade in modalidades)
            {
                switch (modalidade)
                {
                    case Modalidade.EJA:
                        await ObterDadosEJA(turma, 1, parametro.Pagina);
                        await ObterDadosEJA(turma, 2, parametro.Pagina);
                        break;
                    default: 
                        executarProximaPagina = await ObterDados(modalidade, turma: turma, pagina: parametro.Pagina);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(parametro.CodigoTurma) && executarProximaPagina)
            {
                parametro.Pagina += 1;
                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.CarregarDadosUeTurmaRegenciaAutomaticamente, parametro, Guid.NewGuid(), null));
            }

            return true;
        }

        private async Task ObterDadosEJA(Turma turma, int semestre, int pagina)
        {
            if ((turma?.Semestre ?? semestre) == semestre)
                await ObterDados(Modalidade.EJA, semestre, turma, pagina);
        }

        private async Task<Turma> ObterTurmaParametrizada(string codigoTurma)
        {
            if (string.IsNullOrEmpty(codigoTurma)) return null;
            return await mediator
                    .Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
        }
        private async Task<bool> ParametroExecutacaoManutencaoAulasInfantilDesligado()
        {
            var executarManutencao = await mediator.Send(ObterExecutarManutencaoAulasInfantilQuery.Instance);
            if (executarManutencao)
                return false;
            
            await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas de regência não iniciada pois seu parâmetro está marcado como não executar", LogNivel.Negocio, LogContexto.Infantil));
            return true;
        }

        private Modalidade[] ObterModalidadesParametrizadas(DadosCriacaoAulasAutomaticasCarregamentoDto parametro, Turma turma)
        {
            if (!string.IsNullOrEmpty(parametro.CodigoTurma))
            {
                var modalidadeTurma = turma?.ModalidadeCodigo ?? 0;
                if (turma.EhNulo() || (modalidadeTurma != Modalidade.Fundamental 
                    && modalidadeTurma != Modalidade.EJA))
                    return new Modalidade[] {};
                else
                    return new Modalidade[] { turma.ModalidadeCodigo };
            }
            else
                return new Modalidade[] { Modalidade.Fundamental, Modalidade.EJA };
        }

        private DadosCriacaoAulasAutomaticasCarregamentoDto ObterDtoParametro(MensagemRabbit mensagemRabbit)
        {
            if (mensagemRabbit.NaoEhNulo() && mensagemRabbit.Mensagem.NaoEhNulo())
                return mensagemRabbit.ObterObjetoMensagem<DadosCriacaoAulasAutomaticasCarregamentoDto>();
            return new DadosCriacaoAulasAutomaticasCarregamentoDto();
        }

        private async Task<bool> ObterDados(Modalidade modalidade, int? semestre = null, Turma turma = null, int pagina = 1)
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var tipoCalendarioId = await mediator
                .Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(modalidade, anoAtual, semestre));

            if (tipoCalendarioId < 1)
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil/Regência não iniciada pois não há Tipo Calendário cadastrado para {string.Concat(modalidade.ShortName(), semestre > 0 ? $" {semestre}º semestre" : string.Empty)}.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var periodosEscolares = await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));            

            if (periodosEscolares.EhNulo() && !periodosEscolares.Any())
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil/Regência não iniciada pois não há Período Escolar cadastrado.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var diasForaDoPeriodoEscolar = (List<DateTime>)await mediator
                .Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));

            var diasLetivosENaoLetivos = await mediator
                .Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));

            var uesCodigos = turma.NaoEhNulo() ? new string[] { turma.Ue.CodigoUe } : await mediator
                .Send(new ObterUesCodigosPorModalidadeEAnoLetivoQuery(modalidade, anoAtual, pagina));

            if (uesCodigos.EhNulo() || !uesCodigos.Any())
                return false;

            foreach (var ueCodigo in uesCodigos)
                await PublicarMensagens(modalidade, turma, anoAtual, tipoCalendarioId, diasForaDoPeriodoEscolar, diasLetivosENaoLetivos, ueCodigo, semestre);

            return true;
        }

        private async Task PublicarMensagens(Modalidade modalidade, Turma turma, int anoAtual, long tipoCalendarioId, IList<DateTime> diasForaDoPeriodoEscolar, IList<DiaLetivoDto> diasLetivosENaoLetivos, string ueCodigo, int? semestre)
        {
            var componentesCurriculares = await mediator.Send(new ObterCodigosComponentesCurricularesRegenciaAulasAutomaticasQuery(modalidade));
            var dadosTurmaComponente = new List<DadosTurmaAulasAutomaticaDto>();
            if (turma.NaoEhNulo())
                await DefinirParaTurma(turma, componentesCurriculares, dadosTurmaComponente);
            else
            {
                dadosTurmaComponente
                    .AddRange(await mediator
                    .Send(new ObterDadosComponenteCurricularTurmaPorUeEAnoLetivoQuery(anoAtual, ueCodigo, componentesCurriculares, semestre)));
            }

            if (dadosTurmaComponente.Any())
            {
                var chaveCache = string.Format(NomeChaveCache.DADOS_CRIACAO_AULAS_AUTOMATICAS_REGENCIA_UE_TIPO_CALENDARIO_MODALIDADE, ueCodigo, tipoCalendarioId, modalidade);
                var dados = new DadosCriacaoAulasAutomaticasDto(ueCodigo, tipoCalendarioId, diasLetivosENaoLetivos, diasForaDoPeriodoEscolar, modalidade, dadosTurmaComponente);

                await repositorioCache.SalvarAsync(chaveCache, dados, 300);

                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.SincronizarDadosUeTurmaRegenciaAutomaticamente, chaveCache, Guid.NewGuid(), null));
            }
        }

        public async Task DefinirParaTurma(Turma turma, string[] componentesCurriculares, IList<DadosTurmaAulasAutomaticaDto> dadosTurmaComponente)
        {
            var componentesTurmaEol = await mediator
                .Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(new string[] { turma.CodigoTurma }, false));

            var componentesConsiderados = componentesTurmaEol
                .Where(ct => ct.Regencia && componentesCurriculares.Contains(ct.Codigo.ToString()));

            var dadosTurmaEol = await mediator
                .Send(new ObterDadosTurmaEolPorCodigoQuery(turma.CodigoTurma));

            componentesConsiderados.ToList().ForEach(cc => dadosTurmaComponente.Add(new DadosTurmaAulasAutomaticaDto()
            {
                ComponenteCurricularCodigo = cc.Codigo.ToString(),
                ComponenteCurricularDescricao = cc.Descricao,
                TurmaCodigo = turma.CodigoTurma,
                DataInicioTurma = dadosTurmaEol.DataInicioTurma
            }));
        }
    }
}
