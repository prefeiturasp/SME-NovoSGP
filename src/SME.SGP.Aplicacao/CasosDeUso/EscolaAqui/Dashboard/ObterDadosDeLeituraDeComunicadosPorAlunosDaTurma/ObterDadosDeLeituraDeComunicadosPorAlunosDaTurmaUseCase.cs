using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma
{
    public class ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase : IObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoAluno servicoAluno;

        public ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase(IMediator mediator, IServicoAluno servicoAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoAluno = servicoAluno ?? throw new ArgumentNullException(nameof(servicoAluno));
        }

        public async Task<IEnumerable<DadosLeituraAlunosComunicadoPorTurmaDto>> Executar(FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto request)
        {
            var dadosLeituraAlunosComunicadoPorTurmaDto = await mediator.Send(new ObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaQuery(request.ComunicadoId, request.CodigoTurma));

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.CodigoTurma.ToString()));
            if (turma == null)
                throw new Exception("Não foi possível localizar a turma");

            var periodoEscolar = await mediator.Send(new ObterUltimoPeriodoEscolarPorDataQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, DateTime.Now.Date));
            if (periodoEscolar == null)
                throw new Exception("Não foi possível localizar o periodo escolar");

            var dadosLeituraAlunosComunicadoPorTurmaComMarcador = new List<DadosLeituraAlunosComunicadoPorTurmaDto>();
            foreach (var item in dadosLeituraAlunosComunicadoPorTurmaDto)
            {
                var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(item.CodigoAluno.ToString(), turma.AnoLetivo, string.Empty));

                if (aluno == null)
                    throw new Exception("Não foi possível localizar o aluno");

                item.Marcador = servicoAluno.ObterMarcadorAluno(aluno, periodoEscolar, false);

                item.EhAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));

                dadosLeituraAlunosComunicadoPorTurmaComMarcador.Add(item);
            }

            return dadosLeituraAlunosComunicadoPorTurmaComMarcador.ToList().OrderBy(a => a.NomeAluno);
        }
    }
}