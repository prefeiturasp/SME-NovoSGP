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
            var alunosAusentesFiltrados = new List<AlunosAusentesDto>();
            foreach (var alunoAusente in alunosAusentes)
            {
                var aluno = alunosTurma.FirstOrDefault(aluno => aluno.CodigoAluno == alunoAusente.CodigoEol);
                await TratarAluno(request.Filtro.CodigoTurma, aluno, alunosAusentesFiltrados);
            }

            return alunosAusentesFiltrados.OrderBy(aluno => aluno.Nome);
        }

        private async Task TratarAluno(string codigoTurma, AlunoPorTurmaResposta aluno, List<AlunosAusentesDto> alunosAusentesFiltrados)
        {
            if (aluno.NaoEhNulo())
            {
                var frequenciaGlobal = await mediator.Send(new ObterConsultaFrequenciaGeralAlunoQuery(aluno.CodigoAluno, codigoTurma));
                var alunoAusente = new AlunosAusentesDto()
                {
                    CodigoEol = aluno.CodigoAluno,
                    Nome = aluno.NomeAluno,
                    NumeroChamada = aluno.NumeroAlunoChamada.GetValueOrDefault(),
                    FrequenciaGlobal = frequenciaGlobal,
                };
                alunosAusentesFiltrados.Add(alunoAusente);
            }
        }
    }
}
