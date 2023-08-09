
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface ISgpContext : IDbConnection, IAsyncDisposable
    {
        IDbConnection Conexao { get; }
        string PerfilUsuario { get; }
        string UsuarioLogadoNomeCompleto { get; }
        string UsuarioLogadoRF { get; }
        string Administrador { get; }

        Task OpenAsync();
        Task CloseAsync();
        Task<IDbTransaction> BeginTransactionAsync();
    }
}