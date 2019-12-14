using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasRegistroPoa : IConsultasRegistroPoa
    {
        private readonly IRepositorioRegistroPoa repositorioRegistroPoa;

        public ConsultasRegistroPoa(IRepositorioRegistroPoa repositorioRegistroPoa)
        {
            this.repositorioRegistroPoa = repositorioRegistroPoa ?? throw new System.ArgumentNullException(nameof(repositorioRegistroPoa));
        }

        public RegistroPoaCompletoDto ObterPorId(long id)
        {
            var retorno = repositorioRegistroPoa.ObterPorId(id);

            if (retorno == null || retorno.Excluido)
                return null;

            return MapearParaDto(retorno);
        }

        private RegistroPoaCompletoDto MapearParaDto(RegistroPoa registroPoa)
        {
            return new RegistroPoaCompletoDto
            {
                AlteradoEm = registroPoa.AlteradoEm,
                AlteradoPor = registroPoa.AlteradoPor,
                AlteradoRF = registroPoa.AlteradoRF,
                CodigoRf = registroPoa.CodigoRf,
                CriadoEm = registroPoa.CriadoEm,
                CriadoPor = registroPoa.CriadoPor,
                CriadoRF = registroPoa.CriadoRF,
                Descricao = registroPoa.Descricao,
                DreId = registroPoa.DreId,
                Excluido = registroPoa.Excluido,
                Id = registroPoa.Id,
                Mes = registroPoa.Mes,
                Titulo = registroPoa.Titulo,
                UeId = registroPoa.UeId
            };
        }
    }
}