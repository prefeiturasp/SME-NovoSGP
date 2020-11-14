using MediatR;
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
            var parametrosSistema = await mediator.Send(new ObterParametrosSistemaPorTipoEAnoQuery(request.TipoParametro, anoAtual));

            if (parametrosSistema != null)
            {
                IList<long> funcionariosId = new List<long>();
                foreach (var parametro in parametrosSistema)
                {
                    IList<long> funcionariosIdTemp = new List<long>();

                    switch (parametro.Valor)
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
                            var administradoresId = await mediator.Send(new ObterAdministradoresPorUEQuery(request.CodigoUe));

                            foreach (var adm in administradoresId)
                            {
                                funcionariosIdTemp.Add(long.Parse(adm));
                            }
                            break;
                        default:
                            break;
                    }

                    funcionariosId = funcionariosId.Concat(funcionariosIdTemp).ToList();
                }

                if (funcionariosId.Count > 0)
                    foreach (var id in funcionariosId)
                    {
                        await mediator.Send(new SalvarPendenciaUsuarioCommand(request.PendenciaId, id));
                    }
            }
            return true;
        }
    }
}
