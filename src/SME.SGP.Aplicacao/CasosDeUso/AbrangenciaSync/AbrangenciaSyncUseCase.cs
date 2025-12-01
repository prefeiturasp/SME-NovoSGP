using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AbrangenciaSync;
using SME.SGP.Aplicacao.Queries.ObterUsuariosPerfis;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.AbrangenciaSync
{
    public class AbrangenciaSyncUseCase : AbstractUseCase, IAbrangenciaSyncUseCase
    {
        public AbrangenciaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var usuarios = await mediator.Send(new ObterUsuariosPerfisQuery());
            foreach (var usuario in usuarios)
            {
                await mediator.Send(new PublicarFilaSgpCommand("sgp.abrangencia.worker.tratar", usuario, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
