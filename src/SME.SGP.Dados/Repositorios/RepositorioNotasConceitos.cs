using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotasConceitos : RepositorioBase<NotaConceito>, IRepositorioNotasConceitos
    {
        public RepositorioNotasConceitos(ISgpContext sgpContext) : base(sgpContext)
        {
        }

        public IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas, IEnumerable<string> alunosIds, string disciplinaId)
        {
            var atividadesAvaliativasString = string.Join(",", atividadesAvaliativas.Distinct());
            var alunosIdsString = $"'{string.Join("','", alunosIds.Distinct())}'";

            var sql = $@"select id, 
                                atividade_avaliativa, 
                                aluno_id, 
                                nota, 
                                conceito, 
                                tipo_nota, 
                                criado_em,
                                criado_por, 
                                criado_rf, 
                                alterado_em, 
                                alterado_por, 
                                alterado_rf
                         from notas_conceito 
                         where atividade_avaliativa = any(array[{atividadesAvaliativasString}]) 
                            and aluno_id = any(array[{alunosIdsString}])
                            and disciplina_id = @disciplinaId";

            return database.Query<NotaConceito>(sql, new { disciplinaId });
        }
        public async Task<IEnumerable<NotaConceito>> ObterNotasPorAlunosAtividadesAvaliativasAsync(long[] atividadesAvaliativasId, string[] alunosIds, string componenteCurricularId)
        {

            var sql = $@"select id, 
                                atividade_avaliativa, 
                                aluno_id, 
                                nota, 
                                conceito, 
                                tipo_nota, 
                                criado_em,
                                criado_por, 
                                criado_rf, 
                                alterado_em, 
                                alterado_por, 
                                alterado_rf
                         from notas_conceito 
                         where atividade_avaliativa = any(@atividadesAvaliativasId) 
                            and aluno_id = any(@alunosIds)
                            and disciplina_id = @componenteCurricularId";

            return await database.QueryAsync<NotaConceito>(sql, new { atividadesAvaliativasId, alunosIds, componenteCurricularId });
        }

        public async Task<NotaConceito> ObterNotasPorGoogleClassroomIdTruemaIdComponentCurricularId(long atividadeClassroomId, string turmaId, string componenteCurricularId)
        {
            var sql = $@"select nc.id, 
                                nc.atividade_avaliativa, 
                                nc.aluno_id, 
                                nc.nota, 
                                nc.conceito, 
                                nc.tipo_nota, 
                                nc.criado_em,
                                nc.criado_por, 
                                nc.criado_rf, 
                                nc.alterado_em, 
                                nc.alterado_por, 
                                nc.alterado_rf
                         from notas_conceito nc
                         inner join atividade_avaliativa aa on nc.atividade_avaliativa = aa.id 
                         where aa.atividade_classroom_id = @atividadeClassroomId and aa.turma_id = @turmaId and nc.disciplina_id = @componenteCurricularId";

            return await database.QuerySingleOrDefaultAsync<NotaConceito>(sql, new { atividadeClassroomId, turmaId, componenteCurricularId });
        }

        public async Task<NotaConceito> ObterNotasPorId(long id)
        {
            var sql = $@"select * from notas_conceito nc where nc.id = @id";

            return await database.QuerySingleOrDefaultAsync<NotaConceito>(sql, new { id });
        }
    }
}