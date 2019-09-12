using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemPlano : RepositorioBase<ObjetivoAprendizagemPlano>, IRepositorioObjetivoAprendizagemPlano
    {
        public RepositorioObjetivoAprendizagemPlano(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<ObjetivoAprendizagemPlano> ObterObjetivosAprendizagemPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<ObjetivoAprendizagemPlano>("select * from objetivo_aprendizagem_plano where plano_id = @Id", new { Id = idPlano });
        }
    }
}