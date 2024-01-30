using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.UseCases
{
    public class LimpezaFechamentoTurmaDisciplinaDuplicadoUseCase : ILimpezaFechamentoTurmaDisciplinaDuplicadoUseCase
    {
        private readonly IRepositorioSGP repositorioSGP;

        public LimpezaFechamentoTurmaDisciplinaDuplicadoUseCase(IRepositorioSGP repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new System.ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var fechamentoTurmaDisciplinaDuplicado = mensagem.ObterObjetoMensagem<FechamentoTurmaDisciplinaDuplicado>();

            await repositorioSGP.AtualizarAlunoFechamentoTurmaDisciplinaDuplicados(fechamentoTurmaDisciplinaDuplicado.FechamentoTurmaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.DisciplinaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.UltimoId);

            await repositorioSGP.AtualizarAnotacaoAlunoFechamentoTurmaDisciplinaDuplicados(fechamentoTurmaDisciplinaDuplicado.FechamentoTurmaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.DisciplinaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.UltimoId);

            await repositorioSGP.AtualizarPendenciaFechamentoTurmaDisciplinaDuplicados(fechamentoTurmaDisciplinaDuplicado.FechamentoTurmaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.DisciplinaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.UltimoId);

            await repositorioSGP.ExcluirFechamentoTurmaDisciplinaDuplicados(fechamentoTurmaDisciplinaDuplicado.FechamentoTurmaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.DisciplinaId,
                                                                                   fechamentoTurmaDisciplinaDuplicado.UltimoId);

            return true;
        }
    }
}
