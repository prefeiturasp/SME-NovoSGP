import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';

const SituacaoEncaminhamentoAEE = () => {
  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  const [situacao, setSituacao] = useState({});

  useEffect(() => {
    const obtemSituacaoEncaminhamento = async () => {
      const retorno = await ServicoEncaminhamentoAEE.obterAlunoSituacaoEncaminhamentoAEE(
        dadosCollapseLocalizarEstudante?.codigoEstudante
      );

      if (retorno.data) {
        setSituacao(retorno.data);
      }
    };
    obtemSituacaoEncaminhamento();
  });

  return situacao?.situacao ? (
    <>
      <strong>Encaminhamento AEE: </strong>
      {situacao?.situacao}
    </>
  ) : (
    ''
  );
};

export default SituacaoEncaminhamentoAEE;
