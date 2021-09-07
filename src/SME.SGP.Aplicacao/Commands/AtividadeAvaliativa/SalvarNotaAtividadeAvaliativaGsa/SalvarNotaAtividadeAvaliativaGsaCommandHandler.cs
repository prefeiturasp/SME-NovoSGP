using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class
        SalvarNotaAtividadeAvaliativaGsaCommandHandler : AsyncRequestHandler<SalvarNotaAtividadeAvaliativaGsaCommand>
    {
        private readonly IRepositorioNotasConceitos repositorioConceitos;
        private readonly IMediator mediator;

        public SalvarNotaAtividadeAvaliativaGsaCommandHandler(
            IRepositorioNotasConceitos repositorioConceitos, IMediator mediator)
        {
            this.repositorioConceitos =
                repositorioConceitos ?? throw new ArgumentNullException(nameof(repositorioConceitos));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(SalvarNotaAtividadeAvaliativaGsaCommand request,
            CancellationToken cancellationToken)
        {
            if (request.NotaConceito != null)
                await AlterarAtividade(request.NotaConceito, request);
            else
                await InserirAtividade(request.NotaConceito, request);
        }

        private async Task InserirAtividade(NotaConceito notaConceito, SalvarNotaAtividadeAvaliativaGsaCommand request)
        {
            if (!request.TipoNota.EhNota())
                notaConceito.ConceitoId = ObterConteceitoPorNota((long?)request.Nota);
            else
                notaConceito.Nota = request.Nota;
            notaConceito.StatusGsa = request.StatusGsa;

            await repositorioConceitos.SalvarAsync(notaConceito);
        }
        private long? ObterConteceitoPorNota(long? notaValor)
        {
            if (notaValor < 5)
                return (long)ConceitoValores.NS;
            else if (notaValor >= 5 && notaValor < 7)
                return (long)ConceitoValores.S;
            else if (notaValor > 7)
                return (long)ConceitoValores.P;
            return null;
        }
        private async Task AlterarAtividade(NotaConceito notaConceito,
            SalvarNotaAtividadeAvaliativaGsaCommand request)
        {
            if (notaConceito.TipoNota == TipoNota.Conceito)
            {
                notaConceito.ConceitoId = ObterConteceitoPorNota((long?)request.Nota);
            }
            else
            {
                notaConceito.Nota = request.Nota;
            }

            notaConceito.StatusGsa = request.StatusGsa;

            await repositorioConceitos.SalvarAsync(notaConceito);
        }
    }
}