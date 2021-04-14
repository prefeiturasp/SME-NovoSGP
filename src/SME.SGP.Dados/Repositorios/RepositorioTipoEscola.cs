using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoEscola : RepositorioBase<TipoEscolaEol>, IRepositorioTipoEscola
    {
        private const string QuerySincronizacao = @"SELECT id, cod_tipo_escola_eol,  descricao, data_atualizacao, criado_por, criado_rf FROM public.tipo_escola where cod_tipo_escola_eol in (#ids);";

        public RepositorioTipoEscola(ISgpContext database) : base(database)
        {
        }

        public async Task<TipoEscolaEol> ObterPorCodigoAsync(long codigo)
        {
            var query = "select tp.*  from tipo_escola tp where tp.cod_tipo_escola_eol = @codigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<TipoEscolaEol>(query, new { codigo });
        }

        public void Sincronizar(IEnumerable<TipoEscolaEol> tiposEscola)
        {
            var armazenados = database.Conexao.Query<TipoEscolaEol>(QuerySincronizacao.Replace("#ids", string.Join(",", tiposEscola.Select(x => $"'{x.CodEol}'"))));
            var novos = tiposEscola.Where(x => !armazenados.Select(y => y.CodEol).Contains(x.CodEol));
            foreach (var item in novos)
            {
                item.DtAtualizacao = DateTime.Today;
                Salvar(item);
            }

            var modificados = from c in tiposEscola
                              join l in armazenados on c.CodEol equals l.CodEol
                              where l.DtAtualizacao != DateTime.Today &&
                                    (c.Descricao != l.Descricao)
                              select new TipoEscolaEol()
                              {
                                  Id = l.Id,
                                  Descricao = c.Descricao,
                                  CriadoPor = l.CriadoPor,
                                  CriadoRF = l.CriadoRF,
                                  DtAtualizacao = DateTime.Today
                              };

            foreach (var item in modificados)
                Salvar(item);
        }
    }
}