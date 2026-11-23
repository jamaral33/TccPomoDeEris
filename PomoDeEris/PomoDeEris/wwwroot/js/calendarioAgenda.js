document.addEventListener('DOMContentLoaded', () => {

    const campoData = document.querySelector('#dataSelecionada');
    const campoHorario = document.querySelector('#horarioCompleto');
    const formulario = document.querySelector('#formularioAgendamento');

    const { Calendar } = window.VanillaCalendarPro;

    // dia bloqueado só ganha cor, NÃO usar disableDates (senão o admin não consegue liberar)
    const popups = {};
    diasBloqueados.forEach(d => popups[d] = { modifier: 'dia-bloqueado', html: 'Dia bloqueado' });

    const calendario = new Calendar('#calendario', {
        locale: 'pt-BR',
        firstWeekday: 0,
        selectionDatesMode: 'single',
        disableWeekdays: [0, 1],
        disableDatesPast: true,
        selectedTheme: 'light',
        popups: popups,
        selectedDates: campoData.value ? [campoData.value] : [],

        onClickDate(self) {
            campoData.value = self.context.selectedDates[0] || '';

            document.querySelectorAll('input[name="HoraSelecionada"]').forEach(input => {
                input.checked = false;
            }); // reseta os horarios dps que clica na data

            if (campoData.value) {
                buscarHorarios(campoData.value);
            }
        }
    });

    calendario.init();

    // depois do POST, pinta de novo os horários da data selecionada
    if (campoData.value) {
        buscarHorarios(campoData.value);
    }

    formulario.addEventListener('submit', event => {

        const selecionado = document.querySelector('input[name="HoraSelecionada"]:checked');
        const botaoHorario = event.submitter.getAttribute('formaction').includes('BloquearHorario');

        if (!campoData.value || (botaoHorario && !selecionado)) {
            event.preventDefault();
            alert(botaoHorario ? 'Selecione uma data e um horário.' : 'Selecione uma data.');
            return;
        }

        if (selecionado) {
            campoHorario.value = `${campoData.value}T${selecionado.value}:00`;
        }
    });

});

function buscarHorarios(data) {

    fetch(`/Colaborador/Agenda/ObterHorarios?data=${data}`)
        .then(response => response.json())
        .then(horariosBloqueados => {

            const horarios = document.querySelectorAll('input[name="HoraSelecionada"]');

            horarios.forEach(horario => {

                const indisponivel = horariosBloqueados.includes(horario.value);

                horario.checked = false;

                horario.closest(".opcao-horario")
                    .classList.toggle("indisponivel", indisponivel);

            });

        });

}