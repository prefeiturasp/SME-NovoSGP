using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoAutenticacao
    {
        Task<(UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>)> AutenticarNoEol(string login, string senha);
        Task<UsuarioAutenticacaoRetornoDto> AutenticarNoEol(string login, string senha);

        Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto);
    }
}