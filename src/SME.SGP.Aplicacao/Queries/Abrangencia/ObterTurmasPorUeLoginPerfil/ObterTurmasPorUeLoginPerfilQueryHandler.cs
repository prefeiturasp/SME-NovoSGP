using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorUeLoginPerfilQueryHandler : IRequestHandler<ObterTurmasPorUeLoginPerfilQuery, IEnumerable<AbrangenciaTurmaRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IMediator mediator;

        public ObterTurmasPorUeLoginPerfilQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterTurmasPorUeLoginPerfilQuery request, CancellationToken cancellationToken)
        {
            var anosInfantilDesconsiderar = await mediator.Send(
                new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(request.AnoLetivo, Modalidade.EducacaoInfantil));

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(
                request.CodigoUe, request.Login, request.Perfil,
                request.Modalidade,
                request.Tipos != null && request.Tipos.Any() ? request.Tipos : null,
                request.Periodo, request.ConsideraHistorico, request.AnoLetivo,
                anosInfantilDesconsiderar);

            var codigosTurmas = result?.Select(t => t.Codigo.ToString()).ToList();
            var listaTurmaEOL = await mediator.Send(new ObterTurmasApiEolQuery(codigosTurmas));

            var codigosValidos = new HashSet<string>(listaTurmaEOL.Select(x => x.Codigo.ToString()));

            var resultAtualizado = result
                .Where(x => !string.IsNullOrEmpty(x.Codigo) && codigosValidos.Contains(x.Codigo))
                .ToList();

            return OrdernarTurmasItinerario(resultAtualizado);
        }

        private IEnumerable<AbrangenciaTurmaRetorno> OrdernarTurmasItinerario(IEnumerable<AbrangenciaTurmaRetorno> result)
        {
            var turmasItinerario = result.Where(t => t.TipoTurma == (int)TipoTurma.Itinerarios2AAno || t.TipoTurma == (int)TipoTurma.ItinerarioEnsMedio);
            var turmas = result.Where(t => !turmasItinerario.Any(ti => ti.Id == t.Id));

            return turmas.OrderBy(a => a.ModalidadeTurmaNome)
                .Concat(turmasItinerario.OrderBy(a => a.ModalidadeTurmaNome));
        }
    }
}
