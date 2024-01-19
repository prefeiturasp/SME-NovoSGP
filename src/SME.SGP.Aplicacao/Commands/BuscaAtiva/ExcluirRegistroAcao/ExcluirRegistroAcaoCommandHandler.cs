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
    public class ExcluirRegistroAcaoCommandHandler : IRequestHandler<ExcluirRegistroAcaoCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirRegistroAcaoCommandHandler(IMediator mediator, IRepositorioRegistroAcaoBuscaAtiva repositorioRegistroAcao, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroAcao = repositorioRegistroAcao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirRegistroAcaoCommand request, CancellationToken cancellationToken)
        {
            var idEntidadeExcluida = await ExcluirRegistroAcao(request.RegistroAcaoId);
            return idEntidadeExcluida != 0;
        }

        private async Task<long> ExcluirRegistroAcao(long registroAcaoId)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await ExcluirArquivos(registroAcaoId);
                    var idEntidadeExcluida = await repositorioRegistroAcao.RemoverLogico(registroAcaoId);
                    var secoes = await mediator.Send(new ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand(registroAcaoId));
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
        private async Task ExcluirArquivos(long registroAcaoId)
        {
            var codigos = await repositorioRegistroAcao.ObterCodigoArquivoPorRegistroAcaoId(registroAcaoId);
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
