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
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaPreDefinidaConsulta : IRepositorioFrequenciaPreDefinidaConsulta
    {
        private readonly ISgpContextConsultas sgpContextConsultas;
        private readonly ISgpContext sgpContext;

        public RepositorioFrequenciaPreDefinidaConsulta(ISgpContextConsultas dataBase, ISgpContext sgpContext)
        {
            this.sgpContextConsultas = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
            this.sgpContext = sgpContext;
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

            return await sgpContextConsultas.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
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

            return await sgpContext.Conexao.QueryFirstOrDefaultAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
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

            return await sgpContextConsultas.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<FrequenciaPreDefinida>> ObterListaFrequenciaPreDefinida(long turmaId, long componenteCurricularId)
        {
            var query = new StringBuilder(@"select *
                                              from frequencia_pre_definida fpd
                                             where fpd.turma_id = @turmaId
                                               and fpd.componente_curricular_id = @componenteCurricularId ");

            return await sgpContextConsultas.Conexao.QueryAsync<FrequenciaPreDefinida>(query.ToString(), new { turmaId, componenteCurricularId });
        }
    }
}
