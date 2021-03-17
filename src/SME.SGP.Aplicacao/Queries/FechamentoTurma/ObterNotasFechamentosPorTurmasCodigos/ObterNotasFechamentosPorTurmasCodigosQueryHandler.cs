using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosQueryHandler : IRequestHandler<ObterNotasFechamentosPorTurmasCodigosQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;

        public ObterNotasFechamentosPorTurmasCodigosQueryHandler(IRepositorioFechamentoNota repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFechamentosPorTurmasCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoNota.ObterNotasAlunoPorTurmasCodigosAsync(request.TurmasCodigos, request.AlunoCodigo);
        }

    }
}
