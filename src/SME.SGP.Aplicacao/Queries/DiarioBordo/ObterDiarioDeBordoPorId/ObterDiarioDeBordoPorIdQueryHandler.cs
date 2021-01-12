using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioDeBordoPorIdQueryHandler : IRequestHandler<ObterDiarioDeBordoPorIdQuery, DiarioBordoDetalhesDto>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;

        public ObterDiarioDeBordoPorIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DiarioBordoDetalhesDto> Handle(ObterDiarioDeBordoPorIdQuery request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorIdAsync(request.Id);
            var usuario = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            var observacoes = await mediator.Send(new ListarObservacaoDiarioBordoQuery(diarioBordo.Id, usuario));

            return MapearParaDto(diarioBordo, observacoes);
        }

        private DiarioBordoDetalhesDto MapearParaDto(Dominio.DiarioBordo diarioBordo, IEnumerable<ListarObservacaoDiarioBordoDto> observacoes)
        {
            throw new NotImplementedException();
        }
    }
}
