import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderPlanoAnual = ({ children }) => {
  const exibirLoaderPlanoAnual = useSelector(
    store => store.planoAnual.exibirLoaderPlanoAnual
  );

  return <Loader loading={exibirLoaderPlanoAnual}>{children}</Loader>;
};

LoaderPlanoAnual.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LoaderPlanoAnual.defaultProps = {
  children: () => {},
};

export default LoaderPlanoAnual;
