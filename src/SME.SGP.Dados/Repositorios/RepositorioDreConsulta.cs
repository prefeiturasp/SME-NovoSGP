using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDreConsulta
        : IRepositorioDreConsulta
    {
        private const string ColunasDre = @"
            d.id AS Id,
            d.dre_id AS CodigoDre,
            d.abreviacao AS Abreviacao,
            d.nome AS Nome,
            d.data_atualizacao AS DataAtualizacao";

        private const string QuerySincronizacao = @"
            SELECT
                d.id AS Id,
                d.dre_id AS CodigoDre,
                d.abreviacao AS Abreviacao,
                d.nome AS Nome,
                d.data_atualizacao AS DataAtualizacao
            FROM public.dre d
            WHERE d.dre_id IN (#ids);";

        private readonly ISgpContextConsultas contexto;

        public RepositorioDreConsulta(
            ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Dre> ListarPorCodigos(
            string[] dresCodigos)
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d
                WHERE d.dre_id = ANY(@dresCodigos);";

            return contexto.Conexao.Query<Dre>(
                query,
                new { dresCodigos });
        }

        public async Task<IEnumerable<Dre>>
            ListarPorCodigosAsync(
                string[] dresCodigos)
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d
                WHERE d.dre_id = ANY(@dresCodigos);";

            return await contexto.Conexao
                .QueryAsync<Dre>(
                    query,
                    new { dresCodigos });
        }

        public (
            IEnumerable<Dre> Dres,
            string[] CodigosDresNaoEncontrados)
            MaterializarCodigosDre(
                string[] idDres)
        {
            var sql = QuerySincronizacao.Replace(
                "#ids",
                string.Join(
                    ",",
                    idDres.Select(
                        codigo => $"'{codigo}'")));

            var armazenados =
                contexto.Conexao.Query<Dre>(sql);

            var codigosArmazenados =
                armazenados
                    .Select(dre => dre.CodigoDre)
                    .ToHashSet();

            var naoEncontradas =
                idDres
                    .Where(codigo =>
                        !codigosArmazenados.Contains(codigo))
                    .ToArray();

            return (
                armazenados,
                naoEncontradas);
        }

        public async Task<string>
            ObterCodigoDREPorTurmaId(
                long turmaId)
        {
            const string query = @"
                SELECT d.dre_id
                FROM turma t
                INNER JOIN ue u
                    ON u.id = t.ue_id
                INNER JOIN dre d
                    ON d.id = u.dre_id
                WHERE t.id = @turmaId;";

            return await contexto.Conexao
                .QueryFirstOrDefaultAsync<string>(
                    query,
                    new { turmaId });
        }

        public async Task<string>
            ObterCodigoDREPorUEId(
                long ueId)
        {
            const string query = @"
                SELECT d.dre_id
                FROM ue u
                INNER JOIN dre d
                    ON d.id = u.dre_id
                WHERE u.id = @ueId;";

            return await contexto.Conexao
                .QueryFirstOrDefaultAsync<string>(
                    query,
                    new { ueId });
        }

        public async Task<long>
            ObterIdDrePorCodigo(
                string codigo)
        {
            const string query = @"
                SELECT d.id
                FROM dre d
                WHERE d.dre_id = @codigo;";

            return await contexto.Conexao
                .QueryFirstOrDefaultAsync<long>(
                    query,
                    new { codigo });
        }

        public async Task<Dre> ObterPorCodigo(
            string codigo)
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d
                WHERE d.dre_id = @codigo;";

            return await contexto.Conexao
                .QueryFirstOrDefaultAsync<Dre>(
                    query,
                    new { codigo });
        }

        public Dre ObterPorId(long dreId)
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d
                WHERE d.id = @dreId;";

            return contexto.Conexao
                .QueryFirstOrDefault<Dre>(
                    query,
                    new { dreId });
        }

        public async Task<Dre> ObterPorIdAsync(
            long dreId)
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d
                WHERE d.id = @dreId;";

            return await contexto.Conexao
                .QueryFirstOrDefaultAsync<Dre>(
                    query,
                    new { dreId });
        }

        public async Task<IEnumerable<Dre>>
            ObterTodas()
        {
            var query = $@"
                SELECT
                    {ColunasDre}
                FROM dre d;";

            return await contexto.Conexao
                .QueryAsync<Dre>(query);
        }

        public async Task<IEnumerable<long>>
            ObterIdsDresAsync()
        {
            const string query = @"
                SELECT d.id
                FROM dre d;";

            return await contexto.Conexao
                .QueryAsync<long>(query);
        }
    }
}