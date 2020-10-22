import moment from 'moment';
import modalidadeDto from '~/dtos/modalidade';
import modalidadeTipoCalendario from '~/dtos/modalidadeTipoCalendario';
import api from '~/servicos/api';

class ServicoCalendarios {
  obterTiposCalendario = async anoLetivo => {
    if (!anoLetivo) anoLetivo = moment().year();

    return api
      .get(`v1/calendarios/tipos/anos/letivos/${anoLetivo}`)
      .then(resposta => resposta)
      .catch(() => []);
  };

  converterModalidade = modalidadeCalendario => {
    let modalidade = modalidadeDto.FUNDAMENTAL;
    if (modalidadeCalendario === modalidadeTipoCalendario.EJA) {
      modalidade = modalidadeDto.EJA;
    } else if (modalidadeCalendario === modalidadeTipoCalendario.Infantil) {
      modalidade = modalidadeDto.INFANTIL;
    }
    return modalidade;
  };

  gerarRelatorio = payload => {
    return api.post('v1/relatorios/calendarios/impressao', payload);
  };

  obterTiposCalendarioAutoComplete = (descricao = '') => {
    return api.get(`v1/calendarios/tipos/anos-letivos?descricao=${descricao}`);
  };

  obterDatasDeAulasDisponiveis = (
    anoLetivo,
    turma,
    codigoComponenteCurricular
  ) => {
    const url = `v1/calendarios/frequencias/aulas/datas/${anoLetivo}/turmas/${turma}/disciplinas/${codigoComponenteCurricular}`;
    return api.get(url);
  };
}

export default new ServicoCalendarios();
