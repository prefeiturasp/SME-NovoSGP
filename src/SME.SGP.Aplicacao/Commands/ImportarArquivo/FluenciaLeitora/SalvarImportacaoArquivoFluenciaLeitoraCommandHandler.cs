using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora
{
    public class SalvarImportacaoArquivoFluenciaLeitoraCommandHandler : IRequestHandler<SalvarImportacaoArquivoFluenciaLeitoraCommand, ArquivoFluenciaLeitora>
    {
        private readonly IRepositorioArquivoFluenciaLeitora repositorioArquivoIbep;
        public SalvarImportacaoArquivoFluenciaLeitoraCommandHandler(IRepositorioArquivoFluenciaLeitora repositorioArquivoIbep)
        {
            this.repositorioArquivoIbep = repositorioArquivoIbep ?? throw new ArgumentNullException(nameof(repositorioArquivoIbep));
        }

        public async Task<ArquivoFluenciaLeitora> Handle(SalvarImportacaoArquivoFluenciaLeitoraCommand request, CancellationToken cancellationToken)
        {
            var arquivoFluenciaLeitora = MapearParaEntidade(request);

            await repositorioArquivoIbep.SalvarAsync(arquivoFluenciaLeitora);

            return arquivoFluenciaLeitora;
        }

        private ArquivoFluenciaLeitora MapearParaEntidade(SalvarImportacaoArquivoFluenciaLeitoraCommand request)
        => new ArquivoFluenciaLeitora()
        {
            CodigoEOLTurma = request.ArquivoFluenciaLeitora.CodigoEOLTurma,
            CodigoEOLAluno = request.ArquivoFluenciaLeitora.CodigoEOLAluno,
            Fluencia = request.ArquivoFluenciaLeitora.Fluencia,
            Periodo = request.ArquivoFluenciaLeitora.Periodo
        };
    }
}
