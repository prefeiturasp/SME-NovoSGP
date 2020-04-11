import React, { useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';

import { Cabecalho } from '~/componentes-sgp';
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import Filtro from '../Filtro';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const ComunicadosLista = () => {
  const Badge = styled.button``;
  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const [itensSelecionados, setItensSelecionados] = useState([]);

  useCallback(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const onSelecionarItems = items => {
    setItensSelecionados(items);
  };

  const onClickExcluir = async () => {};

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickBotaoPrincipal = () => {
    history.push(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/novo`);
  };

  const onClickEditar = mensagem => {
    history.push(
      `${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/editar/${mensagem.Id}`
    );
  };

  const [filtro, setFiltro] = useState({});

  const onChangeFiltro = valoresFiltro => {
    setFiltro({ ...valoresFiltro });
  };

  const colunas = [
    {
      title: 'Grupo',
      dataIndex: 'Mensagem_Grupo',
      render: grupos => {
        return grupos.map(grupo => (
          <Badge
            key={shortid.generate()}
            alt={grupo.Nome_Grupo}
            className="badge badge-pill bg-white border text-dark font-weight-light px-2 py-1 mr-2"
          >
            {grupo.Nome_Grupo}
          </Badge>
        ));
      },
    },
    {
      title: 'Título',
      dataIndex: 'Titulo',
    },
    {
      title: 'Data de envio',
      dataIndex: 'Data_Envio',
      render: data => {
        return window.moment(data).format('DD/MM/YYYY');
      },
    },
    {
      title: 'Data de expiração',
      dataIndex: 'Data_Expiracao',
      render: data => {
        return window.moment(data).format('DD/MM/YYYY');
      },
    },
  ];

  return (
    <>
      <Cabecalho pagina="Comunicação com pais ou responsáveis" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]}
            temItemSelecionado={
              itensSelecionados && itensSelecionados.length >= 1
            }
            onClickExcluir={onClickExcluir}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Novo"
          />
          <Filtro onFiltrar={onChangeFiltro} />
          <div className="col-md-12 pt-2 py-0 px-0">
            <ListaPaginada
              id="lista-comunicados"
              url="https://demo4860311.mockable.io/mensagens"
              idLinha="Id"
              colunaChave="Id"
              colunas={colunas}
              onClick={onClickEditar}
              multiSelecao
              filtro={filtro}
              selecionarItems={onSelecionarItems}
              filtroEhValido
            />
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default ComunicadosLista;
