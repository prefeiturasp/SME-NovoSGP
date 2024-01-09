using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class PlanosAulaDiarioUseCase : IPlanosAulaDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioPlanosAulaDiario repositorioPlanosAula;

        public PlanosAulaDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioPlanosAulaDiario repositorioPlanosAula)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioPlanosAula = repositorioPlanosAula ?? throw new ArgumentNullException(nameof(repositorioPlanosAula));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();

            var quantidadeRegistros = await repositorioSGP.ObterQuantidadePlanosAulaDia(parametro.Data);
            if (quantidadeRegistros > 0)
                await repositorioPlanosAula.InserirAsync(new Entidade.PlanosAulaDiario(parametro.Data, quantidadeRegistros));
            return true;
        }
    }
}
