import React, { useState } from 'react';
import PropTypes from 'prop-types';

import { Button, Colors, DataTable } from '~/componentes';

import ModalReestruturacaoPlano from '../ModalReestruturacaoPlano/modalReestruturacaoPlano';
import { BotaoEstilizado, TextoEstilizado } from './reestruturacaoTabela.css';

const ReestruturacaoTabela = ({
  key,
  listaDados,
  match,
  listaVersao,
  semestre,
}) => {
  const [exibirModal, setModalVisivel] = useState(false);
  const [modoVisualizacao, setModoVisualizacao] = useState(false);
  const [dadosVisualizacao, setDadosVisualizacao] = useState();

  const cliqueVisualizar = dadosLinha => {
    setModalVisivel(true);
    setModoVisualizacao(true);
    setDadosVisualizacao(dadosLinha);
  };

  const montarUltimaColuna = (texto, dadosLinha) => {
    return (
      <div className="d-flex">
        <TextoEstilizado className="mr-4 limitar-texto">
          {texto}
        </TextoEstilizado>
        <BotaoEstilizado
          id="btn-visualizar"
          icon="eye"
          iconType="far"
          color={Colors.Verde}
          onClick={() => cliqueVisualizar(dadosLinha)}
          height="24px"
          width="24px"
        />
      </div>
    );
  };

  const colunas = [
    {
      title: 'Data',
      dataIndex: 'data',
    },
    {
      title: 'Versão do plano',
      dataIndex: 'versao',
    },
    {
      title: 'Descrição da reestruturação',
      dataIndex: 'descricaoSimples',
      render: montarUltimaColuna,
    },
  ];

  const esconderModal = () => setModalVisivel(false);

  const cliqueNovaRestruturacao = () => {
    setDadosVisualizacao([]);
    setModoVisualizacao(false);
    setModalVisivel(true);
  };

  return (
    <>
      <ModalReestruturacaoPlano
        key={key}
        esconderModal={esconderModal}
        exibirModal={exibirModal}
        modoVisualizacao={modoVisualizacao}
        dadosVisualizacao={dadosVisualizacao}
        listaVersao={listaVersao}
        semestre={semestre}
        match={match}
      />
      <div>
        <DataTable
          rowKey="key"
          columns={colunas}
          dataSource={listaDados}
          pagination={false}
        />
      </div>
      <Button
        id={`btn-${key}`}
        label="Nova reestruturação"
        icon="plus"
        color={Colors.Azul}
        border
        className="mr-3 mt-2"
        onClick={cliqueNovaRestruturacao}
      />
    </>
  );
};

ReestruturacaoTabela.defaultProps = {
  listaVersao: [],
  match: {},
};

ReestruturacaoTabela.propTypes = {
  key: PropTypes.string.isRequired,
  listaDados: PropTypes.oneOfType([PropTypes.array]).isRequired,
  listaVersao: PropTypes.oneOfType([PropTypes.array]),
  semestre: PropTypes.number.isRequired,
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default ReestruturacaoTabela;
