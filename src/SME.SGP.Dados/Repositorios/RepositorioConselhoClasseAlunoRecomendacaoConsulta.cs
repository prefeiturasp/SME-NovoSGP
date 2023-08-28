using Dapper;
using Npgsql;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoRecomendacaoConsulta : IRepositorioConselhoClasseAlunoRecomendacaoConsulta
    {
        protected readonly ISgpContextConsultas database;
        public RepositorioConselhoClasseAlunoRecomendacaoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> ObterRecomendacoesPorAlunoTurma(string codigoAluno, string codigoTurma, int anoLetivo, int? modalidade, int semestre)
        {
            var query = new StringBuilder(@$"select distinct t.turma_id TurmaCodigo, cca.aluno_codigo AlunoCodigo,
                                 cca.recomendacoes_aluno RecomendacoesAluno, 
                                 cca.recomendacoes_familia RecomendacoesFamilia,
                                 cca.anotacoes_pedagogicas AnotacoesPedagogicas
                                from conselho_classe cc
                                inner join fechamento_turma ft on cc.fechamento_turma_id = ft.id 
                                inner join turma t on ft.turma_id = t.id 
                                inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id
                                {(modalidade.HasValue ? @" inner join tipo_calendario tc on t.ano_letivo = tc.ano_letivo and tc.modalidade = @modalidadeTipoCalendario
                                                           inner join periodo_escolar p on p.tipo_calendario_id = tc.id " : string.Empty)}
                                where 1 = 1");

            if (!string.IsNullOrEmpty(codigoAluno))
                query.AppendLine(" and cca.aluno_codigo = @codigoAluno ");

            if (!string.IsNullOrEmpty(codigoTurma))
                query.AppendLine(" and t.turma_id = @codigoTurma ");

            if (anoLetivo > 0)
                query.AppendLine(" and t.ano_letivo = @anoLetivo ");

            if (modalidade.HasValue)
                query.AppendLine(" and t.modalidade_codigo = @modalidade ");

            DateTime dataReferencia = DateTime.MinValue;
            if (modalidade.HasValue && (Modalidade)modalidade == Modalidade.EJA)
            {
                var periodoReferencia = semestre == 1 ? "periodo_inicio < @dataReferencia" : "periodo_fim > @dataReferencia";
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = tc.id and {periodoReferencia})");

                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
            }
            var parametros = new
            {
                codigoAluno,
                codigoTurma,
                anoLetivo,
                modalidade = modalidade.HasValue ? (int)modalidade : (int)default,
                modalidadeTipoCalendario = modalidade.HasValue ? ((Modalidade)modalidade).ObterModalidadeTipoCalendario() : (int)default,
                dataReferencia
            };

            return await database.Conexao.QueryAsync<RecomendacaoConselhoClasseAlunoDTO>(query.ToString(), parametros);
            
        }

        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesAlunoFamiliaPorAlunoETurma(string codigoAluno, string codigoTurma)
        {
            string sql = @"select distinct ccr.recomendacao, ccr.tipo from conselho_classe_aluno_recomendacao ccar
                                 inner join conselho_classe_recomendacao ccr on ccr.id = ccar.conselho_classe_recomendacao_id
                                 inner join conselho_classe_aluno cca on cca.id = ccar.conselho_classe_aluno_id
                                 inner join conselho_classe cc on cc.id = cca.conselho_classe_id
                                 inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                                 inner join turma t on t.id = ft.turma_id
                                    where t.turma_id = @codigoTurma and cca.aluno_codigo = @codigoAluno";

           return await database.Conexao.QueryAsync<RecomendacoesAlunoFamiliaDto>(sql, new { codigoAluno, codigoTurma });
        }
    }
}
