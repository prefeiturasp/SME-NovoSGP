using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaQuestaoPorArquivoCommandHandler : IRequestHandler<ExcluirItineranciaQuestaoPorArquivoCommand,bool>
    {
        private readonly IRepositorioItineranciaQuestao repositorioItineranciaQuestao;

        public ExcluirItineranciaQuestaoPorArquivoCommandHandler(IRepositorioItineranciaQuestao repositorioItineranciaQuestao)
        {
            this.repositorioItineranciaQuestao = repositorioItineranciaQuestao ?? throw new ArgumentNullException(nameof(repositorioItineranciaQuestao));
        }

        public async Task<bool> Handle(ExcluirItineranciaQuestaoPorArquivoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioItineranciaQuestao.ExcluirItineranciaQuestaoPorArquivo(request.ArquivoId);
        }
    }
}