using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using SME.SGP.Infra;
using System.Linq;

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
                            and aluno_id = any(array[@alunosIdsString])
                            and disciplina_id = @disciplinaId";

            var parametros = new
            {                
                alunosIdsString,
                disciplinaId
            };

            return database.Query<NotaConceito>(sql, parametros);
        }
    }
}