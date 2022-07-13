using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosTurmaEolPorCodigoQueryHandler : IRequestHandler<ObterDadosTurmaEolPorCodigoQuery,DadosTurmaEolDto>
    {
        private readonly IServicoEol servicoEol;

        public ObterDadosTurmaEolPorCodigoQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            
        }

        public async Task<DadosTurmaEolDto> Handle(ObterDadosTurmaEolPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await servicoEol.ObterDadosTurmaPorCodigo(request.CodigoTurma);
        }
    }
}