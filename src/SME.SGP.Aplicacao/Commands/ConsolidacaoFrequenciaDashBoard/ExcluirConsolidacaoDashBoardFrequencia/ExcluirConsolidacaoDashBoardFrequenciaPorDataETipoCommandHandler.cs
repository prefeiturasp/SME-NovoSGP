using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommandHandler : IRequestHandler<ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma;

        public ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommandHandler(IRepositorioConsolidacaoFrequenciaTurma repositorioConsolidacaoFrequenciaTurma)
        {
            this.repositorioConsolidacaoFrequenciaTurma = repositorioConsolidacaoFrequenciaTurma ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoFrequenciaTurma));
        }

        public async Task<bool> Handle(ExcluirConsolidacaoDashBoardFrequenciaPorDataETipoCommand request, CancellationToken cancellationToken)
        {
            await repositorioConsolidacaoFrequenciaTurma.ExcluirConsolidacaoDashBoard(request.AnoLetivo,
                                                                                        request.TurmaId,
                                                                                        request.DataAula,
                                                                                        request.DataInicioSemana,
                                                                                        request.DataFinalSemena,
                                                                                        request.Mes,
                                                                                        request.TipoPeriodo);
            return true;
        }            
    }
}
