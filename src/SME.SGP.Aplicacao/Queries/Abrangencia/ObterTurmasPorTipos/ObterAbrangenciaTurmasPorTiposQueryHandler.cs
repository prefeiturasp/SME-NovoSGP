using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasPorTiposQueryHandler : IRequestHandler<ObterAbrangenciaTurmasPorTiposQuery, IEnumerable<AbrangenciaTurmaRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private IMediator mediator;

        public ObterAbrangenciaTurmasPorTiposQueryHandler(IRepositorioAbrangencia repositorioAbrangencia, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> Handle(ObterAbrangenciaTurmasPorTiposQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await repositorioAbrangencia.ObterTurmasPorTipos(request.CodigoUe, request.Login, request.Perfil, request.Modalidade, request.Tipos.Any() ? request.Tipos : null, request.Periodo, request.ConsideraHistorico, request.AnoLetivo, request.AnosInfantilDesconsiderar);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao Obter Turmas Por Tipos.", LogNivel.Critico, LogContexto.Aula, ex.Message, innerException: ex.InnerException.ToString(), rastreamento: ex.StackTrace));
                throw;
            }
        }
    }
}