using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake
{
    public class PublicarFilaSgpCommandFakeCriarAulasInfantilERegenciaAutomaticamente : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly ICriarAulasInfantilERegenciaUseCase criarAulasInfantilERegenciaUseCase;
        private readonly ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase sincronizarUeTurmaAulaRegenciaAutomaticaUseCase;
        private readonly ICarregarUesTurmasRegenciaAulaAutomaticaUseCase carregarUesTurmasRegenciaAulaAutomaticaUseCase;
        private readonly ICriarAulasInfantilAutomaticamenteUseCase criarAulasInfantilAutomaticamenteUseCase;

        public PublicarFilaSgpCommandFakeCriarAulasInfantilERegenciaAutomaticamente(ICriarAulasInfantilERegenciaUseCase criarAulasInfantilERegenciaUseCase,
                                                                                    ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase sincronizarUeTurmaAulaRegenciaAutomaticaUseCase,
                                                                                    ICarregarUesTurmasRegenciaAulaAutomaticaUseCase carregarUesTurmasRegenciaAulaAutomaticaUseCase,
                                                                                    ICriarAulasInfantilAutomaticamenteUseCase criarAulasInfantilAutomaticamenteUseCase)
        {
            this.criarAulasInfantilERegenciaUseCase = criarAulasInfantilERegenciaUseCase ?? throw new ArgumentNullException(nameof(criarAulasInfantilERegenciaUseCase));
            this.sincronizarUeTurmaAulaRegenciaAutomaticaUseCase = sincronizarUeTurmaAulaRegenciaAutomaticaUseCase ?? throw new ArgumentNullException(nameof(sincronizarUeTurmaAulaRegenciaAutomaticaUseCase));
            this.carregarUesTurmasRegenciaAulaAutomaticaUseCase = carregarUesTurmasRegenciaAulaAutomaticaUseCase ?? throw new ArgumentNullException(nameof(carregarUesTurmasRegenciaAulaAutomaticaUseCase));
            this.criarAulasInfantilAutomaticamenteUseCase = criarAulasInfantilAutomaticamenteUseCase;
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(request.Filtros));

            switch (request.Rota)
            {
                case RotasRabbitSgpAula.SincronizarDadosUeTurmaRegenciaAutomaticamente:
                    return await sincronizarUeTurmaAulaRegenciaAutomaticaUseCase.Executar(mensagem);
                case RotasRabbitSgpAula.CarregarDadosUeTurmaRegenciaAutomaticamente:
                    return await carregarUesTurmasRegenciaAulaAutomaticaUseCase.Executar(mensagem);
                case RotasRabbitSgpAula.RotaSincronizarAulasInfantil:
                    return await criarAulasInfantilAutomaticamenteUseCase.Executar(mensagem);
                default:
                    return await criarAulasInfantilERegenciaUseCase.Executar(mensagem);
            }
        }
    }
}
