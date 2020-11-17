import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { ContainerPaginacao } from './paginacao.css';

const Paginacao = props => {
  const { numeroRegistros, onChangePaginacao } = props;

  const [paginaAtual, setPaginaAtual] = useState({
    defaultCurrent: 1,
    total: numeroRegistros,
    current: 1,
    pageSize: 5,
  });

  const executaPaginacao = pagina => {
    onChangePaginacao(pagina);
    const novaPagina = { ...paginaAtual };
    novaPagina.current = pagina;
    setPaginaAtual(novaPagina);
  };

  return <ContainerPaginacao {...paginaAtual} onChange={executaPaginacao} />;
};

Paginacao.propTypes = {
  numeroPagina: PropTypes.oneOfType([PropTypes.any]),
  numeroRegistros: PropTypes.oneOfType([PropTypes.any]),
  onChangePaginacao: PropTypes.func,
};

Paginacao.defaultProps = {
  numeroPagina: 1,
  numeroRegistros: 0,
  onChangePaginacao: () => {},
};

export default Paginacao;
