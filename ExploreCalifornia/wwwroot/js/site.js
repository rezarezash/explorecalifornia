$(function () {
    $("#mainContent").on("click", '.pager a', function () {
        var url = $(this).attr('href');

        console.log(url);
        $("#mainContent").load(url);
        return false;
    })
});
