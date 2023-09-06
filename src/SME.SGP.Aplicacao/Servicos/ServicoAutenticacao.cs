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
        private readonly IServicoEol servicoEOL;

        public ServicoAutenticacao(IServicoEol servicoEOL)
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

        public async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEol(string login, string senha)
        {
            var retornoServicoEol = await servicoEOL.Autenticar(login, senha);

            return await ObterAutenticacao(retornoServicoEol);
        }

        public async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEolSemSenha(string login)
        {
            var retornoServicoEol = await servicoEOL.ObtenhaAutenticacaoSemSenha(login);

            return await ObterAutenticacao(retornoServicoEol);
        }

        private async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> ObterAutenticacao(AutenticacaoApiEolDto retornoServicoEol)
        {
            var retornoDto = new UsuarioAutenticacaoRetornoDto();

            if (retornoServicoEol == null) 
                return (retornoDto, "", null, false, false);
            
            retornoDto.Autenticado = retornoServicoEol.Status is AutenticacaoStatusEol.Ok or AutenticacaoStatusEol.SenhaPadrao;
            retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;
            retornoDto.UsuarioId = retornoServicoEol.UsuarioId;

            var perfis = await servicoEOL.ObterPerfisPorLogin(retornoServicoEol.CodigoRf);

            if (perfis == null)
                throw new NegocioException("Usuário sem perfis de acesso.");

            return (retornoDto, retornoServicoEol.CodigoRf, perfis.Perfis, perfis.PossuiCargoCJ, perfis.PossuiPerfilCJ);
        }

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }
    }
}