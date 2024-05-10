import React, { useEffect, useState } from 'react';

import { useDispatch, useSelector } from 'react-redux';
import history from '~/servicos/history';
import { URL_HOME } from '~/constantes/url';
import Alert from '~/componentes/alert';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { Loader } from '~/componentes';
import SelectComponent from '~/componentes/select';

import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { erros } from '~/servicos';
import ListaBimestres from './Componentes/listaBimestres';
import modalidade from '~/dtos/modalidade';
import { setBimestreSelecionado } from '~/redux/modulos/acompanhamentoFrequencia/actions';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const AcompanhamentoFrequencia = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const dispatch = useDispatch();

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const listagemBimestres = [
    {
      id: 1,
      descricao: '1° Bimestre',
    },
    {
      id: 2,
      descricao: '2° Bimestre',
    },
    {
      id: 3,
      descricao: '3° Bimestre',
    },
    {
      id: 4,
      descricao: '4° Bimestre',
    },
    {
      id: 0,
      descricao: 'Final',
    },
  ];
  const listagemBimestresEJA = [
    {
      id: 1,
      descricao: '1° Bimestre',
    },
    {
      id: 2,
      descricao: '2° Bimestre',
    },
    {
      id: 0,
      descricao: 'Final',
    },
  ];

  const [bimestres, setBimestres] = useState([]);

  const [
    carregandoComponentesCurriculares,
    setCarregandoComponentesCurriculares,
  ] = useState(false);
  const [podeLancarFrequencia, setPodeLancarFrequencia] = useState(false);

  const [
    listaComponentesCurriculares,
    setListaComponentesCurriculares,
  ] = useState([]);
  const [
    componenteCurricularIdSelecionado,
    setComponenteCurricularIdSelecionado,
  ] = useState(undefined);
  const [
    desabilitarComponenteCurricular,
    setDesabilitarComponenteCurricular,
  ] = useState(false);

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const resetarFiltro = () => {
    setDesabilitarComponenteCurricular(false);
    setPodeLancarFrequencia(false);
    setListaComponentesCurriculares([]);
    setComponenteCurricularIdSelecionado(undefined);
  };

  useEffect(() => {
    const obterComponentesCurriculares = async () => {
      setCarregandoComponentesCurriculares(true);

      const componentesCurriculares = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaSelecionada.turma
      ).catch(e => erros(e));
      if (
        componentesCurriculares &&
        componentesCurriculares.data &&
        componentesCurriculares.data.length
      ) {
        setListaComponentesCurriculares(componentesCurriculares.data);
      } else {
        setListaComponentesCurriculares([]);
      }
      if (
        componentesCurriculares &&
        componentesCurriculares.data &&
        componentesCurriculares.data.length === 1
      ) {
        const componenteCurricular = componentesCurriculares.data[0];
        setComponenteCurricularIdSelecionado(
          String(componenteCurricular.codigoComponenteCurricular)
        );
        setDesabilitarComponenteCurricular(true);
      }
      if (turmaSelecionada.anoLetivo === new Date().getFullYear()) {
        const bimestreSelecionado = await ServicoConselhoClasse.obterBimestreAtual(
          turmaSelecionada.modalidade
        );
        if (bimestreSelecionado?.data) {
          dispatch(setBimestreSelecionado(bimestreSelecionado.data));
        } else {
          dispatch(setBimestreSelecionado(1));
        }
      } else {
        dispatch(setBimestreSelecionado(1));
      }
      setCarregandoComponentesCurriculares(false);
    };

    if (turmaSelecionada.turma) {
      resetarFiltro();
      obterComponentesCurriculares();
    } else {
      resetarFiltro();
      setDesabilitarComponenteCurricular(true);
    }
  }, [turmaSelecionada, modalidadesFiltroPrincipal]);

  const onChangeComponenteCurricular = async componenteCurricularId => {
    setComponenteCurricularIdSelecionado(componenteCurricularId);
    setPodeLancarFrequencia(false);
  };

  useEffect(() => {
    if (componenteCurricularIdSelecionado) {
      const componenteCurriular = listaComponentesCurriculares.find(
        item =>
          String(item.codigoComponenteCurricular) ===
          String(componenteCurricularIdSelecionado)
      );
      setPodeLancarFrequencia(componenteCurriular?.registraFrequencia);
      if (turmaSelecionada.modalidade === String(modalidade.EJA)) {
        setBimestres(listagemBimestresEJA);
      } else {
        setBimestres(listagemBimestres);
      }
    }
  }, [componenteCurricularIdSelecionado]);

  const obterComponenteCurricularId = () => {
    const componenteSelecionado = listaComponentesCurriculares.find(
      c =>
        String(c.codigoComponenteCurricular) ===
        String(componenteCurricularIdSelecionado)
    );
    return componenteSelecionado.territorioSaber
      ? componenteSelecionado.id
      : componenteCurricularIdSelecionado;
  };

  return (
    <>
      <div className="col-12">
        {!turmaSelecionada.turma ? (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'compensacao-selecione-turma',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        ) : (
          ''
        )}
        {!podeLancarFrequencia && componenteCurricularIdSelecionado && (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'compensacao-selecione-turma',
              mensagem:
                'Este componente curricular não possui controle de frequência.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        )}
        <Cabecalho pagina="Acompanhamento de Frequência" />
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
              </div>
            </div>
            <div className="row">
              <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3 mb-2">
                <Loader loading={carregandoComponentesCurriculares} tip="">
                  <SelectComponent
                    id="disciplina"
                    name="disciplinaId"
                    lista={listaComponentesCurriculares}
                    valueOption="codigoComponenteCurricular"
                    valueText="nome"
                    valueSelect={componenteCurricularIdSelecionado}
                    onChange={onChangeComponenteCurricular}
                    placeholder="Componente curricular"
                    disabled={desabilitarComponenteCurricular}
                  />
                </Loader>
              </div>
            </div>

            {componenteCurricularIdSelecionado && podeLancarFrequencia ? (
              <>
                <div className="row">
                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                    <ListaBimestres
                      bimestres={bimestres}
                      componenteCurricularIdSelecionado={obterComponenteCurricularId()}
                    />
                  </div>
                </div>
              </>
            ) : (
              ''
            )}
          </div>
        </Card>
      </div>
    </>
  );
};

export default AcompanhamentoFrequencia;
