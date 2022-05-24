using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreUseCase : AbstractUseCase, IObterSupervisoresPorDreUseCase
    {
        public ObterSupervisoresPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterNomesResponsaveisAtribuidos(IEnumerable<SupervisorEscolasDreDto> responsaveisAtribuidos,
            TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var listaNomesResponsaveisAtribuidos = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            if (responsaveisAtribuidos == null || !responsaveisAtribuidos.Any())
                return listaNomesResponsaveisAtribuidos;

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var nomesFuncionariosAtribuidos = await mediator.Send(new ObterListaNomePorListaLoginQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var funcionario in nomesFuncionariosAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    {
                        var nomesServidoresAtribuidos = await mediator.Send(new ObterListaNomePorListaRFQuery(responsaveisAtribuidos?.Select(c => c.SupervisorId)));

                        foreach (var supervisor in nomesServidoresAtribuidos)
                        {
                            listaNomesResponsaveisAtribuidos.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = supervisor.CodigoRF,
                                NomeServidor = supervisor.Nome
                            });
                        }

                        break;
                    }
            }

            if (listaNomesResponsaveisAtribuidos == null || !listaNomesResponsaveisAtribuidos.Any())
                throw new NegocioException("A API/EOL não retornou os nomes dos responsáveis atribuídos.");

            return await Task.FromResult(listaNomesResponsaveisAtribuidos);
        }

        private async Task<IEnumerable<ResponsavelRetornoDto>> ObterResponsaveisEol_CoreSSO(string dreCodigo, TipoResponsavelAtribuicao tipoResponsavelAtribuicao)
        {
            var listaResponsaveis = Enumerable.Empty<ResponsavelRetornoDto>().ToList();

            switch (tipoResponsavelAtribuicao)
            {
                case TipoResponsavelAtribuicao.PAAI:
                    {
                        var funcionariosEol = await mediator.Send(new ObterFuncionariosPorDreECargoQuery(dreCodigo, 29));

                        foreach (var funcionario in funcionariosEol)
                        {
                            listaResponsaveis.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.CodigoRf,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                case TipoResponsavelAtribuicao.PsicologoEscolar:
                case TipoResponsavelAtribuicao.Psicopedagogo:
                case TipoResponsavelAtribuicao.AssistenteSocial:
                    {
                        var perfil = Perfis.PERFIL_PSICOLOGO_ESCOLAR;

                        if (tipoResponsavelAtribuicao == TipoResponsavelAtribuicao.Psicopedagogo)
                            perfil = Perfis.PERFIL_PSICOPEDAGOGO;
                        else if (tipoResponsavelAtribuicao == TipoResponsavelAtribuicao.AssistenteSocial)
                            perfil = Perfis.PERFIL_ASSISTENTE_SOCIAL;

                        var funcionariosUnidades = (await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(dreCodigo,
                            new List<Guid> { perfil }))).ToList();

                        foreach (var funcionario in funcionariosUnidades)
                        {
                            listaResponsaveis.Add(new ResponsavelRetornoDto()
                            {
                                CodigoRf_Login = funcionario.Login,
                                NomeServidor = funcionario.NomeServidor
                            });
                        }

                        break;
                    }
                default:
                    var supervisoresEol = (await mediator.Send(new ObterSupervisoresPorDreEolQuery(dreCodigo))).ToList();

                    foreach (var supervisor in supervisoresEol)
                    {
                        listaResponsaveis.Add(new ResponsavelRetornoDto()
                        {
                            CodigoRf_Login = supervisor.CodigoRf,
                            NomeServidor = supervisor.NomeServidor
                        });
                    }

                    break;
            }

            return await Task.FromResult(listaResponsaveis);
        }

        public async Task<IEnumerable<SupervisorDto>> Executar(ObterSupervisoresPorDreDto filtro)
        {
            var lstSupervisores = new List<SupervisorDto>();

            if ((int)filtro.TipoResponsavelAtribuicao == 0)
                return lstSupervisores;

            var responsaveisEol_CoreSSO = await ObterResponsaveisEol_CoreSSO(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao);

            //-> Obtem os resposáveis já atribuidos
            var responsaveisAtribuidos = (await mediator.Send(new ObterSupervisoresPorDreQuery(filtro.DreCodigo, filtro.TipoResponsavelAtribuicao)))
                .DistinctBy(c => c.SupervisorId);

            var nomesResponsaveisAtribuidos = await ObterNomesResponsaveisAtribuidos(responsaveisAtribuidos, filtro.TipoResponsavelAtribuicao);

            if (responsaveisEol_CoreSSO != null && responsaveisEol_CoreSSO.Any())
            {
                lstSupervisores.AddRange(responsaveisEol_CoreSSO?.Select(a => new SupervisorDto()
                {
                    SupervisorId = a.CodigoRf_Login,
                    SupervisorNome = a.NomeServidor
                }));
            }

            if (responsaveisAtribuidos != null && responsaveisAtribuidos.Any())
            {
                lstSupervisores.AddRange(responsaveisAtribuidos?.Where(s => !responsaveisEol_CoreSSO.Select(se => se.CodigoRf_Login).Contains(s.SupervisorId))?
                    .Select(a => new SupervisorDto()
                    {
                        SupervisorId = a.SupervisorId,
                        SupervisorNome = nomesResponsaveisAtribuidos?.FirstOrDefault(n => n.CodigoRf_Login == a.SupervisorId)?.NomeServidor
                    }));
            }

            return lstSupervisores?.OrderBy(s => s.SupervisorNome).ThenBy(c => c.SupervisorId);
        }
    }
}
