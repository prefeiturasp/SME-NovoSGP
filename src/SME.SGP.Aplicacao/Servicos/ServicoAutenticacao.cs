using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IMediator mediator;

        public ServicoAutenticacao(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task AlterarSenha(string login, string senhaAtual, string novaSenha)
        {
            var autenticacao = await mediator.Send(new AutenticarQuery(login, senhaAtual));
            
            if (autenticacao.EhNulo() || autenticacao.Status != AutenticacaoStatusEol.Ok)
                throw new NegocioException(MensagemNegocioComuns.SENHA_ATUAL_INCORRETA, HttpStatusCode.Unauthorized);

            var alteracaoSenha = await mediator.Send(new AlterarSenhaUsuarioCommand(login, novaSenha));
            if (!alteracaoSenha.SenhaAlterada)
                throw new NegocioException(alteracaoSenha.Mensagem);
        }

        public async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEol(AutenticacaoApiEolDto retornoServicoEol)
        {
            return await ObterAutenticacao(retornoServicoEol);
        }

        public async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEolSemSenha(string login)
        {
            var retornoServicoEol = await mediator.Send(new ObterAutenticacaoSemSenhaQuery(login));

            return await ObterAutenticacao(retornoServicoEol);
        }

        private async Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> ObterAutenticacao(AutenticacaoApiEolDto retornoServicoEol)
        {
            var retornoDto = new UsuarioAutenticacaoRetornoDto();

            if (retornoServicoEol.EhNulo()) 
                return (retornoDto, "", null, false, false);
            
            retornoDto.Autenticado = retornoServicoEol.Status is AutenticacaoStatusEol.Ok or AutenticacaoStatusEol.SenhaPadrao;
            retornoDto.ModificarSenha = retornoServicoEol.Status == AutenticacaoStatusEol.SenhaPadrao;
            retornoDto.UsuarioId = retornoServicoEol.UsuarioId;

            var perfis = await mediator.Send(new ObterPerfisPorLoginQuery(retornoServicoEol.CodigoRf));

            if (perfis.EhNulo())
                throw new NegocioException("Usuário sem perfis de acesso.");

            return (retornoDto, retornoServicoEol.CodigoRf, perfis.Perfis, perfis.PossuiCargoCJ, perfis.PossuiPerfilCJ);
        }

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }
    }
}