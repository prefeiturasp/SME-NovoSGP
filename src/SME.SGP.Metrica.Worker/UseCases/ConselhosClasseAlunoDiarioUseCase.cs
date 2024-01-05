using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class ConselhosClasseAlunoDiarioUseCase : IConselhosClasseAlunoDiarioUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioConselhosClasseAlunoDiario repositorioConselhosClasseAluno;

        public ConselhosClasseAlunoDiarioUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioConselhosClasseAlunoDiario repositorioConselhosClasseAluno)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioConselhosClasseAluno = repositorioConselhosClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhosClasseAluno));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();

            var quantidadeRegistrosBimestrais = await repositorioSGP.ObterQuantidadeConselhosClasseAlunoDia(parametro.Data);
            foreach (var qdadePorBimestre in quantidadeRegistrosBimestrais)
                await repositorioConselhosClasseAluno.InserirAsync(new Entidade.ConselhosClasseAlunoDiario(parametro.Data, qdadePorBimestre.Quantidade, qdadePorBimestre.Bimestre));
            return true;
        }
    }
}
