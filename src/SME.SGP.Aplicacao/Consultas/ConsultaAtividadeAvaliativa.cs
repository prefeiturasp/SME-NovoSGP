using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaAtividadeAvaliativa : ConsultasBase, IConsultaAtividadeAvaliativa
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ConsultaAtividadeAvaliativa(
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioTipoCalendario repositorioTipoCalendario,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario;
        }

        public async Task<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>> ListarPaginado(FiltroAtividadeAvaliativaDto filtro)
        {
            return MapearParaDtoComPaginacao(await repositorioAtividadeAvaliativa
                .Listar(filtro.DataAvaliacao.HasValue ? filtro.DataAvaliacao.Value.Date : filtro.DataAvaliacao,
                        filtro.DreId,
                        filtro.UeID,
                        filtro.Nome,
                        filtro.TipoAvaliacaoId,
                        filtro.TurmaId,
                        Paginacao
                        ));
        }

        public IEnumerable<AtividadeAvaliativa> ObterAvaliacoesDoBimestre(string turmaId, string disciplinaId, int anoLetivo, int bimestre, ModalidadeTipoCalendario modalidade)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);

            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendario escolar, para a modalidade informada");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado periodo Escolar para a modalidade informada");

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoEscolar == null)
                throw new NegocioException("Não foi encontrado periodo escolar para o bimestre solicitado");

            var avaliacoes = repositorioAtividadeAvaliativa.ListarPorTurmaDisciplinaPeriodo(turmaId, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);

            if (avaliacoes == null || !avaliacoes.Any())
                throw new NegocioException("Não foi encontrada nenhuma avaliação para o bimestre informado");

            return avaliacoes;
        }

        public async Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id)
        {
            IEnumerable<AtividadeAvaliativaRegencia> atividadeRegencias = null;
            var atividade = await repositorioAtividadeAvaliativa.ObterPorIdAsync(id);
            if (atividade is null)
                throw new NegocioException("Atividade avaliativa não encontrada");
            if (atividade.EhRegencia)
                atividadeRegencias = await repositorioAtividadeAvaliativaRegencia.Listar(id);
            return MapearParaDto(atividade, atividadeRegencias);
        }

        private IEnumerable<AtividadeAvaliativaCompletaDto> MapearAtividadeAvaliativaParaDto(IEnumerable<AtividadeAvaliativa> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }

        private AtividadeAvaliativaCompletaDto MapearParaDto(AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AtividadeAvaliativaRegencia> regencias = null)
        {
            return atividadeAvaliativa == null ? null : new AtividadeAvaliativaCompletaDto
            {
                Id = atividadeAvaliativa.Id,
                CategoriaId = (CategoriaAtividadeAvaliativa)atividadeAvaliativa.Categoria,
                DataAvaliacao = atividadeAvaliativa.DataAvaliacao,
                Descricao = atividadeAvaliativa.DescricaoAvaliacao,
                DisciplinaId = atividadeAvaliativa.DisciplinaId,
                DreId = atividadeAvaliativa.DreId,
                UeId = atividadeAvaliativa.UeId,
                Nome = atividadeAvaliativa.NomeAvaliacao,
                TipoAvaliacaoId = atividadeAvaliativa.TipoAvaliacaoId,
                TurmaId = atividadeAvaliativa.TurmaId,
                AlteradoEm = atividadeAvaliativa.AlteradoEm,
                AlteradoPor = atividadeAvaliativa.AlteradoPor,
                AlteradoRF = atividadeAvaliativa.AlteradoRF,
                CriadoEm = atividadeAvaliativa.CriadoEm,
                CriadoPor = atividadeAvaliativa.CriadoPor,
                CriadoRF = atividadeAvaliativa.CriadoRF,
                Categoria = atividadeAvaliativa.TipoAvaliacao?.Descricao,
                EhRegencia = atividadeAvaliativa.EhRegencia,
                AtividadesRegencia = regencias?.Select(x => new AtividadeAvaliativaRegenciaDto
                {
                    AtividadeAvaliativaId = x.AtividadeAvaliativaId,
                    DisciplinaContidaRegenciaId = x.DisciplinaContidaRegenciaId,
                    Id = x.Id
                }).ToList()
            };
        }

        private PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<AtividadeAvaliativa> atividadeAvaliativaPaginado)
        {
            if (atividadeAvaliativaPaginado == null)
            {
                atividadeAvaliativaPaginado = new PaginacaoResultadoDto<AtividadeAvaliativa>();
            }
            return new PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>
            {
                Items = MapearAtividadeAvaliativaParaDto(atividadeAvaliativaPaginado.Items),
                TotalPaginas = atividadeAvaliativaPaginado.TotalPaginas,
                TotalRegistros = atividadeAvaliativaPaginado.TotalRegistros
            };
        }
    }
}