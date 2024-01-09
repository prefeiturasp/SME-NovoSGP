using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AcessosDiarioSGPUseCase : IAcessosDiarioSGPUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAcessos repositorioAcessos;

        public AcessosDiarioSGPUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioAcessos repositorioAcessos)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAcessos = repositorioAcessos ?? throw new ArgumentNullException(nameof(repositorioAcessos));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();
            
            var quantidadeAcessos = await repositorioSGP.ObterQuantidadeAcessosDia(parametro.Data);
            await repositorioAcessos.InserirAsync(new Entidade.AcessosDiario(parametro.Data, quantidadeAcessos));
            return true;
        }
    }
}
