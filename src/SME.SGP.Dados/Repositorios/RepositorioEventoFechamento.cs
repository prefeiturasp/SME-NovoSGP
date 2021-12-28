using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Globalization;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamento : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamento
    {
        public RepositorioEventoFechamento(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia)
        {
            var query = @"select pe.*
                          from periodo_fechamento_bimestre pfb  
                          join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                         where pfb.inicio_fechamento <= @dataReferencia
                           and pfb.final_fechamento >= @dataReferencia
                        union all
                        select pe.*
                          from fechamento_reabertura fr 
                          left join ue on fr.ue_id = ue.id
                          join fechamento_reabertura_bimestre frb on frb.fechamento_reabertura_id = fr.id
                          join periodo_escolar pe on pe.tipo_calendario_id = fr.tipo_calendario_id and pe.bimestre = frb.bimestre
                         where fr.inicio <= @dataReferencia
                           and fr.fim >= @dataReferencia
                           and (ue.id is null or ue.id = @ueId)";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { ueId, dataReferencia });
        }

        public EventoFechamento ObterPorIdFechamento(long fechamentoId)
        {
            var query = @"select ef.*, e.*
                        from evento_fechamento ef 
                        inner join evento e on e.id = ef.evento_id
                        where ef.fechamento_id = @fechamentoId";

            return database.Conexao.Query<EventoFechamento, Evento, EventoFechamento>(query,
                (eventoFechamento, evento) =>
                {
                    eventoFechamento.Evento = evento;
                    return eventoFechamento;
                }
                , new { fechamentoId }).FirstOrDefault();
        }

        public async Task<bool> SmeEmFechamento(DateTime dataReferencia, long tipoCalendarioId, int bimestre)
        {
            var query = new StringBuilder();
            var consultaObterBimestreFinal = "(select pe2.bimestre from periodo_escolar pe2 where pe.tipo_calendario_id = pe2.tipo_calendario_id order by pe2.bimestre desc limit 1)";

            query.AppendLine(@"select count(pf.id) from periodo_fechamento pf 
                        inner join periodo_fechamento_bimestre pfb on pf.id = pfb.periodo_fechamento_id 
                        inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                        where pe.tipo_calendario_id = @tipoCalendarioId
                        and pf.ue_id is null
                        and pf.dre_id is null
                        and TO_DATE(pfb.inicio_fechamento::TEXT, 'yyyy/mm/dd') <= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
                        and TO_DATE(pfb.final_fechamento::TEXT, 'yyyy/mm/dd') >= TO_DATE(@dataReferencia, 'yyyy/mm/dd')");


            query.AppendLine($"and pe.bimestre =  {(bimestre > 0 ? "@bimestre" : consultaObterBimestreFinal)}");

            return await database.Conexao.QueryFirstAsync<int>(query.ToString(), new
            {
                dataReferencia = dataReferencia.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                bimestre,
                tipoCalendarioId
            }) > 0;
        }

        public async Task<bool> UeEmFechamento(long tipoCalendarioId, bool modalidadeEhInfantil, int bimestre, DateTime dataReferencia)
        {
            var query = new StringBuilder();

            var consultaObterBimestreFinal = "(select pe2.bimestre from periodo_escolar pe2 where pe.tipo_calendario_id = pe2.tipo_calendario_id order by pe2.bimestre desc limit 1)";

            query.AppendLine(@"select count(pf.id) from periodo_fechamento pf 
				inner join periodo_fechamento_bimestre pfb on pf.id = pfb.periodo_fechamento_id 
				inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
				where pe.tipo_calendario_id = @tipoCalendarioId
				and pf.ue_id is null
				and pf.dre_id is null
				and TO_DATE(pfb.inicio_fechamento::TEXT, 'yyyy/mm/dd') <= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
				and TO_DATE(pfb.final_fechamento::TEXT, 'yyyy/mm/dd') >= TO_DATE(@dataReferencia, 'yyyy/mm/dd')");


            query.AppendLine($"and pe.bimestre =  {(bimestre > 0 ? "@bimestre" : consultaObterBimestreFinal)}");

            return await database.Conexao.QueryFirstAsync<int>(query.ToString(), new
            {
                dataReferencia = dataReferencia.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                bimestre,
                tipoCalendarioId
            }) > 0;
        }
    }
}