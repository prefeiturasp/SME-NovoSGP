using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQueryHandler : IRequestHandler<ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery, IEnumerable<FechamentoReaberturaBimestre>>
    {
        private readonly IRepositorioFechamentoReaberturaBimestre repositorioFechamentoReaberturaBimestre;

        public ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQueryHandler(IRepositorioFechamentoReaberturaBimestre repositorioFechamentoReaberturaBimestre)
        {
            this.repositorioFechamentoReaberturaBimestre = repositorioFechamentoReaberturaBimestre ?? throw new ArgumentNullException(nameof(repositorioFechamentoReaberturaBimestre));
        }
        public async Task<IEnumerable<FechamentoReaberturaBimestre>> Handle(ObterFechamentoReaberturaBimestrePorFechamentoReaberturaIdQuery request, CancellationToken cancellationToken)
         => await repositorioFechamentoReaberturaBimestre.ObterPorFechamentoReaberturaIdAsync(request.FechamentoReaberturaId);
    }
}
