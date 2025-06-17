using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFechamentoAlunoUseCase : AbstractUseCase, ISalvarAnotacaoFechamentoAlunoUseCase
    {
        public SalvarAnotacaoFechamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaPersistenciaDto> Executar(AnotacaoAlunoDto anotacaoDto)
        {
            var anotacao = await MapearParaEntidade(anotacaoDto);

            // Excluir anotacao quando enviado string vazia
            if (string.IsNullOrEmpty(anotacaoDto.Anotacao))
                await mediator.Send(new ExcluirAnotacaoFechamentoAlunoCommand(anotacao));
            else
                await mediator.Send(new SalvarAnotacaoFechamentoAlunoCommand(anotacao));

            return (AuditoriaPersistenciaDto)anotacao;
        }


        private async Task<AnotacaoFechamentoAluno> MapearParaEntidade(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await mediator.Send(new ObterAnotacaoFechamentoAlunoPorFechamentoEAlunoQuery(anotacaoAluno.FechamentoId, anotacaoAluno.CodigoAluno));
            await MoverRemoverExcluidos(anotacaoAluno, anotacao);
            if (anotacao.EhNulo())
                anotacao = new AnotacaoFechamentoAluno()
                {
                    FechamentoAlunoId = await ObterFechamentoAluno(anotacaoAluno.FechamentoId, anotacaoAluno.CodigoAluno),
                    Anotacao = anotacaoAluno.Anotacao
                };
            else
                anotacao.Anotacao = anotacaoAluno.Anotacao;

            return anotacao;
        }

        private async Task<long> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var fechamentoAluno = await mediator.Send(new ObterFechamentoAlunoPorTurmaIdQuery(fechamentoTurmaDisciplinaId, alunoCodigo));

            if (fechamentoAluno is null)
                return await GerarFechamentoAluno(fechamentoTurmaDisciplinaId, alunoCodigo);

            return fechamentoAluno.Id;
        }

        private async Task<long> GerarFechamentoAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var fechamentoAluno = new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                AlunoCodigo = alunoCodigo
            };

            await mediator.Send(new SalvarFechamentoAlunoCommand(fechamentoAluno));

            return fechamentoAluno.Id;
        }

        private async Task MoverRemoverExcluidos(AnotacaoAlunoDto anotacaoAluno, AnotacaoFechamentoAluno anotacao)
        {
            if (!string.IsNullOrEmpty(anotacaoAluno?.Anotacao))
            {
                anotacaoAluno.Anotacao = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.FechamentoAnotacao, anotacao.NaoEhNulo() ? anotacao.Anotacao : string.Empty, anotacaoAluno.Anotacao));
            }
            if (!string.IsNullOrEmpty(anotacao?.Anotacao))
            {
                var aquivoNovo = (anotacaoAluno?.Anotacao).NaoEhNulo() ? anotacaoAluno?.Anotacao : string.Empty;
                await mediator.Send(new RemoverArquivosExcluidosCommand(arquivoAtual: anotacao.Anotacao, arquivoNovo: aquivoNovo, caminho: TipoArquivo.FechamentoAnotacao.Name()));
            }
        }

    }
}
