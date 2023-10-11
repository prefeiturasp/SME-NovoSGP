using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake : IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        private readonly string ALUNO_CODIGO_7 = "7";
        private readonly string ALUNO_CODIGO_8 = "8";
        private readonly string ALUNO_CODIGO_9 = "9";
        private readonly string ALUNO_CODIGO_10 = "10";
        private readonly string ALUNO_CODIGO_11 = "11";
        private readonly string ALUNO_CODIGO_12 = "12";

        private readonly string ATIVO = "Ativo";
        private readonly string INATIVO = "Inativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosPorTurmaEAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var dataSituacao = new DateTime(anoAtual, 2, 1);

            var alunos = new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_2,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_2,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_3,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_3,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_4,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_4,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_5,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_5,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_6,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_6,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_7,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_7,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_8,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_8,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_9,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_9,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_10,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= dataSituacao,
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_10,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_11,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.ReclassificadoSaida,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= dataSituacao,
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_11,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_12,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Transferido,
                      CodigoTurma=int.Parse(request.CodigoTurma),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new(DateTimeExtension.HorarioBrasilia().Year, 11, 20),
                      DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_12,
                      NumeroAlunoChamada=12,
                      SituacaoMatricula= INATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                }
            };

            return alunos.Where(x => x.CodigoTurma.ToString() == request.CodigoTurma);
        }
    }
}
