using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamentoConsulta
        : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamentoConsulta
    {
        public RepositorioEventoFechamentoConsulta(
            ISgpContextConsultas database,
            IServicoAuditoria servicoAuditoria)
            : base(database, servicoAuditoria)
        {
        }

        public Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(
            long ueId,
            DateTime dataReferencia,
            int anoLetivo)
        {
            var query = @"
                select
                    pe.id AS Id,
                    pe.criado_em AS CriadoEm,
                    pe.criado_por AS CriadoPor,
                    pe.alterado_em AS AlteradoEm,
                    pe.alterado_por AS AlteradoPor,
                    pe.alterado_rf AS AlteradoRF,
                    pe.criado_rf AS CriadoRF,
                    pe.bimestre AS Bimestre,
                    pe.migrado AS Migrado,
                    pe.periodo_fim AS PeriodoFim,
                    pe.periodo_inicio AS PeriodoInicio,
                    pe.tipo_calendario_id AS TipoCalendarioId
                from periodo_fechamento_bimestre pfb
                join periodo_escolar pe
                    on pe.id = pfb.periodo_escolar_id
                join tipo_calendario tc
                    on pe.tipo_calendario_id = tc.id
                join periodo_fechamento pf
                    on pf.id = pfb.periodo_fechamento_id
                where pfb.inicio_fechamento <= @dataReferencia
                  and pfb.final_fechamento >= @dataReferencia
                  and tc.ano_letivo = @anoLetivo
                  and COALESCE(pf.aplicacao, 1) = 1

                union all

                select
                    pe.id AS Id,
                    pe.criado_em AS CriadoEm,
                    pe.criado_por AS CriadoPor,
                    pe.alterado_em AS AlteradoEm,
                    pe.alterado_por AS AlteradoPor,
                    pe.alterado_rf AS AlteradoRF,
                    pe.criado_rf AS CriadoRF,
                    pe.bimestre AS Bimestre,
                    pe.migrado AS Migrado,
                    pe.periodo_fim AS PeriodoFim,
                    pe.periodo_inicio AS PeriodoInicio,
                    pe.tipo_calendario_id AS TipoCalendarioId
                from fechamento_reabertura fr
                left join ue
                    on fr.ue_id = ue.id
                join fechamento_reabertura_bimestre frb
                    on frb.fechamento_reabertura_id = fr.id
                join periodo_escolar pe
                    on pe.tipo_calendario_id = fr.tipo_calendario_id
                   and pe.bimestre = frb.bimestre
                join tipo_calendario tc
                    on pe.tipo_calendario_id = tc.id
                where fr.inicio <= @dataReferencia
                  and fr.fim >= @dataReferencia
                  and tc.ano_letivo = @anoLetivo
                  and (ue.id is null or ue.id = @ueId)";

            return database.Conexao.QueryAsync<PeriodoEscolar>(
                query,
                new
                {
                    ueId,
                    dataReferencia,
                    anoLetivo
                });
        }

        public async Task<EventoFechamento> ObterPorIdFechamento(long fechamentoId)
        {
            var query = @"
                select
                    ef.id AS Id,
                    ef.criado_em AS CriadoEm,
                    ef.criado_por AS CriadoPor,
                    ef.alterado_em AS AlteradoEm,
                    ef.alterado_por AS AlteradoPor,
                    ef.alterado_rf AS AlteradoRF,
                    ef.criado_rf AS CriadoRF,
                    ef.evento_id AS EventoId,
                    ef.fechamento_id AS FechamentoId,
                    ef.excluido AS Excluido,

                    e.id AS EventoIdMap,
                    e.id AS Id,
                    e.criado_em AS EventoCriadoEm,
                    e.criado_por AS EventoCriadoPor,
                    e.alterado_em AS EventoAlteradoEm,
                    e.alterado_por AS EventoAlteradoPor,
                    e.alterado_rf AS EventoAlteradoRF,
                    e.criado_rf AS EventoCriadoRF,
                    e.data_fim AS EventoDataFim,
                    e.data_inicio AS EventoDataInicio,
                    e.descricao AS EventoDescricao,
                    e.dre_id AS EventoDreId,
                    e.evento_pai_id AS EventoPaiId,
                    e.excluido AS EventoExcluido,
                    e.feriado_id AS FeriadoId,
                    e.letivo AS Letivo,
                    e.migrado AS Migrado,
                    e.nome AS Nome,
                    e.status AS Status,
                    e.tipo_calendario_id AS TipoCalendarioId,
                    e.tipo_evento_id AS TipoEventoId,
                    e.tipo_perfil_cadastro AS TipoPerfilCadastro,
                    e.ue_id AS UeId,
                    e.wf_aprovacao_id AS WorkflowAprovacaoId
                from evento_fechamento ef
                inner join evento e
                    on e.id = ef.evento_id
                where ef.fechamento_id = @fechamentoId";

            return (await database.Conexao.QueryAsync<EventoFechamento, Evento, EventoFechamento>(
                query,
                (eventoFechamento, evento) =>
                {
                    eventoFechamento.Evento = evento;
                    return eventoFechamento;
                },
                new { fechamentoId },
                splitOn: "EventoIdMap")).FirstOrDefault();
        }

        public async Task<bool> UeEmFechamento(
            DateTime dataReferencia,
            long tipoCalendarioId,
            bool ehModalidadeInfantil,
            int bimestre = 0)
        {
            var retorno = await UeEmFechamentoBimestreVigente(
                dataReferencia,
                tipoCalendarioId,
                ehModalidadeInfantil,
                bimestre);

            return retorno.NaoEhNulo();
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(
            DateTime dataReferencia,
            long tipoCalendarioId,
            bool ehModalidadeInfantil,
            int bimestre)
        {
            return await UeEmFechamentoBimestreVigente(
                dataReferencia,
                tipoCalendarioId,
                ehModalidadeInfantil,
                bimestre);
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestreVigente(
            DateTime dataReferencia,
            long tipoCalendarioId,
            bool ehModalidadeInfantil,
            int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = @"
                (
                    select pe2.bimestre
                    from periodo_escolar pe2
                    where @tipoCalendarioId = pe2.tipo_calendario_id
                    order by pe2.bimestre desc
                    limit 1
                )";

            query.AppendLine(@"
                select
                    pfb.periodo_fechamento_id AS PeriodoFechamentoId,
                    pfb.final_fechamento AS FinalDoFechamento,
                    pfb.inicio_fechamento AS InicioDoFechamento,
                    pfb.periodo_escolar_id AS PeriodoEscolarId
                from periodo_fechamento pf
                inner join periodo_fechamento_bimestre pfb
                    on pf.id = pfb.periodo_fechamento_id
                inner join periodo_escolar pe
                    on pe.id = pfb.periodo_escolar_id
                where pe.tipo_calendario_id = @tipoCalendarioId
                  and pf.ue_id is null
                  and pf.dre_id is null
                  and COALESCE(pf.aplicacao, 1) = 1
                  and to_date(pfb.inicio_fechamento::text, 'yyyy/mm/dd')
                        <= to_date(@dataReferencia, 'yyyy/mm/dd')
                  and to_date(pfb.final_fechamento::text, 'yyyy/mm/dd')
                        >= to_date(@dataReferencia, 'yyyy/mm/dd')");

            if (bimestre > 0)
            {
                query.AppendLine(
                    $"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, ehModalidadeInfantil)}");
            }
            else
            {
                query.AppendLine(
                    $"and pe.bimestre = {consultaObterBimestreFinal}");
            }

            query.AppendLine("order by COALESCE(pf.alterado_em, pf.criado_em) desc, pf.id desc, pfb.id desc");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoBimestre>(
                query.ToString(),
                new
                {
                    dataReferencia = dataReferencia.ToString(
                        "yyyy-MM-dd",
                        DateTimeFormatInfo.InvariantInfo),
                    bimestre,
                    tipoCalendarioId
                });
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestre(
            long tipoCalendarioId,
            bool ehModalidadeInfantil,
            int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = @"
                (
                    select pe2.bimestre
                    from periodo_escolar pe2
                    where @tipoCalendarioId = pe2.tipo_calendario_id
                    order by pe2.bimestre desc
                    limit 1
                )";

            query.AppendLine(@"
                select
                    pfb.periodo_fechamento_id AS PeriodoFechamentoId,
                    pfb.final_fechamento AS FinalDoFechamento,
                    pfb.inicio_fechamento AS InicioDoFechamento,
                    pfb.periodo_escolar_id AS PeriodoEscolarId
                from periodo_fechamento pf
                inner join periodo_fechamento_bimestre pfb
                    on pf.id = pfb.periodo_fechamento_id
                inner join periodo_escolar pe
                    on pe.id = pfb.periodo_escolar_id
                where pe.tipo_calendario_id = @tipoCalendarioId
                  and pf.ue_id is null
                  and pf.dre_id is null
                  and COALESCE(pf.aplicacao, 1) = 1");

            if (bimestre > 0)
            {
                query.AppendLine(
                    $"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, ehModalidadeInfantil)}");
            }
            else
            {
                query.AppendLine(
                    $"and pe.bimestre = {consultaObterBimestreFinal}");
            }

            query.AppendLine("order by COALESCE(pf.alterado_em, pf.criado_em) desc, pf.id desc, pfb.id desc");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoBimestre>(
                query.ToString(),
                new
                {
                    bimestre,
                    tipoCalendarioId
                });
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>>
            ObterPeriodosFechamentoTurmaInfantil(
                long tipoCalendarioId,
                int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = @"
                (
                    select pe2.bimestre
                    from periodo_escolar pe2
                    where @tipoCalendarioId = pe2.tipo_calendario_id
                    order by pe2.bimestre desc
                    limit 1
                )";

            query.AppendLine(@"
                select
                    pfb.periodo_fechamento_id AS PeriodoFechamentoId,
                    pfb.final_fechamento AS FinalDoFechamento,
                    pfb.inicio_fechamento AS InicioDoFechamento,
                    pfb.periodo_escolar_id AS PeriodoEscolarId
                from periodo_fechamento pf
                inner join periodo_fechamento_bimestre pfb
                    on pf.id = pfb.periodo_fechamento_id
                inner join periodo_escolar pe
                    on pe.id = pfb.periodo_escolar_id
                where pe.tipo_calendario_id = @tipoCalendarioId
                  and pf.ue_id is null
                  and pf.dre_id is null
                  and COALESCE(pf.aplicacao, 1) = 1");

            if (bimestre > 0)
            {
                query.AppendLine(
                    $"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, true)}");
            }
            else
            {
                query.AppendLine(
                    $"and pe.bimestre = {consultaObterBimestreFinal}");
            }

            return await database.Conexao.QueryAsync<PeriodoFechamentoBimestre>(
                query.ToString(),
                new
                {
                    bimestre,
                    tipoCalendarioId
                });
        }

        public async Task<IEnumerable<PeriodoEscolar>>
            ObterPeriodoFechamentoEmAbertoTurma(
                string codigoTurma,
                ModalidadeTipoCalendario modalidade,
                DateTime dataReferencia)
        {
            var query = @"
                select distinct
                    periodos.Id AS Id,
                    periodos.CriadoEm AS CriadoEm,
                    periodos.CriadoPor AS CriadoPor,
                    periodos.AlteradoEm AS AlteradoEm,
                    periodos.AlteradoPor AS AlteradoPor,
                    periodos.AlteradoRF AS AlteradoRF,
                    periodos.CriadoRF AS CriadoRF,
                    periodos.Bimestre AS Bimestre,
                    periodos.Migrado AS Migrado,
                    periodos.PeriodoFim AS PeriodoFim,
                    periodos.PeriodoInicio AS PeriodoInicio,
                    periodos.TipoCalendarioId AS TipoCalendarioId
                from
                (
                    select
                        pe.id AS Id,
                        pe.criado_em AS CriadoEm,
                        pe.criado_por AS CriadoPor,
                        pe.alterado_em AS AlteradoEm,
                        pe.alterado_por AS AlteradoPor,
                        pe.alterado_rf AS AlteradoRF,
                        pe.criado_rf AS CriadoRF,
                        pe.bimestre AS Bimestre,
                        pe.migrado AS Migrado,
                        pe.periodo_fim AS PeriodoFim,
                        pe.periodo_inicio AS PeriodoInicio,
                        pe.tipo_calendario_id AS TipoCalendarioId
                    from periodo_fechamento_bimestre pfb
                    join periodo_escolar pe
                        on pe.id = pfb.periodo_escolar_id
                    join tipo_calendario tc
                        on pe.tipo_calendario_id = tc.id
                    join periodo_fechamento pf
                        on pf.id = pfb.periodo_fechamento_id
                    inner join turma t
                        on t.ano_letivo = tc.ano_letivo
                    where pfb.inicio_fechamento <= @dataReferencia
                      and pfb.final_fechamento >= @dataReferencia
                      and t.turma_id = @codigoTurma
                      and tc.modalidade = @modalidade
                      and not tc.excluido
                      and COALESCE(pf.aplicacao, 1) = 1

                    union all

                    select
                        pe.id AS Id,
                        pe.criado_em AS CriadoEm,
                        pe.criado_por AS CriadoPor,
                        pe.alterado_em AS AlteradoEm,
                        pe.alterado_por AS AlteradoPor,
                        pe.alterado_rf AS AlteradoRF,
                        pe.criado_rf AS CriadoRF,
                        pe.bimestre AS Bimestre,
                        pe.migrado AS Migrado,
                        pe.periodo_fim AS PeriodoFim,
                        pe.periodo_inicio AS PeriodoInicio,
                        pe.tipo_calendario_id AS TipoCalendarioId
                    from fechamento_reabertura fr
                    left join ue
                        on fr.ue_id = ue.id
                    join fechamento_reabertura_bimestre frb
                        on frb.fechamento_reabertura_id = fr.id
                    join periodo_escolar pe
                        on pe.tipo_calendario_id = fr.tipo_calendario_id
                       and pe.bimestre = frb.bimestre
                    join tipo_calendario tc
                        on pe.tipo_calendario_id = tc.id
                    inner join turma t
                        on t.ano_letivo = tc.ano_letivo
                    where fr.inicio <= @dataReferencia
                      and fr.fim >= @dataReferencia
                      and t.turma_id = @codigoTurma
                      and tc.modalidade = @modalidade
                      and (ue.id is null or ue.id = t.ue_id)
                      and not tc.excluido
                ) AS periodos";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(
                query,
                new
                {
                    codigoTurma,
                    dataReferencia,
                    modalidade
                });
        }
    }
}
