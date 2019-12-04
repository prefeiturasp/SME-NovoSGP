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
            IUnitOfWork unitOfWork)

        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.consultasDisciplina = consultasDisciplina ?? throw new System.ArgumentException(nameof(consultasDisciplina));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentException(nameof(unitOfWork));
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentException(nameof(repositorioAula));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentException(nameof(repositorioPeriodoEscolar));
        }

        public async Task Alterar(AtividadeAvaliativaDto dto, long id)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, id, usuario.CodigoRf);
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Excluir(long idAtividadeAvaliativa)
        {
            var atividadeAvaliativa = repositorioAtividadeAvaliativa.ObterPorId(idAtividadeAvaliativa);
            if (atividadeAvaliativa is null)
                throw new NegocioException("Não foi possível localizar esta avaliação.");
            atividadeAvaliativa.Excluir();
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Inserir(AtividadeAvaliativaDto dto)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            var disciplina = ObterDisciplina(dto.DisciplinaId);
            dto.EhRegencia = disciplina.Regencia;
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L, usuario.CodigoRf);
            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Validar(FiltroAtividadeAvaliativaDto filtro)
        {
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
            if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoComMesmoNome(filtro.NomeAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, usuario.CodigoRf, perioEscolar.PeriodoInicio, perioEscolar.PeriodoFim))
            {
                throw new NegocioException("Já existe atividade avaliativa cadastrada com esse nome para esse bimestre.");
            }

            if (disciplina.Regencia)
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, filtro.DisciplinaContidaRegenciaId, usuario.CodigoRf))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para esse data e disciplina.");
                }
            }
            else
            {
                if (await repositorioAtividadeAvaliativa.VerificarSeJaExisteAvaliacaoNaoRegencia(dataAvaliacao, filtro.DreId, filtro.UeID, filtro.TurmaId, usuario.CodigoRf))
                {
                    throw new NegocioException("Já existe atividade avaliativa cadastrada para esse data.");
                }
            }
        }

        private AtividadeAvaliativa MapearDtoParaEntidade(AtividadeAvaliativaDto dto, long id, string usuarioRf)
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
            atividadeAvaliativa.CategoriaId = (int)dto.CategoriaId;
            atividadeAvaliativa.DisciplinaId = dto.DisciplinaId;
            atividadeAvaliativa.TipoAvaliacaoId = dto.TipoAvaliacaoId;
            atividadeAvaliativa.NomeAvaliacao = dto.Nome;
            atividadeAvaliativa.DescricaoAvaliacao = dto.Descricao;
            atividadeAvaliativa.DataAvaliacao = dto.DataAvaliacao;
            atividadeAvaliativa.DisciplinaContidaRegenciaId = dto.DisciplinaContidaRegenciaId;
            atividadeAvaliativa.EhRegencia = dto.EhRegencia;
            return atividadeAvaliativa;
        }

        private DisciplinaDto ObterDisciplina(int idDisciplina)
        {
            long[] disciplinaId = { idDisciplina };
            var disciplina = servicoEOL.ObterDisciplinasPorIds(disciplinaId);
            if (!disciplina.Any())
                throw new NegocioException("Disciplina não encontrada no EOL.");
            return disciplina.FirstOrDefault();
        }
    }
}