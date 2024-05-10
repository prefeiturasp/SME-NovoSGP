using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivosUseCase : AbstractUseCase, IExcluirArquivosUseCase
    {
        public ExcluirArquivosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(IEnumerable<Guid> param)
        {
            var arquivos = (await mediator.Send(new ObterArquivosPorCodigosQuery(param.ToArray()))).ToList();

            if (!arquivos.Any()) 
                throw new NegocioException(MensagemNegocioComuns.NENHUM_ARQUIVO_ENCONTRADO);

            var idsParaExcluisao = arquivos.Select(x => x.Id).ToList();

            await mediator.Send(new ExcluirArquivosRepositorioPorIdsCommand(idsParaExcluisao));

            foreach (var arquivo in arquivos)
                await ExcluirArquivoArmazenamento(arquivo: arquivo);

            return true;
        }


        private async Task<bool> ExcluirArquivoArmazenamento(Arquivo arquivo)
        {
            var extencao = Path.GetExtension(arquivo.Nome);
            var filtro = new FiltroExcluirArquivoArmazenamentoDto {ArquivoNome = arquivo.Codigo.ToString() + extencao};
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
        }
    }
}