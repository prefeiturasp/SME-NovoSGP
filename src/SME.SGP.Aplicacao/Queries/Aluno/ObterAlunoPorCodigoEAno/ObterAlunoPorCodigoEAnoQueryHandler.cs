using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEAnoQueryHandler : IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>
    {
        private readonly IMediator mediator;

        public ObterAlunoPorCodigoEAnoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoEAnoQuery request,
            CancellationToken cancellationToken)
        {
            var alunoPorTurmaResposta =
                (await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(request.CodigoAluno, request.AnoLetivo, false,
                    request.TurmaHistorica))).OrderByDescending(a => a.DataSituacao).FirstOrDefault();

            if (alunoPorTurmaResposta == null)
                throw new NegocioException("Aluno não localizado");

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno)
                    ? alunoPorTurmaResposta.NomeAluno
                    : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.NumeroAlunoChamada,
                DataNascimento = alunoPorTurmaResposta.DataNascimento,
                DataSituacao = alunoPorTurmaResposta.DataSituacao,
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                Situacao = alunoPorTurmaResposta.SituacaoMatricula,
                TurmaEscola = await OberterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma)
            };

            return alunoReduzido;
        }

        private async Task<string> OberterNomeTurmaFormatado(long turmaId)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
    }
}