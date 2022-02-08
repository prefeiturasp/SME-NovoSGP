using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQueryHandler : IRequestHandler<ObterPorFechamentosTurmaQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;

        public ObterPorFechamentosTurmaQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }
        public async Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterPorFechamentosTurmaQuery request, CancellationToken cancellationToken)
             => await repositorioFechamentoNota.ObterPorFechamentosTurma(request.Ids);
    }
}
