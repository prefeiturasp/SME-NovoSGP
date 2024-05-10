using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDiarioBordoAulaPorObservacaoIdQueryHandler : IRequestHandler<ObterTurmaDiarioBordoAulaPorObservacaoIdQuery, Turma>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public ObterTurmaDiarioBordoAulaPorObservacaoIdQueryHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao;
        }

        public async Task<Turma> Handle(ObterTurmaDiarioBordoAulaPorObservacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioDiarioBordoObservacao.ObterTurmaDiarioBordoAulaPorObservacaoId(request.ObservacaoId);
    }
}