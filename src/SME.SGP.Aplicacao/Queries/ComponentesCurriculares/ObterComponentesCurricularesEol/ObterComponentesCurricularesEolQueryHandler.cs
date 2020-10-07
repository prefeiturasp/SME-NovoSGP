using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesEolQueryHandler : IRequestHandler<ObterComponentesCurricularesEolQuery, IEnumerable<ComponenteCurricularDto>>
    {
        private readonly IServicoEol servicoEol;

        public ObterComponentesCurricularesEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesCurricularesEolQuery request, CancellationToken cancellationToken)
        {
            var componentes = (await servicoEol.ObterComponentesCurriculares())?.ToList();
            if (componentes == null || !componentes.Any())
            {
                throw new NegocioException("Não foi encontrado nenhum componente curricular no EOL");
            }
            return componentes;
        }
    }
}
