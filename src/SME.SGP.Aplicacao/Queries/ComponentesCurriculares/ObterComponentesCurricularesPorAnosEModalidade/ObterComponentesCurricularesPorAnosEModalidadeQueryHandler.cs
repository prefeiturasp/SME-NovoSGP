using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade
{
    public class ObterComponentesCurricularesPorAnosEModalidadeQueryHandler : IRequestHandler<ObterComponentesCurricularesPorAnosEModalidadeQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IServicoEol servicoEol;

        public ObterComponentesCurricularesPorAnosEModalidadeQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesPorAnosEModalidadeQuery request, CancellationToken cancellationToken)
        {
            var componentes = (await servicoEol.ObterComponentesCurricularesPorAnosEModalidade(request.CodigoUe, request.Modalidade, request.AnosEscolares, request.AnoLetivo))?.ToList();
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Nenhum componente localizado para a modalidade e anos informados.");
            }
            componentes = componentes.OrderBy(c => c.Descricao).ToList();
            componentes.Insert(0, new ComponenteCurricularEol
            {
                Codigo = -99,
                Descricao = "Todos"
            });
            return componentes;
        }
    }
}
