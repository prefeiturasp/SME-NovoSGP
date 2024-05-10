using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDiarioBordoUseCase : IExcluirDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public ExcluirDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long diarioBordoId) =>
            (await mediator.Send(new ExcluirDiarioBordoCommand(diarioBordoId)));
        
    }
}
