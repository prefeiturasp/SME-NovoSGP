using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class RegistrosFrequenciaDiarioUseCase : IRegistrosFrequenciaDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioRegistrosFrequenciaDiario repositorioRegistrosFrequencia;

        public RegistrosFrequenciaDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioRegistrosFrequenciaDiario repositorioRegistrosFrequencia)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioRegistrosFrequencia = repositorioRegistrosFrequencia ?? throw new ArgumentNullException(nameof(repositorioRegistrosFrequencia));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo() 
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1)) 
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();

            var quantidadeRegistros = await repositorioSGP.ObterQuantidadeRegistrosFrequenciaDia(parametro.Data);
            if (quantidadeRegistros > 0)
                await repositorioRegistrosFrequencia.InserirAsync(new Entidade.RegistrosFrequenciaDiario(parametro.Data, quantidadeRegistros));
            return true;
        }
    }
}
