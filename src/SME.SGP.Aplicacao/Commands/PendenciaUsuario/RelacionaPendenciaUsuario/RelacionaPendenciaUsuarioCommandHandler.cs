using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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
            var listaFuncionarioPerfilAtribuido = new Dictionary<long, int>();

            foreach (var perfilUsuario in request.PerfisUsuarios)
            {
                try
                {
                    List<long> funcionariosIdTemp = new List<long>();

                    switch (perfilUsuario)
                    {
                        case "Professor":
                            funcionariosIdTemp.Add(request.ProfessorId.Value);
                            AtribuirFuncionarioPerfil(funcionariosIdTemp, (int)PerfilUsuario.PROFESSOR, listaFuncionarioPerfilAtribuido);
                            break;
                        case "CP":
                            var funcionariosIdCP = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.CP));
                            AtribuirFuncionarioPerfil(funcionariosIdCP, (int) PerfilUsuario.CP, listaFuncionarioPerfilAtribuido);
                            break;
                        case "AD":
                            var funcionarioAD = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.AD));
                            AtribuirFuncionarioPerfil(funcionarioAD, (int)PerfilUsuario.AD, listaFuncionarioPerfilAtribuido);
                            break;
                        case "Diretor":
                            var funcionarioDiretor = await mediator.Send(new ObterFuncionariosIdPorCodigoUeECargoQuery(request.CodigoUe, Cargo.Diretor));
                            AtribuirFuncionarioPerfil(funcionarioDiretor, (int)PerfilUsuario.DIRETOR, listaFuncionarioPerfilAtribuido);
                            break;
                        case "ADM UE":
                            var funcionarioADMUE = await ObterAdministradoresPorUE(request.CodigoUe);
                            AtribuirFuncionarioPerfil(funcionarioADMUE, (int)PerfilUsuario.ADMDRE, listaFuncionarioPerfilAtribuido);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do usuário.", LogNivel.Negocio, LogContexto.Usuario, ex.Message));
                }
            }

            if (listaFuncionarioPerfilAtribuido.Any())
            {
                int nivel = 0;
                foreach (var valores in listaFuncionarioPerfilAtribuido)
                {                
                    if(valores.Value > 0)
                    {
                        switch (valores.Value)
                        {
                            case (int)PerfilUsuario.ADMUE:
                                nivel = 1;
                                break;

                            case (int)PerfilUsuario.DIRETOR:
                                if (!listaFuncionarioPerfilAtribuido.Any(l => l.Value.Equals((int) PerfilUsuario.ADMUE)))
                                    nivel = 1;             
                                else
                                    nivel = 2;
                                break;

                            case (int)PerfilUsuario.AD:
                                if (!listaFuncionarioPerfilAtribuido.Any(l => l.Value.Equals((int)PerfilUsuario.DIRETOR)))
                                    nivel = 2;
                                else
                                    nivel = 3;
                                break;

                            case (int)PerfilUsuario.CP:
                                if (!listaFuncionarioPerfilAtribuido.Any(l => l.Value.Equals((int)PerfilUsuario.AD)))
                                    nivel = 3;
                                else
                                    nivel = 4;
                                break;

                            case (int)PerfilUsuario.PROFESSOR:
                                if (!listaFuncionarioPerfilAtribuido.Any(l => l.Value.Equals((int)PerfilUsuario.CP)))
                                    nivel = 4;
                                else
                                    nivel = 5;
                                break;
                        }
                    }
                   
                    await mediator.Send(new SalvarPendenciaUsuarioCommand(request.PendenciaId, valores.Key, valores.Value, nivel));
                }
            }
                

            return true;
        }

        private Dictionary<long, int> AtribuirFuncionarioPerfil(IEnumerable<long> listaFuncionarios, int Cargo, Dictionary<long, int> listaFuncionarioPerfilAtribuido)
        {
            foreach(var funcionario in listaFuncionarios)
            {
                listaFuncionarioPerfilAtribuido.Add(funcionario, Cargo);
            }
            return listaFuncionarioPerfilAtribuido;
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
