using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoFechamentoBimestre: IRepositorioPeriodoFechamentoBimestre
    {
        protected readonly ISgpContext database;

        public RepositorioPeriodoFechamentoBimestre(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> SalvarAsync(PeriodoFechamentoBimestre entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));

            return entidade.Id;
        }


        public async Task<IEnumerable<PeriodoFechamentoBimestre>> ObterBimestreParaAlteracaoHierarquicaAsync(long periodoEscolarId, long? dreId, DateTime inicioDoFechamento, DateTime finalDoFechamento)
        {
            var queryDre = dreId.HasValue ? "and p.dre_id = @dreId and p.ue_id is not null" : "";

            var query = $@"select b.*, e.*, p.*, t.*
                  from periodo_fechamento p 
                inner join periodo_fechamento_bimestre b on b.periodo_fechamento_id = p.Id
                inner join periodo_escolar e on e.id = b.periodo_escolar_id
                inner join tipo_calendario t on t.id = e.tipo_calendario_id
                where (inicio_fechamento < @inicioDoFechamento or final_fechamento > @finalDoFechamento)
                  and periodo_escolar_id = @periodoEscolarId
                  {queryDre}";

            return await database.Conexao.QueryAsync<PeriodoFechamentoBimestre, PeriodoEscolar, PeriodoFechamento, TipoCalendario, PeriodoFechamentoBimestre>(query,
                (periodoFechamentoBimestre, periodoEscolar, periodoFechamento, tipoCalendario) =>
                {
                    periodoEscolar.TipoCalendario = tipoCalendario;
                    periodoFechamentoBimestre.PeriodoEscolar = periodoEscolar;
                    periodoFechamentoBimestre.PeriodoFechamento = periodoFechamento;
                    return periodoFechamentoBimestre;
                }
                , new { periodoEscolarId, dreId, inicioDoFechamento, finalDoFechamento });
        }

        public async Task<PeriodoFechamentoBimestre> ObterPeridoFechamentoBimestrePorDreUeEData(ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataInicio, int bimestre, long? dreId, long? ueId)
        {
            var filtroDre = dreId.HasValue ? "p.dre_id = @dreId" : "p.dre_id is null";
            var filtroUe = ueId.HasValue ? "p.ue_id = @ueId" : "p.ue_id is null";

            var query = $@"select b.*, p.*, e.*
                      from periodo_fechamento p 
                     inner join periodo_fechamento_bimestre b on b.periodo_fechamento_id = p.Id
                     inner join periodo_escolar e on e.id = b.periodo_escolar_id
                     inner join tipo_calendario t on t.id = e.tipo_calendario_id
                     where not t.excluido
                       and e.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre, modalidadeTipoCalendario == ModalidadeTipoCalendario.Infantil)}
                       and t.modalidade = @modalidade
                       and b.inicio_fechamento = @dataInicio 
                       and {filtroDre} 
                       and {filtroUe}";

            return (await database.Conexao.QueryAsync<PeriodoFechamentoBimestre, PeriodoFechamento, PeriodoEscolar, PeriodoFechamentoBimestre>(query, 
                (periodoFechamentoBimestre, periodoFechamento, periodoEscolar) =>
                {
                    periodoFechamentoBimestre.PeriodoFechamento = periodoFechamento;
                    periodoFechamentoBimestre.PeriodoEscolar = periodoEscolar;

                    return periodoFechamentoBimestre;
                }, new { modalidade = (int)modalidadeTipoCalendario, dataInicio, bimestre, dreId, ueId })).FirstOrDefault();
        }

        public async Task<bool> ExistePeriodoFechamentoPorDataPeriodoEscolar(long periodoEscolarId, DateTime dataReferencia)
        {
            var query = @"select 1
                          from periodo_fechamento p 
                         inner join periodo_fechamento_bimestre b on b.periodo_fechamento_id = p.Id
                        where b.periodo_escolar_id = @periodoEscolarId 
                            and TO_DATE(fr.inicio::TEXT, 'yyyy/mm/dd') <= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
                            and TO_DATE(fr.fim::TEXT, 'yyyy/mm/dd') >= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
                            and fr.status = 1 
                        ";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { periodoEscolarId, dataReferencia });
        }
    }
}
