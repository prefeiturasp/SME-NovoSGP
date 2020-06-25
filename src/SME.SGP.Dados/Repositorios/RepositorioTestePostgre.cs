using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    //public class RepositorioTestePostgre : RepositorioBase<Ciclo>, IRepositorioTestePostgre
    public class RepositorioTestePostgre : IRepositorioTestePostgre
    {
        private readonly ISgpContext database;

        public RepositorioTestePostgre(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public IEnumerable<Ciclo> Listar()
        {
            throw new System.NotImplementedException();
        }

        public Ciclo ObterPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Ciclo> ObterPorIdAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(Ciclo entidade)
        {
            throw new System.NotImplementedException();
        }

        public long Salvar(Ciclo entidade)
        {
            entidade.CriadoEm = DateTime.Now;
            entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
            entidade.CriadoRF = database.UsuarioLogadoRF;
            database.Conexao.Insert(entidade);
            return entidade.Id;
        }




        //using (var conexao = new NpgsqlConnection("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=sgp_db;Pooling=true;MaxPoolSize=10;ConnectionIdleLifetime=10;"))
        //{
        //database.Conexao.Open();
        //database.AbrirConexao();
        //database.FecharConexao();
        //database.Conexao.Close();
        //}

        public Task<long> SalvarAsync(Ciclo entidade)
        {
            throw new System.NotImplementedException();
        }
    }
}
