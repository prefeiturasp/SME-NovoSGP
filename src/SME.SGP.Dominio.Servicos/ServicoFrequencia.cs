using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFrequencia : IServicoFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFrequencia(IRepositorioFrequencia repositorioFrequencia,
                                 IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
                                 IRepositorioAula repositorioAula,
                                 IServicoUsuario servicoUsuario,
                                 IUnitOfWork unitOfWork,
                                 IServicoEOL servicoEOL)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new System.ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new System.ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaAusenciasPorAula(long aulaId)
        {
            return repositorioFrequencia.ObterListaFrequenciaPorAula(aulaId);
        }

        public async Task Registrar(long aulaId, IEnumerable<RegistroAusenciaAluno> registroAusenciaAlunos)
        {
            var aula = ObterAula(aulaId);

            await ValidaSeUsuarioPodeCriarAula(aula);
            var alunos = await ObterAlunos(aula);

            var registroFrequencia = repositorioFrequencia.ObterRegistroFrequenciaPorAulaId(aulaId);

            unitOfWork.IniciarTransacao();

            registroFrequencia = RegistraFrequenciaTurma(aula, registroFrequencia);

            RegistraAusenciaAlunos(registroAusenciaAlunos, alunos, registroFrequencia);

            unitOfWork.PersistirTransacao();
        }

        private async Task<IEnumerable<Aplicacao.Integracoes.Respostas.AlunoPorTurmaResposta>> ObterAlunos(Aula aula)
        {
            var alunos = await servicoEOL.ObterAlunosPorTurna(aula.TurmaId);
            if (alunos == null || !alunos.Any())
            {
                throw new NegocioException("Não foram encontrados alunos para a turma informada.");
            }

            return alunos;
        }

        private Aula ObterAula(long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
            {
                throw new NegocioException("A aula informada não foi encontrada.");
            }

            return aula;
        }

        private void RegistraAusenciaAlunos(IEnumerable<RegistroAusenciaAluno> registroAusenciaAlunos, IEnumerable<Aplicacao.Integracoes.Respostas.AlunoPorTurmaResposta> alunos, RegistroFrequencia registroFrequencia)
        {
            foreach (var ausencia in registroAusenciaAlunos)
            {
                var aluno = alunos.FirstOrDefault(c => c.CodigoAluno == ausencia.CodigoAluno);
                if (aluno == null)
                {
                    throw new NegocioException("O aluno informado na frequência não pertence a essa turma.");
                }
                if (aluno.EstaInativo())
                {
                    throw new NegocioException($"Não foi possível registrar a frequência pois o aluno: '{aluno.NomeAluno}' está com a situação: '{aluno.SituacaoMatricula}'.");
                }
                ausencia.RegistroFrequenciaId = registroFrequencia.Id;
                repositorioRegistroAusenciaAluno.Salvar(ausencia);
            }
        }

        private RegistroFrequencia RegistraFrequenciaTurma(Aula aula, RegistroFrequencia registroFrequencia)
        {
            if (registroFrequencia == null)
            {
                registroFrequencia = new RegistroFrequencia(aula);
                repositorioFrequencia.Salvar(registroFrequencia);
            }
            else
                repositorioRegistroAusenciaAluno.MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(registroFrequencia.Id);
            return registroFrequencia;
        }

        private async Task ValidaSeUsuarioPodeCriarAula(Aula aula)
        {
            var usuario = await servicoUsuario.ObterUsuarioLogado();
            if (!usuario.PodeRegistrarFrequencia(aula))
            {
                throw new NegocioException("Você não pode registrar frequência para a aula/turma informada.");
            }
        }
    }
}