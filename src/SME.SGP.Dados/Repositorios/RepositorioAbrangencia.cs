using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Linq;
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
	                            abrangencia
                            where
	                            usuario_id = (select id from usuario where login = @login)
	                            and perfil = @perfil";

            return (await database.Conexao.QueryAsync<bool>(query, new { login, perfil })).Single();
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
            var query = @"insert into abrangencia_turmas
            (turma_id, abrangencia_ues_id, nome, ano_letivo, ano, modalidade, modalidade_codigo, semestre)
            values(@turma_id, @abrangencia_ues_id, @nome, @ano_letivo, @ano, @modalidade, @modalidade_codigo, @semestre )
            RETURNING id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                turma_id = abrangenciaTurma.Codigo,
                abrangencia_ues_id = idAbragenciaUe,
                nome = abrangenciaTurma.NomeTurma,
                ano_letivo = abrangenciaTurma.AnoLetivo,
                ano = abrangenciaTurma.Ano,
                modalidade = abrangenciaTurma.Modalidade,
                modalidade_codigo = abrangenciaTurma.CodigoModalidade,
                semestre = abrangenciaTurma.Semestre
            });

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