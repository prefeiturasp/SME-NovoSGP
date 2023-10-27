using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEAnoHistoricoQueryHandler : IRequestHandler<ObterAlunoPorCodigoQuery, AlunoReduzidoDto>
    {
        private readonly IMediator mediator;
        public ObterAlunoPorCodigoEAnoHistoricoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoQuery request, CancellationToken cancellationToken)
        {
            var alunoPorTurmaResposta = (await mediator.Send(new ObterTurmaAlunoPorCodigoAlunoQuery(request.CodigoAluno))).OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();
            if (alunoPorTurmaResposta.EhNulo())
                throw new NegocioException("Aluno não localizado");

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
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

            if (turma.NaoEhNulo())
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
    }
}
