import React, { useEffect, useState } from 'react';

const ContadorExpiracao = () => {
  const calculateTimeLeft = () => {
    const diferenca = +new Date('2020-01-01') - +new Date();
    let tempoParaExpirar = {};

    if (diferenca > 0) {
      tempoParaExpirar = {
        dias: Math.floor(diferenca / (1000 * 60 * 60 * 24)),
        horas: Math.floor((diferenca / (1000 * 60 * 60)) % 24),
        minutos: Math.floor((diferenca / 1000 / 60) % 60),
        segundos: Math.floor((diferenca / 1000) % 60),
      };
    }

    return tempoParaExpirar;
  };

  const [tempoParaExpirar, setTempoParaExpirar] = useState(calculateTimeLeft());

  useEffect(() => {
    setTimeout(() => {
      setTempoParaExpirar(calculateTimeLeft());
    }, 1000);
  });

  return (
    <>
      {tempoParaExpirar.minutos
        ? tempoParaExpirar.minutos + ':' + tempoParaExpirar.segundos
        : '00:00'}
    </>
  );
};

export default ContadorExpiracao;
