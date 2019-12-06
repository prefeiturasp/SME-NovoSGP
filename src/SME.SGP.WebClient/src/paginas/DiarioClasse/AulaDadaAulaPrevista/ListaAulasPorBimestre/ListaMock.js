
export const getMock = () => {
  const dados = {
    bimestres: [
      {
        bimestre: 1,
        inicio: new Date(),
        fim: new Date(),
        previstas: {
          quantidade: 1,
          temDivergencia: false
        },
        criadas: {
          professorTitular: 1,
        },
        dadas: 1,
        respostas: 1,
      },
      {
        bimestre: 2,
        inicio: new Date(),
        fim: new Date(),
        previstas: {
          quantidade: 1,
          temDivergencia: true
        },
        criadas: {
          professorTitular: 1,
        },
        dadas: 1,
        respostas: 1,
      },
      {
        bimestre: 3,
        inicio: new Date(),
        fim: new Date(),
        previstas: {
          quantidade: 1,
          temDivergencia: false
        },
        criadas: {
          professorTitular: 2,
        },
        dadas: 1,
        respostas: 1,
      },
      {
        bimestre: 4,
        inicio: new Date(),
        fim: new Date(),
        previstas: {
          quantidade: 1,
          temDivergencia: false
        },
        criadas: {
          professorTitular: 1,
        },
        dadas: 1,
        respostas: 1,
      }
    ],
    totalPrevistas: 4,
    totalCriadasProfTitular: 4,
    totalCriadasProfCj: 4,
    totalDadas:4,
    totalRespostas:4,
  }
  return dados;
}
