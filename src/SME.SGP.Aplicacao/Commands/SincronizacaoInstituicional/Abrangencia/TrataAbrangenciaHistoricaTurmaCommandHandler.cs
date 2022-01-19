using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataAbrangenciaHistoricaTurmaCommandHandler : IRequestHandler<TrataAbrangenciaHistoricaTurmaCommand, bool>
    {
        private readonly IServicoAbrangencia servicoAbrangencia;
        
        public TrataAbrangenciaHistoricaTurmaCommandHandler(IServicoAbrangencia servicoAbrangencia)
        {
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
        }

        public async Task<bool> Handle(TrataAbrangenciaHistoricaTurmaCommand request, CancellationToken cancellationToken)
        {
            return await servicoAbrangencia.SincronizarAbrangenciaHistorica(request.AnoLetivo, request.ProfessorRf);
        }
    }
}
