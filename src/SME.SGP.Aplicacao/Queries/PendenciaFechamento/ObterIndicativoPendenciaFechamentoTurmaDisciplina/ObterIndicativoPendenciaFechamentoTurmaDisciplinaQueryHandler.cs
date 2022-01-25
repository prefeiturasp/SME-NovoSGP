using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciaFechamentoTurmaDisciplinaQueryHandler : IRequestHandler<ObterIndicativoPendenciaFechamentoTurmaDisciplinaQuery, bool>
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;

        public ObterIndicativoPendenciaFechamentoTurmaDisciplinaQueryHandler(IRepositorioPendenciaFechamento repositorioPendenciaFechamento)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
        }

        public async Task<bool> Handle(ObterIndicativoPendenciaFechamentoTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaFechamento.PossuiPendenciasAbertoPorTurmaDisciplina(request.TurmaId, request.Bimestre, request.DisciplinaId);
        }
    }
}
