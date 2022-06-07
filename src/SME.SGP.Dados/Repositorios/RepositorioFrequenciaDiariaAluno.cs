using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public  class RepositorioFrequenciaDiariaAluno : IRepositorioFrequenciaDiariaAluno
    {
        private readonly ISgpContext database;
        public RepositorioFrequenciaDiariaAluno(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIdsDto>> ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIds(int[] aulaIds)
        {
            var query = @"
                        select 
                            an.id AS AnotacaoId,
                            a.id as AulaId,
                            a.data_aula AS DataAula,
                            case when ma.descricao is not null then ma.descricao else an.anotacao end as MotivoAusencia 
                        from aula a
	                        inner join anotacao_frequencia_aluno an on a.id = an.aula_id 
	                        inner join motivo_ausencia ma on an.motivo_ausencia_id = ma.id
                        WHERE a.id = any(aulaIds)";
            var parametros = new
            {
                aulaIds
            };
            return await database.Conexao.QueryAsync<ObterMotivoAusenciaAlunoFrequenciaDiariaPorAulasIdsDto>(query, parametros);
        }

        public async Task<IEnumerable<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>> ObterQuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplina(int bimestre, string codigoAluno, long turmaId, string aulaDisciplinaId)
        {
            var query = @"
                        SELECT
	                    count(rfa.id)AS TotalAulas,
	                    a.data_aula  AS DataAula,
	                    a.id  AS AulasId,
	                    rfa.valor AS TipoFrequencia
                    FROM
	                    registro_frequencia_aluno rfa
                    INNER JOIN registro_frequencia rf ON
	                    rfa.registro_frequencia_id = rf.id
                    INNER JOIN aula a ON
	                    rf.aula_id = a.id
                    INNER JOIN turma t ON
	                    t.turma_id = a.turma_id
                    INNER JOIN periodo_escolar pe ON
	                    a.tipo_calendario_id = pe.tipo_calendario_id
	                    AND a.data_aula BETWEEN pe.periodo_inicio AND pe.periodo_fim
	                    AND pe.bimestre = @bimestre
                    WHERE
	                    NOT rfa.excluido
	                    AND NOT rf.excluido
	                    AND NOT a.excluido
	                    AND rfa.codigo_aluno = codigoAluno
	                    AND t.id = @turmaId
	                    AND a.disciplina_id = aulaDisciplinaId
                        GROUP  BY a.data_aula,a.id,rfa.valor";

            var parametros = new
            {
                bimestre,
                codigoAluno,
                turmaId,
                aulaDisciplinaId
            };

            return await database.Conexao.QueryAsync<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto>(query,parametros);
        }
    }
}
