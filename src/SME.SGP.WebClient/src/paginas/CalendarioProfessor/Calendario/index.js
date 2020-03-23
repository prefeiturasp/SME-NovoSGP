import React, { useState, useEffect, useCallback, useMemo } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { Tooltip, Switch } from 'antd';
import shortid from 'shortid';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Calendario from '~/componentes-sgp/calendarioProfessor/Calendario';
import { Base, Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import { store } from '~/redux';
import { zeraCalendario } from '~/redux/modulos/calendarioProfessor/actions';
import ModalidadeDTO from '~/dtos/modalidade';
import ServicoCalendarios from '~/servicos/Paginas/Calendario/ServicoCalendarios';
import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import { Loader } from '~/componentes';
import Alert from '~/componentes/alert';
import { erro } from '~/servicos/alertas';
import { AlertaSelecionarTurma } from '~/componentes-sgp';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;
const Icone = styled.i``;
const Label = styled.label``;

const CalendarioProfessor = () => {
  const [tiposCalendario, setTiposCalendario] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    undefined
  );

  const [diasLetivos, setDiasLetivos] = useState();
  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada: turmaSelecionadaStore } = usuario;
  const [controleTurmaSelecionada, setControleTurmaSelecionada] = useState();

  const modalidadesAbrangencia = useSelector(state => state.filtro.modalidades);
  const anoLetivo = useMemo(() => {
    return (turmaSelecionadaStore && turmaSelecionadaStore.anoLetivo) || null;
  }, [turmaSelecionadaStore]);

  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);

  const modalidadesPorAbrangencia = useCallback(() => {
    const modalidades = [];
    if (modalidadesAbrangencia) {
      modalidadesAbrangencia.forEach(modalidade => {
        if (
          (modalidade.valor === ModalidadeDTO.FUNDAMENTAL ||
            modalidade.valor === ModalidadeDTO.ENSINO_MEDIO) &&
          !modalidades.includes(1)
        )
          modalidades.push(1);
        if (modalidade.valor === ModalidadeDTO.EJA && !modalidades.includes(2))
          modalidades.push(2);
      });
    }
    return modalidades;
  }, [modalidadesAbrangencia]);

  const tiposDeCalendario = useMemo(() => {
    let tipos = tiposCalendario;

    if (tipos.length > 0 && modalidadesPorAbrangencia.length === 1) {
      tipos = tiposCalendario.filter(
        x => Number(x.modalidade) === modalidadesPorAbrangencia[0]
      );
    }

    if (Object.entries(turmaSelecionadaStore).length > 0) {
      const modalidadeSelecionada =
        String(turmaSelecionadaStore.modalidade) === String(ModalidadeDTO.EJA)
          ? 2
          : 1;

      tipos = tiposCalendario
        .filter(x => x.anoLetivo === anoLetivo)
        .filter(y => Number(y.modalidade) === Number(modalidadeSelecionada));

      if (!tipos || tipos.length === 0) {
        erro(
          'Nenhum tipo de calendário encontrado para o ano letivo e modalidade selecionada'
        );
      }
    } else {
      tipos = [];
    }

    if (tipos && tipos.length > 0) {
      setTipoCalendarioSelecionado(tipos[0].valor.toString());
    } else {
      setTipoCalendarioSelecionado(undefined);
    }

    return tipos;
  }, [
    anoLetivo,
    modalidadesPorAbrangencia,
    tiposCalendario,
    turmaSelecionadaStore,
  ]);

  useEffect(() => {
    // Busca os calendarios disponíveis por ano letivo
    const buscarTipos = async () => {
      setCarregandoTipos(true);
      const { data, status } = await ServicoCalendarios.obterTiposCalendario(
        turmaSelecionadaStore.anoLetivo
      );
      if (data && status === 200) {
        if (Object.entries(turmaSelecionadaStore).length > 0) {
          setTiposCalendario(
            data.map(x => ({
              desc: x.nome,
              valor: x.id,
              modalidade: x.modalidade,
              anoLetivo: x.anoLetivo,
            }))
          );
        }
        setCarregandoTipos(false);
      }
    };
    buscarTipos();
  }, [turmaSelecionadaStore, turmaSelecionadaStore.anoLetivo]);

  const eventoAulaCalendarioEdicao = useSelector(
    state => state.calendarioProfessor.eventoAulaCalendarioEdicao
  );

  const [eventoSme, setEventoSme] = useState(true);

  useEffect(() => {
    if (
      tiposCalendario &&
      eventoAulaCalendarioEdicao &&
      eventoAulaCalendarioEdicao.tipoCalendario
    ) {
      setTipoCalendarioSelecionado(eventoAulaCalendarioEdicao.tipoCalendario);
      if (eventoAulaCalendarioEdicao.eventoSme)
        setEventoSme(eventoAulaCalendarioEdicao.eventoSme);
    }
  }, [eventoAulaCalendarioEdicao, tiposCalendario]);

  const [dreSelecionada, setDreSelecionada] = useState(undefined);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState(
    undefined
  );

  const aoSelecionarTipoCalendario = tipo => {
    store.dispatch(zeraCalendario());
    setTipoCalendarioSelecionado(tipo);
  };

  const [turmaSelecionada, setTurmaSelecionada] = useState(undefined);
  const [todasTurmas, setTodasTurmas] = useState(false);

  const [filtros, setFiltros] = useState({
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
    todasTurmas,
  });

  const [opcaoTurma, setOpcaoTurma] = useState(undefined);
  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState([]);

  const obterDres = () => {
    setCarregandoDres(true);
    api
      .get(`v1/abrangencias/${turmaSelecionadaStore.consideraHistorico}/dres`)
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(dre => {
              lista.push({
                desc: dre.nome,
                valor: dre.codigo,
                abrev: dre.abreviacao,
              });
            });
            setDres(lista.sort(FiltroHelper.ordenarLista('desc')));
            setCarregandoDres(false);
          }
        }
      })
      .catch(() => {
        setDres(dresStore);
        setCarregandoDres(false);
      });
  };

  useEffect(() => {
    if (tipoCalendarioSelecionado) {
      obterDres();
    } else {
      setDiasLetivos();
      setDreSelecionada();
      setUnidadeEscolarSelecionada();
      setOpcaoTurma();
    }
  }, [tipoCalendarioSelecionado]);

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const aoTrocarEventoSme = () => {
    setEventoSme(!eventoSme);
  };

  const [dreDesabilitada, setDreDesabilitada] = useState(false);

  useEffect(() => {
    if (dres.length === 1) {
      setDreSelecionada(dres[0].valor);
      setDreDesabilitada(true);
    } else if (
      dres &&
      eventoAulaCalendarioEdicao &&
      eventoAulaCalendarioEdicao.dre
    ) {
      setDreSelecionada(eventoAulaCalendarioEdicao.dre);
    } else if (Object.entries(turmaSelecionadaStore).length > 0) {
      setDreSelecionada(turmaSelecionadaStore.dre);
      setDreDesabilitada(true);
    }
  }, [dres, eventoAulaCalendarioEdicao, turmaSelecionadaStore]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);
  const [unidadeEscolarDesabilitada, setUnidadeEscolarDesabilitada] = useState(
    false
  );

  const obterUnidadesEscolares = () => {
    setCarregandoUes(true);
    api
      .get(
        `v1/abrangencias/${turmaSelecionadaStore.consideraHistorico}/dres/${dreSelecionada}/ues`
      )
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(unidade => {
              lista.push({
                desc: `${tipoEscolaDTO[unidade.tipoEscola]} ${unidade.nome}`,
                valor: unidade.codigo,
              });
            });
            setUnidadesEscolares(lista.sort(FiltroHelper.ordenarLista('desc')));
            setCarregandoUes(false);
          }
        }
      })
      .catch(() => {
        setUnidadesEscolares(unidadesEscolaresStore);
        setCarregandoUes(false);
      });
  };

  useEffect(() => {
    if (unidadesEscolares.length === 1) {
      setUnidadeEscolarSelecionada(unidadesEscolares[0].valor);
      setUnidadeEscolarDesabilitada(true);
    } else if (
      unidadesEscolares &&
      eventoAulaCalendarioEdicao &&
      eventoAulaCalendarioEdicao.unidadeEscolar
    ) {
      setUnidadeEscolarSelecionada(eventoAulaCalendarioEdicao.unidadeEscolar);
    } else if (Object.entries(turmaSelecionadaStore).length > 0) {
      setUnidadeEscolarSelecionada(turmaSelecionadaStore.unidadeEscolar);
      setUnidadeEscolarDesabilitada(true);
    }
  }, [eventoAulaCalendarioEdicao, unidadesEscolares]);

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
  };

  useEffect(() => {
    if (dreSelecionada) {
      obterUnidadesEscolares();
    } else {
      setUnidadeEscolarSelecionada();
      setOpcaoTurma();
    }
  }, [dreSelecionada]);

  const listaTurmas = [
    { valor: 1, desc: 'Todas as turmas' },
    { valor: 2, desc: 'Turma selecionada' },
  ];

  const aoSelecionarUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
  };

  const [turmas] = useState(listaTurmas);
  const [turmaDesabilitada, setTurmaDesabilitada] = useState(false);

  useEffect(() => {
    if (!unidadeEscolarSelecionada) setOpcaoTurma();
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    if (turmas.length && !opcaoTurma) {
      if (usuario.ehProfessor || usuario.ehProfessorCj) {
        if (Object.entries(turmaSelecionadaStore).length) {
          setOpcaoTurma(listaTurmas[1].valor.toString());
          setTurmaSelecionada(turmaSelecionadaStore.turma);
          setTodasTurmas(false);
        } else {
          setTipoCalendarioSelecionado();
          setDreSelecionada();
          setUnidadeEscolarSelecionada();
          setOpcaoTurma();
          setTurmaSelecionada();
          setTodasTurmas(false);
        }
      } else if (Object.entries(eventoAulaCalendarioEdicao).length) {
        setOpcaoTurma(
          listaTurmas[eventoAulaCalendarioEdicao.turma ? 1 : 0].valor.toString()
        );
        setTurmaSelecionada(eventoAulaCalendarioEdicao.turma || '');
        setTodasTurmas(!eventoAulaCalendarioEdicao.turma);
      } else if (Object.entries(turmaSelecionadaStore).length) {
        setOpcaoTurma(listaTurmas[1].valor.toString());
        setTurmaSelecionada(turmaSelecionadaStore.turma);
        setTodasTurmas(false);
        setTurmaDesabilitada(true);
      }
    }
  }, [
    turmas,
    turmaSelecionadaStore,
    usuario.ehProfessor,
    usuario.ehProfessorCj,
    listaTurmas,
    unidadeEscolarSelecionada,
    eventoAulaCalendarioEdicao,
    opcaoTurma,
  ]);

  const aoSelecionarTurma = turma => {
    setOpcaoTurma(turma);
  };

  useEffect(() => {
    if (opcaoTurma === '1') {
      // Todas as turmas
      setTurmaSelecionada();
      setTodasTurmas(true);
    } else if (opcaoTurma === '2') {
      // Turma selecionada
      if (Object.entries(turmaSelecionadaStore).length) {
        setTurmaSelecionada(turmaSelecionadaStore.turma);
      }
      setTodasTurmas(false);
    } else store.dispatch(zeraCalendario());
  }, [opcaoTurma, turmaSelecionadaStore]);

  useEffect(() => {
    setFiltros({
      tipoCalendarioSelecionado,
      eventoSme,
      dreSelecionada,
      unidadeEscolarSelecionada,
      turmaSelecionada,
      todasTurmas,
    });
  }, [
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
    todasTurmas,
  ]);

  return (
    <Div className="col-12">
      <AlertaSelecionarTurma />
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Calendário do professor</Titulo>
      </Grid>
      <Card className="rounded mb-4 mx-auto">
        <Grid cols={12} className="mb-4">
          <Div className="row">
            <Grid cols={4}>
              <Loader loading={carregandoTipos} tip="">
                <SelectComponent
                  className="fonte-14"
                  onChange={aoSelecionarTipoCalendario}
                  lista={tiposDeCalendario}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={tipoCalendarioSelecionado}
                  placeholder="Selecione o tipo de calendário"
                />
              </Loader>
            </Grid>
            <Grid cols={4}>
              {diasLetivos && diasLetivos.dias ? (
                <Div>
                  <Button
                    id={shortid.generate()}
                    label={diasLetivos.dias.toString()}
                    color={
                      diasLetivos.estaAbaixoPermitido
                        ? Colors.Vermelho
                        : Colors.Verde
                    }
                    className="float-left"
                  />
                  <Div className="float-left w-50 ml-2 mt-1">
                    Nº de dias letivos no calendário
                  </Div>
                </Div>
              ) : (
                <Div />
              )}
              {diasLetivos && diasLetivos.estaAbaixoPermitido && (
                <Div
                  className="clearfix font-weight-bold pt-2"
                  style={{ clear: 'both', color: Base.Vermelho, fontSize: 12 }}
                >
                  <Icone className="fa fa-exclamation-triangle mr-2" />
                  Abaixo do mínimo estabelecido pela legislação
                </Div>
              )}
            </Grid>
            <Grid cols={4}>
              <Button
                id={shortid.generate()}
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                onClick={aoClicarBotaoVoltar}
                border
                className="ml-auto"
              />
            </Grid>
          </Div>
        </Grid>
        {turmaSelecionadaStore && turmaSelecionadaStore.turma ? (
          <>
            <Grid cols={12} className="mb-4">
              <Div className="row">
                <Grid cols={1} className="d-flex align-items-center pr-0">
                  <Div className="w-100">
                    <Tooltip
                      placement="top"
                      title={`${
                        eventoSme
                          ? 'Exibindo eventos da SME'
                          : 'Não exibindo eventos da SME'
                      }`}
                    >
                      <Switch
                        onChange={aoTrocarEventoSme}
                        checked={eventoSme}
                        size="small"
                        className="mr-2"
                      />
                      <Label className="my-auto">SME</Label>
                    </Tooltip>
                  </Div>
                </Grid>
                <Grid cols={5}>
                  <Loader loading={carregandoDres} tip="">
                    <SelectComponent
                      className="fonte-14"
                      onChange={aoSelecionarDre}
                      lista={dres}
                      valueOption="valor"
                      valueText="desc"
                      valueSelect={dreSelecionada}
                      placeholder="Diretoria Regional de Educação (DRE)"
                      disabled
                    />
                  </Loader>
                </Grid>
                <Grid cols={4}>
                  <Loader loading={carregandoUes} tip="">
                    <SelectComponent
                      className="fonte-14"
                      onChange={aoSelecionarUnidadeEscolar}
                      lista={unidadesEscolares}
                      valueOption="valor"
                      valueText="desc"
                      valueSelect={unidadeEscolarSelecionada}
                      placeholder="Unidade Escolar (UE)"
                      disabled
                    />
                  </Loader>
                </Grid>
                <Grid cols={2}>
                  <SelectComponent
                    className="fonte-14"
                    onChange={aoSelecionarTurma}
                    lista={turmas}
                    valueOption="valor"
                    valueText="desc"
                    valueSelect={opcaoTurma}
                    placeholder="Turma"
                    disabled={!unidadeEscolarSelecionada || turmaDesabilitada}
                  />
                </Grid>
              </Div>
            </Grid>
            <Grid cols={12}>
              <Calendario filtros={filtros} />
            </Grid>
          </>
        ) : null}
      </Card>
    </Div>
  );
};

export default CalendarioProfessor;
