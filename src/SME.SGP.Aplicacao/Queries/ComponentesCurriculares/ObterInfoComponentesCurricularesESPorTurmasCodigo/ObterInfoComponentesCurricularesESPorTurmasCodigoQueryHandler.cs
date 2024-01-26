using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler : IRequestHandler<ObterInfoComponentesCurricularesESPorTurmasCodigoQuery, IEnumerable<InfoComponenteCurricular>>
    {
        private readonly IMediator mediator;
        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<InfoComponenteCurricular>> Handle(ObterInfoComponentesCurricularesESPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            var turmas = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(request.CodigosDeTurmas), cancellationToken);
            var componentesDaTurma = new List<RetornoConsultaListagemTurmaComponenteDto>();

            foreach (var turma in turmas)
            {
                var dataReferenciaInicioAnoLetivo = turma.Historica ? new DateTime(turma.AnoLetivo, 2, 5) : DateTimeExtension.HorarioBrasilia().Date;
                var componentesObtidos = (await mediator.Send(new ObterTurmasComComponentesQuery(turma, 0, 0, dataReferenciaInicioAnoLetivo), cancellationToken))?.Items;

                if (componentesObtidos != null && componentesObtidos.Any())
                    componentesDaTurma.AddRange(componentesObtidos);
                else
                    throw new NegocioException($"Não foram retornados items ao obter turmas com componentes. Turma: {turma.CodigoTurma}.");
            }

            var codigosComponentes = componentesDaTurma.Select(x => x.ComponenteCurricularCodigo).Distinct().ToArray();

            var componentesCurricularesSgp = await mediator.Send(new ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(codigosComponentes), cancellationToken);
            return componentesCurricularesSgp;
        }
    }
}
