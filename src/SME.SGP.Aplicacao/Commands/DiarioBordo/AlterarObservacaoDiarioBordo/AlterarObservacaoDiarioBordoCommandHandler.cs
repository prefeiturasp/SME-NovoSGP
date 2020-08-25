using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarObservacaoDiarioBordoCommandHandler : IRequestHandler<AlterarObservacaoDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public AlterarObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
        }

        public async Task<AuditoriaDto> Handle(AlterarObservacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordoObservacao = await repositorioDiarioBordoObservacao.ObterPorIdAsync(request.ObservacaoId);
            if (diarioBordoObservacao == null)
                throw new NegocioException("Observação do diário de bordo não encontrada.");

            diarioBordoObservacao.ValidarUsuarioAlteracao(request.UsuarioId);

            diarioBordoObservacao.Observacao = request.Observacao;

            await repositorioDiarioBordoObservacao.SalvarAsync(diarioBordoObservacao);
            return (AuditoriaDto)diarioBordoObservacao;
        }
    }
}
