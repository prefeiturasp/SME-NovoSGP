using MediatR;
using Sentry;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelacionaPendenciaUsuarioCommandHandler : IRequestHandler<RelacionaPendenciaUsuarioCommand, bool>
    {
        private readonly IMediator mediator;

        public RelacionaPendenciaUsuarioCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(RelacionaPendenciaUsuarioCommand request, CancellationToken cancellationToken)
        {
            var anoAtual = DateTime.Today.Year;
            IList<long> funcionariosId = new List<long>();
            foreach (var perfilUsuario in request.PerfisUsuarios)
            {
                try
                {
                    List<long> funcionariosIdTemp = new List<long>();

                    switch (perfilUsuario)
                    {
                        case "Professor":
                            funcionariosIdTemp.Add(request.ProfessorId.Value);
                            break;
                        case "CP":
                            var funcionariosIdCP = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.CP));
                            funcionariosIdTemp = funcionariosIdCP.ToList();
                            break;
                        case "AD":
                            var funcionarioAD = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.AD));
                            funcionariosIdTemp = funcionarioAD.ToList();
                            break;
                        case "Diretor":
                            var funcionarioDiretor = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.Diretor));
                            funcionariosIdTemp = funcionarioDiretor.ToList();
                            break;
                        case "ADM UE":
                            funcionariosIdTemp.AddRange(await ObterAdministradoresPorUE(request.CodigoUe));
                            break;
                        default:
                            break;
                    }

                    funcionariosId = funcionariosId.Concat(funcionariosIdTemp).ToList();

                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }            
            }

            if (funcionariosId.Any())
                foreach (var id in funcionariosId)
                {
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(request.PendenciaId, id));
                }

            return true;
        }

        private async Task<List<long>> ObterAdministradoresPorUE(string CodigoUe)
        {
            var administradoresId = await mediator.Send(new ObterAdministradoresPorUEQuery(CodigoUe));
            var AdministradoresUeId = new List<long>();

            foreach (var adm in administradoresId)
            {
                AdministradoresUeId.Add(await ObterUsuarioId(adm));
            }
            return AdministradoresUeId;
        }


        private async Task<long> ObterUsuarioId(string rf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rf));
            return usuarioId;
        }
    }
}
