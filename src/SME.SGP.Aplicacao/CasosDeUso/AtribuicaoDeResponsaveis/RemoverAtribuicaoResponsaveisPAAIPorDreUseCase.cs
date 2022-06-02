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
    public class RemoverAtribuicaoResponsaveisPAAIPorDreUseCase : IRemoverAtribuicaoResponsaveisPAAIPorDreUseCase
    {
        private readonly IRepositorioSupervisorEscolaDre _repositorioSupervisorEscolaDre;
        private readonly IServicoEol _servicoEOL;
        private readonly IMediator _mediator;
        public RemoverAtribuicaoResponsaveisPAAIPorDreUseCase(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre, IServicoEol servicoEol, IMediator mediator)
        {
            _repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre;
            _servicoEOL = servicoEol;
            _mediator = mediator;
        }
        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var dre = param.ObterObjetoMensagem<string>();

                var paaiEscolasDres = await _repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(dre, TipoResponsavelAtribuicao.PAAI);
                var perfil = Perfis.PERFIL_PAAI;

                if (paaiEscolasDres.Any())
                {
                    var supervisoresIds = paaiEscolasDres.GroupBy(a => a.SupervisorId).Select(a => a.Key);
                    var funcionarios = await _servicoEOL.ObterFuncionariosPorPerfilDre(perfil, dre);

                    if (funcionarios != null && funcionarios.Any())
                    {
                        if (funcionarios.Count() != supervisoresIds.Count())
                            RemoverPAAISemAtribuicao(paaiEscolasDres, funcionarios);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível executar a remoção da atribuição de responsavel PAAI por DRE", LogNivel.Critico, LogContexto.RemoverAtribuicaoReponsavel, ex.Message));
                return false;
            }
        }

        private void RemoverPAAISemAtribuicao(IEnumerable<SupervisorEscolasDreDto> supervisoresEscolasDres, IEnumerable<UsuarioEolRetornoDto> supervisoresEol)
        {
            var supervisoresSemAtribuicao = supervisoresEscolasDres;

            if (supervisoresEol != null)
            {
                supervisoresSemAtribuicao = supervisoresEscolasDres
                    .Where(s => s.Tipo == (int)TipoResponsavelAtribuicao.PAAI &&
                        !supervisoresEol.Select(e => e.CodigoRf)
                    .Contains(s.SupervisorId));
            }

            if (supervisoresSemAtribuicao != null && supervisoresSemAtribuicao.Any())
            {
                foreach (var supervisor in supervisoresSemAtribuicao)
                {
                    if (supervisor.Tipo == (int)TipoResponsavelAtribuicao.PAAI)
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
