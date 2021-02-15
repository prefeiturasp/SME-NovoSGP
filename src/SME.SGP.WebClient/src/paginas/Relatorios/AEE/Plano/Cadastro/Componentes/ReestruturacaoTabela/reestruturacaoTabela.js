import React, { useState } from 'react';
import PropTypes from 'prop-types';

import { Button, Colors, DataTable } from '~/componentes';

import ModalReestruturacaoPlano from '../ModalReestruturacaoPlano/modalReestruturacaoPlano';
import { BotaoEstilizado } from './reestruturacaoTabela.css';

const ReestruturacaoTabela = ({ key }) => {
  const [exibirModal, setModalVisivel] = useState(false);

  const montarBotaoRemover = text => {
    return (
      <div className="d-flex">
        <div className="mr-4">{text}</div>
        <BotaoEstilizado
          id="btn-visualizar"
          icon="eye"
          iconType="far"
          color={Colors.Verde}
          onClick={() => {}}
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
      dataIndex: 'versaoPlano',
    },
    {
      title: 'Descrição da reestruturação',
      dataIndex: 'descricaoReestruturacao',
      render: montarBotaoRemover,
    },
  ];

  const esconderModal = () => setModalVisivel(false);

  return (
    <>
      <ModalReestruturacaoPlano
        key={key}
        esconderModal={esconderModal}
        exibirModal={exibirModal}
      />
      <div>
        <DataTable
          rowKey="id"
          columns={colunas}
          dataSource={[
            {
              id: 1,
              data: '05/10/2020',
              versaoPlano: 'v7 - 19/02/2020',
              descricaoReestruturacao:
                'Alteradas as atividades que o aluno fazia após o período de aula, porque não houve muitos res…',
            },
          ]}
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
        onClick={() => setModalVisivel(true)}
      />
    </>
  );
};

ReestruturacaoTabela.propTypes = {
  key: PropTypes.string.isRequired,
};

export default ReestruturacaoTabela;
