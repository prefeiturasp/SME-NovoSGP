using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemPlano : RepositorioBase<ObjetivoAprendizagemPlano>, IRepositorioObjetivoAprendizagemPlano
    {
        public RepositorioObjetivoAprendizagemPlano(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<ComponenteCurricularSimplificadoDto> ObterDisciplinasDoBimestrePlanoAula(int ano, int bimestre, long turmaId, long componenteCurricularId)
        {
            var query = @"select distinct c.id, c.descricao_eol as descricao
                   from plano_anual pa
                  inner join objetivo_aprendizagem_plano o on o.plano_id = pa.id
                  inner join componente_curricular_jurema c on c.id = o.componente_curricular_id
                  where pa.ano = @ano
                    and pa.bimestre = @bimestre
                    and pa.turma_id = @turmaId
                    and pa.componente_curricular_eol_id = @componenteCurricularId";

            return database.Conexao.Query<ComponenteCurricularSimplificadoDto>(query, new
            {
                ano,
                bimestre,
                turmaId,
                componenteCurricularId
            });
        }

        public long ObterIdPorObjetivoAprendizagemJurema(long planoId, long objetivoAprendizagemJuremaId)
        {
            var query = "select id from objetivo_aprendizagem_plano where plano_id = @planoId and objetivo_aprendizagem_jurema_id = @objetivoAprendizagemJuremaId";

            return database.Conexao.QueryFirstOrDefault<long>(query, new { planoId, objetivoAprendizagemJuremaId });
        }

        public IEnumerable<ObjetivoAprendizagemPlano> ObterObjetivosAprendizagemPorIdPlano(long idPlano)
        {
            return database.Conexao.Query<ObjetivoAprendizagemPlano>("select * from objetivo_aprendizagem_plano where plano_id = @Id", new { Id = idPlano });
        }

        public Task<IEnumerable<ObjetivoAprendizagem>> ObterObjetivosPlanoDisciplina(int ano, int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool ehRegencia = false)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select oa.*");
            query.AppendLine(" from plano_anual pa");
            query.AppendLine("inner join objetivo_aprendizagem_plano o on o.plano_id = pa.id");            
            query.AppendLine("inner join componente_curricular_jurema cc on cc.id = o.componente_curricular_id");            
            query.AppendLine("inner join objetivo_aprendizagem oa on o.objetivo_aprendizagem_jurema_id = oa.id ");
            query.AppendLine("where pa.ano = @ano");
            query.AppendLine("and pa.bimestre = @bimestre");
            query.AppendLine("and pa.componente_curricular_eol_id = @componenteCurricularId");
            query.AppendLine("and pa.turma_id = @turmaId");

            if (ehRegencia)
                query.AppendLine("and cc.codigo_eol = @disciplinaId");

            return database.Conexao.QueryAsync<ObjetivoAprendizagem>(query.ToString(), new { ano, bimestre, componenteCurricularId, turmaId, disciplinaId });
        }
    }
}