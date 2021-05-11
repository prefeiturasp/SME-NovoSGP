import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import GraficoBarras from '~/componentes-sgp/Graficos/graficoBarras';
import { ModalidadeDTO } from '~/dtos';
import { AbrangenciaServico, erros } from '~/servicos';
import ServicoDashboardFrequencia from '~/servicos/Paginas/Dashboard/ServicoDashboardFrequencia';

const GraficoQuantidadeJustificativasPorMotivo = props => {
  const { anoLetivo, dreId, ueId, modalidade, semestre, codigoUe } = props;

  const listaAnosEscolares = useSelector(
    store =>
      store.dashboardFrequencia?.dadosDashboardFrequencia?.listaAnosEscolares
  );

  const consideraHistorico = useSelector(
    store =>
      store.dashboardFrequencia?.dadosDashboardFrequencia?.consideraHistorico
  );

  const [dadosGrafico, setDadosGrafico] = useState([]);
  const [exibirLoader, setExibirLoader] = useState(false);
  const [anoEscolar, setAnoEscolar] = useState();
  const [listaTurmas, setListaTurmas] = useState([]);
  const [turmaId, setTurmaId] = useState();

  const [carregandoTurma, setCarregandoTurma] = useState();

  const OPCAO_TODOS = '-99';

  const obterDadosGrafico = useCallback(async () => {
    setExibirLoader(true);
    const retorno = await ServicoDashboardFrequencia.obterQuantidadeJustificativasMotivo(
      anoLetivo,
      dreId === OPCAO_TODOS ? '' : dreId,
      ueId === OPCAO_TODOS ? '' : ueId,
      modalidade,
      semestre,
      anoEscolar === OPCAO_TODOS ? '' : anoEscolar,
      turmaId === OPCAO_TODOS ? '' : turmaId
    )
      .catch(e => erros(e))
      .finally(() => setExibirLoader(false));

    if (retorno?.data?.length) {
      setDadosGrafico(retorno.data);
    } else {
      setDadosGrafico([]);
    }
  }, [anoLetivo, dreId, ueId, modalidade, semestre, turmaId, anoEscolar]);

  useEffect(() => {
    const consultaPorTurma = ueId && ueId !== OPCAO_TODOS;
    const consultaPorAnoEscolar = ueId === OPCAO_TODOS;

    if (
      anoLetivo &&
      dreId &&
      ueId &&
      modalidade &&
      !!(Number(modalidade) === ModalidadeDTO.EJA ? semestre : !semestre) &&
      ((consultaPorTurma && turmaId) || (consultaPorAnoEscolar && anoEscolar))
    ) {
      obterDadosGrafico();
    } else {
      setDadosGrafico([]);
    }
  }, [
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre,
    turmaId,
    anoEscolar,
    obterDadosGrafico,
  ]);

  useEffect(() => {
    if (listaAnosEscolares?.length) {
      if (listaAnosEscolares?.length === 1) {
        setAnoEscolar(listaAnosEscolares[0].ano);
      }

      if (listaAnosEscolares?.length > 1) {
        const temTodos = listaAnosEscolares.find(
          item => item.ano === OPCAO_TODOS
        );
        if (temTodos) {
          setAnoEscolar(OPCAO_TODOS);
        }
      }
    }
  }, [listaAnosEscolares]);

  const onChangeAnoEscolar = valor => setAnoEscolar(valor);

  const obterTurmas = useCallback(async () => {
    setCarregandoTurma(true);
    const resultado = await AbrangenciaServico.buscarTurmas(
      codigoUe,
      modalidade,
      '',
      anoLetivo,
      consideraHistorico
    );
    if (resultado?.data?.length) {
      setListaTurmas(resultado.data);
      if (resultado.data.length === 1) {
        setTurmaId(String(resultado.data[0].id));
      }

      if (resultado.data.length > 1) {
        resultado.data.unshift({ id: OPCAO_TODOS, nome: 'Todas' });
        setTurmaId(OPCAO_TODOS);
      }
    }
    setCarregandoTurma(false);
  }, [codigoUe, modalidade, anoLetivo, consideraHistorico]);

  useEffect(() => {
    if (
      modalidade &&
      dreId &&
      ueId &&
      ueId !== OPCAO_TODOS &&
      codigoUe &&
      anoLetivo
    ) {
      obterTurmas();
    } else {
      setTurmaId();
      setListaTurmas([]);
    }
  }, [modalidade]);

  const onChangeTurma = valor => setTurmaId(valor);

  return (
    <>
      <div className="row">
        <div className="col-sm-12 col-md-6 col-lg-3 col-xl-3 mb-2">
          {ueId && ueId !== OPCAO_TODOS ? (
            <Loader loading={carregandoTurma}>
              <SelectComponent
                id="turma"
                lista={listaTurmas}
                valueOption="id"
                valueText="nome"
                label="Turma"
                disabled={!modalidade || listaTurmas?.length === 1}
                valueSelect={turmaId}
                placeholder="Turma"
                onChange={onChangeTurma}
                allowClear={false}
              />
            </Loader>
          ) : (
            <SelectComponent
              id="ano-escolar"
              lista={listaAnosEscolares}
              valueOption="ano"
              valueText="modalidadeAno"
              disabled={listaAnosEscolares?.length === 1}
              valueSelect={anoEscolar}
              onChange={onChangeAnoEscolar}
              placeholder="Selecione o ano"
              allowClear={false}
            />
          )}
        </div>
      </div>
      <Loader
        loading={exibirLoader}
        className={exibirLoader ? 'text-center' : ''}
      >
        {dadosGrafico?.length ? (
          <GraficoBarras data={dadosGrafico} />
        ) : !exibirLoader ? (
          <div className="text-center">Sem dados</div>
        ) : (
          ''
        )}
      </Loader>
    </>
  );
};

GraficoQuantidadeJustificativasPorMotivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  codigoUe: PropTypes.string,
};

GraficoQuantidadeJustificativasPorMotivo.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
  codigoUe: '',
};

export default GraficoQuantidadeJustificativasPorMotivo;
