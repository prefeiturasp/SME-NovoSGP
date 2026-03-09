using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioAbrangencia : IRepositorioAbrangencia
    {
        protected readonly ISgpContext database;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public RepositorioAbrangencia(ISgpContext database,
                                      IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task AtualizaAbrangenciaHistorica(IEnumerable<long> ids)
        {
            var dtFimVinculo = DateTimeExtension.HorarioBrasilia().Date;

            string comando = $@" update abrangencia as a
                                set historico = true, dt_fim_vinculo = '{dtFimVinculo.Year}-{dtFimVinculo.Month}-{dtFimVinculo.Day}'
                                from abrangencia ab
                                left join turma t on t.id = ab.turma_id
                                where a.id = ab.id
                                and (ab.turma_id is null Or (t.id = ab.turma_id and t.ano_letivo = {dtFimVinculo.Year}))                                    
                                and a.id in (#ids) ";

            for (int i = 0; i < ids.Count(); i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);
                await database.Conexao.ExecuteAsync(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public async Task AtualizaAbrangenciaHistoricaAnosAnteriores(IEnumerable<long> ids, int anoLetivo)
        {
            var dtFimVinculo = DateTimeExtension.HorarioBrasilia().Date;

            string comando = $@" update abrangencia as a
                                set historico = true, dt_fim_vinculo = '{dtFimVinculo.Year}-{dtFimVinculo.Month}-{dtFimVinculo.Day}'
                                from abrangencia ab
                                left join turma t on t.id = ab.turma_id
                                where a.id = ab.id
                                and (ab.turma_id is null Or (t.id = ab.turma_id and t.ano_letivo = {anoLetivo}))                                    
                                and a.id in (#ids) ";

            for (int i = 0; i < ids.Count(); i += 900)
            {
                var iteracao = ids.Skip(i).Take(900);
                await database.Conexao.ExecuteAsync(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public async Task ExcluirAbrangencias(IEnumerable<long> ids)
        {
            const string comando = @"delete from public.abrangencia where id in (#ids) and historico = false";

            for (int i = 0; i < ids.Count(); i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);

                await database.Conexao.ExecuteAsync(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public async Task ExcluirAbrangenciasHistoricas(IEnumerable<long> ids)
        {
            const string comando = @"delete from public.abrangencia where id in (#ids) and historico = true";

            for (int i = 0; i < ids.Count(); i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);

                await database.Conexao.ExecuteAsync(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public async Task InserirAbrangencias(IEnumerable<Abrangencia> abrangencias, string login)
        {
            foreach (var item in abrangencias)
            {
                const string comando = @"insert into public.abrangencia (usuario_id, dre_id, ue_id, turma_id, perfil, historico)
                                        values ((select id from usuario where login = @login), @dreId, @ueId, @turmaId, @perfil, @historico)
                                        RETURNING id";

                await database.Conexao.ExecuteAsync(comando,
                    new
                    {
                        login,
                        dreId = item.DreId,
                        ueId = item.UeId,
                        turmaId = item.TurmaId,
                        perfil = item.Perfil,
                        historico = item.Historico
                    });
            }
        }

        public async Task<bool> JaExisteAbrangencia(string login, Guid perfil)
        {
            var query = @"select
                                1
                            from
                                abrangencia
                            where
                                usuario_id = (select id from usuario where login = @login)
                                and perfil = @perfil
                                and abrangencia.historico = false";

            return (await database.Conexao.QueryAsync<bool>(query, new { login, perfil })).FirstOrDefault();
        }

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil, bool consideraHistorico, string[] anosInfantilDesconsiderar = null)
        {
            texto = $"%{(texto ?? "").ToUpper()}%";

            var query = new StringBuilder();

            query.AppendLine("select distinct abt.codigomodalidade modalidade,");
            query.AppendLine("                abt.anoletivo anoLetivo,");
            query.AppendLine("                abt.ano,");
            query.AppendLine("                  dre.dre_id codigoDre,");
            query.AppendLine("                abt.codigo codigoTurma,");
            query.AppendLine("                ue.ue_id codigoUe,");
            query.AppendLine("                dre.nome nomeDre,");
            query.AppendLine("                t.nome nomeTurma,");
            query.AppendLine("                ue.nome nomeUe,");
            query.AppendLine("                t.semestre,");
            query.AppendLine("                  t.qt_duracao_aula qtDuracaoAula,");
            query.AppendLine("                t.tipo_turno tipoTurno,");
            query.AppendLine("                ue.tipo_escola tipoEscola,");
            query.AppendLine("                abt.turma_id TurmaId,");
            query.AppendLine("                abt.ensinoespecial");
            query.AppendLine("    from f_abrangencia_turmas(@login, @perfil, @consideraHistorico, 0, 0, null, 0, @anosInfantilDesconsiderar) abt");
            query.AppendLine("        inner join turma t");
            query.AppendLine("            on abt.codigo = t.turma_id");
            query.AppendLine("        inner join ue");
            query.AppendLine("            on t.ue_id = ue.id");
            query.AppendLine("        inner join dre");
            query.AppendLine("            on ue.dre_id = dre.id");
            query.AppendLine("where upper(abt.nome) like @texto or upper(f_unaccent(ue.nome)) like @texto");
            query.AppendLine("order by ue.nome");

            return await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query.ToString(), new { texto, login, perfil, consideraHistorico, anosInfantilDesconsiderar });
        }

        public Task<IEnumerable<AbrangenciaSinteticaDto>> ObterAbrangenciaSintetica(string login, Guid perfil, string turmaId = "", bool consideraHistorico = false)
        {
            var query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("usuario_id,");
            query.AppendLine("login,");
            query.AppendLine("dre_id,");
            query.AppendLine("codigo_dre,");
            query.AppendLine("ue_id,");
            query.AppendLine("codigo_ue,");
            query.AppendLine("turma_id,");
            query.AppendLine("codigo_turma,");
            query.AppendLine("perfil,");
            query.AppendLine("historico");
            query.AppendLine("from");
            query.AppendLine("public.v_abrangencia_sintetica where login = @login and perfil = @perfil");

            if (consideraHistorico)
                query.AppendLine("and historico = true");
            else query.AppendLine("and historico = false");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and codigo_turma = @turmaId");

            return database.Conexao.QueryAsync<AbrangenciaSinteticaDto>(query.ToString(), new { login, perfil, turmaId });
        }

        public async Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistoricaPorLogin(string login)
        {
            var query = new StringBuilder();

            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("usuario_id,");
            query.AppendLine("login,");
            query.AppendLine("dre_id,");
            query.AppendLine("codigo_dre,");
            query.AppendLine("ue_id,");
            query.AppendLine("codigo_ue,");
            query.AppendLine("turma_id,");
            query.AppendLine("codigo_turma,");
            query.AppendLine("perfil");
            query.AppendLine("from");
            query.AppendLine("public.v_abrangencia_sintetica where login = @login and historico");

            return await database.Conexao.QueryAsync<AbrangenciaHistoricaDto>(query.ToString(), new { login });
        }

        public async Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, string login, Guid perfil, bool consideraHistorico = false, bool abrangenciaPermitida = false)
        {

            var query = new StringBuilder();

            query.AppendLine("select t.modalidade_codigo as modalidade,");
            query.AppendLine("       t.turma_ano_letivo as anoLetivo,");
            query.AppendLine("       t.turma_ano as ano,");
            query.AppendLine("       t.dre_codigo as codigoDre,");
            query.AppendLine("       t.turma_id as codigoTurma,");
            query.AppendLine("       t.ue_codigo as codigoUe,");
            query.AppendLine("       ue.tipo_escola as tipoEscola,");
            query.AppendLine("       t.dre_nome as nomeDre,");
            query.AppendLine("       t.turma_nome as nomeTurma,");
            query.AppendLine("       t.ue_nome as nomeUe,");
            query.AppendLine("       t.turma_semestre as semestre,");
            query.AppendLine("       t.qt_duracao_aula as qtDuracaoAula,");
            query.AppendLine("       t.tipo_turno as tipoTurno,");
            query.AppendLine("       t.tipo_turma as tipoTurma");
            query.AppendLine("from abrangencia a");
            query.AppendLine("  join usuario u");
            query.AppendLine("      on a.usuario_id = u.id");
            query.AppendLine("  inner join v_abrangencia_cadeia_turmas t");
            query.AppendLine("     on (a.turma_id notnull and a.turma_id = t.turma_id) or");
            query.AppendLine("        (a.turma_id is null and a.ue_id is null and a.dre_id = t.dre_id) or-- admin dre");
            query.AppendLine("        (a.turma_id is null and a.dre_id is null and a.ue_id = t.ue_id) --admin ue");
            query.AppendLine("  inner join ue");
            query.AppendLine("      on ue.id = t.ue_id");
            query.AppendLine($"where { (!consideraHistorico ? "not " : string.Empty) } coalesce(nullif(t.turma_historica, false), a.historico)");
            query.AppendLine("  and u.login = @login");
            query.AppendLine("  and a.perfil = @perfil");
            query.AppendLine("  and t.turma_codigo = @turma;");

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query.ToString(), new { turma, login, perfil }))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<int>> ObterAnosLetivos(string login, Guid perfil, bool consideraHistorico, int anoMinimo)
        {
            var anos = (await database.Conexao.QueryAsync<int>(@"select f_abrangencia_anos_letivos(@login, @perfil, @consideraHistorico)
                                                             order by 1 desc", new { login, perfil, consideraHistorico }));
            return anos.Where(a => a >= anoMinimo);
        }

        public async Task<IEnumerable<string>> ObterAnosTurmasPorCodigoUeModalidade(string login, Guid perfil, string codigoUe, Modalidade modalidade, bool consideraHistorico,int? anoLetivo)
        {
           var query = new StringBuilder(@"select distinct act.turma_ano
                                from v_abrangencia_nivel_dre a
                                    inner join v_abrangencia_cadeia_turmas act
                                        on a.dre_id = act.dre_id
                            where a.login = @login and  ");

            if (anoLetivo.HasValue && anoLetivo.Value > 0)
                query.AppendLine(" act.turma_ano_letivo = @anoLetivo and ");

            query.AppendLine(@" a.perfil_id = @perfil and      
                                  act.turma_historica = @consideraHistorico and
                                  act.modalidade_codigo = @modalidade and
                                  (@codigoUe = '-99' or (@codigoUe <> '-99' and act.ue_codigo = @codigoUe))");

            query.AppendLine(@"             union

                            select distinct act.turma_ano
                                from v_abrangencia_nivel_ue a
                                    inner join v_abrangencia_cadeia_turmas act
                                        on a.ue_id = act.ue_id
                            where a.login = @login and ");

            if (anoLetivo.HasValue && anoLetivo.Value > 0)
                query.AppendLine(" act.turma_ano_letivo = @anoLetivo and ");

            query.AppendLine(@"  a.perfil_id = @perfil 
                                  and act.turma_historica = @consideraHistorico 
                                  and act.modalidade_codigo = @modalidade 
                                  and (@codigoUe = '-99' or (@codigoUe <> '-99' and act.ue_codigo = @codigoUe)) 
                                  and
                                  (
                                    (@perfil <> '4ee1e074-37d6-e911-abd6-f81654fe895d') 
                                    or
                                    (@consideraHistorico = true and  @perfil = '4ee1e074-37d6-e911-abd6-f81654fe895d' 
                                       and (
                                            act.dre_id in (select dre_id from v_abrangencia_nivel_dre where login = @login and historico = false) 
                                            or 
                                            act.ue_id in (select ue_id from v_abrangencia_nivel_ue where login = @login and historico = false)
                                            )
                                    ) 
                                    or 
                                    (@consideraHistorico = false and @perfil = '4ee1e074-37d6-e911-abd6-f81654fe895d' and a.historico = false))");

            query.AppendLine(@" union
                            select distinct act.turma_ano
                                from v_abrangencia_nivel_turma a
                                    inner join v_abrangencia_cadeia_turmas act
                                        on a.turma_id = act.turma_id
                            where a.login = @login and  ");

            if (anoLetivo.HasValue && anoLetivo.Value > 0)
                query.AppendLine(" act.turma_ano_letivo = @anoLetivo and ");
            
            query.AppendLine(@"  a.perfil_id = @perfil and
                                  act.modalidade_codigo = @modalidade and
                                  (@codigoUe = '-99' or (@codigoUe <> '-99' and act.ue_codigo = @codigoUe)) and
                                  ((@consideraHistorico = true and a.historico = true) or
                                   (@consideraHistorico = false and a.historico  = false and act.turma_historica = false)); ");


            return (await database.Conexao.QueryAsync<string>(query.ToString(), new {login, perfil, codigoUe, modalidade = (int) modalidade, consideraHistorico,anoLetivo}));
        }

        public async Task<AbrangenciaDreRetornoDto> ObterDre(string dreCodigo, string ueCodigo, string login, Guid perfil)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.dre_codigo as codigo,");
            query.AppendLine("va.dre_nome as nome,");
            query.AppendLine("va.dre_abreviacao as abreviacao");
            query.AppendLine("from");
            query.AppendLine("v_abrangencia_usuario va");
            query.AppendLine("where va.login = @login");
            query.AppendLine("and va.usuario_perfil = @perfil");
            query.AppendLine("and va.dre_codigo is not null");

            if (!string.IsNullOrEmpty(dreCodigo))
                query.AppendLine("and va.dre_codigo = @dreCodigo");

            if (!string.IsNullOrEmpty(ueCodigo))
                query.AppendLine("and va.ue_codigo = @ueCodigo");

            return (await database.Conexao.QueryFirstOrDefaultAsync<AbrangenciaDreRetornoDto>(query.ToString(), new { dreCodigo, ueCodigo, login, perfil }));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "", bool filtroEhCodigo = false)
        {
            var query = new StringBuilder();
            query.AppendLine("select distinct abreviacao, ");
            query.AppendLine("codigo,");
            query.AppendLine("nome,");
            query.AppendLine("dre_id as id");
            query.AppendLine("from f_abrangencia_dres(@login , @perfil, @consideraHistorico, @modalidade, @semestre, @anoLetivo)");

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = $"%{filtro.ToUpper()}%";

                if (filtroEhCodigo)
                    query.AppendLine("where upper(codigo) like @filtro");
                else
                    query.AppendLine("where upper(nome) like @filtro");

                query.AppendLine("limit 10;");
            }

            var parametros = new
            {
                login,
                perfil,
                consideraHistorico,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                anoLetivo,
                filtro
            };

            var retorno = await database.Conexao
                .QueryAsync<AbrangenciaDreRetornoDto>(query.ToString(), parametros);

            if (perfil == Perfis.PERFIL_SUPERVISOR)
                retorno = await AcrescentarDresSupervisor(login, modalidade ?? 0, periodo, consideraHistorico, anoLetivo, retorno);

            return retorno;
        }

        public async Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil, int anoLetivo, bool consideraHistorico, IEnumerable<Modalidade> modalidadesQueSeraoIgnoradas)
        {
            var query = @"select f_abrangencia_modalidades(@login, @perfil, @consideraHistorico, @anoLetivo, @modalidadesQueSeraoIgnoradas) order by 1";
            var modalidadesQueSeraoIgnoradasArray = modalidadesQueSeraoIgnoradas?.Select(x => (int)x).ToArray();
            var retorno = await database.Conexao
                .QueryAsync<int>(query, new { login, perfil, consideraHistorico, anoLetivo, modalidadesQueSeraoIgnoradas = modalidadesQueSeraoIgnoradasArray });

            if (perfil == Perfis.PERFIL_SUPERVISOR)
            {
                retorno = await AcrescentarModalidadesSupervisor(login, consideraHistorico, anoLetivo, retorno);
                if (modalidadesQueSeraoIgnoradas.NaoEhNulo() && modalidadesQueSeraoIgnoradas.Any())
                    retorno = retorno.Where(r => !modalidadesQueSeraoIgnoradas.Contains((Modalidade)r));
            }

            return retorno
                .Distinct()
                .OrderByDescending(r => r);
        }

        public async Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0, string dreCodigo = null, string ueCodigo = null)
        {
            var parametros = new { login, perfil, consideraHistorico, modalidade, anoLetivo, dreCodigo, ueCodigo };

            var retorno = await database.Conexao
                .QueryAsync<int>(@"select f_abrangencia_semestres(@login, @perfil, @consideraHistorico, @modalidade, @anoLetivo, @dreCodigo, @ueCodigo)
                                   order by 1", parametros);

            if (perfil == Perfis.PERFIL_SUPERVISOR)
                retorno = await AcrescentarSemestresSupervisor(login, modalidade, consideraHistorico, anoLetivo, retorno);

            return retorno
                .Distinct()
                .OrderBy(s => s);
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var query = @"select ano,
                                 anoLetivo,
                                 codigo,
                                 codigoModalidade,
                                 nome,
                                 semestre,
                                 qtDuracaoAula,
                                 tipoTurno,
                                 ensinoEspecial,
                                 turma_id as id,
                                 nome_filtro as nomeFiltro
                            from f_abrangencia_turmas(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @codigoUe, @anoLetivo)
                          order by 5";

            return await database.Conexao.QueryAsync<AbrangenciaTurmaRetorno>(query.ToString(), new { login, perfil, consideraHistorico, modalidade, semestre = periodo, codigoUe, anoLetivo });
        }

        public async Task<AbrangenciaUeRetorno> ObterUe(string codigo, string login, Guid perfil)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.ue_codigo as codigo,");
            query.AppendLine("va.ue_nome as nome,");
            query.AppendLine("u.tipo_escola as tipoEscola");
            query.AppendLine("from");
            query.AppendLine("v_abrangencia_usuario va");
            query.AppendLine("inner join ue u");
            query.AppendLine("on u.ue_id = va.ue_codigo");
            query.AppendLine("where");
            query.AppendLine("va.ue_codigo = @codigo");
            query.AppendLine("and va.login = @login");
            query.AppendLine("and va.usuario_perfil = @perfil");

            return (await database.Conexao.QueryFirstOrDefaultAsync<AbrangenciaUeRetorno>(query.ToString(), new { codigo, login, perfil }));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, int[] ignorarTiposUE = null, string filtro = "", bool filtroEhCodigo = false)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct codigo,");
            query.AppendLine("nome as NomeSimples,");
            query.AppendLine("tipoescola,");
            query.AppendLine("ue_id as id");
            query.AppendLine("from f_abrangencia_ues(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @codigoDre, @anoLetivo, @ignorarTiposUE)");

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = $"%{filtro.ToUpper()}%";

                if (filtroEhCodigo)
                    query.AppendLine("where upper(codigo) like @filtro");
                else
                    query.AppendLine("where upper(nome) like @filtro");

                query.AppendLine("order by 2");
                query.AppendLine("limit 10;");
            }
            else
                query.AppendLine("order by 2;");

            var parametros = new
            {
                login,
                perfil,
                consideraHistorico,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                codigoDre,
                anoLetivo,
                ignorarTiposUE,
                filtro
            };

            var retorno = await database.Conexao
                .QueryAsync<AbrangenciaUeRetorno>(query.ToString(), parametros);

            if (perfil == Perfis.PERFIL_SUPERVISOR)
            {
                retorno = await AcrescentarUesSupervisor(login, modalidade ?? 0, periodo, codigoDre, consideraHistorico, anoLetivo, ignorarTiposUE, retorno);
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    filtro = filtro.Replace("%", string.Empty);
                    retorno = filtroEhCodigo ?
                        retorno.Where(r => r.Codigo.ToUpper().Contains(filtro)).Take(10) :
                        retorno.Where(r => r.NomeSimples.ToUpper().Contains(filtro)).Take(10);
                }
            }

            return retorno;
        }

        public bool PossuiAbrangenciaTurmaAtivaPorLogin(string login, bool cj = false)
        {
            var sql = $@"select count(*) from usuario u
                         inner join abrangencia a on a.usuario_id = u.id
                         inner join turma t on a.turma_id = t.id
                         where u.login = @login and historico = false and t.turma_id is not null
                              and {(cj ? string.Empty : "not")} a.perfil = ANY(@perfisCJ) and t.ano_letivo = @anoLetivo;";

            var parametros = new { login, perfisCJ = new Guid[] { Perfis.PERFIL_CJ, Perfis.PERFIL_CJ_INFANTIL }, anoLetivo = DateTime.Now.Year };

            return database.Conexao.QueryFirstOrDefault<int>(sql, parametros) > 0;
        }

        public bool PossuiAbrangenciaTurmaInfantilAtivaPorLogin(string login, bool cj = false)
        {
            var sql = @"select count(*) from usuario u
                        inner join abrangencia a on a.usuario_id = u.id
                        inner join turma t on a.turma_id = t.id
                        where u.login = @login and historico = false and t.turma_id is not null
                              and a.perfil = @perfilINFANTIL and t.ano_letivo = @anoLetivo;";

            var parametros = new { login, perfilINFANTIL = cj ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_PROFESSOR_INFANTIL, anoLetivo = DateTime.Now.Year };

            return database.Conexao.QueryFirstOrDefault<int>(sql, parametros) > 0;
        }

        public async Task RemoverAbrangenciasForaEscopo(string login, Guid perfil, TipoAbrangenciaSincronizacao escopo)
        {
            var query = "delete from abrangencia where usuario_id = (select id from usuario where login = @login) and historico = false and perfil = @perfil and #escopo";

            switch (escopo)
            {
                case TipoAbrangenciaSincronizacao.PorDre:
                    query = query.Replace("#escopo", " ue_id is not null and turma_id is not null");
                    break;

                case TipoAbrangenciaSincronizacao.PorUe:
                    query = query.Replace("#escopo", " dre_id is not null and turma_id is not null");
                    break;

                case TipoAbrangenciaSincronizacao.PorTurma:
                    query = query.Replace("#escopo", " ue_id is not null and dre_id is not null");
                    break;
            }

            await database.Conexao.ExecuteAsync(query, new { login, perfil });
        }

        public async Task<bool> UsuarioPossuiAbrangenciaAdm(long usuarioId)
        {
            var query = "select 1 from abrangencia where usuario_id = @usuarioId and turma_id is null";

            return (await database.Conexao.QueryAsync<int>(query, new { usuarioId })).Count() > 0;
        }

        public async Task<bool> UsuarioPossuiAbrangenciaDeUmDosTipos(Guid perfil, IEnumerable<TipoPerfil> tipos)
        {
            var query = "select 1 from prioridade_perfil where codigo_perfil = @perfil and tipo = any(@tipos)";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { perfil, tipos = tipos?.Select(c => (int)c)?.ToArray() });
        }
        public async Task<IEnumerable<Modalidade>> ObterModalidadesPorUe(string codigoUe)
        {
            var query = @"select
                                distinct t.modalidade_codigo
                            from
                                turma t
                            inner join ue u on
                                t.ue_id = u.id
                            where
                                u.ue_id = @codigoUe  and t.modalidade_codigo <> 0";

            return await database.Conexao.QueryAsync<Modalidade>(query, new { codigoUe });
        }

        public async Task<IEnumerable<Modalidade>> ObterModalidadesPorCodigosUe(string[] codigosUe)
        {
            var query = @"select
                                distinct t.modalidade_codigo
                            from
                                turma t
                            inner join ue u on
                                t.ue_id = u.id
                            where
                                u.ue_id = any(@codigosUe)";

            return await database.Conexao.QueryAsync<Modalidade>(query, new { codigosUe });
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestre(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, string[] anosInfantilDesconsiderar = null)
        {
            var query = new StringBuilder();

            query.AppendLine(@"select t.turma_id as valor, coalesce(t.nome_filtro, t.nome) as descricao from turma t
                            inner join ue ue on ue.id = t.ue_id");

            query.AppendLine("where ue.ue_id = @codigoUe and ano_letivo = @anoLetivo");

            if (modalidade.HasValue && modalidade != 0)
                query.AppendLine("and t.modalidade_codigo = @modalidade");

            if (semestre > 0)
                query.AppendLine("and semestre = @semestre");

            if (anosInfantilDesconsiderar.NaoEhNulo() && anosInfantilDesconsiderar.Any())
            {
                query.AppendLine("and t.ano <> ALL(@anosInfantilDesconsiderar)");
            }

            var dados = await database.Conexao.QueryAsync<OpcaoDropdownDto>(query.ToString(), new { codigoUe, anoLetivo, modalidade, semestre, anosInfantilDesconsiderar });

            return dados.OrderBy(x => x.Descricao);
        }

        public async Task<IEnumerable<Modalidade>> ObterModalidadesPorUeAbrangencia(string codigoUe, string login, Guid perfilAtual, IEnumerable<Modalidade> modadlidadesQueSeraoIgnoradas, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var query = "";
            if (consideraHistorico)
                query = @"select f_abrangencia_modalidades(@login, @perfilAtual, true, @anoLetivo, @modalidadesQueSeraoIgnoradasArray::int4[], @codigoUe) modalidade_codigo order by 1";
            else
            {
                query = @"select distinct vau.modalidade_codigo from v_abrangencia_usuario vau 
                            where vau.login = @login
                            and usuario_perfil  = @perfilAtual
                            and vau.ue_codigo = @codigoUe
                            and (@modalidadesQueSeraoIgnoradasArray::int4[] is null or not(vau.modalidade_codigo = ANY(@modalidadesQueSeraoIgnoradasArray::int4[])))";
            }

            var modalidadesQueSeraoIgnoradasArray = modadlidadesQueSeraoIgnoradas?.Select(x => (int)x).ToArray();
            return await database.Conexao.QueryAsync<Modalidade>(query, new { codigoUe, login, perfilAtual, modalidadesQueSeraoIgnoradasArray, anoLetivo = anoLetivo > 0 ? anoLetivo : DateTime.Today.Year }, commandTimeout: 60);
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestreAnos(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, IList<string> anos)
        {
            var query = new StringBuilder();
            query.AppendLine(@"select distinct t.turma_id as valor, t.nome as descricao from turma t
                            inner join ue ue on ue.id = t.ue_id
                            inner join tipo_ciclo_ano tca on tca.ano = t.ano ");

            query.AppendLine("where ue.ue_id = @codigoUe and ano_letivo = @anoLetivo ");

            if (modalidade.HasValue && modalidade != 0)
                query.AppendLine("and t.modalidade_codigo = @modalidade ");

            if (semestre > 0)
                query.AppendLine("and semestre = @semestre ");

            if (anos.NaoEhNulo() && anos.Any())
                query.AppendLine(" and tca.ano IN (#anos)");

            var dados = await database.Conexao.QueryAsync<OpcaoDropdownDto>(query.ToString().Replace("#anos", "'" + string.Join("','", anos) + "'"), new { codigoUe, anoLetivo, modalidade, semestre, anos });

            return dados.OrderBy(x => x.Descricao);
        }

        private static void AdicionarCondicionalModalidadesSemEJAObterTurmas(StringBuilder query, int[] modalidades)
        {
            if (modalidades.Any() && !modalidades.Any(c => c == -99))
                query.AppendLine("and t.modalidade_codigo = any(@modalidadesSemEja) ");
        }

        private static void AdicionarCondicionalAnosObterTurmas(StringBuilder query, string[] anos, int[] modalidades)
        {
            if (anos.PossuiRegistros() && !anos.Any(a => a == "-99") 
                && modalidades.NaoPossuiRegistros(a => a == (int)Modalidade.MOVA))
                query.AppendLine(" and tca.ano = any(@anos)");
        }

        public async Task<IEnumerable<DropdownTurmaRetornoDto>> ObterTurmasPorAnoLetivoUeModalidadeSemestreEAnosEscolares(int anoLetivo, string codigoUe, int[] modalidades, int semestre, string[] anos, bool historico)
        {
            var query = new StringBuilder();
            var modalidadeEja = 0;
            var modalidadesSemEja = FiltrarModalidadesExcetoEJA(modalidades);

            if (modalidadesSemEja.Any())
            {
                query.AppendLine(@"select distinct t.turma_id as valor, 
                                                   coalesce(t.nome_filtro, t.nome) as descricao,
                                                   t.modalidade_codigo as modalidade
                                   from turma t
                                   inner join ue ue on ue.id = t.ue_id
                                   inner join tipo_ciclo_ano tca on tca.ano = t.ano 
                                   where ano_letivo = @anoLetivo
                                         and ue.ue_id = @codigoUe and t.historica = @historico ");
                AdicionarCondicionalModalidadesSemEJAObterTurmas(query, modalidades);
                AdicionarCondicionalAnosObterTurmas(query, anos, modalidadesSemEja);

                if (anos.Any(a => a == "-99"))
                {
                    query.AppendLine(@"union");
                    query.AppendLine(@"select distinct t.turma_id as valor, 
                                                   coalesce(t.nome_filtro, t.nome) as descricao,
                                                   t.modalidade_codigo as modalidade
                                       from turma t
                                       inner join ue ue on ue.id = t.ue_id
                                       where ano_letivo = @anoLetivo
                                             and ue.ue_id = @codigoUe and t.historica = @historico  and t.ano = '0' and t.tipo_turma = 3 ");
                    AdicionarCondicionalModalidadesSemEJAObterTurmas(query, modalidades);
                }

            }

            if (modalidadesSemEja.Any() && modalidades.Any(m => (Modalidade)m == Modalidade.EJA))
                query.AppendLine(" union ");

            if (modalidades.Any(m => (Modalidade)m == Modalidade.EJA))
            {
                modalidadeEja = (int)Modalidade.EJA;

                query.AppendLine(@"select distinct t.turma_id as valor, 
                                                   coalesce(t.nome_filtro, t.nome) as descricao,
                                                   t.modalidade_codigo as modalidade
                                              from turma t
                                             inner join ue ue on ue.id = t.ue_id
                                             inner join tipo_ciclo_ano tca on tca.ano = t.ano
                                             where ano_letivo = @anoLetivo
                                               and ue.ue_id = @codigoUe 
                                               and t.modalidade_codigo = @modalidadeEja 
                                               and semestre = @semestre and t.historica = @historico ");
                AdicionarCondicionalAnosObterTurmas(query, anos, null);
            }

            var parametros = new
            {
                codigoUe,
                anoLetivo,
                modalidadesSemEja,
                semestre,
                anos,
                modalidadeEja,
                historico
            };

            var dados = await database.Conexao.QueryAsync<DropdownTurmaRetornoDto>(query.ToString(), parametros);
            return dados.OrderBy(x => x.Descricao);
        }

        private static int[] FiltrarModalidadesExcetoEJA(int[] modalidades)
        {
            if (modalidades.Any() && !modalidades.Any(c => c == -99))
                return modalidades.Where(m => (Modalidade)m != Modalidade.EJA).ToArray();
            return new int[] { };
        }

        public async Task<bool> ObterUsuarioPossuiAbrangenciaAcessoSondagemTiposEscola(string usuarioRF, Guid usuarioPerfil)
        {
            var query = @"select 1 from abrangencia a 
                            inner join usuario u 
                                on u.id = a.usuario_id 
                            inner join ue ue 
                                on ue.id = a.ue_id 
                        where u.rf_codigo  = @usuarioRF
                            and a.perfil  = @usuarioPerfil 
                            and ue.tipo_escola in (1,3,4,16)";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { usuarioRF, usuarioPerfil });
        }
        public async Task<IEnumerable<Abrangencia>> ObterAbrangenciaGeralPorUsuarioId(long usuarioId)
        {
            var query = @"select id,usuario_id,dre_id,ue_id,turma_id,perfil,historico from abrangencia where usuario_id = @usuarioId";
            return await database.Conexao.QueryAsync<Abrangencia>(query, new { usuarioId });
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPorTipos(string codigoUe, string login, Guid perfil, Modalidade modalidade, int[] tipos, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string[] anosInfantilDesconsiderar = null)
        {
            var query = @"select ano,
                                 anoLetivo,
                                 codigo,
                                 codigoModalidade,
                                 nome,
                                 semestre,
                                 qtDuracaoAula,
                                 tipoTurno,
                                 ensinoEspecial,
                                 turma_id as id,
                                 tipoturma,
                                 nome_filtro as nomeFiltro 
                            from f_abrangencia_turmas_tipos(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @codigoUe, @anoLetivo, @tipos, @anosInfantilDesconsiderar)
                          order by 5";

            var resultado = await database.Conexao
                .QueryAsync<AbrangenciaTurmaRetorno>(query.ToString(), new { login, perfil, consideraHistorico, modalidade, semestre = periodo, codigoUe, anoLetivo, tipos, anosInfantilDesconsiderar });

            var resultadoFiltrado = resultado.GroupBy(x => x.Codigo).SelectMany(y => y.OrderBy(a => a.Codigo).Take(1));

            if (perfil == Perfis.PERFIL_SUPERVISOR)
            {
                resultadoFiltrado = await AcrescentarTurmasSupervisor(login, modalidade, periodo, codigoUe, consideraHistorico, anoLetivo, resultadoFiltrado);

                if (tipos.NaoEhNulo() && tipos.Any())
                    resultadoFiltrado = resultadoFiltrado.Where(r => tipos.Contains(r.TipoTurma));

                if (anosInfantilDesconsiderar.NaoEhNulo() && anosInfantilDesconsiderar.Any())
                    resultadoFiltrado = resultadoFiltrado.Where(r => !anosInfantilDesconsiderar.Contains(r.Ano));
            }

            return resultadoFiltrado.DistinctBy(p => new {p.Codigo, p.Nome});
        }
        
         public async Task<bool> VerificarUsuarioLogadoPertenceMesmaUE(string codigoUe, string login, Guid perfil, Modalidade modalidade, int anoLetivo, int periodo, bool consideraHistorico = false)
        {
            var query = @"select 1  from f_abrangencia_turmas(@login, @perfil, @consideraHistorico, @modalidade, @periodo, @codigoUe, @anoLetivo) limit 1";

            var retorno = await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { login, perfil, consideraHistorico, modalidade, periodo, codigoUe, anoLetivo});

            return retorno;
        }

        public async Task<IEnumerable<string>> ObterLoginsAbrangenciaUePorPerfil(long ueId, Guid perfil, bool historica = false)
        {
            var sqlQuery = @"select distinct u.login
                                from abrangencia a 
                                    inner join usuario u
                                        on a.usuario_id = u.id
                             where a.ue_id = @ueId and
                                   a.historico = @historica and      
                                   a.perfil = @perfil;";

            return await database.Conexao.QueryAsync<string>(sqlQuery, new { ueId, perfil, historica });
        }

        public async Task<IEnumerable<string>> ObterProfessoresTurmaPorAbrangencia(string turmaCodigo)
        {
            var sqlQuery = @"select distinct (u.rf_codigo) from usuario u 
                            inner join abrangencia a on a.usuario_id = u.id 
                            inner join turma t on t.id = a.turma_id 
                            where t.turma_id = @turmaCodigo and not a.historico 
                            and (a.perfil = @professor or a.perfil = @professorInfantil or 
                            a.perfil = @professorCJ or a.perfil = @professorCJInfantil);";

            Guid professor = Guid.Parse(PerfilUsuario.PROFESSOR.Name());
            Guid professorInfantil = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.Name());
            Guid professorCJ = Guid.Parse(PerfilUsuario.CJ.Name());
            Guid professorCJInfantil = Guid.Parse(PerfilUsuario.CJ_INFANTIL.Name());

            return await database.Conexao.QueryAsync<string>(sqlQuery, new { turmaCodigo, professor, professorInfantil, professorCJ, professorCJInfantil });
        }

        private async Task<IEnumerable<DadosAbrangenciaSupervisorDto>> ObterDadosAbrangenciaSupervisor(string login, bool consideraHistorico, int anoLetivo)
        {
            return await repositorioSupervisorEscolaDre
                .ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);
        }

        private async Task<IEnumerable<int>> AcrescentarModalidadesSupervisor(string login, bool consideraHistorico, int anoLetivo, IEnumerable<int> retorno)
        {
            var dadosAbrangenciaSupervisor =
                await ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                retorno = retorno.Concat(dadosAbrangenciaSupervisor
                    .Select(d => d.Modalidade)
                    .Distinct());
            }

            return retorno;
        }

        private async Task<IEnumerable<int>> AcrescentarSemestresSupervisor(string login, Modalidade modalidade, bool consideraHistorico, int anoLetivo, IEnumerable<int> retorno)
        {
            var dadosAbrangenciaSupervisor =
                await ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                retorno = retorno.Concat(dadosAbrangenciaSupervisor
                    .Where(d => (Modalidade)d.Modalidade == modalidade)
                    .Select(d => d.Semestre)
                    .Distinct());
            }

            return retorno;
        }

        private async Task<IEnumerable<AbrangenciaDreRetornoDto>> AcrescentarDresSupervisor(string login, Modalidade modalidade, int semestre, bool consideraHistorico, int anoLetivo, IEnumerable<AbrangenciaDreRetornoDto> retorno)
        {
            var dadosAbrangenciaSupervisor =
                await ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                var dres = retorno.Select(d => d.Id).ToList();

                var dresComplementares = (from da in dadosAbrangenciaSupervisor
                                          where (modalidade == 0 || (Modalidade)da.Modalidade == modalidade) &&
                                                (semestre == 0 || (semestre > 0 && da.Semestre == semestre)) &&
                                                !dres.Contains(da.DreId)
                                          select new
                                          {
                                              da.AbreviacaoDre,
                                              da.CodigoDre,
                                              da.DreNome,
                                              da.DreId
                                          }).Distinct();

                var listaDistinta = dresComplementares
                    .Select(d => new AbrangenciaDreRetornoDto()
                    {
                        Abreviacao = d.AbreviacaoDre,
                        Codigo = d.CodigoDre,
                        Id = d.DreId,
                        Nome = d.DreNome
                    });

                retorno = retorno
                    .Concat(listaDistinta)
                    .OrderBy(d => d.Codigo);
            }

            return retorno;
        }

        private async Task<IEnumerable<AbrangenciaUeRetorno>> AcrescentarUesSupervisor(string login, Modalidade modalidade, int semestre, string dre, bool consideraHistorico, int anoLetivo, int[] tiposEscolasIgnoradas, IEnumerable<AbrangenciaUeRetorno> retorno)
        {
            var retornoUesSupervisor = new List<AbrangenciaUeRetorno>();   
            var dadosAbrangenciaSupervisor =
                await ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);

            if(retorno.Any())
                retornoUesSupervisor.AddRange(retorno);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                var ues = retorno.Select(u => u.Id).ToList();
                var uesComplementares = (from da in dadosAbrangenciaSupervisor select new { da.CodigoUe, da.UeNome, da.TipoEscola, da.UeId });

                if (modalidade > 0)
                {
                    uesComplementares = (from da in dadosAbrangenciaSupervisor
                                         where (Modalidade)da.Modalidade == modalidade &&
                                               da.CodigoDre == dre &&
                                               !tiposEscolasIgnoradas.Contains((int)da.TipoEscola) &&
                                               (semestre == 0 || (semestre > 0 && da.Semestre == semestre)) &&
                                               !ues.Contains(da.UeId)
                                         select new
                                         {
                                             da.CodigoUe,
                                             da.UeNome,
                                             da.TipoEscola,
                                             da.UeId
                                         }).Distinct();
                }
                else
                {
                    uesComplementares = (from da in dadosAbrangenciaSupervisor
                                         where da.CodigoDre == dre &&
                                               !tiposEscolasIgnoradas.Contains((int)da.TipoEscola) &&
                                               (semestre == 0 || (semestre > 0 && da.Semestre == semestre)) &&
                                               !ues.Contains(da.UeId)
                                         select new
                                         {
                                             da.CodigoUe,
                                             da.UeNome,
                                             da.TipoEscola,
                                             da.UeId
                                         }).Distinct();
                }

                if (uesComplementares.Any())
                {
                    var listaDistinta = uesComplementares
                                                      .Select(u => new AbrangenciaUeRetorno()
                                                      {
                                                          Codigo = u.CodigoUe,
                                                          NomeSimples = u.UeNome,
                                                          TipoEscola = u.TipoEscola,
                                                          Id = u.UeId
                                                      });

                    retornoUesSupervisor.AddRange(listaDistinta);
                }

            }

            return retornoUesSupervisor.Distinct().OrderBy(r=> r.Nome);
        }

        private async Task<IEnumerable<AbrangenciaTurmaRetorno>> AcrescentarTurmasSupervisor(string login, Modalidade modalidade, int semestre, string ue, bool consideraHistorico, int anoLetivo, IEnumerable<AbrangenciaTurmaRetorno> retorno)
        {
            var dadosAbrangenciaSupervisor =
                await ObterDadosAbrangenciaSupervisor(login, consideraHistorico, anoLetivo);

            if (dadosAbrangenciaSupervisor.NaoEhNulo() && dadosAbrangenciaSupervisor.Any())
            {
                var turmas = retorno.Select(t => t.Id).ToList();

                var turmasComplementares = (from da in dadosAbrangenciaSupervisor
                                            where (Modalidade)da.Modalidade == modalidade &&
                                                  da.CodigoUe == ue &&
                                                  (semestre == 0 || (semestre > 0 && da.Semestre == semestre)) &&
                                                  !turmas.Contains(da.TurmaId)
                                            select new AbrangenciaTurmaRetorno
                                            {
                                                NomeFiltro = da.NomeFiltro,
                                                Ano = da.TurmaAno,
                                                AnoLetivo = da.TurmaAnoLetivo,
                                                Codigo = da.CodigoTurma,
                                                CodigoModalidade = da.Modalidade,
                                                Nome = da.TurmaNome,
                                                Semestre = da.Semestre,
                                                EnsinoEspecial = da.EnsinoEspecial,
                                                Id = da.TurmaId,
                                                TipoTurma = da.TipoTurma
                                            });

                retorno = retorno
                    .Concat(turmasComplementares)
                    .OrderBy(d => d.Nome);
            }

            return retorno;
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorAbrangenciaCPParaCopiaAvaliacao(int anoLetivo, string codigoRf, int modalidadeTurma, string anoTurma, long turmaIdReferencia)
        {
            var query = $@"select t.* from abrangencia a
                                inner join usuario u on u.id = a.usuario_id
                                inner join turma t on t.ue_id = a.ue_id
                                where a.perfil = @perfilCP and t.ano_letivo = @anoLetivo
                                and u.rf_codigo = @codigoRf and t.modalidade_codigo = @modalidadeTurma
                                and t.ano = @anoTurma and t.id != @turmaIdReferencia";

            return await database.Conexao.QueryAsync<Turma>(query, new { perfilCP = Guid.Parse(PerfilUsuario.CP.Name().ToString()), anoLetivo, codigoRf, modalidadeTurma, anoTurma, turmaIdReferencia });
        }

        public async Task<bool> VerificaSeUsuarioPossuiAbrangencia(string usuarioRf, Guid perfil)
        {
            var query = @"select
	                        count(a.id) > 0
                        from
	                        abrangencia a
                        inner join usuario u on
	                        u.id = a.usuario_id
                        where
	                        u.rf_codigo = @usuarioRf and a.perfil = @perfil";

            var retorno = await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { usuarioRf, perfil });

            return retorno;
        }
    }
}
