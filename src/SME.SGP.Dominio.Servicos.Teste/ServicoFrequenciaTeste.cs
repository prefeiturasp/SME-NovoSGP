using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoFrequenciaTeste
    {
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioFrequencia> repositorioFrequencia;
        private readonly Mock<IRepositorioRegistroAusenciaAluno> repositorioRegistroAusenciaAluno;
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

            servicoFrequencia = new ServicoFrequencia(repositorioFrequencia.Object,
                                                      repositorioRegistroAusenciaAluno.Object,
                                                      repositorioAula.Object,
                                                      servicoUsuario.Object,
                                                      unitOfWork.Object,
                                                      servicoEOL.Object);
        }

        [Fact]
        public async Task DeveRegistrarFrequencia()
        {
            repositorioAula.Setup(c => c.ObterPorId(It.IsAny<long>()))
                .Returns(new Aula()
                {
                    ProfessorRf = "1"
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
    }
}