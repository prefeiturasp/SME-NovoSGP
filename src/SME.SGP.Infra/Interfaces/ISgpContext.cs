
using System;
using System.Data;

namespace SME.SGP.Infra
{
    public interface ISgpContext : IDbConnection
    {
        IDbConnection Conexao { get; }
        string UsuarioLogado { get; }
        string UsuarioLogadoNomeCompleto { get; }
        string UsuarioLogadoRF { get; }
        Guid UsuarioPerfil { get;}
        void AbrirConexao();
        void FecharConexao();
    }
}