
using System;
using System.Data;

namespace SME.SGP.Infra
{
    public interface ISgpContext : IDbConnection
    {
        IDbConnection Conexao { get; }
        string UsuarioLogado { get; }
        Guid PerfilUsuario { get; }
        string UsuarioLogadoNomeCompleto { get; }
        string UsuarioLogadoRF { get; }

        void AbrirConexao();
        void FecharConexao();
    }
}