using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System;
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
        public async Task<IEnumerable<PainelEducacionalNotasVisaoUeRetornoSelectDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            try
            {
                string query = @"select ano_letivo as AnoLetivo,
                                     codigo_dre as CodigoDre,
                                     codigo_ue as CodigoUe,
                                     bimestre,
                                     modalidade_ensino as Modalidade,
                                     serie_turma as AnoTurma,
                                     quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                     quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                     quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                     quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                     quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                     quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                             from painel_educacional_consolidacao_nota_ue
                             where 1 = 1 ";

                if (!string.IsNullOrWhiteSpace(codigoUe))
                    query += " AND codigo_ue = @codigoUe ";

                if (anoLetivo > 0)
                    query += " AND ano_letivo = @anoLetivo ";

                if (bimestre > 0)
                    query += " AND bimestre = @bimestre ";

                if (modalidade > 0)
                    query += " AND modalidade_ensino = @modalidade ";

                return await database.Conexao.QueryAsync<PainelEducacionalNotasVisaoUeRetornoSelectDto>(query, new
                {
                    codigoUe,
                    anoLetivo,
                    bimestre,
                    modalidade
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma)
        {
            try
            {
                string query = @"select ano_letivo as AnoLetivo,
                                     bimestre,
                                     ano_turma as AnoTurma,
                                     modalidade_ensino as Modalidade,
                                     quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                     quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                     quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                     quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                     quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                     quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                             from painel_educacional_consolidacao_nota
                             where 1 = 1 ";

                if (!string.IsNullOrWhiteSpace(codigoDre))
                    query += " AND codigo_dre = @codigoDre ";

                if (anoLetivo > 0)
                    query += " AND ano_letivo = @anoLetivo ";

                if (bimestre > 0)
                    query += " AND bimestre = @bimestre ";

                if (!string.IsNullOrEmpty(anoTurma))
                    query += " AND ano_turma = @anoTurma ";

                return await database.Conexao.QueryAsync<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>(query, new
                {
                    codigoDre,
                    anoLetivo,
                    bimestre,
                    anoTurma
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
