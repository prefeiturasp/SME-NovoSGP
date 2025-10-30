using SME.SGP.Dominio.Dtos;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioReclassificacaoConsulta : IRepositorioReclassificacaoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioReclassificacaoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ReclassificacaoRawDto>> ObterReclassificacao(string codigoDre, string codigoUe, int anoLetivo, int anoTurma)
        {
            string query = @"
                select 
                    modalidade_turma as CodigoModalidade,
                    ano_turma as AnoTurma,
                    quantidade_alunos_reclassificados as QuantidadeAlunos
                from painel_educacional_consolidacao_reclassificacao
                where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                query += " AND codigo_ue = @codigoUe ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (anoTurma > 0)
                query += " AND ano_turma = @anoTurma ";

            return await database.Conexao.QueryAsync<ReclassificacaoRawDto>(query, new
            {
                codigoDre,
                codigoUe,
                anoLetivo,
                anoTurma
            });
        }
    }
}
