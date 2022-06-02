using SME.SGP.Aplicacao.Interfaces;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoResponsaveisASPPPorDreUseCase : IRemoverAtribuicaoResponsaveisASPPPorDreUseCase
    {
        private readonly IRepositorioSupervisorEscolaDre _repositorioSupervisorEscolaDre;
        private readonly IServicoEol _servicoEOL;
        private readonly IMediator _mediator;
        public RemoverAtribuicaoResponsaveisASPPPorDreUseCase(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre, IServicoEol servicoEOL, IMediator mediator)
        {
            _repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre;
            _servicoEOL = servicoEOL;
            _mediator = mediator;
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var dre = param.ObterObjetoMensagem<string>();
                var assitenteSocialEscolasDres = await _repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dre, TipoResponsavelAtribuicao.AssistenteSocial);
                var psicologosEscolasDres = await _repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dre, TipoResponsavelAtribuicao.PsicologoEscolar);
                var psicopedagogosEscolasDres = await _repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dre, TipoResponsavelAtribuicao.Psicopedagogo);

                var funcionariosASPP = new List<SupervisorEscolasDreDto>();

                funcionariosASPP.AddRange(assitenteSocialEscolasDres);
                funcionariosASPP.AddRange(psicologosEscolasDres);
                funcionariosASPP.AddRange(psicopedagogosEscolasDres);

                if (funcionariosASPP.Any())
                {
                    var funcionarios = new List<UsuarioEolRetornoDto>();

                    var supervisoresIds = funcionariosASPP.GroupBy(a => a.SupervisorId).Select(a => a.Key);

                    var funcionariosPsicoloEscolar = await _servicoEOL.ObterUsuarioFuncionarioCoreSSO(Perfis.PERFIL_PSICOLOGO_ESCOLAR, dre);
                    var funcionariosPsicoPedagogos = await _servicoEOL.ObterUsuarioFuncionarioCoreSSO(Perfis.PERFIL_PSICOPEDAGOGO, dre);
                    var funcionariosAssistenteSocial = await _servicoEOL.ObterUsuarioFuncionarioCoreSSO(Perfis.PERFIL_ASSISTENTE_SOCIAL, dre);

                    funcionarios.AddRange(funcionariosPsicoloEscolar);
                    funcionarios.AddRange(funcionariosPsicoPedagogos);
                    funcionarios.AddRange(funcionariosAssistenteSocial);

                    if (funcionarios != null && funcionarios.Any())
                    {
                        if (funcionarios.Count() != supervisoresIds.Count())
                            RemoverASPPCoreSSoSemAtribuicao(funcionariosASPP, funcionarios);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel do Core SSO (Assistente social, Psicopedagogo, Psicologo) por DRE", LogNivel.Critico, LogContexto.RemoverAtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private void RemoverASPPCoreSSoSemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<UsuarioEolRetornoDto> supervisoresEol)
        {
            var listaAsspSemAtribuicao = new List<SupervisorEscolasDreDto>();

            if (supervisoresEol != null)
            {
                var assitenteSocialEscolas = supervisoresEscolasDres.Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.AssistenteSocial && !supervisoresEol.Select(e => e.UsuarioId.ToString()).Contains(s.SupervisorId));
                var psicologosEscolas = supervisoresEscolasDres.Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.PsicologoEscolar && !supervisoresEol.Select(e => e.UsuarioId.ToString()).Contains(s.SupervisorId));
                var psicopedagogosEscolas = supervisoresEscolasDres.Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.Psicopedagogo && !supervisoresEol.Select(e => e.UsuarioId.ToString()).Contains(s.SupervisorId));

                listaAsspSemAtribuicao.AddRange(assitenteSocialEscolas);
                listaAsspSemAtribuicao.AddRange(psicologosEscolas);
                listaAsspSemAtribuicao.AddRange(psicopedagogosEscolas);
            }

            if (listaAsspSemAtribuicao != null && listaAsspSemAtribuicao.Any())
            {
                foreach (var supervisor in listaAsspSemAtribuicao)
                {
                    if (supervisor.Tipo == (int)TipoResponsavelAtribuicao.AssistenteSocial ||
                        supervisor.Tipo == (int)TipoResponsavelAtribuicao.PsicologoEscolar ||
                        supervisor.Tipo == (int)TipoResponsavelAtribuicao.Psicopedagogo)
                    {
                        var supervisorEntidadeExclusao = MapearDtoParaEntidade(supervisor);
                        supervisorEntidadeExclusao.Excluir();
                        _repositorioSupervisorEscolaDre.Salvar(supervisorEntidadeExclusao);
                    }
                }
            }
        }
        private static SupervisorEscolaDre MapearDtoParaEntidade(SupervisorEscolasDreDto dto)
        {
            return new SupervisorEscolaDre()
            {
                DreId = dto.DreId,
                SupervisorId = dto.SupervisorId,
                EscolaId = dto.EscolaId,
                Id = dto.Id,
                Excluido = dto.Excluido,
                AlteradoEm = dto.AlteradoEm,
                AlteradoPor = dto.AlteradoPor,
                AlteradoRF = dto.AlteradoRF,
                CriadoEm = dto.CriadoEm,
                CriadoPor = dto.CriadoPor,
                CriadoRF = dto.CriadoRF,
                Tipo = dto.Tipo
            };
        }
    }
}
