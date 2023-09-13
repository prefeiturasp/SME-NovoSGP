using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using System.Threading;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaPlanoAulaUseCase : AbstractUseCase, IPendenciaAulaPlanoAulaUseCase
    {
        public PendenciaAulaPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<DreUeDto>();

                var aulas = (await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula,
                    "plano_aula",
                    new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                    filtro.DreId, filtro.UeId)));

                aulas = await RemoverAulasComFechamentoTurmaDisciplinaProcessado(aulas);
                if (aulas.NaoEhNulo() && aulas.Any())
                    await RegistraPendencia(aulas, TipoPendencia.PlanoAula);
             
                await VerificaPendenciaAulaGeradaParaInfantil(filtro.DreId, filtro.UeId);

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Registrar Pendencia Aula do Plano Aula.",  LogNivel.Critico, LogContexto.Aula, ex.Message,innerException: ex.InnerException.ToString(),rastreamento:ex.StackTrace));
                throw;
            }
        }

        private async Task<IEnumerable<Aula>> RemoverAulasComFechamentoTurmaDisciplinaProcessado(IEnumerable<Aula> aulas)
        {
            return aulas.NaoEhNulo() ? await mediator.Send(new ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery(aulas)) : null;
        }

        private async Task VerificaPendenciaAulaGeradaParaInfantil(long dreId, long ueId)
        {
            var pendenciasId = await mediator.Send(new ObterPendenciaPlanoAulaPorDreIdUeIdModalidadeQuery(dreId, ueId, TipoPendencia.PlanoAula, Modalidade.EducacaoInfantil));

            if (pendenciasId.Any())
            {
                await mediator.Send(new ExcluirPendenciasPorIdsCommand() { PendenciasIds = pendenciasId.ToArray()});
            }
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            await mediator.Send(new SalvarPendenciaAulasPorTipoCommand(aulas, tipoPendenciaAula));
        }
    }
}
