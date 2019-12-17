import React, { useState, useEffect } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { erro } from '~/servicos/alertas';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import Filtro from './componentes/Filtro';

// Funções
import { renderizarMes } from '~/utils/funcoes/dataMes';

function RegistroPOALista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const colunas = [
    {
      title: 'Mês',
      dataIndex: 'mes',
      width: '20%',
      render: valor => {
        return renderizarMes(valor);
      },
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
    },
  ];

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () =>
    history.push(`/diario-classe/registro-poa/novo`);

  const onClickEditar = item =>
    history.push(`/diario-classe/registro-poa/editar/${item.id}`);

  const onSelecionarItems = lista => setItensSelecionados(lista);

  const onClickExcluir = itens => console.log(itens);

  const onChangeFiltro = valoresFiltro => {
    setFiltro({
      ...valoresFiltro,
      CodigoRf: valoresFiltro.professorRf,
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
              url="v1/atribuicao/poa/listar"
              idLinha="id"
              colunaChave="id"
              colunas={colunas}
              onClick={onClickEditar}
              multiSelecao
              filtro={filtro}
              onSelecionarLinhas={onSelecionarItems}
              onErro={err => erro(JSON.stringify(err))}
            />
          </div>
        </Card>
      </Loader>
    </>
  );
}

export default RegistroPOALista;
