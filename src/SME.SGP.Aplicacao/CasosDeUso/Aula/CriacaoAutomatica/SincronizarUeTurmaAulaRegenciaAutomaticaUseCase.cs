using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SincronizarUeTurmaAulaRegenciaAutomaticaUseCase : AbstractUseCase, ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase
    {
        public SincronizarUeTurmaAulaRegenciaAutomaticaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dadosCriacao = mensagemRabbit.ObterObjetoMensagem<DadosCriacaoAulasAutomaticasDto>();

            if (dadosCriacao.TipoCalendarioId > 0)
            {
                foreach (var dadosTurma in dadosCriacao?.DadosTurmas)
                {
                    var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(dadosTurma.TurmaCodigo, dadosTurma.ComponenteCurricularCodigo));
                    var dadosAulaCriada = new DadosAulaCriadaAutomaticamenteDto((dadosTurma.ComponenteCurricularCodigo, dadosTurma.ComponenteCurricularDescricao), dadosCriacao.Modalidade == Modalidade.EJA ? 5 : 1, professorTitular?.ProfessorRf);
                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(dadosTurma.TurmaCodigo));
                    var comando = new CriarAulasInfantilERegenciaAutomaticamenteCommand(dadosCriacao.DiasLetivosENaoLetivos, turma, dadosCriacao.TipoCalendarioId, dadosCriacao.DiasForaDoPeriodoEscolar, new string[] { dadosTurma.ComponenteCurricularCodigo }, dadosAulaCriada);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaCriarAulasInfatilERegenciaAutomaticamente, comando, Guid.NewGuid(), null));
                }
                return true;
            }
            else
                await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", LogNivel.Critico, LogContexto.Aula));

            return false;
        }
    }
}