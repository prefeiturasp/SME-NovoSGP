using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoMensalUseCase : IAulasSemAtribuicaoSubstituicaoMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;

        public AulasSemAtribuicaoSubstituicaoMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametro = mensagem.EhNulo() || mensagem.Mensagem.EhNulo()
                            ? new FiltroDataDto(DateTime.Now.Date.AddDays(-1))
                            : mensagem.ObterObjetoMensagem<FiltroDataDto>();
            return false;
            var quantidadeRegistros = await repositorioSGP.ObterQuantidadeAulasCJMes(parametro.Data);

            await repositorioAulas.InserirAsync(new Entidade.AulasSemAtribuicaoSubstituicaoMensal(parametro.Data, quantidadeRegistros));
            
            return true;
        }
    }
}
