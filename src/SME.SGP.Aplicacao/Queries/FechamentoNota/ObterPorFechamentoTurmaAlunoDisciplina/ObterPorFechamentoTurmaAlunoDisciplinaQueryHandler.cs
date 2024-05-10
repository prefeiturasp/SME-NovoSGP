using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentoTurmaAlunoDisciplinaQueryHandler : IRequestHandler<ObterPorFechamentoTurmaAlunoDisciplinaQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;

        public ObterPorFechamentoTurmaAlunoDisciplinaQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
            IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterPorFechamentoTurmaAlunoDisciplinaQuery request, CancellationToken cancellationToken)
             => await repositorioFechamentoNota.ObterPorFechamentoTurmaAlunoEDisciplinaAsync(request.Id, request.AlunoCodigo, request.ComponenteCurricularId);
    }
}