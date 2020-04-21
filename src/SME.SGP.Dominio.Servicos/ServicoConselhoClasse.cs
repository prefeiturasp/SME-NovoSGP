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
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public ServicoConselhoClasse(IRepositorioConselhoClasse repositorioConselhoClasse,
                                     IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                     IRepositorioFechamentoTurma repositorioFechamentoTurma,
                                     IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                     IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                     IConsultasConselhoClasse consultasConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(conselhoClasse.FechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Não foi possível localizar o fechamento da turma informado!");

            var conselhoClasseExistente = await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId);
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
                var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                if (!validacaoConselhoFinal.Item2)
                    throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
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
