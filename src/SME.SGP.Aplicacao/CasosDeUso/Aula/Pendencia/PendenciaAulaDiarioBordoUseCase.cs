using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaDiarioBordoUseCase : AbstractUseCase, IPendenciaAulaDiarioBordoUseCase
    {
        public PendenciaAulaDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreUeDto>();
            var uesDre = filtro.UeId > 0 ?
                await CarregarUePorId(filtro.UeId) :
                await CarregarUesPorDreId(filtro.DreId);

            foreach (var ue in uesDre)
            {
                var turmasUe = await mediator.Send(new ObterTurmasInfantilPorUEQuery(DateTimeExtension.HorarioBrasilia().Year, ue));
                foreach (var turma in turmasUe)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordoTurma, turma.TurmaId));
            }

            return true;
        }

        private Task<IEnumerable<string>> CarregarUesPorDreId(long dreId)
            => mediator.Send(new ObterUesCodigosPorDreQuery(dreId));

        private async Task<IEnumerable<string>> CarregarUePorId(long ueId)
        {
            var ueCodigo = await mediator.Send(new ObterUeCodigoPorIdQuery(ueId));
            if (string.IsNullOrEmpty(ueCodigo))
                throw new NegocioException("Código da escola não localizado pelo identificador");

            return new List<string>() { ueCodigo };
        }
    }
}
