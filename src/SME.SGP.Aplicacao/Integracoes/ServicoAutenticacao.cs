using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAutenticacao(IServicoEOL servicoEOL,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new System.ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            var retornoDto = new UsuarioAutenticacaoRetornoDto();
            if (retornoServicoEol != null)
            {
                retornoDto.Autenticado = retornoServicoEol.Status == AutenticacaoStatusEol.Ok;
                retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;

                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoServicoEol.CodigoRf, login);

                retornoDto.Token = GeraTokenSeguranca(usuario);
                retornoDto.Perfis = DefinirPerfilPrioritario(usuario, retornoServicoEol.Perfis);
            }

            return retornoDto;
        }

        private IEnumerable<PerfilDto> DefinirPerfilPrioritario(Usuario usuario, IEnumerable<Guid> perfis)
        {
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfis);

            return null;
        }

        private string GeraTokenSeguranca(Usuario usuario)
        {
            return string.Empty;
        }
    }
}