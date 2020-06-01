using System;

namespace SME.SGP.Aplicacao.Queries.Usuario.ObterUsuarioPossuiPermissaoNaTurmaEDisciplina
{
    public class ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery
    {
        public ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(string componenteCurricularId, DateTime data, Dominio.Usuario usuario)
        {
            //TODO VALIDAR CAMPOS

            ComponenteCurricularId = componenteCurricularId;
            Data = data;
            Usuario = usuario;
        }

        public string ComponenteCurricularId { get; set; }
        public DateTime Data { get; set; }
        public Dominio.Usuario Usuario { get; set; }
    }
}
