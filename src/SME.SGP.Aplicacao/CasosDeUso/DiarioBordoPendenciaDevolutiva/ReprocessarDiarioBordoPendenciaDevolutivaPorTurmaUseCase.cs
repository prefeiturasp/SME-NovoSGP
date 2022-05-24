using SME.SGP.Aplicacao.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase
    {
        public ReprocessarDiarioBordoPendenciaDevolutivaPorTurmaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
            var turmas = await mediator.Send(new ObterCodigosTurmasPorAnoModalidadeUeQuery(filtro.AnoLetivo, Modalidade.EducacaoInfantil,filtro.UeCodigo));
            foreach (var turmaCodigo in turmas)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma,
                    new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo:filtro.AnoLetivo,dreCodigo:filtro.DreCodigo,ueCodigo:filtro.UeCodigo,turmaCodigo: turmaCodigo),Guid.NewGuid(),null));
            }

            return true;
        }
    }
}
