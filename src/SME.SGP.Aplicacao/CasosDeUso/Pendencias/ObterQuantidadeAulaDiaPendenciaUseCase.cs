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
                var anoletivo = JsonConvert.DeserializeObject<int?>(param.Mensagem.ToString());
                var pendencias = await mediator.Send(new ObterPendenciasParaInserirAulasEDiasQuery(anoletivo));
                foreach (var pendencia in pendencias)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaCargaAdicionarQuantidadeAulaDiaPendencia, new CargaAulasDiasPendenciaDto(pendencia.PendenciaId, pendencia.QuantidadeDias,pendencia.QuantidadeAulas), Guid.NewGuid()));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao realizar a carga de dias e aulas  na pendencia", LogNivel.Negocio, LogContexto.Pendencia, ex.Message));
                throw;
            }
        }
    }
}