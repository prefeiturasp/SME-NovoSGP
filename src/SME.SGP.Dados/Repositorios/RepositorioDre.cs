using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dommel;
using System.Linq;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDre : IRepositorioDre
    {
        const string QuerySincronizacao = @"SELECT id, dre_id, abreviacao, nome, data_atualizacao FROM public.dre where dre_id in (#ids);";
        const string Update = "UPDATE public.dre SET abreviacao = @abreviacao, nome = @nome, data_atualizacao = @dataAtualizacao WHERE id = @id;";


        private readonly ISgpContext contexto;

        public RepositorioDre(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Dre> Sincronizar(IEnumerable<Dre> entidades)
        {
            List<Dre> resultado = new List<Dre>();

            var armazenados = contexto.Conexao.Query<Dre>(QuerySincronizacao.Replace("#ids", string.Join(",", entidades.Select(x => $"'{x.CodigoDre}'"))));

            var novos = entidades.Where(x => !armazenados.Select(y => y.CodigoDre).Contains(x.CodigoDre));

            foreach (var item in novos)
            {
                item.DataAtualizacao = DateTime.Today;
                item.Id = (long)contexto.Conexao.Insert(item);

                resultado.Add(item);
            }

            foreach (var item in armazenados)
            {
                var entidade = entidades.First(x => x.CodigoDre == item.CodigoDre);
                entidade.Id = item.Id;

                if (item.DataAtualizacao.Date != DateTime.Today)
                {
                    contexto.Conexao.Execute(Update, new { abreviacao = item.Abreviacao, nome = item.Nome, dataAtualizacao = DateTime.Today, id = item.Id });
                }

                resultado.Add(entidade);
            }

            return resultado;

        }
    }

}
