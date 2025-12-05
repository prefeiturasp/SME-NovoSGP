using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesAtendimentoNAAPAPorModalidadesQueryHandler : IRequestHandler<ObterSecoesAtendimentoNAAPAPorModalidadesQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoAtendimentoNAAPA repositorioSecaoEncaminhamentoNAPPA;
       
        public ObterSecoesAtendimentoNAAPAPorModalidadesQueryHandler(IRepositorioSecaoAtendimentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesAtendimentoNAAPAPorModalidadesQuery request, CancellationToken cancellationToken)
            => await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorModalidades(request.TipoQuestionario, request.Modalidades);

    }
}
