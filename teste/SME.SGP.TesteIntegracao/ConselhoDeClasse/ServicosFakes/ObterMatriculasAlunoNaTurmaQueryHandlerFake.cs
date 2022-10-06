using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterMatriculasAlunoNaTurmaQueryHandlerFake : IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    Ano = 7,
                    CelularResponsavel = "99999999999",
                    CodigoAluno = "1",
                    CodigoEscola = "1",
                    CodigoTurma = 1,
                    DataMatricula = DateTimeExtension.HorarioBrasilia().AddMonths(-1),
                    DataNascimento = DateTimeExtension.HorarioBrasilia().AddMonths(-12),
                    DataSituacao = DateTimeExtension.HorarioBrasilia().AddDays(-10),
                    EscolaTransferencia = "Escola tranferência",
                    NomeAluno = "Nome Aluno",
                    NomeResponsavel = "Nome responsável",
                    PossuiDeficiencia = false,
                    SituacaoMatricula = "Ativo",
                    TipoResponsavel = "Tipo responsável",
                    Transferencia_Interna = false,
                    TurmaEscola = "1",
                    TurmaRemanejamento = "",
                    TurmaTransferencia = "",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia().AddDays(-20),
                    NomeSocialAluno = "Nome social",
                    NumeroAlunoChamada = '1',
                    CodigoComponenteCurricular = null,
                    ParecerConclusivo = null
                }
            });
        }
    }
}