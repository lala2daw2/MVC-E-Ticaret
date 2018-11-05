$(document).ready(function myfunction() {
    $('.addCart').click(function myfunction() {
        var btn = $(this);
        var id = btn.data("id");
        $.ajax({
            url: "/Sepet/AddCart/" + id,
            type: "GET",
            success: function myfunction() {
                $.ajax({
                    url: "/Home/MiniSepet",
                    type: "GET",
                    success: function (veri) {
                        $('.cart-all').html('');
                        $('.cart-all').html(veri);
                    }
                });
            },
            error: function myfunction() {
                alert("Tekrar deneyiniz");
            }
        });
    });
});

