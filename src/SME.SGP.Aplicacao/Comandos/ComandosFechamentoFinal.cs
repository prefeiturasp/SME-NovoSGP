using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;
        private readonly IServicoLog servicoLog;

        public ComandosFechamentoFinal(IRepositorioConceito repositorioConceito,
            IServicoFechamentoFinal servicoFechamentoFinal, IRepositorioTurma repositorioTurma, IRepositorioFechamentoFinal repositorioFechamentoFinal, IServicoLog servicoLog)
        {
            this.repositorioConceito = repositorioConceito ?? throw new System.ArgumentNullException(nameof(repositorioConceito));
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new System.ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoFinal));
            this.servicoLog = servicoLog ?? throw new System.ArgumentNullException(nameof(servicoLog));
        }

        public async Task<string[]> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var turma = ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);
            await servicoFechamentoFinal.VerificaPersistenciaGeral(turma);

            var fechamentos = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma);
            var mensagensDeErro = new List<string>();

            foreach (var fechamento in fechamentos)
            {
                try
                {
                    await servicoFechamentoFinal.SalvarAsync(fechamento);
                }
                catch (NegocioException nEx)
                {
                    mensagensDeErro.Add($"Não foi possível salvar o fechamento final do aluno de rf {fechamento.AlunoCodigo}. {nEx.Message}");
                }
                catch (System.Exception ex)
                {
                    servicoLog.Registrar(ex);
                    mensagensDeErro.Add($"Não foi possível salvar o fechamento final do aluno de rf {fechamento.AlunoCodigo}. Erro interno.");
                }
            }
            if (!mensagensDeErro.Any())
                mensagensDeErro.Add("Fechamento(s) salvo(s) com sucesso!");

            return mensagensDeErro.ToArray();
        }

        private Turma ObterTurma(string turmaCodigo)
        {
            var turma = repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");
            return turma;
        }

        private async Task<IEnumerable<FechamentoFinal>> TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, Turma turma)
        {
            var fechamentosFinais = new List<FechamentoFinal>();

            var fechamentosDaTurmaEDisciplina = await repositorioFechamentoFinal.ObterPorFiltros(fechamentoFinalSalvarDto.TurmaCodigo, null);

            foreach (var fechamentoItemDto in fechamentoFinalSalvarDto.Itens)
            {
                FechamentoFinal fechamentoFinal;

                fechamentoFinal = fechamentosDaTurmaEDisciplina.FirstOrDefault(a => a.AlunoCodigo == fechamentoItemDto.AlunoRf && a.DisciplinaCodigo == fechamentoItemDto.ComponenteCurricularCodigo);

                if (fechamentoFinal == null)
                {
                    fechamentoFinal = new FechamentoFinal();

                    fechamentoFinal.AtualizarTurma(turma);
                    fechamentoFinal.DisciplinaCodigo = fechamentoItemDto.ComponenteCurricularCodigo;
                    fechamentoFinal.AlunoCodigo = fechamentoItemDto.AlunoRf;
                }

                if (fechamentoItemDto.EhNota())
                    fechamentoFinal.Nota = fechamentoItemDto.Nota.Value;
                else
                {
                    var conceito = repositorioConceito.ObterPorId(fechamentoItemDto.ConceitoId.Value);
                    if (conceito == null)
                        throw new NegocioException("Não foi possível localizar o conceito.");

                    fechamentoFinal.AtualizaConceito(conceito);
                }

                fechamentoFinal.EhRegencia = fechamentoFinalSalvarDto.EhRegencia;

                fechamentosFinais.Add(fechamentoFinal);
            }
            return fechamentosFinais;
        }
    }
}