using MediatR;
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
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultaAtividadeAvaliativa(
            IConsultasProfessor consultasProfessor,
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
            IRepositorioAulaConsulta repositorioAula,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IServicoUsuario servicoUsuario,
            IContextoAplicacao contextoAplicacao,
            IConsultasTurma consultasTurma,
            IConsultasPeriodoEscolar consultasPeriodoEscolar,
            IConsultasPeriodoFechamento consultasPeriodoFechamento, IMediator mediator) : base(contextoAplicacao)
        {
            this.consultasProfessor = consultasProfessor ?? throw new ArgumentNullException(nameof(consultasProfessor));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>> ListarPaginado(FiltroAtividadeAvaliativaDto filtro)
        {
            return MapearParaDtoComPaginacao(await repositorioAtividadeAvaliativa.Listar(filtro.DataAvaliacao.HasValue
                                                                                               ? filtro.DataAvaliacao.Value.Date
                                                                                               : filtro.DataAvaliacao,
                                                                                         filtro.DreId,
                                                                                         filtro.UeID,
                                                                                         filtro.Nome,
                                                                                         filtro.TipoAvaliacaoId,
                                                                                         filtro.TurmaId,
                                                                                         Paginacao));
        }

        public async Task<IEnumerable<AtividadeAvaliativa>> ObterAvaliacoesNoBimestre(string turmaCodigo, string disciplinaId, DateTime periodoInicio, DateTime periodoFim)
            => await repositorioAtividadeAvaliativa.ListarPorTurmaDisciplinaPeriodo(turmaCodigo, disciplinaId, periodoInicio, periodoFim);

        public async Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var atividade = await repositorioAtividadeAvaliativa.ObterPorIdAsync(id);

            if (atividade is null)
                throw new NegocioException("Atividade avaliativa não encontrada");

            IEnumerable<AtividadeAvaliativaRegencia> atividadeRegencias = null;

            IEnumerable<AtividadeAvaliativaDisciplina> atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(id);

            var podeEditarAvaliacao = usuarioLogado.EhProfessorCj() && atividade.EhCj || usuarioLogado.EhProfessor() && !atividade.EhCj || usuarioLogado.EhGestorEscolar();

            if (atividade.EhRegencia)
                atividadeRegencias = await repositorioAtividadeAvaliativaRegencia.Listar(id);

            var dentroPeriodo = await AtividadeAvaliativaDentroPeriodo(atividade);

            return MapearParaDto(atividade, atividadeRegencias, atividadeDisciplinas, dentroPeriodo, podeEditarAvaliacao);
        }

        public async Task<bool> AtividadeAvaliativaDentroPeriodo(AtividadeAvaliativa atividadeAvaliativa)
        {
            return await AtividadeAvaliativaDentroPeriodo(atividadeAvaliativa.TurmaId, atividadeAvaliativa.DataAvaliacao);
        }

        public async Task<bool> AtividadeAvaliativaDentroPeriodo(string turmaId, DateTime dataAula)
        {
            var turma = await consultasTurma.ObterComUeDrePorCodigo(turmaId);

            if (turma.EhNulo())
                throw new NegocioException($"Não foi possivel obter a turma da aula");

            var bimestreAtual = await consultasPeriodoEscolar.ObterBimestre(dataAula, turma.ModalidadeCodigo, turma.Semestre);
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

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId.ToString()));
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            if (usuario.EhPerfilProfessor())
            {
                var turmasAtribuidasAoProfessor = await consultasProfessor.Listar(usuario.CodigoRf);

                var lstTurmasCJ = await repositorioAtribuicaoCJ.ObterPorFiltros(turma.ModalidadeCodigo, null, null,
                                        Convert.ToInt64(disciplinaId), usuario.CodigoRf, null, true);

                var turmasTitular = turmasAtribuidasAoProfessor.Where(t => t.AnoLetivo == turma.AnoLetivo &&
                                                                           t.Ano == turma.Ano &&
                                                                           t.Modalidade == turma.ModalidadeCodigo.ToString() &&
                                                                           t.CodTurma.ToString() != turma.CodigoTurma);

                if (turmasTitular.NaoEhNulo() && turmasTitular.Any())
                {
                    retorno.AddRange(turmasTitular
                           .Select(x => new TurmaRetornoDto() { Codigo = x.CodTurma.ToString(), Nome = x.NomeTurma })
                           .ToList());
                }

                var turmasCJ = lstTurmasCJ.Where(t => t.Turma.AnoLetivo == turma.AnoLetivo &&
                                                      t.Turma.Ano == turma.Ano &&
                                                      t.Turma.ModalidadeCodigo == turma.ModalidadeCodigo &&
                                                      t.TurmaId != turma.CodigoTurma);

                if (turmasCJ.NaoEhNulo() && turmasCJ.Any())
                {
                    retorno.AddRange(turmasCJ
                          .Select(x => new TurmaRetornoDto() { Codigo = x.TurmaId, Nome = x.Turma.Nome })
                          .ToList());
                }
            }
            else if (usuario.EhCP())
            {
                var turmasAtribuidasCP = await mediator.Send(new ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery(turma.AnoLetivo, usuario.CodigoRf, (int)turma.ModalidadeCodigo, turma.Ano, turma.Id));

                if (turmasAtribuidasCP.Any() && turmasAtribuidasCP.NaoEhNulo())
                    return turmasAtribuidasCP.Select(t => new TurmaRetornoDto()
                    {
                        Codigo = t.CodigoTurma,
                        Nome = t.Nome
                    });
            }

            return retorno.DistinctBy(x => x.Codigo);
        }

        public async Task<IEnumerable<AtividadeAvaliativaExistenteRetornoDto>> ValidarAtividadeAvaliativaExistente(FiltroAtividadeAvaliativaExistenteDto dto)
        {
            var retorno = new List<AtividadeAvaliativaExistenteRetornoDto>();

            if (dto.AtividadeAvaliativaTurmaDatas.NaoEhNulo() && dto.AtividadeAvaliativaTurmaDatas.Any())
            {
                foreach (var filtro in dto.AtividadeAvaliativaTurmaDatas)
                {
                    if (filtro.DisciplinasId.Length <= 0)
                        throw new NegocioException("É necessário informar o componente curricular");
                    var disciplina = await ObterDisciplina(Convert.ToInt32(filtro.DisciplinasId[0]));
                    var usuario = await servicoUsuario.ObterUsuarioLogado();

                    var regenteAtual = !usuario.EhProfessorCj() && !usuario.EhGestorEscolar()
                    ? await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(disciplina.Id, filtro.TurmaId.ToString(), DateTime.Now.Date, usuario))
                    : true;

                    DateTime dataAvaliacao = filtro.DataAvaliacao.Date;
                    var aula = await repositorioAula.ObterAulas(filtro.TurmaId.ToString(), null, regenteAtual ? string.Empty : usuario.CodigoRf, dataAvaliacao, filtro.DisciplinasId, usuario.EhProfessorCj());

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
                        if (perioEscolar.EhNulo())
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
                                    Mensagem = "Já existe atividade avaliativa cadastrada para essa data e componente curricular.",
                                    TurmaId = filtro.TurmaId
                                });
                            }
                            else if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, null, null, filtro.TurmaId.ToString(), filtro.DisciplinasId, usuario.CodigoRf, null))
                            {
                                retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                                {
                                    Erro = true,
                                    Mensagem = "Já existe atividade avaliativa cadastrada para essa data e componente curricular.",
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

        private AtividadeAvaliativaCompletaDto MapearParaDto(AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AtividadeAvaliativaRegencia> regencias = null, IEnumerable<AtividadeAvaliativaDisciplina> disciplinas = null, bool dentroPeriodo = true, bool podeEditarAvaliacao = false)
        {
            return atividadeAvaliativa.EhNulo() ? null : new AtividadeAvaliativaCompletaDto
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
                Importado = atividadeAvaliativa.AtividadeClassroomId.HasValue,
                PodeEditarAvaliacao = podeEditarAvaliacao,
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
            if (atividadeAvaliativaPaginado.EhNulo())
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

        private async Task<DisciplinaDto> ObterDisciplina(long idDisciplina)
        {
            var disciplina = await mediator.Send(new ObterComponenteCurricularPorIdQuery(idDisciplina));
            if (disciplina is null)
                throw new NegocioException("Componente curricular não encontrado no EOL.");
            return disciplina;
        }
    }
}