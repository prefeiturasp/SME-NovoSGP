using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQueryHandler : IRequestHandler<ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;
        private readonly IMediator mediator;

        public ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual, IMediator mediator)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery request, CancellationToken cancellationToken)
        {
            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo));
            if (!alunos?.Any() ?? true)
                throw new NegocioException($"Não existem alunos para a turma {request.TurmaCodigo}.");

            var ultimosRegistrosIndividuaisAlunosTurma = await repositorioRegistroIndividual.ObterUltimosRegistrosPorAlunoTurma(request.TurmaId);
            if (!ultimosRegistrosIndividuaisAlunosTurma?.Any() ?? true) return alunos;

            var alunosComAusenciaRegistroIndividualPorDias = new List<AlunoPorTurmaResposta>();
            foreach (var aluno in alunos.Where(a => a.EstaAtivo(DateTime.Now)))
            {
                var ultimoRegistroIndividualAlunoTurma = ultimosRegistrosIndividuaisAlunosTurma.FirstOrDefault(x => x.CodigoAluno == aluno.CodigoAluno);
                if (ultimoRegistroIndividualAlunoTurma is null)
                {
                    alunosComAusenciaRegistroIndividualPorDias.Add(aluno);
                    continue;
                }

                var direrencaEntreDataAtualEUltimoRegistro = DateTime.Today.Subtract(ultimoRegistroIndividualAlunoTurma.DataRegistro.Date);
                if (direrencaEntreDataAtualEUltimoRegistro.Days >= request.DiasAusencia)
                    alunosComAusenciaRegistroIndividualPorDias.Add(aluno);
            }

            return alunosComAusenciaRegistroIndividualPorDias;
        }
    }
}