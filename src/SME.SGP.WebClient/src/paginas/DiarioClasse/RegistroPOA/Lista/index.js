import React, { useState, useEffect } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import Filtro from './componentes/Filtro';

function RegistroPOALista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const colunas = [
    {
      title: 'Mês',
      dataIndex: 'mes',
      key: 'mes',
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
      key: 'titulo',
    },
  ];

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () =>
    history.push(`/diario-classe/registro-poa/novo`);

  const onSelecionarItems = lista => {
    setItensSelecionados(lista);
  };

  const onClickEditar = item => {
    history.push(`/diario-classe/registro-poa/editar/${item.id}`);
  };

  const onClickExcluir = itens => {
    console.log(itens);
  };

  const onChangeFiltro = valoresFiltro => {
    setFiltro({});
  };

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  return (
    <>
      <Cabecalho pagina="Registro do professor orientador da área" />
      <Loader loading={false}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.REGISTRO_POA]}
            temItemSelecionado={
              itensSelecionados && itensSelecionados.length >= 1
            }
            onClickExcluir={onClickExcluir}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Novo"
            desabilitarBotaoPrincipal={
              !!filtro.DreId === false && !!filtro.UeId === false
            }
          />
          <Filtro onFiltrar={onChangeFiltro} />
          <div className="col-md-12 pt-2 py-0 px-0">
            <ListaPaginada
              id="lista-atribuicoes-cj"
              idLinha="modalidadeId"
              colunaChave="id"
              columns={colunas}
              onClickRow={onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
            />
          </div>
        </Card>
      </Loader>
    </>
  );
}

export default RegistroPOALista;
