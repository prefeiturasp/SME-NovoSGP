using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase : AbstractUseCase, IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase
    {
        public ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase(IMediator mediator) : base (mediator)
        {
        }

        public async Task<IEnumerable<DisciplinaNomeDto>> Executar(IEnumerable<string> turmasCodigos)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var componetesCurriculares = new List<DisciplinaNomeDto>();

            foreach (var turmaCodigo in turmasCodigos)
            {
                var componentesNaTurma = await mediator.Send(new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo));
                if ((componentesNaTurma.NaoEhNulo()) && componentesNaTurma.Any())
                    componetesCurriculares.AddRange(componentesNaTurma);
            }

            return componetesCurriculares.DistinctBy(x => x.Codigo);
        }
    }
}
