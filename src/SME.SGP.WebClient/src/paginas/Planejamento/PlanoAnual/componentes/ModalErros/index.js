import React, { useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { ModalMultiLinhas } from '~/componentes';

function ModalErros({ visivel, erros, onCloseErrosBimestre, bimestre }) {
  useEffect(() => {
    console.log(bimestre && `| ${String(bimestre.bimestre)}º Bimestre`);
  }, [bimestre]);
  return (
    <ModalMultiLinhas
      key="errosBimestre"
      visivel={visivel}
      onClose={() => onCloseErrosBimestre()}
      type="error"
      conteudo={erros}
      titulo={`Erros plano anual ${bimestre &&
        `| ${String(bimestre.bimestre)}º Bimestre`}`}
    />
  );
}

ModalErros.propTypes = {
  visivel: t.bool,
  bimestre: t.oneOfType([t.object, t.any]),
  erros: t.oneOfType([t.any]),
  onCloseErrosBimestre: t.func,
};

ModalErros.defaultProps = {
  visivel: false,
  bimestre: '',
  erros: [],
  onCloseErrosBimestre: () => {},
};

export default ModalErros;
