using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentosNotaDiarioUseCase : IFechamentosNotaDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentosNotaDiario repositorioFechamentosNota;

        public FechamentosNotaDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioFechamentosNotaDiario repositorioFechamentosNota)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentosNota = repositorioFechamentosNota ?? throw new ArgumentNullException(nameof(repositorioFechamentosNota));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();
            var quantidadeRegistrosBimestrais = await repositorioSGP.ObterQuantidadeFechamentosNotaDia(parametro.Data);

            foreach (var qdadePorBimestre in quantidadeRegistrosBimestrais)
                await repositorioFechamentosNota.InserirAsync(new Entidade.FechamentosNotaDiario(parametro.Data, qdadePorBimestre.Quantidade, qdadePorBimestre.Bimestre));

            return true;
        }
    }
}
