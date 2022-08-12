using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaDiarioBordoQueryHandler : IRequestHandler<ExistePendenciaDiarioBordoQuery, bool>
    {
        private readonly IRepositorioDiarioBordo repositorio;
        private readonly IMediator mediator;
        public ExistePendenciaDiarioBordoQueryHandler(IRepositorioDiarioBordo repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ExistePendenciaDiarioBordoQuery request, CancellationToken cancellationToken)
        {
            var totalDiasPermitidos = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PeriodoDeDiasDevolutiva, DateTime.Now.Year), cancellationToken);

            var consulta = await repositorio.DiarioBordoSemDevolutiva(request.TurmaId, request.ComponenteCodigo);

            if (consulta?.Count() > 0)
            {
                var dataAtual = DateTime.Now;
                int totalDeDias = 0;
                foreach (var datas in consulta)
                {
                    if (datas.PeriodoInicio <= dataAtual && datas.PeriodoFim <= dataAtual)
                        totalDeDias += (int)datas.PeriodoFim.Subtract(datas.PeriodoInicio).TotalDays;
                    else if (datas.PeriodoInicio <= dataAtual && datas.PeriodoFim > dataAtual)
                        totalDeDias += (int)dataAtual.Subtract(datas.PeriodoInicio).TotalDays;
                }
                return totalDeDias > Convert.ToInt32(totalDiasPermitidos.Valor);
            }
            return false;
        }
    }
}
