using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultaDres : IConsultaDres
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEOL servicoEOL;

        public ConsultaDres(IServicoEOL servicoEOL, IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public IEnumerable<UnidadeEscolarDto> ObterEscolasSemAtribuicao(string dreId)
        {
            var escolasPorDre = servicoEOL.ObterEscolasPorDre(dreId);

            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemSupervisoresEscola(dreId, string.Empty);

            return TrataEscolasSemSupervisores(escolasPorDre, supervisoresEscolasDres);
        }

        public IEnumerable<DreConsultaDto> ObterTodos()
        {
            var respostaEol = servicoEOL.ObterDres();

            return MapearParaDto(respostaEol);
        }

        private IEnumerable<DreConsultaDto> MapearParaDto(IEnumerable<DreRespostaEolDto> respostaEol)
        {
            foreach (var item in respostaEol)
            {
                yield return new DreConsultaDto()
                {
                    Id = item.CodigoDRE,
                    Nome = item.NomeDRE,
                    Sigla = item.SiglaDRE
                };
            }
        }

        private IEnumerable<UnidadeEscolarDto> TrataEscolasSemSupervisores(IEnumerable<EscolasRetornoDto> escolasPorDre,
                    IEnumerable<SupervisorEscolasDreDto> supervisoresEscolas)
        {
            var escolasComSupervisor = supervisoresEscolas
                .Select(a => a.EscolaId)
                .ToList();

            var escolasSemSupervisor = escolasPorDre
                .Where(a => !escolasComSupervisor.Contains(a.CodigoEscola))
                .ToList();

            return from t in escolasSemSupervisor
                   select new UnidadeEscolarDto() { Codigo = t.CodigoEscola, Nome = t.NomeEscola };
        }
    }
}