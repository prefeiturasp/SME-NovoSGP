using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAbandonoUe : IRepositorioPainelEducacionalAbandonoUe
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPainelEducacionalAbandonoUe(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<PainelEducacionalAbandonoUe>> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros)
        {
            var offset = (numeroPagina - 1) * numeroRegistros;
            var query = MontarQuery(codigoDre);

            var registros = await database.Conexao.QueryAsync<PainelEducacionalAbandonoUe>(
                query,
                new { anoLetivo, codigoUe, modalidade, codigoDre, offset, numeroRegistros }
            );

            var totalRegistros = registros.FirstOrDefault()?.TotalRegistros ?? 0;
            var totalPaginas = totalRegistros == 0 ? 0 : (int)Math.Ceiling((double)totalRegistros / numeroRegistros);

            foreach (var item in registros)
            {
                item.TotalRegistros = totalRegistros;
                item.TotalPaginas = totalPaginas;
            }

            return registros;
        }

        private static string MontarQuery(string codigoDre)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
                SELECT 
                    codigo_turma AS CodigoTurma,
                    nome_turma AS NomeTurma,
                    quantidade_desistencias AS QuantidadeDesistencias,
                    COUNT(*) OVER() AS TotalRegistros
                FROM painel_educacional_consolidacao_abandono_ue
                WHERE ano_letivo = @anoLetivo
                  AND codigo_ue = @codigoUe
                  AND modalidade = @modalidade");

            if (!string.IsNullOrEmpty(codigoDre))
                sb.AppendLine(" AND codigo_dre = @codigoDre");

            sb.AppendLine(" ORDER BY nome_turma");
            sb.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");

            return sb.ToString();
        }
    }
}




//Abordagem com o select

/*public async Task<(IEnumerable<PainelEducacionalAbandonoTurmaDto> Modalidades, int TotalPaginas, int TotalRegistros)> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros)
        {
            var query = new StringBuilder();
            
            var sqlCount = @"SELECT COUNT(*) FROM painel_educacional_consolidacao_abandono_ue 
                            WHERE ano_letivo = @anoLetivo 
                            AND codigo_ue = @codigoUe 
                            AND modalidade = @modalidade";
            
            if (!string.IsNullOrEmpty(codigoDre))
                sqlCount += " AND codigo_dre = @codigoDre";
                
            var totalRegistros = await database.Conexao.QueryFirstAsync<int>(sqlCount, new { anoLetivo, codigoUe, modalidade, codigoDre });
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / numeroRegistros);

            query.AppendLine(@"SELECT turma, quantidade_desistentes as QuantidadeDesistentes 
                            FROM painel_educacional_consolidacao_abandono_ue 
                            WHERE ano_letivo = @anoLetivo 
                            AND codigo_ue = @codigoUe 
                            AND modalidade = @modalidade");
            
            if (!string.IsNullOrEmpty(codigoDre))
                query.AppendLine(" AND codigo_dre = @codigoDre");
                
            query.AppendLine(" ORDER BY turma");
            query.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");
            
            var offset = (numeroPagina - 1) * numeroRegistros;
            var modalidades = await database.Conexao.QueryAsync<PainelEducacionalAbandonoTurmaDto>(query.ToString(), 
                            new { anoLetivo, codigoUe, modalidade, codigoDre, offset, numeroRegistros });
                            
            return (modalidades, totalPaginas, totalRegistros);
        }*/






//primeira abordgem para testar

/*
        
*/





//Testar essa abordagem

/*public async Task<(IEnumerable<PainelEducacionalAbandonoTurmaDto> Modalidades, int TotalPaginas, int TotalRegistros)>
    ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, int modalidade, int numeroPagina, int numeroRegistros)
{
    var query = new StringBuilder();

    query.AppendLine(@"
        SELECT 
            turma, 
            quantidade_desistentes AS QuantidadeDesistentes,
            COUNT(*) OVER() AS TotalRegistros
        FROM painel_educacional_consolidacao_abandono_ue
        WHERE ano_letivo = @anoLetivo
          AND codigo_ue = @codigoUe
          AND modalidade = @modalidade");

    if (!string.IsNullOrEmpty(codigoDre))
        query.AppendLine(" AND codigo_dre = @codigoDre");

    query.AppendLine(" ORDER BY turma");
    query.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");

    var offset = (numeroPagina - 1) * numeroRegistros;

    var resultado = await database.Conexao.QueryAsync<PainelEducacionalAbandonoTurmaDto, int, (PainelEducacionalAbandonoTurmaDto, int)>(
        query.ToString(),
        (turmaDto, totalRegistros) => (turmaDto, totalRegistros),
        new { anoLetivo, codigoUe, modalidade, codigoDre, offset, numeroRegistros },
        splitOn: "TotalRegistros"
    );

    var lista = resultado.Select(r => r.Item1).ToList();
    var totalRegistros = resultado.FirstOrDefault().Item2;
    var totalPaginas = totalRegistros == 0 ? 0 : (int)Math.Ceiling((double)totalRegistros / numeroRegistros);

    return (lista, totalPaginas, totalRegistros);
}
*/