/**
 * @file 
 * Provides main Backbone view events and models.
 *
 * all te backbone events and models are presence here
 *
 * Author: Viniston Fernando
 */
$(document).ready(function () {


    /**
     * Backbone view.
     **/

    window.AppView = Backbone.View.extend({

        el: $(".bulkemailer"),

        // Main initialization entry point...

        initialize: function () {
            this.nominationId = 0;
            this.render();
        },


        getParameterByName: function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        },

        getMessage: function () {
            var self = this;
            self.nominationId = self.getParameterByName('nominationid');

            $.ajax({
                url: "api/Common/GetEricaNominatorMessage/" + self.getParameterByName('nominationid'),
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Response != null) {
                        if (data.Response.Message) {
                            self.$("#nominee-email-name").html(data.Response.NomineeName);
                            self.$("#replace-message-tag").replaceWith(data.Response.Message);
                            initSample();
                            CKEDITOR.instances["editor"].on("instanceReady",
                                function() {
                                    //set keyup event
                                    this.document.on("change", CK_jQ);
                                    this.document.on("keyup", CK_jQ);
                                    this.document.on("paste", CK_jQ);
                                    this.document.on("keypress", CK_jQ);
                                    this.document.on("blur", CK_jQ);
                                    this.document.on("change", CK_jQ);
                                    this.document.on("mousedown", CK_jQ);
                                    this.document.on("click", CK_jQ);
                                    this.document.on("mouseup", CK_jQ);
                                    this.document.on("mouseleave", CK_jQ);
                                    
                                    //and paste event
                                    this.document.on("paste", CK_jQ);
                                });

                            function CK_jQ() {
                                for (var instance in CKEDITOR.instances) { CKEDITOR.instances[instance].updateElement(); }
                            }
                        }
                    }
                },
                error: function (response) {
                    $.notify("Error occured", "warn");
                }
            });
        },

        processEmail: function (e) {
            var self = this;
            var editorElement = CKEDITOR.document.getById('editor');
            var message = editorElement.getHtml();
            var inputModel = { NominationId: self.nominationId, MailBody: message };
            $.ajax({
                url: "api/Common/SendMail",
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    $.notify("Mail sent successfully", "success");
                },
                error: function () {
                    $.notify("Error occured", "warn");
                },
                data: inputModel
            });

        },

        // Backbone View events ...

        events: {
            "click #uploadbtn": "doUpload",
            "click .btn-process": "processEmail"
        },

        render: function () {
            this.getMessage();
        }

    });

    var appview = new AppView();
});