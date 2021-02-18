import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

import shortid from 'shortid';
import { Button, Colors } from '~/componentes';

import {
  BotaoEstilizado,
  CustomTable,
  LabelEstilizado,
} from './tabelaCampoRemovivel.css';

import { ordenarPor } from '~/utils/funcoes/gerais';

const TabelaLinhaRemovivel = ({
  botaoAdicionar,
  dadosTabela,
  dataIndex,
  labelBotao,
  labelTabela,
  ordenacao,
  removerUsuario,
  tituloTabela,
  desabilitadoIncluir,
  desabilitadoExcluir,
  ...rest
}) => {
  const [dadosSelecionados, setDadosSelecionados] = useState();

  useEffect(() => {
    if (dadosTabela) {
      const dadosTabelaOrdadenados = ordenacao
        ? ordenarPor(dadosTabela, dataIndex)
        : dadosTabela;
      setDadosSelecionados(dadosTabelaOrdadenados);
    }
  }, [dadosTabela, dataIndex, ordenacao]);

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
        disabled={desabilitadoExcluir}
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
        <CustomTable
          {...rest}
          dataSource={dadosSelecionados}
          columns={colunas}
        />
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
          disabled={desabilitadoIncluir}
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
  ordenacao: PropTypes.bool,
  removerUsuario: PropTypes.func,
  tituloTabela: PropTypes.string,
  desabilitadoIncluir: PropTypes.bool,
  desabilitadoExcluir: PropTypes.bool,
};

TabelaLinhaRemovivel.defaultProps = {
  botaoAdicionar: () => {},
  dadosTabela: [],
  dataIndex: '',
  labelBotao: '',
  labelTabela: '',
  ordenacao: false,
  removerUsuario: () => {},
  tituloTabela: '',
  desabilitadoIncluir: false,
  desabilitadoExcluir: false,
};

export default TabelaLinhaRemovivel;
