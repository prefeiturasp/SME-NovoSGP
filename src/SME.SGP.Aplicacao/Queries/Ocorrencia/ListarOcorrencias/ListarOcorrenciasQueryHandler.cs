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
    public class ListarOcorrenciasQueryHandler : IRequestHandler<ListarOcorrenciasQuery, IEnumerable<OcorrenciaListagemDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IMediator mediator;

        public ListarOcorrenciasQueryHandler(IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));

            string[] codigosAlunosLike = null;

            if (!string.IsNullOrEmpty(request.AlunoNome))
                codigosAlunosLike = alunos.Where(a => a.NomeAluno.IndexOf(request.AlunoNome, StringComparison.OrdinalIgnoreCase) != -1)?.Select(a => a.CodigoAluno)?.ToArray();

            var lstOcorrencias = await repositorioOcorrencia.Listar(request.TurmaId, request.Titulo, request.AlunoNome, request.DataOcorrenciaInicio, request.DataOcorrenciaFim, codigosAlunosLike);

            return MapearParaDto(lstOcorrencias, alunos);
        }

        private IEnumerable<OcorrenciaListagemDto> MapearParaDto(IEnumerable<Ocorrencia> ocorrencias, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            foreach (var ocorrencia in ocorrencias)
            {
                var alunoOcorrencia = ocorrencia.Alunos.Count > 1
                    ? $"{ocorrencia.Alunos.Count} crianças"
                    : GetDescricaoOcorrenciaAluno(alunos, ocorrencia);

                yield return new OcorrenciaListagemDto()
                {
                    AlunoOcorrencia = alunoOcorrencia,
                    DataOcorrencia = ocorrencia.DataOcorrencia.ToString("dd/MM/yyyy"),
                    Id = ocorrencia.Id,
                    Titulo = ocorrencia.Titulo,
                    TurmaId = ocorrencia.TurmaId
                };
            }
        }

        private string GetDescricaoOcorrenciaAluno(IEnumerable<AlunoPorTurmaResposta> alunos, Ocorrencia ocorrencia)
        {
            var ocorrenciaAluno = ocorrencia.Alunos.FirstOrDefault();
            if (ocorrenciaAluno is null) return default;

            var alunoTurma = alunos.FirstOrDefault(a => a.CodigoAluno == ocorrenciaAluno.CodigoAluno.ToString());
            if (alunoTurma is null) return default;

            return alunoTurma.NomeSocialAluno;
        }
    }
}
