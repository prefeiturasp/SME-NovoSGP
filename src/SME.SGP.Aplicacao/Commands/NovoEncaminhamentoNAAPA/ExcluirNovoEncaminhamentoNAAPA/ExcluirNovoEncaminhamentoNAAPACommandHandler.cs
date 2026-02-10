using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.ExcluirNovoEncaminhamentoNAAPA
{
    public class ExcluirNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<ExcluirNovoEncaminhamentoNAAPACommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirNovoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
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

                    var idEntidadeExcluida = await repositorioNovoEncaminhamentoNAAPA.RemoverLogico(encaminhamentoNAAPAId);

                    var secoes = await mediator.Send(new ExcluirSecoesAtendimentoNAAPAPorAtendimentoIdCommand(encaminhamentoNAAPAId));

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
            var codigos = await repositorioNovoEncaminhamentoNAAPA.ObterCodigoArquivoPorEncaminhamentoNAAPAId(encaminhamentoNAAPAId);
            if (codigos.NaoEhNulo() && codigos.Any())
            {
                foreach (var item in codigos)
                {
                    var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(item.Codigo));
                    if (entidadeArquivo.EhNulo())
                        throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);


                    var extencao = Path.GetExtension(entidadeArquivo.Nome);

                    var filtro = new FiltroExcluirArquivoArmazenamentoDto { ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));

                }
            }
        }
    }
}