using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class InserirAulaUnicaCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly InserirAulaUnicaCommandHandler inserirAulaUnicaCommandHandler;

        public InserirAulaUnicaCommandHandlerTeste(Mock<IMediator> mediator, Mock<IRepositorioAula> repositorioAula, InserirAulaUnicaCommandHandler inserirAulaUnicaCommandHandler)
        {
            this.mediator = new Mock<IMediator>(); 
            this.repositorioAula = new Mock<IRepositorioAula>(); 
            this.inserirAulaUnicaCommandHandler = new InserirAulaUnicaCommandHandler(repositorioAula.Object ,mediator.Object); ;
        }
    }
}
