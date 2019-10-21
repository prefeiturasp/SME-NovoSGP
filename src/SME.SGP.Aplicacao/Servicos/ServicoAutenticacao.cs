using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoAutenticacao : IServicoAutenticacao
    {
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoTokenJwt servicoTokenJwt;

        public ServicoAutenticacao(IServicoEOL servicoEOL, IServicoTokenJwt servicoTokenJwt)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoTokenJwt = servicoTokenJwt ?? throw new ArgumentNullException(nameof(servicoTokenJwt));
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