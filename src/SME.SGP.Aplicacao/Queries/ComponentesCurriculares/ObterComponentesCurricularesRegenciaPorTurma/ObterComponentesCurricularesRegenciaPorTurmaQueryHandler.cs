using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesRegenciaPorTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;

        private static readonly long[] IDS_COMPONENTES_REGENCIA = { 2, 7, 8, 89, 138 };

        public ObterComponentesCurricularesRegenciaPorTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesRegenciaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = request.Turma;
            var regencias = await mediator.Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(turma.CodigoTurma));

            if (usuario.EhProfessorCj())
                return await mediator.Send(new ObterComponentesCJQuery(turma.ModalidadeCodigo, turma.CodigoTurma, turma.Ue.CodigoUe, request.ComponenteCurricularId, usuario.CodigoRf, false));
            else
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery(turma.CodigoTurma, usuario.Login, usuario.PerfilAtual));

                return componentesCurriculares.Where(x => x.Regencia && regencias.Any(c => c.CodigoComponenteCurricular == x.Codigo)).OrderBy(c => c.Descricao);
            }
        }
    }
}
