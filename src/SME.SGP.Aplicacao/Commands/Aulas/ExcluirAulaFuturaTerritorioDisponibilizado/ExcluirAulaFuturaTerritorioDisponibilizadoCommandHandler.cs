using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaFuturaTerritorioDisponibilizadoCommandHandler : IRequestHandler<ExcluirAulaFuturaTerritorioDisponibilizadoCommand, RetornoBaseDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioAula repositorioAula;
        
        public ExcluirAulaFuturaTerritorioDisponibilizadoCommandHandler(IMediator mediator,
                                              IRepositorioAula repositorioAula)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<RetornoBaseDto> Handle(ExcluirAulaFuturaTerritorioDisponibilizadoCommand request, CancellationToken cancellationToken)
        {
            var aula = await repositorioAula.ObterPorIdAsync(request.AulaId);
            aula.Excluido = true;
            await repositorioAula.SalvarAsync(aula);

            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add("Aula excluída com sucesso.");
            return retorno;
        }

    }
}
