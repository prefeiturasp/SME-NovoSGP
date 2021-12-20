using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroFrequenciaAlunoConsulta : IRepositorioRegistroFrequenciaAlunoConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioRegistroFrequenciaAlunoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId)
        {
            var query = @"select 
                        rfa.codigo_aluno as CodigoAluno,
                        rfa.numero_aula as NumeroAula,
                        rfa.valor as TipoFrequencia
                        from registro_frequencia rf 
                        inner join registro_frequencia_aluno rfa on rf.id = rfa.registro_frequencia_id 
                        where not rf.excluido
                          and not rfa.excluido
                          and rf.aula_id = @aulaId";

            return await database.Conexao.QueryAsync<FrequenciaAlunoSimplificadoDto>(query, new { aulaId });
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> codigoAlunos)
        {
            var query = @"           
                    select
	                count(distinct(rfa.registro_frequencia_id*rfa.numero_aula)) as TotalAusencias,
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
                    and rfa.valor = @tipoFrequencia
                    and rfa.numero_aula <= a.quantidade 
                group by
	                p.id,
	                p.periodo_inicio,
	                p.periodo_fim,
	                p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await database.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmaId, tipoFrequencia = (int)TipoFrequencia.F });
        }
    }
}
