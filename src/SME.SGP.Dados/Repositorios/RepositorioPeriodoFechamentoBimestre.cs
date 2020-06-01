using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
    }
}
