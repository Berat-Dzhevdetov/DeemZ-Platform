document.querySelector('.mobile-dropdown').addEventListener('click', function (e) {
    $(this).next(".links-holder").first().slideToggle(300);
    let i = e.currentTarget.querySelector('i');
    let span = e.currentTarget.querySelector('span');
    toggleArrow(i);
    toggleSpanText(span);
});

function toggleArrow(i) {
    if (i.classList.contains('fa-arrow-down'))
        i.classList = "fas fa-arrow-up";
    else
        i.classList = "fas fa-arrow-down";
}

function toggleSpanText(span) {
    if (span.innerText.trim() === "Show links")
        span.innerText = "Hide links";
    else
        span.innerText = "Show links";
}