using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
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
            var totalItensPorPagina = 10;
            var dadosCriacao = mensagemRabbit.ObterObjetoMensagem<DadosCriacaoAulasAutomaticasDto>();

            if (dadosCriacao.TipoCalendarioId > 0)
            {
                if (dadosCriacao.DadosTurmas != null && dadosCriacao.DadosTurmas.Any())
                {
                    var totalPaginas = Math.Ceiling((decimal)dadosCriacao.DadosTurmas.Count() / totalItensPorPagina);
                    for (int pagina = 1; pagina <= totalPaginas; pagina++)
                    {
                        var dadosTurmas = dadosCriacao.DadosTurmas.Skip(pagina == 1 ? 0 : (pagina - 1) * totalItensPorPagina).Take(totalItensPorPagina);

                        foreach(var dadoTurma in dadosTurmas)
                        {                            
                            var comando = new CriarAulasRegenciaAutomaticamenteCommand(dadosCriacao.Modalidade, dadosCriacao.TipoCalendarioId, dadosCriacao.UeCodigo, dadosCriacao.DiasLetivosENaoLetivos, 
                                dadosTurmas.Where(x => x.TurmaCodigo == dadoTurma.TurmaCodigo), dadosCriacao.DiasForaDoPeriodoEscolar);
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.SincronizarAulasRegenciaAutomaticamente, comando, Guid.NewGuid(), null));
                        }
                    }
                }
                return true;
            }
            else
                await mediator.Send(new SalvarLogViaRabbitCommand($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Rotina de manutenção de aulas do Infantil não iniciada pois não há Calendário Escolar cadastrado.", LogNivel.Critico, LogContexto.Aula));

            return false;
        }
    }
}