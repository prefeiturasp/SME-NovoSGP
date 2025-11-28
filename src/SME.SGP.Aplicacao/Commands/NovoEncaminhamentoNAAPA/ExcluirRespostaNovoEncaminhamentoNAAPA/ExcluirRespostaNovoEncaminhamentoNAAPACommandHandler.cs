using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirRespostaNovoEncaminhamentoNAAPA
{
    public class ExcluirRespostaNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<ExcluirRespostaNovoEncaminhamentoNAAPACommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA;

        public ExcluirRespostaNovoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioRespostaNovoEncaminhamentoNAAPA repositorioRespostaNovoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRespostaNovoEncaminhamentoNAAPA = repositorioRespostaNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioRespostaNovoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirRespostaNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            request.Resposta.Excluido = true;
            var arquivoId = request.Resposta.ArquivoId;
            request.Resposta.ArquivoId = null;

            await repositorioRespostaNovoEncaminhamentoNAAPA.SalvarAsync(request.Resposta);

            if (arquivoId.HasValue)
                await RemoverArquivo(arquivoId);

            return true;

        }

        private async Task RemoverArquivo(long? arquivoId)
        {
            await mediator.Send(new ExcluirArquivoPorIdCommand(arquivoId.Value));
        }
    }
}