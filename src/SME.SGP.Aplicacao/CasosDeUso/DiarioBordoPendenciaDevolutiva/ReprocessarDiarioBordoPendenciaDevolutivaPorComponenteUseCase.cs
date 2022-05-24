using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase : AbstractUseCase,IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUe;
        public ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUe) : base(mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();

            var existeDiarioSemDevolutiva = await mediator.Send(new ExistePendenciaDiarioBordoQuery(filtro.TurmaCodigo,filtro.ComponenteCodigo));
            
            if (existeDiarioSemDevolutiva)
            {
                var ue = ObterUe(filtro.UeCodigo);
                //Gerar a pendencia
                long pendenciaId = await GerarPedencia(ue.Id);
                //Inserir dados na Entidade PendenciaDevolutiva
                SalvarPendenciaDevolutiva(pendenciaId);
                //Enviar Notificacao

            }
            return true;
        }

        private async Task<long> GerarPedencia(long ueId)
        {
            long pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.Devolutiva, ueId, "Descrição", "Intrução"));
            await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, ObterCodigoPerfis()));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, ueId), Guid.NewGuid()));

            return pendenciaId;
        }
        private void SalvarPendenciaDevolutiva(long pendenciaId)
        {

        }
        private Ue ObterUe(string ueCodigo)
        {
            return repositorioUe.ObterPorCodigo(ueCodigo);
        }
        private List<PerfilUsuario> ObterCodigoPerfis()
         => new List<PerfilUsuario> { PerfilUsuario.CP };
    }
}
