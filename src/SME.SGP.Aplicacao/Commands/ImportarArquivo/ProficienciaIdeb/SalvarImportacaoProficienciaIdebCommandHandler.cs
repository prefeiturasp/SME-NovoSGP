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
               AnoLetivo = request.ProficienciaIdeb.AnoLetivo,
               SerieAno = request.ProficienciaIdeb.SerieAno,
               CodigoEOLEscola = request.ProficienciaIdeb.CodigoEOLEscola,
               Proficiencia = request.ProficienciaIdeb.Proficiencia,
               ComponenteCurricular = request.ProficienciaIdeb.ComponenteCurricular,
           };
    }
}
