using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Auditoria.Worker.Interfaces;
using System;
using System.Data;

namespace SME.SGP.Auditoria.Worker
{
    public class SgpContext : ISgpContext
    {
        private readonly IDbConnection conexao;

        public SgpContext(IConfiguration configuration)
        {
            conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
        }

        public IDbConnection Conexao => conexao;

        public void AbrirConexao()
        {
            Open();
        }

        public void FecharConexao()
        {
            if (conexao.State != ConnectionState.Closed)
            {
                Close();
            }
        }

        public void Close()
        {
            conexao.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (conexao.State == ConnectionState.Open)
                conexao.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Open()
        {
            if (conexao.State != ConnectionState.Open)
                conexao.Open();
        }


    }
}
