document.addEventListener('DOMContentLoaded', () => {

    const inputData = document.getElementById('dataSelecionada');
    const { Calendar } = window.VanillaCalendarPro;

    const calendario = new Calendar('#calendario', {
        locale: 'pt-BR',
        firstWeekday: 0,
        selectionDatesMode: 'single',
        selectedDates: inputData.value ? [inputData.value] : [],
        disableWeekdays: [0, 1],
        selectedTheme: 'light',

        onClickDate(self) {
            const dataClicada = self.context.selectedDates[0];
            if (!dataClicada) return;

            inputData.value = dataClicada;
        }
    });

    calendario.init();

});