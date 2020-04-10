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

            var query = $@"select distinct va.modalidade_codigo as modalidade,
                                           va.turma_ano_letivo as anoLetivo,
                                           va.turma_ano as  ano,
                                           va.dre_codigo as codigoDre,
                                           va.turma_id as codigoTurma,
                                           va.ue_codigo as codigoUe,
                                           u.tipo_escola as tipoEscola,
                                           va.dre_nome as nomeDre,
                                           va.turma_nome as nomeTurma,
                                           va.ue_nome as nomeUe,
                                           va.turma_semestre as semestre,
                                           va.qt_duracao_aula as qtDuracaoAula,
                                           va.tipo_turno as tipoTurno
                           from
                               { (consideraHistorico ? "v_abrangencia_historica" : "v_abrangencia") } va
                           inner join ue u
                               on u.ue_id = va.ue_codigo and (upper(va.turma_nome) like @texto OR upper(f_unaccent(va.ue_nome)) LIKE @texto)
                           inner join usuario us
    	                       on va.usuario_id = us.id    	
                           where
                                us.login = @login
                                and va.usuario_perfil = @perfil
                           order by va.ue_nome
                           limit 10";                      

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query, new { texto, login, perfil })).AsList();
        }

        public Task<IEnumerable<AbrangenciaSinteticaDto>> ObterAbrangenciaSintetica(string login, Guid perfil, string turmaId = "")
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

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and codigo_turma = @turmaId");

            return database.Conexao.QueryAsync<AbrangenciaSinteticaDto>(query.ToString(), new { login, perfil, turmaId });
        }

        public async Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, string login, Guid perfil, bool consideraHistorico = false)
        {
            var query = new StringBuilder();

            query.AppendLine(@"select
                            va.modalidade_codigo as modalidade,
                            va.turma_ano_letivo as anoLetivo,
	                        va.turma_ano as  ano,
                            va.dre_codigo as codigoDre,
                            va.turma_id as codigoTurma,
                            va.ue_codigo as codigoUe,
	                        u.tipo_escola as tipoEscola,
	                        va.dre_nome as nomeDre,
	                        va.turma_nome as nomeTurma,
	                        va.ue_nome as nomeUe,
	                        va.turma_semestre as semestre,
                            va.qt_duracao_aula as qtDuracaoAula,
                            va.tipo_turno as tipoTurno from");

            query.AppendLine($"{(consideraHistorico ? "v_abrangencia_historica" : "v_abrangencia")} va");

            query.AppendLine(@"inner join ue u
                            on u.ue_id = va.ue_codigo
                        where
	                        va.usuario_id = (select id from usuario where login = @login)
                            and va.usuario_perfil = @perfil
                            and va.turma_id = @turma
                        order by va.ue_nome");

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query.ToString(), new { turma, login, perfil }))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<int>> ObterAnosLetivos(string login, Guid perfil, bool consideraHistorico)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select distinct va.turma_ano_letivo from");
            query.AppendLine(consideraHistorico ? "v_abrangencia_historica va" : "v_abrangencia va");
            query.AppendLine("inner join usuario u");
            query.AppendLine("on va.usuario_id = u.id");
            query.AppendLine("where va.turma_ano_letivo is not null and va.usuario_perfil = @perfil");
            query.AppendLine("and u.login = @login");
            query.AppendLine("order by turma_ano_letivo");

            return (await database.Conexao.QueryAsync<int>(query.ToString(), new { login, perfil }));
        }

        public async Task<AbrangenciaDreRetorno> ObterDre(string dreCodigo, string ueCodigo, string login, Guid perfil)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.dre_codigo as codigo,");
            query.AppendLine("va.dre_nome as nome,");
            query.AppendLine("va.dre_abreviacao as abreviacao");
            query.AppendLine("from");
            query.AppendLine("v_abrangencia va");
            query.AppendLine("inner join usuario u");
            query.AppendLine("on va.usuario_id = u.id");
            query.AppendLine("where u.login = @login");
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
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.dre_abreviacao as abreviacao,");
            query.AppendLine("va.dre_codigo as codigo,");
            query.AppendLine("va.dre_nome as nome");
            query.AppendLine("from");
            query.AppendLine(consideraHistorico ? "v_abrangencia_historica va" : "v_abrangencia va");
            query.AppendLine("inner join usuario u on va.usuario_id = u.id");
            query.AppendLine("where");            
            query.AppendLine("va.usuario_perfil = @perfil");
            query.AppendLine("and va.dre_codigo is not null");
            query.AppendLine("and u.login = @login");

            if (modalidade.HasValue)
                query.AppendLine("and va.modalidade_codigo = @modalidade");

            if (periodo > 0)
                query.AppendLine("and va.turma_semestre = @semestre");

            if (consideraHistorico && anoLetivo > 0)
                query.Append("and va.turma_ano_letivo = @anoLetivo");

            var parametro = new
            {
                login,
                perfil,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                anoLetivo
            };

            return (await database.Conexao.QueryAsync<AbrangenciaDreRetorno>(query.ToString(), parametro)).AsList();
        }

        public async Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil, int anoLetivo, bool consideraHistorico)
        {
            var query = $@"select
                                distinct va.modalidade_codigo
                           from
                                { (consideraHistorico ? "v_abrangencia_historica" : "v_abrangencia") } va
                                    inner join usuario u
                                        on va.usuario_id = u.id
                           where
                                u.login = @login
                            and va.usuario_perfil = @perfil
                            and va.modalidade_codigo is not null
                            and va.turma_ano_letivo = @anoLetivo
                            and va.modalidade_codigo > 0";   

            return (await database.Conexao.QueryAsync<int>(query, new { login, perfil, anoLetivo })).AsList();
        }

        public async Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select distinct va.turma_semestre as semestre");
            query.AppendLine("from");
            query.AppendLine(consideraHistorico ? "v_abrangencia_historica va" : " v_abrangencia va");
            query.AppendLine("inner join usuario u on va.usuario_id = u.id");
            query.AppendLine("where u.login = @login");
            query.AppendLine("and va.usuario_perfil = @perfil");
            query.AppendLine("and va.modalidade_codigo = @modalidade");
            query.AppendLine("and va.turma_semestre is not null");

            if (consideraHistorico && anoLetivo > 0)
                query.AppendLine(" and va.turma_ano_letivo = @anoLetivo");

            var parametros = new { login, perfil, modalidade, anoLetivo };

            return (await database.Conexao.QueryAsync<int>(query.ToString(), parametros)).AsList();
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var queryBase = $@"select distinct
                                 va.turma_ano as ano,
                                 va.turma_ano_letivo as anoLetivo,
                                 va.turma_id as codigo,
                                 va.modalidade_codigo as codigoModalidade,
                                 va.turma_nome as nome,
	                             va.turma_semestre as semestre,
                                 va.qt_duracao_aula as qtDuracaoAula,
                                 va.tipo_turno as tipoTurno
                               from
                                 { (consideraHistorico ? "v_abrangencia_historica" : "v_abrangencia") } va
                                    inner join usuario u
                                        on va.usuario_id = u.id
                            where
                                va.ue_codigo = @codigoUe
                                and va.turma_id is not null
                                and u.login = @login
                                and va.usuario_perfil = @perfil";            

            StringBuilder query = new StringBuilder();

            query.AppendLine(queryBase);

            if (modalidade > 0)
                query.AppendLine("and va.modalidade_codigo = @modalidade");

            if (periodo > 0)
                query.AppendLine("and va.turma_semestre = @semestre");

            if (consideraHistorico && anoLetivo > 0)
                query.AppendLine("and va.turma_ano_letivo = @anoLetivo");

            return (await database.Conexao.QueryAsync<AbrangenciaTurmaRetorno>(query.ToString(), new { codigoUe, login, perfil, modalidade, semestre = periodo, anoLetivo })).AsList();
        }

        public async Task<AbrangenciaUeRetorno> ObterUe(string codigo, string login, Guid perfil)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.ue_codigo as codigo,");
            query.AppendLine("va.ue_nome as nome,");
            query.AppendLine("u.tipo_escola as tipoEscola");
            query.AppendLine("from");
            query.AppendLine("v_abrangencia va");
            query.AppendLine("inner join ue u");
            query.AppendLine("on u.ue_id = va.ue_codigo");
            query.AppendLine("where");
            query.AppendLine("va.ue_codigo = @codigo");
            query.AppendLine("and va.usuario_id = (select id from usuario where login = @login)");
            query.AppendLine("and va.usuario_perfil = @perfil");

            return (await database.Conexao.QueryFirstOrDefaultAsync<AbrangenciaUeRetorno>(query.ToString(), new { codigo, login, perfil }));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("va.ue_codigo as codigo,");
            query.AppendLine("va.ue_nome as nome,");
            query.AppendLine("u.tipo_escola as tipoEscola");
            query.AppendLine("from");
            query.AppendLine(consideraHistorico ? "v_abrangencia_historica va" : "v_abrangencia va");
            query.AppendLine("inner join ue u");
            query.AppendLine("on u.ue_id = va.ue_codigo");
            query.AppendLine("inner join usuario us");
            query.AppendLine("on va.usuario_id = us.id");
            query.AppendLine("where");
            query.AppendLine("va.dre_codigo = @codigoDre");
            query.AppendLine("and us.login = @login");
            query.AppendLine("and va.usuario_perfil = @perfil");

            if (modalidade.HasValue)
                query.AppendLine("and va.modalidade_codigo = @modalidade");

            if (periodo > 0)
                query.AppendLine("and va.turma_semestre = @semestre");

            if (consideraHistorico && anoLetivo > 0)
                query.AppendLine("and va.turma_ano_letivo = @anoLetivo");

            var parametros = new
            {
                codigoDre,
                login,
                perfil,
                modalidade = modalidade ?? 0,
                semestre = periodo,
                anoLetivo
            };

            return (await database.Conexao.QueryAsync<AbrangenciaUeRetorno>(query.ToString(), parametros)).AsList();
        }

        public bool PossuiAbrangenciaTurmaAtivaPorLogin(string login)
        {
            var sql = @"select count(*) from usuario u
                        inner join abrangencia a on a.usuario_id = u.id
                        where u.login = @login and historico = false and turma_id is not null;";

            var parametros = new { login };

            return database.Conexao.QueryFirstOrDefault<int>(sql, parametros) > 0;
        }

        public void RemoverAbrangenciasForaEscopo(string login, Guid perfil, TipoAbrangencia escopo)
        {
            var query = "delete from abrangencia where usuario_id = (select id from usuario where login = @login) and historico = false and perfil = @perfil and #escopo";

            switch (escopo)
            {
                case TipoAbrangencia.PorDre:
                    query = query.Replace("#escopo", " ue_id is not null and turma_id is not null");
                    break;

                case TipoAbrangencia.PorUe:
                    query = query.Replace("#escopo", " dre_id is not null and turma_id is not null");
                    break;

                case TipoAbrangencia.PorTurma:
                    query = query.Replace("#escopo", " ue_id is not null and dre_id is not null");
                    break;
            }

            database.Execute(query, new { login, perfil });
        }
    }
}