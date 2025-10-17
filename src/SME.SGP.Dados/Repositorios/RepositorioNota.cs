using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNota : IRepositorioNota
    {
        private readonly ISgpContext database;
        public RepositorioNota(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, int anoTurma)
        {
             string query = @"select ano_letivo as AnoLetivo,
                                     ano_turma as AnoTurma,
                                     bimestre,
                                     modalidade,
                                     quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                     quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                     quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                     quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                     quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                     quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                             from painel_educacional_consolidacao_notas
                             where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (bimestre > 0)
                query += " AND bimestre = @bimestre ";

            if (anoTurma > 0)
                query += " AND ano_turma = @anoTurma ";

            return await database.Conexao.QueryAsync<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>(query, new
            {
                codigoDre,
                anoLetivo,
                bimestre,
                anoTurma
            });
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoUeRetornoSelectDto>> ObterNotasVisaoUe(string codigoDre, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            string query = @"select  ano_letivo as AnoLetivo,
                                     turma as Turma,
                                     bimestre,
                                     modalidade,
                                     quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                     quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                     quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                     quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                     quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                     quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                             from painel_educacional_consolidacao_notas_ue
                             where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (bimestre > 0)
                query += " AND bimestre = @bimestre ";

            if (modalidade > 0)
                query += " AND modalidade = @modalidade ";           

            return await database.Conexao.QueryAsync<PainelEducacionalNotasVisaoUeRetornoSelectDto>(query, new
            {
                codigoDre,
                anoLetivo,
                bimestre,
                modalidade
            });
        }
    }
}
