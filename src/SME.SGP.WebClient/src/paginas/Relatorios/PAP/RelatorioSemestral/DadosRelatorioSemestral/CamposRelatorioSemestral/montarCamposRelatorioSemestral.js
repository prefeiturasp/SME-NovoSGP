import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import CampoRelatorioSemestral from './campoRelatorioSemestral';

const MontarCamposRelatorioSemestral = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestralPAP.dadosAlunoObjectCard
  );

  const dadosRelatorioSemestral = useSelector(
    store => store.relatorioSemestralPAP.dadosRelatorioSemestral
  );

  const { desabilitado } = dadosAlunoObjectCard;

  return (
    <>
      {dadosRelatorioSemestral && dadosRelatorioSemestral.secoes
        ? dadosRelatorioSemestral.secoes.map(item => {
            return (
              <div key={shortid.generate()}>
                <CampoRelatorioSemestral
                  descricao={item.descricao}
                  idSecao={item.id}
                  nome={item.nome}
                  obrigatorio={item.obrigatorio}
                  valor={item.valor}
                  alunoDesabilitado={desabilitado}
                />
              </div>
            );
          })
        : ''}
    </>
  );
};

export default MontarCamposRelatorioSemestral;
