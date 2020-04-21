using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCicloEnsino : RepositorioBase<CicloEnsino>, IRepositorioCicloEnsino
    {
        private const string QuerySincronizacao = @"SELECT id, cod_ciclo_ensino_eol,codigo_modalidade_ensino,codigo_etapa_ensino,descricao, data_atualizacao FROM public.ciclo_ensino where cod_ciclo_ensino_eol in (#ids);";

        public RepositorioCicloEnsino(ISgpContext database) : base(database)
        {
        }

        public void Sincronizar(IEnumerable<CicloEnsino> ciclosEnsino)
        {
            var armazenados = database.Conexao.Query<CicloEnsino>(QuerySincronizacao.Replace("#ids", string.Join(",", ciclosEnsino.Select(x => $"'{x.CodEol}'"))));
            var novos = ciclosEnsino.Where(x => !armazenados.Select(y => y.CodEol).Contains(x.CodEol));
            foreach (var item in novos)
            {
                item.DtAtualizacao = DateTime.Today;
                Salvar(item);
            }

            var modificados = from c in ciclosEnsino
                              join l in armazenados on c.CodEol equals l.CodEol
                              where l.DtAtualizacao != DateTime.Today &&
                                    (c.Descricao != l.Descricao) &&
                                    (c.CodigoModalidadeEnsino != l.CodigoModalidadeEnsino) &&
                                    (c.CodigoEtapaEnsino != l.CodigoEtapaEnsino)
                              select new CicloEnsino()
                              {
                                  Id = l.Id,
                                  Descricao = c.Descricao,
                                  DtAtualizacao = DateTime.Today
                              };

            foreach (var item in modificados)
                Salvar(item);
        }
    }
}