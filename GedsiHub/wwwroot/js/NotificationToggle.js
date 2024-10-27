document.querySelectorAll('.toggle-option').forEach(option => {
    option.addEventListener('click', function () {
        document.querySelectorAll('.toggle-option').forEach(el => el.classList.remove('selected'));
        this.classList.add('selected');
    });
});