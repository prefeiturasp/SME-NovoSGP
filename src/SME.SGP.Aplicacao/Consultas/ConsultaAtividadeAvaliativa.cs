using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaAtividadeAvaliativa : ConsultasBase, IConsultaAtividadeAvaliativa
    {
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEol servicoEOL;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultaAtividadeAvaliativa(
            IConsultasProfessor consultasProfessor,
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioTurma repositorioTurma,
            IRepositorioAula repositorioAula,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IServicoUsuario servicoUsuario,
            IServicoEol servicoEOL,
            IContextoAplicacao contextoAplicacao,
            IConsultasTurma consultasTurma,
            IConsultasPeriodoEscolar consultasPeriodoEscolar,
            IConsultasPeriodoFechamento consultasPeriodoFechamento) : base(contextoAplicacao)
        {
            this.consultasProfessor = consultasProfessor ?? throw new System.ArgumentNullException(nameof(consultasProfessor));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
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

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAvaliacoesNoBimestre(string turmaCodigo, string disciplinaId, DateTime periodoInicio, DateTime periodoFim)
            => await repositorioAtividadeAvaliativa.ListarPorTurmaDisciplinaPeriodo(turmaCodigo, disciplinaId, periodoInicio, periodoFim);

        public async Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id)
        {
            var atividade = await repositorioAtividadeAvaliativa.ObterPorIdAsync(id);

            if (atividade is null)
                throw new NegocioException("Atividade avaliativa não encontrada");

            IEnumerable<AtividadeAvaliativaRegencia> atividadeRegencias = null;

            IEnumerable<AtividadeAvaliativaDisciplina> atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(id);

            if (atividade.EhRegencia)
                atividadeRegencias = await repositorioAtividadeAvaliativaRegencia.Listar(id);

            var dentroPeriodo = await AtividadeAvaliativaDentroPeriodo(atividade);

            return MapearParaDto(atividade, atividadeRegencias, atividadeDisciplinas, dentroPeriodo);
        }

        public async Task<bool> AtividadeAvaliativaDentroPeriodo(AtividadeAvaliativa atividadeAvaliativa)
        {
            return await AtividadeAvaliativaDentroPeriodo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao);
        }

        public async Task<bool> AtividadeAvaliativaDentroPeriodo(string turmaId, DateTime dataAula)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma == null)
                throw new NegocioException($"Não foi possivel obter a turma da aula");

            var bimestreAtual = await consultasPeriodoEscolar.ObterBimestre(DateTime.Now, turma.ModalidadeCodigo, turma.Semestre);
            var bimestreAvaliacao = await consultasPeriodoEscolar.ObterBimestre(dataAula, turma.ModalidadeCodigo, turma.Semestre);

            if (bimestreAtual == 0 || bimestreAvaliacao == 0)
                return false;

            if (bimestreAvaliacao >= bimestreAtual)
                return true;

            return await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma, DateTime.Now, bimestreAtual);
        }

        public async Task<IEnumerable<TurmaRetornoDto>> ObterTurmasCopia(string turmaId, string disciplinaId)
        {
            var retorno = new List<TurmaRetornoDto>();

            var turma = await repositorioTurma.ObterPorCodigo(turmaId.ToString());
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var turmasAtribuidasAoProfessor = consultasProfessor.Listar(usuario.CodigoRf);

            var lstTurmasCJ = await repositorioAtribuicaoCJ.ObterPorFiltros(turma.ModalidadeCodigo, null, null,
                                    Convert.ToInt64(disciplinaId), usuario.CodigoRf, null, true);

            var turmasTitular = turmasAtribuidasAoProfessor.Where(t => t.AnoLetivo == turma.AnoLetivo &&
                                                                       t.Ano == turma.Ano &&
                                                                       t.Modalidade == turma.ModalidadeCodigo.ToString() &&
                                                                       t.CodTurma.ToString() != turma.CodigoTurma);

            if (turmasTitular != null && turmasTitular.Any())
            {
                retorno.AddRange(turmasTitular
                  .Select(x => new TurmaRetornoDto() { Codigo = x.CodTurma.ToString(), Nome = x.NomeTurma })
                  .ToList());
            }

            var turmasCJ = lstTurmasCJ.Where(t => t.Turma.AnoLetivo == turma.AnoLetivo &&
                                                  t.Turma.Ano == turma.Ano &&
                                                  t.Turma.ModalidadeCodigo == turma.ModalidadeCodigo &&
                                                  t.TurmaId != turma.CodigoTurma);

            if (turmasCJ != null && turmasCJ.Any())
            {
                retorno.AddRange(turmasCJ
                      .Select(x => new TurmaRetornoDto() { Codigo = x.TurmaId, Nome = x.Turma.Nome })
                      .ToList());
            }

            return retorno;
        }

        public async Task<IEnumerable<AtividadeAvaliativaExistenteRetornoDto>> ValidarAtividadeAvaliativaExistente(FiltroAtividadeAvaliativaExistenteDto dto)
        {
            var retorno = new List<AtividadeAvaliativaExistenteRetornoDto>();

            if (dto.AtividadeAvaliativaTurmaDatas != null && dto.AtividadeAvaliativaTurmaDatas.Any())
            {
                foreach (var filtro in dto.AtividadeAvaliativaTurmaDatas)
                {
                    if (filtro.DisciplinasId.Length <= 0)
                        throw new NegocioException("É necessário informar a disciplina");
                    var disciplina = ObterDisciplina(Convert.ToInt32(filtro.DisciplinasId[0]));
                    var usuario = await servicoUsuario.ObterUsuarioLogado();
                    DateTime dataAvaliacao = filtro.DataAvaliacao.Date;
                    var aula = await repositorioAula.ObterAulas(filtro.TurmaId.ToString(), null, usuario.CodigoRf, dataAvaliacao, filtro.DisciplinasId);

                    //verificar se tem para essa atividade
                    if (!aula.Any())
                    {
                        retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                        {
                            Erro = true,
                            Mensagem = "Não existe aula cadastrada para esse data.",
                            TurmaId = filtro.TurmaId
                        });
                    }
                    else
                    {
                        var tipoCalendarioId = aula.FirstOrDefault().TipoCalendarioId;
                        var perioEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataAvaliacao);
                        if (perioEscolar == null)
                        {
                            retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                            {
                                Erro = true,
                                Mensagem = "Não existe aula cadastrada para esse data.",
                                TurmaId = filtro.TurmaId
                            });
                        }
                        else
                        {
                            if (disciplina.Regencia && await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoRegencia(dataAvaliacao, null, null, filtro.TurmaId.ToString(), filtro.DisciplinasId, null, usuario.CodigoRf, null))
                            {
                                retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                                {
                                    Erro = true,
                                    Mensagem = "Já existe atividade avaliativa cadastrada para essa data e disciplina.",
                                    TurmaId = filtro.TurmaId
                                });
                            }
                            else if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, null, null, filtro.TurmaId.ToString(), filtro.DisciplinasId, usuario.CodigoRf, null))
                            {
                                retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                                {
                                    Erro = true,
                                    Mensagem = "Já existe atividade avaliativa cadastrada para essa data e disciplina.",
                                    TurmaId = filtro.TurmaId
                                });
                            }
                        }
                    }
                }
            }

            return retorno;
        }

        private IEnumerable<AtividadeAvaliativaCompletaDto> MapearAtividadeAvaliativaParaDto(IEnumerable<AtividadeAvaliativa> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }

        private AtividadeAvaliativaCompletaDto MapearParaDto(AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AtividadeAvaliativaRegencia> regencias = null, IEnumerable<AtividadeAvaliativaDisciplina> disciplinas = null, bool dentroPeriodo = true)
        {
            return atividadeAvaliativa == null ? null : new AtividadeAvaliativaCompletaDto
            {
                Id = atividadeAvaliativa.Id,
                CategoriaId = atividadeAvaliativa.Categoria,
                DataAvaliacao = atividadeAvaliativa.DataAvaliacao,
                Descricao = atividadeAvaliativa.DescricaoAvaliacao,
                DreId = atividadeAvaliativa.DreId,
                UeId = atividadeAvaliativa.UeId,
                Nome = atividadeAvaliativa.NomeAvaliacao,
                TipoAvaliacaoId = atividadeAvaliativa.TipoAvaliacaoId,
                TurmaId = atividadeAvaliativa.TurmaId,
                DentroPeriodo = dentroPeriodo,
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
                }).ToList(),
                DisciplinasId = disciplinas?.Select(x => x.DisciplinaId).ToArray()
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

        private DisciplinaDto ObterDisciplina(long idDisciplina)
        {
            long[] disciplinaId = { idDisciplina };
            var disciplina = servicoEOL.ObterDisciplinasPorIds(disciplinaId);
            if (!disciplina.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");
            return disciplina.FirstOrDefault();
        }
    }
}