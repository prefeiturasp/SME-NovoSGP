using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class PlanosAEEMensalUseCase : IPlanosAEEMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioPlanosAEEMensal repositorioPlanosAEE;

        public PlanosAEEMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioPlanosAEEMensal repositorioPlanosAEE)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioPlanosAEE = repositorioPlanosAEE ?? throw new ArgumentNullException(nameof(repositorioPlanosAEE));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.ObterObjetoMensagem<FiltroDataDto>();
            var quantidadeRegistros = await repositorioSGP.ObterQuantidadePlanosAEEMes(parametro.Data);

            await repositorioPlanosAEE.InserirAsync(new Entidade.PlanosAEEMensal(parametro.Data, quantidadeRegistros));
            
            return true;
        }
    }
}
