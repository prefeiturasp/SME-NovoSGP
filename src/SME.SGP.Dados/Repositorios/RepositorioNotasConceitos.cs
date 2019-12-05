using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotasConceitos : RepositorioBase<NotaConceito>, IRepositorioNotasConceitos
    {
        public RepositorioNotasConceitos(ISgpContext sgpContext) : base(sgpContext)
        {
        }

        public IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas, IEnumerable<string> alunosIds)
        {
            var sql = @"select id, atividade_avaliativa, aluno_id, nota, conceito, tipo_nota, criado_em, 
                        criado_por, criado_rf, alterado_em, alterado_por, alterado_rf  
                        from notas_conceito where atividadeAvaliativa in @atividadesAvaliativas and aluno_id in @alunosIds";

            var parametros = new { atividadesAvaliativas, alunosIds };

            return database.Query<NotaConceito>(sql, parametros);
        }
    }
}