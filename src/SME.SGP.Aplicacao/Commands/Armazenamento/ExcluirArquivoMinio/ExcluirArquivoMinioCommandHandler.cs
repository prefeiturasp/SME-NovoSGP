using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class ExcluirArquivoMinioCommandHandler : IRequestHandler<ExcluirArquivoMinioCommand,bool>
    {
        private readonly IServicoArmazenamento servicoArmazenamento;

        public ExcluirArquivoMinioCommandHandler(IServicoArmazenamento servicoArmazenamento)
        {
            this.servicoArmazenamento = servicoArmazenamento ?? throw new ArgumentNullException(nameof(servicoArmazenamento));
        }

        public async Task<bool> Handle(ExcluirArquivoMinioCommand request, CancellationToken cancellationToken)
        {
            return await servicoArmazenamento.Excluir(request.ArquivoNome,request.BucketNome);
        }
    }

}