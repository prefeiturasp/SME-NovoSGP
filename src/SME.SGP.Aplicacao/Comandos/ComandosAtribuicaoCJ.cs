using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoCJ : IComandosAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto[] atribuicaoCJPersistenciaDtos)
        {
            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDtos)
            {
                var atribuicao = await repositorioAtribuicaoCJ.ObterPorComponenteTurmaModalidadeUe(atribuicaoDto.Modalidade, atribuicaoDto.TurmaId, atribuicaoDto.UeId, atribuicaoDto.ComponenteCurricularId);
                if (atribuicao != null)
                {
                    if (atribuicao.Substituir != atribuicaoDto.Substituir)
                    {
                        atribuicao.Substituir = atribuicaoDto.Substituir;
                        await repositorioAtribuicaoCJ.SalvarAsync(atribuicao);
                    }
                }
                else
                {
                    var novaAtribuicao = TransaformaDtoEmEntidade(atribuicaoDto);
                    await repositorioAtribuicaoCJ.SalvarAsync(novaAtribuicao);
                }
            }
        }

        private AtribuicaoCJ TransaformaDtoEmEntidade(AtribuicaoCJPersistenciaDto dto)
        {
            return new AtribuicaoCJ()
            {
                DreId = dto.DreId,
                Modalidade = dto.Modalidade,
                ProfessorRf = dto.UsuarioRf,
                Substituir = dto.Substituir,
                TurmaId = dto.TurmaId,
                UeId = dto.UeId,
                ComponenteCurricularId = dto.ComponenteCurricularId
            };
        }
    }
}