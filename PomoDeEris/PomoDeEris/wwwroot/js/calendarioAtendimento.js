document.addEventListener('DOMContentLoaded', () => {

    const dataSelecionada = document.getElementById('dataSelecionada').value;
    const { Calendar } = window.VanillaCalendarPro;

    const calendario = new Calendar('#calendario', {
        locale: 'pt-BR',
        firstWeekday: 0,
        selectionDatesMode: 'single',
        selectedDates: [dataSelecionada],
        disableWeekdays: [0, 1],
        selectedTheme: 'light',

        onClickDate(self) {

            const dataSelecionada = self.context.selectedDates[0];

            if (!dataSelecionada) return;

        }
    });

    calendario.init();

});