using Dapper;
using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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

        public async Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano)
        {
            var query = @"select distinct t.modalidade_codigo from turma t
                                inner join ue u
                                on t.ue_id = u.id
                                    where u.ue_id = @ueCodigo
                                and t.ano_letivo = @ano";

            return await contexto.QueryAsync<Modalidade>(query, new { ueCodigo, ano });
        }

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
                    contexto.Conexao.Execute(Update, new { nome = item.Nome, tipoEscola = item.TipoEscola, dataAtualizacao = item.DataAtualizacao, id = item.Id });

                    resultado.Add(item);
                }

                resultado.AddRange(armazenados.Where(x => !resultado.Select(y => y.CodigoUe).Contains(x.CodigoUe)));
            }

            return resultado;
        }
    }
}