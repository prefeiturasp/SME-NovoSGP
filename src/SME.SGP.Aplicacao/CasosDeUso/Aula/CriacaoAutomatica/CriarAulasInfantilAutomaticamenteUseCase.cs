using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteUseCase : ICriarAulasInfantilAutomaticamenteUseCase
    {
        private const string CODIGO_COMPONENTE_REGENCIA_INFANTIL = "512";
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator, IRepositorioCache repositorioCache)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var executarManutencao = await mediator
                .Send(ObterExecutarManutencaoAulasInfantilQuery.Instance);

            if (!executarManutencao)
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois seu parâmetro está marcado como não executar", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

             var dadosCriacaoAulaInfantil = mensagemRabbit.NaoEhNulo() && mensagemRabbit.Mensagem.NaoEhNulo() ?
                mensagemRabbit.ObterObjetoMensagem<DadosCriacaoAulasAutomaticasCarregamentoDto>() : new DadosCriacaoAulasAutomaticasCarregamentoDto();

            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;

            var tipoCalendarioId = await mediator
                .Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EducacaoInfantil, anoAtual, null));

            if (tipoCalendarioId < 1)
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var periodosEscolares = await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Período Escolar cadastrado.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var diasForaDoPeriodoEscolar = await mediator
                .Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));

            var turmas = await mediator
                .Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual, dadosCriacaoAulaInfantil?.CodigoTurma, dadosCriacaoAulaInfantil?.Pagina ?? 1));

            if (turmas.EhNulo() || !turmas.Any())
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não foram encontradas turmas.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            foreach (var turma in turmas)
            {
                var diasLetivosENaoLetivos = await mediator
                .Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId, turma.Ue.CodigoUe));

                var chaveCache = string.Format(NomeChaveCache.DADOS_CRIACAO_AULA_AUTOMATICA_INFANTIL_REGENCIA_TURMA, turma.CodigoTurma);
                var dadosAulaCriadaAutomaticamente = new DadosAulaCriadaAutomaticamenteDto((CODIGO_COMPONENTE_REGENCIA_INFANTIL, "Regência de classe infantil"));                
                var comando = new CriarAulasInfantilERegenciaAutomaticamenteCommand(diasLetivosENaoLetivos.ToList(), turma, tipoCalendarioId, diasForaDoPeriodoEscolar, new string[] { CODIGO_COMPONENTE_REGENCIA_INFANTIL }, dadosAulaCriadaAutomaticamente);

                await repositorioCache.SalvarAsync(chaveCache, comando, 300);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaCriarAulasInfantilERegenciaAutomaticamente, chaveCache, Guid.NewGuid(), null));
            }

            if (dadosCriacaoAulaInfantil != null && string.IsNullOrEmpty(dadosCriacaoAulaInfantil?.CodigoTurma))
            {
                dadosCriacaoAulaInfantil.Pagina += 1;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaSincronizarAulasInfantil, dadosCriacaoAulaInfantil, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}