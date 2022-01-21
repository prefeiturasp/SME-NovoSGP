using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase : AbstractUseCase, IObterComponentesCurricularesPorProfessorETurmasCodigosUseCase
    {
        public ObterComponentesCurricularesPorProfessorETurmasCodigosUseCase(IMediator mediator) : base (mediator)
        {
        }

        public async Task<IEnumerable<DisciplinaNomeDto>> Executar(IEnumerable<string> turmasCodigos,bool realizarAgrupamentoComponente)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componetesCurriculares = new List<DisciplinaNomeDto>();

            foreach (var turmaCodigo in turmasCodigos)
            {
                var componentesNaTurma = await mediator.Send(new ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery(usuarioLogado, turmaCodigo, realizarAgrupamentoComponente));
                if ((componentesNaTurma != null) && componentesNaTurma.Any())
                    componetesCurriculares.AddRange(componentesNaTurma);
            }

            return componetesCurriculares.DistinctBy(x => x.Codigo);
        }
    }
}
