﻿using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class EncerrarEncaminhamentoAEEAutomaticoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly EncerrarEncaminhamentoAEEAutomaticoUseCase _encerrarEncaminhamentoAEEAutomaticoUseCase;

        public EncerrarEncaminhamentoAEEAutomaticoUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _encerrarEncaminhamentoAEEAutomaticoUseCase = new EncerrarEncaminhamentoAEEAutomaticoUseCase(_mediator.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task Deve_Atualizar_Encaminhamento_AEE_Para_Encerrado_Automaticamente_E_Excluir_Pendencias()
        {
            //-> Assert
            _mediator.Setup(c => c.Send(It.IsAny<AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand>(), new CancellationToken()))
                .ReturnsAsync(new EncaminhamentoAEE
                {
                    Id = 1983,
                    TurmaId = 869773,
                    AlunoCodigo = "4824410",
                    Excluido = false,
                    CriadoEm = DateTime.Now,
                    CriadoPor = "PAULO ROBERTO ANTUNES OLIVEIRA MANOEL",
                    AlteradoEm = DateTime.Now,
                    AlteradoPor = "PAULO ROBERTO ANTUNES OLIVEIRA MANOEL",
                    CriadoRF = "8425825",
                    AlteradoRF = "8425825",
                    Situacao = SituacaoAEE.EncerradoAutomaticamente,
                    AlunoNome = "NICOLAS DOS SANTOS ALMEIDA SILVA"
                });

            _mediator.Setup(c => c.Send(It.IsAny<ObterPendenciasDoEncaminhamentoAEEPorIdQuery>(), new CancellationToken()))
                .ReturnsAsync(new List<PendenciaEncaminhamentoAEE>
                { 
                    new PendenciaEncaminhamentoAEE
                    {
                        Id = 1,
                        EncaminhamentoAEEId = 1983,
                        PendenciaId = 1,
                        CriadoEm = DateTime.UtcNow.AddHours(-1),
                        CriadoPor = "ELIANE DA SILVA ROSA",
                        CriadoRF = "7956029"
                    }
                });

            //-> Act
            var retorno = await _encerrarEncaminhamentoAEEAutomaticoUseCase.Executar(CriarMensagemRabbitEncerrarEncaminhamentoAEEAutomatico());

            //-> Assert
            _mediator.Verify(x => x.Send(It.IsAny<AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<ObterPendenciasDoEncaminhamentoAEEPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retorno);
        }

        private MensagemRabbit CriarMensagemRabbitEncerrarEncaminhamentoAEEAutomatico()
        {
            var filtro = new FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto(1983);
            var json = JsonConvert.SerializeObject(filtro);

            return new MensagemRabbit(json)
            {
                Action = "sgp/encaminhamento/aee/encerrar/automatico/encerrar",
                UsuarioLogadoNomeCompleto = "MILENA PEDROSO RUELLA MARTINS",
                UsuarioLogadoRF = "7707533"
            };
        }
    }
}
