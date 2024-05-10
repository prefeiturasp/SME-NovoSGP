using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarHistoricoConceitoCommandHandler : IRequestHandler<SalvarHistoricoConceitoCommand, long>
    {
        private readonly IRepositorioHistoricoNota repositorioHistoricoNota;

        public SalvarHistoricoConceitoCommandHandler(IRepositorioHistoricoNota repositorioHistoricoNota)
        {
            this.repositorioHistoricoNota = repositorioHistoricoNota ?? throw new ArgumentNullException(nameof(repositorioHistoricoNota));
        }

        public async Task<long> Handle(SalvarHistoricoConceitoCommand request, CancellationToken cancellationToken)
        {
            var historicoNota = MapearParaEntidade(request);

            return await repositorioHistoricoNota.SalvarAsync(historicoNota);
        }

        private HistoricoNota MapearParaEntidade(SalvarHistoricoConceitoCommand request)
            => new HistoricoNota()
            {
                ConceitoAnteriorId = request.ConceitoAnteriorId,
                ConceitoNovoId = request.ConceitoNovoId,
                CriadoRF = request.CriadoRF,
                CriadoPor = request.CriadoPor
            };
    }
}
