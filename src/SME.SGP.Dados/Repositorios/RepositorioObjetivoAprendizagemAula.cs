using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemAula : RepositorioBase<ObjetivoAprendizagemAula>, IRepositorioObjetivoAprendizagemAula
    {
        public RepositorioObjetivoAprendizagemAula(ISgpContext database) : base(database)
        { }

        public async Task LimparObjetivosAula(long planoAulaId)
        {
            var command = "delete from objetivo_aprendizagem_aula where plano_aula_id = @planoAulaId";

            await database.ExecuteAsync(command, new { planoAulaId });
        }

        public async Task<IEnumerable<ObjetivoAprendizagemComponenteDto>> ObterObjetivosComComponentePlanoAula(long planoAulaId)
        {
            var query = @"select
                             oaa.componente_curricular_id as ComponenteCurricularId,
                             oaa.objetivo_aprendizagem_id as Id
                        from objetivo_aprendizagem_aula oaa
                        where not oaa.excluido
                         and oaa.plano_aula_id = @planoAulaId";

            return await database.Conexao.QueryAsync<ObjetivoAprendizagemComponenteDto>(query, new { planoAulaId });
        }

        public async Task<IEnumerable<ObjetivoAprendizagemAula>> ObterObjetivosPlanoAula(long planoAulaId)
        {
            var query = @"select
                         oaa.plano_aula_id as PlanoAulaId,
                         oaa.objetivo_aprendizagem_plano_id as ObjetivoAprendizagemPlanoId,
                         oa.id as objetivoAprendizagemId
                        from
                         objetivo_aprendizagem_aula oaa
                        inner join objetivo_aprendizagem oa on
                         oaa.objetivo_aprendizagem_id = oa.id 
                        where
                         not oaa.excluido
                         and oaa.plano_aula_id = @planoAulaId";

            return await database.Conexao.QueryAsync<ObjetivoAprendizagemAula, ObjetivoAprendizagem, ObjetivoAprendizagemAula>(query,
                (objetivoAula, objetivoAprendizagem) =>
                {
                    objetivoAula.ObjetivoAprendizagem = objetivoAprendizagem;
                    return objetivoAula;
                },
                new { planoAulaId },
                splitOn: "id, objetivoAprendizagemId");
        }
    }
}
