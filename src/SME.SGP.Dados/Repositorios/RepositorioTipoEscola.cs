using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoEscola : RepositorioBase<TipoEscolaEol>, IRepositorioTipoEscola
    {
        private const string QuerySincronizacao = @"SELECT id, cod_tipo_escola_eol,  descricao, data_atualizacao, criado_por, criado_rf FROM public.tipo_escola where cod_tipo_escola_eol in (#ids);";

        public RepositorioTipoEscola(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
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
        public async Task<IEnumerable<TipoEscolaDto>> ObterTipoEscolaPorDreEUe(string dreCodigo, string ueCodigo, int[] modalidades)
        {
            var query = new StringBuilder(@"select distinct te.id,
                                                   te.cod_tipo_escola_eol as CodTipoEscola,
                                                   te.descricao
                                              from tipo_escola te
                                             inner join ue on ue.tipo_escola = te.cod_tipo_escola_eol
                                             inner join dre on dre.id = ue.dre_id ");

            if (modalidades.Length > 0)
                query.AppendLine(" inner join turma t on ue.id = t.ue_id ");

            if (!string.IsNullOrWhiteSpace(dreCodigo) && dreCodigo != "-99")
                query.AppendLine("where dre.dre_id = @dreCodigo ");

            if (!string.IsNullOrWhiteSpace(ueCodigo) && ueCodigo != "-99")
                query.AppendLine("and ue.ue_id = @ueCodigo ");

            if (modalidades.Length > 0)
                query.AppendLine(" and t.modalidade_codigo = any(@modalidades) ");

            return await database.Conexao.QueryAsync<TipoEscolaDto>(query.ToString(), new { dreCodigo, ueCodigo, modalidades });
        }

        public async Task<IEnumerable<TipoEscolaEol>> ObterTodasAsync()
        {
            var query = "select tp.*  from tipo_escola tp";

            return await database.Conexao.QueryAsync<TipoEscolaEol>(query);
        }
    }
}