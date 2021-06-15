using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}