using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUeConsulta;
        public ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUeConsulta) : base(mediator)
        {
            this.repositorioUeConsulta = repositorioUeConsulta ?? throw new ArgumentNullException(nameof(repositorioUeConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
                var ues =  (await repositorioUeConsulta.ObterPorDre(filtro.DreId)).ToList();

                foreach (var tipoEscola in ues.GroupBy(ue => ue.TipoEscola))
                {
                    var ignorarGeracaoPendencia = await mediator.Send(new ObterTipoUeIgnoraGeracaoPendenciasQuery(tipoEscola.Key, ""));
                    if (!ignorarGeracaoPendencia)
                        foreach (var ue in ues.Where(ue => ue.TipoEscola == tipoEscola.Key))
                        {
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma,
                            new FiltroDiarioBordoPendenciaDevolutivaDto(dreId: filtro.DreId, ueCodigo: ue.CodigoUe, anoLetivo: filtro.AnoLetivo, ueId: ue.Id),
                            Guid.NewGuid(), null));
                        }
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a verificação de pendencias de devolutivas por UE", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                return false;
            }
        }
    }
}
