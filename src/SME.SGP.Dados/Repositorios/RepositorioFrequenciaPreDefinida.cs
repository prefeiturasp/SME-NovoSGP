using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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
                                               and codigo_aluno = @alunoCodigo
                                               order by fpd.id desc;");

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
            var query = new StringBuilder(@"select distinct fpd.codigo_aluno as CodigoAluno,
                                                   fpd.tipo_frequencia as Tipo
                                              from frequencia_pre_definida fpd
                                             where fpd.turma_id = @turmaId
                                               and fpd.componente_curricular_id = @componenteCurricularId ");

            var parametros = new
            {
                turmaId,
                componenteCurricularId,
            };

            return await database.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
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

        public async Task<IEnumerable<FrequenciaPreDefinida>> ObterListaFrequenciaPreDefinida(long turmaId, long componenteCurricularId)
        {
            var query = new StringBuilder(@"select *
                                              from frequencia_pre_definida fpd
                                             where fpd.turma_id = @turmaId
                                               and fpd.componente_curricular_id = @componenteCurricularId ");

            return await database.Conexao.QueryAsync<FrequenciaPreDefinida>(query.ToString(), new { turmaId, componenteCurricularId });
        }
        public async Task<bool> InserirVarios(IEnumerable<FrequenciaPreDefinida> registros)
        {
            var sql = @"copy frequencia_pre_definida (                                         
                                        componente_curricular_id, 
                                        turma_id, 
                                        codigo_aluno, 
                                        tipo_frequencia)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var frequencia in registros)
                {
                    writer.StartRow();
                    writer.Write(frequencia.ComponenteCurricularId, NpgsqlDbType.Bigint);
                    writer.Write(frequencia.TurmaId, NpgsqlDbType.Bigint);
                    writer.Write(frequencia.CodigoAluno);
                    writer.Write((int)frequencia.TipoFrequencia, NpgsqlDbType.Integer);
                }
                writer.Complete();
            }

            return true;
        }
    }
}
