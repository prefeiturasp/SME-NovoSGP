using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelPlanoAEECommandHandler : IRequestHandler<RemoverResponsavelPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEE repositorioPlanoAee;
        private readonly IUnitOfWork unitOfWork;

        public RemoverResponsavelPlanoAEECommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE,IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAee = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(RemoverResponsavelPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAee = await mediator.Send(new ObterPlanoAEEComTurmaPorIdQuery(request.PlanoAeeId));

            if (planoAee.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.O_plano_aee_informado_nao_foi_encontrado);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAee.TurmaId));

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            planoAee.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.AtribuicaoPAAI;
            
            planoAee.ResponsavelPaaiId = null;

            var pendenciaPlanoAEE = await mediator.Send(new ObterUltimaPendenciaPlanoAEEQuery(planoAee.Id));
            
            unitOfWork.IniciarTransacao();
            
            try
            {
                await repositorioPlanoAee.SalvarAsync(planoAee);
               
                if (pendenciaPlanoAEE.NaoEhNulo())
                   await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoAee.Id));
                
                if (await ParametroGeracaoPendenciaAtivo())
                    await mediator.Send(new GerarPendenciaValidacaoPlanoAEECommand(planoAee.Id, PerfilUsuario.CEFAI));
                
                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return planoAee.Id != 0;
        }
        
        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
