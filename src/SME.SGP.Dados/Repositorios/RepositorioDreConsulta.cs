using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDreConsulta : IRepositorioDreConsulta
    {
        private const string QuerySincronizacao = @"SELECT id, dre_id, abreviacao, nome, data_atualizacao FROM public.dre where dre_id in (#ids);";

        private readonly ISgpContextConsultas contexto;

        public RepositorioDreConsulta(ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos)
        {
            var query = "select id, dre_id, abreviacao, nome from dre d where d.dre_id = ANY(@dresCodigos)";

            return contexto.Conexao.Query<Dre>(query, new { dresCodigos });
        }

        public (IEnumerable<Dre> Dres,string[] CodigosDresNaoEncontrados) MaterializarCodigosDre(string[] idDres)
        {
            string[] naoEncontradas;

            var armazenados = contexto.Conexao.Query<Dre>(QuerySincronizacao.Replace("#ids", string.Join(",", idDres.Select(x => $"'{x}'"))));

            naoEncontradas = idDres.Where(x => !armazenados.Select(y => y.CodigoDre).Contains(x)).ToArray();

            return (armazenados, naoEncontradas);
        }

        public async Task<string> ObterCodigoDREPorTurmaId(long turmaId)
        {
            var query = @"select dre.dre_id 
                          from turma t
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id
                         where t.id = @turmaId";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<string>(query, new { turmaId });
        }

        public async Task<string> ObterCodigoDREPorUEId(long ueId)
        {
            var query = @"select dre.dre_id 
                          from ue 
                         inner join dre on dre.id = ue.dre_id
                         where ue.id = @ueId";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<string>(query, new { ueId });
        }

        public async Task<long> ObterIdDrePorCodigo(string codigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<long>("select Id from dre where dre_id = @codigo", new { codigo });
        }

        public async Task<Dre> ObterPorCodigo(string codigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Dre>("select * from dre where dre_id = @codigo", new { codigo });
        }

        public Dre ObterPorId(long dreId)
        {
            return contexto.Conexao.QueryFirstOrDefault<Dre>("select * from dre where id = @dreId", new { dreId });
        }

        public async Task<Dre> ObterPorIdAsync(long dreId)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Dre>("select * from dre where id = @dreId", new { dreId });
        }

        public async Task<IEnumerable<Dre>> ObterTodas()
        {
            return await contexto.Conexao.QueryAsync<Dre>("select id, dre_id, abreviacao, nome from dre");
        }

        public async Task<IEnumerable<long>> ObterIdsDresAsync()
        {
            return await contexto.Conexao.QueryAsync<long>("select Id from dre ", new { });
        }
    }
}