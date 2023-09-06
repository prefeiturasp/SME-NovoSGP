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
        
        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaNomeDto>> Handle(ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery request, CancellationToken cancellationToken)
        {

            if (request.UsuarioLogado.EhProfessorCj())
                return await ObterComponentesAtribuicaoCj(request.TurmaCodigo, request.UsuarioLogado.CodigoRf);
            else
                return await ObterComponentesCurricularesUsuario(request.TurmaCodigo, request.UsuarioLogado.CodigoRf ?? request.UsuarioLogado.Login, request.UsuarioLogado.PerfilAtual);
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesCurricularesUsuario(string turmaCodigo, string codigoRf, Guid perfilAtual)
        {
            var obterTurma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            bool realizarAgrupamentoComponente = obterTurma.AnoLetivo != DateTimeExtension.HorarioBrasilia().Year;
            var componentesCurricularesEol = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, codigoRf,
                                                               perfilAtual,
                                                               realizarAgrupamentoComponente));

            if (componentesCurricularesEol == null || !componentesCurricularesEol.Any())
                return null;

            return (await ObterComponentesCurricularesRepositorioSgp(componentesCurricularesEol, obterTurma.ModalidadeCodigo == Modalidade.EducacaoInfantil, codigoTurma: turmaCodigo))?
                .OrderBy(c => c.Nome)?.ToList();

        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesCurricularesRepositorioSgp(IEnumerable<ComponenteCurricularEol> componentesCurricularesEol, bool ehEducacaoInfatil, string codigoTurma)
        {
            var componentesSgp = await mediator
                .Send(new ObterComponentesCurricularesPorIdsQuery(componentesCurricularesEol
                    .Select(a => a.TerritorioSaber ? a.CodigoComponenteTerritorioSaber : 
                                (a.Regencia && !ehEducacaoInfatil && a.CodigoComponenteCurricularPai.HasValue && a.CodigoComponenteCurricularPai.Value > 0 ? a.CodigoComponenteCurricularPai.Value : a.Codigo)).ToArray()));

            return MapearParaComponenteNomeDto(componentesSgp, componentesCurricularesEol, ehEducacaoInfatil);
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesAtribuicaoCj(string turmaCodigo, string login)
        {
            var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, turmaCodigo, string.Empty, 0, login, string.Empty, true));

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));

            return MapearParaComponenteNomeDto(disciplinasEol);
        }

        private IEnumerable<DisciplinaNomeDto> MapearParaComponenteNomeDto(IEnumerable<DisciplinaDto> componentesSgp, IEnumerable<ComponenteCurricularEol> componentesCurricularesEol, bool ehEducacaoInfatil)
        {
            foreach (var componenteSgp in componentesSgp)
            {
                var componenteEol = componentesCurricularesEol
                    .FirstOrDefault(c => componenteSgp.Id == (c.TerritorioSaber ? c.CodigoComponenteTerritorioSaber : (c.Regencia && !ehEducacaoInfatil && c.CodigoComponenteCurricularPai.HasValue && c.CodigoComponenteCurricularPai.Value > 0 ? c.CodigoComponenteCurricularPai.Value : c.Codigo)));

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
                            Nome = componenteEol.ExibirComponenteEOL && ehEducacaoInfatil ? componenteSgp.NomeComponenteInfantil : componenteSgp.Nome
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
