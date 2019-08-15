using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoDesenvolvimentoPlano : RepositorioBase<ObjetivoDesenvolvimentoPlano>, IRepositorioObjetivoDesenvolvimentoPlano
    {
        public RepositorioObjetivoDesenvolvimentoPlano(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<ObjetivoDesenvolvimentoPlano> ObterObjetivosDesenvolvimentoPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<ObjetivoDesenvolvimentoPlano>("select * from objetivo_desenvolvimento_plano where plano_id = @Id", new { Id = idPlano });
        }
    }
}