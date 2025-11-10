using MediatR;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class DownloadTodosAnexosInformativoUseCase : AbstractUseCase, IDownloadTodosAnexosInformativoUseCase
    {
        public DownloadTodosAnexosInformativoUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<(byte[], string, string)> Executar(long informativoId)
        {
            List<(string Nome, byte[] Conteudo)> arquivos = await ObterByteArrayArquivosAnexosInformativo(informativoId);

            byte[] arquivoZipBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive arquivoZip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var arquivo in arquivos)
                    {
                        var entry = arquivoZip.CreateEntry(arquivo.Nome, CompressionLevel.Optimal);
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(arquivo.Conteudo, 0, arquivo.Conteudo.Length);
                        }
                    }
                }
                arquivoZipBytes = ms.ToArray();
            }

            return (arquivoZipBytes, "application/zip", $"AnexosInformativo{informativoId}.zip");
        }

        private async Task<List<(string Nome, byte[] Conteudo)>> ObterByteArrayArquivosAnexosInformativo(long informativoId)
        {
            var anexos = await mediator.Send(new ObterAnexosPorInformativoIdQuery(informativoId));
            var arquivos = new List<(string, byte[])>();
            foreach (var anexo in anexos)
            {
                var extensao = Path.GetExtension(anexo.Nome);
                var nomeArquivoComExtensao = $"{anexo.Codigo}{extensao}";
                arquivos.Add((anexo.Nome,
                              await mediator.Send(
                                    new DownloadArquivoCommand(anexo.Codigo,
                                                               nomeArquivoComExtensao,
                                                               anexo.Tipo))));
            }
            return arquivos;
        }
    }
}
