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
    public class ConsultasSupervisor : IConsultasSupervisor
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IRepositorioCache repositorioCache;

        public ConsultasSupervisor(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
            IServicoEol servicoEOL,
            IRepositorioAbrangencia repositorioAbrangencia,
            IServicoUsuario servicoUsuario,
            IRepositorioUeConsulta repositorioUe, IRepositorioCache repositorioCache)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var supervisoresEscolasDres = await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dreId, TipoResponsavelAtribuicao.SupervisorEscolar);

            var listaRetorno = new List<ResponsavelEscolasDto>();

            TratarRegistrosComResponsaveis(escolasPorDre, supervisoresEscolasDres, listaRetorno);
            TrataEscolasSemResponsaveis(escolasPorDre, listaRetorno);

            return listaRetorno;
        }

        public async Task<IEnumerable<ResponsavelEscolasDto>> ObterPorDreESupervisor(string supervisorId, string dreId)
        {
            var responsaveisEscolasDres = await repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, supervisorId);

            IEnumerable<ResponsavelEscolasDto> lista = new List<ResponsavelEscolasDto>();

            if (responsaveisEscolasDres.Any())
                lista = MapearResponsavelEscolaDre(responsaveisEscolasDres).ToList();

            return lista;
        }

        public IEnumerable<UnidadeEscolarResponsavelDto> ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(string supervisoresId, string dreId)
        {
            if (string.IsNullOrEmpty(supervisoresId) || string.IsNullOrEmpty(dreId))
                throw new NegocioException("Necessário informar o Código da DRE e o Código do Responsável");

            var responsaveisEscolasDres = repositorioSupervisorEscolaDre.ObterUesAtribuidasAoResponsavelPorSupervisorIdeDre(dreId, supervisoresId);

            if (responsaveisEscolasDres == null || !responsaveisEscolasDres.Any())
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
            var responsavelEscolaDreDto = await repositorioSupervisorEscolaDre.ObterAtribuicaoResponsavel(filtro);

            if (responsavelEscolaDreDto == null)
                responsavelEscolaDreDto = new List<SupervisorEscolasDreDto>() { new SupervisorEscolasDreDto() { EscolaId = filtro.UeCodigo } };


            responsavelEscolaDreDto = await AdicionarTiposNaoExistente(responsavelEscolaDreDto.ToList(), filtro);
            return MapearResponsavelEscolaDre(responsavelEscolaDreDto);
        }

        private async Task<List<SupervisorEscolasDreDto>> AdicionarTiposNaoExistente(List<SupervisorEscolasDreDto> responsavelEscolaDreDto, FiltroObterSupervisorEscolasDto filtro)
        {
            var agrupamentoUe = responsavelEscolaDreDto.GroupBy(x => x.EscolaId).ToList();
            var tipos = Enum.GetValues(typeof(TipoResponsavelAtribuicao)).Cast<TipoResponsavelAtribuicao>().Select(d => new { codigo = (int)d }).Select(x => x.codigo);
            if (agrupamentoUe.Count > 0)
            {
                for (int i = 0; i < agrupamentoUe.Count; i++)
                {

                    var itemTipo = agrupamentoUe[i].Select(e => e.TipoAtribuicao);
                    var naotemTipo = tipos.Except(itemTipo).ToList();

                    for (int n = 0; n < naotemTipo.Count; n++)
                    {
                        var registro = new SupervisorEscolasDreDto
                        {
                            Id = agrupamentoUe[i].FirstOrDefault().Id,
                            DreId = agrupamentoUe[i].FirstOrDefault().DreId,
                            EscolaId = agrupamentoUe[i].FirstOrDefault().EscolaId,
                            TipoAtribuicao = naotemTipo[n],
                            SupervisorId = null,
                            TipoEscola = agrupamentoUe[i].FirstOrDefault().TipoEscola,
                            UeNome = agrupamentoUe[i].FirstOrDefault().UeNome,
                            DreNome = agrupamentoUe[i].FirstOrDefault().DreNome,
                            AtribuicaoExcluida = true,
                            UeId = agrupamentoUe[i].FirstOrDefault().UeId
                        };
                        responsavelEscolaDreDto.Add(registro);
                    }
                }
            }
            else
            {
                if (filtro.TipoCodigo == null && string.IsNullOrEmpty(filtro.UeCodigo) && string.IsNullOrEmpty(filtro.SupervisorId) && !string.IsNullOrEmpty(filtro.DreCodigo))
                {
                    var registroSemAtribuicao = await repositorioSupervisorEscolaDre.ObterTodosAtribuicaoResponsavelPorDreCodigo(filtro.DreCodigo);
                    if (registroSemAtribuicao.Count == 0)
                    {
                        var registro = new SupervisorEscolasDreDto
                        {
                            Id = 0,
                            DreId = filtro.DreCodigo,
                            EscolaId = filtro.UeCodigo,
                            TipoAtribuicao = (int)filtro.TipoCodigo,
                            SupervisorId = null,
                            AtribuicaoExcluida = true,
                            UeId = filtro.UeCodigo
                        };
                        responsavelEscolaDreDto.Add(registro);
                    }
                    else
                    {
                        for (int r = 0; r < registroSemAtribuicao.Count(); r++)
                        {
                            for (int t = 0; t < tipos.ToList().Count; t++)
                            {
                                var registro = new SupervisorEscolasDreDto
                                {
                                    Id = registroSemAtribuicao[r].Id,
                                    DreId = registroSemAtribuicao[r].DreId,
                                    EscolaId = registroSemAtribuicao[r].EscolaId,
                                    TipoAtribuicao = tipos.ToList()[t],
                                    SupervisorId = null,
                                    AtribuicaoExcluida = true,
                                    UeId = registroSemAtribuicao[r].UeId
                                };
                                responsavelEscolaDreDto.Add(registro);
                            }
                        }
                    }
                }
            }
            return responsavelEscolaDreDto;
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

        private IEnumerable<ResponsavelEscolasDto> MapearResponsavelEscolaDre(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres)
        {
            ResponsavelRetornoDto listaResponsaveis = null;

            foreach (var supervisor in supervisoresEscolasDres)
            {
                switch (supervisor.TipoAtribuicao)
                {
                    case (int)TipoResponsavelAtribuicao.PsicologoEscolar:
                    case (int)TipoResponsavelAtribuicao.Psicopedagogo:
                    case (int)TipoResponsavelAtribuicao.AssistenteSocial:
                        {
                            var nomesFuncionariosAtribuidos = servicoEOL.ObterListaNomePorListaLogin(new List<string> { supervisor.SupervisorId }).Result;
                            if (nomesFuncionariosAtribuidos.Any())
                                listaResponsaveis = new ResponsavelRetornoDto() { CodigoRfOuLogin = nomesFuncionariosAtribuidos.FirstOrDefault().Login, NomeServidor = nomesFuncionariosAtribuidos.FirstOrDefault().NomeServidor };
                            break;
                        }
                    default:
                        {
                            var nomesServidoresAtribuidos = servicoEOL.ObterListaNomePorListaRF(new List<string> { supervisor.SupervisorId }).Result;
                            if (nomesServidoresAtribuidos.Any())
                                listaResponsaveis = new ResponsavelRetornoDto() { CodigoRfOuLogin = nomesServidoresAtribuidos.FirstOrDefault().CodigoRF, NomeServidor = nomesServidoresAtribuidos.FirstOrDefault().Nome };
                            break;
                        }
                }

                string nomeResponsavel = listaResponsaveis != null ? listaResponsaveis.NomeServidor + " - " + listaResponsaveis.CodigoRfOuLogin
                                         : string.Empty;


                yield return new ResponsavelEscolasDto()
                {
                    Id = supervisor.Id,
                    Responsavel = supervisor.AtribuicaoExcluida ? null : nomeResponsavel,
                    ResponsavelId = supervisor.AtribuicaoExcluida ? null : supervisor.SupervisorId,
                    TipoResponsavel = ObterTipoResponsavelDescricao(supervisor.TipoAtribuicao),
                    TipoResponsavelId = supervisor.TipoAtribuicao,
                    UeNome = $"{supervisor.TipoEscola.ShortName()} {supervisor.UeNome}",
                    UeId = supervisor.UeId,
                    DreId = supervisor.DreId,
                    DreNome = supervisor.DreNome,
                    AlteradoEm = supervisor.AlteradoEm,
                    AlteradoPor = supervisor.AlteradoPor,
                    AlteradoRF = supervisor.AlteradoRF,
                    CriadoEm = supervisor.CriadoEm,
                    CriadoPor = supervisor.CriadoPor,
                    CriadoRF = supervisor.CriadoRF,
                };
            }
        }

        private static string ObterTipoResponsavelDescricao(int tipo)
        {
            var tipoDescricao = Enum.GetValues(typeof(TipoResponsavelAtribuicao))
                .Cast<TipoResponsavelAtribuicao>()
                .Where(w => (int)w == tipo)
                .Select(d => new { descricao = d.Name() })
                .FirstOrDefault()?.descricao;

            return tipoDescricao != null ? tipoDescricao : null;
        }

        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.Id,
                Excluido = dto.AtribuicaoExcluida,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.TipoAtribuicao
            };
        }

        private void TratarRegistrosComResponsaveis(IEnumerable<AbrangenciaUeRetorno> escolasPorDre, IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, List<ResponsavelEscolasDto> listaRetorno)
        {
            if (supervisoresEscolasDres.Any())
            {
                var supervisoresIds = supervisoresEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                var supervisores = servicoEOL.ObterSupervisoresPorCodigo(supervisoresIds.ToArray());

                if (supervisores != null && supervisores.Any())
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

        private void RemoverSupervisorSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres,
            IEnumerable<SupervisoresRetornoDto> supervisoresEol)
        {
            var supervisoresSemAtribuicao = supervisoresEscolasDres;

            if (supervisoresEol != null)
            {
                supervisoresSemAtribuicao = supervisoresEscolasDres
                    .Where(s => s.TipoAtribuicao == (int)TipoResponsavelAtribuicao.SupervisorEscolar &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (supervisoresSemAtribuicao != null && supervisoresSemAtribuicao.Any())
            {
                foreach (var supervisor in supervisoresSemAtribuicao)
                {
                    if (supervisor.TipoAtribuicao == (int)TipoResponsavelAtribuicao.SupervisorEscolar)
                    {
                        var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                        supervisorEntidadeExclusao.Excluir();
                        repositorioSupervisorEscolaDre.Salvar(supervisorEntidadeExclusao);
                    }
                }
            }
        }
    }
}