using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorAnoLetivoEModalidadeQueryHandler : IRequestHandler<ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery, long>
    {
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ObterTipoCalendarioIdPorAnoLetivoEModalidadeQueryHandler(IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async Task<long> Handle(ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery request, CancellationToken cancellationToken)
            => await repositorioTipoCalendario.ObterIdPorAnoLetivoEModalidadeAsync(request.AnoLetivo, request.Modalidade, request.Semestre);
    }
}
