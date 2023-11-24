using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDiarioBordo : RepositorioBase<PendenciaDiarioBordo>, IRepositorioPendenciaDiarioBordo
    {
        public RepositorioPendenciaDiarioBordo(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task Excluir(long aulaId, long componenteCurricularId)
        {
            var sql = @"delete from pendencia_diario_bordo where aula_id = @aulaId and componente_curricular_id = @componenteCurricularId ";

            await database.Conexao.ExecuteScalarAsync(sql, new { aulaId, componenteCurricularId }, commandTimeout: 60);
        }

        public async Task ExcluirPorAulaId(long aulaId)
        {
            var sql = @"delete from pendencia_diario_bordo where aula_id = @aulaId";

            await database.Conexao.ExecuteScalarAsync(sql, new { aulaId }, commandTimeout: 60);
        }

        public async Task<bool> VerificarSeExistePendenciaDiarioComPendenciaId(long pendenciaId)
        {
            var sql = @"select count(id) from pendencia_diario_bordo where pendencia_id = @pendenciaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { pendenciaId }) > 0;
        }

        public async Task<IEnumerable<AulaComComponenteDto>> ListarPendenciasDiario(string turmaId, long[] componentesCurricularesId)
        {
            var sqlQuery = @" select distinct a.id as Id,
                                              a.turma_id as TurmaId,
                                              db.componente_curricular_id as ComponenteId,
                                              pe.id as PeriodoEscolarId
                              from aula a
                                   join turma t on a.turma_id = t.turma_id and t.modalidade_codigo = @modalidadeCodigo
                                   join ue on t.ue_id = ue.id
                                   join periodo_escolar pe on a.data_aula between pe.periodo_inicio and pe.periodo_fim     
                                   left join diario_bordo db on db.aula_id = a.id and db.componente_curricular_id = ANY(@componentesCurricularesId) and not db.excluido
                                   left join pendencia_diario_bordo pdb on pdb.aula_id = a.id and pdb.componente_curricular_id = ANY(@componentesCurricularesId)
                                   left join pendencia p on p.id = pdb.pendencia_id and not p.excluido and p.tipo = @tipoPendencia
                              where not a.excluido
                                    and a.data_aula < @hoje
                                    and t.turma_id = @turmaId
                                    and pdb.id is null
                                    and p.id is null
                                    and pe.tipo_calendario_id = a.tipo_calendario_id";
            return await database.Conexao.QueryAsync<AulaComComponenteDto>(sqlQuery.ToString(), new
            {
                hoje = DateTime.Today.Date,
                turmaId,
                componentesCurricularesId,
                modalidadeCodigo = Modalidade.EducacaoInfantil,
                tipoPendencia = TipoPendencia.DiarioBordo
            }, commandTimeout: 200);
        }
    }
}
