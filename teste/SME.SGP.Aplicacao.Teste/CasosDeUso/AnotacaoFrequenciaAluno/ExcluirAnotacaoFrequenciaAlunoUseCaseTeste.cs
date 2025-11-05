using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AnotacaoFrequenciaAluno
{
    public class ExcluirAnotacaoFrequenciaAlunoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirAnotacaoFrequenciaAlunoUseCase _useCase;

        public ExcluirAnotacaoFrequenciaAlunoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirAnotacaoFrequenciaAlunoUseCase(_mediatorMock.Object);
        }

        private Dominio.AnotacaoFrequenciaAluno ObterAnotacaoMock(long aulaId, string anotacao = "Teste", long id = 1)
        {
            return new Dominio.AnotacaoFrequenciaAluno(aulaId, anotacao, "123") { Id = id };
        }

        private Dominio.Aula ObterAulaMock(string disciplinaId = "100", string turmaId = "T1", DateTime? dataAula = null)
        {
            return new Dominio.Aula { DisciplinaId = disciplinaId, TurmaId = turmaId, DataAula = dataAula ?? DateTime.Now.Date };
        }

        private Usuario ObterUsuarioMock(bool ehProfessorCj = false, bool ehGestorEscolar = false)
        {
            var usuario = new Usuario();
            
            var perfis = new List<PrioridadePerfil>();
            
            if (ehProfessorCj)
            {
                perfis.Add(new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_CJ, Tipo = TipoPerfil.UE });
                usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_CJ);
            }
            else if (ehGestorEscolar)
            {
                perfis.Add(new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_CP, Tipo = TipoPerfil.UE });
                usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_CP);
            }
            else
            {
                perfis.Add(new PrioridadePerfil { CodigoPerfil = Dominio.Perfis.PERFIL_PROFESSOR, Tipo = TipoPerfil.UE });
                usuario.DefinirPerfilAtual(Dominio.Perfis.PERFIL_PROFESSOR);
            }
            
            usuario.DefinirPerfis(perfis);
            return usuario;
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Nao_Localizada_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 1;
            Dominio.AnotacaoFrequenciaAluno anotacaoNula = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacaoNula);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Eh_ProfessorCj_Deve_Excluir_E_DeletarArquivo()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId, "arquivo.pdf");
            var aula = ObterAulaMock();
            var usuario = ObterUsuarioMock(ehProfessorCj: true);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacaoFrequenciaAlunoCommand>(c => c.AnotacaoFrequenciaAluno == anotacao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<DeletarArquivoDeRegistroExcluidoCommand>(c => c.ArquivoAtual == "arquivo.pdf"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Eh_GestorEscolar_Deve_Excluir_E_DeletarArquivo()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId, "arquivo.pdf");
            var aula = ObterAulaMock();
            var usuario = ObterUsuarioMock(ehGestorEscolar: true);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacaoFrequenciaAlunoCommand>(c => c.AnotacaoFrequenciaAluno == anotacao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<DeletarArquivoDeRegistroExcluidoCommand>(c => c.ArquivoAtual == "arquivo.pdf"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Nao_Tem_Permissao_Deve_Lancar_NegocioException()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId);
            var aula = ObterAulaMock("100", "T1", new DateTime(2025, 1, 1));
            var usuario = ObterUsuarioMock();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(anotacaoId));
            Assert.Equal("Você não pode fazer alterações ou inclusões nesta turma, componente e data.", ex.Message);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(q => q.ComponenteCurricularId == 100 && q.CodigoTurma == "T1" && q.Data == aula.DataAula && q.Usuario == usuario), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Tem_Permissao_E_Nao_Tem_Anotacao_De_Arquivo_Deve_Excluir_Sem_DeletarArquivo()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId, ""); 
            var aula = ObterAulaMock("100", "T1");
            var usuario = ObterUsuarioMock();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacaoFrequenciaAlunoCommand>(c => c.AnotacaoFrequenciaAluno == anotacao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Usuario_Tem_Permissao_E_Tem_Anotacao_De_Arquivo_Deve_Excluir_E_DeletarArquivo()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId, "caminho/arquivo.doc");
            var aula = ObterAulaMock("100", "T1");
            var usuario = ObterUsuarioMock();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacaoFrequenciaAlunoCommand>(c => c.AnotacaoFrequenciaAluno == anotacao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<DeletarArquivoDeRegistroExcluidoCommand>(c => c.ArquivoAtual == "caminho/arquivo.doc"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Anotacao_Tem_Anotacao_So_Com_Espacos_Deve_Excluir_Sem_DeletarArquivo()
        {
            long anotacaoId = 1;
            long aulaId = 10;
            var anotacao = ObterAnotacaoMock(aulaId, "   ");
            var aula = ObterAulaMock("100", "T1");
            var usuario = ObterUsuarioMock();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAnotacaoFrequenciaAlunoPorIdQuery>(q => q.Id == anotacaoId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacao);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aula);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(anotacaoId);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacaoFrequenciaAlunoCommand>(c => c.AnotacaoFrequenciaAluno == anotacao), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeletarArquivoDeRegistroExcluidoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
