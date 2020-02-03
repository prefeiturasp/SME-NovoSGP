using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IRepositorioTurma repositorioTurma,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new System.ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new System.ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId)
        {
            var turma = repositorioTurma.ObterPorId(codigoTurma);
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }
            var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(periodoEscolarId);
            if (periodoEscolar == null)
            {
                throw new NegocioException("Período escolar não encontrado.");
            }
            if (turma.MesmaModalidadePeriodoEscolar(periodoEscolar.TipoCalendario.Modalidade))
            {
            }
            var fechamento = new Fechamento(turma.Id, disciplinaId, periodoEscolar.Id);
            fechamento.Id = repositorioFechamento.Salvar(fechamento);
            servicoPendenciaFechamento.ValidarAvaliacoesSemNotasParaNenhumAluno(fechamento.Id, codigoTurma, disciplinaId, periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim);
        }
    }
}