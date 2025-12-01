using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class SalvarAnotacaoFrequenciaAlunoUseCaseTeste
    {
        private readonly SalvarAnotacaoFrequenciaAlunoUseCase salvarAnotacaoFrequenciaAlunoUseCase;
        private readonly Mock<IMediator> mediator;

        public SalvarAnotacaoFrequenciaAlunoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            salvarAnotacaoFrequenciaAlunoUseCase = new SalvarAnotacaoFrequenciaAlunoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Frequencia_Aluno()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = new DateTime(hoje.Year, hoje.Month, hoje.Day), TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("anotacao modificada");

            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aula_Nao_Encontrada()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((SME.SGP.Dominio.Aula)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Aula não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nao_Possui_Permissao_Na_Turma()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Você não pode fazer alterações ou inclusões nesta turma, componente e data.", exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync((AlunoPorTurmaResposta)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Aluno não encontrado.", exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Crianca_Nao_Encontrada_Para_Infantil()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = true,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.EducacaoInfantil,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync((AlunoPorTurmaResposta)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Criança não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Inativo_Na_Turma()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                  DataSituacao = hoje.AddDays(-1)
              });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Aluno não ativo na turma.", exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Crianca_Inativa_Na_Turma_Para_Infantil()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = true,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            
            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.EducacaoInfantil,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                  DataSituacao = hoje.AddDays(-1)
              });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Criança não ativa na turma.", exception.Message);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Para_Usuario_Professor_CJ()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            var usuarioProfessorCj = new Usuario() { Id = 1, Login = "123456", Nome = "Professor CJ" };
            usuarioProfessorCj.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_CJ } });
            usuarioProfessorCj.DefinirPerfilAtual(Perfis.PERFIL_CJ);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuarioProfessorCj);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Para_Usuario_Gestor_Escolar()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;
            var usuarioGestorEscolar = new Usuario() { Id = 1, Login = "123456", Nome = "Gestor Escolar" };
            usuarioGestorEscolar.DefinirPerfis(new List<PrioridadePerfil>() { new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_DIRETOR } });
            usuarioGestorEscolar.DefinirPerfilAtual(Perfis.PERFIL_DIRETOR);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuarioGestorEscolar);

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Para_Turma_Programa()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = "teste",
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Programa
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            var exception = await Assert.ThrowsAsync<NegocioException>(() => salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto));
            Assert.Equal("Você não pode fazer alterações ou inclusões nesta turma, componente e data.", exception.Message);
            
            mediator.Verify(x => x.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Salvar_Anotacao_Sem_Mover_Arquivos_Quando_Anotacao_Vazia()
        {
            var dto = new SalvarAnotacaoFrequenciaAlunoDto()
            {
                Anotacao = string.Empty,
                AulaId = 1,
                CodigoAluno = "123",
                ComponenteCurricularId = 139,
                EhInfantil = false,
                MotivoAusenciaId = 1
            };
            var hoje = DateTime.Today;

            mediator.Setup(a => a.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new SME.SGP.Dominio.Aula() { DataAula = hoje, TurmaId = "1" });

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma() { 
                    UeId = 1,
                    CodigoTurma = "1",
                    Historica = false,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Nome = "1",
                    TipoTurma = TipoTurma.Regular
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorTurmaAlunoCodigoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new AlunoPorTurmaResposta()
              {
                  CodigoAluno = "123",
                  CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                  DataSituacao = hoje
              });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuditoriaDto() { Id = 1 });

            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_ArgumentNullException_Quando_Mediator_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SalvarAnotacaoFrequenciaAlunoUseCase(null));
        }
    }
}
