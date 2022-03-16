using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RemoverArquivosExcluidosCommand : IRequest<bool>
    {
        public RemoverArquivosExcluidosCommand(string arquivoAtual, string arquivoNovo, string caminho)
        {
            ArquivoAtual = arquivoAtual;
            ArquivoNovo = arquivoNovo;
            Caminho = caminho;
        }

        public string ArquivoAtual { get; set; }
        public string ArquivoNovo { get; set; }
        public string Caminho { get; set; }
    }
}
