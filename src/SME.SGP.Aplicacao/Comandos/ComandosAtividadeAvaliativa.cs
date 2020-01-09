

using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtividadeAvaliativa : IComandosAtividadeAvaliativa
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasProfessor consultasProfessor;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAtividadeAvaliativa(
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IConsultasDisciplina consultasDisciplina,
            IConsultasProfessor consultasProfessor,
            IRepositorioAula repositorioAula,
            IServicoUsuario servicoUsuario,
            IServicoEOL servicoEOL,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IUnitOfWork unitOfWork,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)

        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentException(nameof(consultasDisciplina));
            this.consultasProfessor = consultasProfessor ?? throw new ArgumentException(nameof(consultasProfessor));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            this.repositorioAula = repositorioAula ?? throw new ArgumentException(nameof(repositorioAula));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentException(nameof(repositorioPeriodoEscolar));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaDisciplina));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Alterar(AtividadeAvaliativaDto dto, long id)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var disciplina = ObterDisciplina(dto.DisciplinasId[0]);
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, id, usuario.CodigoRf, disciplina.Regencia);

            var atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(id);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                if (disciplina.Regencia)
                {
                    var regencias = await repositorioAtividadeAvaliativaRegencia.Listar(atividadeAvaliativa.Id);
                    foreach (var regencia in regencias)
                        repositorioAtividadeAvaliativaRegencia.Remover(regencia);
                    foreach (string idRegencia in dto.DisciplinaContidaRegenciaId)
                    {
                        var ativRegencia = new AtividadeAvaliativaRegencia
                        {
                            AtividadeAvaliativaId = atividadeAvaliativa.Id,
                            DisciplinaContidaRegenciaId = idRegencia
                        };
                        await repositorioAtividadeAvaliativaRegencia.SalvarAsync(ativRegencia);
                    }
                }

                foreach(var atividadeDisciplina in atividadeDisciplinas)
                {
                    atividadeDisciplina.Excluir();
                    var existeDisciplina = dto.DisciplinasId.Any(a => a == atividadeDisciplina.DisciplinaId);
                    if(existeDisciplina)
                    {
                        atividadeDisciplina.Excluido = false;
                    }
                    await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(atividadeDisciplina);
                }

                foreach(var disciplinaId in dto.DisciplinasId)
                {
                    var existeDisciplina = atividadeDisciplinas.Any(a => a.DisciplinaId == disciplinaId);
                    if (!existeDisciplina)
                    {
                        var novaDisciplina = new AtividadeAvaliativaDisciplina
                        {
                            AtividadeAvaliativaId = atividadeAvaliativa.Id,
                            DisciplinaId = disciplinaId
                        };
                        await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(novaDisciplina);
                    }
                }

                await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
                unitOfWork.PersistirTransacao();
            }

            mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto("Atividade Avaliativa alterada com sucesso", true));
            mensagens.AddRange(await CopiarAtividadeAvaliativa(dto, atividadeAvaliativa.ProfessorRf));

            return mensagens;
        }

        public async Task Excluir(long idAtividadeAvaliativa)
        {
            var atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(idAtividadeAvaliativa);
            if (atividadeAvaliativa is null)
                throw new NegocioException("Não foi possível localizar esta avaliação.");
            var atividadeDisciplinas = await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(idAtividadeAvaliativa);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                foreach (var atividadeDisciplina in atividadeDisciplinas)
                {
                    var disciplina = ObterDisciplina(atividadeDisciplina.DisciplinaId);
                    atividadeDisciplina.Excluir();
                    atividadeAvaliativa.Excluir();
                    await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
                    if (disciplina.Regencia)
                    {
                        var regencias = await repositorioAtividadeAvaliativaRegencia.Listar(atividadeAvaliativa.Id);
                        foreach (var regencia in regencias)
                        {
                            regencia.Excluir();
                            await repositorioAtividadeAvaliativaRegencia.SalvarAsync(regencia);
                        }
                    }
                    await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(atividadeDisciplina);
                }
                unitOfWork.PersistirTransacao();
            }
        }

        public async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Inserir(AtividadeAvaliativaDto dto)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var disciplina = ObterDisciplina(dto.DisciplinasId[0]);
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L, usuario.CodigoRf, disciplina.Regencia);
            mensagens.AddRange(await Salvar(atividadeAvaliativa, dto));
            mensagens.AddRange(await CopiarAtividadeAvaliativa(dto, atividadeAvaliativa.ProfessorRf));

            return mensagens;
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> Salvar(AtividadeAvaliativa atividadeAvaliativa, AtividadeAvaliativaDto dto, bool ehCopia = false)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            unitOfWork.IniciarTransacao();

            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
            if (atividadeAvaliativa.EhRegencia)
            {
                if (dto.DisciplinaContidaRegenciaId.Length == 0)
                    throw new NegocioException("É necessário informar os componentes curriculares da regência");

                foreach (string id in dto.DisciplinaContidaRegenciaId)
                {
                    var ativRegencia = new AtividadeAvaliativaRegencia
                    {
                        AtividadeAvaliativaId = atividadeAvaliativa.Id,
                        DisciplinaContidaRegenciaId = id
                    };
                    await repositorioAtividadeAvaliativaRegencia.SalvarAsync(ativRegencia);
                }
            }


            foreach (var id in dto.DisciplinasId)
            {
                var ativDisciplina = new AtividadeAvaliativaDisciplina
                {
                    AtividadeAvaliativaId = atividadeAvaliativa.Id,
                    DisciplinaId = id
                };
                await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(ativDisciplina);
            }

            unitOfWork.PersistirTransacao();

            if (!ehCopia)
                mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto("Atividade Avaliativa criada com sucesso", true));

            return mensagens;
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> CopiarAtividadeAvaliativa(AtividadeAvaliativaDto atividadeAvaliativaDto, string usuarioRf)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            if (atividadeAvaliativaDto.TurmasParaCopiar != null && atividadeAvaliativaDto.TurmasParaCopiar.Any())
            {
                mensagens.AddRange(await ValidarCopias(atividadeAvaliativaDto, usuarioRf));

                if (mensagens.Any())
                    return mensagens;

                foreach (var turma in atividadeAvaliativaDto.TurmasParaCopiar)
                {
                    atividadeAvaliativaDto.TurmaId = turma.TurmaId;
                    atividadeAvaliativaDto.DataAvaliacao = turma.DataAtividadeAvaliativa;

                    try
                    {
                        await Validar(MapearDtoParaFiltroValidacao(atividadeAvaliativaDto));
                        var atividadeParaCopiar = MapearDtoParaEntidade(new AtividadeAvaliativa(), atividadeAvaliativaDto, usuarioRf);
                        await Salvar(atividadeParaCopiar, atividadeAvaliativaDto, true);

                        mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto($"Atividade copiada para a turma: '{turma.TurmaId}' na data '{turma.DataAtividadeAvaliativa.ToString("dd/MM/yyyy")}'.", true));
                    }
                    catch (NegocioException nex)
                    {
                        mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto($"Erro ao copiar para a turma: '{turma.TurmaId}' na data '{turma.DataAtividadeAvaliativa.ToString("dd/MM/yyyy")}'. {nex.Message}"));
                    }
                }
                unitOfWork.PersistirTransacao();
            }

            return mensagens;
        }

        private FiltroAtividadeAvaliativaDto MapearDtoParaFiltroValidacao(AtividadeAvaliativaDto atividadeAvaliativaDto)
        {
            return new FiltroAtividadeAvaliativaDto()
            {
                DataAvaliacao = atividadeAvaliativaDto.DataAvaliacao,
                DisciplinaContidaRegenciaId = atividadeAvaliativaDto.DisciplinaContidaRegenciaId,
                DreId = atividadeAvaliativaDto.DreId,
                Nome = atividadeAvaliativaDto.Nome,
                TipoAvaliacaoId = (int)atividadeAvaliativaDto.TipoAvaliacaoId,
                TurmaId = atividadeAvaliativaDto.TurmaId,
                UeID = atividadeAvaliativaDto.UeId
            };
        }

        public async Task Validar(FiltroAtividadeAvaliativaDto filtro)
        {
            if (filtro.DisciplinasId.Length <= 0)
                throw new NegocioException("É necessário informar o componente curricular");
            var disciplina = ObterDisciplina(filtro.DisciplinasId[0]);
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            DateTime dataAvaliacao = filtro.DataAvaliacao.Value.Date;
            var aula = await repositorioAula.ObterAulas(filtro.TurmaId, filtro.UeID, usuario.CodigoRf, dataAvaliacao, filtro.DisciplinasId);

            //verificar se tem para essa atividade
            if (!aula.Any())
                throw new NegocioException("Não existe aula cadastrada para esse data.");

            var tipoCalendarioId = aula.FirstOrDefault().TipoCalendarioId;
            var perioEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataAvaliacao);
            if (perioEscolar == null)
                throw new NegocioException("Não foi encontrado nenhum período escolar para essa data.");

            //verificar se já existe atividade com o mesmo nome no mesmo bimestre
            if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoComMesmoNome(filtro.Nome, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinasId, usuario.CodigoRf, perioEscolar.PeriodoInicio, perioEscolar.PeriodoFim, filtro.Id))
            {
                throw new NegocioException("Já existe atividade avaliativa cadastrada com esse nome para esse bimestre.");
            }

            if (disciplina.Regencia)
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinasId, filtro.DisciplinaContidaRegenciaId, usuario.CodigoRf, filtro.Id))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
                }
            }
            else
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinasId, usuario.CodigoRf, filtro.Id))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para essa data e componente curricular.");
                }
            }
        }

        private AtividadeAvaliativa MapearDtoParaEntidade(AtividadeAvaliativaDto dto, long id, string usuarioRf, bool ehRegencia)
        {
            AtividadeAvaliativa atividadeAvaliativa = new AtividadeAvaliativa();
            if (id > 0L)
            {
                atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(id);
            }
            if (string.IsNullOrEmpty(atividadeAvaliativa.ProfessorRf))
            {
                atividadeAvaliativa.ProfessorRf = usuarioRf;
            }
            atividadeAvaliativa.UeId = dto.UeId;
            atividadeAvaliativa.DreId = dto.DreId;
            atividadeAvaliativa.TurmaId = dto.TurmaId;
            atividadeAvaliativa.Categoria = dto.CategoriaId;
            atividadeAvaliativa.TipoAvaliacaoId = dto.TipoAvaliacaoId;
            atividadeAvaliativa.NomeAvaliacao = dto.Nome;
            atividadeAvaliativa.DescricaoAvaliacao = dto.Descricao;
            atividadeAvaliativa.DataAvaliacao = dto.DataAvaliacao.Local();
            atividadeAvaliativa.EhRegencia = ehRegencia;
            return atividadeAvaliativa;
        }

        private AtividadeAvaliativa MapearDtoParaEntidade(AtividadeAvaliativa atividadeAvaliativa, AtividadeAvaliativaDto dto, string usuarioRf)
        {
            atividadeAvaliativa.ProfessorRf = usuarioRf;
            atividadeAvaliativa.UeId = dto.UeId;
            atividadeAvaliativa.DreId = dto.DreId;
            atividadeAvaliativa.TurmaId = dto.TurmaId;
            atividadeAvaliativa.Categoria = dto.CategoriaId;
            atividadeAvaliativa.TipoAvaliacaoId = dto.TipoAvaliacaoId;
            atividadeAvaliativa.NomeAvaliacao = dto.Nome;
            atividadeAvaliativa.DescricaoAvaliacao = dto.Descricao;
            atividadeAvaliativa.DataAvaliacao = dto.DataAvaliacao.Local();
            atividadeAvaliativa.EhRegencia = dto.EhRegencia;
            return atividadeAvaliativa;
        }

        private DisciplinaDto ObterDisciplina(string idDisciplina)
        {
            long[] disciplinaId = { long.Parse(idDisciplina) };
            var disciplina = servicoEOL.ObterDisciplinasPorIds(disciplinaId);
            if (!disciplina.Any())
                throw new NegocioException("Componente curricular não encontrado no EOL.");
            return disciplina.FirstOrDefault();
        }

        private async Task<IEnumerable<RetornoCopiarAtividadeAvaliativaDto>> ValidarCopias(AtividadeAvaliativaDto atividadeAvaliativaDto, string codigoRf)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            var turmasAtribuidasAoProfessor = await ObterTurmasAtribuidasAoProfessor(codigoRf, long.Parse(atividadeAvaliativaDto.DisciplinasId[0]));

            if (atividadeAvaliativaDto.TurmasParaCopiar != null && atividadeAvaliativaDto.TurmasParaCopiar.Any())
            {
                var idsTurmasSelecionadas = atividadeAvaliativaDto.TurmasParaCopiar.Select(x => x.TurmaId).ToList();
                idsTurmasSelecionadas.Add(atividadeAvaliativaDto.TurmaId);

                mensagens.AddRange(ValidaTurmasProfessor(turmasAtribuidasAoProfessor, idsTurmasSelecionadas));

                if (mensagens.Any())
                    return mensagens;

                mensagens.AddRange(ValidaTurmasAno(turmasAtribuidasAoProfessor, idsTurmasSelecionadas));
            }

            return mensagens;
        }

        private async Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessor(string codigoRf, long disciplinaId)
        {
            var turmasAtribuidasAoProfessorTitular = consultasProfessor.Listar(codigoRf);
            var turmasAtribuidasAoProfessorCJ = await repositorioAtribuicaoCJ.ObterPorFiltros(null, null, null, disciplinaId, codigoRf, null, true);

            var turmasAtribuidasAoProfessor = new List<TurmaDto>();

            if (turmasAtribuidasAoProfessorTitular != null && turmasAtribuidasAoProfessorTitular.Any())
                turmasAtribuidasAoProfessor.AddRange(turmasAtribuidasAoProfessorTitular
                          .Select(x => new TurmaDto() { CodigoTurma = x.CodTurma, NomeTurma = x.NomeTurma, Ano = x.Ano })
                          .ToList());

            if (turmasAtribuidasAoProfessorCJ != null && turmasAtribuidasAoProfessorCJ.Any())
                turmasAtribuidasAoProfessor.AddRange(turmasAtribuidasAoProfessorCJ
                      .Select(x => new TurmaDto()
                      {
                          CodigoTurma = Convert.ToInt32(x.TurmaId),
                          NomeTurma = x.Turma.Nome,
                          Ano = x.Turma.Ano
                      })
                      .ToList());

            return turmasAtribuidasAoProfessor;
        }

        private IEnumerable<RetornoCopiarAtividadeAvaliativaDto> ValidaTurmasAno(
                                     IEnumerable<TurmaDto> turmasAtribuidasAoProfessor,
                                     IEnumerable<string> idsTurmasSelecionadas)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            var turmasAtribuidasSelecionadas = turmasAtribuidasAoProfessor.Where(t => idsTurmasSelecionadas.Contains(t.CodigoTurma.ToString()));
            var anoTurma = turmasAtribuidasSelecionadas.First().Ano;

            if (!turmasAtribuidasSelecionadas.All(x => x.Ano == anoTurma))
            {
                mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto(
                    "Somente é possível copiar a atividade avaliativa para turmas dentro do mesmo ano"));
            }

            return mensagens;
        }

        private IEnumerable<RetornoCopiarAtividadeAvaliativaDto> ValidaTurmasProfessor(IEnumerable<TurmaDto> turmasAtribuidasAoProfessor,
                                                                                       IEnumerable<string> idsTurmasSelecionadas)
        {
            var mensagens = new List<RetornoCopiarAtividadeAvaliativaDto>();

            if (turmasAtribuidasAoProfessor != null && idsTurmasSelecionadas.Any(c => !turmasAtribuidasAoProfessor.Select(t => t.CodigoTurma.ToString()).Contains(c)))
                mensagens.Add(new RetornoCopiarAtividadeAvaliativaDto("Somente é possível copiar a atividade avaliativa para turmas atribuidas ao professor"));

            return mensagens;
        }
    }
}