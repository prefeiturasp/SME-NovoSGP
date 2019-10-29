using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IServicoEOL servicoEOL;

        public ServicoAutenticacao(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task AlterarSenha(string login, string senhaAtual, string novaSenha)
        {
            var autenticacao = await servicoEOL.Autenticar(login, senhaAtual);
            if (autenticacao == null || autenticacao.Status != AutenticacaoStatusEol.Ok)
            {
                throw new NegocioException("Senha atual incorreta.", HttpStatusCode.Unauthorized);
            }

            var alteracaoSenha = await servicoEOL.AlterarSenha(login, novaSenha);
            if (!alteracaoSenha.SenhaAlterada)
            {
                throw new NegocioException(alteracaoSenha.Mensagem);
            }
        }

        public async Task<(UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>)> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            var retornoDto = new UsuarioAutenticacaoRetornoDto();
            if (retornoServicoEol != null)
            {
                retornoDto.Autenticado = retornoServicoEol.Status == AutenticacaoStatusEol.Ok || retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;
                retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;
            }

            return (retornoDto, retornoServicoEol?.CodigoRf, retornoServicoEol?.Perfis);
        }

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }
    }
}