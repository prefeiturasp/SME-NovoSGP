using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioRegistroFrequenciaAlunoConsulta : IRepositorioRegistroFrequenciaAlunoConsulta
    {
        private readonly ISgpContextConsultas sgpContextConsultas;

        public RepositorioRegistroFrequenciaAlunoConsulta(ISgpContextConsultas sgpContextConsultas)
        {
            this.sgpContextConsultas = sgpContextConsultas ?? throw new ArgumentNullException(nameof(sgpContextConsultas));
        }

        public async Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId)
        {
            var query = @"SELECT rfa.codigo_aluno AS CodigoAluno,
                               rfa.numero_aula AS NumeroAula,
                               rfa.valor AS TipoFrequencia
                        FROM registro_frequencia_aluno rfa
                        WHERE NOT rfa.excluido
                          AND rfa.aula_id = @aulaId";

            return await sgpContextConsultas.Conexao.QueryAsync<FrequenciaAlunoSimplificadoDto>(query, new { aulaId });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAula(long aulaId)
        {
            var query = @"SELECT *
                            FROM registro_frequencia_aluno rfa
                            WHERE NOT rfa.excluido
                              AND rfa.valor = @tipoFrequencia
                              AND rfa.aula_id = @aulaId";

            return sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipoFrequencia = (int)TipoFrequencia.F });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosFrequenciaPorAulaId(long aulaId)
        {
            var query = @"SELECT *
                            FROM registro_frequencia_aluno rfa
                            WHERE NOT rfa.excluido
                              AND rfa.aula_id = @aulaId";

            return sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId});
        }

        public async Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, IEnumerable<string> codigoAlunos, params string[] turmasId)
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
                inner join aula a on
                    rfa.aula_id = a.id
                inner join periodo_escolar p on
                    a.tipo_calendario_id = p.tipo_calendario_id
                where
                    not rfa.excluido
                    and not a.excluido
                    and rfa.codigo_aluno = any(@codigoAlunos)                    
                    and a.turma_id = any(@turmasId)
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

            return await sgpContextConsultas.Conexao.QueryAsync<AusenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmasId, tipoFrequencia = (int)TipoFrequencia.F });
        }
        public async Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano)
        {
            var query = @"           
                        select distinct   
                            a.disciplina_id as DisciplinaId,
                            a.turma_id as TurmaId,
                            p.periodo_fim as DataAula,
                            rfa.codigo_aluno as AlunoCodigo
                        from
                            registro_frequencia_aluno rfa
                            join aula a on rfa.aula_id = a.id
                            join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
                        where
                            not rfa.excluido
                            and not a.excluido
                            and extract(year from p.periodo_inicio) = @ano    
                            and a.data_aula >= p.periodo_inicio
                            and a.data_aula <= p.periodo_fim                    
                            and rfa.numero_aula <= a.quantidade                            
                        order by a.disciplina_id,
                                 a.turma_id,
                                 p.periodo_fim,
                                 rfa.codigo_aluno";

            return await sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>(query, new { ano });
        }

        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<string> codigoAlunos, string professor = null, long[] idsRegistrosFrequenciaDesconsiderados = null)
        {
            var query = @$"           
                select
                    count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 1 and not rfa.excluido) as TotalPresencas,
                    count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 2 and not rfa.excluido) as TotalAusencias,
                    count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 3 and not rfa.excluido) as TotalRemotos,
                    p.id as PeriodoEscolarId,
                    p.periodo_inicio as PeriodoInicio,
                    p.periodo_fim as PeriodoFim,
                    p.bimestre,
                    rfa.codigo_aluno as AlunoCodigo,
                    a.disciplina_id as ComponenteCurricularId
                from registro_frequencia_aluno rfa
                inner join aula a on rfa.aula_id = a.id and not a.excluido                 
                inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
                where
                    not a.excluido
                    and rfa.codigo_aluno = any(@codigoAlunos)                    
                    and a.turma_id = any(@turmasId)
                    and p.periodo_inicio <= @dataAula
                    and p.periodo_fim >= @dataAula
                    and a.data_aula >= p.periodo_inicio
                    and a.data_aula <= p.periodo_fim                    
                    and rfa.numero_aula <= a.quantidade
                    {(!string.IsNullOrWhiteSpace(professor) ? " and a.professor_rf = @professor " : string.Empty)}
                    {(idsRegistrosFrequenciaDesconsiderados != null && idsRegistrosFrequenciaDesconsiderados.Any() ? " and rfa.id <> any(@idsRegistrosFrequenciaDesconsiderados) " : string.Empty)}
                group by
                    p.id,
                    p.periodo_inicio,
                    p.periodo_fim,
                    p.bimestre,
                    rfa.codigo_aluno,
                    a.disciplina_id";

            return await sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, codigoAlunos, turmasId, professor, idsRegistrosFrequenciaDesconsiderados });
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId)
        {
            var query = @"select * 
                          from registro_frequencia_aluno rfa
                          where not rfa.excluido and rfa.valor = @tipo
                            and rfa.aula_id = @aulaId ";

            return sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { aulaId, tipo = (int)TipoFrequencia.F });
        }

        public Task<IEnumerable<FrequenciaAlunoAulaDto>> ObterFrequenciasDoAlunoNaAula(string codigoAluno, long aulaId)
        {
            var query = @"select
                            rfa.id as FrequenciaAlunoId,
                            rfa.valor TipoFrequencia,
                            rfa.numero_aula as NumeroAula,
                            rfa.codigo_aluno as AlunoCodigo 
                        from registro_frequencia_aluno rfa
                            join aula a on rfa.aula_id = a.id
                        where not rfa.excluido and not a.excluido
                            and rfa.codigo_aluno = @codigoAluno and a.id = @aulaId
                            order by rfa.id desc";

            return sgpContextConsultas.Conexao.QueryAsync<FrequenciaAlunoAulaDto>(query, new { aulaId, codigoAluno });
        }

        public async Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string[] disciplinaIdsConsideradas, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null, string professor = null, params string[] turmasId)
        {
            var query = BuildQueryObterTotalAulasPorDisciplinaETurma(disciplinaIdsConsideradas, dataMatriculaAluno, dataSituacaoAluno, professor);
            return await sgpContextConsultas.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(),
                new { dataAula, disciplinaIdsConsideradas, turmasId, dataMatriculaAluno = dataMatriculaAluno.HasValue ? dataMatriculaAluno.Value.Date : (DateTime?)null, dataSituacaoAluno = dataSituacaoAluno.HasValue ? dataSituacaoAluno.Value.Date : (DateTime?)null, professor });
        }

        private string BuildQueryObterTotalAulasPorDisciplinaETurma(string[] disciplinaIdsConsideradas, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null, string professor = null)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select COALESCE(SUM(a.quantidade),0) AS total");
            query.AppendLine("  from aula a ");
            query.AppendLine("      inner join periodo_escolar p");
            query.AppendLine("          on a.tipo_calendario_id = p.tipo_calendario_id");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and @dataAula::date between p.periodo_inicio and p.periodo_fim");
            query.AppendLine("and a.data_aula::date between p.periodo_inicio and p.periodo_fim");

            if (disciplinaIdsConsideradas.NaoEhNulo() && disciplinaIdsConsideradas.Any())
                query.AppendLine("and a.disciplina_id = any(@disciplinaIdsConsideradas)");

            if (dataMatriculaAluno.HasValue && dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date between (@dataMatriculaAluno::date + 1) and @dataSituacaoAluno::date");
            else if (dataMatriculaAluno.HasValue)
                query.AppendLine("and a.data_aula::date > @dataMatriculaAluno::date");
            else if (dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date <= @dataSituacaoAluno::date");

            if (!string.IsNullOrWhiteSpace(professor))
                query.AppendLine("and a.professor_rf = @professor");

            query.AppendLine("and a.turma_id = any(@turmasId)");            
            query.AppendLine("and exists (select 1");
            query.AppendLine("                from registro_frequencia_aluno rfa");
            query.AppendLine("              where a.id = rfa.aula_id and");
            query.AppendLine("                  not a.excluido and");
            query.AppendLine("                  rfa.numero_aula between 1 and a.quantidade);");

            return query.ToString();
        }

        public async Task<IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>> ObterRegistroFrequenciaAlunosPorTurmaEMes(string turmaCodigo, int mes)
        {
            const string query = @"select a.turma_id TurmaId,
                                          count(distinct(rfa.aula_id*rfa.numero_aula)) as QuantidadeAulas,
                                          count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 2) as QuantidadeAusencias,
                                          count(caaa.id) as QuantidadeCompensacoes,
                                          rfa.codigo_aluno as AlunoCodigo,
                                          a.turma_id as TurmaId
                                   from registro_frequencia_aluno rfa
                                       inner join aula a 
                                          on rfa.aula_id = a.id and not a.excluido   
                                       left join compensacao_ausencia_aluno_aula caaa 
                                          on caaa.registro_frequencia_aluno_id = rfa.id and not caaa.excluido
                                   where
                                       a.turma_id = @turmaCodigo
                                       and extract(month from a.data_aula) = @mes                    
                                       and rfa.numero_aula <= a.quantidade
                                       and not rfa.excluido
                                   group by
                                       a.turma_id,
                                       rfa.codigo_aluno;";

            var parametros = new { turmaCodigo, mes };

            return await sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaAlunoPorTurmaEMesDto>(query, parametros);
        }

        public async Task<IEnumerable<FrequenciaAlunoTurmaDto>> ObterRegistroFrequenciaAlunosNaTurma(string turmaCodigo, string alunoCodigo)
        {
            const string query = @"select  a.id as AulaId, rfa.id as RegistroFrequenciaAlunoId, a.data_aula as DataAula, a.disciplina_id as DisciplinaCodigo, rfa.valor as Valor
                                            from registro_frequencia_aluno rfa 
                                            inner join registro_frequencia rf on rf.id  = rfa.registro_frequencia_id 
                                            inner join aula a on a.id = rf.aula_id 
                                            where a.turma_id = @turmaCodigo and rfa.codigo_aluno = @alunoCodigo
                                            and not a.excluido 
                                            order by a.data_aula";

            var parametros = new { turmaCodigo, alunoCodigo };

            return await sgpContextConsultas.Conexao.QueryAsync<FrequenciaAlunoTurmaDto>(query, parametros);
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorIdRegistro(long registroFrequenciaId)
        {
            var query = @"SELECT
                            id,
                            valor,
                            codigo_aluno,
                            numero_aula,
                            registro_frequencia_id,
                            criado_em,
                            criado_por,
                            criado_rf,
                            alterado_em,
                            alterado_por,
                            alterado_rf,
                            excluido,
                            migrado,
                            aula_id
                        FROM
                            registro_frequencia_aluno
                        where not excluido 
                            and registro_frequencia_id = @registroFrequenciaId";
            return sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaAluno>(query, new { registroFrequenciaId });
        }
        
        public async Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<(string codigo, DateTime dataMatricula, DateTime? dataSituacao)> alunos, bool somenteAusencias = false)
        {
            var query = "with lista1 as (";
            var listaAlunos = alunos.ToList();

            for (int i = 0; i < listaAlunos.Count; i++)
            {
                query += $@"
                    select a.id aula_id,
                           pe.id periodo_id,
                             pe.periodo_inicio,
                             pe.periodo_fim,
                             pe.bimestre,
                             coalesce(rfa.codigo_aluno, '{listaAlunos[i].codigo}') codigo_aluno,
                             a.disciplina_id, 
                             coalesce(rfa.valor, 1) valor,      
                             coalesce(rfa.criado_em, a.criado_em) criado_em,
                           coalesce(rfa.numero_aula, 1) numero_aula,
                           coalesce(rfa.id, 0) registro_frequencia_aluno_id                           
                          from aula a
                              inner join periodo_escolar pe
                                  on a.tipo_calendario_id = pe.tipo_calendario_id
                              left join registro_frequencia_aluno rfa 
                                  on a.id = rfa.aula_id and
                                   not rfa.excluido and
                                   rfa.codigo_aluno = '{listaAlunos[i].codigo}'
                    where not a.excluido and                            
                          a.turma_id = any(@turmasId) and
                          @dataAula::date between pe.periodo_inicio and pe.periodo_fim and
                          a.data_aula::date between pe.periodo_inicio and pe.periodo_fim and
                          a.data_aula::date > '{listaAlunos[i].dataMatricula:yyyy-MM-dd}'::date
                          {(listaAlunos[i].dataSituacao.HasValue ? $" and a.data_aula::date <= '{listaAlunos[i].dataSituacao.Value:yyyy-MM-dd}'::date" : string.Empty)}";

                query += i + 1 == listaAlunos.Count ? string.Empty : " union ";
            }
            
            query += $@"), lista2 as (
                    select *,
                           row_number() over (partition by aula_id, codigo_aluno, numero_aula order by registro_frequencia_aluno_id desc) sequencia
                    from lista1)
                    select {(somenteAusencias ? string.Empty : "count(distinct (tmp.aula_id, tmp.numero_aula)) filter (where tmp.valor = 1) TotalPresencas,")}
                           count(distinct (tmp.aula_id, tmp.numero_aula)) filter (where tmp.valor = 2) TotalAusencias,
                           {(somenteAusencias ? string.Empty : "count(distinct (tmp.aula_id, tmp.numero_aula)) filter (where tmp.valor = 3) TotalRemotos,")}
                           tmp.periodo_id as PeriodoEscolarId,
                           tmp.periodo_inicio as PeriodoInicio,
                           tmp.periodo_fim as PeriodoFim,
                           tmp.bimestre,
                           tmp.codigo_aluno as AlunoCodigo,
                           tmp.disciplina_id as ComponenteCurricularId
                        from lista2 tmp
                    where tmp.sequencia = 1                                   
                    group by tmp.periodo_id,
                             tmp.periodo_inicio,
                             tmp.periodo_fim,
                             tmp.bimestre,
                             tmp.codigo_aluno,
                             tmp.disciplina_id;";

            return await sgpContextConsultas.Conexao.QueryAsync<RegistroFrequenciaPorDisciplinaAlunoDto>(query, new { dataAula, turmasId }, commandTimeout: 120);
        }

        public async Task<int> ObterTotalAulasPorDisciplinaTurmaAluno(DateTime dataAula, string codigoAluno, string disciplinaId, params string[] turmasId)
        {
            var query = $@"
                            with qdadeAulasAluno as (
                                select coalesce(a.quantidade, 0) as qdade
                                              from aula a 
                                                  inner join periodo_escolar p
                                                      on a.tipo_calendario_id = p.tipo_calendario_id
                                                  inner join registro_frequencia_aluno rfa on rfa.aula_id = a.id and not rfa.excluido 
                                                  where not a.excluido
                                                  and rfa.codigo_aluno = @codigoAluno  
                                                  and @dataAula::date between p.periodo_inicio and p.periodo_fim
                                                  and a.data_aula::date between p.periodo_inicio and p.periodo_fim
                                                  and a.turma_id = any(@turmasId)
                                                  {(!string.IsNullOrWhiteSpace(disciplinaId) ? " and a.disciplina_id = @disciplinaId" : String.Empty)}
                                                  group by a.id)
                            select coalesce(sum(qdade), 0) from qdadeAulasAluno;";

            
            return await sgpContextConsultas.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(),
                new { dataAula, disciplinaId, turmasId, codigoAluno });
        }

        public async Task<RegistroFrequenciaAlunoPorTurmaEMesDto> ObterRegistroFrequenciaAlunoPorTurmaMesDataRef(string turmaCodigo, string alunoCodigo, DateTime dataRef, int mes = 0)
        {
            string query = @$"select a.turma_id TurmaId,
                                          count(distinct(rfa.aula_id*rfa.numero_aula)) as QuantidadeAulas,
                                          count(distinct(rfa.aula_id*rfa.numero_aula)) filter (where rfa.valor = 2) as QuantidadeAusencias,
                                          count(caaa.id) as QuantidadeCompensacoes,
                                          rfa.codigo_aluno as AlunoCodigo,
                                          a.turma_id as TurmaId
                                   from registro_frequencia_aluno rfa
                                       inner join aula a 
                                          on rfa.aula_id = a.id and not a.excluido   
                                       left join compensacao_ausencia_aluno_aula caaa 
                                          on caaa.registro_frequencia_aluno_id = rfa.id and not caaa.excluido
                                   where
                                       a.turma_id = @turmaCodigo
                                       and rfa.codigo_aluno = @alunoCodigo                   
                                       and rfa.numero_aula <= a.quantidade
                                       AND a.data_aula::date <= @dataRef
                                       and not rfa.excluido
                                       {(mes > 0 ? "and extract(month from a.data_aula) = @mes" : string.Empty)}
                                   group by
                                       a.turma_id,
                                       rfa.codigo_aluno;";

            var parametros = new { turmaCodigo, mes, alunoCodigo, dataRef };

            return await sgpContextConsultas.Conexao.QueryFirstOrDefaultAsync<RegistroFrequenciaAlunoPorTurmaEMesDto>(query, parametros);
        }

    }
}
