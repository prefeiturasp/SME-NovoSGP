using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommandHandler : IRequestHandler<SalvarHistoricoNotaCommand, AuditoriaDto>
    {
        private readonly IRepositorioHistoricoNota repositorioHistoricoNota;

        public SalvarHistoricoNotaCommandHandler(IRepositorioHistoricoNota repositorioHistoricoNota)
        {
            this.repositorioHistoricoNota = repositorioHistoricoNota ?? throw new ArgumentNullException(nameof(repositorioHistoricoNota));
        }
        public async Task<AuditoriaDto> Handle(SalvarHistoricoNotaCommand request, CancellationToken cancellationToken)
        {
            var historicoNota = MapearParaEntidade(request);

            await repositorioHistoricoNota.SalvarAsync(historicoNota);

            return (AuditoriaDto)historicoNota;
        }

        private HistoricoNota MapearParaEntidade(SalvarHistoricoNotaCommand request)
            => new HistoricoNota()
            {
                NotaAnterior = request.NotaAnterior,
                NotaNova = request.NotaNova
            };
    }
}
