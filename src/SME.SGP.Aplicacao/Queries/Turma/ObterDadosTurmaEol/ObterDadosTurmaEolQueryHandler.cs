using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosTurmaEolQueryHandler : IRequestHandler<ObterDadosTurmaEolQuery, DadosTurmaEolDto>
    {
        private readonly IServicoEol servicoEol;

        public ObterDadosTurmaEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<DadosTurmaEolDto> Handle(ObterDadosTurmaEolQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.ObterDadosTurmaPorCodigo(request.CodigoTurma);
        }
    }
}
