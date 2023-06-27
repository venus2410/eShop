var homeController = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $('#create').click(this.loadCreatModal());
    },
    loadCreatModal: function () {
        $.ajax({
            type: "Get",
            URL: 'User/Create',
            dataType: "Text",
            success: function (data) {
                $("#modalHere").html(data);
                $("#exampleModal").modal('show');
            }

        });
    }
}
homeController.init();