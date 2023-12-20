using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class RegistrosFrequenciaEfetuadosUseCase : IRegistrosFrequenciaEfetuadosUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioRegistrosFrequencia repositorioRegistrosFrequencia;

        public RegistrosFrequenciaEfetuadosUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioRegistrosFrequencia repositorioRegistrosFrequencia)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioRegistrosFrequencia = repositorioRegistrosFrequencia ?? throw new ArgumentNullException(nameof(repositorioRegistrosFrequencia));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.ObterObjetoMensagem<FiltroDataDto>();
            var quantidadeAcessos = await repositorioSGP.ObterQuantidadeRegistrosFrequenciaEfetuadosDia(parametro.Data);

            await repositorioRegistrosFrequencia.InserirAsync(new Entidade.RegistrosFrequenciaDiario(parametro.Data, quantidadeAcessos));

            return true;
        }
    }
}
