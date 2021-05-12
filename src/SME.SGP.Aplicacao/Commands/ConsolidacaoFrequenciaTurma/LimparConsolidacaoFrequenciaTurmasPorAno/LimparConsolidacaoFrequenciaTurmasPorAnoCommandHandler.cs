using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoFrequenciaTurmasPorAnoCommandHandler : IRequestHandler<LimparConsolidacaoFrequenciaTurmasPorAnoCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorio;

        public LimparConsolidacaoFrequenciaTurmasPorAnoCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(LimparConsolidacaoFrequenciaTurmasPorAnoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.LimparConsolidacaoFrequenciasTurmasPorAno(request.Ano);

            return true;
        }
    }
}
