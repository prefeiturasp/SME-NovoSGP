using MediatR;
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
    public class ConsultasSupervisor : IConsultasSupervisor
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
                                   IRepositorioAbrangencia repositorioAbrangencia,
                                   IServicoUsuario servicoUsuario,
                                   IRepositorioUeConsulta repositorioUe,
                                   IRepositorioCache repositorioCache,
                                   IMediator mediator)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var supervisoresEscolasDres = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dreId, TipoResponsavelAtribuicao.SupervisorEscolar);

            var listaRetorno = new List<ResponsavelEscolasDto>();

            await TratarRegistrosComResponsaveis(escolasPorDre, supervisoresEscolasDres, listaRetorno);
            TrataEscolasSemResponsaveis(escolasPorDre, listaRetorno);

            return listaRetorno;
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var responsaveisEscolasDres = await repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, supervisorId);

            IEnumerable<ResponsavelEscolasDto> lista = new List<ResponsavelEscolasDto>();

            if (responsaveisEscolasDres.Any())
                lista = await MapearResponsavelEscolaDre(responsaveisEscolasDres);

            return lista;
        }

        public async Task<IEnumerable<UnidadeEscolarResponsavelDto>> ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(string supervisoresId, string dreId, int tipoResponsavel)
        {
            if (string.IsNullOrEmpty(supervisoresId) || string.IsNullOrEmpty(dreId) || tipoResponsavel == 0)
                throw new NegocioException("Necessário informar o Código da DRE o Código do Responsável e o Tipo de responsável");

            var responsaveisEscolasDres = await repositorioSupervisorEscolaDre
                .ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(dreId, supervisoresId, tipoResponsavel);

            if (responsaveisEscolasDres.EhNulo() || !responsaveisEscolasDres.Any())
                return Enumerable.Empty<UnidadeEscolarResponsavelDto>();
            else
                return responsaveisEscolasDres;
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro)
        {
            if (string.IsNullOrEmpty(filtro.DreCodigo))
                throw new NegocioException("Necessário informar o Codigo da DRE");

            return await ListaDeAtribuicaoResponsavel(filtro);
        }

        private async Task<IEnumerable<ResponsavelEscolasDto>> ListaDeAtribuicaoResponsavel(FiltroObterSupervisorEscolasDto filtro)
        {
            var responsavelEscolaDreDto = await repositorioSupervisorEscolaDre
                .ObterTodosAtribuicaoResponsavelPorDreCodigo(filtro.DreCodigo);

            if (responsavelEscolaDreDto.EhNulo())
                responsavelEscolaDreDto = new List<SupervisorEscolasDreDto>() { new SupervisorEscolasDreDto() { EscolaId = filtro.UeCodigo } };

            var escolaDreDto = AdicionarTiposNaoExistente(responsavelEscolaDreDto, filtro);
            return await MapearResponsavelEscolaDre(escolaDreDto);
        }

        private List<SupervisorEscolasDreDto> AdicionarTiposNaoExistente(List<SupervisorEscolasDreDto> responsavelEscolaDreDto, FiltroObterSupervisorEscolasDto filtro)
        {
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                            .Cast<TipoResponsavelAtribuicao>()
                            .Select(d => new { codigo = (int)d })
                            .Select(x => x.codigo);

            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var listaCodigoSupervisor = filtro.SupervisorId.NaoEhNulo() ? filtro.SupervisorId.Split(",").ToArray() : new string[] { };
            var responsavelEscolaDreDtoComTiposNaoExistente = new List<SupervisorEscolasDreDto>();

            if (responsavelEscolaDreDto.Count > 0)
            {
                for (int i = 0; i < responsavelEscolaDreDto.Count; i++)
                {
                    var codUE = responsavelEscolaDreDto[i].UeId;
                    var supervisorEscolasDreDto = responsavelEscolaDreDto.Where(x => x.UeId == codUE);
                    var quantidadeTipos = supervisorEscolasDreDto.Select(t => t.TipoAtribuicao);
                    if (quantidadeTipos.Count() < tipos.Count())
                    {
                        var naotemTipo = tipos.Except(quantidadeTipos).ToList();

                        for (int n = 0; n < naotemTipo.Count; n++)
                        {
                            var registro = new SupervisorEscolasDreDto
                            {
                                AtribuicaoSupervisorId = supervisorEscolasDreDto.FirstOrDefault().AtribuicaoSupervisorId,
                                DreId = supervisorEscolasDreDto.FirstOrDefault().DreId,
                                EscolaId = supervisorEscolasDreDto.FirstOrDefault().EscolaId,
                                TipoAtribuicao = naotemTipo[n],
                                SupervisorId = null,
                                TipoEscola = supervisorEscolasDreDto.FirstOrDefault().TipoEscola,
                                UeNome = supervisorEscolasDreDto.FirstOrDefault().UeNome,
                                DreNome = supervisorEscolasDreDto.FirstOrDefault().DreNome,
                                AtribuicaoExcluida = true,
                                UeId = supervisorEscolasDreDto.FirstOrDefault().UeId,
                            };
                            responsavelEscolaDreDtoComTiposNaoExistente.Add(registro);
                        }
                    }
                }

                responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Concat(responsavelEscolaDreDto).ToList();

                responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(x => x.TipoAtribuicao != 0).ToList();

                if (!string.IsNullOrEmpty(filtro.UeCodigo) && !filtro.UESemResponsavel)
                    responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(x => x.UeId == filtro.UeCodigo).ToList();

                if (filtro.TipoCodigo > 0)
                    responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(x => x.TipoAtribuicao == filtro.TipoCodigo).ToList();
                else
                {
                    if (perfilAtual == Perfis.PERFIL_CEFAI)
                        responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(f => f.TipoAtribuicao.Equals((int)TipoResponsavelAtribuicao.PAAI)).ToList();
                    else if (perfilAtual == Perfis.PERFIL_COORDENADOR_NAAPA)
                        responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(f => f.TipoAtribuicao.Equals((int)TipoResponsavelAtribuicao.AssistenteSocial) ||
                                                                                     f.TipoAtribuicao.Equals((int)TipoResponsavelAtribuicao.PsicologoEscolar) ||
                                                                                     f.TipoAtribuicao.Equals((int)TipoResponsavelAtribuicao.Psicopedagogo)).ToList();
                }

                if (listaCodigoSupervisor.Any())
                    responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(x => listaCodigoSupervisor.Contains(x.SupervisorId) && !x.AtribuicaoExcluida).ToList();

                if (filtro.SupervisorId?.Length == 0 || filtro.SupervisorId.EhNulo() && filtro.UESemResponsavel || filtro.UESemResponsavel)
                    responsavelEscolaDreDtoComTiposNaoExistente = responsavelEscolaDreDtoComTiposNaoExistente.Where(x => x.AtribuicaoExcluida).ToList();
            }

            return responsavelEscolaDreDtoComTiposNaoExistente.OrderBy(x => x.TipoEscola).ThenBy(x => x.Nome).ToList();
        }

        private static void TrataEscolasSemResponsaveis(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, List<ResponsavelEscolasDto> listaRetorno)
        {
            if (listaRetorno.Count != escolasPorDre.Count())
            {
                var escolasComResponsavel = listaRetorno
                    .SelectMany(a => a.Escolas.Select(b => b.Codigo))
                    .ToList();

                var escolasSemResponsavel = escolasPorDre.Where(a => !escolasComResponsavel.Contains(a.Codigo)).ToList();

                var escolaResponsavelRetorno = new ResponsavelEscolasDto() { ResponsavelId = string.Empty, Responsavel = "NÃO ATRIBUÍDO" };

                var escolas = from t in escolasSemResponsavel
                              select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                escolaResponsavelRetorno.Escolas = escolas.ToList();

                listaRetorno.Add(escolaResponsavelRetorno);
            }
        }

        private async Task<IEnumerable<ResponsavelEscolasDto>> MapearResponsavelEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            ResponsavelRetornoDto listaResponsaveis = null;
            var listaRetorno = new List<ResponsavelEscolasDto>();

            var supervisor = supervisoresEscolasDres.ToList();
            var totalRegistros = supervisor.Count;
            for (int i = 0; i < totalRegistros; i++)
            {
                if (supervisor[i].SupervisorId.EhNulo())
                    listaResponsaveis = null;
                else
                    switch (supervisor[i].TipoAtribuicao)
                    {
                        case (int)TipoResponsavelAtribuicao.PsicologoEscolar:
                        case (int)TipoResponsavelAtribuicao.Psicopedagogo:
                        case (int)TipoResponsavelAtribuicao.AssistenteSocial:
                            {
                                var nomesFuncionariosAtribuidos = await mediator.Send(new ObterFuncionariosPorLoginsQuery(new List<string> { supervisor[i].SupervisorId }));
                                if (nomesFuncionariosAtribuidos.Any())
                                    listaResponsaveis = new ResponsavelRetornoDto() { CodigoRfOuLogin = nomesFuncionariosAtribuidos.FirstOrDefault().Login, NomeServidor = nomesFuncionariosAtribuidos.FirstOrDefault().NomeServidor };
                                break;
                            }
                        default:
                            {
                                var nomesServidoresAtribuidos = await mediator.Send(new ObterFuncionariosPorRFsQuery(new List<string> { supervisor[i].SupervisorId }));
                                if (nomesServidoresAtribuidos.Any())
                                    listaResponsaveis = new ResponsavelRetornoDto() { CodigoRfOuLogin = nomesServidoresAtribuidos.FirstOrDefault().CodigoRF, NomeServidor = nomesServidoresAtribuidos.FirstOrDefault().Nome };
                                break;
                            }
                    }
                string nomeResponsavel = listaResponsaveis != null ? listaResponsaveis?.NomeServidor + " - " + listaResponsaveis?.CodigoRfOuLogin
                                         : string.Empty;

                var itemRetorno = new ResponsavelEscolasDto()
                {
                    Id = supervisor[i].AtribuicaoSupervisorId,
                    Responsavel = supervisor[i].AtribuicaoExcluida ? null : nomeResponsavel,
                    ResponsavelId = supervisor[i].AtribuicaoExcluida ? null : supervisor[i].SupervisorId,
                    TipoResponsavel = ObterTipoResponsavelDescricao(supervisor[i].TipoAtribuicao),
                    TipoResponsavelId = supervisor[i].TipoAtribuicao,
                    UeNome = supervisor[i].Nome,
                    UeId = !string.IsNullOrEmpty(supervisor[i].UeId) ? supervisor[i].UeId : supervisor[i].EscolaId,
                    DreId = supervisor[i].DreId,
                    DreNome = supervisor[i].DreNome,
                    AlteradoEm = supervisor[i].AlteradoEm,
                    AlteradoPor = supervisor[i].AlteradoPor,
                    AlteradoRF = supervisor[i].AlteradoRF,
                    CriadoEm = supervisor[i].CriadoEm,
                    CriadoPor = supervisor[i].CriadoPor,
                    CriadoRF = supervisor[i].CriadoRF,
                };

                listaRetorno.Add(itemRetorno);
            }
            return listaRetorno;
        }

        private static string ObterTipoResponsavelDescricao(int tipo)
        {
            var tipoDescricao = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Where(w => (int)w == tipo)
                .Select(d => new { descricao = d.Name() })
                .FirstOrDefault()?.descricao;

            return tipoDescricao.NaoEhNulo() ? tipoDescricao : null;
        }

        private async Task TratarRegistrosComResponsaveis(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, List<ResponsavelEscolasDto> listaRetorno)
        {
            if (supervisoresEscolasDres.Any())
            {
                var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                var supervisores = await mediator.Send(new ObterSupervisorPorCodigoSupervisorQuery(supervisoresIds.ToArray()));

                if (supervisores.NaoEhNulo() && supervisores.Any())
                {
                    foreach (var supervisorEscolaDre in supervisoresIds)
                    {
                        var responsavelEscolasDto = new ResponsavelEscolasDto
                        {
                            Responsavel = supervisores.FirstOrDefault(a => a.CodigoRf == supervisorEscolaDre)?.NomeServidor,
                            ResponsavelId = supervisorEscolaDre
                        };

                        var idsEscolasDoSupervisor = supervisoresEscolasDres.Where(a => a.SupervisorId == supervisorEscolaDre)
                            .Select(a => a.EscolaId)
                            .ToList();

                        var escolas = from t in escolasPorDre
                                      where idsEscolasDoSupervisor.Contains(t.Codigo)
                                      select new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.NomeSimples };

                        responsavelEscolasDto.Escolas = escolas.ToList();

                        listaRetorno.Add(responsavelEscolasDto);
                    }
                }
            }
        }

        public async Task<IEnumerable<ListaUesConsultaAtribuicaoResponsavelDto>> ObterListaDeUesFiltroPrincipal(string dreCodigo)
        {
            var consulta = await repositorioSupervisorEscolaDre.ObterListaDeUesFiltroPrincipal(dreCodigo);

            return consulta.OrderBy(x => x.Nome);
        }

        public IEnumerable<TipoReponsavelRetornoDto> ObterTiposResponsaveis()
        {
            var perfilAtual = servicoUsuario
                .ObterPerfilAtual();

            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                            .Cast<TipoResponsavelAtribuicao>()
                            .Select(d => new TipoReponsavelRetornoDto
                            {
                                Codigo = (int)d,
                                Descricao = d.Name()
                            })
                            .OrderBy(x => x.Descricao)
                            .AsEnumerable();

            if (perfilAtual == Perfis.PERFIL_CEFAI)
                return tipos.Where(t => (TipoResponsavelAtribuicao)t.Codigo == TipoResponsavelAtribuicao.PAAI);
            else if (perfilAtual == Perfis.PERFIL_COORDENADOR_NAAPA)
            {
                var permitidos = new int[] { (int)TipoResponsavelAtribuicao.AssistenteSocial,
                                             (int)TipoResponsavelAtribuicao.PsicologoEscolar,
                                             (int)TipoResponsavelAtribuicao.Psicopedagogo };

                return tipos
                    .Where(t => permitidos.Contains(t.Codigo));
            }

            return tipos;
        }
    }
}