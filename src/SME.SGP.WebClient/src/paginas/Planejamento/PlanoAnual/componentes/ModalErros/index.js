import React from 'react';
import t from 'prop-types';

// Componentes
import { ModalMultiLinhas } from '~/componentes';

function ModalErros({ visivel, erros, onCloseErrosBimestre }) {
  return (
    <ModalMultiLinhas
      key="errosBimestre"
      visivel={visivel}
      onClose={() => onCloseErrosBimestre()}
      type="error"
      conteudo={erros}
      titulo="Erros plano anual"
    />
  );
}

ModalErros.propTypes = {
  visivel: t.bool,
  erros: t.oneOfType([t.any]),
  onCloseErrosBimestre: t.func,
};

ModalErros.defaultProps = {
  visivel: false,
  erros: [],
  onCloseErrosBimestre: () => {},
};

export default ModalErros;
