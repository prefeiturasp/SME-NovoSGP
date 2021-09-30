using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase : AbstractUseCase, IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase
    {
        public ExecutarSincronizacaoDresConsolidacaoMatriculasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var dre = mensagem.ObterObjetoMensagem<FiltroConsolidacaoMatriculaDreDto>();
            var ues = await mediator.Send(new ObterUesCodigosPorDreQuery(dre.Id));
            foreach (var ueCodigo in ues)
            {
                var ueDto = new FiltroConsolidacaoMatriculaUeDto(ueCodigo, dre.AnosAnterioresParaConsolidar);
                try
                {
                    var publicarTratamentoCiclo = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidacaoMatriculasTurmasCarregar, ueDto, Guid.NewGuid(), null));
                    if (!publicarTratamentoCiclo)
                    {
                        var mensagemLog = $"Não foi possível inserir a dre : {publicarTratamentoCiclo} na fila de sync.";
                        await mediator.Send(new SalvarLogViaRabbitCommand(mensagemLog, LogNivel.Negocio, LogContexto.Frequencia, "Executar Sincronizacao Dres Consolidacao Matriculas UseCase"));                        
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Executar Sincronizacao Dres Consolidacao Matriculas UseCase", LogNivel.Critico, LogContexto.ConsolidacaoMatricula, ex.Message));                    
                }
            }
            return true;
        }
    }
}
