using System;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulaDiaPendenciaUseCase : AbstractUseCase,IObterQuantidadeAulaDiaPendenciaUseCase
    {
        public ObterQuantidadeAulaDiaPendenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                int? anoletivo = null;
                if(!string.IsNullOrEmpty(param.Mensagem?.ToString()))
                        anoletivo = int.Parse(param.Mensagem.ToString()!);

                var listaUes = await mediator.Send(ObterTodasUesIdsQuery.Instance);
                foreach (var ue in listaUes)
                {
                    var dto = new ObterQuantidadeAulaDiaPendenciaDto
                    {
                        UeId = ue,
                        AnoLetivo = anoletivo
                    };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaBuscarAdicionarQuantidadeAulaDiaPendenciaUe, dto, Guid.NewGuid()));
                }

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