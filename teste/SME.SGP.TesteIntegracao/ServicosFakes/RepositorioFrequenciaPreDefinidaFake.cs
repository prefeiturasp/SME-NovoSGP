using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RepositorioFrequenciaPreDefinidaFake : IRepositorioFrequenciaPreDefinida
    {
        private readonly ISgpContext database;

        public RepositorioFrequenciaPreDefinidaFake(ISgpContext dataBase)
        {
            this.database = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
        }

        public async Task RemoverPorCCIdETurmaId(long componenteCurricularId, long turmaId, string[] alunosComFrequenciaRegistrada)
        {
            await database.Conexao.ExecuteAsync("DELETE FROM frequencia_pre_definida " +
            "WHERE turma_id = @turmaId AND componente_curricular_id = @componenteCurricularId and codigo_aluno = any(@alunosComFrequenciaRegistrada);",
            new { turmaId, componenteCurricularId, alunosComFrequenciaRegistrada });            
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

        public async Task Atualizar(FrequenciaPreDefinida frequenciaPreDefinida)
        {
            await database.Conexao.ExecuteAsync(@"UPDATE frequencia_pre_definida 
                                                  SET  componente_curricular_id = @componenteCurricularId,
                                                       turma_id = @turmaId,
                                                       codigo_aluno = @codigoAluno,
                                                       tipo_frequencia = @tipoFrequencia
                                                  WHERE Id = @id",
               new
               {
                   id = frequenciaPreDefinida.Id,
                   turmaId = frequenciaPreDefinida.TurmaId,
                   componenteCurricularId = frequenciaPreDefinida.ComponenteCurricularId,
                   codigoAluno = frequenciaPreDefinida.CodigoAluno,
                   tipoFrequencia = frequenciaPreDefinida.TipoFrequencia
               });
        }
        
        public async Task<bool> InserirVarios(IEnumerable<FrequenciaPreDefinida> registros)
        {
            foreach (var entidade in registros)
            {
                if (entidade.Id > 0)
                {                
                    await database.Conexao.UpdateAsync(entidade);
                }
                else
                {
                    entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
                }
            }

            return true; 
        }
    }
}
