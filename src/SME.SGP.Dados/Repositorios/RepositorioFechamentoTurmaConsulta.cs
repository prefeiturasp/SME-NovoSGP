using FluentValidation.Results;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaConsulta : RepositorioBase<FechamentoTurma>, IRepositorioFechamentoTurmaConsulta
    {
        public RepositorioFechamentoTurmaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
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

        public async Task<IEnumerable<FechamentoTurma>> ObterPorTurmaBimestreComponenteCurricular(long turmaId, int bimestre, long componenteCurricularId)
        {
            var query = new StringBuilder(@"select f.* from fechamento_turma ft
                    inner join fechamento_turma_disciplina ftd on
                        ft.id = ftd.fechamento_turma_id 
                    left join periodo_escolar p on p.id = f.periodo_escolar_id
                    where ft.turma_id = @turmaId and 
                        ftd.disciplina_id = @componenteCurricularId and 
                        p.bimestre = @bimestre  ");

            return await database.Conexao.QueryAsync<FechamentoTurma>(query.ToString(), new { turmaId, componenteCurricularId, bimestre });
        }

        public async Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0)
        {
            var query = new StringBuilder(@"with fechamentos as (select f.*,
                            row_number() over (partition by f.id, f.turma_id order by f.id desc) sequencia
                            from fechamento_turma f
                          inner join turma t on t.id = f.turma_id
                           left join periodo_escolar p on p.id = f.periodo_escolar_id
                           left join tipo_calendario tp on tp.id = p.tipo_calendario_id and not tp.excluido
                          where t.turma_id = @turmaCodigo");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");
            else
                query.AppendLine(" and f.periodo_escolar_id is null");

            query.AppendLine(" order by f.excluido) select * from fechamentos where sequencia = 1;");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { turmaCodigo, bimestre });
        }

        public async Task<FechamentoTurma> ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestre(string turmaCodigo, int bimestre, int anoLetivoTurma, int? semestre, long? tipoCalendario = null)
        {
            var query = new StringBuilder(@"with fechamentos as (select f.*,
                            row_number() over (partition by f.id, f.turma_id order by f.id desc) sequencia
                            from fechamento_turma f
                          inner join turma t on t.id = f.turma_id
                                left JOIN conselho_classe cc ON cc.fechamento_turma_id  = f.id 
                           left join periodo_escolar p on p.id = f.periodo_escolar_id
                           left join tipo_calendario tp on tp.id = p.tipo_calendario_id and not tp.excluido
                          where t.turma_id = @turmaCodigo ");
            query.AppendLine(bimestre > 0 ? " and p.bimestre = @bimestre " : " and f.periodo_escolar_id is null");
            query.AppendLine(bimestre > 0 && tipoCalendario.HasValue ? " and tp.id =@tipoCalendario" : string.Empty);
            query.AppendLine(" ) select * from fechamentos where sequencia = 1;");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { turmaCodigo, bimestre, tipoCalendario });
        }

        public async Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0)
        {
            var query = new StringBuilder(@"with fechamentos as (select f.*,
                            row_number() over (partition by f.id, f.turma_id order by f.id desc) sequencia
                            from fechamento_turma f
                           where f.turma_id = @turmaId ");
            if (periodoId > 0)
                query.AppendLine(" and f.periodo_escolar_id = @periodoId");
            else
                query.AppendLine(" and f.periodo_escolar_id is null");

            query.AppendLine(" ) select * from fechamentos where sequencia = 1;");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { turmaId, periodoId });
        }

        public async Task<FechamentoTurma> ObterPorFechamentoTurmaIdAsync(long fechamentoTurmaId)
        {
            var query = new StringBuilder(@"select * 
                            from fechamento_turma 
                           where id = @fechamentoTurmaId ");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { fechamentoTurmaId });
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterPorTurmaPeriodoCCAsync(long turmaId, long periodoEscolarId, long componenteCurricularId, bool ehRegencia = false)
        {
            var query = new StringBuilder(@"with lista as (select ftd.*, fa.*, fn.*, 
                                                         row_number() over (partition by t.id, fa.aluno_codigo, p.id, fn.disciplina_id order by fn.id desc) sequencia
                                         from fechamento_turma_disciplina ftd
                                        inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                                        left join periodo_escolar p on p.id = ft.periodo_escolar_id 
                                        inner join turma t on t.id = ft.turma_id
                                        inner join fechamento_aluno fa on ftd.id = fa.fechamento_turma_disciplina_id 
                                        left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id");

            if (!ehRegencia)
                query.Append(" and ftd.disciplina_id = fn.disciplina_id");

            query.AppendLine(@" left join componente_curricular cc on cc.id = fn.disciplina_id
                                where t.id = @turmaId 
                                and (ftd.disciplina_id = @componenteCurricularId or cc.id = @componenteCurricularId)
                                and ft.periodo_escolar_id = @periodoEscolarId                        
                                ORDER BY fn.alterado_em, fn.criado_em) select * from lista where sequencia = 1;");

            IList<FechamentoTurmaDisciplina> fechammentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();

            await database.Conexao.QueryAsync<FechamentoTurmaDisciplina, FechamentoAluno, FechamentoNota, FechamentoTurmaDisciplina>(query.ToString(),
                (fechamentoTurmaDiscplina, fechamentoAluno, fechamentoNota) =>
                {
                    var fechamentoTurmaDisciplinaLista = fechammentosTurmaDisciplina.FirstOrDefault(ftd => ftd.Id == fechamentoTurmaDiscplina.Id);

                    if (fechamentoTurmaDisciplinaLista.EhNulo())
                    {
                        fechamentoTurmaDisciplinaLista = fechamentoTurmaDiscplina;
                        fechammentosTurmaDisciplina.Add(fechamentoTurmaDiscplina);
                    }
                    
                    fechamentoTurmaDisciplinaLista.FechamentoAlunos.Add(fechamentoAluno);

                    fechamentoTurmaDisciplinaLista.AdicionarNota(fechamentoNota);

                    return fechamentoTurmaDiscplina;
                }, new { turmaId, componenteCurricularId, periodoEscolarId });

            return fechammentosTurmaDisciplina;
        }

        public async Task<bool> VerificaExistePorTurmaCCPeriodoEscolar(long turmaId, long componenteCurricularId, long? periodoEscolarId)
        {
            var query = new StringBuilder(@"select 1 from fechamento_turma ft
                    inner join fechamento_turma_disciplina ftd on
                    ft.id = ftd.fechamento_turma_id 
                    where ft.turma_id = @turmaId and 
                        ftd.disciplina_id = @componenteCurricularId and 
                        ft.periodo_escolar_id = @periodoEscolarId  ");
            
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query.ToString(), new { turmaId, componenteCurricularId, periodoEscolarId });
        }

        public Task<FechamentoTurmaPeriodoEscolarDto> ObterIdEPeriodoPorTurmaBimestre(long turmaId, int? bimestre)
        {
            var query = @"select ft.id as FechamentoTurmaId
                            , pe.id as PeriodoEscolarId
                          from fechamento_turma ft
                         left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                        where ft.turma_id = @turmaId
                          and pe.bimestre = @bimestre ";

            return database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurmaPeriodoEscolarDto>(query, new { turmaId, bimestre });
        }

        public async Task<FechamentoTurma> ObterPorTurma(long turmaId, int? bimestre = 0)
        {
            var query = new StringBuilder(@"select ft.*, p.*  
                            from fechamento_turma ft
                            left join periodo_escolar p
                            on p.id = ft.periodo_escolar_id
                           where ft.turma_id = @turmaId and not ft.excluido ");

            if (bimestre > 0)
                query.AppendLine(@" and p.bimestre = @bimestre");
            else
                query.AppendLine(@" and ft.periodo_escolar_id is null");

            var retornoFechamentoTurma = new FechamentoTurma();

            await database.Conexao.QueryAsync<FechamentoTurma, PeriodoEscolar, FechamentoTurma>(query.ToString(),
                (fechamentoTurma, periodoEscolar) =>
                {
                    if (periodoEscolar.NaoEhNulo())
                        fechamentoTurma.AdicionarPeriodoEscolar(periodoEscolar);
                    
                    retornoFechamentoTurma = fechamentoTurma;
                    return fechamentoTurma;
                    
                }, new { turmaId, bimestre });

            return retornoFechamentoTurma;
        }
    }
}
