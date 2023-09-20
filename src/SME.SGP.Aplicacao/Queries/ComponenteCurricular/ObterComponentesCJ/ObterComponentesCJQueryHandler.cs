using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCJQueryHandler : IRequestHandler<ObterComponentesCJQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IMediator mediator;

        public ObterComponentesCJQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                                              IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,
                                              IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCJQuery request, CancellationToken cancellationToken)
        {
            var codigosTerritorioEquivalentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(request.ComponenteCurricular, request.TurmaCodigo, null));

            var codigoTerritorioConsiderado = codigosTerritorioEquivalentes.NaoEhNulo() ?
                codigosTerritorioEquivalentes.OrderBy(c => c.codigoComponente.Length).First().codigoComponente : null;

            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(request.Modalidade,
                request.TurmaCodigo,
                request.UeCodigo,
                !string.IsNullOrWhiteSpace(codigoTerritorioConsiderado) ? long.Parse(codigoTerritorioConsiderado) : request.ComponenteCurricular,
                request.ProfessorRf,
                string.Empty,
                true);

            if (atribuicoes.EhNulo() || !atribuicoes.Any())
                return null;

            var disciplinasEol = await repositorioComponenteCurricular.ObterDisciplinasPorIds(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            IEnumerable<ComponenteCurricularEol> componentes = null;

            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);

            if (request.ListarComponentesPlanejamento && componenteRegencia.NaoEhNulo())
            {
                var componentesRegencia = await repositorioComponenteCurricular.ObterDisciplinasPorIds(IDS_COMPONENTES_REGENCIA);
                if (componentesRegencia.NaoEhNulo())
                    componentes = TransformarListaDisciplinaEolParaRetornoDto(componentesRegencia);
            }
            else
                componentes = TransformarListaDisciplinaEolParaRetornoDto(disciplinasEol);
            return componentes;
        }

        private IEnumerable<ComponenteCurricularEol> TransformarListaDisciplinaEolParaRetornoDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
            {
                yield return MapearDisciplinaResposta(disciplinaEol);
            }
        }

        private ComponenteCurricularEol MapearDisciplinaResposta(DisciplinaDto disciplinaEol) => new ComponenteCurricularEol()
        {
            Codigo = disciplinaEol.CodigoComponenteCurricular,
            CodigoComponenteCurricularPai = disciplinaEol.CdComponenteCurricularPai,
            Descricao = disciplinaEol.Nome,
            Regencia = disciplinaEol.Regencia,
            Compartilhada = disciplinaEol.Compartilhada,
            RegistraFrequencia = disciplinaEol.RegistraFrequencia,
            LancaNota = disciplinaEol.LancaNota,
        };

    }
}
