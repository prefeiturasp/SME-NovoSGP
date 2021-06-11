using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioRegistroFrequenciaAluno : RepositorioBase<RegistroFrequenciaAluno>, IRepositorioRegistroFrequenciaAluno
    {
        public RepositorioRegistroFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> codigoAlunos)
        {
            var query = @"           
                    select
	                count(rfa.id) as TotalAusencias,
	                p.id as PeriodoEscolarId,
	                p.periodo_inicio as PeriodoInicio,
	                p.periodo_fim as PeriodoFim,
	                p.bimestre,
                    rfa.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId                    
                from
	                registro_frequencia_aluno rfa
                inner join registro_frequencia rf on
	                rfa.registro_frequencia_id = rf.id
                inner join aula a on
	                rf.aula_id = a.id
                inner join periodo_escolar p on
	                a.tipo_calendario_id = p.tipo_calendario_id
                where
	                not rfa.excluido
	                and not a.excluido
	                and rfa.codigo_aluno = any(@codigoAlunos)	                
	                and a.turma_id = @turmaId
	                and p.periodo_inicio <= @dataAula
	                and p.periodo_fim >= @dataAula
	                and a.data_aula >= p.periodo_inicio
	                and a.data_aula <= p.periodo_fim
	                and not a.excluido 
                    and rfa.valor = @tipoFrequencia
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmaId, tipoFrequencia = (int)TipoFrequencia.F });
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId)
        {
			var query = @"select 
                        rfa.codigo_aluno as CodigoAluno,
                        rfa.numero_aula as NumeroAula,
                        rfa.valor as TipoFrequencia
                        from registro_frequencia rf 
                        inner join registro_frequencia_aluno rfa on rf.id = rfa.registro_frequencia_id 
                        where rf.aula_id = @aulaId";

			return await database.Conexao.QueryAsync<FrequenciaAlunoSimplificadoDto>(query, new { aulaId });
		}

        public async Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM registro_frequencia_aluno WHERE registro_frequencia_id = @registroFrequenciaId", 
                new { registroFrequenciaId });
        }

        public async Task<IEnumerable<FiltroMigracaoFrequenciaAulasDto>> ObterTurmasIdFrequenciasExistentesPorAnoAsync(int[] anosLetivos)
        {
            var query = @"  select distinct(t.turma_id) turma_codigo, 
	                               a.id aula_id, 
                                   a.tipo_calendario_id,
	                               a.quantidade quantidade_aula,
	                               a.data_aula,
                                   rfa.registro_frequencia_id
                             from registro_frequencia_aluno rfa 
                            inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id 
                            inner join aula a on a.id = rf.aula_id 
                            inner join turma t on t.turma_id = a.turma_id 
                            where t.ano_letivo = ANY(@anosLetivos)";

            return await database.Conexao.QueryAsync<FiltroMigracaoFrequenciaAulasDto>(query, new { anosLetivos });
        }

        public async Task<IEnumerable<string>> ObterCodigosAlunosComRegistroFrequenciaAlunoAsync(long registroFrequenciaId, string[] codigosAlunos, int numeroAula)
        {
            var query = @"  select distinct(codigo_aluno) 
				              from registro_frequencia_aluno
		  		             where registro_frequencia_id = @registroFrequenciaId
		  		               and numero_aula = @numeroAula
		  		               and codigo_aluno = ANY(@codigosAlunos)";

            var resultado = await database.Conexao.QueryAsync<string>(query, new { registroFrequenciaId, codigosAlunos, numeroAula });
            return resultado;
        }
    }
}
