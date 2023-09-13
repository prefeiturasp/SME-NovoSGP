using MediatR;
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
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirEncaminhamentoNAAPACommandHandler : IRequestHandler<ExcluirEncaminhamentoNAAPACommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var idEntidadeExcluida = await ExcluirEncaminhamentoNAAPA(request.EncaminhamentoNAAPAId);

            return idEntidadeExcluida != 0;
        }

        private async Task<long> ExcluirEncaminhamentoNAAPA(long encaminhamentoNAAPAId)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await ExcluirArquivos(encaminhamentoNAAPAId);

                    var idEntidadeExcluida = await repositorioEncaminhamentoNAAPA.RemoverLogico(encaminhamentoNAAPAId);

                    var secoes = await mediator.Send(new ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand(encaminhamentoNAAPAId));

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
        private async Task ExcluirArquivos(long encaminhamentoNAAPAId)
        {
            var codigos = await repositorioEncaminhamentoNAAPA.ObterCodigoArquivoPorEncaminhamentoNAAPAId(encaminhamentoNAAPAId);
            if (codigos !=null && codigos.Any())
            {
                foreach (var item in codigos)
                {
                    var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(item.Codigo));
                    if (entidadeArquivo.EhNulo())
                        throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);

                    
                    var extencao = Path.GetExtension(entidadeArquivo.Nome);

                    var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao};
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));

                }
            }

        }
    }
}
