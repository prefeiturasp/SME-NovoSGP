using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarUeTurmaAulaRegenciaAutomaticaUseCase : AbstractUseCase, ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase
    {
        private readonly IRepositorioCache repositorioCache;

        public SincronizarUeTurmaAulaRegenciaAutomaticaUseCase(IMediator mediator, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var jsonDadosCriacao = await repositorioCache.ObterAsync(mensagemRabbit.Mensagem?.ToString());

            if (string.IsNullOrWhiteSpace(jsonDadosCriacao))
                return false;

            await repositorioCache.RemoverAsync(mensagemRabbit.Mensagem.ToString());

            var dadosCriacao = JsonConvert.DeserializeObject<DadosCriacaoAulasAutomaticasDto>(jsonDadosCriacao);

            if (dadosCriacao.TipoCalendarioId > 0)
            {
                foreach (var dadosTurma in dadosCriacao?.DadosTurmas)
                {
                    var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(dadosTurma.TurmaCodigo, dadosTurma.ComponenteCurricularCodigo));
                    var dadosAulaCriada = new DadosAulaCriadaAutomaticamenteDto((dadosTurma.ComponenteCurricularCodigo, dadosTurma.ComponenteCurricularDescricao), dadosCriacao.Modalidade == Modalidade.EJA ? 5 : 1, professorTitular?.ProfessorRf);
                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(dadosTurma.TurmaCodigo));

                    if (turma == null)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil, a turma de código {dadosTurma.TurmaCodigo} não foi localizada na base do SGP.", LogNivel.Critico, LogContexto.Aula));
                        continue;
                    }

                    var chaveCache = string.Format(NomeChaveCache.DADOS_CRIACAO_AULA_AUTOMATICA_INFANTIL_REGENCIA_TURMA, turma.CodigoTurma);
                    var comando = new CriarAulasInfantilERegenciaAutomaticamenteCommand(dadosCriacao.DiasLetivosENaoLetivos, turma, dadosCriacao.TipoCalendarioId, dadosCriacao.DiasForaDoPeriodoEscolar, new string[] { dadosTurma.ComponenteCurricularCodigo }, dadosAulaCriada);

                    await repositorioCache.SalvarAsync(chaveCache, comando, 300);

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaCriarAulasInfantilERegenciaAutomaticamente, chaveCache, Guid.NewGuid(), null));
                }
                return true;
            }
            else
                await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTimeExtension.HorarioBrasilia():dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", LogNivel.Critico, LogContexto.Aula));

            return false;
        }
    }
}