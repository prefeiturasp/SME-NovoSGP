using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    //public class RepositorioTestePostgre : RepositorioBase<Ciclo>, IRepositorioTestePostgre
    public class RepositorioTestePostgre : RepositorioBase<Ciclo>, IRepositorioTestePostgre
    {
        private readonly ISgpContext database;

        public RepositorioTestePostgre(ISgpContext database) : base(database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        //public IEnumerable<Ciclo> Listar()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Ciclo ObterPorId(long id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task<Ciclo> ObterPorIdAsync(long id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void Remover(long id)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void Remover(Ciclo entidade)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public long Salvar(Ciclo entidade)
        //{
        //    entidade.CriadoEm = DateTime.Now;
        //    //entidade.CriadoPor = "teste";
        //    //entidade.CriadoRF = "teste";
        //    entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
        //    entidade.CriadoRF = database.UsuarioLogadoRF;



        //    //database.Conexao.Insert(entidade);

        //    //using (var conexao = new NpgsqlConnection("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=sgp_db;Pooling=true;MaxPoolSize=10;ConnectionIdleLifetime=10;"))
        //    //{
        //    //database.Conexao.Open();
        //    //database.AbrirConexao();
        //    database.Conexao.Insert(entidade);
        //    //database.FecharConexao();
        //    //database.Conexao.Close();
        //    //}
        //    return entidade.Id;
        //}

        //public Task<long> SalvarAsync(Ciclo entidade)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
