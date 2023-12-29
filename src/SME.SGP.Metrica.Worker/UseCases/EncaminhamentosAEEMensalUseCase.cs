using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class EncaminhamentosAEEMensalUseCase : IEncaminhamentosAEEMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioEncaminhamentosAEEMensal repositorioEncaminhamentosAEE;

        public EncaminhamentosAEEMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioEncaminhamentosAEEMensal repositorioEncaminhamentosAEE)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioEncaminhamentosAEE = repositorioEncaminhamentosAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentosAEE));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTimeExtension.HorarioBrasilia().Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();
            var quantidadeRegistros = await repositorioSGP.ObterQuantidadeEncaminhamentosAEEMes(parametro.Data);

            await repositorioEncaminhamentosAEE.InserirAsync(new Entidade.EncaminhamentosAEEMensal(parametro.Data, quantidadeRegistros));
            
            return true;
        }
    }
}
