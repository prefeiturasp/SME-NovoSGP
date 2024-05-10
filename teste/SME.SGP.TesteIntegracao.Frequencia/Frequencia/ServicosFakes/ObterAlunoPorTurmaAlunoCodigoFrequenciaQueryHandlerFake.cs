using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFakes
{
    public class ObterAlunoPorTurmaAlunoCodigoFrequenciaQueryHandlerFake : IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        private const string ALUNO_DESISTENTE_CODIGO_77777 = "77777";
        private const string ALUNO_DESISTENTE_NOME_77777 = "ALUNO INATIVO 77777";
        private const string DESISTENTE = "Desistente";
        private const string ALUNO_DESISTENTE_RESPONSAVEL_77777 = "RESPONSAVEL DO ALUNO DESISTENTE 77777";
        private const string TIPO_RESPONSAVEL_4 = "4";
        private const string CELULAR_11111111111 = "11111111111";

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.TurmaCodigo))
                return await Task.FromResult(Obter(request.TurmaCodigo).OrderByDescending(a => a.DataSituacao)?.FirstOrDefault());

            return await Task.FromResult(Obter(request.TurmaCodigo).Where(da => da.CodigoTurma.ToString().Equals(request.TurmaCodigo)).FirstOrDefault());
        }

        private static List<AlunoPorTurmaResposta> Obter(string turmaId)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return new List<AlunoPorTurmaResposta> {
                new AlunoPorTurmaResposta
                {
                    Ano=0,
                    CodigoAluno = ALUNO_DESISTENTE_CODIGO_77777,
                    CodigoComponenteCurricular= 0,
                    CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                    CodigoTurma=int.Parse(turmaId),
                    DataNascimento=new DateTime(dataReferencia.AddYears(-15).Year,01,16,00,00,00),
                    DataSituacao= new DateTime(dataReferencia.Year,11,09,17,25,31),
                    DataMatricula= new DateTime(dataReferencia.Year,11,09,17,25,31),
                    EscolaTransferencia=null,
                    NomeAluno= ALUNO_DESISTENTE_NOME_77777,
                    NomeSocialAluno=null,
                    NumeroAlunoChamada=1,
                    ParecerConclusivo=null,
                    PossuiDeficiencia=false,
                    SituacaoMatricula=DESISTENTE,
                    Transferencia_Interna=false,
                    TurmaEscola=null,
                    TurmaRemanejamento=null,
                    TurmaTransferencia=null,
                    NomeResponsavel=ALUNO_DESISTENTE_RESPONSAVEL_77777,
                    TipoResponsavel=TIPO_RESPONSAVEL_4,
                    CelularResponsavel= CELULAR_11111111111,
                    DataAtualizacaoContato= new DateTime(dataReferencia.AddYears(-1).Year,06,22,19,02,35),
                }
            };
        }
    }
}
