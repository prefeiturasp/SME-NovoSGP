using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDre : IRepositorioDre
    {
        private const string QuerySincronizacao = @"SELECT id, dre_id, abreviacao, nome, data_atualizacao FROM public.dre where dre_id in (#ids);";
        private const string Update = "UPDATE public.dre SET abreviacao = @abreviacao, nome = @nome, data_atualizacao = @dataAtualizacao WHERE id = @id;";

        private readonly ISgpContext contexto;

        public RepositorioDre(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Dre> ListarPorCodigos(string[] dresCodigos)
        {
            var query = "select id, dre_id, abreviacao, nome from dre d where d.dre_id = ANY(@dresCodigos)";

            return contexto.Conexao.Query<Dre>(query, new { dresCodigos });
        }

        public IEnumerable<Dre> MaterializarCodigosDre(string[] idDres, out string[] naoEncontradas)
        {
            var armazenados = contexto.Conexao.Query<Dre>(QuerySincronizacao.Replace("#ids", string.Join(",", idDres.Select(x => $"'{x}'"))));

            naoEncontradas = idDres.Where(x => !armazenados.Select(y => y.CodigoDre).Contains(x)).ToArray();

            return armazenados;
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

        public Dre ObterPorCodigo(string codigo)
        {
            return contexto.Conexao.QueryFirstOrDefault<Dre>("select * from dre where dre_id = @codigo", new { codigo });
        }

        public Dre ObterPorId(long dreId)
        {
            return contexto.Conexao.QueryFirstOrDefault<Dre>("select * from dre where id = @dreId", new { dreId });
        }

        public async Task<Dre> ObterPorIdAsync(long dreId)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Dre>("select * from dre where id = @dreId", new { dreId });
        }

        public IEnumerable<Dre> ObterTodas()
        {
            return contexto.Conexao.Query<Dre>("select id, dre_id, abreviacao, nome from dre");
        }

        public async Task<IEnumerable<Dre>> SincronizarAsync(IEnumerable<Dre> entidades)
        {
            List<Dre> resultado = new List<Dre>();           

            var armazenados = await contexto.Conexao.QueryAsync<Dre>(QuerySincronizacao.Replace("#ids", string.Join(",", entidades.Select(x => $"'{x.CodigoDre}'"))));

            var novos = entidades.Where(x => !armazenados.Select(y => y.CodigoDre).Contains(x.CodigoDre));

            foreach (var item in novos)
            {
                item.DataAtualizacao = DateTime.Today;
                item.Id = (long) await contexto.Conexao.InsertAsync(item);

                resultado.Add(item);
            }

            var modificados = from c in entidades
                              join l in armazenados on c.CodigoDre equals l.CodigoDre
                              where l.DataAtualizacao != DateTime.Today &&
                                    (c.Abreviacao != l.Abreviacao ||
                                    c.Nome != l.Nome)
                              select new Dre()
                              {
                                  Id = l.Id,
                                  Nome = c.Nome,
                                  Abreviacao = c.Abreviacao,
                                  CodigoDre = c.CodigoDre,
                                  DataAtualizacao = DateTime.Today
                              };

            foreach (var item in modificados)
            {
                await contexto.Conexao.ExecuteAsync(Update, new { abreviacao = item.Abreviacao, nome = item.Nome, dataAtualizacao = item.DataAtualizacao, id = item.Id });
                resultado.Add(item);
            }

            resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoDre).Contains(x.CodigoDre)));

            return resultado;
        }

        public async Task<IEnumerable<long>> ObterIdsDresAsync()
        {
            return await contexto.Conexao.QueryAsync<long>("select Id from dre ", new { });
        }
    }
}