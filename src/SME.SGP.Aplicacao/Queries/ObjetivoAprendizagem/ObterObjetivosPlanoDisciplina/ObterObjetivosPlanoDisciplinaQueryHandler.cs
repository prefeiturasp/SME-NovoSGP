using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPlanoDisciplinaQueryHandler : AbstractUseCase, IRequestHandler<ObterObjetivosPlanoDisciplinaQuery, IEnumerable<ObjetivoAprendizagemDto>>
    {

        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        public ObterObjetivosPlanoDisciplinaQueryHandler(IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                         IMediator mediator) : base(mediator)
        {
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Handle(ObterObjetivosPlanoDisciplinaQuery request, CancellationToken cancellationToken)
        {
            var objetivosPlano = await repositorioObjetivosPlano.ObterObjetivosPlanoDisciplina(request.DataReferencia.Year,
                                                                                               request.Bimestre,
                                                                                               request.TurmaId,
                                                                                               request.ComponenteCurricularId,
                                                                                               request.DisciplinaId,
                                                                                               request.FiltrarSomenteRegencia);

            return MapearParaDto(objetivosPlano).ToList();
        }

        private IEnumerable<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagem> objetivos)
        {
            foreach (ObjetivoAprendizagem objetivoBase in objetivos)
            {
                if (objetivoBase.Ano != 0)
                {
                    yield return new ObjetivoAprendizagemDto()
                    {
                        Descricao = objetivoBase.Descricao,
                        Id = objetivoBase.Id,
                        Ano = objetivoBase.Ano.ToString(),
                        Codigo = objetivoBase.Codigo,
                        IdComponenteCurricular = objetivoBase.ComponenteCurricularId
                    };
                }
            }
        }
    }
}
