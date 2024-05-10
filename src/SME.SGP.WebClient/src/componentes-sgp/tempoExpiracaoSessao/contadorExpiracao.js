import React, { useEffect, useState } from 'react';

const ContadorExpiracao = props => {
  const { dataHoraExpiracao, deslogarDoUsuario } = props;

  const calcularTempoExpiracao = () => {
    const diferenca = +new Date(dataHoraExpiracao) - +new Date();
    let tempoParaExpirar = {};

    if (diferenca > 0) {
      tempoParaExpirar = {
        minutos: Math.floor((diferenca / 1000 / 60) % 60),
        segundos: Math.floor((diferenca / 1000) % 60),
      };
    }

    if (diferenca < 1) {
      deslogarDoUsuario();
    }

    return tempoParaExpirar;
  };

  const [tempoParaExpirar, setTempoParaExpirar] = useState(
    calcularTempoExpiracao()
  );

  useEffect(() => {
    const calculo = setInterval(() => {
      setTempoParaExpirar(calcularTempoExpiracao());
    }, 1000);
    return () => clearInterval(calculo);
  });

  const montarExibicaoTempo = tempo => {
    if (tempo < 10) {
      tempo = '0' + tempo;
    }
    return tempo;
  };

  return (
    <>
      {tempoParaExpirar.minutos || tempoParaExpirar.segundos
        ? montarExibicaoTempo(tempoParaExpirar.minutos) +
          ':' +
          montarExibicaoTempo(tempoParaExpirar.segundos)
        : '00:00'}
    </>
  );
};

export default ContadorExpiracao;
