using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoAutenticacao
    {
        Task AlterarSenha(string login, string senhaAtual, string novaSenha);

        Task<(UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>)> AutenticarNoEol(string login, string senha);

        bool TemPerfilNoToken(string guid);
    }
}