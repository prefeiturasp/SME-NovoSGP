using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilERegenciaUseCase : ICriarAulasInfantilERegenciaUseCase
    {
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;

        public CriarAulasInfantilERegenciaUseCase(IMediator mediator, IRepositorioCache repositorioCache)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var jsonComando = await repositorioCache.ObterAsync(mensagemRabbit.Mensagem?.ToString());

            if (string.IsNullOrWhiteSpace(jsonComando))
                return false;

            await repositorioCache.RemoverAsync(mensagemRabbit.Mensagem.ToString());

            var comando = JsonConvert.DeserializeObject<CriarAulasInfantilERegenciaAutomaticamenteCommand>(jsonComando);

            if (comando.NaoEhNulo())
            {
                await mediator.Send(comando);
                return true;
            }

            return false;
        }
    }
}
