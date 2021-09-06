using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class OrdenarComponentesPorGrupoMatrizAreaConhecimentoQueryHandler : IRequestHandler<OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery, IEnumerable<ComponenteCurricularPorTurma>>
    {
        IRepositorioComponenteCurricularGrupoAreaOrdenacao repositorioComponenteCurricularGrupoAreaOrdenacao;

        public OrdenarComponentesPorGrupoMatrizAreaConhecimentoQueryHandler(IRepositorioComponenteCurricularGrupoAreaOrdenacao repositorioComponenteCurricularGrupoAreaOrdenacao)
        {
            this.repositorioComponenteCurricularGrupoAreaOrdenacao = repositorioComponenteCurricularGrupoAreaOrdenacao ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricularGrupoAreaOrdenacao));
        }

        public async Task<IEnumerable<ComponenteCurricularPorTurma>> Handle(OrdenarComponentesPorGrupoMatrizAreaConhecimentoQuery request, CancellationToken cancellationToken)
        {
            var gruposIds = request.ComponentesCurriculares.Where(cc => cc.GrupoMatriz != null)?.Select(cc => cc.GrupoMatriz.Id).Distinct().ToArray();
            var areasIds = request.ComponentesCurriculares.Where(cc => cc.AreaDoConhecimento != null)?.Select(cc => cc.AreaDoConhecimento.Id).Distinct().ToArray();

            var ordenacao = await repositorioComponenteCurricularGrupoAreaOrdenacao.ObterOrdenacaoPorGruposAreasAsync(gruposIds, areasIds);

            var retorno = request.ComponentesCurriculares.Select(cc =>
            {
                if (cc.AreaDoConhecimento != null)
                    cc.AreaDoConhecimento.DefinirOrdem(ordenacao, cc.GrupoMatriz.Id);

                return cc;
            })
                                                      .OrderBy(c => c.GrupoMatriz.Id)
                                                      .ThenByDescending(c => c.AreaDoConhecimento != null)
                                                      .ThenBy(c => c.AreaDoConhecimento.Ordem)
                                                      .ThenBy(c => c.Disciplina, StringComparer.OrdinalIgnoreCase);

            return retorno;
        }
    }
}
