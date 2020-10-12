using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObjetivosPlanoDisciplinaQueryHandler : AbstractUseCase, IRequestHandler<ObterObjetivosPlanoDisciplinaQuery, IEnumerable<ObjetivoAprendizagemDto>>
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano;
        private readonly IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem;
        public ObterObjetivosPlanoDisciplinaQueryHandler(IRepositorioObjetivoAprendizagemPlano repositorioObjetivosPlano,
                                                         IRepositorioObjetivoAprendizagem repositorioObjetivoAprendizagem,
                                                         IConfiguration configuration,
                                                         IRepositorioCache repositorioCache, 
                                                         IMediator mediator) : base(mediator)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioObjetivosPlano = repositorioObjetivosPlano ?? throw new ArgumentNullException(nameof(repositorioObjetivosPlano));
            this.repositorioObjetivoAprendizagem = repositorioObjetivoAprendizagem ?? throw new ArgumentNullException(nameof(repositorioObjetivoAprendizagem));
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> Handle(ObterObjetivosPlanoDisciplinaQuery request, CancellationToken cancellationToken)
        {
            var objetivosPlano = repositorioObjetivosPlano.ObterObjetivosPlanoDisciplina(request.DataReferencia.Year,
                                                                                         request.Bimestre,
                                                                                         request.TurmaId,
                                                                                         request.ComponenteCurricularId,
                                                                                         request.DisciplinaId,
                                                                                         request.FiltrarSomenteRegencia);

            return objetivosPlano;

            var objetivosJurema = await Listar();

            return objetivosJurema;

            //IEnumerable<ObjetivoAprendizagemDto> objetivosJurema = await Listar();

            //// filtra objetivos do jurema com os objetivos cadastrados no plano anual nesse bimestre
            //return objetivosJurema.
            //    Where(c => objetivosPlano.Any(o => o.ObjetivoAprendizagemJuremaId == c.Id)).OrderBy(c => c.Codigo);
        }

        private async Task<IEnumerable<ObjetivoAprendizagemDto>> Listar()
        {
            int tempoExpiracao = int.Parse(configuration.GetSection("ExpiracaoCache").GetSection("ObjetivosAprendizagem").Value);

            return await repositorioCache.ObterAsync("ObjetivosAprendizagem", () => ListarSemCache(), tempoExpiracao, true);
        }

        private async Task<List<ObjetivoAprendizagemDto>> ListarSemCache()
        {
            IEnumerable<ObjetivoAprendizagem> objetivosJuremaDto = await repositorioObjetivoAprendizagem.ListarAsync();
            return MapearParaDto(objetivosJuremaDto).ToList();
        }

        private IEnumerable<ObjetivoAprendizagemDto> MapearParaDto(IEnumerable<ObjetivoAprendizagemPlano> objetivos)
        {
            foreach (ObjetivoAprendizagemPlano objetivoBase in objetivos)
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
