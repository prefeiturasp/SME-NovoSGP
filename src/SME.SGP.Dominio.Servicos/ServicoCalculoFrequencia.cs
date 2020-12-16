using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoFrequencia : IServicoCalculoFrequencia
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IMediator mediator;

        public ServicoCalculoFrequencia(IRepositorioTurma repositorioTurma,
                                        IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                        IMediator mediator)
        {

            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private async Task<int> ObterBimestre(DateTime data, string turmaId)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaId);
            return await consultasPeriodoEscolar.ObterBimestre(data, turma.ModalidadeCodigo, turma.Semestre);
        }

        public async Task CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId)
        {
            var bimestre = await ObterBimestre(dataAula, turmaId);

            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, dataAula, turmaId, disciplinaId, bimestre));

        }
    }
}