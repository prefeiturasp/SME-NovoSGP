import React from 'react';
import { useSelector } from 'react-redux';
import { ModalidadeDTO } from '~/dtos';
import FrequenciaGlobalPorAno from './FrequenciaGlobalPorAno/frequenciaGlobalPorAno';
import FrequenciaGlobalPorDRE from './FrequenciaGlobalPorDRE/frequenciaGlobalPorDRE';
import QuantidadeAusenciasPossuemJustificativa from './QuantidadeAusenciasPossuemJustificativa/quantidadeAusenciasPossuemJustificativa';
import QuantidadeJustificativasPorMotivo from './QuantidadeJustificativasPorMotivo/quantidadeJustificativasPorMotivo';

const GraficosFrequencia = () => {
  const OPCAO_TODOS = '-99';

  const anoLetivo = useSelector(
    store => store.dashboardFrequencia?.dadosDashboardFrequencia?.anoLetivo
  );
  const dre = useSelector(
    store => store.dashboardFrequencia?.dadosDashboardFrequencia?.dre
  );
  const ue = useSelector(
    store => store.dashboardFrequencia?.dadosDashboardFrequencia?.ue
  );
  const modalidade = useSelector(
    store => store.dashboardFrequencia?.dadosDashboardFrequencia?.modalidade
  );
  const semestre = useSelector(
    store => store.dashboardFrequencia?.dadosDashboardFrequencia?.semestre
  );

  const dreId = OPCAO_TODOS === dre?.codigo ? OPCAO_TODOS : dre?.id;
  const ueId = OPCAO_TODOS === ue?.codigo ? OPCAO_TODOS : ue?.id;

  const exibirFrequenciaGlobalPorDRE =
    dre?.codigo === OPCAO_TODOS && ue?.codigo === OPCAO_TODOS;

  return anoLetivo &&
    dre &&
    ue &&
    modalidade &&
    !!(Number(modalidade) === ModalidadeDTO.EJA ? semestre : !semestre) ? (
    <>
      <FrequenciaGlobalPorAno
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      {exibirFrequenciaGlobalPorDRE && (
        <FrequenciaGlobalPorDRE
          anoLetivo={anoLetivo}
          modalidade={modalidade}
          semestre={semestre}
        />
      )}
      <QuantidadeAusenciasPossuemJustificativa
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      <QuantidadeJustificativasPorMotivo
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
        codigoUe={ue?.codigo}
      />
    </>
  ) : (
    ''
  );
};

export default GraficosFrequencia;
