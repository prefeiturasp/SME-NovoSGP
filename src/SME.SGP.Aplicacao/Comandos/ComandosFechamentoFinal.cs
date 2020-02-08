using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IServicoFechamentoFinal fechamentoFinal;
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioFechamentoFinal repositorioFechamentoFinal;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;

        public ComandosFechamentoFinal(IServicoFechamentoFinal fechamentoFinal, IRepositorioConceito repositorioConceito,
            IServicoFechamentoFinal servicoFechamentoFinal, IRepositorioTurma repositorioTurma, IRepositorioFechamentoFinal repositorioFechamentoFinal)
        {
            this.fechamentoFinal = fechamentoFinal ?? throw new System.ArgumentNullException(nameof(fechamentoFinal));
            this.repositorioConceito = repositorioConceito ?? throw new System.ArgumentNullException(nameof(repositorioConceito));
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new System.ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFechamentoFinal = repositorioFechamentoFinal ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoFinal));
        }

        public async Task SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var fechamentoFinal = TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto);
            await servicoFechamentoFinal.SalvarAsync(fechamentoFinal);
        }

        private FechamentoFinal TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            FechamentoFinal fechamentoFinal;//= new FechamentoFinal();

            var fechamentosDaTurmaEDisciplina = repositorioFechamentoFinal.ObterPorFiltros(fechamentoFinalSalvarDto.TurmaCodigo, fechamentoFinalSalvarDto.ComponenteCurricularCodigo);

            if (fechamentoFinalSalvarDto.EhNota())
                fechamentoFinal.Nota = fechamentoFinalSalvarDto.Nota.Value;
            else
            {
                var conceito = repositorioConceito.ObterPorId(fechamentoFinalSalvarDto.ConceitoId.Value);
                fechamentoFinal.Conceito = conceito ?? throw new NegocioException("Não foi possível localizar o conceito.");
            }

            var turma = repositorioTurma.ObterPorCodigo(fechamentoFinalSalvarDto.TurmaCodigo);
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");

            fechamentoFinal.AtualizarTurma(turma);
            fechamentoFinal.DisciplinaCodigo = fechamentoFinalSalvarDto.ComponenteCurricularCodigo;

            return fechamentoFinal;
        }
    }
}