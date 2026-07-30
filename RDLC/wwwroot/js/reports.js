const requestToken = document.querySelector('meta[name="csrf-token"]').content;

const url = new URL(document.querySelector('meta[name="current-page-path"]').content, location.origin);
url.searchParams.set('handler', 'SelectReport');

const viewerUrl = new URL(document.querySelector('#ifrReport').src);

async function selectReport(element) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': requestToken,
        },
        body: JSON.stringify({
            reportName: element.dataset.reportName
        }),
    });

    if (response.ok) {
        const result = await response.json();

        viewerUrl.searchParams.set('id', result.reportId);
        viewerUrl.searchParams.set('name', result.reportName);

        document.querySelector('#ifrReport').src = viewerUrl;
    } else {
        alert('Failed to select report.');
    }
}