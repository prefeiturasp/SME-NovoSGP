using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AdicionarObservacaoDiarioBordoCommandHandler : IRequestHandler<AdicionarObservacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public AdicionarObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
        }

        public async Task<AuditoriaDto> Handle(AdicionarObservacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordoObservacao = new DiarioBordoObservacao(request.Observacao, request.DiarioBordoId, request.UsuarioId);
            await repositorioDiarioBordoObservacao.SalvarAsync(diarioBordoObservacao);
            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
