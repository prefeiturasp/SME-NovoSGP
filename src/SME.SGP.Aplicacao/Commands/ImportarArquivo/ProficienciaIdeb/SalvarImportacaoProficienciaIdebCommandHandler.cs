using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdeb
{
    public class SalvarImportacaoProficienciaIdebCommandHandler : IRequestHandler<SalvarImportacaoProficienciaIdebCommand, bool>
    {
        private readonly IRepositorioProficienciaIdeb _repositorioProficienciaIdeb;

        public SalvarImportacaoProficienciaIdebCommandHandler(IRepositorioProficienciaIdeb repositorioProficienciaIdeb)
        {
            _repositorioProficienciaIdeb = repositorioProficienciaIdeb;
        }

        public async Task<bool> Handle(SalvarImportacaoProficienciaIdebCommand request, CancellationToken cancellationToken)
        {
            var proficienciaIdeb = MapearParaEntidade(request);

            await _repositorioProficienciaIdeb.SalvarAsync(proficienciaIdeb);

            return proficienciaIdeb.Id > 0;
        }

        private Dominio.Entidades.ProficienciaIdeb MapearParaEntidade(SalvarImportacaoProficienciaIdebCommand request)
           => new Dominio.Entidades.ProficienciaIdeb()
           {
               Id = request.ProficienciaIdeb.Id != null ? request.ProficienciaIdeb.Id.Value : 0,
               AnoLetivo = request.ProficienciaIdeb.AnoLetivo,
               SerieAno = request.ProficienciaIdeb.SerieAno,
               CodigoEOLEscola = request.ProficienciaIdeb.CodigoEOLEscola,
               Boletim = request.ProficienciaIdeb.Boletim
               Proficiencia = request.ProficienciaIdeb.Proficiencia,
               ComponenteCurricular = request.ProficienciaIdeb.ComponenteCurricular,
               Boletim = request.ProficienciaIdeb.Boletim
           };
    }
}
