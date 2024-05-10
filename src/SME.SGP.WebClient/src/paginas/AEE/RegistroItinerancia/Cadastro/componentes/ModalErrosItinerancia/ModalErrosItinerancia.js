import React from 'react';
import PropTypes from 'prop-types';
import { ModalMultiLinhas } from '~/componentes';

const ModalErrosItinerancia = ({ erros, modalVisivel, setModalVisivel }) => {
  return (
    <ModalMultiLinhas
      key="erros-plano-anual"
      visivel={modalVisivel}
      onClose={() => setModalVisivel(false)}
      type="error"
      conteudo={erros}
      titulo="Erros registro de itinerância"
    />
  );
};

ModalErrosItinerancia.defaultProps = {
  modalVisivel: false,
  setModalVisivel: () => {},
  erros: [],
};

ModalErrosItinerancia.propTypes = {
  modalVisivel: PropTypes.bool,
  setModalVisivel: PropTypes.func,
  erros: PropTypes.oneOfType([PropTypes.any]),
};

export default ModalErrosItinerancia;
