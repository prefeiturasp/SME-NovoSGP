using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuarioConsulta : IRepositorioBase<Usuario>
    {
        Task<Usuario> ObterPorCodigoRfLogin(string codigoRf, string login);
        Task<Usuario> ObterPorCodigoRfLoginAsync(string codigoRf, string login);
        Task<Usuario> ObterUsuarioPorCodigoRfAsync(string codigoRf);
        Task<long> ObterUsuarioIdPorCodigoRfAsync(string codigoRf);
        Task<long> ObterUsuarioIdPorLoginAsync(string login);
        Task<ProfessorDto> ObterProfessorDaTurmaPorAulaId(long aulaId);
        Task<IEnumerable<long>> ObterUsuariosIdPorCodigoRf(IList<string> codigoRf);
        Task<IEnumerable<Usuario>> ObterUsuariosPorCodigoRf(IList<string> codigoRf);
        Task<Usuario> ObterPorTokenRecuperacaoSenha(Guid token);
    }
}