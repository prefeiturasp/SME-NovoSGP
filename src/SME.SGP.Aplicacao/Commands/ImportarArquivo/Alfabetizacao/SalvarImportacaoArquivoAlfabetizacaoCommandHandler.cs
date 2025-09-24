using MediatR;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Alfabetizacao;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Alfabetizacao
{
    public class SalvarImportacaoArquivoAlfabetizacaoCommandHandler : IRequestHandler<SalvarImportacaoArquivoAlfabetizacaoCommand, Dominio.Alfabetizacao>
    {
        private readonly IRepositorioAlfabetizacao repositorioAlfabetizacao;
        public SalvarImportacaoArquivoAlfabetizacaoCommandHandler(IRepositorioAlfabetizacao repositorioAlfabetizacao)
        {
            this.repositorioAlfabetizacao = repositorioAlfabetizacao ?? throw new ArgumentNullException(nameof(repositorioAlfabetizacao));
        }

        public async Task<Dominio.Alfabetizacao> Handle(SalvarImportacaoArquivoAlfabetizacaoCommand request, CancellationToken cancellationToken)
        {
            var dto = MapearParaEntidade(request);
            var registroAtual = await repositorioAlfabetizacao.ObterRegistroAlfabetizacaoAsync(dto.AnoLetivo, dto.CodigoEOLEscola);

            if (registroAtual != null)
            {
                registroAtual.TaxaAlfabetizacao = dto.TaxaAlfabetizacao;
                dto = registroAtual;
            }

            await repositorioAlfabetizacao.SalvarAsync(dto);

            return dto;
        }

        private Dominio.Alfabetizacao MapearParaEntidade(SalvarImportacaoArquivoAlfabetizacaoCommand request)
            => new Dominio.Alfabetizacao()
            {
                AnoLetivo = request.ArquivoAlfabetizacao.AnoLetivo,
                CodigoEOLEscola = request.ArquivoAlfabetizacao.CodigoEOLEscola,
                TaxaAlfabetizacao = request.ArquivoAlfabetizacao.TaxaAlfabetizacao
            };
    }
}
