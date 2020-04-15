using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAluno>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database) : base(database)
        {
        }

        public FrequenciaAluno Obter(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim, TipoFrequenciaAluno tipoFrequencia)
        {
            var query = @"select
	                        *
                        from
	                        frequencia_aluno
                        where
	                        codigo_aluno = @codigoAluno
	                        and disciplina_id = @disciplinaId
	                        and tipo = @tipoFrequencia
	                        and periodo_inicio = @periodoInicio
	                        and periodo_fim = @periodoFim";
            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                periodoInicio,
                periodoFim,
                tipoFrequencia
            });
        }

        public IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId)
        {
            var query = @"select f.* 
                          from frequencia_aluno f
                         inner join periodo_escolar p on p.periodo_fim = f.periodo_fim and p.periodo_inicio = f.periodo_inicio
                        where not f.excluido
                          and p.id = @periodoId
                          and f.tipo = 1
                          and f.total_ausencias - f.total_compensacoes > 0 ";

            return database.Conexao.Query<FrequenciaAluno>(query, new { periodoId });
        }

        public IEnumerable<AlunoFaltosoBimestreDto> ObterAlunosFaltososBimestre(bool modalidadeEJA, double percentualFrequenciaMinimo, int bimestre, int anoLetivo)
        {
            var query = new StringBuilder();

            query.AppendLine("select dre.dre_id as DreCodigo, dre.Nome as DreNome, ue.tipo_escola as TipoEscola, ue.ue_id as UeCodigo, ue.nome as UeNome");
            query.AppendLine(", t.turma_id as TurmaCodigo, t.nome as TurmaNome, fa.codigo_aluno as AlunoCodigo");
            query.AppendLine(", ((fa.total_ausencias::numeric - fa.total_compensacoes::numeric ) / fa.total_aulas::numeric)*100 PercentualFaltas");
            query.AppendLine("from frequencia_aluno fa");
            query.AppendLine("inner join turma t on t.turma_id = fa.turma_id");
            query.AppendLine("inner join ue on ue.id = t.ue_id ");
            query.AppendLine("inner join dre on dre.id = ue.dre_id");
            query.AppendLine("inner join (select codigo_aluno, max(id) as maiorId from frequencia_aluno where not excluido group by codigo_aluno ) m on m.codigo_aluno = fa.codigo_aluno and m.maiorId = fa.id");
            query.AppendLine("where fa.tipo = 2");
            query.AppendLine("and fa.bimestre = @bimestre");
            query.AppendLine("and extract(year from fa.periodo_inicio) = @anoLetivo");
            query.AppendLine("and ((fa.total_ausencias::numeric - fa.total_compensacoes::numeric ) / fa.total_aulas::numeric) > (1 -(@percentualFrequenciaMinimo::numeric / 100::numeric)) ");

            if (modalidadeEJA)
                query.AppendLine("and t.modalidade_codigo = 3");
            else query.AppendLine("and t.modalidade_codigo in (5,6)");


            return database.Conexao.Query<AlunoFaltosoBimestreDto>(query.ToString(), new { bimestre, anoLetivo, percentualFrequenciaMinimo });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAluno(string alunoCodigo)
        {
            var query = @"select * 
                            from frequencia_aluno
                           where tipo = 2
	                        and codigo_aluno = @alunoCodigo";

            return await database.Conexao.QueryAsync<FrequenciaAluno>(query, new { alunoCodigo });
        }

        public FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "")
        {
            var query = new StringBuilder(@"select *
                        from frequencia_aluno
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia
	                        and periodo_inicio <= @dataAtual
	                        and periodo_fim >= @dataAtual ");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            return database.QueryFirstOrDefault<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId
            });
        }
        
        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual)
        {
            var query = @"select *
                        from frequencia_aluno
                        where codigo_aluno = @codigoAluno
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and periodo_inicio <= @dataAtual
	                        and periodo_fim >= @dataAtual";

            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                dataAtual,
            });
        }
    }
}