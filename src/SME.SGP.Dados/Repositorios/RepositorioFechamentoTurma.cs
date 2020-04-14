using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurma: RepositorioBase<FechamentoTurma>, IRepositorioFechamentoTurma
    {
        public RepositorioFechamentoTurma(ISgpContext database): base(database)
        {
        }

        public async Task<FechamentoTurma> ObterCompletoPorIdAsync(long fechamentoTurmaId)
        {
            var query = @"select f.*, t.*, ue.*, dre.*, pe.*
                           from fechamento_turma f
                          inner join turma t on t.id = f.turma_id
                          inner join ue on ue.id = t.ue_id
                          inner join dre on dre.id = ue.dre_id
                           left join periodo_escolar pe on pe.id = f.periodo_escolar_id
                          where f.id = @fechamentoTurmaId";

            return (await database.Conexao.QueryAsync<FechamentoTurma, Turma, Ue, Dre, PeriodoEscolar, FechamentoTurma>(query
                , (fechamentoTurma, turma, ue, dre, periodoEscolar) =>
                {
                    ue.Dre = dre;
                    turma.Ue = ue;
                    fechamentoTurma.Turma = turma;
                    fechamentoTurma.PeriodoEscolar = periodoEscolar;

                    return fechamentoTurma;
                }
                , new { fechamentoTurmaId })).FirstOrDefault();
        }

        public async Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0)
        {
            var query = new StringBuilder(@"select * 
                            from fechamento_turma 
                           where not excluido 
                            and turma_id = @turmaId ");
            if (periodoId > 0)
                query.AppendLine(" and periodo_escolar_id = @periodoId");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { turmaId, periodoId });
        }
    }
}
