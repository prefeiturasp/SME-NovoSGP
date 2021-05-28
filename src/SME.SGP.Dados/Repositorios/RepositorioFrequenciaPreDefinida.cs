using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaPreDefinida : IRepositorioFrequenciaPreDefinida
    {
        private readonly ISgpContext dataBase;

        public RepositorioFrequenciaPreDefinida(ISgpContext dataBase)
        {
            this.dataBase = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
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

            return await dataBase.Conexao.QueryAsync<FrequenciaPreDefinidaDto>(query.ToString(), parametros);
        }
    }
}
