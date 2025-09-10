using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAlfabetizacaoCriticaEscrita
{
    public class SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandler : IRequestHandler<SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita repositorioConsolidacaoAlfabetizacaoCriticaEscrita;

        public SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommandHandler(IMediator mediator, IRepositorioConsolidacaoAlfabetizacaoCriticaEscrita repositorioConsolidacaoAlfabetizacaoCriticaEscrita)
        {
            this.mediator = mediator;
            this.repositorioConsolidacaoAlfabetizacaoCriticaEscrita = repositorioConsolidacaoAlfabetizacaoCriticaEscrita;
        }

        public async Task<bool> Handle(SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand request, CancellationToken cancellationToken)
        {
            if (request is null || request.ConsolidacaoAlfabetizacaoCriticaEscrita is null || request.ConsolidacaoAlfabetizacaoCriticaEscrita.Count() == 0)
                return false;

            try
            {
                var dadosTratados = await TratarDadosAsync(request.ConsolidacaoAlfabetizacaoCriticaEscrita);
                await repositorioConsolidacaoAlfabetizacaoCriticaEscrita.ExcluirConsolidacaoAlfabetizacaoCriticaEscrita();
                foreach (var consolidacaoAlfabetizacaoCriticaEscrita in dadosTratados)
                {
                    await repositorioConsolidacaoAlfabetizacaoCriticaEscrita
                        .SalvarConsolidacaoAlfabetizacaoCriticaEscrita(consolidacaoAlfabetizacaoCriticaEscrita);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar consolidação da alfabetização crítica escrita. {ex.Message}");
            }
        }

        private async Task<List<ConsolidacaoAlfabetizacaoCriticaEscrita>>
            TratarDadosAsync(IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto> dto)
        {
            var uesCodigo = dto.Select(c => c.UeCodigo).Distinct();
            var dresCodigo = dto.Select(c => c.DreCodigo).Distinct();
            var ues = await mediator.Send(new ObterUesComDrePorCodigoUesQuery(uesCodigo.ToArray()));
            var dtoOrdenado = dto.OrderByDescending(c => c.PercentualNaoAlfabetizados).ThenByDescending(c => c.QuantidadeNaoAlfabetizados);
            var resultado = new List<ConsolidacaoAlfabetizacaoCriticaEscrita>();

            foreach (var itemWithIndex in dtoOrdenado.Select((item, i) => new { Item = item, Index = i }))
            {
                var ue = ues.FirstOrDefault(u => u.CodigoUe == itemWithIndex.Item.UeCodigo && u.Dre.CodigoDre == itemWithIndex.Item.DreCodigo);
                var ueShortName = ue.TipoEscola.ShortName();
                resultado.Add(new ConsolidacaoAlfabetizacaoCriticaEscrita()
                {
                    DreCodigo = itemWithIndex.Item.DreCodigo,
                    UeCodigo = itemWithIndex.Item.UeCodigo,
                    DreNome = ue.Dre.PrefixoDoNomeAbreviado,
                    UeNome = $"{ueShortName} {ue.Nome}",
                    PercentualTotalAlunos = itemWithIndex.Item.PercentualNaoAlfabetizados,
                    TotalAlunosNaoAlfabetizados = itemWithIndex.Item.QuantidadeNaoAlfabetizados,
                    Posicao = itemWithIndex.Index + 1
                });
            }

            return resultado;
        }
    }
}