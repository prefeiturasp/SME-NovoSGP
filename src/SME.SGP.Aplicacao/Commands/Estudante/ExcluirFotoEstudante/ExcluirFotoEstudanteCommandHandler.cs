using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoEstudanteCommandHandler : IRequestHandler<ExcluirFotoEstudanteCommand, bool>
    {
        private readonly IRepositorioAlunoFoto repositorio;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirFotoEstudanteCommandHandler(IRepositorioAlunoFoto repositorio, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirFotoEstudanteCommand request, CancellationToken cancellationToken)
        {
            var fotoAluno = await mediator.Send(new ObterMiniaturaFotoEstudantePorAlunoCodigoQuery(request.AlunoCodigo));

            if (fotoAluno == null)
                throw new NegocioException("Não foi encontrada nenhuma foto para o aluno");

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await ExcluirFoto(fotoAluno.FotoId, fotoAluno.ArquivoId);
                    await ExcluirFoto(fotoAluno.MiniaturaId, fotoAluno.MiniaturaArquivoId);

                    unitOfWork.PersistirTransacao();

                    return true;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task ExcluirFoto(long id, long arquivoId)
        {
            repositorio.Remover(id);
            await mediator.Send(new ExcluirArquivoPorIdCommand(arquivoId));
        }
    }
}
