using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase : IAulasSemAtribuicaoSubstituicaoTurmaMensalUseCase
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private readonly IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas;

        public AulasSemAtribuicaoSubstituicaoTurmaMensalUseCase(IRepositorioSGPConsulta repositorioSGP, IRepositorioAulasSemAtribuicaoSubstituicaoMensal repositorioAulas)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
            this.repositorioAulas = repositorioAulas ?? throw new ArgumentNullException(nameof(repositorioAulas));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var turma = mensagem.ObterObjetoMensagem<FiltroIdDataDto>();
            var quantidadeRegistros = await repositorioSGP.ObterQuantidadeAulasCJMes(turma.Data);

            await repositorioAulas.InserirAsync(new Entidade.AulasSemAtribuicaoSubstituicaoMensal(turma.Data, quantidadeRegistros));
            
            return true;
        }
    }
}
