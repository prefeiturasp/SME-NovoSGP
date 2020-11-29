using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSintesePorAnoLetivoUseCase : AbstractUseCase, IObterSintesePorAnoLetivoUseCase
    {
        private readonly IRepositorioSintese repositorioSintese;

        public ObterSintesePorAnoLetivoUseCase(IMediator mediator, IRepositorioSintese repositorioSintese) : base(mediator)
        {
            this.repositorioSintese = repositorioSintese ?? throw new ArgumentNullException(nameof(repositorioSintese));
        }

        public async Task<IEnumerable<SinteseDto>> Executar(int anoLetivo)
        {
            var dataReferencia = new DateTime(anoLetivo, 6, 28);
            var sinteses = repositorioSintese.ObterPorData(dataReferencia);

            if (sinteses == null || !sinteses.Any())
                throw new NegocioException("Não foi possível obter as sínteses");

            return await Task.FromResult(MapearParaDto(sinteses));
        }

        private IEnumerable<SinteseDto> MapearParaDto(IEnumerable<Sintese> sinteses)
        {
            foreach (var sintese in sinteses)
            {
                yield return new SinteseDto()
                {
                    Id = (SinteseEnum)sintese.Id,
                    Valor = sintese.Descricao
                };
            }
        }
    }
}
