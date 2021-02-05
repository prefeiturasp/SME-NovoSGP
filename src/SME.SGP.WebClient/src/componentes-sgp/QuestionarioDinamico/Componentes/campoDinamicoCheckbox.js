import PropTypes from 'prop-types';
import React from 'react';
import { CheckboxComponent } from '~/componentes';

const CampoDinamicoCheckbox = props => {
  const { questaoAtual, form, label, desabilitado, onChange } = props;

  return (
    <div className="col-md-12 mb-3">
      <CheckboxComponent
        label={label}
        disabled={desabilitado}
        id={String(questaoAtual?.id)}
        name={String(questaoAtual?.id)}
        form={form}
        onChangeCheckbox={e => {
          const valorAtualSelecionado = e.target.checked;
          onChange(valorAtualSelecionado);
        }}
      />
    </div>
  );
};

CampoDinamicoCheckbox.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
  onChange: PropTypes.oneOfType([PropTypes.any]),
};

CampoDinamicoCheckbox.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
  onChange: () => {},
};

export default CampoDinamicoCheckbox;
