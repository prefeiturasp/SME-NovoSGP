using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUeConsulta : IRepositorioUeConsulta
    {
        private const string QuerySincronizacao = @"SELECT id, ue_id, dre_id, nome, tipo_escola, data_atualizacao FROM public.ue where ue_id in (#ids);";

        private readonly ISgpContextConsultas contexto;

        public RepositorioUeConsulta(ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<Ue> ListarPorCodigos(string[] codigos)
        {
            var query = "select id, ue_id, dre_id, nome, tipo_escola FROM public.ue where ue_id = ANY(@codigos)";

            return contexto.Conexao.Query<Ue>(query, new { codigos });
        }

        public Tuple<List<Ue>, string[]> MaterializarCodigosUe(string[] idUes)
        {
            List<Ue> resultado = new List<Ue>();
            List<string> naoEncontrados = new List<string>();
            string[] codigosNaoEncontrados;

            for (int i = 0; i < idUes.Count(); i = i + 900)
            {
                var iteracao = idUes.Skip(i).Take(900);

                var armazenados = contexto.Conexao.Query<Ue>(QuerySincronizacao.Replace("#ids", string.Join(",", idUes.Select(x => $"'{x}'"))));

                naoEncontrados.AddRange(iteracao.Where(x => !armazenados.Select(y => y.CodigoUe).Contains(x)));

                resultado.AddRange(armazenados);
            }
            codigosNaoEncontrados = naoEncontrados.ToArray();

            return new Tuple<List<Ue>, string[]>(resultado, codigosNaoEncontrados);
        }

        public async Task<IEnumerable<Modalidade>> ObterModalidades(string ueCodigo, int ano, IEnumerable<Modalidade> modalidadesQueSeraoIgnoradas)
        {
            var query = @"select distinct t.modalidade_codigo from turma t
                                inner join ue u
                                on t.ue_id = u.id
                            where u.ue_id = @ueCodigo
                            and t.ano_letivo = @ano
                            and (@modalidadesQueSeraoIgnoradas is null or not(t.modalidade_codigo = any(@modalidadesQueSeraoIgnoradas)))";

            var modalidadesQueSeraoIgnoradasArray = modalidadesQueSeraoIgnoradas.Select(x => (int)x).ToArray();
            return await contexto.QueryAsync<Modalidade>(query, new { ueCodigo, ano, modalidadesQueSeraoIgnoradas = modalidadesQueSeraoIgnoradasArray });
        }

        public Ue ObterPorCodigo(string ueId)
        {
            return contexto.QueryFirstOrDefault<Ue>("select * from ue where ue_id = @ueId", new { ueId });
        }

        public async Task<Ue> ObterUeComDrePorCodigo(string ueCodigo)
        {
            var query = @"select ue.*, dre.* 
                            from ue 
                           inner join dre on dre.id = ue.dre_id
                           where ue_id = @ueCodigo";

            return (await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.AdicionarDre(dre);
                return ue;
            },
            new { ueCodigo })).FirstOrDefault();
        }

        public async Task<Ue> ObterUeComDrePorId(long ueId)
        {
            var query = @"select ue.*, dre.* 
                            from ue 
                           inner join dre on dre.id = ue.dre_id
                           where ue.id = @ueId";

            return (await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.AdicionarDre(dre);
                return ue;
            },
            new { ueId })).FirstOrDefault();
        }

        public async Task<IEnumerable<string>> ObterCodigosUEs()
        {
            var query = @"select ue_id from ue";
            return await contexto.Conexao.QueryAsync<string>(query);
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

        public async Task<IEnumerable<Turma>> ObterTurmas(string ueCodigo, Modalidade modalidade, int ano, bool ehHistorico)
        {
            var query = new StringBuilder(@"select t.* from turma t
                            inner join ue u
                            on t.ue_id = u.id
                            where u.ue_id = @ueCodigo
                            and t.modalidade_codigo = @modalidade
                            and t.ano_letivo = @ano
                            and t.historica = @ehHistorico");

            return await contexto.QueryAsync<Turma>(query.ToString(), new { ueCodigo, modalidade, ano, ehHistorico });
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

        public async Task<Ue> ObterUEPorTurmaId(long turmaId)
        {
            var query = @"select
                            escola.*
                        from
                            ue escola
                        inner
                        join turma t on
                        t.ue_id = escola.id
                        where
                            t.id = @turmaId";
            return await contexto.QueryFirstOrDefaultAsync<Ue>(query, new { turmaId });
        }

        public async Task<bool> ValidarUeEducacaoInfantil(long ueId)
        {
            var query = @"select 1 
                          from ue
                         where ue.Id = @ueId
                           and ue.tipo_escola in (2, 17, 28, 30, 31)";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ueId });
        }

        public async Task<IEnumerable<Ue>> ObterUesPorModalidade(int[] modalidades, int anoLetivo = 0)
        {
            var query = @"select distinct ue.*, dre.*
                          from turma t
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id
                         where t.modalidade_codigo = ANY(@modalidades) ";

            if (anoLetivo > 0)
                query += "and ano_letivo = @anoLetivo";

            return await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.Dre = dre;

                return ue;
            }, new { modalidades, anoLetivo });

        }

        public async Task<IEnumerable<Ue>> ObterUesComDrePorDreEModalidade(string dreCodigo, Modalidade modalidade)
        {
            var query = @"select ue.*, dre.* 
                            from ue 
                           inner join dre on dre.id = ue.dre_id
                           where dre_id = @dreCodigo
                             and exists (select 1 from turma where modalidade_codigo = @modalidade)";

            return (await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.AdicionarDre(dre);
                return ue;
            },
            new { dreCodigo, modalidade }));
        }

        public async Task<IEnumerable<Ue>> ObterUEsSemPeriodoFechamento(long periodoEscolarId, int ano, int[] modalidades, DateTime dataReferencia)
        {
            var query = @" select distinct ue.*, dre.*
                               from ue 
                              inner join dre on dre.id = ue.dre_id
                              inner join turma t on t.ue_id = ue.id
                              where t.modalidade_codigo = any(@modalidades)
                                and t.ano_letivo = @ano
                                and not exists (select 1
    		                            from periodo_fechamento pf
    		                            inner join periodo_fechamento_bimestre pb on pb.periodo_fechamento_id = pf.id
    		                            where pb.periodo_escolar_id = @periodoEscolarId
                                            and TO_DATE(pf.inicio::TEXT, 'yyyy/mm/dd') <= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
                                            and TO_DATE(pf.fim::TEXT, 'yyyy/mm/dd') >= TO_DATE(@dataReferencia, 'yyyy/mm/dd')
                            )";

            return await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query,
                (ue, dre) =>
                {
                    ue.AdicionarDre(dre);

                    return ue;
                }, new { periodoEscolarId, ano, modalidades, dataReferencia });
        }

        public async Task<int> ObterQuantidadeTurmasSeriadas(long ueId, int ano)
        {
            var query = @"select count(id) from turma where ano between '1' and '9' and ue_id = @ueId and ano_letivo = @ano";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<int>(query, new { ueId, ano });
        }

        public async Task<IEnumerable<Ue>> ObterUesPorIds(long[] ids)
        {
            var query = @"select * from ue where id = ANY(@ids)";

            return await contexto.QueryAsync<Ue>(query, new { ids });
        }

        public async Task<IEnumerable<Ue>> ObterUEsComDREsPorIds(long[] ids)
        {
            var query = @"select ue.*, dre.* 
                            from ue 
                           inner join dre on dre.id = ue.dre_id
                           where ue.id = ANY(@ids)";

            return await contexto.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.Dre = dre;

                return ue;
            }, new { ids });
        }

        public async Task<Ue> ObterUePorId(long id)
        {
            var query = @"select ue.* 
                        from ue 
                        inner join dre on dre.id = ue.dre_id
                        where ue.id = @id";

            return await contexto.QueryFirstAsync<Ue>(query, new { id });
        }

        public async Task<TipoEscola> ObterTipoEscolaPorCodigo(string ueCodigo)
        {
            var query = "select tipo_escola from ue where ue_id = @ueCodigo";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<TipoEscola>(query, new { ueCodigo });
        }

        public async Task<IEnumerable<string>> ObterUesCodigosPorDreAsync(long dreId)
        {
            return await contexto.Conexao.QueryAsync<string>("select ue_id from ue where dre_id = @dreId", new { dreId });
        }

        public async Task<int> ObterQuantidadeUesPorAnoLetivoAsync(int anoLetivo)
        {
            var query = @"select count(distinct(t.ue_id)) from turma t where t.ano_letivo = @anoLetivo";

            return await contexto.Conexao.QueryFirstOrDefaultAsync<int>(query, new { anoLetivo });
        }

        public async Task<IEnumerable<string>> ObterUesCodigosPorModalidadeEAnoLetivo(Modalidade modalidade, int anoLetivo)
        {
            var query = @"select distinct(ue.ue_id) 
                            from turma t
                           inner join ue on ue.id = t.ue_id 
                           where modalidade_codigo = @modalidadeInt
                             and t.ano_letivo = @anoLetivo";
            var modalidadeInt = (int)modalidade;
            return await contexto.Conexao.QueryAsync<string>(query, new { modalidadeInt, anoLetivo });
        }

        public Task<DreUeCodigoDto> ObterCodigosDreUePorId(long ueId)
        {
            var query = @"select ue.ue_id as UeCodigo
                                , dre.dre_id as DreCodigo
                          from ue 
                        inner join dre on dre.id = ue.dre_id
                        where ue.id = @ueId";

            return contexto.Conexao.QueryFirstOrDefaultAsync<DreUeCodigoDto>(query, new { ueId });
        }

        public Task<IEnumerable<long>> ObterTodosIds()
        {
            var query = @"select id from ue";

            return contexto.Conexao.QueryAsync<long>(query);
        }

        public async Task<IEnumerable<Ue>> ObterUEsComDREsPorModalidadeTipoCalendarioQuery(int[] modalidades, int anoLetivo = 0)
        {
            var query = @"select distinct ue.*, dre.*
                          from turma t
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id
                         where t.modalidade_codigo = ANY(@modalidades) ";

            if (anoLetivo > 0)
                query += "and ano_letivo = @anoLetivo";

            return await contexto.Conexao.QueryAsync<Ue, Dre, Ue>(query, (ue, dre) =>
            {
                ue.Dre = dre;

                return ue;
            }, new { modalidades, anoLetivo });
        }
    }
}