using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQueryHandler : ConsultasBase, IRequestHandler<ListarOcorrenciasQuery, PaginacaoResultadoDto<OcorrenciaListagemDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IMediator mediator;

        public ListarOcorrenciasQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));

            long[] codigosAlunosLike = null;

            if (!string.IsNullOrEmpty(request.AlunoNome))
                codigosAlunosLike = alunos.Where(a => a.NomeAluno.IndexOf(request.AlunoNome, StringComparison.OrdinalIgnoreCase) != -1)?.Select(a => Convert.ToInt64(a.CodigoAluno))?.ToArray();

            var lstOcorrencias = await repositorioOcorrencia.ListarPaginado(request.TurmaId, request.Titulo, request.AlunoNome, request.DataOcorrenciaInicio, request.DataOcorrenciaFim, codigosAlunosLike, Paginacao);

            return MapearParaDto(lstOcorrencias, alunos);
        }

        private PaginacaoResultadoDto<OcorrenciaListagemDto> MapearParaDto(PaginacaoResultadoDto<Ocorrencia> ocorrencias, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            return new PaginacaoResultadoDto<OcorrenciaListagemDto>()
            {
                Items = ocorrencias.Items.Select(ocorrencia => 
                {
                    var alunoOcorrencia = ocorrencia.Alunos.Count > 1
                    ? $"{ocorrencia.Alunos.Count} crianças"
                    : DefinirDescricaoOcorrenciaAluno(alunos, ocorrencia);

                    return new OcorrenciaListagemDto()
                    {
                        AlunoOcorrencia = alunoOcorrencia,
                        DataOcorrencia = ocorrencia.DataOcorrencia.ToString("dd/MM/yyyy"),
                        Id = ocorrencia.Id,
                        Titulo = ocorrencia.Titulo,
                        TurmaId = ocorrencia.TurmaId
                    };
                }),
                TotalRegistros = ocorrencias.TotalRegistros,
                TotalPaginas = ocorrencias.TotalPaginas
            };
        }

        private string DefinirDescricaoOcorrenciaAluno(IEnumerable<AlunoPorTurmaResposta> alunos, Ocorrencia ocorrencia)
        {
            var ocorrenciaAluno = ocorrencia.Alunos.FirstOrDefault();
            if (ocorrenciaAluno is null) return default;

            var alunoTurma = alunos.FirstOrDefault(a => a.CodigoAluno == ocorrenciaAluno.CodigoAluno.ToString());
            if (alunoTurma is null) return default;

            var nomeDoAluno = string.IsNullOrWhiteSpace(alunoTurma.NomeSocialAluno) ? alunoTurma.NomeAluno : alunoTurma.NomeSocialAluno;
            return $"{nomeDoAluno} ({alunoTurma.CodigoAluno})";
        }
    }
}
