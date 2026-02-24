const ctx = document.getElementById('myChart');
const noChartData = document.getElementById('noChartData');
let chartInstance = null;
let activeYear = new Date().getFullYear();
let activeMonth = new Date().getMonth() + 1;

function loadChart(year, month) {
    fetch(`/Expenses/GetChart?year=${year}&month=${month}`)
        .then(response => response.json())
        .then(data => {
            if (chartInstance) {
                chartInstance.destroy();
                chartInstance = null;
            }

            if (data && data.length > 0) {

                // Show chart & hide no data message
                ctx.style.display = 'block';
                noChartData.style.display = 'none';

                chartInstance = new Chart(ctx, {
                    type: 'pie',
                    data: {
                        labels: data.map(d => d.category),
                        datasets: [{
                            data: data.map(d => d.total),
                            backgroundColor: [
                                '#0d6efd', '#6f42c1', '#198754', '#fd7e14', '#dc3545',
                                '#20c997', '#0dcaf0', '#ffc107', '#6610f2', '#adb5bd'
                            ],
                            borderColor: '#ffffff',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        plugins: {
                            legend: { position: 'bottom' },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return `${context.label}: AED ${context.parsed.toFixed(2)}`;
                                    }
                                }
                            }
                        }
                    }
                });
            } else {
                // Hide chart, show no data message
                ctx.style.display = 'none';
                noChartData.style.display = 'block';
            }
        })
        .catch(error => {
            console.error('Error loading chart:', error);
            ctx.style.display = 'none';
            noChartData.style.display = 'block';
        });
}

function loadTable(year, month, pageNumber = 1) {

    activeYear = year;
    activeMonth = month;

    fetch(`/Expenses/GetExpensesTable?year=${year}&month=${month}&pageNumber=${pageNumber}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById('expensesTableBody').innerHTML = html;
        })
        .catch(error =>
        {
            console.error('Error loading table:', error);

            document.getElementById('expensesTableBody').innerHTML = `
                    <tr>
                        <td colspan="5" class="text-center text-muted py-4">
                            <i class="bi bi-exclamation-triangle" style="font-size: 2rem;"></i>
                            <div class="mt-2">Error Loading Data</div>
                            <small>Please try again later.</small>
                        </td>
                    </tr>`;
        });
}

document.getElementById('filterBtn').addEventListener('click', function ()
{
    const year = parseInt(document.getElementById('yearSelect').value);
    const month = parseInt(document.getElementById('monthSelect').value);

    loadChart(year, month);
    loadTable(year, month);
});

loadChart(activeYear, activeMonth);
loadTable(activeYear, activeMonth);