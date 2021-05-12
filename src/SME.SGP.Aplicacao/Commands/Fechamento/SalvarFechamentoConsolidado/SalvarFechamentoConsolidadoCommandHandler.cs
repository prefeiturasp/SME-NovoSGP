using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoConsolidadoCommandHandler : IRequestHandler<SalvarFechamentoConsolidadoCommand, bool>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEol servicoEol;

        public SalvarFechamentoConsolidadoCommandHandler(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
                                                         IRepositorioFechamentoConsolidado repositorioFechamentoConsolidado,
                                                         IRepositorioTurma repositorioTurma, IServicoEol servicoEol)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoConsolidado = repositorioFechamentoConsolidado ?? throw new ArgumentNullException(nameof(repositorioFechamentoConsolidado));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<bool> Handle(SalvarFechamentoConsolidadoCommand request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurma.ObterPorId(request.TurmaId);

            var fechamentos = await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(request.TurmaId, new long[] { request.ComponenteCurricularId }, request.Bimestre);
            var professoresDaTurma = await servicoEol.ObterProfessoresTitularesDisciplinas(turma.CodigoTurma);

            var lstConsolidado = MapearFechamentoConsolidado(fechamentos, professoresDaTurma);

            foreach (var consolidado in lstConsolidado)
            {
                await repositorioFechamentoConsolidado.SalvarAsync(consolidado);
            }

            return true;
        }

        private IEnumerable<FechamentoConsolidadoComponenteTurma> MapearFechamentoConsolidado(IEnumerable<FechamentoTurmaDisciplina> fechamentos, IEnumerable<Infra.ProfessorTitularDisciplinaEol> professoresDaTurma)
        {
            foreach (var fechamento in fechamentos)
            {
                var professorComponente = professoresDaTurma.FirstOrDefault(p => p.DisciplinaId == fechamento.DisciplinaId);

                yield return new FechamentoConsolidadoComponenteTurma()
                {
                    Bimestre = fechamento.FechamentoTurma.PeriodoEscolar.Bimestre,
                    ComponenteCurricularCodigo = fechamento.DisciplinaId,
                    DataAtualizacao = DateTime.Now,
                    TurmaId = fechamento.FechamentoTurma.TurmaId,
                    Status = fechamento.ObterStatusFechamento(),
                    ProfessorNome = professorComponente.ProfessorNome,
                    ProfessorRf = professorComponente.ProfessorRf
                };
            }
        }
    }
}
