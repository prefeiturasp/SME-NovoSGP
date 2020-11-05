import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderConselhoClasse = ({ children }) => {
  const exibirLoaderGeralConselhoClasse = useSelector(
    store => store.conselhoClasse.exibirLoaderGeralConselhoClasse
  );

  return <Loader loading={exibirLoaderGeralConselhoClasse}>{children}</Loader>;
};

LoaderConselhoClasse.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LoaderConselhoClasse.defaultProps = {
  children: () => {},
};

export default LoaderConselhoClasse;
