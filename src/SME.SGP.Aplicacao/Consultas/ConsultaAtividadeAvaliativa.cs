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
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultaAtividadeAvaliativa(
            IConsultasProfessor consultasProfessor,
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioTurma repositorioTurma,
            IRepositorioAula repositorioAula,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IServicoUsuario servicoUsuario,
            IServicoEOL servicoEOL,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
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

        public async Task<AvaliacoesBimestresDto> ObterAvaliacoesEBimestres(string turmaCodigo, string disciplinaId, int anoLetivo, int? bimestre, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidadeTipoCalendario);

            if (tipoCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário escolar, para a modalidade informada.");

            var periodosEscolares = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            if (!bimestre.HasValue || bimestre.Value == 0)
                bimestre = ObterBimestreAtual(periodosEscolares);

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestre);

            if (periodoEscolar == null)
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            var avaliacoes = await repositorioAtividadeAvaliativa.ListarPorTurmaDisciplinaPeriodo(turmaCodigo, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);

            return new AvaliacoesBimestresDto
            {
                Avaliacoes = avaliacoes,
                PeriodoAtual = periodoEscolar,
                QuantidadeBimestres = periodosEscolares.Count()
            };
        }

        public async Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id)
        {
            IEnumerable<AtividadeAvaliativaRegencia> atividadeRegencias = null;
            IEnumerable<AtividadeAvaliativaDisciplina> atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(id);
            var atividade = await repositorioAtividadeAvaliativa.ObterPorIdAsync(id);
            if (atividade is null)
                throw new NegocioException("Atividade avaliativa não encontrada");
            if (atividade.EhRegencia)
                atividadeRegencias = await repositorioAtividadeAvaliativaRegencia.Listar(id);
            return MapearParaDto(atividade, atividadeRegencias, atividadeDisciplinas);
        }

        public async Task<IEnumerable<TurmaRetornoDto>> ObterTurmasCopia(string turmaId, string disciplinaId)
        {
            var retorno = new List<TurmaRetornoDto>();

            var turma = repositorioTurma.ObterPorId(turmaId.ToString());
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
                        throw new NegocioException("É necessário informar o componente curricular");
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
                        continue;
                    }

                    var tipoCalendarioId = aula.FirstOrDefault().TipoCalendarioId;
                    var perioEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataAvaliacao);
                    if (perioEscolar == null)
                    {
                        retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                        {
                            Erro = true,
                            Mensagem = "Não existe aula cadastrada para esse data.",
                            TurmaId = filtro.TurmaId
                        });
                        continue;
                    }

                    if (disciplina.Regencia)
                    {
                        if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoRegencia(dataAvaliacao, null, null, filtro.TurmaId.ToString(), filtro.DisciplinasId, null, usuario.CodigoRf, null))
                        {
                            retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                            {
                                Erro = true,
                                Mensagem = "Já existe atividade avaliativa cadastrada para essa data e componente curricular.",
                                TurmaId = filtro.TurmaId
                            });
                            continue;
                        }
                    }
                    else
                    {
                        if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, null, null, filtro.TurmaId.ToString(), filtro.DisciplinasId, usuario.CodigoRf, null))
                        {
                            retorno.Add(new AtividadeAvaliativaExistenteRetornoDto()
                            {
                                Erro = true,
                                Mensagem = "Já existe atividade avaliativa cadastrada para essa data e componente curricular.",
                                TurmaId = filtro.TurmaId
                            });
                            continue;
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

        private AtividadeAvaliativaCompletaDto MapearParaDto(AtividadeAvaliativa atividadeAvaliativa, IEnumerable<AtividadeAvaliativaRegencia> regencias = null, IEnumerable<AtividadeAvaliativaDisciplina> disciplinas = null)
        {
            return atividadeAvaliativa == null ? null : new AtividadeAvaliativaCompletaDto
            {
                Id = atividadeAvaliativa.Id,
                CategoriaId = (CategoriaAtividadeAvaliativa)atividadeAvaliativa.Categoria,
                DataAvaliacao = atividadeAvaliativa.DataAvaliacao,
                Descricao = atividadeAvaliativa.DescricaoAvaliacao,
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

        private int? ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return 1;
            else return periodoEscolar.Bimestre;
        }

        private DisciplinaDto ObterDisciplina(long idDisciplina)
        {
            long[] disciplinaId = { idDisciplina };
            var disciplina = servicoEOL.ObterDisciplinasPorIds(disciplinaId);
            if (!disciplina.Any())
                throw new NegocioException("Componente curricular não encontrado no EOL.");
            return disciplina.FirstOrDefault();
        }
    }
}