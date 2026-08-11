using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
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
            var query = @"
                select
                      -- FechamentoTurma
                    f.id                    as FechamentoTurmaId,
                    f.id,
                    f.periodo_escolar_id,
                    f.turma_id              as TurmaId,
                    f.migrado               as Migrado,
                    f.excluido              as Excluido,
                    f.criado_em             as CriadoEm,
                    f.criado_por            as CriadoPor,
                    f.alterado_em           as AlteradoEm,
                    f.alterado_por          as AlteradoPor,
                    f.alterado_rf           as AlteradoRF,
                    f.criado_rf             as CriadoRF,
                    -- Turma 
                    t.id                    as TurmaId,
                    t.id                    ,
                    t.ano,
                    t.ano_letivo,
                    t.turma_id              as CodigoTurma,
                    t.tipo_turma,
                    t.data_atualizacao,
                    t.modalidade_codigo,
                    t.nome,
                    t.qt_duracao_aula     as QuantidadeDuracaoAula,
                    t.semestre,
                    t.tipo_turno,
                    t.serie_ensino,
                    t.ue_id,
                    t.nome_filtro,
                    t.historica,
                    t.ensino_especial,
                    t.data_inicio,
                    t.dt_fim_eol            as DataFim,
                    t.etapa_eja,
                    -- Ue
                    ue.id                   as UeId,
                    ue.id                   ,
                    ue.ue_id                as CodigoUe,
                    ue.data_atualizacao,
                    ue.dre_id,
                    ue.nome,
                    ue.tipo_escola,
                    -- Dre
                    dre.id                  as DreId,
                    dre.id                 ,
                    dre.abreviacao,
                    dre.dre_id               as CodigoDre,
                    dre.data_atualizacao,
                    dre.nome,
                    -- PeriodoEscolar
                    pe.id                   as PeriodoEscolarId,
                    pe.id                  ,
                    pe.criado_em            as CriadoEm,
                    pe.criado_por           as CriadoPor,
                    pe.alterado_em          as AlteradoEm,
                    pe.alterado_por         as AlteradoPor,
                    pe.alterado_rf          as AlteradoRF,
                    pe.criado_rf            as CriadoRF,
                    pe.bimestre,
                    pe.migrado              as Migrado,
                    pe.periodo_fim,
                    pe.periodo_inicio,
                    pe.tipo_calendario_id
                from fechamento_turma f
                inner join turma t
                    on t.id = f.turma_id
                inner join ue
                    on ue.id = t.ue_id
                inner join dre
                    on dre.id = ue.dre_id
                left join periodo_escolar pe
                    on pe.id = f.periodo_escolar_id
                where f.id = @fechamentoTurmaId";

            return (await database.Conexao.QueryAsync<
                FechamentoTurma,
                Turma,
                Ue,
                Dre,
                PeriodoEscolar,
                FechamentoTurma>(
                query,
                (fechamentoTurma, turma, ue, dre, periodoEscolar) =>
                {
                    if (ue != null)
                        ue.AdicionarDre(dre);

                    if (turma != null)
                        turma.Ue = ue;

                    fechamentoTurma.Turma = turma;
                    fechamentoTurma.PeriodoEscolar = periodoEscolar;

                    return fechamentoTurma;
                },
                new { fechamentoTurmaId },
                splitOn: "FechamentoTurmaId,TurmaId,UeId,DreId,PeriodoEscolarId"
            )).FirstOrDefault();
        }

        public async Task<IEnumerable<FechamentoTurma>>ObterPorTurmaBimestreComponenteCurricular(
                long turmaId,
                int bimestre,
                long componenteCurricularId)
        {
            var query = @"
                select
                    -- FechamentoTurma
                    ft.id                    as FechamentoTurmaId,
                    ft.id,
                    ft.periodo_escolar_id,
                    ft.turma_id              as TurmaId,
                    ft.migrado               as Migrado,
                    ft.excluido              as Excluido,
                    ft.criado_em             as CriadoEm,
                    ft.criado_por            as CriadoPor,
                    ft.alterado_em           as AlteradoEm,
                    ft.alterado_por          as AlteradoPor,
                    ft.alterado_rf           as AlteradoRF,
                    ft.criado_rf             as CriadoRF,
                    -- PeriodoEscolar
                    p.id                     as PeriodoEscolarId,
                    p.id                     as Id,
                    p.criado_em              as CriadoEm,
                    p.criado_por            as CriadoPor,
                    p.alterado_em            as AlteradoEm,
                    p.alterado_por           as AlteradoPor,
                    p.alterado_rf            as AlteradoRF,
                    p.criado_rf              as CriadoRF,
                    p.bimestre,
                    p.migrado                as Migrado,
                    p.periodo_fim,
                    p.periodo_inicio,
                    p.tipo_calendario_id
                from fechamento_turma ft
                inner join fechamento_turma_disciplina ftd
                    on ft.id = ftd.fechamento_turma_id
                left join periodo_escolar p
                    on p.id = ft.periodo_escolar_id
                where ft.turma_id = @turmaId
                  and ftd.disciplina_id = @componenteCurricularId
                  and p.bimestre = @bimestre";

            return await database.Conexao.QueryAsync<
                FechamentoTurma,
                PeriodoEscolar,
                FechamentoTurma>(
                query,
                (fechamentoTurma, periodoEscolar) =>
                {
                    fechamentoTurma.PeriodoEscolar = periodoEscolar;
                    return fechamentoTurma;
                },
                new
                {
                    turmaId,
                    componenteCurricularId,
                    bimestre
                },
                splitOn: "FechamentoTurmaId,PeriodoEscolarId");
        }

            public async Task<FechamentoTurma>
                ObterPorTurmaCodigoBimestreAsync(
                    string turmaCodigo,
                    int bimestre = 0)
            {
                var query = new StringBuilder(@"
                    with fechamentos as
                    (
                        select
                            -- FechamentoTurma
                            f.id                    as FechamentoTurmaId,
                            f.id                    as Id,
                            f.criado_em             as CriadoEm,
                            f.criado_por            as CriadoPor,
                            f.alterado_em           as AlteradoEm,
                            f.alterado_por          as AlteradoPor,
                            f.alterado_rf           as AlteradoRF,
                            f.criado_rf             as CriadoRF,
                            f.periodo_escolar_id,
                            f.turma_id,
                            f.migrado               as Migrado,
                            f.excluido              as Excluido,
                            -- PeriodoEscolar
                            p.id                     as PeriodoEscolarId,
                            p.id                     as Id,
                            p.criado_em              as CriadoEm,
                            p.criado_por            as CriadoPor,
                            p.alterado_em           as AlteradoEm,
                            p.alterado_por          as AlteradoPor,
                            p.alterado_rf            as AlteradoRF,
                            p.criado_rf             as CriadoRF,
                            p.bimestre,
                            p.migrado                as Migrado,
                            p.periodo_fim,
                            p.periodo_inicio,
                            p.tipo_calendario_id,
                            -- Turma
                            t.id                    as TurmaId,
                            t.id                    as Id,
                            t.ano,
                            t.ano_letivo,
                            t.turma_id              as CodigoTurma,
                            t.tipo_turma,
                            t.data_atualizacao      as DataAtualizacao,
                            t.modalidade_codigo,
                            t.nome                   as Nome,
                            t.qt_duracao_aula,
                            t.semestre,
                            t.tipo_turno,
                            t.serie_ensino,
                            t.ue_id,
                            t.nome_filtro,
                            t.historica,
                            t.ensino_especial,
                            t.data_inicio,
                            t.dt_fim_eol            as DataFim,
                            t.etapa_eja,
                            -- TipoCalendario
                            tp.id                   as TipoCalendarioId,
                            tp.id                   as Id,
                            tp.criado_em            as CriadoEm,
                            tp.criado_por           as CriadoPor,
                            tp.alterado_em          as AlteradoEm,
                            tp.alterado_por         as AlteradoPor,
                            tp.alterado_rf          as AlteradoRF,
                            tp.criado_rf            as CriadoRF,
                            tp.ano_letivo            as AnoLetivo,
                            tp.excluido              as Excluido,
                            tp.migrado               as Migrado,
                            tp.modalidade,
                            tp.nome                  as Nome,
                            tp.periodo,
                            tp.situacao,
                            tp.semestre              as Semestre,
                            row_number() over (
                                partition by f.id, f.turma_id
                                order by f.id desc
                            ) as sequencia
                        from fechamento_turma f
                        inner join turma t
                            on t.id = f.turma_id
                        left join periodo_escolar p
                            on p.id = f.periodo_escolar_id
                        left join tipo_calendario tp
                            on tp.id = p.tipo_calendario_id
                           and not tp.excluido
                        where t.turma_id = @turmaCodigo
                ");

                if (bimestre > 0)
                    query.AppendLine("and p.bimestre = @bimestre");
                else
                    query.AppendLine("and f.periodo_escolar_id is null");

                query.AppendLine(@"
                        order by f.excluido
                    )
                    select *
                    from fechamentos
                    where sequencia = 1;");

                return (await database.Conexao.QueryAsync<
                    FechamentoTurma,
                    PeriodoEscolar,
                    Turma,
                    TipoCalendario,
                    FechamentoTurma>(
                    query.ToString(),
                    (fechamentoTurma, periodoEscolar, turma, tipoCalendario) =>
                    {
                        if (periodoEscolar != null)
                            periodoEscolar.TipoCalendario = tipoCalendario;

                        fechamentoTurma.PeriodoEscolar = periodoEscolar;
                        fechamentoTurma.Turma = turma;

                        return fechamentoTurma;
                    },
                    new
                    {
                        turmaCodigo,
                        bimestre
                    },
                    splitOn: "PeriodoEscolarId,TurmaId,TipoCalendarioId"
                )).FirstOrDefault();
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
