using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoCommandHandler : IRequestHandler<ExcluirObservacaoDiarioBordoCommand, bool>
    {
        private readonly IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao;

        public ExcluirObservacaoDiarioBordoCommandHandler(IRepositorioDiarioBordoObservacao repositorioDiarioBordoObservacao)
        {
            this.repositorioDiarioBordoObservacao = repositorioDiarioBordoObservacao ?? throw new System.ArgumentNullException(nameof(repositorioDiarioBordoObservacao));
        }

        public async Task<bool> Handle(ExcluirObservacaoDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var diarioBordoObservacao = await repositorioDiarioBordoObservacao.ObterPorIdAsync(request.ObservacaoId);
            if (diarioBordoObservacao == null)
                throw new NegocioException("Observação do diário de bordo não encontrada.");

            diarioBordoObservacao.ValidarUsuarioAlteracao(request.UsuarioId);
            diarioBordoObservacao.Remover();

            await repositorioDiarioBordoObservacao.SalvarAsync(diarioBordoObservacao);
            return true;
        }
    }
}
