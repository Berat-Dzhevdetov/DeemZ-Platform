const orangeColor = "#FFA000";
const regalBlueColor = "#234465";
document.querySelectorAll('li.info-item').forEach(li => {
    li.addEventListener('mouseover', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = regalBlueColor;
    });
    li.addEventListener('mouseout', e => {
        e.currentTarget.querySelector('span.arrow-right').style.background = orangeColor;
    });
})