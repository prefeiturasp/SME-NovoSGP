using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes
{
    public class ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake: IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            var alunosPorTurmaResposta = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "1",
                    NomeAluno = "Nome aluno 1",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-120).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 1
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "2",
                    NomeAluno = "Nome aluno 2",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-119).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 2
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "3",
                    NomeAluno = "Nome aluno 3",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-118).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 3
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "4",
                    NomeAluno = "Nome aluno 4",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-117).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 4
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "5",
                    NomeAluno = "Nome aluno 5",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    DataSituacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,20),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 5
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = "6",
                    NomeAluno = "Nome aluno 6",
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = "DESISTENTE",
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddMonths(-116).Date,
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-1).Date,
                    NumeroAlunoChamada = 6
                },
            };

            return alunosPorTurmaResposta.FirstOrDefault(f => f.CodigoAluno.Equals(request.AlunoCodigo));
        }
    }
}
