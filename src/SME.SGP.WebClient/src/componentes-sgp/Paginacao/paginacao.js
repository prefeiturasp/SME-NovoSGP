import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerPaginacao } from './paginacao.css';

const Paginacao = props => {
  const {
    numeroRegistros,
    onChangePaginacao,
    mostrarNumeroLinhas,
    onChangeNumeroLinhas,
    pageSize,
    ...rest
  } = props;

  const [paginaAtual, setPaginaAtual] = useState({
    defaultCurrent: 1,
    total: numeroRegistros,
    current: 1,
    pageSize,
    showSizeChanger: mostrarNumeroLinhas,
    onShowSizeChanger: mostrarNumeroLinhas,
  });

  const executaPaginacao = pagina => {
    onChangePaginacao(pagina);
    const novaPagina = { ...paginaAtual };
    novaPagina.current = pagina;
    setPaginaAtual(novaPagina);
  };

  useEffect(() => {
    setPaginaAtual(estadoAntigo => ({
      ...estadoAntigo,
      total: numeroRegistros,
      pageSize,
    }));
  }, [numeroRegistros, pageSize]);

  return (
    <ContainerPaginacao
      {...rest}
      {...paginaAtual}
      onChange={executaPaginacao}
      onShowSizeChange={onChangeNumeroLinhas}
    />
  );
};

Paginacao.propTypes = {
  numeroPagina: PropTypes.oneOfType([PropTypes.any]),
  numeroRegistros: PropTypes.oneOfType([PropTypes.any]),
  onChangePaginacao: PropTypes.func,
  mostrarNumeroLinhas: PropTypes.bool,
  onChangeNumeroLinhas: PropTypes.func,
  pageSize: PropTypes.number,
};

Paginacao.defaultProps = {
  numeroPagina: 1,
  numeroRegistros: 0,
  onChangePaginacao: () => {},
  mostrarNumeroLinhas: false,
  onChangeNumeroLinhas: () => {},
  pageSize: 5,
};

export default Paginacao;
