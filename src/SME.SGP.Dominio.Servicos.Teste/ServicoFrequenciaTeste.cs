using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoFrequenciaTeste
    {
        private readonly Mock<IConsultasDisciplina> consultaDisciplina;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioFrequencia> repositorioFrequencia;
        private readonly Mock<IRepositorioRegistroAusenciaAluno> repositorioRegistroAusenciaAluno;
        private readonly Mock<IRepositorioTurma> repositorioTurma;
        private readonly Mock<IRepositorioUe> repositorioUe;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly ServicoFrequencia servicoFrequencia;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoFrequenciaTeste()
        {
            repositorioFrequencia = new Mock<IRepositorioFrequencia>();
            repositorioRegistroAusenciaAluno = new Mock<IRepositorioRegistroAusenciaAluno>();
            repositorioAula = new Mock<IRepositorioAula>();
            servicoUsuario = new Mock<IServicoUsuario>();
            unitOfWork = new Mock<IUnitOfWork>();
            servicoEOL = new Mock<IServicoEOL>();
            consultaDisciplina = new Mock<IConsultasDisciplina>();
            repositorioUe = new Mock<IRepositorioUe>();
            repositorioTurma = new Mock<IRepositorioTurma>();

            servicoFrequencia = new ServicoFrequencia(repositorioFrequencia.Object,
                                                      repositorioRegistroAusenciaAluno.Object,
                                                      repositorioAula.Object,
                                                      servicoUsuario.Object,
                                                      unitOfWork.Object,
                                                      servicoEOL.Object,
                                                      repositorioUe.Object,
                                                      repositorioTurma.Object,
                                                      consultaDisciplina.Object);
            Setup();
        }

        [Fact]
        public async Task Deve_Atualizar_Quantidade_Para_Mais()
        {
            repositorioRegistroAusenciaAluno.Setup(a => a.ObterRegistrosAusenciaPorAula(It.IsAny<long>()))
                .Returns(new List<RegistroAusenciaAluno>()
                {
                    new RegistroAusenciaAluno("123", 1),
                    new RegistroAusenciaAluno("321", 1)
                });

            await servicoFrequencia.AtualizarQuantidadeFrequencia(0, 1, 3);

            // Deve gerar ausencia das aulas 2 e 3 para os dois alunos com ausencia na primeira aula
            repositorioRegistroAusenciaAluno.Verify(c => c.Salvar(It.IsAny<RegistroAusenciaAluno>()), Times.Exactly(4));
        }

        [Fact]
        public async Task Deve_Atualizar_Quantidade_Para_Menos()
        {
            repositorioRegistroAusenciaAluno.Setup(a => a.ObterRegistrosAusenciaPorAula(It.IsAny<long>()))
                .Returns(new List<RegistroAusenciaAluno>()
                {
                    new RegistroAusenciaAluno("123", 1),
                    new RegistroAusenciaAluno("321", 3),
                    new RegistroAusenciaAluno("456", 2)
                });

            await servicoFrequencia.AtualizarQuantidadeFrequencia(0, 3, 1);

            // Deve remover os registros de ausencia das aulas 2 e 3
            repositorioRegistroAusenciaAluno.Verify(c => c.Remover(It.IsAny<RegistroAusenciaAluno>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Deve_Registrar_Frequencia()
        {
            await servicoFrequencia.Registrar(1, new List<RegistroAusenciaAluno> {
                new RegistroAusenciaAluno("123",1),
                new RegistroAusenciaAluno("456",1)
            });

            repositorioAula.Verify(c => c.ObterPorId(It.IsAny<long>()), Times.Once);
            servicoUsuario.Verify(c => c.ObterUsuarioLogado(), Times.Once);
            servicoEOL.Verify(c => c.ObterAlunosPorTurma(It.IsAny<string>()), Times.Once);
            repositorioFrequencia.Verify(c => c.ObterRegistroFrequenciaPorAulaId(It.IsAny<long>()), Times.Once);
            repositorioFrequencia.Verify(c => c.Salvar(It.IsAny<RegistroFrequencia>()), Times.Once);
            repositorioRegistroAusenciaAluno.Verify(c => c.Salvar(It.IsAny<RegistroAusenciaAluno>()), Times.Exactly(2));
            unitOfWork.Verify(c => c.IniciarTransacao(), Times.Once);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Once);
        }

        private void Setup()
        {
            repositorioTurma.Setup(c => c.ObterPorCodigo(It.IsAny<string>()))
                .Returns(new Turma()
                {
                    AnoLetivo = 2019,
                    Ano = "9",
                    ModalidadeCodigo = Modalidade.Medio
                });
            repositorioAula.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(new Aula()
                {
                    ProfessorRf = "1",
                    Quantidade = 2
                });
            servicoUsuario.Setup(c => c.ObterUsuarioLogado())
                .Returns(Task.FromResult(new Usuario
                {
                    Id = 1
                }));

            var alunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno="123",
                    CodigoSituacaoMatricula=SituacaoMatriculaAluno.Ativo
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno="456",
                    CodigoSituacaoMatricula=SituacaoMatriculaAluno.Ativo
                }
            };

            servicoEOL.Setup(c => c.ObterAlunosPorTurma(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<AlunoPorTurmaResposta>>(alunos));
        }
    }
}