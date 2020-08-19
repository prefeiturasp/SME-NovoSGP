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
    public class ObterPareceresConclusivosQueryHandler : IRequestHandler<ObterPareceresConclusivosQuery, IEnumerable<ConselhoClasseParecerConclusivoDto>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo;

        public ObterPareceresConclusivosQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorioConselhoClasseParecerConclusivo)
        {
            this.repositorioConselhoClasseParecerConclusivo = repositorioConselhoClasseParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseParecerConclusivo));
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivoDto>> Handle(ObterPareceresConclusivosQuery request, CancellationToken cancellationToken)
        {
            var pareceresConclusivos = await repositorioConselhoClasseParecerConclusivo.ObterListaVigente(DateTime.Now);

            if (pareceresConclusivos == null || !pareceresConclusivos.Any())
                throw new NegocioException("Não foi possível obter os pareceres conclusivos");

            var listaPareceres =  MapearListaEntidadeParaDto(pareceresConclusivos);

            listaPareceres = listaPareceres.Append(AdicionarSemParecer());

            return listaPareceres.OrderBy(p => p.Id);
        }

        private ConselhoClasseParecerConclusivoDto AdicionarSemParecer()
        {
             return new ConselhoClasseParecerConclusivoDto()
            {
                Id = -1,
                Nome = "Sem Parecer",
            };
        }


        private ConselhoClasseParecerConclusivoDto TransformaEntidadeEmDtoListagem(ConselhoClasseParecerConclusivo item)
        {
            return new ConselhoClasseParecerConclusivoDto()
            {
                Aprovado = item.Aprovado,
                Conselho = item.Conselho,
                FimVigencia = item.FimVigencia,
                Frequencia = item.Frequencia,
                Id = item.Id,
                InicioVigencia = item.InicioVigencia,
                Nome = item.Nome,
                Nota = item.Nota
            };
        }

        private IEnumerable<ConselhoClasseParecerConclusivoDto> MapearListaEntidadeParaDto(IEnumerable<ConselhoClasseParecerConclusivo> items)
        {
            if (items.Any())
            {
                foreach (var item in items)
                {
                    yield return TransformaEntidadeEmDtoListagem(item);
                }
            }
        }
    }
}
