import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

import { Button, Colors, DataTable } from '~/componentes';

import { RotasDto, situacaoPlanoAEE } from '~/dtos';
import { verificaSomenteConsulta } from '~/servicos';

import ModalReestruturacaoPlano from '../ModalReestruturacaoPlano/modalReestruturacaoPlano';
import { BotaoEstilizado, TextoEstilizado } from './reestruturacaoTabela.css';

const ReestruturacaoTabela = ({ key, listaDados, match, semestre }) => {
  const [exibirModal, setModalVisivel] = useState(false);
  const [modoConsulta, setModoConsulta] = useState(false);
  const [dadosVisualizacao, setDadosVisualizacao] = useState();

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const alternarModal = booleano => setModalVisivel(booleano || !exibirModal);

  const cliqueVisualizar = dadosLinha => {
    alternarModal();
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

  const cliqueNovaRestruturacao = () => {
    setDadosVisualizacao([]);
    setModoConsulta(false);
    alternarModal();
  };

  useEffect(() => {
    setModoConsulta(
      somenteConsulta ||
        !permissoesTela.podeIncluir ||
        (planoAEEDados?.situacao !== situacaoPlanoAEE.EmAndamento &&
          planoAEEDados?.situacao !== situacaoPlanoAEE.Expirado &&
          planoAEEDados?.situacao !== situacaoPlanoAEE.Reestruturado)
    );
  }, [somenteConsulta, permissoesTela, planoAEEDados]);

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  return (
    <>
      {exibirModal && (
        <ModalReestruturacaoPlano
          key={key}
          alternarModal={alternarModal}
          exibirModal={exibirModal}
          modoConsulta={modoConsulta}
          dadosVisualizacao={dadosVisualizacao}
          semestre={semestre}
          match={match}
        />
      )}
      <div>
        <DataTable
          semHover
          rowKey="key"
          columns={colunas}
          dataSource={listaDados}
          pagination={false}
        />
      </div>
      {(planoAEEDados?.situacao === situacaoPlanoAEE.EmAndamento ||
        planoAEEDados?.situacao === situacaoPlanoAEE.Expirado ||
        planoAEEDados?.situacao === situacaoPlanoAEE.Reestruturado) &&
        !somenteConsulta &&
        permissoesTela.podeIncluir && (
          <Button
            id={`btn-${key}`}
            label="Nova reestruturação"
            icon="plus"
            color={Colors.Azul}
            border
            className="mr-3 mt-2"
            onClick={cliqueNovaRestruturacao}
          />
        )}
    </>
  );
};

ReestruturacaoTabela.defaultProps = {
  match: {},
};

ReestruturacaoTabela.propTypes = {
  key: PropTypes.string.isRequired,
  listaDados: PropTypes.oneOfType([PropTypes.array]).isRequired,
  semestre: PropTypes.number.isRequired,
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default ReestruturacaoTabela;
