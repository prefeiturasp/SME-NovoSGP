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
    public class ExcluirMapeamentoEstudanteCommandHandler : IRequestHandler<ExcluirMapeamentoEstudanteCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioMapeamentoEstudante repositorioMapeamentoEstudante { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirMapeamentoEstudanteCommandHandler(IMediator mediator, IRepositorioMapeamentoEstudante repositorioMapeamentoEstudante, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioMapeamentoEstudante = repositorioMapeamentoEstudante ?? throw new ArgumentNullException(nameof(repositorioMapeamentoEstudante));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirMapeamentoEstudanteCommand request, CancellationToken cancellationToken)
        {
            var idEntidadeExcluida = await ExcluirMapeamentoEstudante(request.MapeamentoEstudanteId);
            return idEntidadeExcluida != 0;
        }

        private async Task<long> ExcluirMapeamentoEstudante(long mapeamentoEstudanteId)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await ExcluirArquivos(mapeamentoEstudanteId);
                    var idEntidadeExcluida = await repositorioMapeamentoEstudante.RemoverLogico(mapeamentoEstudanteId);
                    var secoes = await mediator.Send(new ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand(mapeamentoEstudanteId));
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
        private async Task ExcluirArquivos(long mapeamentoEstudanteId)
        {
            var codigos = await repositorioMapeamentoEstudante.ObterCodigoArquivoPorMapeamentoEstudanteId(mapeamentoEstudanteId);
            if (codigos.NaoEhNulo() && codigos.Any())
            {
                foreach (var item in codigos)
                {
                    var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(new Guid(item)));
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
