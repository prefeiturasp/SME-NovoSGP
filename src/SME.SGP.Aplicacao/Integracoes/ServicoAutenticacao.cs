using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAutenticacao(IServicoEOL servicoEOL,
                                   IRepositorioUsuario repositorioUsuario,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new System.ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(primeiroAcessoDto.RFCPF, primeiroAcessoDto.Usuario);

            usuario.ValidarSenha(primeiroAcessoDto.NovaSenha);

            return await servicoEOL.AlterarSenha(usuario.Login, primeiroAcessoDto.NovaSenha);
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
                retornoDto.PerfisUsuario = DefinirPerfilPrioritario(retornoServicoEol.Perfis, usuario);
            }

            return retornoDto;
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