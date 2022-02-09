using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoFechamentoConsulta : RepositorioBase<EventoFechamento>, IRepositorioEventoFechamentoConsulta
    {
        public RepositorioEventoFechamentoConsulta(ISgpContextConsultas database) : base(database)
        {
        }

        public Task<IEnumerable<PeriodoEscolar>> ObterPeriodosFechamentoEmAberto(long ueId, DateTime dataReferencia)
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

            return database.Conexao.QueryAsync<PeriodoEscolar>(query, new { ueId, dataReferencia });
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
            var retorno = (await UeEmFechamentoBimestre(dataReferencia, tipoCalendarioId, ehModalidadeInfantil, bimestre));
            return retorno != null;
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoVigente(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
        {
            return await UeEmFechamentoBimestre(dataReferencia, tipoCalendarioId, ehModalidadeInfantil, bimestre);
        }

        public async Task<PeriodoFechamentoBimestre> UeEmFechamentoBimestre(DateTime dataReferencia, long tipoCalendarioId, bool ehModalidadeInfantil, int bimestre)
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
    }
}
