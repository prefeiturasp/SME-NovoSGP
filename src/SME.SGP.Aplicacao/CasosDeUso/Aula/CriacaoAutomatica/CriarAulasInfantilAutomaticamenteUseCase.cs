using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteUseCase : ICriarAulasInfantilAutomaticamenteUseCase
    {
        private readonly IMediator mediator;

        public CriarAulasInfantilAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var executarManutencao = await mediator.Send(new ObterExecutarManutencaoAulasInfantilQuery());
            if (!executarManutencao)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois seu parâmetro está marcado como não executar", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil parâmetro ativo", LogNivel.Informacao, LogContexto.Infantil));

            var dadosCriacaoAulaInfantil = mensagemRabbit?
                .ObterObjetoMensagem<DadosCriacaoAulasAutomaticasCarregamentoDto>() ?? new DadosCriacaoAulasAutomaticasCarregamentoDto();
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator
                .Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EducacaoInfantil, anoAtual, null));
            if (tipoCalendarioId < 1)
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var periodosEscolares = await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            if (periodosEscolares == null || !periodosEscolares.Any())
            {
                await mediator
                    .Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Período Escolar cadastrado.", LogNivel.Negocio, LogContexto.Infantil));
                return false;
            }

            var diasLetivosENaoLetivos = await mediator
                .Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));

            var diasForaDoPeriodoEscolar = await mediator
                .Send(new ObterDiasForaDoPeriodoEscolarQuery(periodosEscolares));

            var turmas = await mediator
                .Send(new ObterTurmasInfantilNaoDeProgramaQuery(anoAtual, dadosCriacaoAulaInfantil?.CodigoTurma, dadosCriacaoAulaInfantil?.Pagina ?? 1));

            if (turmas == null || !turmas.Any())
                return true;

            foreach (var turma in turmas)
            {
                var comando = new CriarAulasInfantilAutomaticamenteCommand(diasLetivosENaoLetivos.ToList(), turma, tipoCalendarioId, diasForaDoPeriodoEscolar);
                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCriarAulasInfatilAutomaticamente, comando, Guid.NewGuid(), null));
            }

            if (dadosCriacaoAulaInfantil != null && string.IsNullOrEmpty(dadosCriacaoAulaInfantil.CodigoTurma))
            {
                dadosCriacaoAulaInfantil.Pagina += 1;
                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaSincronizarAulasInfatil, dadosCriacaoAulaInfantil, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}