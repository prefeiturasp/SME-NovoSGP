using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQueryHandler : ConsultasBase, IRequestHandler<ListarOcorrenciasQuery, PaginacaoResultadoDto<OcorrenciaListagemDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IMediator mediator;
        private IEnumerable<TurmasDoAlunoDto> Alunos;
        private IEnumerable<UsuarioEolRetornoDto> Servidores;
        private const long TODAS_UES = -99;

        public ListarOcorrenciasQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.Alunos = new List<TurmasDoAlunoDto>();
            this.Servidores = new List<UsuarioEolRetornoDto>();
        }

        public async Task<PaginacaoResultadoDto<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                long[] idUes = Array.Empty<long>();

                if(request.Filtro.UeId == TODAS_UES)
                    idUes = await ObterIdUesAbrangencia(request);

                var lstOcorrencias = await repositorioOcorrencia.ListarPaginado(request.Filtro, Paginacao, idUes);

                await CarregarServidores(lstOcorrencias);

                if (!string.IsNullOrEmpty(request.Filtro.ServidorNome))
                    lstOcorrencias = ObterOcorrenciaPorFiltroDeServidores(lstOcorrencias, request.Filtro.ServidorNome);

                await CarregarAlunos(lstOcorrencias);

                if (!string.IsNullOrEmpty(request.Filtro.AlunoNome))
                    lstOcorrencias = ObterOcorrenciaPorFiltroDeAlunos(lstOcorrencias, request.Filtro.AlunoNome);

                return await MapearParaDto(lstOcorrencias);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao executar ListarOcorrenciasQueryHandler", LogNivel.Critico, LogContexto.Geral, ex.Message));
                throw;
            }
        }

        private async Task<long[]> ObterIdUesAbrangencia(ListarOcorrenciasQuery request)
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());
            var dre = await mediator.Send(new ObterDREPorIdQuery(request.Filtro.DreId));
            var ues = await mediator.Send(new ObterUEsPorDREQuery(dre.CodigoDre, login, perfil, 0, 0, request.Filtro.ConsideraHistorico, request.Filtro.AnoLetivo));

            return ues.Select(x => x.Id).ToArray();
        }

        private async Task CarregarServidores(PaginacaoResultadoDto<Ocorrencia> ocorrencias)
        {
            var codigosServidores = ocorrencias.Items.SelectMany(ocorrencia => ocorrencia.Servidores).Select(aluno => aluno.CodigoServidor).ToArray();
            var servidores = new List<UsuarioEolRetornoDto>();
            if (codigosServidores.Any())
            {
                var idUes = ocorrencias.Items.Select(ocorrencia => ocorrencia.UeId).Distinct().ToArray();
                var dtoUes = await mediator.Send(new ObterUesPorIdsQuery(idUes));
                foreach (var dtoue in dtoUes)
                {
                    var funcionarios = await mediator.Send(new ObterFuncionariosPorUeQuery(dtoue.CodigoUe, codigosServidores));
                    if (funcionarios != null)
                        servidores.AddRange(funcionarios);
                }

                this.Servidores = servidores;
            }
        }

        private PaginacaoResultadoDto<Ocorrencia> ObterOcorrenciaPorFiltroDeServidores(PaginacaoResultadoDto<Ocorrencia> ocorrencias, string servidorNome)
        {
            if (this.Servidores.Any())
            {
                var codigosServidoresLike = this.Servidores.Where(a => a.NomeServidor.IndexOf(servidorNome, StringComparison.OrdinalIgnoreCase) != -1)?.Select(a => a.CodigoRf)?.ToArray();

                var ocorrenciasServidores = ocorrencias.Items.Where(ocorrencia => ocorrencia.Servidores.ToList().Exists(servidor => codigosServidoresLike.Contains(servidor.CodigoServidor)));

                return new PaginacaoResultadoDto<Ocorrencia>()
                {
                    Items = ocorrenciasServidores.ToList(),
                    TotalRegistros = ocorrenciasServidores.Count(),
                    TotalPaginas = (int) Math.Ceiling((double) ocorrenciasServidores.Count() / Paginacao.QuantidadeRegistros)
                };
            }

            return ocorrencias;
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

                var ocorrenciasAluno = ocorrencias.Items.Where(ocorrencia => ocorrencia.Alunos.ToList().Exists(aluno => codigosAlunosLike.Contains(aluno.CodigoAluno)));

                return new PaginacaoResultadoDto<Ocorrencia>()
                {
                    Items = ocorrenciasAluno.ToList(),
                    TotalRegistros = ocorrenciasAluno.Count(),
                    TotalPaginas = (int) Math.Ceiling((double) ocorrenciasAluno.Count() / Paginacao.QuantidadeRegistros)
                };
            }

            return ocorrencias;
        }

        private async Task<PaginacaoResultadoDto<OcorrenciaListagemDto>> MapearParaDto(PaginacaoResultadoDto<Ocorrencia> ocorrencias)
        {
            var uesOcorrencias = new List<UesOcorreciaDto>();
            if (ocorrencias.Items.Any())
                uesOcorrencias = await ObterNomeUe(ocorrencias.Items.Select(x => x.UeId).Distinct().ToArray());
            var listaRetorno = new PaginacaoResultadoDto<OcorrenciaListagemDto>()
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
                        ServidorOcorrencia = alunoServidor,
                        DataOcorrencia = ocorrencia.DataOcorrencia.ToString("dd/MM/yyyy"),
                        Id = ocorrencia.Id,
                        UeOcorrencia = uesOcorrencias?.FirstOrDefault(x => x.IdUe == ocorrencia.UeId)?.NomeUe,
                        Titulo = ocorrencia.Titulo,
                        Turma = ocorrencia.Turma?.NomeFiltro,
                    };
                }),
                TotalRegistros = ocorrencias.TotalRegistros,
                TotalPaginas = ocorrencias.TotalPaginas
            };

            listaRetorno.Items = listaRetorno.Items.OrderBy(x => x.UeOcorrencia);
            return listaRetorno;
        }

        private async Task<List<UesOcorreciaDto>> ObterNomeUe(long[] ueIdis)
        {
            var ues = await mediator.Send(new ObterUesPorIdsQuery(ueIdis));
            if (!ues.Any())
                throw new NegocioException("Não foi possível encrontra nenhuma UE!");

            return ues.Select(ue => new UesOcorreciaDto(ue.Id, $"{ue.TipoEscola.ShortName()} {ue.Nome}")).ToList();
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