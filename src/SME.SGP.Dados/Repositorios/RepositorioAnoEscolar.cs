using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnoEscolar : IRepositorioAnoEscolar
    {
        private readonly ISgpContext database;

        public RepositorioAnoEscolar(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<IEnumerable<ModalidadeAnoDto>> ObterPorModalidadeCicloIdAbrangencia(Modalidade modalidade, long cicloId, long usuarioId, Guid perfil)
        {
            var query = new StringBuilder(@"select distinct tca.modalidade, tca.ano from tipo_ciclo tc 
                                                inner join tipo_ciclo_ano tca on tca.tipo_ciclo_id = tc.id 
                                                inner join turma t on t.ano = tca.ano 
                                                inner join ue ue on t.ue_id = ue.id 
                                                inner join v_abrangencia_usuario vau
                                                on vau.turma_id = t.turma_id
                                                where tc.descricao is not null and vau.usuario_id = @usuarioId 
                                                    and vau.usuario_perfil = @perfil
                                                and tca.modalidade = @modalidadeId ");

            if (cicloId > 0)
                query.AppendLine("and tca.tipo_ciclo_id = @cicloId");

            return await database.Conexao.QueryAsync<ModalidadeAnoDto>(query.ToString(), new { cicloId, modalidadeId = (int)modalidade, perfil, usuarioId });

        }

        public async Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> ObterAnosPorCodigoUeModalidade(string codigoUe, int[] modalidades)
        {
            try
            {
                var query = new StringBuilder(@"select distinct tca.id, tca.modalidade, tca.ano from tipo_ciclo tc 
                                                    inner join tipo_ciclo_ano tca on tca.tipo_ciclo_id = tc.id
                                                    inner join turma t on t.ano = tca.ano
                                                    inner join ue ue on t.ue_id = ue.id
                                                    where tc.descricao is not null
                                                    and tca.modalidade = any(@modalidades)");

                if (!String.IsNullOrEmpty(codigoUe))
                    query.AppendLine(" and ue.ue_id = @codigoUe");

                query.AppendLine(" order by tca.modalidade, tca.ano ");

                return await database.Conexao.QueryAsync<AnosPorCodigoUeModalidadeEscolaAquiResult>(query.ToString(), new { codigoUe, modalidades });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}