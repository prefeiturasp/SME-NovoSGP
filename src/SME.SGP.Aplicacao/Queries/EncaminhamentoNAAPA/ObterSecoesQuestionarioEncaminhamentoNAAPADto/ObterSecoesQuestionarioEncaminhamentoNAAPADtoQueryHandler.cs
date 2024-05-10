using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioEncaminhamentoNAAPADtoQueryHandler : IRequestHandler<ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA;

        public ObterSecoesQuestionarioEncaminhamentoNAAPADtoQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA)
        {
            this.repositorioSecaoNAAPA = repositorioSecaoNAAPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoNAAPA));
        }
        
        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesQuestionarioEncaminhamentoNAAPADtoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoNAAPA.ObterSecoesQuestionarioDto(request.Modalidade);
        }
    }
}
