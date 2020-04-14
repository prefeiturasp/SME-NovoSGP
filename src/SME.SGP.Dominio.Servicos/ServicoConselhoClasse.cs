using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoConselhoClasse : IServicoConselhoClasse
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IConsultasTurma consultasTurma;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                     IConsultasTurma consultasTurma)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.consultasTurma = consultasTurma ?? throw new ArgumentNullException(nameof(consultasTurma));
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(conselhoClasse.FechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Não foi possível localizar o fechamento da turma informado!");

            var conselhoClasseExistente = repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(fechamentoTurma.Id, fechamentoTurma.PeriodoEscolarId);
            if (conselhoClasseExistente != null)
                throw new NegocioException($"Já existe um conselho de classe gerado para a turma {fechamentoTurma.Turma.Nome}!");

            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            } 
            else
            {
                // Fechamento Final
                if (!await ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não possui o conselho de classe do último bimestre");
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
        }

        private async Task<bool> ValidaConselhoClasseUltimoBimestre(Turma turma)
        {
            var periodoEscolar = repositorioPeriodoEscolar.ObterUltimoBimestreAsync(turma.AnoLetivo, turma.ObterModalidadeTipoCalendario(), DateTime.Today.Semestre());
            if (periodoEscolar == null)
                throw new NegocioException($"Não foi encontrado o ultimo periodo escolar para a turma {turma.Nome}");

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(turma.Id, periodoEscolar.Id);
            return conselhoClasseUltimoBimestre != null;
        }

        public async Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno)
        {
            // Se não existir conselho de classe para o fechamento gera
            if (conselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(conselhoClasseAluno.ConselhoClasse);
                conselhoClasseAluno.ConselhoClasseId = conselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(conselhoClasseAluno.ConselhoClasse);

            await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

            return (AuditoriaConselhoClasseAlunoDto)conselhoClasseAluno;
        }
    }
}
