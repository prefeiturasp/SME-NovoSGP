using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdebCommandHandler : IRequestHandler<SalvarImportacaoArquivoIdebCommand, Dominio.Ideb>
    {
        private readonly IRepositorioIdeb repositorioIdeb;
        public SalvarImportacaoArquivoIdebCommandHandler(IRepositorioIdeb repositorioIdeb)
        {
            this.repositorioIdeb = repositorioIdeb ?? throw new ArgumentNullException(nameof(repositorioIdeb));
        }

        public async Task<Dominio.Ideb> Handle(SalvarImportacaoArquivoIdebCommand request, CancellationToken cancellationToken)
        {
            var dto = MapearParaEntidade(request);
            var registroAtual = await repositorioIdeb.ObterRegistroIdebAsync(dto.AnoLetivo, dto.SerieAno, dto.CodigoEOLEscola);

            if (registroAtual != null)
            {
                registroAtual.Nota = dto.Nota;
                dto = registroAtual;
            }

            await repositorioIdeb.SalvarAsync(dto);

            return dto;
        }

        private Dominio.Ideb MapearParaEntidade(SalvarImportacaoArquivoIdebCommand request)
        => new Dominio.Ideb()
        {
            AnoLetivo = request.ArquivoIdeb.AnoLetivo,
            SerieAno = request.ArquivoIdeb.SerieAno,
            CodigoEOLEscola = request.ArquivoIdeb.CodigoEOLEscola,
            Nota = request.ArquivoIdeb.Nota,
        };
    }
}
