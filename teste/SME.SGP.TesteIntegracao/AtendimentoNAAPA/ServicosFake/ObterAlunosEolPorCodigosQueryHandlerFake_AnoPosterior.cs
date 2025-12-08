using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterAlunosEolPorCodigosQueryHandlerFake_AnoPosterior : IRequestHandler<ObterAlunosEolPorCodigosQuery, IEnumerable<TurmasDoAlunoDto>>
    {
        private const int ALUNO_1 = 1;
        private const int ALUNO_2 = 2;
        private const int ALUNO_3 = 3;
        public async Task<IEnumerable<TurmasDoAlunoDto>> Handle(ObterAlunosEolPorCodigosQuery request, CancellationToken cancellationToken)
        {
            var dataAnoPosterior = DateTimeExtension.HorarioBrasilia().AddYears(1);
            var dataAnoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1);
            var lista = new List<TurmasDoAlunoDto>()
            {
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_1,
                    NomeAluno = $"Nome do Aluno {ALUNO_1} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_1}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = dataAnoPosterior,
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 2,
                    AnoLetivo= dataAnoPosterior.Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },

                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_2,
                    NomeAluno = $"Nome do Aluno {ALUNO_2} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_2}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = dataAnoAnterior,
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 1,
                    AnoLetivo= dataAnoAnterior.Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_2,
                    NomeAluno = $"Nome do Aluno {ALUNO_2} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_2}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 2,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                },
                new TurmasDoAlunoDto
                {
                    CodigoAluno = ALUNO_3,
                    NomeAluno = $"Nome do Aluno {ALUNO_3} ",
                    NomeSocialAluno = $"Nome Social do Aluno {ALUNO_3}",
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo,
                    DataSituacao = DateTimeExtension.HorarioBrasilia(),
                    NumeroAlunoChamada = 1,
                    CodigoTurma = 2,
                    AnoLetivo= DateTimeExtension.HorarioBrasilia().Year,
                    CodigoTipoTurma= (int)TipoTurma.Regular,
                }
            };

            return await Task.FromResult(lista.FindAll(aluno => aluno.CodigoAluno == request.CodigosAluno.FirstOrDefault()));
        }
    }
}
