using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudanteFotoUseCase : AbstractUseCase, IObterEstudanteFotoUseCase
    {
        public ObterEstudanteFotoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ArquivoDto> Executar(string alunoCodigo)
        {
            if (alunoCodigo.Length <= 0)
                throw new NegocioException("O código do aluno deve ser informado");

            var miniatura = await mediator.Send(new ObterMiniaturaFotoEstudantePorAlunoCodigoQuery(alunoCodigo));

            if (miniatura == null)
                return null;

            return await DownloadMiniatura(miniatura);
        }

        private async Task<ArquivoDto> DownloadMiniatura(MiniaturaFotoDto miniatura)
        {

            var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(miniatura.Codigo, miniatura.Nome, miniatura.Tipo));

            return new ArquivoDto()
            {
                Codigo = miniatura.CodigoFotoOriginal,
                Nome = miniatura.Nome,
                Download = (arquivoFisico, miniatura.TipoConteudo, miniatura.Nome)
            };

        }

    }
}
