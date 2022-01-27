using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Ue>> SincronizarAsync(IEnumerable<Ue> entidades, IEnumerable<Dre> dres)
        {
            List<Ue> resultado = new List<Ue>();

            for (int i = 0; i < entidades.Count(); i = i + 900)
            {
                var iteracao = entidades.Skip(i).Take(900);

                var armazenados = await contexto.Conexao.QueryAsync<Ue>(QuerySincronizacao.Replace("#ids", string.Join(",", iteracao.Select(x => $"'{x.CodigoUe}'"))));

                var novos = iteracao.Where(x => !armazenados.Select(y => y.CodigoUe).Contains(x.CodigoUe));
                
                await PersisteNovosRegistros(dres, resultado, novos);

                var modificados = from c in iteracao
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

        private async Task PersisteNovosRegistros(IEnumerable<Dre> dres, List<Ue> resultado, IEnumerable<Ue> novos)
        {
            foreach (var item in novos)
            {
                item.DataAtualizacao = DateTime.Today;
                item.Dre = dres.First(x => x.CodigoDre == item.Dre.CodigoDre);
                item.DreId = item.Dre.Id;
                item.Id = (long)await contexto.Conexao.InsertAsync(item);
                resultado.Add(item);
            }
        }

        public async Task<long> IncluirAsync(Ue ueParaIncluir)
        {
            return (long)await contexto.Conexao.InsertAsync(ueParaIncluir);
        }

        public async Task AtualizarAsync(Ue ueParaAtualizar)
        {
            await contexto.Conexao.UpdateAsync(ueParaAtualizar);
        }
    }
}