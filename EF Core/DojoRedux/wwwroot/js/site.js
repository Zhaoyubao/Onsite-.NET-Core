$(document).ready(function() {
    $("div.quotes").on("click", "b", function() {
        let id = $(this).attr("id");
        $.ajax({
            method: "GET",
            url: `/user/${id}`,
            success: res => $("div#quotes").html(res)
        })
    });
})