using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtividadeAvaliativa : IComandosAtividadeAvaliativa
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ComandosAtividadeAvaliativa(
            IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa,
            IConsultasDisciplina consultasDisciplina,
            IRepositorioAula repositorioAula,
            IServicoUsuario servicoUsuario,
            IServicoEOL servicoEOL,
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
            IUnitOfWork unitOfWork,
            IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia,
            IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)

        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentException(nameof(consultasDisciplina));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
            this.repositorioAula = repositorioAula ?? throw new ArgumentException(nameof(repositorioAula));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentException(nameof(repositorioPeriodoEscolar));
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaRegencia));
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentException(nameof(repositorioAtividadeAvaliativaDisciplina));
        }

        public async Task Alterar(AtividadeAvaliativaDto dto, long id)
        {
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
                    var existeDisciplina = dto.DisciplinasId.Select(a => a == atividadeDisciplina.Id).FirstOrDefault();
                    if(existeDisciplina)
                    {
                        atividadeDisciplina.Excluido = false;
                        await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(atividadeDisciplina);
                    }
                }

                foreach(var disciplinaId in dto.DisciplinasId)
                {
                    var existeDisciplina = atividadeDisciplinas.Select(a => a.DisciplinaId == disciplinaId).FirstOrDefault();
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
                }
                unitOfWork.PersistirTransacao();
            }
        }

        public async Task Inserir(AtividadeAvaliativaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var disciplina = ObterDisciplina(dto.DisciplinasId[0]);
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L, usuario.CodigoRf, disciplina.Regencia);
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
                if (atividadeAvaliativa.EhRegencia)
                {
                    if (dto.DisciplinaContidaRegenciaId.Length == 0)
                        throw new NegocioException("É necessário informar as disciplinas da regência");

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
                foreach (long id in dto.DisciplinasId)
                {
                    var ativDisciplina = new AtividadeAvaliativaDisciplina
                    {
                        AtividadeAvaliativaId = atividadeAvaliativa.Id,
                        DisciplinaId = id
                    };
                    await repositorioAtividadeAvaliativaDisciplina.SalvarAsync(ativDisciplina);
                }
                unitOfWork.PersistirTransacao();
            }
        }

        public async Task Validar(FiltroAtividadeAvaliativaDto filtro)
        {
            if (string.IsNullOrEmpty(filtro.DisciplinaId))
                throw new NegocioException("É necessário informar a disciplina");
            var disciplina = ObterDisciplina(Convert.ToInt32(filtro.DisciplinaId));
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            DateTime dataAvaliacao = filtro.DataAvaliacao.Value.Date;
            var aula = await repositorioAula.ObterAulas(filtro.TurmaId, filtro.UeID, usuario.CodigoRf, dataAvaliacao, filtro.DisciplinaId);

            //verificar se tem para essa atividade
            if (!aula.Any())
                throw new NegocioException("Não existe aula cadastrada para esse data.");

            var tipoCalendarioId = aula.FirstOrDefault().TipoCalendarioId;
            var perioEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataAvaliacao);
            if (perioEscolar == null)
                throw new NegocioException("Não foi encontrado nenhum período escolar para essa data.");

            //verificar se já existe atividade com o mesmo nome no mesmo bimestre
            if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoComMesmoNome(filtro.Nome, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinaId, usuario.CodigoRf, perioEscolar.PeriodoInicio, perioEscolar.PeriodoFim, filtro.Id))
            {
                throw new NegocioException("Já existe atividade avaliativa cadastrada com esse nome para esse bimestre.");
            }

            if (disciplina.Regencia)
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinaId, filtro.DisciplinaContidaRegenciaId, usuario.CodigoRf, filtro.Id))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para essa data e disciplina.");
                }
            }
            else
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinaId, usuario.CodigoRf, filtro.Id))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para essa data e disciplina.");
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
            atividadeAvaliativa.DataAvaliacao = dto.DataAvaliacao;
            atividadeAvaliativa.EhRegencia = ehRegencia;
            return atividadeAvaliativa;
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