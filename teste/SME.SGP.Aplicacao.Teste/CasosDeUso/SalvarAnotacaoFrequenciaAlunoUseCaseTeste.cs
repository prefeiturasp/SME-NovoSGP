﻿using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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
            // arrange
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
               .ReturnsAsync(new Aula() { DataAula = new DateTime(hoje.Year, hoje.Month, hoje.Day) });

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Usuario());

            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
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

            // act
            var auditoria = await salvarAnotacaoFrequenciaAlunoUseCase.Executar(dto);

            // assert
            mediator.Verify(x => x.Send(It.IsAny<SalvarAnotacaoFrequenciaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(auditoria);
            Assert.True(auditoria.Id == 1);
        }
    }
}
