document.addEventListener('DOMContentLoaded', () => {

    const campoData = document.querySelector('#dataSelecionada');
    const campoHorario = document.querySelector('#horarioCompleto');
    const formulario = document.querySelector('#formularioAgendamento');

    const { Calendar } = window.VanillaCalendarPro;

    const calendario = new Calendar('#calendario', {
        locale: 'pt-BR',
        firstWeekday: 0,
        selectionDatesMode: 'single',
        disableWeekdays: [0, 1],
        disableDatesPast: true,
        selectedTheme: 'light',
        disableDates: diasBloqueados,

        onClickDate(self) {
            campoData.value = self.context.selectedDates[0] || ''; // preenche o campo com a data

            document.querySelectorAll('input[name="Horario"]').forEach(input => {
                input.checked = false;
            }); // reseta os horarios dps que clica na data

            if (campoData.value) {
                buscarHorarios(campoData.value);
            }
        }
    });

    calendario.init();

    formulario.addEventListener('submit', event => {

        const selecionado = document.querySelector('input[name="Horario"]:checked');

        if (!campoData.value || !selecionado) {
            event.preventDefault();
            alert('Selecione uma data e um horário.');
            return;
        }

        campoHorario.value = `${campoData.value}T${selecionado.value}:00`;

        console.log('Data e horário enviados:', campoHorario.value);
    });

});

function buscarHorarios(data) {

    fetch(`/Atendimento/ObterHorarios?data=${data}`)
        .then(response => response.json())
        .then(horariosBloqueados => {

            const horarios = document.querySelectorAll('input[name="Horario"]');

            horarios.forEach(horario => {

                const indisponivel = horariosBloqueados.includes(horario.value);

                horario.disabled = indisponivel;
                horario.checked = false;

                horario.closest(".opcao-horario")
                    .classList.toggle("indisponivel", indisponivel);

            });

        });

}

