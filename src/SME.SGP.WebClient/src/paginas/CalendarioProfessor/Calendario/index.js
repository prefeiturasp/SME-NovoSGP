import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Calendario from '~/componentes-sgp/Calendario/Calendario';
import { Base, Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import { store } from '~/redux';
import { zeraCalendario } from '~/redux/modulos/calendarioEscolar/actions';

const Div = styled.div``;
const Titulo = styled(Div)`
  color: ${Base.CinzaMako};
  font-size: 24px;
`;
const Icone = styled.i``;
const Campo = styled.input``;
const Label = styled.label``;

const CalendarioProfessor = () => {
  const [tiposCalendario, setTiposCalendario] = useState([]);
  const [tipoCalendarioSelecionado, setTipoCalendarioSelecionado] = useState(
    undefined
  );

  const [diasLetivos, setDiasLetivos] = useState({});
  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);
  const modalidadesAbrangencia = useSelector(state => state.filtro.modalidades);

  useEffect(() => {
    const modalidades = [];
    if (modalidadesAbrangencia) {
      modalidadesAbrangencia.forEach(modalidade => {
        if (
          (modalidade.valor === 5 || modalidade.valor === 6) &&
          !modalidades.includes(1)
        )
          modalidades.push(1);
        if (modalidade.valor === 3 && !modalidades.includes(2))
          modalidades.push(2);
      });
    }

    api.get('v1/calendarios/tipos').then(resposta => {
      if (resposta.data) {
        const tipos = resposta.data.filter(
          tipo => modalidades.indexOf(tipo.modalidade) > -1
        );
        const tiposCalendarioLista = [];
        tipos.forEach(tipo => {
          tiposCalendarioLista.push({
            desc: tipo.nome,
            valor: tipo.id,
            modalidade: tipo.modalidade,
          });
        });
        setTiposCalendario(tiposCalendarioLista);
      }
    });
    return () => store.dispatch(zeraCalendario());
  }, []);

  const filtrarPorTurmaSelecionada = () => {
    if (tiposCalendario && Object.entries(turmaSelecionada).length > 0) {
      const modalidadeSelecionada = turmaSelecionada.modalidade === '3' ? 2 : 1;
      setTiposCalendario(
        tiposCalendario.filter(
          tipo => tipo.modalidade === modalidadeSelecionada
        )
      );
    }
  };

  useEffect(() => {
    filtrarPorTurmaSelecionada();
  }, [turmaSelecionada]);

  useEffect(() => {
    setFiltros({ ...filtros, tipoCalendarioSelecionado });
    if (tipoCalendarioSelecionado) {
      consultarDiasLetivos();
      buscarDres();
    }
  }, [tipoCalendarioSelecionado]);

  const aoSelecionarTipoCalendario = tipo => {
    store.dispatch(zeraCalendario());
    setTipoCalendarioSelecionado(tipo);
  };

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const [eventoSme, setEventoSme] = useState(true);

  const aoTrocarEventoSme = () => {
    setEventoSme(!eventoSme);
    setFiltros({ ...filtros, eventoSme: !eventoSme });
  };

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState(undefined);

  const buscarDres = () => {
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

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState([]);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState(
    undefined
  );

  const buscarUnidadesEscolares = () => {
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

  const aoSelecionarDre = dre => {
    setDreSelecionada(dre);
    setFiltros({ ...filtros, dreSelecionada: dre });
  };

  useEffect(() => {
    if (dreSelecionada) {
      consultarDiasLetivos();
      buscarUnidadesEscolares();
    }
  }, [dreSelecionada]);

  const aoSelecionarUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
    setFiltros({ ...filtros, unidadeEscolarSelecionada: unidade });
  };

  useEffect(() => {
    if (unidadeEscolarSelecionada) consultarDiasLetivos();
  }, [unidadeEscolarSelecionada]);

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

  const [filtros, setFiltros] = useState({
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
  });

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Calendário do Professor</Titulo>
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
                    Nº de Dias Letivos no Calendário
                  </Div>
                </Div>
              )}
              {diasLetivos && diasLetivos.estaAbaixoPermitido && (
                <Div
                  className="clearfix font-weight-bold pt-2"
                  style={{ clear: 'both', color: Base.Vermelho, fontSize: 12 }}
                >
                  <Icone className="fa fa-exclamation-triangle mr-2" />
                  Abaixo do mínimo estabelecido pela legislação.
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
            <Grid cols={2} className="d-flex align-items-center">
              <Div className="custom-control custom-switch">
                <Campo
                  id="eventoSme"
                  type="checkbox"
                  className="custom-control-input"
                  onChange={aoTrocarEventoSme}
                  checked={eventoSme}
                />
                <Label
                  className="custom-control-label pt-1"
                  htmlFor="eventoSme"
                >
                  Evento SME
                </Label>
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
            <Grid cols={5}>
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
