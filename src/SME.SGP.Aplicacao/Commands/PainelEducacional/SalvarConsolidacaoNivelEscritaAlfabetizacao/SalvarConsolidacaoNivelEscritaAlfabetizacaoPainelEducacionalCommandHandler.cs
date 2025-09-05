using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.Sondagem;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNivelEscritaAlfabetizacao
{
    public class SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandler : IRequestHandler<SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand, bool>
    {
        private readonly IRepositorioConsolidacaoAlfabetizacaoNivelEscrita repositorioConsolidacaoAlfabetizacaoNivelEscrita;
        public SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommandHandler(IRepositorioConsolidacaoAlfabetizacaoNivelEscrita repositorioConsolidacaoAlfabetizacaoNivelEscrita)
        {
            this.repositorioConsolidacaoAlfabetizacaoNivelEscrita = repositorioConsolidacaoAlfabetizacaoNivelEscrita;
        }
        public async Task<bool> Handle(SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand request, CancellationToken cancellationToken)
        {
            if (request is null || request.ConsolidacaoNivelEscritaAlfabetizacao is null || request.ConsolidacaoNivelEscritaAlfabetizacao.Count() == 0)
                return false;

            try
            {
                await repositorioConsolidacaoAlfabetizacaoNivelEscrita.ExcluirConsolidacaoNivelEscrita();
                foreach (var consolidacaoNivelEscrita in request.ConsolidacaoNivelEscritaAlfabetizacao)
                {
                    await repositorioConsolidacaoAlfabetizacaoNivelEscrita
                        .SalvarConsolidacaoNivelEscrita(MapearDtoParaEntidade(consolidacaoNivelEscrita));
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar consolidação do nível de escrita/alfabetização. {ex.Message}");
            }
        }

        private static ConsolidacaoAlfabetizacaoNivelEscrita MapearDtoParaEntidade(SondagemConsolidacaoNivelEscritaAlfabetizacaoDto dto) => 
            new ConsolidacaoAlfabetizacaoNivelEscrita
        {
            DreCodigo = dto.DreCodigo,
            NivelEscrita = dto.NivelEscrita,
            Periodo = dto.Periodo,
            Quantidade = dto.QuantidadeAlunos,
            UeCodigo = dto.UeCodigo,
            AnoLetivo = short.Parse(dto.AnoLetivo)
        };
    }
}
