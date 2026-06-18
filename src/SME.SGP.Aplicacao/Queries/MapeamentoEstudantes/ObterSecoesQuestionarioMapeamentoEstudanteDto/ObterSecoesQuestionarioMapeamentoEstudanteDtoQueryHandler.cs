using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioMapeamentoEstudanteDtoQueryHandler : IRequestHandler<ObterSecoesQuestionarioMapeamentoEstudanteDtoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoMapeamentoEstudante repositorioSecao;

        public ObterSecoesQuestionarioMapeamentoEstudanteDtoQueryHandler(IRepositorioSecaoMapeamentoEstudante repositorioSecao)
        {
            this.repositorioSecao = repositorioSecao ?? throw new System.ArgumentNullException(nameof(repositorioSecao));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesQuestionarioMapeamentoEstudanteDtoQuery request, CancellationToken cancellationToken)
        => await repositorioSecao.ObterSecoesQuestionarioDto();
    }
}
