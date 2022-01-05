using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery, IEnumerable<DisciplinaNomeDto>>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<IEnumerable<DisciplinaNomeDto>> Handle(ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery request, CancellationToken cancellationToken)
        {

            if (request.UsuarioLogado.EhProfessorCj())
                return await ObterComponentesAtribuicaoCj(request.TurmaCodigo, request.UsuarioLogado.CodigoRf);
            else
                return await ObterComponentesCurricularesUsuario(request.TurmaCodigo, request.UsuarioLogado.CodigoRf, request.UsuarioLogado.PerfilAtual);
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesCurricularesUsuario(string turmaCodigo, string codigoRf, Guid perfilAtual)
        {
            var obterTurma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            bool realizarAgrupamentoComponente = obterTurma.AnoLetivo != DateTimeExtension.HorarioBrasilia().Year;
            var componentesCurricularesEol = await servicoEol.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(turmaCodigo, codigoRf, perfilAtual, realizarAgrupamentoComponente);

            if (componentesCurricularesEol == null || !componentesCurricularesEol.Any())
                return null;

            return (await ObterComponentesCurricularesRepositorioSgp(componentesCurricularesEol))?
                .OrderBy(c => c.Nome)?.ToList();
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesCurricularesRepositorioSgp(IEnumerable<ComponenteCurricularEol> componentesCurricularesEol)
        {
            var componentesSgp = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurricularesEol.Select(a => a.TerritorioSaber ? a.CodigoComponenteTerritorioSaber : a.Codigo).ToArray()));
            return MapearParaComponenteNomeDto(componentesSgp, componentesCurricularesEol);
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesAtribuicaoCj(string turmaCodigo, string login)
        {
            var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, turmaCodigo, string.Empty, 0, login, string.Empty, true));

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));

            return MapearParaComponenteNomeDto(disciplinasEol);
        }

        private IEnumerable<DisciplinaNomeDto> MapearParaComponenteNomeDto(IEnumerable<DisciplinaDto> componentesSgp, IEnumerable<ComponenteCurricularEol> componentesCurricularesEol)
        {
            foreach (var componenteSgp in componentesSgp)
            {
                var componenteEol = componentesCurricularesEol.FirstOrDefault(c => componenteSgp.Id == (c.TerritorioSaber ? c.CodigoComponenteTerritorioSaber : c.Codigo));

                if (componenteEol != null)
                {
                    if (componenteEol.TerritorioSaber)
                        yield return new DisciplinaNomeDto()
                        {
                            Codigo = componenteEol.CodigoComponenteTerritorioSaber.ToString(),
                            Nome = componenteEol.Descricao
                        };
                    else
                        yield return new DisciplinaNomeDto()
                        {
                            Codigo = componenteSgp.Id.ToString(),
                            Nome = componenteSgp.NomeComponenteInfantil
                        };
                }
            }
        }

        private IEnumerable<DisciplinaNomeDto> MapearParaComponenteNomeDto(IEnumerable<DisciplinaDto> disciplinasEol)
        {
            foreach (var disciplinaEol in disciplinasEol)
                yield return new DisciplinaNomeDto()
                {
                    Codigo = disciplinaEol.Id.ToString(),
                    Nome = disciplinaEol.Nome
                };
        }
    }
}
