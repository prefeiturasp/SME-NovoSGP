using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoFechamentoTurmaDisciplinaCommandHandler : IRequestHandler<AtualizarSituacaoFechamentoTurmaDisciplinaCommand, bool>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public AtualizarSituacaoFechamentoTurmaDisciplinaCommandHandler(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<bool> Handle(AtualizarSituacaoFechamentoTurmaDisciplinaCommand request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.AtualizarSituacaoFechamento(request.FechamentoTurmaDisciplinaId, (int)request.SituacaoFechamento);
    }
}
