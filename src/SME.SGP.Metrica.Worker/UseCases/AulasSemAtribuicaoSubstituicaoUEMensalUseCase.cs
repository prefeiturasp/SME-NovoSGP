using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Commands;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoUEMensalUseCase : IAulasSemAtribuicaoSubstituicaoUEMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IMediator mediator;

        public AulasSemAtribuicaoSubstituicaoUEMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IMediator mediator)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var ue = mensagem.ObterObjetoMensagem<FiltroIdDataDto>();
            var turmas = await repositorioSGP.ObterTurmasIdsPorUE(ue.Id, ue.Data.Year);
            foreach (var turma in turmas)
                await mediator.Send(new PublicarFilaCommand(Rotas.RotasRabbitMetrica.AulasSemAtribuicaoSubstituicaoTurmaMensais, new FiltroIdDataDto(turma, ue.Data)));
            return true;
        }
    }
}
