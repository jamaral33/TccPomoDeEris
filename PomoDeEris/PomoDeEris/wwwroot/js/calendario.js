document.addEventListener('DOMContentLoaded', () => {
    // Destructure the Calendar constructor
    const { Calendar } = window.VanillaCalendarPro;

    const campoData = document.querySelector('#dataSelecionada');
    const formulario = document.querySelector('#formularioAgendamento');

    const calendario = new Calendar('#calendario', {
        locale: 'pt-BR',
        firstWeekday: 0,

        selectionDatesMode: 'single',
        selectedTheme: 'light',

        // Domingo e sábado indisponíveis
        disableWeekdays: [0, 1],

        // Impede a seleção de datas anteriores a hoje
        disableDatesPast: true,

        //Quando o usuario clica na data
        onClickDate(self) {
            const data = self.context.selectedDates[0]; // pega a data

            if (!data) return;

            campoData.value = data;

            // Evita interpretar a data como UTC e alterar o dia
            

            carregarHorarios(data);
        }
    });

    calendario.init();

    async function carregarHorarios(data) {
        // Posteriormente desabilitar datas

        document.querySelectorAll(
            'input[name="Horario"]'
        ).forEach(input => {
            input.checked = false;
            input.disabled = false;
        });
    }

    formulario.addEventListener('submit', event => {
        if (!campoData.value) {
            event.preventDefault();
            alert('Selecione uma data para o agendamento.');
            return;
        }

        const horarioSelecionado = document.querySelector(
            'input[name="Horario"]:checked'
        );

        if (!horarioSelecionado) {
            event.preventDefault();
            alert('Selecione um horário para continuar.');
        }
    });
});