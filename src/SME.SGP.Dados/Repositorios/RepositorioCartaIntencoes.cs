using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCartaIntencoes : RepositorioBase<CartaIntencoes>, IRepositorioCartaIntencoes
    {
        public RepositorioCartaIntencoes(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<CartaIntencoes>> ObterPorTurmaEComponente(string turmaCodigo, long componenteCurricularId)
        {
            var query = @"select c.id
	                         , c.planejamento
	                         , c.periodo_escolar_id
	                         , c.criado_em
	                         , c.criado_por
	                         , c.criado_rf
	                         , c.alterado_em
	                         , c.alterado_por
	                         , c.alterado_rf
                          from carta_intencoes c
                         inner join turma t on t.id = c.turma_id
                         where not c.excluido 
                           and t.turma_id = @turmaCodigo
                           and c.componente_curricular_id = @componenteCurricularId";

            return await database.Conexao.QueryAsync<CartaIntencoes>(query, new { turmaCodigo, componenteCurricularId });
        }
    }
}
