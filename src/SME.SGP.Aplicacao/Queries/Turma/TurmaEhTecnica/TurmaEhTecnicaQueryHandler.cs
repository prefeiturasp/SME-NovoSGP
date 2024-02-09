using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TurmaEhTecnicaQueryHandler : IRequestHandler<TurmaEhTecnicaQuery, bool>
    {
        private readonly int[] etapasEnsinoTecnico;
        private readonly IMediator mediator;

        public TurmaEhTecnicaQueryHandler(IMediator mediator)
        {
            etapasEnsinoTecnico = new int[] { (int)EtapaEnsino.TenicoMedio, (int)EtapaEnsino.QualificacaoProfissional };
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(TurmaEhTecnicaQuery request, CancellationToken cancellationToken)
        {
            var componentes = await mediator.Send(new ObterTurmasComComponentesQuery(request.Turma, 0, 0, request.DataReferencia), cancellationToken);
            var etapaEnsino = componentes.Items?.FirstOrDefault()?.EtapaEnsino;
            return etapaEnsino.HasValue && etapasEnsinoTecnico.Contains(etapaEnsino.Value);
        }
    }
}
