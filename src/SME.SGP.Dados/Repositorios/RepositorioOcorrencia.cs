using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioOcorrencia : RepositorioBase<Ocorrencia>, IRepositorioOcorrencia
    {
        private const string ColunasConsulta = @"SELECT
                                                    id,
                                                    alterador_por,
                                                    alterado_rf,
                                                    alterado_em,
                                                    criado_por,
                                                    criado_rf,
                                                    criado_em,
                                                    titulo,
                                                    data_ocorrencia,
                                                    hora_ocorrencia,
                                                    descricao,
                                                    ocorrencia_tipo_id,
                                                    excluido
                                                FROM public.ocorrencia";

        public RepositorioOcorrencia(ISgpContext database) : base(database)
        {
        }

        public Task<IEnumerable<Ocorrencia>> ObterPorIdsAsync(IEnumerable<long> ids)
        {
            var idsParaConsulta = string.Join(", ", ids);
            var query = $@"{ColunasConsulta} WHERE id IN ({idsParaConsulta})";
            return database.Conexao.QueryAsync<Ocorrencia>(query);
        }
    }
}