using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente.ServicosFake
{
    public class ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake : IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        public ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake()
        { }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            var alunosPorTurmaResposta = new List<AlunoPorTurmaResposta>()
        {
            new AlunoPorTurmaResposta()
            {
                CodigoAluno = "1",
                NomeAluno = "Nome aluno 1",
                DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-10).Date,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                SituacaoMatricula = "ATIVO",
                DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-120).Date,
                DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                NumeroAlunoChamada = 1,
                CodigoTurma = 1
            },
            new AlunoPorTurmaResposta()
            {
                CodigoAluno = "2",
                NomeAluno = "Nome aluno 2",
                DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-10).Date,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                SituacaoMatricula = "ATIVO",
                DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-118).Date,
                DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                NumeroAlunoChamada = 2,
                CodigoTurma = 1
            },
            new AlunoPorTurmaResposta()
            {
                CodigoAluno = "3",
                NomeAluno = "Nome aluno 3",
                DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-10).Date,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                SituacaoMatricula = "ATIVO",
                DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-117).Date,
                DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                NumeroAlunoChamada = 3,
                CodigoTurma = 1
            }
         };

            return alunosPorTurmaResposta.FirstOrDefault(f => f.CodigoAluno.Equals(request.AlunoCodigo) && f.CodigoTurma.ToString().Equals(request.TurmaCodigo));
        }
    }
}
