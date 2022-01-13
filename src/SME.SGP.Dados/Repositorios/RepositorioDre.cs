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
    }
}