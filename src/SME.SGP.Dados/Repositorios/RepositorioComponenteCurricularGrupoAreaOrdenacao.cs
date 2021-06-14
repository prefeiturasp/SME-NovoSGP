using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
   public class RepositorioComponenteCurricularGrupoAreaOrdenacao : IRepositorioComponenteCurricularGrupoAreaOrdenacao
    {
        private readonly ISgpContext database;

        public RepositorioComponenteCurricularGrupoAreaOrdenacao(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> ObterOrdenacaoPorGruposAreas(long[] grupoMatrizIds, long[] areaConhecimentoIds)
        {
            var query = @"SELECT grupo_matriz_id GrupoMatrizId, area_conhecimento_id AreaConhecimentoId, ordem
                                FROM public.componente_curricular_grupo_area_ordenacao
                            where grupo_matriz_id = ANY(@grupoMatrizIds) and area_conhecimento_id = ANY(@areaConhecimentoIds)";

            return await database.Conexao.QueryAsync<ComponenteCurricularGrupoAreaOrdenacaoDto>(query.ToString(), new { grupoMatrizIds, areaConhecimentoIds });
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacao>> ObterOrdenacaoPorGruposAreasAsync(long[] grupoMatrizIds, long[] areaConhecimentoIds)
        {
            var query = @"SELECT grupo_matriz_id GrupoMatrizId, area_conhecimento_id AreaConhecimentoId, ordem
                                FROM public.componente_curricular_grupo_area_ordenacao
                            where grupo_matriz_id = ANY(@grupoMatrizIds) and area_conhecimento_id = ANY(@areaConhecimentoIds)";

            return await database.Conexao.QueryAsync<ComponenteCurricularGrupoAreaOrdenacao>(query.ToString(), new { grupoMatrizIds, areaConhecimentoIds });
        }
    }
}
