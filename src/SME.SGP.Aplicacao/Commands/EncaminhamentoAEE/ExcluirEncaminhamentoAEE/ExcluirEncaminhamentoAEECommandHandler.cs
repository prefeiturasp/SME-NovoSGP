﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirEncaminhamentoAEECommandHandler : IRequestHandler<ExcluirEncaminhamentoAEECommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var idEntidadeExcluida = await ExcluirEncaminhamentoAEE(request.EncaminhamentoAeeId);

            return idEntidadeExcluida != 0;
        }

        private async Task<long> ExcluirEncaminhamentoAEE(long encaminhamentoAeeId)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await ExcluirArquivos(encaminhamentoAeeId);

                    var idEntidadeExcluida = await repositorioEncaminhamentoAEE.RemoverLogico(encaminhamentoAeeId);

                    var secoes = await mediator.Send(new ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand(encaminhamentoAeeId));

                    unitOfWork.PersistirTransacao();

                    return idEntidadeExcluida;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();

                    throw;
                }
            }
        }
        private async Task ExcluirArquivos(long encaminhamentoAeeId)
        {
            var codigos = await repositorioEncaminhamentoAEE.ObterCodigoArquivoPorEncaminhamentoAEEId(encaminhamentoAeeId);
            if (codigos.NaoEhNulo() && codigos.Any())
            {
                foreach (var item in codigos)
                {
                    var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(item.Codigo));
                    if (entidadeArquivo.EhNulo())
                        throw new NegocioException("O arquivo informado não foi encontrado");

                    
                    var extencao = Path.GetExtension(entidadeArquivo.Nome);

                    var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));

                }
            }

        }
    }
}
