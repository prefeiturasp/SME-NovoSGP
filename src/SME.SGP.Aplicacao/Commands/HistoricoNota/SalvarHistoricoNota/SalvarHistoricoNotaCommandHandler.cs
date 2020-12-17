using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoNotaCommandHandler : IRequestHandler<SalvarHistoricoNotaCommand, long>
    {
        private readonly IRepositorioHistoricoNota repositorioHistoricoNota;

        public SalvarHistoricoNotaCommandHandler(IRepositorioHistoricoNota repositorioHistoricoNota)
        {
            this.repositorioHistoricoNota = repositorioHistoricoNota ?? throw new ArgumentNullException(nameof(repositorioHistoricoNota));
        }
        public async Task<long> Handle(SalvarHistoricoNotaCommand request, CancellationToken cancellationToken)
        {
            var historicoNota = MapearParaEntidade(request);

            return await repositorioHistoricoNota.SalvarAsync(historicoNota);
        }

        private HistoricoNota MapearParaEntidade(SalvarHistoricoNotaCommand request)
            => new HistoricoNota()
            {
                NotaAnterior = request.NotaAnterior,
                NotaNova = request.NotaNova,
                CriadoRF = request.CriadoRF,
                CriadoPor = request.CriadoPor
            };
    }
}
