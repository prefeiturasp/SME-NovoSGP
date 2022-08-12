using System.Data;

namespace SME.SGP.Auditoria.Worker.Interfaces
{
    public interface ISgpContext 
    {
        IDbConnection Conexao { get; }
        void AbrirConexao();
        void FecharConexao();
    }
}
