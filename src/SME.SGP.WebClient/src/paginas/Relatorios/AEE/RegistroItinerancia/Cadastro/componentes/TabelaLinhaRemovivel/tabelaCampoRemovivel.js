import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

import shortid from 'shortid';
import { Button, Colors } from '~/componentes';

import {
  BotaoEstilizado,
  CustomTable,
  LabelEstilizado,
} from './tabelaCampoRemovivel.css';

const TabelaLinhaRemovivel = ({
  botaoAdicionar,
  dadosTabela,
  dataIndex,
  labelBotao,
  labelTabela,
  removerUsuario,
  tituloTabela,
  ...rest
}) => {
  const [dadosSelecionados, setDadosSelecionados] = useState();

  useEffect(() => {
    if (!dadosSelecionados) {
      setDadosSelecionados(dadosTabela);
    }
  }, [dadosTabela, dadosSelecionados]);

  const montarBotaoRemover = text => {
    return (
      <BotaoEstilizado
        id="btn-excluir"
        icon="trash-alt"
        iconType="far"
        color={Colors.CinzaBotao}
        onClick={() => removerUsuario(text)}
        height="13px"
        width="13px"
      />
    );
  };

  const colunas = [
    {
      title: tituloTabela,
      dataIndex,
      key: dataIndex,
    },
    {
      title: '',
      key: 'action',
      render: montarBotaoRemover,
    },
  ];

  return (
    <>
      <div className="col-8 mb-2">
        <LabelEstilizado>{labelTabela}</LabelEstilizado>
      </div>
      <div className="col-8 mb-3">
        <CustomTable {...rest} dataSource={dadosTabela} columns={colunas} />
      </div>
      <div className="col-8">
        <Button
          id={shortid.generate()}
          label={labelBotao}
          color={Colors.Azul}
          border
          className="mr-2"
          onClick={botaoAdicionar}
          icon="plus"
        />
      </div>
    </>
  );
};

TabelaLinhaRemovivel.propTypes = {
  botaoAdicionar: PropTypes.func,
  dadosTabela: PropTypes.oneOfType([PropTypes.array]),
  dataIndex: PropTypes.string,
  labelBotao: PropTypes.string,
  labelTabela: PropTypes.string,
  removerUsuario: PropTypes.func,
  tituloTabela: PropTypes.string,
};

TabelaLinhaRemovivel.defaultProps = {
  botaoAdicionar: () => {},
  dadosTabela: [],
  dataIndex: '',
  labelBotao: '',
  labelTabela: '',
  removerUsuario: () => {},
  tituloTabela: '',
};

export default TabelaLinhaRemovivel;
