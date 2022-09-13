﻿using System;
using System.IO;
using MediatR;
using SME.SGP.Dominio;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDocumentoUseCase : AbstractUseCase, IExcluirDocumentoUseCase
    {
        public ExcluirDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long documentoId)
        {
            var entidadeDocumento = await mediator.Send(new ObterDocumentoPorIdQuery(documentoId));

            if (entidadeDocumento == null)
                throw new NegocioException("O documento informado não foi encontrado");

            if(entidadeDocumento.ArquivoId != null)
            {
                var entidadeArquivo = await mediator.Send(new ObterArquivoPorIdQuery(entidadeDocumento.ArquivoId.GetValueOrDefault()));

                if (entidadeArquivo == null)
                    throw new NegocioException("O arquivo relacionado não foi encontrado");

                await mediator.Send(new ExcluirReferenciaArquivoDocumentoPorArquivoIdCommand(documentoId, entidadeArquivo.Id));
                await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));
                
                
                var extencao = Path.GetExtension(entidadeArquivo.Nome);

                var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
                
            }

            await mediator.Send(new ExcluirDocumentoPorIdCommand(documentoId));

            return true;
        }
    }
}
