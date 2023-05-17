using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandler : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConciliacaoFrequenciaTurmaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(request.TurmaCodigo, (request.DataInicio, request.DataFim)));

            foreach(var componenteCurricularId in await ObterComponentesCurriculares(request.TurmaCodigo, request.ComponenteCurricularId))
            {
                var alunosCodigo = alunos.Select(a => a.CodigoAluno);

                var comando = new CalcularFrequenciaPorTurmaCommand(alunosCodigo, request.DataFim, request.TurmaCodigo, componenteCurricularId);
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));
            }

            return true;
        }

        private async Task<IEnumerable<string>> ObterComponentesCurriculares(string turmaCodigo, string componenteCurricularId)
        {
            if (!string.IsNullOrEmpty(componenteCurricularId))
                return new List<string>() { componenteCurricularId };

            // Listar componentes da turma
            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasQuery(new string[] { turmaCodigo }));
            if (componentesCurriculares?.Any() != true)
                throw new Exception("Não foi possível obter os componentes curriculares da turma");

            return componentesCurriculares.Select(a => a.Codigo.ToString());
        }
    }
}
