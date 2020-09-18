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
    public class RepositorioUe : IRepositorioUe
    {
        private const string QuerySincronizacao = @"SELECT id, ue_id, dre_id, nome, tipo_escola, data_atualizacao FROM public.ue where ue_id in (#ids);";
        private const string Update = "UPDATE public.ue SET nome = @nome, tipo_escola = @tipoEscola, data_atualizacao = @dataAtualizacao WHERE id = @id;";

        private readonly ISgpContext contexto;

        public RepositorioUe(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Ue> ListarPorCodigos(string[] codigos)
        {
            var query = "select id, ue_id, dre_id, nome, tipo_escola FROM public.ue where ue_id = ANY(@codigos)";

            return contexto.Conexao.Query<Ue>(query, new { codigos });
        }

        public IEnumerable<Ue> MaterializarCodigosUe(string[] idUes, out string[] codigosNaoEncontrados)
        {
            List<Ue> resultado = new List<Ue>();
            List<string> naoEncontrados = new List<string>();

            for (int i = 0; i < idUes.Count(); i = i + 900)
            {
                var iteracao = idUes.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Ue>(QuerySincronizacao.Replace("#ids", string.Join(",", idUes.Select(x => $"'{x}'"))));

                naoEncontrados.AddRange(iteracao.Where(x => !armazenados.Select(y => y.CodigoUe).Contains(x)));

                resultado.AddRange(armazenados);
            }
            codigosNaoEncontrados = naoEncontrados.ToArray();

            return resultado;
        }

        public async Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano)
        {
            var query = @"select distinct t.modalidade_codigo from turma t
                                inner join ue u
                                on t.ue_id = u.id
                                    where u.ue_id = @ueCodigo
                                and t.ano_letivo = @ano";

            return await contexto.QueryAsync<Modalidade>(query, new { ueCodigo, ano });
        }

        public Ue ObterPorCodigo(string ueId)
        {
            return contexto.QueryFirstOrDefault<Ue>("select * from ue where ue_id = @ueId", new { ueId });
        }

        public IEnumerable<Ue> ObterTodas()
        {
            var query = @"select
	                        id,
	                        ue_id,
	                        dre_id,
	                        nome,
	                        tipo_escola,
	                        data_atualizacao
                        from
	                        ue";

            return contexto.Query<Ue>(query);
        }

        public IEnumerable<Ue> ObterPorDre(long dreId)
        {
            var query = @"select
	                        id,
	                        ue_id,
	                        dre_id,
	                        nome,
	                        tipo_escola,
	                        data_atualizacao
                        from
	                        ue
                        where
	                        dre_id = @dreId";

            return contexto.Query<Ue>(query, new { dreId });
        }

        public Ue ObterPorId(long id)
                    => contexto.Conexao.QueryFirst<Ue>("select * from ue where id = @id", new { id });

        public async Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano)
        {
            var query = @"select t.* from turma t
                            inner join ue u
                            on t.ue_id = u.id
                            where u.ue_id = @ueCodigo
                            and t.modalidade_codigo = @modalidade
                            and t.ano_letivo = @ano";

            return await contexto.QueryAsync<Turma>(query, new { ueCodigo, modalidade, ano });
        }

        public Ue ObterUEPorTurma(string turmaId)
        {
            var query = @"select
                            escola.*
                        from
                            ue escola
                        inner
                        join turma t on
                        t.ue_id = escola.id
                        where
                            t.turma_id = @turmaId";
            return contexto.QueryFirstOrDefault<Ue>(query, new { turmaId });
        }

        public async Task<IEnumerable<Ue>> SincronizarAsync(IEnumerable<Ue> entidades, IEnumerable<Dre> dres)
        {
            List<Ue> resultado = new List<Ue>();

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = await contexto.Conexao.QueryAsync<Ue>(QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoUe}'"))));

                var novos = iteracao.Where(x => !armazenados.Select(y => y.CodigoUe).Contains(x.CodigoUe));

                foreach (var item in novos)
                {
                    item.DataAtualizacao = DateTime.Today;
                    item.Dre = dres.First(x => x.CodigoDre == item.Dre.CodigoDre);
                    item.DreId = item.Dre.Id;
                    item.Id = (long)await contexto.Conexao.InsertAsync(item);
                    resultado.Add(item);
                }

                var modificados = from c in entidades
                                  join l in armazenados on c.CodigoUe equals l.CodigoUe
                                  where l.DataAtualizacao != DateTime.Today &&
                                        (c.Nome != l.Nome ||
                                        c.TipoEscola != l.TipoEscola)
                                  select new Ue()
                                  {
                                      CodigoUe = c.CodigoUe,
                                      DataAtualizacao = DateTime.Today,
                                      Dre = l.Dre,
                                      DreId = l.DreId,
                                      Id = l.Id,
                                      Nome = c.Nome,
                                      TipoEscola = c.TipoEscola
                                  };

                foreach (var item in modificados)
                {
                    await contexto.Conexao.ExecuteAsync(Update, new { nome = item.Nome, tipoEscola = item.TipoEscola, dataAtualizacao = item.DataAtualizacao, id = item.Id });

                    resultado.Add(item);
                }

                resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoUe).Contains(x.CodigoUe)));
            }

            return resultado;
        }

        public async Task<long> ObterIdPorCodigo(string ueCodigo)
            => await contexto.QueryFirstOrDefaultAsync("select id from ue where ue_id = @ueCodigo", new { ueCodigo });
    }
}