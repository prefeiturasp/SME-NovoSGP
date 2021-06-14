import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';

const LoaderEncaminhamento = ({ children }) => {
  const exibirLoaderEncaminhamentoAEE = useSelector(
    store => store.encaminhamentoAEE.exibirLoaderEncaminhamentoAEE
  );

  return <Loader loading={exibirLoaderEncaminhamentoAEE}>{children}</Loader>;
};

LoaderEncaminhamento.propTypes = {
  children: PropTypes.oneOfType([PropTypes.element, PropTypes.func]),
};

LoaderEncaminhamento.defaultProps = {
  children: () => {},
};

export default LoaderEncaminhamento;
