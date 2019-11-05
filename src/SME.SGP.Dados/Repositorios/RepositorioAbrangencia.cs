using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
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

        public async Task<bool> JaExisteAbrangencia(string login, Guid perfil)
        {
            var query = @"select
	                            1
                            from
	                            abrangencia_dres
                            where
	                            usuario_id = (select id from usuario where login = @login)
	                            and perfil = @perfil";

            return (await database.Conexao.QueryAsync<bool>(query, new { login, perfil })).FirstOrDefault();
        }

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorFiltro(string texto, string login, Guid perfil)
        {
            texto = $"%{texto.ToUpper()}%";

            var query = @"select
	                    ab_turmas.modalidade_codigo as modalidade,
	                    ab_turmas.ano_letivo as anoLetivo,
	                    ab_dres.dre_id as codigoDre,
	                    ab_turmas.turma_id as codigoTurma,
	                    ab_ues.ue_id as codigoUe,
	                    ab_dres.nome as nomeDre,
	                    ab_turmas.nome as nomeTurma,
	                    ab_ues.nome as nomeUe,
	                    ab_turmas.semestre
                    from
	                    abrangencia_turmas ab_turmas
                    inner join abrangencia_ues ab_ues on
	                    ab_turmas.abrangencia_ues_id = ab_ues.id
                    inner join abrangencia_dres ab_dres on
	                    ab_ues.abrangencia_dres_id = ab_dres.id
                    where
                        ab_dres.usuario_id = (select id from usuario where login = @login)
	                    and ab_dres.perfil = @perfil
                        and (upper(ab_turmas.nome) like @texto OR upper(f_unaccent(ab_ues.nome)) LIKE @texto)
                    order by nomeUe
                    OFFSET 0 ROWS FETCH NEXT  10 ROWS ONLY";

            return (await database.Conexao.QueryAsync<AbrangenciaFiltroRetorno>(query, new { texto, login, perfil })).AsList();
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(string login, Guid perfil, Modalidade? modalidade = null)
        {

            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("ab_dres.abreviacao,");
            query.AppendLine("ab_dres.dre_id as codigo,");
            query.AppendLine("ab_dres.nome");
            query.AppendLine("from");
            query.AppendLine("abrangencia_dres ab_dres");
            query.AppendLine("inner join abrangencia_ues ab_ues");
            query.AppendLine("on ab_ues.abrangencia_dres_id = ab_dres.id");
            query.AppendLine("inner join abrangencia_turmas ab_turmas");
            query.AppendLine("on ab_turmas.abrangencia_ues_id = ab_ues.id");
            query.AppendLine("where");
            query.AppendLine("ab_dres.usuario_id = (select id from usuario where login = @login)");
            query.AppendLine("and ab_dres.perfil = @perfil");
            
            if (modalidade.HasValue)
                query.AppendLine("and ab_turmas.modalidade_codigo = @modalidade");

                        return (await database.Conexao.QueryAsync<AbrangenciaDreRetorno>(query.ToString(), new { login, perfil, modalidade = (modalidade.HasValue ? modalidade.Value : 0) })).AsList();
        }

        public async Task<IEnumerable<int>> ObterModalidades(string login, Guid perfil)
        {
            var query = @"select
	                        distinct ab_turmas.modalidade_codigo
                        from
	                        abrangencia_dres ab_dres
                        inner join abrangencia_ues ab_ues on
	                        ab_ues.abrangencia_dres_id = ab_dres.id
                        inner join abrangencia_turmas ab_turmas on
	                        ab_turmas.abrangencia_ues_id = ab_ues.id
                        where
	                        ab_dres.usuario_id = ( select id from usuario where login = @login)
	                        and ab_dres.perfil = @perfil";

            return (await database.Conexao.QueryAsync<int>(query, new { login, perfil })).AsList();
        }

        public async Task<IEnumerable<int>> ObterSemestres(string login, Guid perfil, Modalidade modalidade)
        {
            var query = @"select distinct ab_turmas.semestre
                    from
	                    abrangencia_turmas ab_turmas
                    inner join abrangencia_ues ab_ues on
	                    ab_turmas.abrangencia_ues_id = ab_ues.id
                    inner join abrangencia_dres ab_dres on
	                    ab_ues.abrangencia_dres_id = ab_dres.id
                    where
                        ab_dres.usuario_id = (select id from usuario where login = @login)
	                    and ab_dres.perfil = @perfil
                        and ab_turmas.modalidade_codigo = @modalidade";

            return (await database.Conexao.QueryAsync<int>(query, new { login, perfil, modalidade })).AsList();
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, string login, Guid perfil, Modalidade modalidade)
        {
            var query = @"select distinct
	                    ab_turmas.ano,
	                    ab_turmas.ano_letivo as anoLetivo,
	                    ab_turmas.turma_id as codigo,
	                    ab_turmas.modalidade_codigo as codigoModalidade,
	                    ab_turmas.nome,
	                    ab_turmas.semestre
                    from
	                    abrangencia_turmas ab_turmas
                    inner join abrangencia_ues ab_ues on
	                    ab_turmas.abrangencia_ues_id = ab_ues.id
                    inner join abrangencia_dres ab_dres on
	                    ab_ues.abrangencia_dres_id = ab_dres.id
                    where
                        ab_ues.ue_id = @codigoUe
	                    and ab_dres.usuario_id = (select id from usuario where login = @login)
	                    and ab_dres.perfil = @perfil
                        and ab_turmas.modalidade_codigo = @modalidade";

            return (await database.Conexao.QueryAsync<AbrangenciaTurmaRetorno>(query, new { codigoUe, login, perfil, modalidade })).AsList();
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, string login, Guid perfil, Modalidade? modalidade = null)
        {
            var query = new StringBuilder();

            query.AppendLine("select distinct");
            query.AppendLine("ab_ues.ue_id as codigo,");
            query.AppendLine("ab_ues.nome");
            query.AppendLine("from");
            query.AppendLine("abrangencia_turmas ab_turmas");
            query.AppendLine("inner join abrangencia_ues ab_ues on");
            query.AppendLine("ab_turmas.abrangencia_ues_id = ab_ues.id");
            query.AppendLine("inner join abrangencia_dres ab_dres on");
            query.AppendLine("ab_ues.abrangencia_dres_id = ab_dres.id");
            query.AppendLine("where");
            query.AppendLine("ab_dres.dre_id = @codigoDre");
            query.AppendLine("and ab_dres.usuario_id = (select id from usuario where login = @login)");
            query.AppendLine("and ab_dres.perfil = @perfil");

            if (modalidade.HasValue)
                query.AppendLine("and ab_turmas.modalidade_codigo = @modalidade");

            return (await database.Conexao.QueryAsync<AbrangenciaUeRetorno>(query.ToString(), new { codigoDre, login, perfil, modalidade = (modalidade.HasValue ? modalidade.Value : 0) })).AsList();
        }

        public async Task RemoverAbrangencias(string login)
        {
            var query = "delete from abrangencia_dres where usuario_id = (select id from usuario where login = @login)";

            await database.ExecuteAsync(query, new { login });
        }

        public async Task<long> SalvarDre(AbrangenciaDreRetornoEolDto abrangenciaDre, string login, Guid perfil)
        {
            var query = @"insert into abrangencia_dres
            (usuario_id, dre_id, abreviacao, nome, perfil)values
            ((select id from usuario where login = @login), @dre_id, @abreviacao, @nome, @perfil)
            RETURNING id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                dre_id = abrangenciaDre.Codigo,
                abreviacao = abrangenciaDre.Abreviacao,
                nome = abrangenciaDre.Nome,
                login,
                perfil
            });

            return resultadoTask.Single();
        }

        public async Task<long> SalvarTurma(AbrangenciaTurmaRetornoEolDto abrangenciaTurma, long idAbragenciaUe)
        {
            var query = @"insert
	                        into
	                        abrangencia_turmas (turma_id,
	                        abrangencia_ues_id,
	                        nome,
	                        ano_letivo,
	                        ano,
	                        modalidade_codigo,
	                        semestre)
                        values(@turma_id,
                        @abrangencia_ues_id,
                        @nome,
                        @ano_letivo,
                        @ano,
                        @modalidade_codigo,
                        @semestre ) returning id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                turma_id = abrangenciaTurma.Codigo,
                abrangencia_ues_id = idAbragenciaUe,
                nome = abrangenciaTurma.NomeTurma,
                ano_letivo = abrangenciaTurma.AnoLetivo,
                ano = abrangenciaTurma.Ano,
                modalidade_codigo = int.Parse(abrangenciaTurma.CodigoModalidade),
                semestre = abrangenciaTurma.Semestre
            }); ;

            return resultadoTask.Single();
        }

        public async Task<long> SalvarUe(AbrangenciaUeRetornoEolDto abrangenciaUe, long idAbragenciaDre)
        {
            var query = @"insert into abrangencia_ues
            (ue_id, abrangencia_dres_id, nome)values(@ue_id, @abrangencia_dres_id, @nome)
            RETURNING id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                ue_id = abrangenciaUe.Codigo,
                abrangencia_dres_id = idAbragenciaDre,
                nome = abrangenciaUe.Nome,
            });

            return resultadoTask.Single();
        }
    }
}