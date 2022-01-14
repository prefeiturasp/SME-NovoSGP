using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDevolutivaCommandHandler : IRequestHandler<InserirDevolutivaCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDevolutiva repositorioDevolutiva;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public InserirDevolutivaCommandHandler(IMediator mediator,
                                                IRepositorioDevolutiva repositorioDevolutiva, IRepositorioTurmaConsulta repositorioTurmaConsulta)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }

        public async Task<AuditoriaDto> Handle(InserirDevolutivaCommand request, CancellationToken cancellationToken)
        {
            Devolutiva devolutiva = MapearParaEntidade(request);

            await repositorioDevolutiva.SalvarAsync(devolutiva);

            var turma = await repositorioTurmaConsulta.ObterTurmaComUeEDrePorId(request.TurmaId);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNovaNotificacaoDevolutiva,
                       new SalvarNotificacaoDevolutivaDto(turma, usuarioLogado, devolutiva.Id), Guid.NewGuid(), null));

            return (AuditoriaDto)devolutiva;
        }

        private Devolutiva MapearParaEntidade(InserirDevolutivaCommand request)
            => new Devolutiva()
            {
                CodigoComponenteCurricular = request.CodigoComponenteCurricular,
                PeriodoInicio = request.PeriodoInicio,
                PeriodoFim = request.PeriodoFim,
                Descricao = request.Descricao
            };
    }
}
