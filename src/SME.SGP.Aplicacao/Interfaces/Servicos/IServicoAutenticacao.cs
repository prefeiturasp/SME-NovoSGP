using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IServicoAutenticacao
    {
        Task AlterarSenha(string login, string senhaAtual, string novaSenha);

        //sugestao de nomear tuplas se for manter ou trocar para records ou tipos de estrutura mais flexiveis
        //que encapsulem o retorno
        Task<(UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, bool PossuiCargoCJ, bool PossuiPerfilCJ)> AutenticarNoEol(string login, string senha);

        bool TemPerfilNoToken(string guid);

        Task<(UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>, bool, bool)> AutenticarNoEolSemSenha(string login);
    }
}