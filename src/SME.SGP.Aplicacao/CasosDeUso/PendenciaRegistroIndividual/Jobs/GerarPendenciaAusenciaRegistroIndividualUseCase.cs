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
                    SentrySdk.AddBreadcrumb($"Não foram encontradas turmas para geração de pendências de ausência de registro individual .", $"Rabbit - {nameof(GerarPendenciaAusenciaRegistroIndividualUseCase)}");
                    return false;
                }

                //TODO: Remover antes de ir para dev
                //foreach (var turma in turmasDoEnsinoInfantil.Where(a => a.CodigoTurma == "2252164").ToList()) PARA TESTAR
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
                var retorno = await mediator.Send(new GerarPendenciaAusenciaRegistroIndividualTurmaCommand(turma));
                if (retorno is null)
                {
                    SentrySdk.AddBreadcrumb($"Não foi possível gerar a pendência de ausência de registro individual para a turma {turma.Id}.", $"Rabbit - {nameof(GerarPendenciaAusenciaRegistroIndividualUseCase)}");
                    return;
                }

                if (retorno.ExistemErros)
                {
                    var erros = string.Join(", ", retorno.Mensagens);
                    SentrySdk.AddBreadcrumb($"Não foi possível gerar a pendência de ausência de registro individual para a turma {turma.Id}. {erros}", $"Rabbit - {nameof(GerarPendenciaAusenciaRegistroIndividualUseCase)}");
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