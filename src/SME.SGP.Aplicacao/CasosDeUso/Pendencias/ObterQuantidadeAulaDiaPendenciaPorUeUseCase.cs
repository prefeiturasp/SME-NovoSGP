using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulaDiaPendenciaPorUeUseCase : AbstractUseCase,IObterQuantidadeAulaDiaPendenciaPorUeUseCase
    {
        public ObterQuantidadeAulaDiaPendenciaPorUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<ObterQuantidadeAulaDiaPendenciaDto>();
                var pendencias = await mediator.Send(new ObterPendenciasParaInserirAulasEDiasQuery(filtro.AnoLetivo,filtro.UeId));
                foreach (var pendencia in pendencias)
                {
                    var dto = new AulasDiasPendenciaDto
                    {
                        PendenciaId = pendencia.PendenciaId,
                        QuantidadeDias = pendencia.QuantidadeDias,
                        QuantidadeAulas = pendencia.QuantidadeAulas
                    };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaCargaAdicionarQuantidadeAulaDiaPendencia, dto, Guid.NewGuid()));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao realizar a carga de dias e aulas  na pendencia", LogNivel.Negocio, LogContexto.Pendencia, ex.Message,innerException:ex.InnerException?.ToString()));
                return false;
            }
        }
    }
}