using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusencia>> Listar(Paginacao paginacao, string turmaId, string disciplinaId, int bimestre, string nomeAtividade)
        {
            var retorno = new PaginacaoResultadoDto<CompensacaoAusencia>();

            var query = new StringBuilder(MontaQuery(paginacao, disciplinaId, bimestre, nomeAtividade));
            query.AppendLine(";");
            query.AppendLine(MontaQuery(paginacao, disciplinaId, bimestre, nomeAtividade, true));

            var nomeAtividadeTratado = $"%{(nomeAtividade ?? "").ToLower()}%";

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new { turmaId, disciplinaId, bimestre, nomeAtividade = nomeAtividadeTratado }))
            {
                retorno.Items = multi.Read<CompensacaoAusencia>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQuery(Paginacao paginacao, string disciplinaId, int bimestre, string nomeAtividade, bool contador = false)
        {
            var select = contador ? "count(c.id)" : "c.id, c.bimestre, c.nome";
            var query = new StringBuilder(string.Format(@"select {0}
                          from compensacao_ausencia c
                         inner join turma t on t.id = c.turma_id
                          where not c.excluido  
                            and t.turma_id = @turmaId ", select));

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and c.disciplina_id = @disciplinaId");
            if (bimestre != 0)
                query.AppendLine("and c.bimestre = @bimestre");
            if (!string.IsNullOrEmpty(nomeAtividade))
                query.AppendLine("and lower(f_unaccent(c.nome)) like @nomeAtividade");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");

            return query.ToString();
        }

        public async Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar)
        {
            var query = @"select * 
                            from compensacao_ausencia c
                          where not excluido
                            and ano_letivo = @anoLetivo
                            and turma_id = @turmaId
                            and nome = @nome
                            and id <> @idIgnorar";

            return await database.Conexao.QueryFirstOrDefaultAsync<CompensacaoAusencia>(query, new { anoLetivo, turmaId, nome, idIgnorar });
        }
    }
}
