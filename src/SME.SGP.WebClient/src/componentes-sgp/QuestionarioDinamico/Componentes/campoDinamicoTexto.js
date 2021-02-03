import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch } from 'react-redux';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { CampoTexto } from '~/componentes';

const CampoDinamicoTexto = props => {
  const dispatch = useDispatch();

  const { questaoAtual, form, label, desabilitado } = props;

  return (
    <div className="col-md-12 mb-3">
      {label}
      <CampoTexto
        id={String(questaoAtual?.id)}
        name={String(questaoAtual?.id)}
        form={form}
        type="textarea"
        maxLength={999999}
        desabilitado={desabilitado}
        onChange={() => {
          dispatch(setQuestionarioDinamicoEmEdicao(true));
        }}
        minRowsTextArea="4"
      />
    </div>
  );
};

CampoDinamicoTexto.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
};

CampoDinamicoTexto.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
};

export default CampoDinamicoTexto;
