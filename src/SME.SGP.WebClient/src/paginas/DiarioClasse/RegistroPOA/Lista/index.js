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

  const renderizarBimestres = valor => {
    const bimestres = [
      {
        valor: '1',
        desc: '1º Bimestre',
      },
      {
        valor: '2',
        desc: '2º Bimestre',
      },
      {
        valor: '3',
        desc: '3º Bimestre',
      },
      {
        valor: '4',
        desc: '4º Bimestre',
      },
    ];
    return bimestres.find(bimestre => bimestre.valor === valor.toString()).desc;
  };

  const colunas = [
    {
      title: 'Bimestre',
      dataIndex: 'bimestre',
      width: '20%',
      render: valor => {
        return renderizarBimestres(valor);
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
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  useEffect(() => {
    setFiltroValido(
      () => !!filtro && !!filtro.dreId && !!filtro.ueId && !!filtro.codigoRf
    );
  }, [filtro]);

  const onChangeFiltro = useCallback(
    valoresFiltro => {
      const paramsConsulta = {};
      if (valoresFiltro.bimestre) {
        paramsConsulta.bimestre = valoresFiltro.bimestre;
      }
      if (valoresFiltro.professorRf) {
        paramsConsulta.codigoRf = valoresFiltro.professorRf;
      }
      if (valoresFiltro.dreId) {
        paramsConsulta.dreId = valoresFiltro.dreId;
      }
      if (valoresFiltro.titulo) {
        paramsConsulta.titulo = valoresFiltro.titulo;
      }
      if (valoresFiltro.ueId) {
        paramsConsulta.ueId = valoresFiltro.ueId;
      }
      if (anoLetivo) {
        paramsConsulta.anoLetivo = anoLetivo;
      }
      setFiltro(paramsConsulta);
    },
    [anoLetivo]
  );

  return (
    <>
      <Cabecalho pagina="Registro do professor orientador de área" />
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
