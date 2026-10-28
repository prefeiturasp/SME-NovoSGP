using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class SalvarPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();
        private readonly SalvarPlanoAEEUseCase useCase;

        public SalvarPlanoAEEUseCaseTeste()
        {
            useCase = new SalvarPlanoAEEUseCase(mediator.Object);
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { Id = 1, TipoTurma = TipoTurma.Regular });
            mediator.Setup(m => m.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlunoPorTurmaResposta { CodigoAluno = "123", NomeAluno = "Aluno Teste" });
            mediator.Setup(m => m.Send(It.IsAny<SalvarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetornoPlanoAEEDto(1, 10));
            mediator.Setup(m => m.Send(It.IsAny<ObterPlanoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.PlanoAEE { Id = 1 });
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(1, false)]
        public async Task Deve_Enviar_Srm_Atualizada_Para_Persistencia_Da_Primeira_Ou_Nova_Versao(long planoId, bool possuiMatricula)
        {
            var plano = CriarPlano(planoId);
            var dadosSrm = new List<SrmPaeeColaborativoSgpDto>();
            if (possuiMatricula)
                dadosSrm.Add(new SrmPaeeColaborativoSgpDto
                {
                    DreUe = "DRE - EMEF Atual",
                    TurmaTurno = "SG - Tarde",
                    ComponenteCurricular = "SRM"
                });

            mediator.Setup(m => m.Send(It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosSrm);
            string respostaEnviada = null;
            mediator.Setup(m => m.Send(It.IsAny<SalvarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<RetornoPlanoAEEDto>, CancellationToken>((request, _) =>
                {
                    var command = (SalvarPlanoAeeCommand)request;
                    respostaEnviada = command.PlanoAEEDto.Questoes[0].Resposta;
                    Assert.Equal(planoId, command.PlanoAEEDto.Id);
                    Assert.Equal(20, command.PlanoAEEDto.Questoes[0].QuestaoId);
                    Assert.Equal("Texto preenchido pelo usuário", command.PlanoAEEDto.Questoes[1].Resposta);
                })
                .ReturnsAsync(new RetornoPlanoAEEDto(1, 10));

            var retorno = await useCase.Executar(plano);

            Assert.Equal(JsonConvert.SerializeObject(dadosSrm), respostaEnviada);
            Assert.Equal(10, retorno.PlanoVersaoId);
            mediator.Verify(m => m.Send(It.Is<ObterDadosSrmPaeeColaborativoEolQuery>(q => q.CodigoAluno == 123),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Nao_Deve_Persistir_Plano_Quando_Consulta_Srm_Falhar(long planoId)
        {
            var plano = CriarPlano(planoId);
            mediator.Setup(m => m.Send(It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NegocioException("Falha ao consultar SRM no EOL"));

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(plano));

            Assert.Equal("matrícula anterior", plano.Questoes[0].Resposta);
            mediator.Verify(m => m.Send(It.IsAny<SalvarPlanoAeeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<SalvarPlanoAEETurmaAlunoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static PlanoAEEPersistenciaDto CriarPlano(long id)
            => new PlanoAEEPersistenciaDto
            {
                Id = id,
                AlunoCodigo = "123",
                TurmaCodigo = "1",
                Questoes = new List<PlanoAEEQuestaoDto>
                {
                    new PlanoAEEQuestaoDto { QuestaoId = 20, TipoQuestao = TipoQuestao.InformacoesSrm, Resposta = "matrícula anterior" },
                    new PlanoAEEQuestaoDto { QuestaoId = 21, TipoQuestao = TipoQuestao.Texto, Resposta = "Texto preenchido pelo usuário" }
                }
            };
    }
}
