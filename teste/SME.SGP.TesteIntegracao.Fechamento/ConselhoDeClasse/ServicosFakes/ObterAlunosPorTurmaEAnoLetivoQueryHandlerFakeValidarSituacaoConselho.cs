using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarSituacaoConselho : IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";

        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var dataRefencia = DateTimeExtension.HorarioBrasilia();

            return await Task.FromResult(new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_1,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-12),
                    DataSituacao = dataRefencia.AddDays(-20),
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel =CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddMonths(-1)
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_2,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-11),
                    DataSituacao = dataRefencia.AddDays(-20),
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel =CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddMonths(-1)
                },
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_3,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-15),
                    DataSituacao = dataRefencia.AddDays(-20),
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel =CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddMonths(-1)
                },                
                new AlunoPorTurmaResposta
                {
                    Ano = 0,
                    CodigoAluno = ALUNO_CODIGO_4,
                    CodigoComponenteCurricular = 0,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    CodigoTurma = int.Parse(request.CodigoTurma),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddYears(-14),
                    DataSituacao = dataRefencia.AddDays(-20),
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    NomeAluno = ALUNO_CODIGO_1,
                    NumeroAlunoChamada = 0,
                    SituacaoMatricula = ATIVO,
                    NomeResponsavel = RESPONSAVEL,
                    TipoResponsavel = TIPO_RESPONSAVEL_4,
                    CelularResponsavel =CELULAR_RESPONSAVEL,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddMonths(-1)
                }
            });
        }
    }
}