using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualUseCase : AbstractUseCase, IGerarPendenciaAusenciaRegistroIndividualUseCase
    {
        public GerarPendenciaAusenciaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var turmasDoEnsinoInfantil = await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(DateTime.Now.Year, Modalidade.Infantil));
                if (!turmasDoEnsinoInfantil?.Any() ?? true)
                {
                    // TO DO log
                    return false;
                }

                foreach (var turma in turmasDoEnsinoInfantil)
                    await GerarPendenciaAusenciaRegistroIndividualTurmaAsync(turma);

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private async Task GerarPendenciaAusenciaRegistroIndividualTurmaAsync(Turma turma)
        {
            if (turma is null) return;

            try
            {
                var retorno = await mediator.Send(new GerarPendenciaAusenciaRegistroIndividualTurmaCommand(turma.Id));
                if (retorno is null)
                {
                    // TO DO LOG
                    return;
                }

                if (retorno.ExistemErros)
                {
                    // TO DO LOG
                    return;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}