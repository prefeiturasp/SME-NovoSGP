using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAlunosAusentesQueryHandler : IRequestHandler<ObterTurmasAlunosAusentesQuery, IEnumerable<AlunosAusentesDto>>
    {
        private readonly IRepositorioConsultaCriancasEstudantesAusentes repositorio;
        private readonly IMediator mediator;

        public ObterTurmasAlunosAusentesQueryHandler(IRepositorioConsultaCriancasEstudantesAusentes repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunosAusentesDto>> Handle(ObterTurmasAlunosAusentesQuery request, CancellationToken cancellationToken)
        {
            var alunosAusentes = await repositorio.ObterTurmasAlunosAusentes(request.Filtro);
            IEnumerable<AlunoPorTurmaResposta> alunosTurma = null;

            if (alunosAusentes.Any())
                alunosTurma = await mediator.Send(new ObterAlunosEolPorTurmaQuery(request.Filtro.CodigoTurma));

            foreach (var alunoAusente in alunosAusentes)
            {
                var aluno = alunosTurma.FirstOrDefault(aluno => aluno.CodigoAluno == alunoAusente.CodigoEol);
                await TratarAluno(request.Filtro.CodigoTurma, alunoAusente, aluno);
            }

            return alunosAusentes.OrderBy(aluno => aluno.Nome);
        }

        private async Task TratarAluno(string codigoTurma, AlunosAusentesDto alunoAusente, AlunoPorTurmaResposta aluno)
        {
            if (aluno.NaoEhNulo())
            {
                alunoAusente.Nome = aluno.NomeAluno;
                alunoAusente.NumeroChamada = aluno.NumeroAlunoChamada.GetValueOrDefault();
                alunoAusente.FrequenciaGlobal = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(alunoAusente.CodigoEol, codigoTurma));
            }
        }
    }
}
