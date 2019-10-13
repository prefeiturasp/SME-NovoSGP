using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
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

        public async Task RemoverAbrangencias(long idUsuario)
        {
            var query = "delete from abrangencia_dres where usuario_id = @idUsuario";

            await database.ExecuteAsync(query, new { idUsuario });
        }

        public async Task<long> SalvarDre(AbrangenciaDreRetornoEolDto abrangenciaDre, long usuarioId)
        {
            var query = @"insert into abrangencia_dres
            (usuario_id, dre_id, abreviacao, nome)values
            (@usuarioId, @dre_id, @abreviacao, @nome) RETURNING id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                dre_id = abrangenciaDre.Codigo,
                abreviacao = abrangenciaDre.Abreviacao,
                nome = abrangenciaDre.Nome,
                usuarioId
            });

            return resultadoTask.Single();
        }

        public async Task<long> SalvarTurma(AbrangenciaTurmaRetornoEolDto abrangenciaTurma, long idAbragenciaUe)
        {
            var query = @"insert into abrangencia_turmas
            (turma_id, abrangencia_ues_id, nome, ano_letivo, ano, modalidade, modalidade_codigo)values(@turma_id, @abrangencia_ues_id, @nome, @ano_letivo, @ano, @modalidade, @modalidade_codigo ) RETURNING id";

            var resultadoTask = await database.Conexao.QueryAsync<long>(query, new
            {
                turma_id = abrangenciaTurma.Codigo,
                abrangencia_ues_id = idAbragenciaUe,
                nome = abrangenciaTurma.NomeTurma,
                ano_letivo = abrangenciaTurma.AnoLetivo,
                ano = abrangenciaTurma.Ano,
                modalidade = abrangenciaTurma.Modalidade,
                modalidade_codigo = abrangenciaTurma.CodigoModalidade
            });

            return resultadoTask.Single();
        }

        public async Task<long> SalvarUe(AbrangenciaUeRetornoEolDto abrangenciaUe, long idAbragenciaDre)
        {
            var query = @"insert into abrangencia_ues
            (ue_id, abrangencia_dres_id, nome)values(@ue_id, @abrangencia_dres_id, @nome) RETURNING id";

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