using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaPreDefinida : IRepositorioFrequenciaPreDefinida
    {
        private readonly ISgpContext database;

        public RepositorioFrequenciaPreDefinida(ISgpContext dataBase)
        {
            this.database = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
        }

        public async Task<IEnumerable<FrequenciaPreDefinidaDto>> Listar(long turmaId, long componenteCurricularId, string codigoAluno)
        {
            var query = new StringBuilder(@"select codigo_aluno as codigoAluno,
                                                   tipo_frequencia as tipo
                                              from frequencia_pre_definida fpd 
                                             where turma_id = @turmaId
                                               and componente_curricular_id  = @componenteCurricularId  ");

            if (!string.IsNullOrEmpty(codigoAluno))
                query.AppendLine("and codigo_aluno = @codigoAluno");

            var parametros = new
            {
                turmaId,
                componenteCurricularId,
                codigoAluno
            };

            return await database.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
        }

        public async Task<FrequenciaPreDefinidaDto> ObterPorTurmaECCEAlunoCodigo(long turmaId, long componenteCurricularId, string alunoCodigo)
        {
            var query = new StringBuilder(@"select codigo_aluno as codigoAluno,
                                                   tipo_frequencia as tipo
                                              from frequencia_pre_definida fpd 
                                             where turma_id = @turmaId
                                               and componente_curricular_id = @componenteCurricularId 
                                               and codigo_aluno = @alunoCodigo; ");

            var parametros = new
            {
                turmaId,
                componenteCurricularId,
                alunoCodigo
            };

            return await database.Conexao.QueryFirstOrDefaultAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<FrequenciaPreDefinidaDto>> ObterPorTurmaEComponente(long turmaId, long componenteCurricularId)
        {
            var query = new StringBuilder(@"select fpd.codigo_aluno as AlunoCodigo,
                                                   fpd.tipo_frequencia as Tipo
                                              from frequencia_pre_definida fpd 
                                            inner join turma t on t.id = fpd.turma_id
                                             where t.turma_id = @turmaId
                                               and fpd.componente_curricular_id = @componenteCurricularId ");

            var parametros = new
            {
                turmaId,
                componenteCurricularId,
            };

            return await database.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
        }

        public async Task RemoverPorCCIdETurmaId(long componenteCurricularId, long turmaId)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM frequencia_pre_definida " +
                "WHERE turma_id = @turmaId AND componente_curricular_id = @componenteCurricularId;",
                new { turmaId, componenteCurricularId });

        }

        public async Task Salvar(FrequenciaPreDefinida frequenciaPreDefinida)
        {
            await database.Conexao.ExecuteAsync(@"INSERT INTO frequencia_pre_definida 
                (componente_curricular_id,turma_id,codigo_aluno,tipo_frequencia) values 
                (@componenteCurricularId, @turmaId, @codigoAluno, @tipoFrequencia);",
               new
               {
                   turmaId = frequenciaPreDefinida.TurmaId,
                   componenteCurricularId = frequenciaPreDefinida.ComponenteCurricularId,
                   codigoAluno = frequenciaPreDefinida.CodigoAluno,
                   tipoFrequencia = frequenciaPreDefinida.TipoFrequencia
               });
        }
    }
}
