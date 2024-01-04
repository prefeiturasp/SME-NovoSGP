using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class FechamentosTurmaDisciplinaDiarioUseCase : IFechamentosTurmaDisciplinaDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioFechamentosTurmaDisciplinaDiario repositorioFechamentosTurmaDisciplina;

        public FechamentosTurmaDisciplinaDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioFechamentosTurmaDisciplinaDiario repositorioFechamentosTurmaDisciplina)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioFechamentosTurmaDisciplina = repositorioFechamentosTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentosTurmaDisciplina));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();
            var quantidadeRegistrosBimestrais = await repositorioSGP.ObterQuantidadeFechamentosTurmaDisciplinaDia(parametro.Data);

            foreach (var qdadePorBimestre in quantidadeRegistrosBimestrais)
                await repositorioFechamentosTurmaDisciplina.InserirAsync(new Entidade.FechamentosTurmaDisciplinaDiario(parametro.Data, 
                                                                                                                       qdadePorBimestre.Quantidade, 
                                                                                                                       qdadePorBimestre.Bimestre));

            return true;
        }
    }
}
