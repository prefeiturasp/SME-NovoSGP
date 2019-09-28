using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IServicoEOL servicoEOL;

        public ServicoAutenticacao(IServicoEOL servicoEOL,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new System.ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<(UsuarioAutenticacaoRetornoDto, string)> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            var retornoDto = new UsuarioAutenticacaoRetornoDto();
            if (retornoServicoEol != null)
            {
                retornoDto.Autenticado = retornoServicoEol.Status == AutenticacaoStatusEol.Ok;
                retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;

                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoServicoEol.CodigoRf, login);

                retornoDto.Token = GeraTokenSeguranca(usuario);
                retornoDto.PerfisUsuario = DefinirPerfilPrioritario(retornoServicoEol.Perfis, usuario);
            }

            return (retornoDto, retornoServicoEol == null ? string.Empty : retornoServicoEol.CodigoRf);
        }

        private PerfisPorPrioridadeDto DefinirPerfilPrioritario(IEnumerable<Guid> perfis, Usuario usuario)
        {
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfis);

            Guid perfilPrioritario = usuario.ObterPerfilPrioritario(perfisUsuario);

            var perfisPorPrioridade = new PerfisPorPrioridadeDto
            {
                PerfilSelecionado = perfilPrioritario,
                Perfis = MapearPerfisParaDto(perfisUsuario)
            };
            return perfisPorPrioridade;
        }

        private string GeraTokenSeguranca(Usuario usuario)
        {
            // priorizar os perfis
            //Gerar o token com os permissionamentos do perfil priorizado
            return string.Empty;
        }

        private IList<PerfilDto> MapearPerfisParaDto(IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            return perfisUsuario?.Select(c => new PerfilDto
            {
                CodigoPerfil = c.CodigoPerfil,
                NomePerfil = c.NomePerfil
            }).ToList();
        }
    }
}