function toggleDropdown() {
    var dropdown = document.getElementById("dropdown");
    if (dropdown.style.display === "none" || dropdown.style.display === "") {
        dropdown.style.display = "block";
    } else {
        dropdown.style.display = "none";
    }
}

document.querySelectorAll('.vertical-nav a').forEach(item => {
    item.addEventListener('click', event => {
        event.preventDefault();
        const target = event.target.dataset.target;
        loadPartialView(target);
    });
});

function loadPartialView(target) {
    const url = '@Url.Action("LoadPartialView", "Manager")?target=' + target;
    //console.log('Generated URL:', url);
    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.getElementById('content').innerHTML = data;
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

// Add event listener to all edit links
document.querySelectorAll('.edit-link').forEach(item => {
    item.addEventListener('click', event => {
        event.preventDefault();
        const targetUrl = event.target.dataset.target;
        loadEditForm(targetUrl);
    });
});

// Function to load the edit form partial view
function loadEditForm(targetUrl) {
    fetch(targetUrl)
        .then(response => response.text())
        .then(data => {
            console.log('Edit form loaded successfully:', data);
            document.getElementById('content').innerHTML = data;
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function exportToExcel() {
    console.log('Exporting to Excel...');
    $.ajax({
        url: '/Manager/ExportToExcel', // URL to your export action
        type: 'GET',
        xhrFields: {
            responseType: 'blob' // Set the response type to blob
        },
        success: function (response) {
            var blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'Employees.xlsx';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        },
        error: function (xhr, status, error) {
            alert('Export failed: ' + error);
        }
    });
    // Prevent the default behavior of the anchor tag
    return false;
}

function clockIn() {
    $.ajax({
        url: '/Manager/ClockIn',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var clockInTime = new Date();
                var formattedClockInTime = formatDate(clockInTime);
                $('#clockDisplay').text('Clock In Time: ' + formattedClockInTime);
            } else {
                showError(response.message);
            }
        },
        error: function (xhr, status, error) {
            showError('Clock in operation failed: ' + error);
        }
    });
}

function clockOut() {
    $.ajax({
        url: '/Manager/ClockOut',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                var clockOutTime = new Date();
                var formattedClockOutTime = formatDate(clockOutTime);
                var message = 'Clock Out Time: ' + formattedClockOutTime;
                if (response.timeWorked) {
                    message += ' | Time Worked: ' + response.timeWorked;
                }
                $('#clockDisplay').text(message);
            } else {
                showError(response.message);
            }
        },
        error: function (xhr, status, error) {
            showError('Clock out operation failed: ' + error);
        }
    });
}

function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    return pad(hours) + ':' + pad(minutes) + ':' + pad(seconds);
}

function pad(num) {
    return (num < 10 ? '0' : '') + num;
}

function showError(message) {
    // Display error message to the user
    $('#clockDisplay').text(message);
}