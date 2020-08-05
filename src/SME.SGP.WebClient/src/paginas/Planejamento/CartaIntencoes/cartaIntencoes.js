import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AlertaNaoPermiteTurmaInfantil from '~/componentes-sgp/AlertaNaoPermiteTurmaInfantil/alertaNaoPermiteTurmaInfantil';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import {
  limparDadosCartaIntencoes,
  setCarregandoCartaIntencoes,
  setDadosCartaIntencoes,
} from '~/redux/modulos/cartaIntencoes/actions';
import { erros } from '~/servicos/alertas';
import ServicoCartaIntencoes from '~/servicos/Paginas/CartaIntencoes/ServicoCartaIntencoes';
import servicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import { Container } from './cartaIntencoes.css';
import BotoesAcoesCartaIntencoes from './DadosCartaIntencoes/BotoesAcoes/botoesAcoesCartaIntencoes';
import DadosCartaIntencoes from './DadosCartaIntencoes/dadosCartaIntencoes';
import ModalErrosCartaIntencoes from './DadosCartaIntencoes/ModalErros/ModalErrosCartaIntencoes';
import LoaderCartaIntencoes from './LoaderCartaIntencoes/laderCartaIntencoes';
import servicoSalvarCartaIntencoes from './servicoSalvarCartaIntencoes';

const CartaIntencoes = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma } = turmaSelecionada;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [componenteCurricular, setComponenteCurricular] = useState(undefined);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosCartaIntencoes());
  }, [dispatch]);

  const mostrarLoader = useCallback(
    mostrar => {
      dispatch(setCarregandoCartaIntencoes(mostrar));
    },
    [dispatch]
  );

  const obterListaBimestres = useCallback(async () => {
    if (turma) {
      mostrarLoader(true);
      const retorno = await ServicoCartaIntencoes.obterBimestres(
        turma,
        componenteCurricular
      ).catch(e => erros(e));

      if (retorno && retorno.data) {
        dispatch(setDadosCartaIntencoes(retorno.data));
      } else {
        resetarInfomacoes();
      }
      mostrarLoader(false);
    }
  }, [dispatch, turma, resetarInfomacoes, componenteCurricular, mostrarLoader]);

  const obterListaComponenteCurricular = useCallback(async () => {
    const resposta = await servicoDisciplinas
      .obterDisciplinasPorTurma(turma)
      .catch(e => erros(e));

    if (resposta && resposta.data) {
      setListaComponenteCurricular(resposta.data);
      if (resposta.data.length === 1) {
        const disciplina = resposta.data[0];
        setComponenteCurricular(String(disciplina.codigoComponenteCurricular));
      }
    } else {
      setListaComponenteCurricular([]);
      setComponenteCurricular(undefined);
    }
  }, [turma]);

  useEffect(() => {
    resetarInfomacoes();
    if (
      turma &&
      ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterListaComponenteCurricular();
    } else {
      setComponenteCurricular(undefined);
      setListaComponenteCurricular([]);
    }
  }, [
    turma,
    resetarInfomacoes,
    obterListaComponenteCurricular,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
  ]);

  useEffect(() => {
    if (componenteCurricular) {
      obterListaBimestres(componenteCurricular);
    } else {
      resetarInfomacoes();
    }
  }, [componenteCurricular, obterListaBimestres, resetarInfomacoes]);

  const onChangeSemestreComponenteCurricular = async valor => {
    const continuar = await servicoSalvarCartaIntencoes.perguntaDescartarRegistros();

    if (continuar) {
      setComponenteCurricular(valor);
    }
  };

  const recaregarDados = () => {
    obterListaBimestres(componenteCurricular);
  };

  return (
    <Container>
      {!turmaSelecionada.turma &&
      ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <div className="col-md-12">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-sem-turma-relatorio-semestral',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </div>
      ) : (
        ''
      )}
      {/* <AlertaNaoPermiteTurmaInfantil /> */}
      <ModalErrosCartaIntencoes />
      <Cabecalho pagina="Carta de Intenções" />
      <LoaderCartaIntencoes>
        <Card>
          <>
            <div className="col-md-12">
              <div className="row">
                <div className="col-md-12 d-flex justify-content-end pb-4">
                  <BotoesAcoesCartaIntencoes
                    onClickCancelar={recaregarDados}
                    onClickSalvar={recaregarDados}
                  />
                </div>
              </div>
            </div>
          </>
          {turmaSelecionada.turma &&
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
            <>
              <div className="col-md-12">
                <div className="row">
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-4 mb-2">
                    <SelectComponent
                      id="componente-curricular"
                      lista={listaComponenteCurricular || []}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      valueSelect={componenteCurricular}
                      onChange={onChangeSemestreComponenteCurricular}
                      placeholder="Selecione um componente curricular"
                      disabled={
                        listaComponenteCurricular &&
                        listaComponenteCurricular.length === 1
                      }
                    />
                  </div>
                  {componenteCurricular ? (
                    <div className="col-md-12">
                      <DadosCartaIntencoes />
                    </div>
                  ) : (
                    ''
                  )}
                </div>
              </div>
            </>
          ) : (
            ''
          )}
        </Card>
      </LoaderCartaIntencoes>
    </Container>
  );
};

export default CartaIntencoes;
