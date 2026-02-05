using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.AbrangenciaSync;
using SME.SGP.Aplicacao.Queries.ObterUsuariosPerfis;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Abrangencia;
using System;
using System.Linq;
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

            var usuariosAgrupados = usuarios
                    .GroupBy(x => x.Login)
                    .Select(g => new AbrangenciaAgrupadaUsuarioPerfisDto
                    {
                        Login = g.Key,
                        Perfil = g.Select(p => p.Perfil).ToList()
                    })
                    .ToList();

            foreach (var usuario in usuariosAgrupados)
            {
                await mediator.Send(new PublicarFilaSgpCommand("sgp.abrangencia.worker.tratar", usuario, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
