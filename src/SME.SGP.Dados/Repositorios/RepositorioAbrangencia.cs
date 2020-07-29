using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAbrangencia : IRepositorioAbrangencia
    {
        protected readonly ISgpContext database;

        public RepositorioAbrangencia(ISgpContext database)
        {
            this.database = database;
        }

        public void AtualizaAbrangenciaHistorica(IEnumerable<long> ids)
        {
            var dtFimVinculo = DateTime.Today;

            string comando = $"update public.abrangencia set historico = true , dt_fim_vinculo = '{dtFimVinculo.Year}-{dtFimVinculo.Month}-{dtFimVinculo.Day}'  where id in (#ids)";

            for (int i = 0; i < ids.Count(); i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);

                database.Conexao.Execute(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public void ExcluirAbrangencias(IEnumerable<long> ids)
        {
            const string comando = @"delete from public.abrangencia where id in (#ids) and historico = false";

            for (int i = 0; i < ids.Count(); i = i + 900)
            {
                var iteracao = ids.Skip(i).Take(900);

                database.Conexao.Execute(comando.Replace("#ids", string.Join(",", iteracao.Concat(new long[] { 0 }))));
            }
        }

        public void InserirAbrangencias(IEnumerable<Abrangencia> abrangencias, string login)
        {
            foreach (var item in abrangencias)
            {
                const string comando = @"insert into public.abrangencia (usuario_id, dre_id, ue_id, turma_id, perfil)
                                        values ((select id from usuario where login = @login), @dreId, @ueId, @turmaId, @perfil)
                                        RETURNING id";

                database.Conexao.Execute(comando,
                    new
                    {
                        login,
                        dreId = item.DreId,
                        ueId = item.UeId,
                        turmaId = item.TurmaId,
                        perfil = item.Perfil
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

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil, bool consideraHistorico)
        {
            texto = $"%{texto.ToUpper()}%";

            var query = new StringBuilder();

            query.AppendLine("select distinct modalidade,");
            query.AppendLine("                anoLetivo,");
            query.AppendLine("                ano,");
            query.AppendLine("                codigoDre,");
            query.AppendLine("                codigoTurma,");
            query.AppendLine("                codigoUe,");
            query.AppendLine("                ue.tipo_escola as tipoEscola,");
            query.AppendLine("                nomeDre,");
            query.AppendLine("                nomeTurma,");
            query.AppendLine("                nomeUe,");
            query.AppendLine("                semestre,");
            query.AppendLine("                qtDuracaoAula,");
            query.AppendLine("                tipoTurno");
            query.AppendLine("from(");
            query.AppendLine("         select coalesce(t_tur.modalidade_codigo, t_ue.modalidade_codigo, t_dre.modalidade_codigo) as modalidade,");
            query.AppendLine("                coalesce(t_tur.turma_ano_letivo, t_ue.turma_ano_letivo, t_dre.turma_ano_letivo) as anoLetivo,");
            query.AppendLine("                coalesce(t_tur.turma_ano, t_ue.turma_ano, t_dre.turma_ano) as ano,");
            query.AppendLine("                coalesce(t_tur.dre_codigo, t_ue.dre_codigo, t_dre.dre_codigo) as codigoDre,");
            query.AppendLine("                coalesce(t_tur.turma_id, t_ue.turma_id, t_dre.turma_id) as codigoTurma,");
            query.AppendLine("                coalesce(t_tur.ue_codigo, t_ue.ue_codigo, t_dre.ue_codigo) as codigoUe,");
            query.AppendLine("                coalesce(t_tur.dre_nome, t_ue.dre_nome, t_dre.dre_nome) as nomeDre,");
            query.AppendLine("                coalesce(t_tur.turma_nome, t_ue.turma_nome, t_dre.turma_nome) as nomeTurma,");
            query.AppendLine("                coalesce(t_tur.ue_nome, t_ue.ue_nome, t_dre.ue_nome) as nomeUe,");
            query.AppendLine("                coalesce(t_tur.turma_semestre, t_ue.turma_semestre, t_dre.turma_semestre) as semestre,");
            query.AppendLine("                coalesce(t_tur.qt_duracao_aula, t_ue.qt_duracao_aula, t_dre.qt_duracao_aula) as qtDuracaoAula,");
            query.AppendLine("                coalesce(t_tur.tipo_turno, t_ue.tipo_turno, t_dre.tipo_turno) as tipoTurno");
            query.AppendLine("         from abrangencia a");
            query.AppendLine("                  join usuario u on a.usuario_id = u.id");
            query.AppendLine("                  left join v_abrangencia_cadeia_turmas t_tur");
            query.AppendLine("              on (a.turma_id notnull and a.turma_id = t_tur.turma_id)");
            query.AppendLine("                  left join v_abrangencia_cadeia_turmas t_dre");
            query.AppendLine("                            on(a.turma_id is null and a.ue_id is null and a.dre_id = t_dre.dre_id)-- admin dre");
            query.AppendLine("                  left join v_abrangencia_cadeia_turmas t_ue");
            query.AppendLine("                            on(a.turma_id is null and a.dre_id is null and a.ue_id = t_ue.ue_id)-- admin ue");
            query.AppendLine($"         where { (!consideraHistorico ? " not " : string.Empty) } a.historico");
            query.AppendLine("           and u.login = @login");
            query.AppendLine("           and a.perfil = @perfil");
            query.AppendLine("     ) t");
            query.AppendLine("         inner join ue on ue.ue_id = t.codigoUe");
            query.AppendLine("where upper(t.nomeTurma) like @texto OR upper(f_unaccent(t.nomeUe)) LIKE @texto");
            query.AppendLine("order by nomeUe");
            query.AppendLine("limit 10;");

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query.ToString(), new { texto, login, perfil })).AsList();
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
            query.AppendLine("perfil");
            query.AppendLine("from");
            query.AppendLine("public.v_abrangencia_sintetica where login = @login and perfil = @perfil");

            if (consideraHistorico)
                query.AppendLine("and historico = true");
            else query.AppendLine("and historico = false");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and codigo_turma = @turmaId");

            return database.Conexao.QueryAsync<AbrangenciaSinteticaDto>(query.ToString(), new { login, perfil, turmaId });
        }

        public async Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, string login, Guid perfil, bool consideraHistorico = false)
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
            query.AppendLine("       t.tipo_turno as tipoTurno");
            query.AppendLine("from abrangencia a");
            query.AppendLine("  join usuario u");
            query.AppendLine("      on a.usuario_id = u.id");
            query.AppendLine("  inner join v_abrangencia_cadeia_turmas t");
            query.AppendLine("     on (a.turma_id notnull and a.turma_id = t.turma_id) or");
            query.AppendLine("        (a.turma_id is null and a.ue_id is null and a.dre_id = t.dre_id) or-- admin dre");
            query.AppendLine("        (a.turma_id is null and a.dre_id is null and a.ue_id = t.ue_id) --admin ue");
            query.AppendLine("  inner join ue");
            query.AppendLine("      on ue.id = t.ue_id");
            query.AppendLine($"where { (!consideraHistorico ? " not " : string.Empty) } a.historico");
            query.AppendLine("  and u.login = @login");
            query.AppendLine("  and a.perfil = @perfil");
            query.AppendLine("  and t.turma_codigo = @turma;");

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query.ToString(), new { turma, login, perfil }))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<int>> ObterAnosLetivos(string login, Guid perfil, bool consideraHistorico)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            return (await database.Conexao.QueryAsync<int>(@"select f_abrangencia_anos_letivos(@login, @perfil, @consideraHistorico)
                                                             order by 1", new { login, perfil, consideraHistorico }));
        }

        public async Task<AbrangenciaDreRetorno> ObterDre(string dreCodigo, string ueCodigo, string login, Guid perfil)
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

            return (await database.Conexao.QueryFirstOrDefaultAsync<AbrangenciaDreRetorno>(query.ToString(), new { dreCodigo, ueCodigo, login, perfil }));
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            string query = @"select distinct abreviacao, 
                                             codigo, 
                                             nome 
                             from f_abrangencia_dres(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @anoLetivo)
                             order by 3";

            var parametros = new
            {
                login,
                perfil,
                consideraHistorico,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                anoLetivo
            };

            return (await database.Conexao.QueryAsync<AbrangenciaDreRetorno>(query, parametros)).AsList();
        }

        public async Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil, int anoLetivo, bool consideraHistorico)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            return (await database.Conexao.QueryAsync<int>(@"select f_abrangencia_modalidades(@login, @perfil, @consideraHistorico, @anoLetivo)
                                                             order by 1", new { login, perfil, consideraHistorico, anoLetivo })).AsList();
        }

        public async Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            var parametros = new { login, perfil, consideraHistorico, modalidade, anoLetivo };

            return (await database.Conexao.QueryAsync<int>(@"select f_abrangencia_semestres(@login, @perfil, @consideraHistorico, @modalidade, @anoLetivo)
                                                             order by 1", parametros)).AsList();
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            var query = @"select ano,
	                             anoLetivo,
	                             codigo,
	                             codigoModalidade,
	                             nome,
	                             semestre,
	                             qtDuracaoAula,
	                             tipoTurno,
                                 ensinoEspecial
                            from f_abrangencia_turmas(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @codigoUe, @anoLetivo)
                          order by 5";

            var result = (await database.Conexao.QueryAsync<AbrangenciaTurmaRetorno>(query.ToString(), new { login, perfil, consideraHistorico, modalidade, semestre = periodo, codigoUe, anoLetivo })).AsList();

            return result;
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

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            // Foi utilizada função de banco de dados com intuíto de melhorar a performance
            var query = @"select distinct codigo,
	                                      nome as NomeSimples,
	                                      tipoescola
	                         from f_abrangencia_ues(@login, @perfil, @consideraHistorico, @modalidade, @semestre, @codigoDre, @anoLetivo)
                          order by 2;";

            var parametros = new
            {
                login,
                perfil,
                consideraHistorico,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                codigoDre,
                anoLetivo
            };

            return await database.Conexao.QueryAsync<AbrangenciaUeRetorno>(query, parametros);
        }

        public bool PossuiAbrangenciaTurmaAtivaPorLogin(string login)
        {
            var sql = @"select count(*) from usuario u
                        inner join abrangencia a on a.usuario_id = u.id
                        where u.login = @login and historico = false and turma_id is not null
                              and not a.perfil = ANY(@perfisCJ) ;";

            var parametros = new { login, perfisCJ = new Guid[] { Perfis.PERFIL_CJ, Perfis.PERFIL_CJ_INFANTIL } };

            return database.Conexao.QueryFirstOrDefault<int>(sql, parametros) > 0;
        }

        public void RemoverAbrangenciasForaEscopo(string login, Guid perfil, TipoAbrangenciaSincronizacao escopo)
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

            database.Execute(query, new { login, perfil });
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
                                u.ue_id = @codigoUe";

            return await database.Conexao.QueryAsync<Modalidade>(query, new { codigoUe });
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> ObterDropDownTurmasPorUeAnoLetivoModalidadeSemestre(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre)
        {
            var query = new StringBuilder();

            query.AppendLine(@"select t.turma_id as valor, t.nome as descricao from turma t
                            inner join ue ue on ue.id = t.ue_id");

            query.AppendLine("where ue.ue_id = @codigoUe and ano_letivo = @anoLetivo");

            if (modalidade.HasValue && modalidade != 0)
                query.AppendLine("and t.modalidade_codigo = @modalidade");

            if (semestre > 0)
                query.AppendLine("and semestre = @semestre");

            var dados = await database.Conexao.QueryAsync<OpcaoDropdownDto>(query.ToString(), new { codigoUe, anoLetivo, modalidade, semestre });

            return dados.OrderBy(x => x.Descricao);
        }
    }
}