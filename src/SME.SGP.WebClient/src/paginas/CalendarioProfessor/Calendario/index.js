import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { Tooltip, Switch } from 'antd';
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

  const [diasLetivos, setDiasLetivos] = useState({});
  const turmaSelecionadaStore = useSelector(
    state => state.usuario.turmaSelecionada
  );
  const modalidadesAbrangencia = useSelector(state => state.filtro.modalidades);
  const anosLetivosAbrangencia = useSelector(state => state.filtro.anosLetivos);

  const obterTiposCalendario = async modalidades => {
    return api
      .get('v1/calendarios/tipos')
      .then(resposta => {
        const tiposCalendarioLista = [];
        if (resposta.data) {
          const anos = [];
          anosLetivosAbrangencia.forEach(ano => {
            if (!anos.includes(ano.valor)) anos.push(ano.valor);
          });
          const tipos = resposta.data.filter(tipo => {
            return (
              modalidades.indexOf(tipo.modalidade) > -1 &&
              anos.indexOf(tipo.anoLetivo) > -1
            );
          });
          tipos.forEach(tipo => {
            tiposCalendarioLista.push({
              desc: tipo.nome,
              valor: tipo.id,
              modalidade: tipo.modalidade,
            });
          });
        }
        return tiposCalendarioLista;
      })
      .catch(() => {
        return [];
      });
  };

  const listarModalidadesPorAbrangencia = () => {
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
  };

  const listarTiposCalendarioPorTurmaSelecionada = async tiposLista => {
    if (Object.entries(turmaSelecionadaStore).length > 0) {
      const modalidadeSelecionada =
        turmaSelecionadaStore.modalidade === ModalidadeDTO.EJA.toString()
          ? 2
          : 1;

      if (tiposLista) {
        setTiposCalendario(
          tiposLista.filter(tipo => tipo.modalidade === modalidadeSelecionada)
        );
      } else if (tiposCalendario) {
        setTiposCalendario(
          tiposCalendario.filter(
            tipo => tipo.modalidade === modalidadeSelecionada
          )
        );
      }
    } else {
      setTiposCalendario(
        await obterTiposCalendario(listarModalidadesPorAbrangencia())
      );
    }
  };

  const listarTiposCalendario = async () => {
    listarTiposCalendarioPorTurmaSelecionada(
      await obterTiposCalendario(listarModalidadesPorAbrangencia())
    );
  };

  const eventoAulaCalendarioEdicao = useSelector(
    state => state.calendarioProfessor.eventoAulaCalendarioEdicao
  );

  useEffect(() => {
    listarTiposCalendario();
    return () => store.dispatch(zeraCalendario());
  }, []);

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
  }, [tiposCalendario]);

  useEffect(() => {
    listarTiposCalendarioPorTurmaSelecionada();
  }, [turmaSelecionadaStore]);

  const consultarDiasLetivos = () => {
    api
      .post('v1/calendarios/dias-letivos', {
        tipoCalendarioId: tipoCalendarioSelecionado,
        dreId: dreSelecionada,
        ueId: unidadeEscolarSelecionada,
      })
      .then(resposta => {
        if (resposta.data) setDiasLetivos(resposta.data);
      })
      .catch(() => {
        setDiasLetivos({});
      });
  };

  const aoSelecionarTipoCalendario = tipo => {
    store.dispatch(zeraCalendario());
    setTipoCalendarioSelecionado(tipo);
  };

  useEffect(() => {
    if (tipoCalendarioSelecionado) {
      consultarDiasLetivos();
      obterDres();
    } else {
      setDiasLetivos({});
      setDreSelecionada();
      setUnidadeEscolarSelecionada();
      setTurmaSelecionada();
    }
    setFiltros({ ...filtros, tipoCalendarioSelecionado });
  }, [tipoCalendarioSelecionado]);

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const [eventoSme, setEventoSme] = useState(true);

  const aoTrocarEventoSme = () => {
    setEventoSme(!eventoSme);
  };

  useEffect(() => {
    setFiltros({ ...filtros, eventoSme });
  }, [eventoSme]);

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState(undefined);

  const obterDres = () => {
    api
      .get('v1/abrangencias/dres')
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
            setDres(lista);
          }
        }
      })
      .catch(() => {
        setDres(dresStore);
      });
  };

  useEffect(() => {
    if (dres && eventoAulaCalendarioEdicao && eventoAulaCalendarioEdicao.dre) {
      setDreSelecionada(eventoAulaCalendarioEdicao.dre);
    }
  }, [dres]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState(
    undefined
  );

  const obterUnidadesEscolares = () => {
    api
      .get(`v1/abrangencias/dres/${dreSelecionada}/ues`)
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          if (resposta.data) {
            resposta.data.forEach(unidade => {
              lista.push({
                desc: unidade.nome,
                valor: unidade.codigo,
              });
            });
            setUnidadesEscolares(lista);
          }
        }
      })
      .catch(() => {
        setUnidadesEscolares(unidadesEscolaresStore);
      });
  };

  useEffect(() => {
    if (
      unidadesEscolares &&
      eventoAulaCalendarioEdicao &&
      eventoAulaCalendarioEdicao.unidadeEscolar
    ) {
      setDreSelecionada(eventoAulaCalendarioEdicao.unidadeEscolar);
    }
  }, [unidadesEscolares]);

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
  };

  useEffect(() => {
    if (dreSelecionada) {
      consultarDiasLetivos();
      obterUnidadesEscolares();
    } else {
      setUnidadeEscolarSelecionada();
      setTurmaSelecionada();
    }
    setFiltros({ ...filtros, dreSelecionada });
  }, [dreSelecionada]);

  const obterTurmas = () => {
    api
      .get(`v1/abrangencias/dres/ues/${unidadeEscolarSelecionada}/turmas`)
      .then(resposta => {
        if (resposta.data) {
          const lista = [];
          resposta.data.forEach(turma => {
            lista.push({
              desc: turma.nome,
              valor: turma.codigo,
              ano: turma.ano,
            });
          });
          setTurmas(lista);
        }
      })
      .catch(() => {
        setTurmas([]);
      });
  };

  const aoSelecionarUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
  };

  useEffect(() => {
    if (unidadeEscolarSelecionada) {
      consultarDiasLetivos();
      obterTurmas();
    } else {
      setTurmaSelecionada();
    }
    setFiltros({ ...filtros, unidadeEscolarSelecionada });
  }, [unidadeEscolarSelecionada]);

  const [turmas, setTurmas] = useState([]);
  const [turmaSelecionada, setTurmaSelecionada] = useState(undefined);

  const aoSelecionarTurma = turma => {
    setTurmaSelecionada(turma);
  };

  useEffect(() => {
    setFiltros({ ...filtros, turmaSelecionada });
  }, [turmaSelecionada]);

  const [filtros, setFiltros] = useState({
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
  });

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Calendário do professor</Titulo>
      </Grid>
      <Card className="rounded mb-4 mx-auto">
        <Grid cols={12} className="mb-4">
          <Div className="row">
            <Grid cols={4}>
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarTipoCalendario}
                lista={tiposCalendario}
                valueOption="valor"
                valueText="desc"
                valueSelect={tipoCalendarioSelecionado}
                placeholder="Tipo de Calendário"
              />
            </Grid>
            <Grid cols={4}>
              {diasLetivos && diasLetivos.dias && (
                <Div>
                  <Button
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
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarDre}
                lista={dres}
                valueOption="valor"
                valueText="desc"
                valueSelect={dreSelecionada}
                placeholder="Diretoria Regional de Educação (DRE)"
                disabled={!tipoCalendarioSelecionado}
              />
            </Grid>
            <Grid cols={4}>
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarUnidadeEscolar}
                lista={unidadesEscolares}
                valueOption="valor"
                valueText="desc"
                valueSelect={unidadeEscolarSelecionada}
                placeholder="Unidade Escolar (UE)"
                disabled={!dreSelecionada}
              />
            </Grid>
            <Grid cols={2}>
              <SelectComponent
                className="fonte-14"
                onChange={aoSelecionarTurma}
                lista={turmas}
                valueOption="valor"
                valueText="desc"
                valueSelect={turmaSelecionada}
                placeholder="Turma"
                disabled={!unidadeEscolarSelecionada}
              />
            </Grid>
          </Div>
        </Grid>
        <Grid cols={12}>
          <Calendario filtros={filtros} />
        </Grid>
      </Card>
    </Div>
  );
};

export default CalendarioProfessor;
