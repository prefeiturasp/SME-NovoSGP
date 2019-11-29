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
    public class RepositorioUe : IRepositorioUe
    {
        const string QuerySincronizacao = @"SELECT id, ue_id, dre_id, nome, tipo_escola, data_atualizacao FROM public.ue where ue_id in (#ids);";
        const string Update = "UPDATE public.ue SET nome = @nome, tipo_escola = @tipoEscola, data_atualizacao = @dataAtualizacao WHERE id = @id;";

        private readonly ISgpContext contexto;
        private readonly IRepositorioDre respositorioDre;

        public RepositorioUe(ISgpContext contexto, IRepositorioDre respositorioDre)
        {
            this.contexto = contexto;
            this.respositorioDre = respositorioDre;
        }

        public IEnumerable<Ue> Sincronizar(IEnumerable<Ue> entidades, IEnumerable<Dre> dres)
        {
            List<Ue> resultado = new List<Ue>();

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Ue>(QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoUe}'"))));

                var novos = iteracao.Where(x => !armazenados.Select(y => y.CodigoUe).Contains(x.CodigoUe));

                foreach (var item in novos)
                {
                    item.DataAtualizacao = DateTime.Today;
                    item.Dre = dres.First(x => x.CodigoDre == item.Dre.CodigoDre);
                    item.DreId = item.Dre.Id;
                    item.Id = (long)contexto.Conexao.Insert(item);
                    resultado.Add(item);
                }

                foreach (var item in armazenados)
                {
                    var entidade = iteracao.First(x => x.CodigoUe == item.CodigoUe);
                    entidade.Id = item.Id;
                    entidade.Dre = item.Dre;
                    entidade.DreId = item.DreId;

                    if (item.DataAtualizacao.Date != DateTime.Today)
                    {
                        contexto.Conexao.Execute(Update, new { nome = item.Nome, tipoEscola = item.TipoEscola, dataAtualizacao = DateTime.Today, id = item.Id });
                    }
                    resultado.Add(entidade);
                }
            }
            return resultado;

        }
    }

}
