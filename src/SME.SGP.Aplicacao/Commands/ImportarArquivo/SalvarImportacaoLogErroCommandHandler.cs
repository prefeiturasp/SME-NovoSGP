using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo
{
    public class SalvarImportacaoLogErroCommandHandler : IRequestHandler<SalvarImportacaoLogErroCommand, ImportacaoLogErro>
    {
        private readonly IRepositorioImportacaoLogErro repositorioImportacaoLogErro;
        public SalvarImportacaoLogErroCommandHandler(IRepositorioImportacaoLogErro repositorioImportacaoLogErro)
        {
            this.repositorioImportacaoLogErro = repositorioImportacaoLogErro ?? throw new ArgumentNullException(nameof(repositorioImportacaoLogErro));
        }
        public async Task<ImportacaoLogErro> Handle(SalvarImportacaoLogErroCommand request, CancellationToken cancellationToken)
        {
            var importacaoLogErro = MapearParaEntidade(request);

            await repositorioImportacaoLogErro.SalvarAsync(importacaoLogErro);

            return importacaoLogErro;
        }

        private ImportacaoLogErro MapearParaEntidade(SalvarImportacaoLogErroCommand request)
            => new ImportacaoLogErro()
            {
                ImportacaoLogId = request.ImportacaoLogErro.ImportacaoLogId,
                LinhaArquivo = request.ImportacaoLogErro.LinhaArquivo,
                MotivoFalha = request.ImportacaoLogErro.MotivoFalha,
            };
    }
}
