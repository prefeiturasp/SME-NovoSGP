using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora
{
    public class SalvarImportacaoArquivoFluenciaLeitoraCommandHandler : IRequestHandler<SalvarImportacaoArquivoFluenciaLeitoraCommand, Dominio.FluenciaLeitora>
    {
        private readonly IRepositorioFluenciaLeitora repositorioFluenciaLeitora;
        public SalvarImportacaoArquivoFluenciaLeitoraCommandHandler(IRepositorioFluenciaLeitora repositorioFluenciaLeitora)
        {
            this.repositorioFluenciaLeitora = repositorioFluenciaLeitora ?? throw new ArgumentNullException(nameof(repositorioFluenciaLeitora));
        }

        public async Task<Dominio.FluenciaLeitora> Handle(SalvarImportacaoArquivoFluenciaLeitoraCommand request, CancellationToken cancellationToken)
        {
            var dto = MapearParaEntidade(request);
            var registroAtual = await repositorioFluenciaLeitora.ObterRegistroFluenciaLeitoraAsync(
                dto.AnoLetivo,
                dto.CodigoEOLTurma,
                dto.CodigoEOLAluno,
                dto.TipoAvaliacao
            );

            if (registroAtual != null)
            {
                registroAtual.Fluencia = dto.Fluencia;
                dto = registroAtual;
            }

            await repositorioFluenciaLeitora.SalvarAsync(dto);

            return dto;
        }

        private Dominio.FluenciaLeitora MapearParaEntidade(SalvarImportacaoArquivoFluenciaLeitoraCommand request)
        => new Dominio.FluenciaLeitora()
        {
            AnoLetivo = request.ArquivoFluenciaLeitora.AnoLetivo,
            CodigoEOLTurma = request.ArquivoFluenciaLeitora.CodigoEOLTurma,
            CodigoEOLAluno = request.ArquivoFluenciaLeitora.CodigoEOLAluno,
            Fluencia = request.ArquivoFluenciaLeitora.Fluencia,
            TipoAvaliacao = request.ArquivoFluenciaLeitora.TipoAvaliacao
        };
    }
}
