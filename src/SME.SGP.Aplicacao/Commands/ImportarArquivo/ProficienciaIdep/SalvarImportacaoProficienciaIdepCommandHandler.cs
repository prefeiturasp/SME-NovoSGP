using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class SalvarImportacaoProficienciaIdepCommandHandler : IRequestHandler<SalvarImportacaoProficienciaIdepCommand, bool>
    {
        private readonly IRepositorioProficienciaIdep _repositorioProficienciaIdep;

        public SalvarImportacaoProficienciaIdepCommandHandler(IRepositorioProficienciaIdep repositorioProficienciaIdep)
        {
            _repositorioProficienciaIdep = repositorioProficienciaIdep ?? throw new ArgumentNullException(nameof(repositorioProficienciaIdep));
        }

        public async Task<bool> Handle(SalvarImportacaoProficienciaIdepCommand request, CancellationToken cancellationToken)
        {
            var proficienciaIdep = MapearParaEntidade(request);

            await _repositorioProficienciaIdep.SalvarAsync(proficienciaIdep);

            return proficienciaIdep.Id > 0;
        }

        private Dominio.ProficienciaIdep MapearParaEntidade(SalvarImportacaoProficienciaIdepCommand request)
        => new Dominio.ProficienciaIdep()
        {
            AnoLetivo = request.ProficienciaIdep.AnoLetivo,
            SerieAno = request.ProficienciaIdep.SerieAno,
            CodigoEOLEscola = request.ProficienciaIdep.CodigoEOLEscola,
            Proficiencia = request.ProficienciaIdep.Proficiencia,
            ComponenteCurricular = request.ProficienciaIdep.ComponenteCurricular,
        };
    }
}
