using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoAluno : IComandosFechamentoAluno
    {
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAlunoConsulta;
        private readonly IMediator mediator;

        public ComandosFechamentoAluno(IRepositorioFechamentoAluno repositorioFechamentoAluno, IRepositorioFechamentoAlunoConsulta repositorioFechamentoAlunoConsulta, IMediator mediator)
        {
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoAlunoConsulta = repositorioFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoAlunoConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAnotacaoAluno(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await MapearParaEntidade(anotacaoAluno);

            // Excluir anotacao quando enviado string vazia
            if (string.IsNullOrEmpty(anotacaoAluno.Anotacao))
                anotacao.Excluido = true;

            await repositorioFechamentoAluno.SalvarAsync(anotacao);
            return (AuditoriaPersistenciaDto)anotacao;
        }

        private async Task<FechamentoAluno> MapearParaEntidade(AnotacaoAlunoDto anotacaoAluno)
        {
            var anotacao = await repositorioFechamentoAlunoConsulta.ObterFechamentoAluno(anotacaoAluno.FechamentoId, anotacaoAluno.CodigoAluno);
            await MoverRemoverExcluidos(anotacaoAluno, anotacao);
            if (anotacao == null)
                anotacao = new FechamentoAluno()
                {
                    FechamentoTurmaDisciplinaId = anotacaoAluno.FechamentoId,
                    AlunoCodigo = anotacaoAluno.CodigoAluno,
                    Anotacao = anotacaoAluno.Anotacao
                };
            else
                anotacao.Anotacao = anotacaoAluno.Anotacao;

            return anotacao;
        }
        private async Task MoverRemoverExcluidos(AnotacaoAlunoDto anotacaoAluno, FechamentoAluno anotacao)
        {
            if (!string.IsNullOrEmpty(anotacaoAluno?.Anotacao))
            {
                anotacaoAluno.Anotacao = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.FechamentoAnotacao, anotacao != null ? anotacao.Anotacao : string.Empty, anotacaoAluno.Anotacao));
            }
            if (!string.IsNullOrEmpty(anotacao?.Anotacao))
            {
                var aquivoNovo = anotacaoAluno?.Anotacao != null ? anotacaoAluno.Anotacao : string.Empty;
                await mediator.Send(new RemoverArquivosExcluidosCommand(arquivoAtual: anotacao.Anotacao, arquivoNovo: aquivoNovo, caminho: TipoArquivo.FechamentoAnotacao.Name()));
            }
        }
    }
}
