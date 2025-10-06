using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSondagemEscrita : IRepositorioSondagemEscrita
    {
        private readonly ISgpContext database;
        public RepositorioSondagemEscrita(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<SondagemEscritaDto>> ObterSondagemEscritaAsync(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno)
        {
            string query = @"select * from painel_educacional_consolidacao_sondagem_escrita_ue
                             where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                query += " AND codigo_ue = @codigoUe ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (bimestre > 0)
                query += " AND bimestre = @bimestre ";

            if (serieAno > 0)
                query += " AND serie_ano = @serieAno ";

            return await database.Conexao.QueryAsync<SondagemEscritaDto>(query, new
            {
                codigoDre,
                codigoUe,
                anoLetivo,
                bimestre,
                serieAno
            });
        }       
    }
}
