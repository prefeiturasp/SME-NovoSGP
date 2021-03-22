using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasPorTurmasComponentePeriodoQueryHandler : IRequestHandler<ObterAulasDadasPorTurmasComponentePeriodoQuery, int>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ObterAulasDadasPorTurmasComponentePeriodoQueryHandler(IRepositorioAula repositorioAula, IRepositorioTipoCalendario repositorioTipoCalendario, IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<int> Handle(ObterAulasDadasPorTurmasComponentePeriodoQuery request, CancellationToken cancellationToken)
        {
            //var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(request.TurmasCodigo));
            //var tipoDeCalendarios = new List<>
            //var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeParaModalidadeTipoCalendario(turmaModalidade), semestre);
            throw new NotImplementedException();
        }
    }
}
