import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch } from 'react-redux';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { CampoData } from '~/componentes/campoData/campoData';
import { Base } from '~/componentes/colors';

const CampoDinamicoPeriodo = props => {
  const dispatch = useDispatch();

  const { questaoAtual, form, label, desabilitado } = props;

  const obterErroQuestaoAtual = () => {
    return form &&
      form?.touched[questaoAtual?.id] &&
      form?.errors[questaoAtual?.id]
      ? form.errors[questaoAtual?.id]
      : '';
  };

  const obterErroPorCampo = nomeCampo => {
    const nomeErro = obterErroQuestaoAtual();

    let textoErro = '';

    if (nomeErro) {
      const naoTemValorCampo = !form?.values?.[questaoAtual?.id]?.[nomeCampo];

      switch (nomeErro) {
        case 'OBRIGATORIO':
          if (naoTemValorCampo) {
            textoErro = 'Campo obrigatório';
          }
          break;
        case 'PERIODO_INICIO_MAIOR_QUE_FIM':
          if (nomeCampo === 'periodoInicio') {
            textoErro = 'Período início não pode ser maior que o fim';
          }
          break;

        default:
          break;
      }
    }

    if (textoErro) {
      return <span style={{ color: Base.Vermelho }}>{textoErro}</span>;
    }
    return '';
  };

  return (
    <div className="col-md-12 mb-3">
      {label}
      <div className="row">
        <div className="col-md-2">
          <CampoData
            form={form}
            id={`${questaoAtual?.id}.periodoInicio`}
            name={`${questaoAtual?.id}.periodoInicio`}
            placeholder="Início"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={valorData => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              form.setFieldTouched(questaoAtual?.id, true);
              form.setFieldValue(
                `${questaoAtual?.id}.periodoInicio`,
                valorData || ''
              );
            }}
            executarOnChangeExterno
            className={obterErroPorCampo('periodoInicio') ? 'is-invalid' : ''}
          />
          {obterErroPorCampo('periodoInicio')}
        </div>
        <span style={{ marginTop: 5 }}>à</span>
        <div className="col-md-2">
          <CampoData
            form={form}
            id={`${questaoAtual?.id}.periodoFim`}
            name={`${questaoAtual?.id}.periodoFim`}
            placeholder="Fim"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={valorData => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              form.setFieldTouched(questaoAtual?.id, true);
              form.setFieldValue(
                `${questaoAtual?.id}.periodoFim`,
                valorData || ''
              );
            }}
            executarOnChangeExterno
            className={obterErroPorCampo('periodoFim') ? 'is-invalid' : ''}
          />
          {obterErroPorCampo('periodoFim')}
        </div>
      </div>
    </div>
  );
};

CampoDinamicoPeriodo.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
};

CampoDinamicoPeriodo.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
};

export default CampoDinamicoPeriodo;
