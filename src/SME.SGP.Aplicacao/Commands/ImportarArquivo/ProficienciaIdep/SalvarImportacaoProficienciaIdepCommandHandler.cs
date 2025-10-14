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

        private static Dominio.Entidades.ProficienciaIdep MapearParaEntidade(SalvarImportacaoProficienciaIdepCommand request)
            => new Dominio.Entidades.ProficienciaIdep()
            {
                Id = request.ProficienciaIdep.Id != null ? request.ProficienciaIdep.Id.Value : 0,
                Boletim = request.ProficienciaIdep.Boletim,
                AnoLetivo = request.ProficienciaIdep.AnoLetivo,
                SerieAno = request.ProficienciaIdep.SerieAno,
                CodigoUe = request.ProficienciaIdep.CodigoEOLEscola,
                Proficiencia = request.ProficienciaIdep.Proficiencia,
                ComponenteCurricular = request.ProficienciaIdep.ComponenteCurricular,
            };
    }
}
