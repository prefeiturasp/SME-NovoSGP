using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.UE.ObterUEsPorDRE
{
    public class 
        ObterUEsPorDREQueryHandler : IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterUEsPorDREQueryHandler(IMediator mediator, IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Handle(ObterUEsPorDREQuery request, CancellationToken cancellationToken)
        {
            var anoNovosTiposUE = ObterAno(request.ConsideraNovasUEs, request.AnoLetivo);
            var parametroNovosTiposUE = await mediator.Send(new ObterNovosTiposUEPorAnoQuery(anoNovosTiposUE));
            var novosTiposUE = parametroNovosTiposUE?.Split(',').Select(a => int.Parse(a)).ToArray();

            var ues = await repositorioAbrangencia.ObterUes(request.CodigoDre, request.Login, request.Perfil,
                new FiltroModalidade(request.Modalidade ?? 0, request.AnosTurma),
                new FiltroPeriodoLetivo(request.AnoLetivo, request.ConsideraHistorico, request.Periodo),
                novosTiposUE,
                new FiltroTexto(request.Filtro, request.FiltroEhCodigo));

            if (request.FiltrarTipoEscolaPorAnoLetivo && request.AnoLetivo <= 2020)
                ues = ues.Where(u => !u.EhInfantil);

            return ues.OrderBy(c => c.Nome);
        }

        private int ObterAno(bool consideraNovasUEs, int anoLetivo)
        {
            var ano = anoLetivo > 0 ? anoLetivo : DateTime.Today.Year;
            return consideraNovasUEs ? ano + 1 : ano;
        }
    }
}
