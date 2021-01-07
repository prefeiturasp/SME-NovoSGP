using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
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
            if (ocorrencia is null)
                throw new NegocioException($"Não foi possível localizar a ocorrência {request.Id}.");

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(ocorrencia.TurmaId));
            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));

            return MapearParaDto(ocorrencia, alunos);
        }

        private OcorrenciaDto MapearParaDto(Ocorrencia ocorrencia, IEnumerable<AlunoPorTurmaResposta> alunos) 
            => new OcorrenciaDto()
            {
                Auditoria = (AuditoriaDto)ocorrencia,
                DataOcorrencia = ocorrencia.DataOcorrencia,
                Descricao = ocorrencia.Descricao,
                HoraOcorrencia = ocorrencia.HoraOcorrencia?.ToString(@"hh\:mm") ?? string.Empty,
                OcorrenciaTipoId = ocorrencia.OcorrenciaTipoId.ToString(),
                TurmaId = ocorrencia.TurmaId,
                Titulo = ocorrencia.Titulo,
                Alunos = ocorrencia.Alunos.Select(ao => new OcorrenciaAlunoDto()
                {
                    Id = ao.Id,
                    CodigoAluno = ao.CodigoAluno,
                    Nome = alunos.FirstOrDefault(a => a.CodigoAluno == ao.CodigoAluno.ToString())?.NomeAluno
                })
            };
    }
}

