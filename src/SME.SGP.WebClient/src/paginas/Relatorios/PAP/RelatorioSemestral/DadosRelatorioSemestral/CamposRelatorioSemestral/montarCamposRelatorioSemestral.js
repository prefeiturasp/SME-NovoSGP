import React from 'react';
import { useSelector } from 'react-redux';
import CampoRelatorioSemestral from './campoRelatorioSemestral';

const MontarCamposRelatorioSemestral = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestral.dadosAlunoObjectCard
  );

  const dadosRelatorioSemestral = useSelector(
    store => store.relatorioSemestral.dadosRelatorioSemestral
  );

  const { desabilitado } = dadosAlunoObjectCard;

  return (
    <>
      {dadosRelatorioSemestral && dadosRelatorioSemestral.secoes
        ? dadosRelatorioSemestral.secoes.map(item => {
            return (
              <>
                <CampoRelatorioSemestral
                  descricao={item.descricao}
                  idSecao={item.id}
                  nome={item.nome}
                  obrigatorio={item.obrigatorio}
                  valor={item.valor}
                  alunoDesabilitado={desabilitado}
                />
              </>
            );
          })
        : ''}
    </>
  );
};

export default MontarCamposRelatorioSemestral;
