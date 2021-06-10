using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoAcompanhamentoAlunoCommandHandler : IRequestHandler<ExcluirFotoAcompanhamentoAlunoCommand, AuditoriaDto>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirFotoAcompanhamentoAlunoCommandHandler(IRepositorioAcompanhamentoAlunoFoto repositorio, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(ExcluirFotoAcompanhamentoAlunoCommand request, CancellationToken cancellationToken)
        {
            var acompanhamentoSemestre = await mediator.Send(new ObterAcompanhamentoAlunoSemestrePorIdQuery(request.Acompanhamento.AcompanhamentoAlunoSemestreId));

            var arquivoComplementar = request.Acompanhamento.MiniaturaId.HasValue ?
                await repositorio.ObterPorIdAsync(request.Acompanhamento.MiniaturaId.Value) :
                await repositorio.ObterFotoPorMiniaturaId(request.Acompanhamento.Id);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    if (request.Acompanhamento.MiniaturaId.HasValue)
                    {
                        await ExcluirFoto(request.Acompanhamento);
                        await ExcluirFoto(arquivoComplementar);
                    }
                    else
                    {
                        await ExcluirFoto(arquivoComplementar);
                        await ExcluirFoto(request.Acompanhamento);
                    }

                    await mediator.Send(new SalvarAcompanhamentoAlunoSemestreCommand(acompanhamentoSemestre));

                    unitOfWork.PersistirTransacao();

                    return (AuditoriaDto)acompanhamentoSemestre;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task ExcluirFoto(AcompanhamentoAlunoFoto foto)
        {
            repositorio.Remover(foto);
            await mediator.Send(new ExcluirArquivoPorIdCommand(foto.ArquivoId));
        }
    }
}
