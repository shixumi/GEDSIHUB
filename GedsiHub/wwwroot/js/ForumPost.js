// JavaScript to update the file name in the label
document.getElementById('imageUpload').addEventListener('change', function () {
    var fileName = this.files[0] ? this.files[0].name : "No file chosen";
    document.getElementById('fileLabel').textContent = fileName;
});

