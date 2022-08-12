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

        public async Task<long> ObterIdPorObjetivoAprendizagemJuremaAsync(long planoId, long objetivoAprendizagemJuremaId)
        {
            var query = "select id from objetivo_aprendizagem_plano where plano_id = @planoId and objetivo_aprendizagem_jurema_id = @objetivoAprendizagemJuremaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { planoId, objetivoAprendizagemJuremaId });
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
            query.AppendLine("inner join componente_curricular cc on cc.id = o.componente_curricular_id");            
            query.AppendLine("inner join objetivo_aprendizagem oa on o.objetivo_aprendizagem_jurema_id = oa.id ");
            query.AppendLine("where pa.ano = @ano");
            query.AppendLine("and pa.bimestre = @bimestre");
            query.AppendLine("and pa.componente_curricular_eol_id = @componenteCurricularId");
            query.AppendLine("and pa.turma_id = @turmaId");

            if (ehRegencia)
                query.AppendLine("and cc.id = @disciplinaId");

            return database.Conexao.QueryAsync<ObjetivoAprendizagem>(query.ToString(), new { ano, bimestre, componenteCurricularId, turmaId, disciplinaId });
        }

        public async Task<IEnumerable<ObjetivosAprendizagemPorComponenteDto>> ObterObjetivosPorComponenteNoPlano(int bimestre, long turmaId, long componenteCurricularId, long disciplinaId, bool ehRegencia = false)
        {
            var query = new StringBuilder(@"select cc.id, oa.*
             from planejamento_anual pa
            inner join planejamento_anual_periodo_escolar ppe on ppe.planejamento_anual_id = pa.id
            inner join periodo_escolar pe on pe.id = ppe.periodo_escolar_id
            inner join planejamento_anual_componente pc on pc.planejamento_anual_periodo_escolar_id = ppe.id
            inner join componente_curricular cc on cc.id = pc.componente_curricular_id
            inner join planejamento_anual_objetivos_aprendizagem po on po.planejamento_anual_componente_id = pc.id
            inner join objetivo_aprendizagem oa on oa.Id = po.objetivo_aprendizagem_id
            where pa.turma_id = @turmaId
              and pe.bimestre = @bimestre
              and pa.componente_curricular_id = @componenteCurricularId
              and pa.excluido = false
              and pc.excluido = false
              and po.excluido = false
              and ppe.excluido = false");

            if (ehRegencia)
                query.AppendLine(" and cc.id = @disciplinaId");

            query.AppendLine(" order by oa.codigo");
            var lookup = new Dictionary<long, ObjetivosAprendizagemPorComponenteDto>();


            await database.Conexao.QueryAsync<long, ObjetivoAprendizagem, long>(query.ToString(), (componenteId, objetivoAprendizagem) =>
                {
                    ObjetivosAprendizagemPorComponenteDto retorno = null;
                    if (!lookup.TryGetValue(componenteId, out retorno))
                    {
                        retorno = new ObjetivosAprendizagemPorComponenteDto();
                        retorno.ComponenteCurricularId = componenteId;
                        lookup.Add(componenteId, retorno);
                    }

                    retorno.ObjetivosAprendizagem.Add(new ObjetivoAprendizagemDto()
                    {
                        Id = objetivoAprendizagem.Id,
                        Ano = objetivoAprendizagem.Ano.ToString(),
                        Codigo = objetivoAprendizagem.Codigo,
                        IdComponenteCurricular = objetivoAprendizagem.ComponenteCurricularId,
                        Descricao = objetivoAprendizagem.Descricao
                    });

                    return componenteId;
                }
                , new { bimestre, componenteCurricularId, turmaId, disciplinaId });

            return lookup.Values;
        }
    }
}