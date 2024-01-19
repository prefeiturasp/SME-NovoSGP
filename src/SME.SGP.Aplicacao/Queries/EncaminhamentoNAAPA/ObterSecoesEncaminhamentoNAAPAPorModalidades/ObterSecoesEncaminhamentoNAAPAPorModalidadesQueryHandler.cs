using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesEncaminhamentoNAAPAPorModalidadesQueryHandler : IRequestHandler<ObterSecoesEncaminhamentoNAAPAPorModalidadesQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;
       
        public ObterSecoesEncaminhamentoNAAPAPorModalidadesQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesEncaminhamentoNAAPAPorModalidadesQuery request, CancellationToken cancellationToken)
            => await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorModalidades(request.TipoQuestionario, request.Modalidades);

    }
}
