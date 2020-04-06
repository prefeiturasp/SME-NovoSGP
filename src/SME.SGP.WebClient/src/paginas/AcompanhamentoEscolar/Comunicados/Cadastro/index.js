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

const ComunicadosCadastro = () => {
  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  useCallback(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  return (
    <>
      <Cabecalho pagina="Comunicado" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]}
            // onClickExcluir={onClickExcluir}
            // onClickVoltar={onClickVoltar}
            // onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Novo"
          />
        </Card>
      </Loader>
    </>
  );
};

export default ComunicadosCadastro;
