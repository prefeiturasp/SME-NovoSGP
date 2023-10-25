using MediatR;
using Newtonsoft.Json;
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

        public InserirDevolutivaCommandHandler(IMediator mediator, IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));            
        }

        public async Task<AuditoriaDto> Handle(InserirDevolutivaCommand request, CancellationToken cancellationToken)
        {
            var devolutiva = MapearParaEntidade(request);

            await repositorioDevolutiva.SalvarAsync(devolutiva);            

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (usuarioLogado.EhNulo())
                throw new NegocioException("Não foi possível obter o usuário logado");

            var filtro = new SalvarNotificacaoDevolutivaDto(request.TurmaId, usuarioLogado.Nome, usuarioLogado.CodigoRf, devolutiva.Id);           

            var codigoCorrelacao = Guid.NewGuid();

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNovaNotificacaoDevolutiva, JsonConvert.SerializeObject(filtro), codigoCorrelacao, null));

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
