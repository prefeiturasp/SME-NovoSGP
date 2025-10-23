using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAula
{
    public class ObterPlanoAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPlanoAulaUseCase _useCase;

        public ObterPlanoAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPlanoAulaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Plano_Aula_Completo_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now.AddDays(-1), TipoCalendarioId = 99, Quantidade = 2, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };
            var atividadeAvaliativa = new AtividadeAvaliativa { Id = 3 };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value) && q.CodigoTurma == aulaDto.TurmaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.TurmaId == filtro.TurmaId && q.PeriodoEscolarId == periodoEscolar.Id && q.ComponenteCurricularId == disciplinaDto.CodigoComponenteCurricular), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date && q.DisciplinaId == aulaDto.DisciplinaId && q.TurmaId == aulaDto.TurmaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(atividadeAvaliativa);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(planoAula.Id, resultado.Id);
            Assert.True(resultado.PossuiPlanoAnual);
            Assert.Equal(atividadeAvaliativa.Id, resultado.IdAtividadeAvaliativa);
            Assert.True(resultado.PodeLancarNota);
            Assert.Equal(aulaDto.Quantidade, resultado.QtdAulas);
        }

        [Fact]
        public async Task Executar_Quando_Plano_Aula_Nao_Existe_Deve_Retornar_Dto_Vazio_Com_Dados_Da_Aula_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, null);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now.AddDays(1), TipoCalendarioId = 99, Quantidade = 2, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((PlanoAulaObjetivosAprendizagemDto)null);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.TurmaId == filtro.TurmaId && q.PeriodoEscolarId == periodoEscolar.Id && q.ComponenteCurricularId == long.Parse(aulaDto.DisciplinaId)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date && q.DisciplinaId == aulaDto.DisciplinaId && q.TurmaId == aulaDto.TurmaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AtividadeAvaliativa)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(0, resultado.Id);
            Assert.True(resultado.PossuiPlanoAnual);
            Assert.Null(resultado.IdAtividadeAvaliativa);
            Assert.False(resultado.PodeLancarNota);
            Assert.Equal(aulaDto.Quantidade, resultado.QtdAulas);
        }

        [Fact]
        public async Task Executar_Quando_Periodo_Escolar_Nulo_Deve_Lancar_Negocio_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99 };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((PeriodoEscolar)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Período escolar não localizado.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Planejamento_Anual_Inexistente_Deve_Lancar_Negocio_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99 };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == disciplinaDto.CodigoComponenteCurricular), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível carregar o plano de aula porque não há plano anual cadastrado", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Planejamento_Inexistente_Mas_Usuario_Cj_Nao_Deve_Lancar_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_CJ };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == disciplinaDto.CodigoComponenteCurricular), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AtividadeAvaliativa)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.False(resultado.PossuiPlanoAnual);
        }

        [Fact]
        public async Task Executar_Quando_Planejamento_Inexistente_Mas_Territorio_Saber_Nao_Deve_Lancar_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = true };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == disciplinaDto.CodigoComponenteCurricular), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AtividadeAvaliativa)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.False(resultado.PossuiPlanoAnual);
        }

        [Fact]
        public async Task Executar_Quando_Planejamento_Inexistente_Mas_Ano_Anterior_Nao_Deve_Lancar_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 10, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year - 1 } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == disciplinaDto.CodigoComponenteCurricular), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AtividadeAvaliativa)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.False(resultado.PossuiPlanoAnual);
        }

        [Fact]
        public async Task Executar_Quando_Componente_Id_Informado_Mas_Nao_Encontrado_Deve_Usar_Disciplina_Da_Aula_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99, UeId = "UE1" };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinasRetorno = new List<DisciplinaDto>();
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == long.Parse(aulaDto.DisciplinaId)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1L);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadeAvaliativaQuery>(
                             q => q.DataAvaliacao == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((AtividadeAvaliativa)null);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.True(resultado.PossuiPlanoAnual);
        }

        [Fact]
        public async Task Executar_Quando_Disciplina_Dto_Encontrado_Mas_Codigo_Zero_Deve_Lancar_Exception_()
        {
            var filtro = new FiltroObterPlanoAulaDto(1, 1, 10);
            var aulaDto = new Dominio.Aula { Id = 1, TurmaId = "T1", DisciplinaId = "123", DataAula = DateTime.Now, TipoCalendarioId = 99 };
            var usuario = new Usuario { PerfilAtual = Guid.NewGuid() };
            var planoAula = new PlanoAulaObjetivosAprendizagemDto { Id = 5, AulaId = 1, Descricao = "Teste" };
            var disciplinaDto = new DisciplinaDto { CodigoComponenteCurricular = 0, TerritorioSaber = false };
            var disciplinasRetorno = new List<DisciplinaDto> { disciplinaDto };
            var periodoEscolar = new PeriodoEscolar { Id = 2, TipoCalendario = new Dominio.TipoCalendario { AnoLetivo = DateTime.Now.Year } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaDto);
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulaEObjetivosAprendizagemQuery>(q => q.AulaId == filtro.AulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(planoAula);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(
                             q => q.Ids.Contains(filtro.ComponenteCurricularId.Value)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(disciplinasRetorno);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarPorCalendarioEDataQuery>(
                             q => q.TipoCalendarioId == aulaDto.TipoCalendarioId && q.DataParaVerificar == aulaDto.DataAula.Date), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(periodoEscolar);
            _mediatorMock.Setup(m => m.Send(It.Is<ExistePlanejamentoAnualParaTurmaPeriodoEComponenteQuery>(
                             q => q.ComponenteCurricularId == 0), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
        }
    }
}
