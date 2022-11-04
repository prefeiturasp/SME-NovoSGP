using MediatR;
using SME.SGP.Dominio;
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
        private IEnumerable<TurmasDoAlunoDto> Alunos;
        private IEnumerable<UsuarioEolRetornoDto> Servidores;

        public ListarOcorrenciasQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.Alunos = new List<TurmasDoAlunoDto>();
            this.Servidores = new List<UsuarioEolRetornoDto>();
        }

        public async Task<PaginacaoResultadoDto<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            string[] codigosServidoresLike = null;

            await CarregarServidores(request.Filtro.UeId);

            if (!string.IsNullOrEmpty(request.Filtro.ServidorNome))
                codigosServidoresLike = ObterCodigosDosServidores(request.Filtro.ServidorNome);

            var lstOcorrencias = await repositorioOcorrencia.ListarPaginado(request.Filtro, codigosServidoresLike, Paginacao);

            await CarregarAlunos(lstOcorrencias);

            if (!string.IsNullOrEmpty(request.Filtro.AlunoNome))
                lstOcorrencias = ObterOcorrenciaPorFiltroDeAlunos(lstOcorrencias, request.Filtro.AlunoNome);

            return MapearParaDto(lstOcorrencias);
        }

        private async Task CarregarServidores(long ueId)
        {
            var dtoUe = await mediator.Send(new ObterCodigoUEDREPorIdQuery(ueId));
            this.Servidores = await mediator.Send(new ObterFuncionariosPorUeQuery(dtoUe.UeCodigo));
        }

        private string[] ObterCodigosDosServidores(string nomeServidor)
        {
            return this.Servidores.Where(servidor => servidor.NomeServidor.IndexOf(nomeServidor, StringComparison.OrdinalIgnoreCase) != -1)?.Select(servidor => servidor.CodigoRf)?.ToArray();
        }

        private async Task CarregarAlunos(PaginacaoResultadoDto<Ocorrencia> ocorrencias)
        {
            var codigosAlunos = ocorrencias.Items.SelectMany(ocorrencia => ocorrencia.Alunos).Select(aluno => aluno.CodigoAluno).ToArray();

            if (codigosAlunos.Any())
            {
                this.Alunos = await mediator.Send(new ObterAlunosEolPorCodigosQuery(codigosAlunos));
            }
        }

        private PaginacaoResultadoDto<Ocorrencia> ObterOcorrenciaPorFiltroDeAlunos(PaginacaoResultadoDto<Ocorrencia> ocorrencias, string alunoNome)
        {
            if (this.Alunos.Any())
            {
                var codigosAlunosLike = this.Alunos.Where(a => a.NomeAluno.IndexOf(alunoNome, StringComparison.OrdinalIgnoreCase) != -1)?.Select(a => Convert.ToInt64(a.CodigoAluno))?.ToArray();

                var ocorrenciasAluno = ocorrencias.Items.Where(ocorrencia => ocorrencia.Alunos != null && ocorrencia.Alunos.ToList().Exists(aluno => codigosAlunosLike.Contains(aluno.CodigoAluno)));

                return new PaginacaoResultadoDto<Ocorrencia>()
                {
                    Items = ocorrenciasAluno.ToList(),
                    TotalRegistros = ocorrenciasAluno.Count(),
                    TotalPaginas = (int)Math.Ceiling((double)ocorrenciasAluno.Count() / Paginacao.QuantidadeRegistros)
                };
            }

            return ocorrencias;
        }

        private PaginacaoResultadoDto<OcorrenciaListagemDto> MapearParaDto(PaginacaoResultadoDto<Ocorrencia> ocorrencias)
        {
            return new PaginacaoResultadoDto<OcorrenciaListagemDto>()
            {
                Items = ocorrencias.Items.Select(ocorrencia => 
                {
                    var alunoOcorrencia = ocorrencia.Alunos?.Count > 1
                    ? $"{ocorrencia.Alunos?.Count} crianças/estudantes"
                    : DefinirDescricaoOcorrenciaAluno(ocorrencia);

                    var alunoServidor = ocorrencia.Servidores?.Count > 1
                      ? $"{ocorrencia.Servidores?.Count} servidores"
                      : DefinirDescricaoOcorrenciaServidor(ocorrencia);

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

        private string DefinirDescricaoOcorrenciaAluno(Ocorrencia ocorrencia)
        {
            var ocorrenciaAluno = ocorrencia.Alunos?.FirstOrDefault();
            if (ocorrenciaAluno is null) return default;

            var alunoTurma = this.Alunos.FirstOrDefault(a => a.CodigoAluno == ocorrenciaAluno.CodigoAluno);
            if (alunoTurma is null) return default;

            var nomeDoAluno = string.IsNullOrWhiteSpace(alunoTurma.NomeSocialAluno) ? alunoTurma.NomeAluno : alunoTurma.NomeSocialAluno;
            return $"{nomeDoAluno} ({alunoTurma.CodigoAluno})";
        }

        private string DefinirDescricaoOcorrenciaServidor(Ocorrencia ocorrencia)
        {
            var ocorrenciaServidor = ocorrencia.Servidores?.FirstOrDefault();
            if (ocorrenciaServidor is null) return default;

            var servidor = this.Servidores.FirstOrDefault(servidor => servidor.CodigoRf == ocorrenciaServidor.CodigoServidor);
            if (servidor is null) return default;

            return $"{servidor.NomeServidor} ({servidor.CodigoRf})";
        }
    }
}
