using Dapper;
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
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamentoConsulta : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamentoConsulta
    {
        public RepositorioEventoFechamentoConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia, int anoLetivo)
        {
            var query = @"select pe.*
                          from periodo_fechamento_bimestre pfb  
                          join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                          join tipo_calendario tc on pe.tipo_calendario_id = tc.id
                         where pfb.inicio_fechamento <= @dataReferencia
                           and pfb.final_fechamento >= @dataReferencia
                           and tc.ano_letivo = @anoLetivo
                        union all
                        select pe.*
                          from fechamento_reabertura fr 
                          left join ue on fr.ue_id = ue.id
                          join fechamento_reabertura_bimestre frb on frb.fechamento_reabertura_id = fr.id
                          join periodo_escolar pe on pe.tipo_calendario_id = fr.tipo_calendario_id and pe.bimestre = frb.bimestre
                          join tipo_calendario tc on pe.tipo_calendario_id = tc.id
                         where fr.inicio <= @dataReferencia
                           and fr.fim >= @dataReferencia
                           and tc.ano_letivo = @anoLetivo
                           and (ue.id is null or ue.id = @ueId)";

            return database.Conexao.QueryAsync<PeriodoEscolar>(query, new { ueId, dataReferencia, anoLetivo });
        }

        public async Task<EventoFechamento> ObterPorIdFechamento(long fechamentoId)
        {
            var query = @"select ef.*, e.*
                        from evento_fechamento ef 
                        inner join evento e on e.id = ef.evento_id
                        where ef.fechamento_id = @fechamentoId";

            return (await database.Conexao.QueryAsync<EventoFechamento, Evento, EventoFechamento>(query,
                (eventoFechamento, evento) =>
                {
                    eventoFechamento.Evento = evento;
                    return eventoFechamento;
                }
                , new { fechamentoId })).FirstOrDefault();
        }

        public async Task<bool> UeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
        {
            var retorno = (await UeEmFechamentoBimestreVigente(dataReferencia,tipoCalendarioId, ehModalidadeInfantil, bimestre));
            return retorno.NaoEhNulo();
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
        {
            return await UeEmFechamentoBimestreVigente(dataReferencia, tipoCalendarioId, ehModalidadeInfantil, bimestre);
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestreVigente(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = "(select pe2.bimestre from periodo_escolar pe2 where @tipoCalendarioId = pe2.tipo_calendario_id order by pe2.bimestre desc limit 1)";

            query.AppendLine(@"select pfb.* from periodo_fechamento pf 
				inner join periodo_fechamento_bimestre pfb on pf.id = pfb.periodo_fechamento_id 
				inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
				where pe.tipo_calendario_id = @tipoCalendarioId
				and pf.ue_id is null
				and pf.dre_id is null
				and TO_DATE(pfb.inicio_fechamento::TEXT, 'yyyy/mm/dd') <= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
				and TO_DATE(pfb.final_fechamento::TEXT, 'yyyy/mm/dd') >= TO_DATE(@dataReferencia, 'yyyy/mm/dd')");

            if (bimestre > 0)
                query.AppendLine($"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, ehModalidadeInfantil)}");

            else
                query.AppendLine($"and pe.bimestre =  {consultaObterBimestreFinal}");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoBimestre>(query.ToString(), new
            {
                dataReferencia = dataReferencia.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                bimestre,
                tipoCalendarioId
            });
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestre(long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = "(select pe2.bimestre from periodo_escolar pe2 where @tipoCalendarioId = pe2.tipo_calendario_id order by pe2.bimestre desc limit 1)";

            query.AppendLine(@"select pfb.* from periodo_fechamento pf 
				inner join periodo_fechamento_bimestre pfb on pf.id = pfb.periodo_fechamento_id 
				inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
				where pe.tipo_calendario_id = @tipoCalendarioId
				and pf.ue_id is null
				and pf.dre_id is null ");

            if (bimestre > 0)
                query.AppendLine($"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, ehModalidadeInfantil)} ");

            else
                query.AppendLine($" and pe.bimestre =  {consultaObterBimestreFinal} ");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoBimestre>(query.ToString(), new
            {
                bimestre,
                tipoCalendarioId
            });
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoTurmaInfantil(long tipoCalendarioId, int bimestre)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = "(select pe2.bimestre from periodo_escolar pe2 where @tipoCalendarioId = pe2.tipo_calendario_id order by pe2.bimestre desc limit 1)";

            query.AppendLine(@"select pfb.* 
                                 from periodo_fechamento pf 
				                inner join periodo_fechamento_bimestre pfb on pf.id = pfb.periodo_fechamento_id 
				                inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
				                where pe.tipo_calendario_id = @tipoCalendarioId
				                  and pf.ue_id is null
				                  and pf.dre_id is null");

            if (bimestre > 0)
                query.AppendLine($"and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, true)}");
            else
                query.AppendLine($"and pe.bimestre =  {consultaObterBimestreFinal}");

            var parametros = new
            {
                bimestre,
                tipoCalendarioId
            };

            return await database.Conexao.QueryAsync<PeriodoFechamentoBimestre>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodoFechamentoEmAbertoTurma(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = @"  select distinct  * from (     
                         select pe.id, pe.tipo_calendario_id, pe.bimestre, pe.periodo_inicio, pe.periodo_fim
                          from periodo_fechamento_bimestre pfb  
                          join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                          join tipo_calendario tc on pe.tipo_calendario_id = tc.id
                          inner join turma t on t.ano_letivo = tc.ano_letivo 
                         where pfb.inicio_fechamento <= @dataReferencia
                           and pfb.final_fechamento >= @dataReferencia
                           and turma_id = @codigoTurma
                           and tc.modalidade = @modalidade
                           and not tc.excluido
                        union all
                        select pe.id, pe.tipo_calendario_id, pe.bimestre, pe.periodo_inicio, pe.periodo_fim
                          from fechamento_reabertura fr 
                          left join ue on fr.ue_id = ue.id
                          join fechamento_reabertura_bimestre frb on frb.fechamento_reabertura_id = fr.id
                          join periodo_escolar pe on pe.tipo_calendario_id = fr.tipo_calendario_id and pe.bimestre = frb.bimestre
                          join tipo_calendario tc on pe.tipo_calendario_id = tc.id
                          inner join turma t on t.ano_letivo = tc.ano_letivo 
                         where fr.inicio <= @dataReferencia
                           and fr.fim >= @dataReferencia
                           and turma_id = @codigoTurma
                           and tc.modalidade = @modalidade
                           and (ue.id is null or ue.id = t.ue_id)
                           and not tc.excluido
                           ) as periodos";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { codigoTurma, dataReferencia, modalidade });
        }
    }
}
