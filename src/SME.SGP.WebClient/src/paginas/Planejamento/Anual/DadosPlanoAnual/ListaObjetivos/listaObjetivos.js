import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import TransferenciaLista from '~/componentes-sgp/TranferenciaLista/transferenciaLista';
import {
  setDadosBimestresPlanoAnual,
  setExibirLoaderPlanoAnual,
  setPlanoAnualEmEdicao,
} from '~/redux/modulos/anual/actions';
import { erros } from '~/servicos/alertas';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ContainerListaObjetivos } from './listaObjetivos.css';

const ListaObjetivos = React.memo(props => {
  const { dadosBimestre, tabAtualComponenteCurricular } = props;
  const { bimestre, periodoAberto } = dadosBimestre;

  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const dadosBimestrePlanoAnual = useSelector(
    store => store.planoAnual.dadosBimestresPlanoAnual[bimestre]
  );

  const planoAnualSomenteConsulta = useSelector(
    store => store.planoAnual.planoAnualSomenteConsulta
  );

  const [dadosEsquerda, setDadosEsquerda] = useState([]);
  const [dadosDireita, setDadosDireita] = useState([]);
  const [idsSelecionadsEsquerda, setIdsSelecionadsEsquerda] = useState([]);
  const [idsSelecionadsDireita, setIdsSelecionadsDireita] = useState([]);

  const obterDadosComponenteAtual = useCallback(() => {
    return dadosBimestrePlanoAnual?.componentes.find(
      item =>
        String(item.componenteCurricularId) ===
        String(tabAtualComponenteCurricular.codigoComponenteCurricular)
    );
  }, [tabAtualComponenteCurricular, dadosBimestrePlanoAnual]);

  const montarDadosListasObjetivos = useCallback(
    listaObjetivos => {
      const dadosComponenteAtual = obterDadosComponenteAtual();

      if (
        dadosComponenteAtual &&
        dadosComponenteAtual.objetivosAprendizagemId.length
      ) {
        const listaEsquerda = [];
        const listaDireita = [];
        listaObjetivos.forEach(objetivo => {
          if (
            dadosComponenteAtual.objetivosAprendizagemId.find(
              objetivoId => objetivoId === objetivo.id
            )
          ) {
            listaDireita.push(objetivo);
          } else {
            listaEsquerda.push(objetivo);
          }
        });

        if (listaEsquerda.length) {
          setDadosEsquerda(listaEsquerda);
        }
        if (listaDireita.length) {
          setDadosDireita(listaDireita);
        }
      } else {
        setDadosEsquerda(listaObjetivos);
      }
    },
    [obterDadosComponenteAtual]
  );

  // TODO Verificar para salvar dados no redux e não consultar no banco novamente a cada mudança de tab!
  const obterObjetivosPorAnoEComponenteCurricular = useCallback(() => {
    if (
      tabAtualComponenteCurricular &&
      tabAtualComponenteCurricular.codigoComponenteCurricular
    ) {
      dispatch(setExibirLoaderPlanoAnual(true));
      ServicoPlanoAnual.obterListaObjetivosPorAnoEComponenteCurricular(
        turmaSelecionada.ano,
        turmaSelecionada.ensinoEspecial,
        tabAtualComponenteCurricular.codigoComponenteCurricular
      )
        .then(listaObjetivos => {
          montarDadosListasObjetivos(listaObjetivos);
        })
        .catch(e => {
          erros(e);
        })
        .finally(() => {
          dispatch(setExibirLoaderPlanoAnual(false));
        });
    }
  }, [
    dispatch,
    tabAtualComponenteCurricular,
    turmaSelecionada,
    montarDadosListasObjetivos,
  ]);

  useEffect(() => {
    if (tabAtualComponenteCurricular && obterDadosComponenteAtual()) {
      obterObjetivosPorAnoEComponenteCurricular();
    }
  }, [
    tabAtualComponenteCurricular,
    obterObjetivosPorAnoEComponenteCurricular,
    obterDadosComponenteAtual,
  ]);

  const parametrosListaEsquerda = {
    title: 'Objetivos de aprendizagem',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosEsquerda,
    onSelectRow: setIdsSelecionadsEsquerda,
    selectedRowKeys: idsSelecionadsEsquerda,
    selectMultipleRows: periodoAberto && !planoAnualSomenteConsulta,
  };

  const parametrosListaDireita = {
    title:
      dadosDireita && dadosDireita.length
        ? 'Objetivos de aprendizagem selecionados para este componente curricular'
        : 'Você precisa selecionar ao menos um objetivo para poder inserir a descrição do plano',
    columns: [
      {
        title: 'Código',
        dataIndex: 'codigo',
        className: 'desc-codigo fundo-codigo-lista-direita',
        width: '90px',
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        className: 'desc-descricao',
      },
    ],
    dataSource: dadosDireita,
    onSelectRow: setIdsSelecionadsDireita,
    selectedRowKeys: idsSelecionadsDireita,
    selectMultipleRows: periodoAberto && !planoAnualSomenteConsulta,
  };

  const obterListaComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => String(id) === String(item.id)));
  };

  const obterListaSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => String(id) === String(item.id)));
  };

  const onClickAdicionar = () => {
    if (
      idsSelecionadsEsquerda &&
      idsSelecionadsEsquerda.length &&
      !planoAnualSomenteConsulta
    ) {
      const novaListaDireita = obterListaComIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      const novaListaEsquerda = obterListaSemIdsSelecionados(
        dadosEsquerda,
        idsSelecionadsEsquerda
      );

      setDadosEsquerda([...novaListaEsquerda]);
      setDadosDireita([...novaListaDireita, ...dadosDireita]);

      // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
      const dados = { ...dadosBimestrePlanoAnual };
      dados.componentes.forEach(item => {
        if (
          String(item.componenteCurricularId) ===
          String(tabAtualComponenteCurricular.codigoComponenteCurricular)
        ) {
          const novaLista = [...novaListaDireita, ...dadosDireita];
          item.objetivosAprendizagemId = novaLista.map(c => c.id);
          item.emEdicao = true;
          dispatch(setDadosBimestresPlanoAnual(dados));
        }
      });

      dispatch(setPlanoAnualEmEdicao(true));
      setIdsSelecionadsEsquerda([]);
    }
  };

  const onClickRemover = async () => {
    if (
      idsSelecionadsDireita &&
      idsSelecionadsDireita.length &&
      !planoAnualSomenteConsulta
    ) {
      const novaListaEsquerda = obterListaComIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      const novaListaDireita = obterListaSemIdsSelecionados(
        dadosDireita,
        idsSelecionadsDireita
      );

      setDadosEsquerda([...novaListaEsquerda, ...dadosEsquerda]);
      setDadosDireita([...novaListaDireita]);

      // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
      const dados = { ...dadosBimestrePlanoAnual };
      dados.componentes.forEach(item => {
        if (
          String(item.componenteCurricularId) ===
          String(tabAtualComponenteCurricular.codigoComponenteCurricular)
        ) {
          const novaLista = [...novaListaDireita];
          item.objetivosAprendizagemId = novaLista.map(c => c.id);
          item.emEdicao = true;
          dispatch(setDadosBimestresPlanoAnual(dados));
        }
      });
      dispatch(setPlanoAnualEmEdicao(true));
      setIdsSelecionadsDireita([]);
    }
  };

  // TODO Verificar para renderizar somente quando terminar de obter os dados!
  return (
    <ContainerListaObjetivos>
      <TransferenciaLista
        listaEsquerda={parametrosListaEsquerda}
        listaDireita={parametrosListaDireita}
        onClickAdicionar={onClickAdicionar}
        onClickRemover={onClickRemover}
      />
    </ContainerListaObjetivos>
  );
});

ListaObjetivos.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

ListaObjetivos.defaultProps = {
  dadosBimestre: {},
  tabAtualComponenteCurricular: {},
};

export default ListaObjetivos;
