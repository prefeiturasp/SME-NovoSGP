using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteCurricularGrupoAreaOrdenacaoQueryHandler : IRequestHandler<ObterComponenteCurricularGrupoAreaOrdenacaoQuery, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>
    {
        private readonly IRepositorioComponenteCurricularGrupoAreaOrdenacao repositorioCCGrupoAreaOrdenacao;

        public ObterComponenteCurricularGrupoAreaOrdenacaoQueryHandler(IRepositorioComponenteCurricularGrupoAreaOrdenacao repositorioCCGrupoAreaOrdenacao)
        {
            this.repositorioCCGrupoAreaOrdenacao = repositorioCCGrupoAreaOrdenacao ?? throw new ArgumentNullException(nameof(repositorioCCGrupoAreaOrdenacao));
        }

        public async Task<IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>> Handle(ObterComponenteCurricularGrupoAreaOrdenacaoQuery request, CancellationToken cancellationToken)
        {
            var ordenacoes = await repositorioCCGrupoAreaOrdenacao.ObterOrdenacaoPorGruposAreas(request.GrupoMatrizIds, request.AreaDoConhecimentoIds);

            if (ordenacoes == null || !ordenacoes.Any())
                throw new NegocioException("Não foi possível obter as ordenações dos grupos e areas dos componentes informados");

            return ordenacoes;
        }
    }
}
