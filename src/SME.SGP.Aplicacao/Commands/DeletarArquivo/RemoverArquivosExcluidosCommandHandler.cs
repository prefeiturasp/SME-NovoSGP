﻿using System;
using MediatR;
using SME.SGP.Infra;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class RemoverArquivosExcluidosCommandHandler : IRequestHandler<RemoverArquivosExcluidosCommand, bool>
    {
        private readonly IMediator mediator;

        public RemoverArquivosExcluidosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RemoverArquivosExcluidosCommand request, CancellationToken cancellationToken)
        {
            var arquivoAtual = request.ArquivoAtual.Replace(@"\", @"/");
            var arquivoNovo = request.ArquivoNovo.Replace(@"\", @"/");
            var regex = new Regex(ArmazenamentoObjetos.EXPRESSAO_NOME_ARQUIVO);
            var atual = regex.Matches(arquivoAtual).Cast<Match>().Select(c => c.Value).ToList();
            var novo = regex.Matches(arquivoNovo).Cast<Match>().Select(c => c.Value).ToList();
            var diferente = atual.Except(novo);

            foreach (var item in diferente)
            {
                var filtro = new FiltroExcluirArquivoArmazenamentoDto{ArquivoNome = item};
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null), cancellationToken);
            }

            return true;
        }
    }
}