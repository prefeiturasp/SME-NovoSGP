using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.HistoricoEscolar.ServicosFakes
{
    public class ObterAlunosEolPorTurmaQueryHandlerFake : IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";


        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosEolPorTurmaQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunoPorTurmaResposta> {

                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(request.TurmaId),
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
                      CodigoTurma=int.Parse(request.TurmaId),
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
                      CodigoTurma=int.Parse(request.TurmaId),
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
                      CodigoTurma=int.Parse(request.TurmaId),
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
                      CodigoTurma=int.Parse(request.TurmaId),
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
                }
            };

            return alunos.Where(x => x.CodigoTurma.ToString() == request.TurmaId);
        }
    }
}
