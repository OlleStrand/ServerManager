var serverChartId = $('#serverChart')
var serverChart = new Chart(serverChartId, {
    type: 'doughnut',
    data: {
        labels: ['Active', 'Free'],
        datasets: [{
            backgroundColor: [
                'rgba(51, 184, 74, 0.8)',
                'rgba(245, 66, 66, 0.8)'
            ],
            borderColor: 'rgb(0, 0, 0)',
            borderWidth: 0.3,
            data: [1, 9]
        }]
    },
    options: {
        legend: {
            display: false
        },
        tooltips: {
            enabled: false
        }
    }
});

var accountChartId = $('#accountChart')
var accountChart = new Chart(accountChartId, {
    type: 'doughnut',
    data: {
        labels: ['Accounts', 'Free'],
        datasets: [{
            backgroundColor: [
                'rgba(51, 184, 74, 0.8)',
                'rgba(245, 66, 66, 0.8)'
            ],
            borderColor: 'rgb(0, 0, 0)',
            borderWidth: 0.3,
            data: [7, 23]
        }]
    },
    options: {
        legend: {
            display: false
        },
        tooltips: {
            enabled: false
        }
    }
});

var bestServerChartId = $('#bestServerChart')
var bestServerChart = new Chart(bestServerChartId, {
    type: 'doughnut',
    data: {
        labels: ['Average Players', ''],
        datasets: [{
            backgroundColor: [
                'rgba(51, 184, 74, 0.8)',
                'rgba(245, 66, 66, 0.8)'
            ],
            borderColor: 'rgb(0, 0, 0)',
            borderWidth: 0.3,
            data: [47, 64-47]
        }]
    },
    options: {
        legend: {
            display: false
        },
        tooltips: {
            enabled: false
        }
    }
});