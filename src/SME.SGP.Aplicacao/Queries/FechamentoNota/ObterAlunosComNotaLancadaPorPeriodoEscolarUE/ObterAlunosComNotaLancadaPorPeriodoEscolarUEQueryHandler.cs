using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComNotaLancadaPorPeriodoEscolarUEQueryHandler : IRequestHandler<ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery, IEnumerable<AlunosFechamentoNotaDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;

        public ObterAlunosComNotaLancadaPorPeriodoEscolarUEQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<AlunosFechamentoNotaDto>> Handle(ObterAlunosComNotaLancadaPorPeriodoEscolarUEQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoNota.ObterComNotaLancadaPorPeriodoEscolarUE(request.UeId, request.PeriodoEscolarId);
        }
    }
}
