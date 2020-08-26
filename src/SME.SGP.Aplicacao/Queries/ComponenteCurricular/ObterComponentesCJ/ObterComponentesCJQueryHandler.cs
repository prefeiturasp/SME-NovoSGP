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
        private readonly IServicoEol servicoEOL;

        public ObterComponentesCJQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                                              IServicoEol servicoEOL)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCJQuery request, CancellationToken cancellationToken)
        {
            var atribuicoes = await repositorioAtribuicaoCJ.ObterPorFiltros(request.Modalidade,
                request.TurmaCodigo,
                request.UeCodigo,
                request.ComponenteCurricular,
                request.ProfessorRf,
                string.Empty,
                true);

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await servicoEOL.ObterDisciplinasPorIdsAsync(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray());

            IEnumerable<ComponenteCurricularEol> componentes = null;

            var componenteRegencia = disciplinasEol?.FirstOrDefault(c => c.Regencia);

            if (request.ListarComponentesPlanejamento && componenteRegencia != null)
            {
                var componentesRegencia = await servicoEOL.ObterDisciplinasPorIdsAsync(IDS_COMPONENTES_REGENCIA);
                if (componentesRegencia != null)
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
