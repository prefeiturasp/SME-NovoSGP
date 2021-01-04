using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciaPorIdQueryHandler : IRequestHandler<ObterOcorrenciaPorIdQuery, OcorrenciaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IMediator mediator;

        public ObterOcorrenciaPorIdQueryHandler(IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator,
                                                IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno,
                                                IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaAluno));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaTipo));
        }

        public async Task<OcorrenciaDto> Handle(ObterOcorrenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            var ocorrencia = await repositorioOcorrencia.ObterPorIdAsync(request.Id);
            var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(ocorrencia.OcorrenciaTipoId);
            var alunosOcorrencia = await repositorioOcorrenciaAluno.ObterAlunosPorOcorrencia(request.Id);
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(ocorrencia.TurmaId));
            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));

            return MapearParaDto(ocorrencia, ocorrenciaTipo, alunosOcorrencia, turma, alunos);
        }

        private OcorrenciaDto MapearParaDto(Ocorrencia ocorrencia, OcorrenciaTipo ocorrenciaTipo,
                                            IEnumerable<string> alunosOcorrencia, Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            return new OcorrenciaDto()
            {
                Auditoria = (AuditoriaDto)ocorrencia,
                DataOcorrencia = ocorrencia.DataOcorrencia.ToString("dd/MM/yyyy"),
                Descricao = ocorrencia.Descricao,
                HoraOcorrencia = ocorrencia.HoraOcorrencia.HasValue ? ocorrencia.HoraOcorrencia.Value.ToString(@"hh\:mm\:ss") : "00:00:00",
                OcorrenciaTipoId = ocorrencia.OcorrenciaTipoId,
                TurmaId = ocorrencia.TurmaId,
                Titulo = ocorrencia.Titulo,
                Alunos = alunosOcorrencia.Select(ao => new OcorrenciaAlunoDto()
                {
                    CodigoAluno = Convert.ToInt64(ao),
                    Nome = alunos.FirstOrDefault(a => a.CodigoAluno == ao)?.NomeSocialAluno
                })
            };
        }
    }
}

