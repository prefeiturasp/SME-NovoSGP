import { Dropdown, Menu } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import shortid from 'shortid';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';

const Container = styled(Dropdown)`
  background-color: #064f79 !important;
  color: white !important;
  width: 91.7px !important;
  height: 38px !important;

  i {
    transform: rotate(90deg) !important;
  }

  .ant-btn:hover,
  .ant-btn:focus {
    color: white !important;
    background-color: #064f79 !important;
    border-color: #064f79 !important;
  }

  .ant-dropdown-menu-item:hover,
  .ant-dropdown-menu-submenu-title:hover {
    background-color: red !important;
  }

  li {
    background-color: red !important;
  }
`;

const ContainerMenu = styled(Menu)`
  .ant-dropdown-menu-item:hover,
  .ant-dropdown-menu-submenu-title:hover {
    background-color: ${Base.Roxo};
    color: white;
  }

  .ant-dropdown-menu-item {
    transition: none !important;
    -webkit-transition: none !important;
  }
`;

const Ordenacao = props => {
  const {
    className,
    conteudoParaOrdenar,
    ordenarColunaNumero,
    ordenarColunaTexto,
    retornoOrdenado,
    desabilitado,
    onChangeOrdenacao,
  } = props;

  const ordenarMenorParaMaior = () => {
    const ordenar = (a, b) => {
      return a[ordenarColunaNumero] - b[ordenarColunaNumero];
    };
    const retorno = conteudoParaOrdenar.sort(ordenar);
    onChangeOrdenacao(3);
    retornoOrdenado([...retorno]);
  };

  const ordenarMaiorParaMenor = () => {
    const ordenar = (a, b) => {
      return b[ordenarColunaNumero] - a[ordenarColunaNumero];
    };
    const retorno = conteudoParaOrdenar.sort(ordenar);
    onChangeOrdenacao(4);
    retornoOrdenado([...retorno]);
  };

  const ordenarAZ = () => {
    const ordenar = (a, b) => {
      return a[ordenarColunaTexto] > b[ordenarColunaTexto]
        ? 1
        : b[ordenarColunaTexto] > a[ordenarColunaTexto]
        ? -1
        : 0;
    };
    const retorno = conteudoParaOrdenar.sort(ordenar);
    onChangeOrdenacao(1);
    retornoOrdenado([...retorno]);
  };

  const ordenarZA = () => {
    const ordenar = (a, b) => {
      return b[ordenarColunaTexto] > a[ordenarColunaTexto]
        ? 1
        : a[ordenarColunaTexto] > b[ordenarColunaTexto]
        ? -1
        : 0;
    };
    const retorno = conteudoParaOrdenar.sort(ordenar);
    onChangeOrdenacao(2);
    retornoOrdenado([...retorno]);
  };

  const menu = (
    <ContainerMenu>
      <Menu.Item onClick={ordenarMenorParaMaior}>
        Número (Menor para o maior)
      </Menu.Item>
      <Menu.Item onClick={ordenarMaiorParaMenor}>
        Número (Maior para o menor)
      </Menu.Item>
      <Menu.Item onClick={ordenarAZ}>Por ordem alfabética (A–Z)</Menu.Item>
      <Menu.Item onClick={ordenarZA}>Por ordem alfabética (Z–A)</Menu.Item>
    </ContainerMenu>
  );

  return (
    <Container
      trigger={['click']}
      overlay={menu}
      placement="bottomLeft"
      disabled={desabilitado}
    >
      <Button
        id={shortid.generate()}
        label="Ordenar"
        icon="exchange-alt"
        color={Colors.Azul}
        border
        className={`mr-2 ${className}`}
        disabled={desabilitado}
      />
    </Container>
  );
};

Ordenacao.defaultProps = {
  className: '',
  conteudoParaOrdenar: [],
  ordenarColunaNumero: 'id',
  ordenarColunaTexto: 'nome',
  retornoOrdenado: () => {},
  desabilitado: false,
  onChangeOrdenacao: () => {},
};

Ordenacao.propTypes = {
  className: PropTypes.string,
  conteudoParaOrdenar: PropTypes.oneOfType([PropTypes.array]),
  ordenarColunaNumero: PropTypes.string,
  ordenarColunaTexto: PropTypes.string,
  retornoOrdenado: PropTypes.func,
  desabilitado: PropTypes.bool,
  onChangeOrdenacao: PropTypes.func,
};
export default Ordenacao;
