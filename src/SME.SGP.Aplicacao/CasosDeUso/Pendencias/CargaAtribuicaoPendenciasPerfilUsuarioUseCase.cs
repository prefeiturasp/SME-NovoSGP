using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CargaAtribuicaoPendenciasPerfilUsuarioUseCase : AbstractUseCase, ICargaAtribuicaoPendenciasPerfilUsuarioUseCase
    {
        public CargaAtribuicaoPendenciasPerfilUsuarioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var cargaPendencias = await mediator.Send(new ObterPendenciaCargaQuery());

                foreach (var pendencia in cargaPendencias)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendencia.PendenciaId, pendencia.UeId), Guid.NewGuid()));
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao realizar a carga de atribuição de pendencia usuário", LogNivel.Negocio, LogContexto.Avaliacao, ex.Message));
            }
            return true;
        }
    }
}
