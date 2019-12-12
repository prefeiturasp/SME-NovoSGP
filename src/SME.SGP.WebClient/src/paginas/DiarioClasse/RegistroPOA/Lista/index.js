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
    history.push(
      `/gestao/atribuicao-cjs/novo?dreId=${filtro.DreId}&ueId=${filtro.UeId}`
    );

  const onSelecionarItems = lista => {
    setItensSelecionados(lista);
  };

  const onClickEditar = item => {
    history.push(
      `/gestao/atribuicao-cjs/editar?modalidadeId=${item.modalidadeId}&turmaId=${item.turmaId}&dreId=${filtro.DreId}&ueId=${filtro.UeId}`
    );
  };

  const onClickExcluir = itens => {
    console.log(itens);
  };

  const onChangeFiltro = valoresFiltro => {
    setFiltro({
      AnoLetivo: '2019',
      DreId: valoresFiltro.dreId,
      UeId: valoresFiltro.ueId,
      UsuarioRF: valoresFiltro.professorRf,
    });
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
