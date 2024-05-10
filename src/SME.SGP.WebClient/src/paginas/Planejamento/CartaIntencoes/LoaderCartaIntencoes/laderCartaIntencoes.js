import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderCartaIntencoes = ({ children }) => {
  const carregandoCartaIntencoes = useSelector(
    store => store.cartaIntencoes.carregandoCartaIntencoes
  );

  return <Loader loading={carregandoCartaIntencoes}>{children}</Loader>;
};

LoaderCartaIntencoes.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LoaderCartaIntencoes.defaultProps = {
  children: () => {},
};

export default LoaderCartaIntencoes;
