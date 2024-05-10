using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterAlunoPorCodigoEAnoRelAcompanhamentoQueryHandlerFake : IRequestHandler<ObterAlunoPorCodigoEAnoQuery, AlunoReduzidoDto>
    {
        public async Task<AlunoReduzidoDto> Handle(ObterAlunoPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            return new AlunoReduzidoDto()
            {
                CodigoAluno = "1",
                Nome = "Nome Aluno",
                NumeroAlunoChamada = 1,
                DataNascimento = new DateTime(1990,2,1),
                DataSituacao = DateTimeExtension.HorarioBrasilia(),
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                Situacao = "Ativo",
                TurmaEscola = "1",
                CodigoTurma = "1",
                NomeResponsavel = "Nome do Responsavel",
                TipoResponsavel = "Tipo de Responsavel",
                CelularResponsavel = "999999999999",
                DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                EhAtendidoAEE = false
            };
        }
    }
}