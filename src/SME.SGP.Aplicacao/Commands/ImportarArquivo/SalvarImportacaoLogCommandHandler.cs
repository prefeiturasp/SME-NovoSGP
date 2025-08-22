using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo
{
    public class SalvarImportacaoLogCommandHandler : IRequestHandler<SalvarImportacaoLogCommand, ImportacaoLog>
    {
        private readonly IRepositorioImportacaoLog repositorioImportacaoLog;
        public SalvarImportacaoLogCommandHandler(IRepositorioImportacaoLog repositorioImportacaoLog)
        {
            this.repositorioImportacaoLog = repositorioImportacaoLog ?? throw new ArgumentNullException(nameof(repositorioImportacaoLog));
        }
        public async Task<ImportacaoLog> Handle(SalvarImportacaoLogCommand request, CancellationToken cancellationToken)
        {
            var importacaoLog = MapearParaEntidade(request);

            var importacaoLogId =  await repositorioImportacaoLog.SalvarAsync(importacaoLog);
            importacaoLog.Id = importacaoLogId;

            return importacaoLog;
        }

        private ImportacaoLog MapearParaEntidade(SalvarImportacaoLogCommand request)
            => new ImportacaoLog()
            {
                NomeArquivo = request.ImportacaoLog.Arquivo.FileName,
                TipoArquivoImportacao = request.ImportacaoLog.TipoArquivoImportacao,
                StatusImportacao = request.ImportacaoLog.StatusImportacao,
                DataInicioProcessamento = DateTime.Now,
            };
    }
}
