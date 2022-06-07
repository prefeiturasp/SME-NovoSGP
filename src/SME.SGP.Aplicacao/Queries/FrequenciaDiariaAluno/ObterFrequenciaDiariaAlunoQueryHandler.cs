using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaDiariaAlunoQueryHandler : ConsultasBase, IRequestHandler<ObterFrequenciaDiariaAlunoQuery, PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
        private IMediator mediator { get; }
        private readonly IRepositorioFrequenciaDiariaAluno repositorioFrequenciaDiaria;
        public ObterFrequenciaDiariaAlunoQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioFrequenciaDiariaAluno repositorioFrequenciaDiaria) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaDiaria = repositorioFrequenciaDiaria ?? throw new ArgumentNullException(nameof(repositorioFrequenciaDiaria));
        }

        public async Task<PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>> Handle(ObterFrequenciaDiariaAlunoQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
