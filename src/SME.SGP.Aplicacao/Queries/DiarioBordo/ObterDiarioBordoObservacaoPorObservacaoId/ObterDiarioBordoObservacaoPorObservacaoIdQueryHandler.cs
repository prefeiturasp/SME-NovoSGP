using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoObservacaoPorObservacaoIdQueryHandler : IRequestHandler<ObterDiarioBordoObservacaoPorObservacaoIdQuery, DiarioBordoObservacaoDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public ObterDiarioBordoObservacaoPorObservacaoIdQueryHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao;
        }

        public async Task<DiarioBordoObservacaoDto> Handle(ObterDiarioBordoObservacaoPorObservacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordoObservacao.ObterDiarioBordoObservacaoPorObservacaoId(request.ObservacaoId);
    }
}