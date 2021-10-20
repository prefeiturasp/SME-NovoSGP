using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacoesFrequencciaDaAulaCommandHandler : IRequestHandler<ExcluirAnotacoesFrequencciaDaAulaCommand, bool>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;
        private readonly IMediator mediator;

        public ExcluirAnotacoesFrequencciaDaAulaCommandHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno, IMediator mediator)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirAnotacoesFrequencciaDaAulaCommand request, CancellationToken cancellationToken)
        {
            var frequencia = await repositorioAnotacaoFrequenciaAluno.ObterPorAlunoId(request.AulaId);
            foreach (var item in frequencia)
            {
                ExcluirArquivo(item.Anotacao);
            }
            return await repositorioAnotacaoFrequenciaAluno.ExcluirAnotacoesDaAula(request.AulaId);
        }


        private void ExcluirArquivo(string anotacao)
        {
            if (!string.IsNullOrEmpty(anotacao))
            {
                var deletarArquivosNaoUtilziados = mediator.Send(new RemoverArquivosExcluidosCommand(anotacao, string.Empty, TipoArquivo.FrequenciaAnotacaoEstudante.Name()));
            }
        }
    }
}
