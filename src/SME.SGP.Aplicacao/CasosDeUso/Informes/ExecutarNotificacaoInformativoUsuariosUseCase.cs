using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarNotificacaoInformativoUsuariosUseCase : AbstractUseCase, IExecutarNotificacaoInformativoUsuariosUseCase
    {
        public ExecutarNotificacaoInformativoUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
             var param = mensagem.ObterObjetoMensagem<string>();
            if (string.IsNullOrEmpty(param)) return false;
            var informativoId = long.Parse(param);

            var informativo = await mediator.Send(new ObterInformesPorIdQuery(informativoId));
            if (informativo.EhNulo())
                throw new NegocioException("Não foi possível encontrar o Informativo informado");

            var guidPerfis = await ObterPerfisPorCodigos(informativo.Perfis.Select(perfil => perfil.CodigoPerfil));
            var usuariosPerfils = await ObterUsuarios(informativo, guidPerfis); 
            var rfUsuarios = await ObterRfUsuarios(usuariosPerfils, informativo, guidPerfis); 
            
            foreach (var usuario in rfUsuarios.Distinct().ToList())
            {
                var notificacaoInformativoUsuario = new NotificacaoInformativoUsuarioFiltro()
                {
                    InformativoId = informativoId,
                    UsuarioRf = usuario,
                    Titulo = informativo.Titulo,
                    Mensagem = informativo.Texto,
                    DreCodigo = informativo.Dre?.CodigoDre,
                    UeCodigo = informativo.Ue?.CodigoUe
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoInformativoUsuario, notificacaoInformativoUsuario, Guid.NewGuid(), null));  
            }               
            return true;
        }

        private async Task<IEnumerable<UsuarioPerfilsAbrangenciaDto>> ObterUsuarios(Informativo informativo, string[] guidPerfis)
        {
            var usuarios = new List<UsuarioPerfilsAbrangenciaDto>();
            if (informativo.Ue?.CodigoUe is null
                && informativo.Dre?.CodigoDre is null)
            {
                var dres = await mediator.Send(ObterTodasDresQuery.Instance);
                var tasks = dres
                     .AsParallel()
                     .WithDegreeOfParallelism(4)
                     .Select(dre => mediator.Send(new ObterRfsUsuariosPorPerfisDreUeQuery(informativo.Ue?.CodigoUe, dre.CodigoDre, guidPerfis)));

                var results = await Task.WhenAll(tasks);
                foreach (var result in results)
                    usuarios.AddRange(result);
                
            }
            else
                usuarios.AddRange(await mediator.Send(new ObterRfsUsuariosPorPerfisDreUeQuery(informativo.Ue?.CodigoUe, informativo.Dre?.CodigoDre, guidPerfis)));

            return usuarios;
        }

        private async Task<IEnumerable<string>> ObterRfUsuarios(IEnumerable<UsuarioPerfilsAbrangenciaDto> usuariosPerfils, Informativo informativo, string[] perfis)
        {
            if (informativo.Modalidades.Any())
                return await ObterRfUsuarioPorPerfilModalidade(usuariosPerfils, informativo.Modalidades, perfis);

           return usuariosPerfils.Select(up => up.UsuarioRf).Distinct();
        }

        private async Task<IEnumerable<string>> ObterRfUsuarioPorPerfilModalidade(IEnumerable<UsuarioPerfilsAbrangenciaDto> usuariosPerfils, IEnumerable<InformativoModalidade> informativoModalidades, string[] perfis)
        {
            var rfUsuarios = new List<string>();
            var perfisUsuarios = usuariosPerfils.Where(usuario => !(usuario.Perfils is null))
                                          .SelectMany(usuario => usuario.Perfils)
                                          .Where(perfil => perfis.Contains(perfil.Perfil));
            var perfisUe = perfisUsuarios.Where(perfil => Perfis.EhPerfilPorUe(new Guid(perfil.Perfil)));
            var perfilSemUe = perfisUsuarios.Except(perfisUe);

            if (perfisUe.Any())
                rfUsuarios.AddRange(await ObterUsuariosUesModalidade(usuariosPerfils, perfisUe, informativoModalidades));

            rfUsuarios.AddRange(usuariosPerfils.Where(usuario => !(usuario.Perfils is null) &&
                                                      usuario.Perfils.Exists(perfil => perfilSemUe.Contains(perfil)))
                                                .Select(up => up.UsuarioRf));

            return rfUsuarios.Distinct();
        }

        private async Task<IEnumerable<string>> ObterUsuariosUesModalidade(
                                                        IEnumerable<UsuarioPerfilsAbrangenciaDto> usuariosPerfils,
                                                        IEnumerable<PerfilsAbrangenciaDto> perfisUe, 
                                                        IEnumerable<InformativoModalidade> informativoModalidades)
        {
            var ues = perfisUe.Where(perfil => !(perfil.Ues is null))
                  .SelectMany(perfil => perfil.Ues).Distinct();
            var modalidades = informativoModalidades.Select(m => m.Modalidade).ToArray();
            var uesModalidade = await mediator.Send(new ObterCodigosUePorModalidadeQuery(ues.ToArray(), modalidades));

            if (!uesModalidade.Any())
                return Enumerable.Empty<string>();

            var perfisModalidade = perfisUe.Where(perfil => !(perfil.Ues is null) &&
                                                 perfil.Ues.Exists(ue => uesModalidade.Contains(ue)));

            return usuariosPerfils.Where(usuario => !(usuario.Perfils is null) &&
                                        usuario.Perfils.Exists(perfil => perfisModalidade.Contains(perfil)))
                                  .Select(up => up.UsuarioRf);
        }

        private async Task<string[]> ObterPerfisPorCodigos(IEnumerable<long> codigosPerfis)
        {
            var perfis = await mediator.Send(new ObterGruposDeUsuariosQuery((int)TipoPerfil.SME));
            return perfis.Where(perfil => codigosPerfis.Contains(perfil.Id)).Select(perfil => perfil.GuidPerfil.ToString()).ToArray();
        }
    }
}