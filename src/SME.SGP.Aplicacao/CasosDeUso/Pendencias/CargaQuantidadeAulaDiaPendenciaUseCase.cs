using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CargaQuantidadeAulaDiaPendenciaUseCase : AbstractUseCase,ICargaQuantidadeAulaDiaPendenciaUseCase
    {
        public CargaQuantidadeAulaDiaPendenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var pendencia = param.ObterObjetoMensagem<AulasDiasPendenciaDto>();
                await mediator.Send(new CargaPendenciasQuantidadeDiasQuantidadeAulasCommand(pendencia));
                return true;
                
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao realizar a carga de dias e aulas  na pendencia", LogNivel.Negocio, LogContexto.Pendencia, ex.Message));
                return false;
            }
        }
    }
}