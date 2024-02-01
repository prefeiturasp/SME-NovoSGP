using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaFechamentoNotaDuplicadoUseCase : ILimpezaFechamentoNotaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaFechamentoNotaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var fechamentoNotaDuplicado = mensagem.ObterObjetoMensagem<FechamentoNotaDuplicado>();

            await repositorioSGP.AtualizarHistoricoFechamentoNotaDuplicados(fechamentoNotaDuplicado.FechamentoAlunoId,
                                                                            fechamentoNotaDuplicado.DisciplinaId,
                                                                            fechamentoNotaDuplicado.UltimoId);

            await repositorioSGP.AtualizarWfAprovacaoFechamentoNotaDuplicados(fechamentoNotaDuplicado.FechamentoAlunoId,
                                                                              fechamentoNotaDuplicado.DisciplinaId,
                                                                              fechamentoNotaDuplicado.UltimoId);

            await repositorioSGP.ExcluirFechamentoNotaDuplicados(fechamentoNotaDuplicado.FechamentoAlunoId,
                                                                 fechamentoNotaDuplicado.DisciplinaId,
                                                                 fechamentoNotaDuplicado.UltimoId);

            return true;
        }
    }
}
