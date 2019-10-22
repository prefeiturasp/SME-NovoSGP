using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IServicoAutenticacao
    {
        Task<(UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>)> AutenticarNoEol(string login, string senha);

        bool TemPerfilNoToken(string guid);
    }
}