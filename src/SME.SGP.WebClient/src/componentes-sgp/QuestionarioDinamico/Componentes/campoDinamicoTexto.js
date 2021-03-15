import PropTypes from 'prop-types';
import React from 'react';
import { CampoTexto } from '~/componentes';

const CampoDinamicoTexto = props => {
  const { questaoAtual, form, label, desabilitado, onChange } = props;

  return (
    <div className="col-md-12 mb-3">
      {label}
      <CampoTexto
        id={String(questaoAtual?.id)}
        name={String(questaoAtual?.id)}
        form={form}
        type="textarea"
        maxLength={999999}
        desabilitado={desabilitado || questaoAtual.somenteLeitura}
        onChange={onChange}
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
  onChange: PropTypes.func,
};

CampoDinamicoTexto.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
  onChange: () => {},
};

export default CampoDinamicoTexto;
