import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderAcompanhamentoAprendizagem = ({ children }) => {
  const exibirLoaderGeralAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .exibirLoaderGeralAcompanhamentoAprendizagem
  );

  return (
    <Loader loading={exibirLoaderGeralAcompanhamentoAprendizagem}>
      {children}
    </Loader>
  );
};

LoaderAcompanhamentoAprendizagem.propTypes = {
  children: PropTypes.oneOfType([PropTypes.any]),
};

LoaderAcompanhamentoAprendizagem.defaultProps = {
  children: () => {},
};

export default LoaderAcompanhamentoAprendizagem;
