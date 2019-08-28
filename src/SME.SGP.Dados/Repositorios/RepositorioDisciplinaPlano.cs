using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDisciplinaPlano : RepositorioBase<DisciplinaPlano>, IRepositorioDisciplinaPlano
    {
        public RepositorioDisciplinaPlano(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<DisciplinaPlano> ObterDisciplinasPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<DisciplinaPlano>("select * from disciplina_plano where plano_id = @Id", new { Id = idPlano });
        }
    }
}