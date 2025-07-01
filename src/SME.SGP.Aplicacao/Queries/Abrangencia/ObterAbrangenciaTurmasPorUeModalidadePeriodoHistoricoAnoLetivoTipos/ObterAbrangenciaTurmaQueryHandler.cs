using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler : IRequestHandler<ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery request,CancellationToken cancellationToken)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var anosInfantilDesconsiderar = !request.ConsideraNovosAnosInfantil
                ? await mediator.Send(
                    new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(request.AnoLetivo,
                        Modalidade.EducacaoInfantil))
                : null;

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(request.CodigoUe, login, perfil,
                request.Modalidade, request.Tipos.NaoEhNulo() && request.Tipos.Any() ? request.Tipos : null, request.Periodo,
                request.ConsideraHistorico, request.AnoLetivo, anosInfantilDesconsiderar);

            var codigosTurmas = result?.Select(t => t.Codigo.ToString())?.ToList();
            var listaTurmaEOL = await mediator.Send(new ObterTurmasApiEolQuery(codigosTurmas));

            var codigosValidos = new HashSet<string>(
                listaTurmaEOL.Select(x => x.Codigo.ToString())
            );

            var resultatualizado = result
                .Where(x => !string.IsNullOrEmpty(x.Codigo) && codigosValidos.Contains(x.Codigo))
                .ToList();

            return OrdernarTurmasItinerario(resultatualizado);
        }  
 
        private IEnumerable<AbrangenciaTurmaRetorno> OrdernarTurmasItinerario(IEnumerable<AbrangenciaTurmaRetorno> result)
        {
            List<AbrangenciaTurmaRetorno> turmasOrdenadas = new List<AbrangenciaTurmaRetorno>();

            var turmasItinerario = result.Where(t => t.TipoTurma == (int)TipoTurma.Itinerarios2AAno || t.TipoTurma == (int)TipoTurma.ItinerarioEnsMedio);
            var turmas = result.Where(t => !turmasItinerario.Any(ti => ti.Id == t.Id));

            turmasOrdenadas.AddRange(turmas.OrderBy(a => a.ModalidadeTurmaNome));
            turmasOrdenadas.AddRange(turmasItinerario.OrderBy(a => a.ModalidadeTurmaNome));

            return turmasOrdenadas;
        }
    }
}
