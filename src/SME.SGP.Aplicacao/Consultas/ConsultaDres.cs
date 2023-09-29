using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaDres : IConsultaDres
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultaDres(IServicoEol servicoEOL,
            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
            IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario)

        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var lista = from a in escolasPorDre
                        select new UnidadeEscolarDto()
                        {
                            Codigo = a.Codigo,
                            Nome = a.NomeSimples
                        };

            return lista;
        }

        public async Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasSemAtribuicao(string dreId, int tipoResponsavel)
        {
            var uesParaAtribuicao = await repositorioSupervisorEscolaDre.ObterListaUEsParaNovaAtribuicaoPorCodigoDre(dreId);
            if (!uesParaAtribuicao.Any())
                return new List<UnidadeEscolarDto>() { new UnidadeEscolarDto() };

            var listaUesParaAtribuicao = AdicionarTiposNaoExistente(uesParaAtribuicao.ToList(), tipoResponsavel);

            return TrataEscolasSemSupervisores(listaUesParaAtribuicao);
        }

        private List<UnidadeEscolarSemAtribuicaolDto> AdicionarTiposNaoExistente(List<UnidadeEscolarSemAtribuicaolDto> uesParaAtribuicao, int tipoResponsavel)
        {
            var listaDeRemovida = new List<UnidadeEscolarSemAtribuicaolDto>();
            var novaLista = new List<UnidadeEscolarSemAtribuicaolDto>();
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao)).Cast<TipoResponsavelAtribuicao>()
            .Select(d => new { codigo = (int)d }).Select(x => x.codigo);

            foreach (var ue in uesParaAtribuicao.ToList())
            {
                var codUE = ue.Codigo;
                var uesParaAtribuicaoDto = uesParaAtribuicao.Where(x => x.Codigo == codUE).ToList();
                var quantidadeTipos = uesParaAtribuicaoDto.Select(t => (int)t.TipoAtribuicao).Distinct();
                if (quantidadeTipos.Count() < tipos.Count())
                {
                    var naotemTipo = tipos.Except(quantidadeTipos).ToList();
                    foreach (var tipo in naotemTipo)
                    {
                        var registro = new UnidadeEscolarSemAtribuicaolDto
                        {
                            Codigo = ue.Codigo,
                            UeNome = ue.UeNome,
                            TipoEscola = ue.TipoEscola,
                            TipoAtribuicao = (TipoResponsavelAtribuicao)tipo,
                            AtribuicaoExcluida = true
                        };
                        uesParaAtribuicao.Add(registro);
                    }
                }
                var listaAtribuicaoRemover = uesParaAtribuicaoDto.FindAll(atribuicao => atribuicao.TipoAtribuicao == (TipoResponsavelAtribuicao)tipoResponsavel);
                if (listaAtribuicaoRemover.Count > 1 && listaAtribuicaoRemover.Exists(atribuicao => !atribuicao.AtribuicaoExcluida))
                    listaDeRemovida.AddRange(listaAtribuicaoRemover.FindAll(atribuicao => atribuicao.AtribuicaoExcluida));
            }

            foreach (var atribuicao in listaDeRemovida)
                uesParaAtribuicao.Remove(atribuicao);

            var retorno = uesParaAtribuicao.Where(x => x.TipoAtribuicao == (TipoResponsavelAtribuicao)tipoResponsavel && x.AtribuicaoExcluida);
            return retorno.OrderBy(x => x.Nome).ToList();
        }

        public async Task<IEnumerable<DreConsultaDto>> ObterTodos()
        {
            var respostaEol = await servicoEOL.ObterDres();

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

        private IEnumerable<UnidadeEscolarDto> TrataEscolasSemSupervisores(IEnumerable<UnidadeEscolarSemAtribuicaolDto> uesParaAtribuicao)
        {
            foreach (var item in uesParaAtribuicao)
            {
                yield return new UnidadeEscolarDto
                {
                    Codigo = item.Codigo,
                    Nome = item.Nome
                };
            }
        }
    }
}