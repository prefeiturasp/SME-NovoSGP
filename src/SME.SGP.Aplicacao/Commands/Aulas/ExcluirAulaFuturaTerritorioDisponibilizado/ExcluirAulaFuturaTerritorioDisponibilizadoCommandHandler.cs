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
            if (aula.WorkflowAprovacaoId.HasValue)
                await PulicaFilaSgp(RotasRabbitSgp.WorkflowAprovacaoExcluir, aula.WorkflowAprovacaoId.Value, null);

            var filas = new [] { RotasRabbitSgpAula.PlanoAulaDaAulaExcluir };
            await PulicaFilaSgp(filas, aula.Id, null);

            aula.Excluido = true;
            await repositorioAula.SalvarAsync(aula);

            var retorno = new RetornoBaseDto();
            retorno.Mensagens.Add("Aula excluída com sucesso.");
            return retorno;
        }

        private async Task PulicaFilaSgp(string fila, long id, Usuario usuario)
        {
            await mediator.Send(new PublicarFilaSgpCommand(fila, new FiltroIdDto(id), Guid.NewGuid(), usuario));
        }

        private async Task PulicaFilaSgp(string[] filas, long id, Usuario usuario)
        {
            var commands = new List<PublicarFilaSgpCommand>();

            filas.ToList()
                .ForEach(f => commands.Add(new PublicarFilaSgpCommand(f, new FiltroIdDto(id), Guid.NewGuid(), usuario)));

            await mediator.Send(new PublicarFilaEmLoteSgpCommand(commands));
        }
    }
}
