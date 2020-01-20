import React, { useState, useEffect, useCallback } from 'react';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderSecao } from '~/redux/modulos/loader/actions';

// Servicos
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { erro, confirmar, sucesso } from '~/servicos/alertas';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import RegistroPOAServico from '~/servicos/Paginas/DiarioClasse/RegistroPOA';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import Filtro from './componentes/Filtro';

function RegistroPOALista() {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [filtroValido, setFiltroValido] = useState(false);
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const dispatch = useDispatch();
  const { loaderSecao } = useSelector(store => store.loader);
  const usuarioLogado = useSelector(store => store.usuario);
  const permissoesTela = usuarioLogado.permissoes;
  const anoLetivo =
    usuarioLogado.turmaSelecionada.anoLetivo || window.moment().format('YYYY');

  const colunas = [
    {
      title: 'Bimestre',
      dataIndex: 'bimestre',
      width: '20%',
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

  const onClickExcluir = async () => {
    if (itensSelecionados && itensSelecionados.length > 0) {
      const listaNomeExcluir = itensSelecionados.map(item => item.titulo);
      const confirmado = await confirmar(
        'Excluir registro',
        listaNomeExcluir,
        `Deseja realmente excluir ${
          itensSelecionados.length > 1 ? 'estes itens' : 'este item'
        }?`,
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        dispatch(setLoaderSecao(true));
        const excluir = await Promise.all(
          itensSelecionados.map(x =>
            RegistroPOAServico.deletarRegistroPOA(x.id)
          )
        );
        if (excluir) {
          const mensagemSucesso = `${
            itensSelecionados.length > 1
              ? 'Registros excluídos'
              : 'Registro excluído'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          setFiltro({
            ...filtro,
            atualizar: !filtro.atualizar || true,
          });
          dispatch(setLoaderSecao(false));
          setItensSelecionados([]);
        }
      }
    }
  };

  useEffect(() => {
    setFiltroValido(ehFiltroValido());
  }, [filtro]);

  const onChangeFiltro = useCallback(
    valoresFiltro => {
      setFiltro({
        ...valoresFiltro,
        CodigoRf: valoresFiltro.professorRf,
        anoLetivo,
        bimestre: valoresFiltro.bimestre || 0,
      });
    },
    [anoLetivo]
  );

  const ehFiltroValido = () =>
    !!filtro.dreId && !!filtro.ueId && !!filtro.professorRf;

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  return (
    <>
      <Cabecalho pagina="Registro do Professor Orientador de Área" />
      <Loader loading={loaderSecao}>
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
              !!filtro.dreId === false && !!filtro.ueId === false
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
              selecionarItems={onSelecionarItems}
              filtroEhValido={filtroValido}
              onErro={err => erro(JSON.stringify(err))}
            />
          </div>
        </Card>
      </Loader>
    </>
  );
}

export default RegistroPOALista;
