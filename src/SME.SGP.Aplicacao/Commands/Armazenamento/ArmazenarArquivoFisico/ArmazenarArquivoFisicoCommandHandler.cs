﻿using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ArmazenarArquivoFisicoCommandHandler : IRequestHandler<ArmazenarArquivoFisicoCommand, string>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;
        private readonly IMediator mediator;

        public ArmazenarArquivoFisicoCommandHandler(IServicoArmazenamento servicoArmazenamento, IMediator mediator)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Handle(ArmazenarArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            var enderecoArquivo = string.Empty;

            var stream = request.Arquivo.OpenReadStream();
            
            var nomeArquivo = $"{request.NomeFisico}{Path.GetExtension(request.Arquivo.FileName)}";

            if (request.TipoArquivo == TipoArquivo.temp || request.TipoArquivo == TipoArquivo.Editor)
                enderecoArquivo =
                    await servicoArmazenamento.ArmazenarTemporaria(nomeArquivo, stream, request.Arquivo.ContentType);
            else
                enderecoArquivo = await servicoArmazenamento.Armazenar(nomeArquivo, stream, request.Arquivo.ContentType);
             
            return enderecoArquivo;
        }
    }
}
