using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IServicoAutenticacao
    {
        Task AlterarSenha(string login, string senhaAtual, string novaSenha);

        Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEol(AutenticacaoApiEolDto autenticacao);

        bool TemPerfilNoToken(string guid);

        Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEolSemSenha(string login);
    }
}