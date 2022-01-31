using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAula : RepositorioBase<PlanoAula>, IRepositorioPlanoAula
    {
        public RepositorioPlanoAula(ISgpContext conexao) : base(conexao) { }

        public async Task ExcluirPlanoDaAula(long aulaId)
        {
            // Excluir objetivos de aprendizagem do plano
            var command = @"update objetivo_aprendizagem_aula 
                                set excluido = true
                            where plano_aula_id in (
                                select id from plano_aula 
                                 where not excluido and aula_id = @aulaId) ";
            await database.ExecuteAsync(command, new { aulaId });

            // Excluir plano de aula
            command = "update plano_aula set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorAula(long aulaId)
        {
            var query = "select * from plano_aula where not excluido and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorAulaRegistroExcluido(long aulaId)
        {
            var query = "select * from plano_aula where aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { aulaId });
        }

        public async Task<PlanoAula> ObterPlanoAulaPorDataDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select pa.*
                 from plano_aula pa
                inner join aula a on a.Id = pa.aula_id
                where not a.excluido 
                  and not pa.excluido
                  and DATE(a.data_aula) = @data
                  and a.turma_id = @turmaId
                  and a.disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanoAula>(query, new { data, turmaId, disciplinaId });
        }

        public async Task<bool> PlanoAulaRegistradoAsync(long aulaId)
        {
            var query = "select 1 from plano_aula where aula_id = @aulaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { aulaId });
        }

        public bool ValidarPlanoExistentePorTurmaDataEDisciplina(DateTime data, string turmaId, string disciplinaId)
        {
            var query = @"select
	                        1
                        from
	                        plano_aula pa
                        inner join aula a on a.Id = pa.aula_id
                        where not a.excluido 
                            and not pa.excluido 
                            and DATE(a.data_aula) = @data
                            and a.turma_id = @turmaId
                            and a.disciplina_id = @disciplinaId";

            return database.Conexao.Query<bool>(query, new { data = data.Date, turmaId, disciplinaId }).SingleOrDefault();
        }

        public async Task<PlanoAulaObjetivosAprendizagemDto> ObterPlanoAulaEObjetivosAprendizagem(long aulaId)
        {
            var query = @"select
                           pa.id, pa.descricao, pa.recuperacao_aula as RecuperacaoAula, pa.licao_casa as LicaoCasa, pa.migrado,
                           pa.criado_em as CriadoEm, pa.alterado_em as AlteradoEm, pa.criado_por as CriadoPor, pa.alterado_por as AlteradoPor, pa.criado_rf as CriadoRf, pa.alterado_rf as AlteradoRf,
                           a.id as AulaId, a.ue_id as UeId, a.disciplina_id as DisciplinaId, a.turma_id as TurmaId,
                           a.quantidade, a.tipo_calendario_id as TipoCalendarioId, a.data_aula as DataAula,
                           oaa.componente_curricular_id as id,
                           oa.id, oa.descricao, oa.codigo, oa.ano_turma as Ano, oa.componente_curricular_id as IdComponenteCurricular
                      from aula a
                      inner join plano_aula pa on a.id = pa.aula_id
                      left join objetivo_aprendizagem_aula oaa on pa.id = oaa.plano_aula_id
                      left join objetivo_aprendizagem oa on oaa.objetivo_aprendizagem_id = oa.id
                     where a.id = @aulaId";

            var lookup = new Dictionary<long, PlanoAulaObjetivosAprendizagemDto>();

            await database.Conexao.QueryAsync<PlanoAulaObjetivosAprendizagemDto, long?, ObjetivoAprendizagemDto, PlanoAulaObjetivosAprendizagemDto>(query, (planoAulaObjetivosAprendizagemDto, componenteId, objetivoAprendizagemDto) =>
            {

                var retorno = new PlanoAulaObjetivosAprendizagemDto();
                if (!lookup.TryGetValue(planoAulaObjetivosAprendizagemDto.Id, out retorno))
                {
                    retorno = planoAulaObjetivosAprendizagemDto;
                    lookup.Add(planoAulaObjetivosAprendizagemDto.Id, retorno);
                }

                var objetivoComponente = retorno.ObjetivosAprendizagemComponente.FirstOrDefault(c => c.ComponenteCurricularId == componenteId);
                if (objetivoComponente == null && componenteId.HasValue)
                {
                    objetivoComponente = new ObjetivosAprendizagemPorComponenteDto();
                    objetivoComponente.ComponenteCurricularId = componenteId.Value;

                    retorno.Adicionar(objetivoComponente);
                }

                if (objetivoComponente != null && objetivoAprendizagemDto != null)
                {
                    objetivoComponente.ObjetivosAprendizagem.Add(objetivoAprendizagemDto);
                }

                return retorno;
            }, param: new
            {
                aulaId
            });

            return lookup.Values.FirstOrDefault();
        }
    }
}
