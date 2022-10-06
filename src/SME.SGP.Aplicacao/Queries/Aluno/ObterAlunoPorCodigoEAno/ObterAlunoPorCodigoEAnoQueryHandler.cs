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

        public async Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var alunoPorTurmaResposta = (await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(request.CodigoAluno, request.AnoLetivo, false, request.TipoTurma))).OrderByDescending(a => a.DataSituacao).ThenByDescending(a => a.NumeroAlunoChamada)?.FirstOrDefault();

            if (alunoPorTurmaResposta == null)
                throw new NegocioException("Aluno não localizado");

            var alunoReduzido = new AlunoReduzidoDto()
            {
                Nome = !string.IsNullOrEmpty(alunoPorTurmaResposta.NomeAluno) ? alunoPorTurmaResposta.NomeAluno : alunoPorTurmaResposta.NomeSocialAluno,
                NumeroAlunoChamada = alunoPorTurmaResposta.ObterNumeroAlunoChamada(),
                DataNascimento = alunoPorTurmaResposta.DataNascimento,
                DataSituacao = alunoPorTurmaResposta.DataSituacao,
                CodigoAluno = alunoPorTurmaResposta.CodigoAluno,
                CodigoSituacaoMatricula = alunoPorTurmaResposta.CodigoSituacaoMatricula,
                Situacao = alunoPorTurmaResposta.SituacaoMatricula,               
                TurmaEscola = await ObterNomeTurmaFormatado(alunoPorTurmaResposta.CodigoTurma.ToString()),
                CodigoTurma = alunoPorTurmaResposta.CodigoTurma.ToString(),
            };

            return alunoReduzido;
        }

        private async Task<string> ObterNomeTurmaFormatado(string turmaCodigo)
        {
            var turmaNome = "";
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
    }
}
